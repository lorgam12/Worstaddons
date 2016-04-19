namespace KappaLissandra
{
    using System;
    using System.Linq;

    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Enumerations;
    using EloBuddy.SDK.Rendering;
    using EloBuddy.SDK.Events;
    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Menu.Values;

    using SharpDX;

    internal class Lissandra
    {
        private static MissileClient LissEMissile;

        public static Spell.Skillshot Q { get; set; }

        public static Spell.Skillshot Qtest { get; set; }

        public static Spell.Skillshot Q2 { get; set; }

        public static Spell.Active W { get; set; }

        public static Spell.Skillshot E { get; set; }

        public static Spell.Skillshot E2 { get; set; }

        public static Spell.Targeted R { get; set; }

        public static Spell.Skillshot F { get; set; }

        public static AIHeroClient player { get; set; }

        public static Menu ComboMenu { get; private set; }

        public static Menu UltMenu { get; private set; }

        public static Menu HarassMenu { get; private set; }

        public static Menu LaneMenu { get; private set; }

        public static Menu MiscMenu { get; private set; }

        public static Menu FlashMenu { get; private set; }

        public static Menu FleeMenu { get; private set; }

        public static Menu DrawMenu { get; private set; }

        private static Menu menuIni;

        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoad;
        }

        private static void OnLoad(EventArgs args)
        {
            if (ObjectManager.Player.BaseSkinName != "Lissandra")
            {
                return;
            }

            menuIni = MainMenu.AddMenu("KappaLissandra", "KappaLissandra");
            menuIni.AddGroupLabel("Welcome to the Worst Lissandra addon!");
            menuIni.AddGroupLabel("Global Settings");
            menuIni.Add("Combo", new CheckBox("Use Combo?"));
            menuIni.Add("Harass", new CheckBox("Use Harass?"));
            menuIni.Add("LaneClear", new CheckBox("Use Lane Clear?"));
            menuIni.Add("JungleClear", new CheckBox("Use Jungle Clear?"));
            menuIni.Add("Flee", new CheckBox("Use Flee?"));
            menuIni.Add("Misc", new CheckBox("Use Misc?"));
            menuIni.Add("Drawings", new CheckBox("Use Drawings?"));

            UltMenu = menuIni.AddSubMenu("Ultimate");
            UltMenu.AddGroupLabel("Ultimate Settings");
            UltMenu.Add("aoeR", new CheckBox("AoE R Logic"));
            UltMenu.Add("RF", new CheckBox("Use R Finisher"));
            UltMenu.Add("RS", new CheckBox("Use R On Self"));
            UltMenu.Add("RE", new CheckBox("Use R On Enemy"));
            UltMenu.Add("hitR", new Slider("R AoE Hit >=", 2, 1, 5));
            UltMenu.Add("shp", new Slider("On Self Health to use R", 15, 0, 100));
            UltMenu.AddGroupLabel("Don't Use Ult On: Enemy");
            foreach (var enemy in ObjectManager.Get<AIHeroClient>())
            {
                CheckBox cb = new CheckBox(enemy.BaseSkinName) { CurrentValue = false };
                if (enemy.Team != ObjectManager.Player.Team)
                {
                    UltMenu.Add("DontUltenemy" + enemy.BaseSkinName, cb);
                }
            }

            ComboMenu = menuIni.AddSubMenu("Combo");
            ComboMenu.AddGroupLabel("Combo Settings");
            ComboMenu.Add("Q", new CheckBox("Use Q"));
            ComboMenu.Add("W", new CheckBox("Use W"));
            ComboMenu.Add("E", new CheckBox("Use E"));
            ComboMenu.Add("ET", new CheckBox("Use E2 If hit target"));
            ComboMenu.Add("E2", new CheckBox("Always E2 Max", false));
            ComboMenu.Add("ES", new CheckBox("Use E2 Safe", false));
            ComboMenu.Add("EHP", new Slider("Use E2 Safe if HP <= %", 30, 0, 100));
            ComboMenu.Add("ESE", new Slider("Use E2 Safe if Enemies are <=", 2, 1, 5));

            HarassMenu = menuIni.AddSubMenu("Harass");
            HarassMenu.AddGroupLabel("Harass Settings");
            HarassMenu.Add("Q", new CheckBox("Use Q"));
            HarassMenu.Add("W", new CheckBox("Use W"));
            HarassMenu.Add("E", new CheckBox("Use E", false));
            HarassMenu.Add("Mana", new Slider("Save Mana %", 30, 0, 100));

            LaneMenu = menuIni.AddSubMenu("Farm");
            LaneMenu.AddGroupLabel("LaneClear Settings");
            LaneMenu.Add("Q", new CheckBox("Use Q"));
            LaneMenu.Add("W", new CheckBox("Use W"));
            LaneMenu.Add("E", new CheckBox("Use E", false));
            LaneMenu.Add("Mana", new Slider("Save Mana %", 30, 0, 100));
            LaneMenu.AddGroupLabel("JungleClear Settings");
            LaneMenu.Add("jQ", new CheckBox("Use Q"));
            LaneMenu.Add("jW", new CheckBox("Use W"));
            LaneMenu.Add("jE", new CheckBox("Use E", false));

            if (Player.Spells.FirstOrDefault(o => o.SData.Name.Contains("SummonerFlash")) != null)
            {
                FlashMenu = menuIni.AddSubMenu("Flashy Combo");
                FlashMenu.AddGroupLabel("Flash Combo Settings");
                FlashMenu.Add("flash", new KeyBind("Enable", false, KeyBind.BindTypes.HoldActive));
                FlashMenu.Add("flashhit", new Slider("Flashy Combo Hit >=", 3, 1, 5));
                FlashMenu.Add("mode", new ComboBox("Flashy combo mode", 0, "Smart", "E > R", "Flash > R"));
                FlashMenu.AddGroupLabel("Smart Mode Explain");
                FlashMenu.AddLabel("Smart mode will do the follwing");
                FlashMenu.AddLabel("1- Will try to Flash > R To hit the Selected number of enemies");
                FlashMenu.AddLabel("2- If cant do step 1 Will try to E > R To hit the Selected number of enemies");
                FlashMenu.AddLabel(
                    "3- If cant do step 1 or 2 Will try to E > Flash > R To hit the Selected number of enemies");
                F = new Spell.Skillshot(ObjectManager.Player.GetSpellSlotFromName("SummonerFlash"), 1000, SkillShotType.Circular, 250, int.MaxValue, 450);
            }

            MiscMenu = menuIni.AddSubMenu("Misc");
            MiscMenu.AddGroupLabel("Misc Settings");
            MiscMenu.Add("gapcloserW", new CheckBox("Anti-GapCloser W"));
            MiscMenu.Add("gapcloserR", new CheckBox("Anti-GapCloser R"));
            MiscMenu.Add("Interruptr", new CheckBox("Interrupt R"));
            MiscMenu.Add("WTower", new CheckBox("Auto W Under Tower"));
            MiscMenu.Add("AutoW", new Slider("Auto W On Hit >=", 2, 1, 5));

            FleeMenu = menuIni.AddSubMenu("Flee");
            FleeMenu.AddGroupLabel("Flee Settings");
            FleeMenu.Add("Q", new CheckBox("Use Q"));
            FleeMenu.Add("W", new CheckBox("Use W"));
            FleeMenu.Add("E", new CheckBox("Use E"));

            DrawMenu = menuIni.AddSubMenu("Drawings");
            DrawMenu.AddGroupLabel("Drawing Settings");
            DrawMenu.Add("Q", new CheckBox("Draw Q"));
            DrawMenu.Add("W", new CheckBox("Draw W"));
            DrawMenu.Add("E", new CheckBox("Draw E"));
            DrawMenu.Add("R", new CheckBox("Draw R"));
            DrawMenu.Add("debug", new CheckBox("debug", false));

            Q = new Spell.Skillshot(SpellSlot.Q, 715, SkillShotType.Linear, 250, 2200, 75);
            Q2 = new Spell.Skillshot(SpellSlot.Q, 825, SkillShotType.Linear, 250, 2200, 90);
            Qtest = new Spell.Skillshot(SpellSlot.Q, 715, SkillShotType.Linear, 250, 2200, 75)
                        { AllowedCollisionCount = int.MaxValue };
            W = new Spell.Active(SpellSlot.W, 400);
            E = new Spell.Skillshot(SpellSlot.E, 1000, SkillShotType.Linear, 250, 850, 125);
            E2 = new Spell.Skillshot(SpellSlot.E, 2000, SkillShotType.Linear, 250, 850, 125);
            R = new Spell.Targeted(SpellSlot.R, 500);

            Game.OnUpdate += OnUpdate;
            GameObject.OnCreate += OnCreate;
            GameObject.OnDelete += OnDelete;
            Drawing.OnDraw += OnDraw;
            Obj_AI_Base.OnProcessSpellCast += OnProcessSpellCast;
            Obj_AI_Base.OnBasicAttack += OnBasicAttack;
            Interrupter.OnInterruptableSpell += OnInterruptableSpell;
            Gapcloser.OnGapcloser += OnGapcloser;
        }

        public static void OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!(args.Target is AIHeroClient))
            {
                return;
            }

            var caster = sender;
            var target = (AIHeroClient)args.Target;

            if ((!(caster is AIHeroClient) && !(caster is Obj_AI_Turret)) || caster == null || target == null)
            {
                return;
            }
            if (target.IsMe)
            {
                var shp = UltMenu["shp"].Cast<Slider>().CurrentValue;
                var useRS = UltMenu["RS"].Cast<CheckBox>().CurrentValue && R.IsReady();
                if (sender == null || sender.IsAlly || sender.IsMe)
                {
                    return;
                }

                if (sender.IsEnemy || sender is Obj_AI_Turret)
                {
                    if (useRS && player.HealthPercent <= shp && !player.HasBuff("kindredrnodeathbuff")
                        && !player.HasBuff("JudicatorIntervention") && !player.HasBuff("ChronoShift")
                        && !player.HasBuff("UndyingRage"))
                    {
                        R.Cast(player);
                    }
                }
            }
        }

        public static void OnBasicAttack(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!(args.Target is AIHeroClient))
            {
                return;
            }

            var caster = sender;
            var target = (AIHeroClient)args.Target;

            if ((!(caster is AIHeroClient) && !(caster is Obj_AI_Turret)) || caster == null || target == null)
            {
                return;
            }

            if (target.IsMe)
            {
                var shp = UltMenu["shp"].Cast<Slider>().CurrentValue;
                var useRS = UltMenu["RS"].Cast<CheckBox>().CurrentValue && R.IsReady();
                if (sender == null || sender.IsAlly || sender.IsMe)
                {
                    return;
                }

                if (sender.IsEnemy || sender is Obj_AI_Turret)
                {
                    if (useRS && player.HealthPercent <= shp && !player.HasBuff("kindredrnodeathbuff")
                        && !player.HasBuff("JudicatorIntervention") && !player.HasBuff("ChronoShift")
                        && !player.HasBuff("UndyingRage"))
                    {
                        R.Cast(player);
                    }
                }
            }
        }

        private static void OnGapcloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs e)
        {
            if (!menuIni.Get<CheckBox>("Misc").CurrentValue || sender == null || sender.IsAlly || sender.IsMe)
            {
                return;
            }

            if (W.IsReady() && sender.IsValidTarget(W.Range - 15) && MiscMenu.Get<CheckBox>("gapcloserW").CurrentValue)
            {
                W.Cast();
                return;
            }

            if (R.IsReady() && sender.IsValidTarget(R.Range) && MiscMenu.Get<CheckBox>("gapcloserR").CurrentValue
                && !UltMenu["DontUltenemy" + sender.BaseSkinName].Cast<CheckBox>().CurrentValue)
            {
                R.Cast(sender);
            }
        }

        private static void OnInterruptableSpell(Obj_AI_Base Sender, Interrupter.InterruptableSpellEventArgs args)
        {
            if (!menuIni.Get<CheckBox>("Misc").CurrentValue || Sender == null || Sender.IsAlly || Sender.IsMe
                || !Sender.IsEnemy)
            {
                return;
            }

            if (R.IsReady() && Sender.IsValidTarget(R.Range) && MiscMenu.Get<CheckBox>("Interruptr").CurrentValue
                && !UltMenu["DontUltenemy" + Sender.BaseSkinName].Cast<CheckBox>().CurrentValue)
            {
                R.Cast(Sender);
            }
        }

        private static void OnUpdate(EventArgs args)
        {
            player = ObjectManager.Player;

            var flags = Orbwalker.ActiveModesFlags;
            if (flags.HasFlag(Orbwalker.ActiveModes.Combo) && menuIni.Get<CheckBox>("Combo").CurrentValue)
            {
                Combo();
            }

            if (flags.HasFlag(Orbwalker.ActiveModes.Harass) && menuIni.Get<CheckBox>("Harass").CurrentValue)
            {
                Harass();
            }

            if (flags.HasFlag(Orbwalker.ActiveModes.LaneClear) && menuIni.Get<CheckBox>("LaneClear").CurrentValue)
            {
                if (player.ManaPercent > LaneMenu["Mana"].Cast<Slider>().CurrentValue)
                {
                    Clear();
                }
            }

            if (flags.HasFlag(Orbwalker.ActiveModes.JungleClear) && menuIni.Get<CheckBox>("JungleClear").CurrentValue)
            {
                if (player.ManaPercent > LaneMenu["Mana"].Cast<Slider>().CurrentValue)
                {
                    jClear();
                }
            }

            if (flags.HasFlag(Orbwalker.ActiveModes.Flee) && menuIni.Get<CheckBox>("Flee").CurrentValue)
            {
                Flee();
            }

            if (Player.Spells.FirstOrDefault(o => o.SData.Name.Contains("SummonerFlash")) != null)
            {
                if (FlashMenu["flash"].Cast<KeyBind>().CurrentValue)
                {
                    Flash();
                    Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos);
                }
            }

            if (W.IsReady())
            {
                if (MiscMenu.Get<CheckBox>("WTower").CurrentValue && player.CountEnemiesInRange(W.Range) >= 1
                    && player.IsUnderHisturret() && player.IsUnderTurret() && !player.IsUnderEnemyturret())
                {
                    W.Cast();
                }

                if (player.CountEnemiesInRange(W.Range) >= MiscMenu.Get<Slider>("AutoW").CurrentValue)
                {
                    W.Cast();
                }
            }
        }

        private static void OnCreate(GameObject sender, EventArgs args)
        {
            var miss = sender as MissileClient;
            if (miss != null && miss.IsValid)
            {
                if (miss.SpellCaster.IsMe && miss.SpellCaster.IsValid && miss.SData.Name == "LissandraEMissile")
                {
                    LissEMissile = miss;
                }
            }
        }

        private static void OnDelete(GameObject sender, EventArgs args)
        {
            var miss = sender as MissileClient;
            if (miss == null || !miss.IsValid)
            {
                return;
            }
            if (miss.SpellCaster is AIHeroClient && miss.SpellCaster.IsValid && miss.SpellCaster.IsMe
                && miss.SData.Name == "LissandraEMissile")
            {
                LissEMissile = null;
            }
        }

        private static void Flash()
        {
            if (R.IsReady())
            {
                var hit = FlashMenu["flashhit"].Cast<Slider>().CurrentValue;

                var enemies = EntityManager.Heroes.Enemies.Where(n => n.IsValidTarget(E2.Range));
                var aoePrediction =
                    Prediction.Position.PredictCircularMissileAoe(
                        enemies.ToArray(),
                        E2.Range,
                        500,
                        R.CastDelay,
                        R.Handle.SData.MissileSpeed)
                        .OrderByDescending(r => r.GetCollisionObjects<AIHeroClient>().Length)
                        .FirstOrDefault();
                var target = TargetSelector.GetTarget(R.Range, DamageType.Magical);

                if (aoePrediction.CollisionObjects.Length < hit || target == null)
                {
                    return;
                }

                if (FlashMenu["mode"].Cast<ComboBox>().CurrentValue == 0)
                {
                    if (player.CountEnemiesInRange(R.Range) >= hit && target.CountEnemiesInRange(R.Range) <= hit)
                    {
                        R.Cast(player);
                    }

                    if (player.CountEnemiesInRange(R.Range) <= hit && target.CountEnemiesInRange(R.Range) >= hit)
                    {
                        R.Cast(target);
                    }

                    if (!aoePrediction.CastPosition.IsInRange(player, R.Range) && aoePrediction.CastPosition.IsInRange(player, E2.Range - 50))
                    {
                        if (LissEMissile == null && E2.IsReady() && player.Mana >= (R.Handle.SData.Mana + E2.Handle.SData.Mana))
                        {
                            E2.Cast(aoePrediction.CastPosition);
                        }
                    }

                    if (!aoePrediction.CastPosition.IsInRange(player, R.Range) && !aoePrediction.CastPosition.IsInRange(player, E.Range + R.Range - 50)
                        && aoePrediction.CastPosition.IsInRange(player, E2.Range - 50) && F.IsReady())
                    {
                        if (LissEMissile == null && E.IsReady() && player.Mana >= (R.Handle.SData.Mana + E2.Handle.SData.Mana))
                        {
                            E2.Cast(aoePrediction.CastPosition);
                        }
                    }

                    if (F.IsReady())
                    {
                        if (LissEMissile != null && LissEMissile.Position.IsInRange(aoePrediction.CastPosition, F.Range - 100))
                        {
                            E2.Cast(Game.CursorPos);
                        }
                    }

                    if (LissEMissile != null && LissEMissile.Position.IsInRange(aoePrediction.CastPosition, R.Range - 250))
                    {
                        E2.Cast(Game.CursorPos);
                    }

                    if (!aoePrediction.CastPosition.IsInRange(player, R.Range) && !E2.IsReady()
                        && !aoePrediction.CastPosition.IsInRange(player, F.Range))
                    {
                        F.Cast(aoePrediction.CastPosition);
                    }
                }

                if (FlashMenu["mode"].Cast<ComboBox>().CurrentValue == 1)
                {
                    if (player.CountEnemiesInRange(R.Range) >= hit)
                    {
                        R.Cast(player);
                    }

                    if (player.CountEnemiesInRange(R.Range) <= hit && target.CountEnemiesInRange(R.Range - 50) >= hit)
                    {
                        R.Cast(target);
                    }

                    if (!aoePrediction.CastPosition.IsInRange(player.Position, R.Range) && aoePrediction.CastPosition.IsInRange(player.Position, E.Range - 50))
                    {
                        if (LissEMissile == null && E.IsReady() && player.Mana >= (R.Handle.SData.Mana + E2.Handle.SData.Mana))
                        {
                            E.Cast(aoePrediction.CastPosition);
                        }
                    }

                    if (LissEMissile != null && LissEMissile.Position.IsInRange(aoePrediction.CastPosition, R.Range - 200))
                    {
                        E.Cast(Game.CursorPos);
                    }
                }

                if (FlashMenu["mode"].Cast<ComboBox>().CurrentValue == 2)
                {
                    if (player.CountEnemiesInRange(R.Range) >= hit)
                    {
                        R.Cast(player);
                    }

                    if (player.CountEnemiesInRange(R.Range) <= hit && target.CountEnemiesInRange(R.Range - 50) >= hit)
                    {
                        R.Cast(target);
                    }

                    if (!player.IsInRange(aoePrediction.CastPosition, R.Range) && player.IsInRange(aoePrediction.CastPosition, F.Range - 100))
                    {
                        F.Cast(aoePrediction.CastPosition);
                    }
                }
            }
        }

        private static void Flee()
        {
            player = ObjectManager.Player;
            var useQ = FleeMenu["Q"].Cast<CheckBox>().CurrentValue && Q.IsReady();
            var useW = FleeMenu["W"].Cast<CheckBox>().CurrentValue && W.IsReady();
            var useE = FleeMenu["E"].Cast<CheckBox>().CurrentValue && E.IsReady();

            if (useW)
            {
                CastW();
            }

            if (useQ)
            {
                CastQ();
            }

            if (LissEMissile == null && useE)
            {
                E.Cast(Game.CursorPos);
            }

            if (useE && LissEMissile != null && LissEMissile.Position.IsInRange(LissEMissile.EndPosition, 50))
            {
                E.Cast(Game.CursorPos);
            }
        }

        private static void Combo()
        {
            player = ObjectManager.Player;
            var useQ = ComboMenu["Q"].Cast<CheckBox>().CurrentValue && Q.IsReady();
            var useW = ComboMenu["W"].Cast<CheckBox>().CurrentValue && W.IsReady();
            var useE = ComboMenu["E"].Cast<CheckBox>().CurrentValue && E.IsReady();
            var useRS = UltMenu["RS"].Cast<CheckBox>().CurrentValue && R.IsReady();
            var useRE = UltMenu["RE"].Cast<CheckBox>().CurrentValue && R.IsReady();

            if (useW)
            {
                CastW();
            }

            if (useQ)
            {
                CastQ();
            }

            if (useE)
            {
                CastE();
            }

            if (useRS || useRE)
            {
                CastR();
            }
        }

        private static void Harass()
        {
            player = ObjectManager.Player;
            var useQ = HarassMenu["Q"].Cast<CheckBox>().CurrentValue;
            var useW = HarassMenu["W"].Cast<CheckBox>().CurrentValue;
            var useE = HarassMenu["E"].Cast<CheckBox>().CurrentValue;

            var Target = TargetSelector.GetTarget(E.Range * 0.94f, DamageType.Magical);

            if (Target == null || !Target.IsValidTarget())
            {
                Target =
                    EntityManager.Heroes.Enemies.FirstOrDefault(
                        h =>
                        h.IsValidTarget()
                        && (Vector3.Distance(h.ServerPosition, player.ServerPosition) < E.Range * 0.94) && !h.IsZombie);
            }

            if (Target != null && !Target.IsInvulnerable)
            {
                if (useQ)
                {
                    CastQ();
                }

                if (useW)
                {
                    CastW();
                }

                if (useE)
                {
                    CastE();
                }
            }
        }

        private static void Clear()
        {
            player = ObjectManager.Player;
            var useQ = LaneMenu["Q"].Cast<CheckBox>().CurrentValue;
            var useW = LaneMenu["W"].Cast<CheckBox>().CurrentValue;
            var useE = LaneMenu["E"].Cast<CheckBox>().CurrentValue;

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

            if (useQ)
            {
                var fl = EntityManager.MinionsAndMonsters.GetLineFarmLocation(allMinions, Q.Width, (int)Q.Range);
                if (fl.HitNumber >= 1)
                {
                    Q.Cast(fl.CastPosition);
                }
            }

            if (useW)
            {
                var fl = EntityManager.MinionsAndMonsters.GetLineFarmLocation(allMinions, 100, (int)W.Range);
                if (fl.HitNumber >= 2)
                {
                    W.Cast();
                }
            }

            if (useE && LissEMissile == null && E.Handle.ToggleState == 1)
            {
                var fl = EntityManager.MinionsAndMonsters.GetLineFarmLocation(allMinions, E.Width, (int)E.Range);
                if (fl.HitNumber >= 1)
                {
                    E.Cast(fl.CastPosition);
                }
            }
        }

        private static void jClear()
        {
            player = ObjectManager.Player;
            var useQ = LaneMenu["jQ"].Cast<CheckBox>().CurrentValue;
            var useW = LaneMenu["jW"].Cast<CheckBox>().CurrentValue;
            var useE = LaneMenu["jE"].Cast<CheckBox>().CurrentValue;

            var jmobs =
                ObjectManager.Get<Obj_AI_Minion>()
                    .OrderBy(m => m.CampNumber)
                    .Where(m => m.IsMonster && m.IsEnemy && !m.IsDead);
            foreach (var jmob in jmobs)
            {
                if (useQ && jmob.IsValidTarget(Q.Range) && jmobs.Any())
                {
                    Q.Cast(jmob.Position);
                }

                if (useW && jmob.IsValidTarget(W.Range))
                {
                    W.Cast();
                }

                if (useE && E.IsReady() && jmob.IsValidTarget(E.Range) && LissEMissile == null
                    && E.Handle.ToggleState == 1)
                {
                    E.Cast(jmob.Position);
                }
            }
        }

        private static void CastQ()
        {
            if (!Q.IsReady())
            {
                return;
            }

            var target = TargetSelector.GetTarget(Q.Range, DamageType.Physical);
            if (target == null)
            {
                return;
            }

            if (target.IsValidTarget(Q.Range))
            {
                var predq = Q.GetPrediction(target);
                Q.Cast(predq.CastPosition);
            }

            var target2 = TargetSelector.GetTarget(Q2.Range, DamageType.Physical);

            if (target2 == null)
            {
                return;
            }

            var pred = Q2.GetPrediction(target2);
            var collisions =
                EntityManager.MinionsAndMonsters.EnemyMinions.Where(it => it.IsValidTarget(Q.Range)).ToList();

            if (!collisions.Any())
            {
                return;
            }

            foreach (var minion in collisions)
            {
                var poly = new Geometry.Polygon.Rectangle(
                    (Vector2)player.ServerPosition,
                    player.ServerPosition.Extend(minion.ServerPosition, Q2.Range),
                    Q2.Width);

                if (poly.IsInside(pred.UnitPosition))
                {
                    Q.Cast(minion.Position);
                }
            }
        }

        private static void CastW()
        {
            if (!W.IsReady())
            {
                return;
            }

            var target = TargetSelector.GetTarget(W.Range, DamageType.Magical);
            if (target != null && Vector3.Distance(target.ServerPosition, player.ServerPosition) <= W.Range - 5)
            {
                W.Cast();
            }

            if (
                EntityManager.Heroes.Enemies.Any(
                    h =>
                    h.IsValidTarget() && h != null
                    && (Vector3.Distance(h.ServerPosition, player.ServerPosition) < W.Range) && !h.IsZombie))
            {
                W.Cast();
            }
        }

        private static void CastE()
        {
            var useE = ComboMenu["E"].Cast<CheckBox>().CurrentValue && E.IsReady();
            var EHP = ComboMenu["EHP"].Cast<Slider>().CurrentValue;
            var ESE = ComboMenu["ESE"].Cast<Slider>().CurrentValue;
            var useET = ComboMenu["ET"].Cast<CheckBox>().CurrentValue && E.IsReady();
            var useE2 = ComboMenu["E2"].Cast<CheckBox>().CurrentValue && E.IsReady();
            var useES = ComboMenu["ES"].Cast<CheckBox>().CurrentValue && E.IsReady();
            if (!E.IsReady())
            {
                return;
            }

            var target = TargetSelector.GetTarget(E.Range + 100, DamageType.Magical);
            if (LissEMissile == null && !player.HasBuff("LissandraE") && target != null && useE)
            {
                var pred = E.GetPrediction(target);
                E.Cast(pred.CastPosition);
            }

            if (useES && LissEMissile != null && LissEMissile.Position.CountEnemiesInRange(W.Range - 50) <= ESE
                && player.HealthPercent <= EHP)
            {
                E.Cast(Game.CursorPos);
            }

            if (useET && LissEMissile != null && LissEMissile.Position.IsInRange(target, 250))
            {
                E.Cast(Game.CursorPos);
            }

            if (useE2 && LissEMissile != null && LissEMissile.Position.IsInRange(LissEMissile.EndPosition, 50))
            {
                E.Cast(Game.CursorPos);
            }
        }

        private static void CastR()
        {
            var aoeR = UltMenu["aoeR"].Cast<CheckBox>().CurrentValue;
            var useRS = UltMenu["RS"].Cast<CheckBox>().CurrentValue && R.IsReady();
            var useRE = UltMenu["RE"].Cast<CheckBox>().CurrentValue && R.IsReady();
            var useRF = UltMenu["RF"].Cast<CheckBox>().CurrentValue && R.IsReady();
            var hitR = UltMenu["hitR"].Cast<Slider>().CurrentValue;
            var target =
                EntityManager.Heroes.Enemies.FirstOrDefault(
                    e =>
                    !e.IsZombie && !e.IsInvulnerable && !e.IsDead && !e.HasBuff("kindredrnodeathbuff")
                    && !e.HasBuff("JudicatorIntervention") && !e.HasBuff("ChronoShift") && !e.HasBuff("UndyingRage"));

            if (target != null && useRE)
            {
                if (aoeR && target.CountEnemiesInRange(R.Range) >= hitR && target.IsValidTarget(R.Range)
                    && !UltMenu["DontUltenemy" + target.BaseSkinName].Cast<CheckBox>().CurrentValue)
                {
                    R.Cast(target);
                }
            }

            if (useRS)
            {
                if (aoeR && player.CountEnemiesInRange(R.Range) >= hitR
                    && !UltMenu["DontUltally" + player.BaseSkinName].Cast<CheckBox>().CurrentValue)
                {
                    R.Cast(player);
                }
            }

            if (target != null && useRF)
            {
                if (target.TotalShieldHealth() < player.GetSpellDamage(target, SpellSlot.R)
                    && !UltMenu["DontUltenemy" + target.BaseSkinName].Cast<CheckBox>().CurrentValue)
                {
                    if (target.IsValidTarget(R.Range))
                    {
                        R.Cast(target);
                    }

                    if (target.IsInRange(player, R.Range)
                        && !UltMenu["DontUltally" + player.BaseSkinName].Cast<CheckBox>().CurrentValue)
                    {
                        R.Cast(player);
                    }
                }
            }
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
                    Circle.Draw(Color.Blue, Q.Range, ObjectManager.Player.Position);
                }

                if (!Q.IsReady())
                {
                    Circle.Draw(Color.DarkBlue, Q.Range, ObjectManager.Player.Position);
                }
            }

            if (DrawMenu.Get<CheckBox>("W").CurrentValue && W.IsLearned)
            {
                if (W.IsReady())
                {
                    Circle.Draw(Color.Blue, W.Range, ObjectManager.Player.Position);
                }

                if (!W.IsReady())
                {
                    Circle.Draw(Color.DarkBlue, W.Range, ObjectManager.Player.Position);
                }
            }

            if (DrawMenu.Get<CheckBox>("E").CurrentValue && E.IsLearned)
            {
                if (E.IsReady())
                {
                    Circle.Draw(Color.Blue, E.Range, ObjectManager.Player.Position);
                }

                if (!E.IsReady())
                {
                    Circle.Draw(Color.DarkBlue, E.Range, ObjectManager.Player.Position);
                }
            }

            if (DrawMenu.Get<CheckBox>("R").CurrentValue && R.IsLearned)
            {
                if (R.IsReady())
                {
                    Circle.Draw(Color.Blue, R.Range, ObjectManager.Player.Position);
                }

                if (!R.IsReady())
                {
                    Circle.Draw(Color.DarkBlue, R.Range, ObjectManager.Player.Position);
                }
            }

            if (DrawMenu["debug"].Cast<CheckBox>().CurrentValue)
            {
                var target =
                    EntityManager.Heroes.Enemies.FirstOrDefault(
                        e =>
                        !e.IsZombie && !e.IsDead && !e.HasBuff("kindredrnodeathbuff")
                        && !e.HasBuff("JudicatorIntervention") && !e.HasBuff("ChronoShift") && !e.HasBuff("UndyingRage")
                        && e.IsHPBarRendered && e.IsEnemy);

                if (LissEMissile != null)
                {
                    Circle.Draw(Color.DarkBlue, W.Range, LissEMissile.Position);
                    Circle.Draw(Color.DarkBlue, W.Range, LissEMissile.EndPosition);
                }

                /*
                if (player != null)
                {
                    var hpPosp = player.HPBarPosition;
                    Circle.Draw(Color.DarkBlue, R.Range, player.Position);
                    Drawing.DrawText(
                        hpPosp.X + 135f,
                        hpPosp.Y,
                        System.Drawing.Color.White,
                        "Enemies in Range " + player.CountEnemiesInRange(R.Range),
                        10);
                }

                if (target != null)
                {
                    var hpPos = target.HPBarPosition;
                    Circle.Draw(Color.White, R.Range, target.Position);
                    Drawing.DrawText(
                        hpPos.X + 135f,
                        hpPos.Y,
                        System.Drawing.Color.White,
                        "Enemies in Range " + target.CountEnemiesInRange(R.Range).ToString(),
                        10);
                }
                */
                var enemies = EntityManager.Heroes.Enemies.Where(n => n.IsValidTarget(E2.Range));
                var aoePrediction =
                    Prediction.Position.PredictCircularMissileAoe(
                        enemies.ToArray(),
                        R.Range,
                        500,
                        R.CastDelay,
                        R.Handle.SData.MissileSpeed)
                        .OrderByDescending(r => r.GetCollisionObjects<AIHeroClient>().Length)
                        .FirstOrDefault();

                if (aoePrediction != null)
                {
                    Circle.Draw(Color.White, R.Range, aoePrediction.CastPosition);
                    Drawing.DrawText(
                        aoePrediction.CastPosition.X + 135f,
                        aoePrediction.CastPosition.Y,
                        System.Drawing.Color.White,
                        "Enemies in Range " + aoePrediction.CastPosition.CountEnemiesInRange(R.Range),
                        10);
                }
            }
        }
    }
}