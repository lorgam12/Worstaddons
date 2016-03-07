namespace Darius // The Dank Memes Master
{
    using System;
    using System.Linq;
    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Enumerations;
    using EloBuddy.SDK.Events;
    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Menu.Values;
    using EloBuddy.SDK.Rendering;
    using SharpDX;

    class Program
    {
        public const string ChampName = "Darius";
        public static readonly Item Cutlass = new Item((int)ItemId.Bilgewater_Cutlass, 550);
        public static readonly Item Botrk = new Item((int)ItemId.Blade_of_the_Ruined_King, 550);
        public static readonly Item Youmuu = new Item((int)ItemId.Youmuus_Ghostblade);
        public static Menu QMenu { get; private set; }
        public static Menu WMenu { get; private set; }
        public static Menu EMenu { get; private set; }
        public static Menu RMenu { get; private set; }
        public static Menu ManaMenu { get; private set; }
        public static Menu ItemsMenu { get; private set; }
        public static Menu DrawMenu { get; private set; }
        private static Menu menuIni;
        public static Spell.Active Q { get; private set; }
        public static Spell.Active W { get; private set; }
        public static Spell.Skillshot E { get; private set; }
        public static Spell.Targeted R { get; private set; }
        private static readonly AIHeroClient player = ObjectManager.Player;

        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoad;
        }


        private static void OnLoad(EventArgs args)
        {
            if (player.ChampionName != ChampName)
            {
                return;
            }

            menuIni = MainMenu.AddMenu("Darius", "Darius");
            menuIni.AddGroupLabel("Darius The Dank Memes Master!");
            menuIni.AddGroupLabel("Global Settings");
            menuIni.Add("Items", new CheckBox("Use Items?"));
            menuIni.Add("Combo", new CheckBox("Use Combo?"));
            menuIni.Add("Harass", new CheckBox("Use Harass?"));
            menuIni.Add("Clear", new CheckBox("Use Clear?"));
            menuIni.Add("Drawings", new CheckBox("Use Drawings?"));


            QMenu = menuIni.AddSubMenu("Q Settings");
            QMenu.AddGroupLabel("Q Settings");
            QMenu.Add("Combo", new CheckBox("Q Combo"));
            QMenu.Add("Harass", new CheckBox("Q Harass"));
            QMenu.Add("Clear", new CheckBox("Q LaneClear"));
            QMenu.Add("Flee", new CheckBox("Q Flee (Ignores Stick to target)"));
            QMenu.Add("QFlee", new Slider("Cast Q flee When HP is below %", 90, 0, 100));
            QMenu.AddGroupLabel("Extra Settings");
            QMenu.Add("Stick", new CheckBox("Stick to Target while Casting Q", false));
            QMenu.Add("range", new CheckBox("Dont Cast Q when Enemy in AA range", false));


            WMenu = menuIni.AddSubMenu("W Settings");
            WMenu.AddGroupLabel("W Settings");
            WMenu.Add("Combo", new CheckBox("W Combo"));
            WMenu.Add("Harass", new CheckBox("W Harass"));
            WMenu.Add("Clear", new CheckBox("W LaneClear"));


            EMenu = menuIni.AddSubMenu("E Settings");
            EMenu.AddGroupLabel("E Settings");
            EMenu.Add("Combo", new CheckBox("E Combo"));
            EMenu.Add("Harass", new CheckBox("E Harass"));
            EMenu.Add("Gapclose", new CheckBox("E To Interrupt"));


            RMenu = menuIni.AddSubMenu("R Settings");
            RMenu.AddGroupLabel("R Settings");
            RMenu.Add("Combo", new CheckBox("R Combo", false));
            RMenu.Add("KillSteal", new CheckBox("R AutoKillSteal"));
            RMenu.Add("count", new Slider("Cast R Combo When Passive Count >=", 5, 0, 5));


            ManaMenu = menuIni.AddSubMenu("Mana Manager");
            ManaMenu.AddGroupLabel("Harass");
            ManaMenu.Add("harassmana", new Slider("Harass Mana %", 75, 0, 100));
            ManaMenu.AddGroupLabel("Lane Clear");
            ManaMenu.Add("lanemana", new Slider("Lane Clear Mana %", 60, 0, 100));


            ItemsMenu = menuIni.AddSubMenu("Items");
            ItemsMenu.AddGroupLabel("Items Settings");
            ItemsMenu.Add("UseHydra", new CheckBox("Use Hydra / Timat / Titanic"));
            ItemsMenu.Add("useGhostblade", new CheckBox("Use Youmuu's Ghostblade"));
            ItemsMenu.Add("UseBOTRK", new CheckBox("Use Blade of the Ruined King"));
            ItemsMenu.Add("UseBilge", new CheckBox("Use Bilgewater Cutlass"));
            ItemsMenu.Add("eL", new Slider("Use On Enemy health", 65, 0, 100));
            ItemsMenu.Add("oL", new Slider("Use On My health", 65, 0, 100));


            DrawMenu = menuIni.AddSubMenu("Drawings");
            DrawMenu.AddGroupLabel("Drawing Settings");
            DrawMenu.Add("Q", new CheckBox("Draw Q"));
            DrawMenu.Add("W", new CheckBox("Draw W"));
            DrawMenu.Add("E", new CheckBox("Draw E"));
            DrawMenu.Add("R", new CheckBox("Draw R"));

            Q = new Spell.Active(SpellSlot.Q, 400);
            W = new Spell.Active(SpellSlot.W, 300);
            E = new Spell.Skillshot(SpellSlot.E, 540, SkillShotType.Cone, 250, 100, 120);
            R = new Spell.Targeted(SpellSlot.R, 460);

            Game.OnUpdate += OnUpdate;
            Drawing.OnDraw += OnDraw;
        }
        

        private static void OnUpdate(EventArgs args)
        {
            var target = TargetSelector.GetTarget(500, DamageType.Physical);
            var lanemana = ManaMenu["lanemana"].Cast<Slider>().CurrentValue;
            var harassmana = ManaMenu["harassmana"].Cast<Slider>().CurrentValue;
            if (player.IsDead || MenuGUI.IsChatOpen || player.IsRecalling())
            {
                return;
            }

            var flags = Orbwalker.ActiveModesFlags;
            if (flags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                Combo();
            }

            if (flags.HasFlag(Orbwalker.ActiveModes.LaneClear) && Player.Instance.ManaPercent >= lanemana)
            {
                Clear();
            }

            if (flags.HasFlag(Orbwalker.ActiveModes.Harass) && Player.Instance.ManaPercent >= harassmana)
            {
                Harass();
            }

            if (flags.HasFlag(Orbwalker.ActiveModes.Flee))
            {
                Flee();
            }

            if (RMenu["KillSteal"].Cast<CheckBox>().CurrentValue)
            {
                KillSteal();
            }
        }
        

        private static void Combo()
        {
            var stick = TargetSelector.GetTarget(700, DamageType.Physical);
            var target = TargetSelector.GetTarget(Q.Range, DamageType.Physical);
            var rt = TargetSelector.GetTarget(R.Range, DamageType.True);
            var Qrange = QMenu["range"].Cast<CheckBox>().CurrentValue;
            var Qcombo = QMenu["Combo"].Cast<CheckBox>().CurrentValue && Q.IsReady();
            var Wcombo = WMenu["Combo"].Cast<CheckBox>().CurrentValue;
            var Ecombo = EMenu["Combo"].Cast<CheckBox>().CurrentValue && E.IsReady();
            var Rcombo = RMenu["Combo"].Cast<CheckBox>().CurrentValue && R.IsReady();
            var buffcount = RMenu["count"].Cast<Slider>().CurrentValue;

            if (target != null)
            {
                if (Ecombo)
                {
                    if (target.IsValidTarget(E.Range) && (Q.IsReady() && !target.IsValidTarget(Q.Range)))
                    {
                        var pred = E.GetPrediction(target);
                        E.Cast(pred.CastPosition);
                    }

                    if (target.IsValidTarget(E.Range) && !player.IsInAutoAttackRange(target))
                    {
                        var pred = E.GetPrediction(target);
                        E.Cast(pred.CastPosition);
                    }
                }

                if (QMenu["Stick"].Cast<CheckBox>().CurrentValue)
                {
                    if (player.HasBuff("RumbleDangerZone"))
                    {
                        Player.IssueOrder(GameObjectOrder.MoveTo, stick.Position);
                    }
                }
                if (Qcombo)
                {
                    if (Qrange && target.IsValidTarget(Q.Range))
                    {
                        if (!player.IsInAutoAttackRange(target))
                        {
                            Q.Cast();
                        }
                    }

                    if (!Qrange && target.IsValidTarget(Q.Range))
                    {
                            Q.Cast();
                    }

                    if (player.GetSpellDamage(target, SpellSlot.Q) > target.TotalShieldHealth())
                    {
                        Q.Cast();
                    }
                }

                if (Wcombo)
                {
                    if (player.IsInAutoAttackRange(target) && W.IsReady())
                    {
                        Player.IssueOrder(GameObjectOrder.AttackUnit, target);
                        W.Cast();
                    }

                    if (player.HasBuff("DariusNoxianTacticsActive"))
                    {
                        Player.IssueOrder(GameObjectOrder.AttackUnit, target);
                    }
                }
                
                if (Rcombo && rt != null)
                {
                    if (rt.IsValidTarget(R.Range) && player.GetSpellDamage(rt, SpellSlot.R) > rt.TotalShieldHealth())
                    {
                        R.Cast(rt);
                    }
                }

                if (menuIni["Items"].Cast<CheckBox>().CurrentValue)
                {
                    Items();
                }
            }
        }

        private static void Harass()
        {
            var target = TargetSelector.GetTarget(Q.Range, DamageType.Physical);
            var Qharass = QMenu["Harass"].Cast<CheckBox>().CurrentValue;
            var Wharass = WMenu["Harass"].Cast<CheckBox>().CurrentValue;
            var Eharass = EMenu["Harass"].Cast<CheckBox>().CurrentValue;

            if (target != null)
            {
                if (Qharass)
                {
                    if (target.IsValidTarget(Q.Range) && !target.IsInAutoAttackRange(player))
                    {
                        Q.Cast();
                    }
                }


                if (Wharass)
                {
                    if (target.IsValidTarget(W.Range))
                    {
                        Player.IssueOrder(GameObjectOrder.AttackUnit, target);
                        W.Cast();
                    }
                }


                if (Eharass)
                {
                    if (target.IsValidTarget(E.Range) && ((Q.IsReady() && !target.IsValidTarget(Q.Range)) || (!Q.IsReady() && target.IsInAutoAttackRange(player))))
                    {
                        var pred = E.GetPrediction(target);
                        E.Cast(pred.CastPosition);
                    }
                }
            }
        }

        private static void Clear()
        {
            var Qclear = QMenu["Clear"].Cast<CheckBox>().CurrentValue;
            var Wclear = WMenu["Clear"].Cast<CheckBox>().CurrentValue;

                var allMinions = EntityManager.MinionsAndMonsters.Get(
                    EntityManager.MinionsAndMonsters.EntityType.Minion,
                    EntityManager.UnitTeam.Enemy,
                    ObjectManager.Player.Position,
                    Q.Range,
                    false);
                if (allMinions == null)
                {
                    return;
                }
                    foreach (var minion in allMinions)
                {
                    if (Qclear)
                    {
                        allMinions.Any();
                        {
                            var fl = EntityManager.MinionsAndMonsters.GetLineFarmLocation(allMinions, 100, (int)Q.Range);
                            if (fl.HitNumber >= 1)
                            {
                                Q.Cast();
                            }
                        }
                    }

                    if (Wclear)
                    {
                        if (minion.IsValidTarget() && W.IsReady()
                        && player.Distance(minion.ServerPosition) <= 225f)
                        {
                            W.Cast();
                            Player.IssueOrder(GameObjectOrder.AttackUnit, minion);
                        }
                    }
                }
        }

        private static void Flee()
        {
            var hp = QMenu["QFlee"].Cast<Slider>().CurrentValue;
            var Qflee = QMenu["Flee"].Cast<CheckBox>().CurrentValue;
            if (Qflee && player.HealthPercent < hp)
            {
                if (player.CountEnemiesInRange(Q.Range) >= 1)
                {
                    Q.Cast();
                }
            }
        }

        private static void KillSteal()
        {
            var Rks = RMenu["KillSteal"].Cast<CheckBox>().CurrentValue;

            if (Rks)
            {
                var target =
                    ObjectManager.Get<AIHeroClient>()
                        .FirstOrDefault(
                            enemy =>
                                enemy.IsEnemy
                                && enemy.IsValidTarget(1000)
                                && !enemy.IsDead
                                && !enemy.HasBuff("kindredrnodeathbuff")
                                && !enemy.HasBuff("JudicatorIntervention")
                                && !enemy.HasBuff("ChronoShift")
                                && !enemy.HasBuff("UndyingRage"));
                if (target != null)
                {
                    // Credits cancerous
                    int passiveCounter = target.GetBuffCount("DariusHemo") <= 0 ? 0 : target.GetBuffCount("DariusHemo");
                    if (RDmg(target, passiveCounter) >= target.Health + PassiveDmg(target, 1))
                    {
                        if (target.IsValidTarget(R.Range))
                        {
                            R.Cast(target);
                        }

                        if (!target.IsValidTarget(R.Range) && target.IsValidTarget(E.Range) && player.Mana >= (R.Handle.SData.Mana + E.Handle.SData.Mana))
                        {
                            E.Cast(target.Position);
                        }
                    }

                    if (target.IsValidTarget(R.Range) && target.TotalShieldHealth() < player.GetSpellDamage(target, SpellSlot.R))
                    {
                        if (!target.IsValidTarget(R.Range) && target.IsValidTarget(E.Range) && player.Mana >= (R.Handle.SData.Mana + E.Handle.SData.Mana))
                        {
                            E.Cast(target.Position);
                        }

                        R.Cast(target);
                    }
                }
            }
        }



        private static void Items()
        {
            var target = TargetSelector.GetTarget(Q.Range, DamageType.Physical);
            if (target == null || !target.IsValidTarget())
            {
                return;
            }

            if (Botrk.IsReady() && Botrk.IsOwned(player) && Botrk.IsInRange(target)
                && target.HealthPercent <= ItemsMenu["eL"].Cast<Slider>().CurrentValue
                && ItemsMenu["UseBOTRK"].Cast<CheckBox>().CurrentValue)
            {
                Botrk.Cast(target);
            }

            if (Botrk.IsReady() && Botrk.IsOwned(player) && Botrk.IsInRange(target)
                && target.HealthPercent <= ItemsMenu["oL"].Cast<Slider>().CurrentValue
                && ItemsMenu["UseBOTRK"].Cast<CheckBox>().CurrentValue)

            {
                Botrk.Cast(target);
            }

            if (Cutlass.IsReady() && Cutlass.IsOwned(player) && Cutlass.IsInRange(target)
                && target.HealthPercent <= ItemsMenu["eL"].Cast<Slider>().CurrentValue
                && ItemsMenu["UseBilge"].Cast<CheckBox>().CurrentValue)
            {
                Cutlass.Cast(target);
            }

            if (Youmuu.IsReady() && Youmuu.IsOwned(player) && target.IsValidTarget(Q.Range)
                && ItemsMenu["useGhostblade"].Cast<CheckBox>().CurrentValue)
            {
                Youmuu.Cast();
            }
        }

        // Credits cancerous
        public static float RDmg(Obj_AI_Base unit, int stackcount)
        {
            var bonus =
                stackcount *
                    (new[] { 20, 20, 40, 60 }[R.Level] + (0.15 * Player.Instance.FlatPhysicalDamageMod));

            return
                (float)
                (bonus
                 + Player.Instance.CalculateDamageOnUnit(
                     unit,
                     DamageType.True,
                     new[] { 100, 100, 200, 300 }[R.Level] + (float)(0.75 * Player.Instance.FlatPhysicalDamageMod)));
        }

        public static float PassiveDmg(Obj_AI_Base unit, int stackcount)
        {
            if (stackcount < 1)
            {
                stackcount = 1;
            }

            return Player.Instance.CalculateDamageOnUnit(
                unit,
                DamageType.Physical,
                (9 + Player.Instance.Level) + (float)(0.3 * Player.Instance.FlatPhysicalDamageMod)) * stackcount;
        }


        private static void OnDraw(EventArgs args)
        {
            if (!menuIni.Get<CheckBox>("Drawings").CurrentValue)
            {
                return;
            }

            if (DrawMenu.Get<CheckBox>("Q").CurrentValue && Q.IsLearned)
            {
                if (Q.IsReady())
                {
                    Circle.Draw(Color.OrangeRed, Q.Range, ObjectManager.Player.Position);
                }

                if (!Q.IsReady())
                {
                    Circle.Draw(Color.DarkRed, Q.Range, ObjectManager.Player.Position);
                }
            }

            if (DrawMenu.Get<CheckBox>("W").CurrentValue && W.IsLearned)
            {
                if (W.IsReady())
                {
                    Circle.Draw(Color.OrangeRed, W.Range, ObjectManager.Player.Position);
                }

                if (!W.IsReady())
                {
                    Circle.Draw(Color.DarkRed, W.Range, ObjectManager.Player.Position);
                }
            }

            if (DrawMenu.Get<CheckBox>("E").CurrentValue && E.IsLearned)
            {
                if (E.IsReady())
                {
                    Circle.Draw(Color.OrangeRed, E.Range, ObjectManager.Player.Position);
                }

                if (!E.IsReady())
                {
                    Circle.Draw(Color.DarkRed, E.Range, ObjectManager.Player.Position);
                }
            }

            if (DrawMenu.Get<CheckBox>("R").CurrentValue && R.IsLearned)
            {
                if (R.IsReady())
                {
                    Circle.Draw(Color.OrangeRed, R.Range, ObjectManager.Player.Position);
                }
                if (!R.IsReady())
                {
                    Circle.Draw(Color.DarkRed, R.Range, ObjectManager.Player.Position);
                }
            }
        }
    }
}
