namespace KappaBrand
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

    internal static class Brand
    {
        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete1;
        }

        public static Menu MenuIni, TS, Auto, Combo, Harass, LaneClear, JungleClear, KillSteal, DrawMenu;

        public static ComboBox tsmode, tselect;

        public static Spell.Skillshot Q = new Spell.Skillshot(SpellSlot.Q, 1000, SkillShotType.Linear, 250, 1600, 120);

        public static Spell.Skillshot W = new Spell.Skillshot(SpellSlot.W, 900, SkillShotType.Circular, 650, -1, 200);

        public static Spell.Targeted E = new Spell.Targeted(SpellSlot.E, 630);

        public static Spell.Targeted R = new Spell.Targeted(SpellSlot.R, 750);

        private static void Loading_OnLoadingComplete1(EventArgs args)
        {
            MenuIni = MainMenu.AddMenu("Brand", "Brand");
            TS = MenuIni.AddSubMenu("TargetSelector");
            Auto = MenuIni.AddSubMenu("Auto");
            Combo = MenuIni.AddSubMenu("Combo");
            Harass = MenuIni.AddSubMenu("Harass");
            LaneClear = MenuIni.AddSubMenu("LaneClear");
            JungleClear = MenuIni.AddSubMenu("JungleClear");
            KillSteal = MenuIni.AddSubMenu("KillSteal");
            DrawMenu = MenuIni.AddSubMenu("Drawings");

            TS.AddGroupLabel("Target Selector");
            tsmode = TS.Add("tsmode", new ComboBox("TargetSelector", 0, "Custom TargetSelector", "Core TargetSelector"));
            tselect = TS.Add("select", new ComboBox("Focus Mode", 0, "Most Passive Stacks", "Less Cast Target", "Near Mouse"));
            if (tsmode.CurrentValue == 1)
            {
                tselect.IsVisible = false;
            }
            tsmode.OnValueChange += delegate { tselect.hide(tsmode); };

            Auto.AddGroupLabel("Auto Settings");
            Auto.Add("AutoR", new Slider("Auto R AoE hit [{0}] Targets or more", 2, 1, 6));
            Auto.Add("Gap", new CheckBox("Anti GapCloser"));
            Auto.Add("Int", new CheckBox("Auto Interrupter"));
            Auto.Add("Danger", new ComboBox("Interrupter Danger Level", 1, "High", "Medium", "Low"));
            Auto.AddSeparator(0);
            Auto.AddGroupLabel("Auto Hit Passive");
            Auto.Add("AutoQ", new CheckBox("Auto Q Dotnate Passive"));
            Auto.Add("AutoW", new CheckBox("Auto W Dotnate Passive", false));
            Auto.Add("AutoE", new CheckBox("Auto E Dotnate Passive"));
            Auto.AddSeparator(0);
            Auto.AddGroupLabel("Anti GapCloser - Spells");
            foreach (var enemy in EntityManager.Heroes.Enemies)
            {
                foreach (var gapspell in Gapcloser.GapCloserList.Where(e => e.ChampName == enemy.ChampionName))
                {
                    Auto.AddLabel(gapspell.ChampName);
                    Auto.Add(gapspell.SpellName, new CheckBox(gapspell.SpellName + " - " + gapspell.SpellSlot));
                }
            }

            Combo.AddGroupLabel("Combo Settings");
            Combo.Add("Q", new CheckBox("Use Q"));
            Combo.AddLabel("Extra Q Settings");
            Combo.Add("Qp", new CheckBox("Q Only for stun"));
            Combo.Add(Q.Slot + "Mana", new Slider("Use Q if Mana% is more than [{0}%]", 10));
            Combo.AddSeparator(1);

            Combo.Add("W", new CheckBox("Use W"));
            Combo.AddLabel("Extra W Settings");
            Combo.Add("Wp", new CheckBox("W Only if target has brand passive", false));
            Combo.Add(W.Slot + "Mana", new Slider("Use W if Mana% is more than [{0}%]", 5));
            Combo.AddSeparator(1);

            Combo.Add("E", new CheckBox("Use E"));
            Combo.AddLabel("Extra E Settings");
            Combo.Add("Ep", new CheckBox("E Only if target has brand passive", false));
            Combo.Add(E.Slot + "Mana", new Slider("Use E if Mana% is more than [{0}%]", 15));
            Combo.AddSeparator(1);

            Combo.Add("RFinisher", new CheckBox("Use R Finisher"));
            Combo.Add("RAoe", new CheckBox("Use R Aoe"));
            Combo.Add("Rhit", new Slider("R AoE hit [{0}] Targets or more", 2, 1, 6));
            Combo.Add(R.Slot + "Mana", new Slider("Use R if Mana% is more than [{0}%]"));

            Harass.AddGroupLabel("Harass");
            Harass.Add("Q", new CheckBox("Use Q"));
            Harass.Add(Q.Slot + "Mana", new Slider("Use Q if Mana% is more than [{0}%]", 65));
            Harass.AddSeparator(1);

            Harass.Add("W", new CheckBox("Use W"));
            Harass.Add(W.Slot + "Mana", new Slider("Use W if Mana% is more than [{0}%]", 65));
            Harass.AddSeparator(1);

            Harass.Add("E", new CheckBox("Use E"));
            Harass.Add(E.Slot + "Mana", new Slider("Use E if Mana% is more than [{0}%]", 65));

            LaneClear.AddGroupLabel("LaneClear");
            LaneClear.Add("Q", new CheckBox("Use Q"));
            LaneClear.Add(Q.Slot + "Mana", new Slider("Use Q if Mana% is more than [{0}%]", 65));
            LaneClear.AddSeparator(1);
            LaneClear.Add("W", new CheckBox("Use W"));
            LaneClear.Add(W.Slot + "Mana", new Slider("Use W if Mana% is more than [{0}%]", 65));
            LaneClear.AddSeparator(1);
            LaneClear.Add("E", new CheckBox("Use E"));
            LaneClear.Add(E.Slot + "Mana", new Slider("Use E if Mana% is more than [{0}%]", 65));

            JungleClear.AddGroupLabel("JungleClear");
            JungleClear.Add("Q", new CheckBox("Use Q"));
            JungleClear.Add(Q.Slot + "Mana", new Slider("Use Q if Mana% is more than [{0}%]", 65));
            JungleClear.AddSeparator(1);
            JungleClear.Add("W", new CheckBox("Use W"));
            JungleClear.Add(W.Slot + "Mana", new Slider("Use W if Mana% is more than [{0}%]", 65));
            JungleClear.AddSeparator(1);
            JungleClear.Add("E", new CheckBox("Use E"));
            JungleClear.Add(E.Slot + "Mana", new Slider("Use E if Mana% is more than [{0}%]", 65));

            KillSteal.AddGroupLabel("KillSteal");
            KillSteal.Add("Q", new CheckBox("Use Q"));
            KillSteal.Add("W", new CheckBox("Use W"));
            KillSteal.Add("E", new CheckBox("Use E"));
            KillSteal.Add("R", new CheckBox("Use R", false));

            DrawMenu.AddGroupLabel("Drawings");
            DrawMenu.Add("damage", new CheckBox("Draw Combo Damage"));
            DrawMenu.AddLabel("Draws = ComboDamage / Enemy Current Health");
            DrawMenu.AddSeparator(1);
            DrawMenu.Add("Q", new CheckBox("Draw Q Range"));
            DrawMenu.Add(Q.Name, new ComboBox("Q Color", 0, "Chartreuse", "BlueViolet", "Aqua", "Purple", "White", "Orange", "Green"));
            DrawMenu.AddSeparator(1);
            DrawMenu.Add("W", new CheckBox("Draw W Range"));
            DrawMenu.Add(W.Name, new ComboBox("W Color", 0, "Chartreuse", "BlueViolet", "Aqua", "Purple", "White", "Orange", "Green"));
            DrawMenu.AddSeparator(1);
            DrawMenu.Add("E", new CheckBox("Draw E Range"));
            DrawMenu.Add(E.Name, new ComboBox("E Color", 0, "Chartreuse", "BlueViolet", "Aqua", "Purple", "White", "Orange", "Green"));
            DrawMenu.AddSeparator(1);
            DrawMenu.Add("R", new CheckBox("Draw R Range"));
            DrawMenu.Add(R.Name, new ComboBox("R Color", 0, "Chartreuse", "BlueViolet", "ChartAquareuse", "Purple", "White", "Orange", "Green"));
            DrawMenu.AddSeparator(1);

            Game.OnTick += Game_OnTick;
            Drawing.OnDraw += Drawing_OnDraw;
            Gapcloser.OnGapcloser += Gapcloser_OnGapcloser;
            Interrupter.OnInterruptableSpell += Interrupter_OnInterruptableSpell;
        }

        private static void Game_OnTick(EventArgs args)
        {
            if (Common.orbmode(Orbwalker.ActiveModes.Combo))
            {
                ComboLogic();
            }

            if (Common.orbmode(Orbwalker.ActiveModes.Harass))
            {
                HarassLogic();
            }

            if (Common.orbmode(Orbwalker.ActiveModes.LaneClear))
            {
                LaneClearLogic();
            }

            if (Common.orbmode(Orbwalker.ActiveModes.JungleClear))
            {
                JungleClearLogic();
            }

            Automated();
            KillStealLogic();
        }

        public static void ComboLogic()
        {
            var target = KappaSelector.SelectTarget(Q.Range + 100);
            if (target == null)
            {
                return;
            }

            var Qready = Combo["Q"].Cast<CheckBox>().CurrentValue && Q.IsReady() && target.IsValidTarget(Q.Range) && Q.Mana(Combo)
                         && (Q.GetPrediction(target).HitChance >= HitChance.High || target.IsCC()) && Common.Qmana;
            var Wready = Combo["W"].Cast<CheckBox>().CurrentValue && W.IsReady() && target.IsValidTarget(W.Range)
                         && (W.GetPrediction(target).HitChance >= HitChance.High || target.IsCC()) && Common.Wmana && W.Mana(Combo);
            var Eready = Combo["E"].Cast<CheckBox>().CurrentValue && E.IsReady() && target.IsValidTarget(E.Range) && Common.Emana && E.Mana(Combo);
            var RFinisher = Combo["RFinisher"].Cast<CheckBox>().CurrentValue && R.IsReady() && target.IsValidTarget(R.Range) && Common.Rmana
                            && R.Mana(Combo);
            var RAoe = Combo["RAoe"].Cast<CheckBox>().CurrentValue && R.IsReady() && target.IsValidTarget(R.Range) && Common.Rmana && R.Mana(Combo);

            if (Qready)
            {
                Qlogic(target);
            }
            if (Wready)
            {
                Wlogic(target);
            }
            if (Eready)
            {
                Elogic(target);
            }
            if (RFinisher)
            {
                Rlogic(target);
            }
            if (RAoe)
            {
                Rlogic(target);
            }
        }

        public static void HarassLogic()
        {
            var target = KappaSelector.SelectTarget(Q.Range + 100);
            if (target == null)
            {
                return;
            }

            var Qready = Combo["Q"].Cast<CheckBox>().CurrentValue && Q.IsReady() && target.IsValidTarget(Q.Range) && Q.Mana(Harass)
                         && (Q.GetPrediction(target).HitChance >= HitChance.High || target.IsCC()) && Common.Qmana;
            var Wready = Combo["W"].Cast<CheckBox>().CurrentValue && W.IsReady() && target.IsValidTarget(W.Range)
                         && (W.GetPrediction(target).HitChance >= HitChance.High || target.IsCC()) && Common.Wmana && W.Mana(Harass);
            var Eready = Combo["E"].Cast<CheckBox>().CurrentValue && E.IsReady() && target.IsValidTarget(E.Range) && Common.Emana && E.Mana(Harass);

            if (Qready)
            {
                Qlogic(target);
            }
            if (Wready)
            {
                Wlogic(target);
            }
            if (Eready)
            {
                Elogic(target);
            }
        }

        public static void LaneClearLogic()
        {
            var target = EntityManager.MinionsAndMonsters.EnemyMinions.FirstOrDefault(m => m.IsValidTarget(Q.Range + 50));

            if (target == null)
            {
                return;
            }

            var Qready = Combo["Q"].Cast<CheckBox>().CurrentValue && Q.IsReady() && target.IsValidTarget(Q.Range) && Q.Mana(LaneClear)
                         && (Q.GetPrediction(target).HitChance >= HitChance.High || target.IsCC()) && Common.Qmana;
            var Wready = Combo["W"].Cast<CheckBox>().CurrentValue && W.IsReady() && target.IsValidTarget(W.Range)
                         && (W.GetPrediction(target).HitChance >= HitChance.High || target.IsCC()) && Common.Wmana && W.Mana(LaneClear);
            var Eready = Combo["E"].Cast<CheckBox>().CurrentValue && E.IsReady() && target.IsValidTarget(E.Range) && Common.Emana && E.Mana(LaneClear);

            if (Qready)
            {
                Qlogic(target);
            }
            if (Wready)
            {
                Wlogic(target);
            }
            if (Eready)
            {
                Elogic(target);
            }
        }

        public static void JungleClearLogic()
        {
            var target = EntityManager.MinionsAndMonsters.GetJungleMonsters().FirstOrDefault(m => m.IsValidTarget(Q.Range + 50));

            if (target == null)
            {
                return;
            }

            var Qready = Combo["Q"].Cast<CheckBox>().CurrentValue && Q.IsReady() && target.IsValidTarget(Q.Range) && Q.Mana(LaneClear)
                         && (Q.GetPrediction(target).HitChance >= HitChance.High || target.IsCC()) && Common.Qmana;
            var Wready = Combo["W"].Cast<CheckBox>().CurrentValue && W.IsReady() && target.IsValidTarget(W.Range)
                         && (W.GetPrediction(target).HitChance >= HitChance.High || target.IsCC()) && Common.Wmana && W.Mana(LaneClear);
            var Eready = Combo["E"].Cast<CheckBox>().CurrentValue && E.IsReady() && target.IsValidTarget(E.Range) && Common.Emana && E.Mana(LaneClear);

            if (Qready)
            {
                Qlogic(target);
            }
            if (Wready)
            {
                Wlogic(target);
            }
            if (Eready)
            {
                Elogic(target);
            }
        }

        public static void KillStealLogic()
        {
            var targets = EntityManager.Heroes.Enemies.Where(e => e.IsValidTarget() && e.IsKillable());

            if (KillSteal.checkbox("Q") && Q.IsReady())
            {
                var kstarget =
                    targets.FirstOrDefault(e => e.IsValidTarget(Q.Range) && Q.GetDamage(e) >= Prediction.Health.GetPrediction(e, Q.CastDelay));

                var Qready = kstarget != null && (Q.GetPrediction(kstarget).HitChance >= HitChance.High || kstarget.IsCC()) && Common.Qmana;
                if (Qready)
                {
                    Q.Cast(kstarget);
                }
            }

            if (KillSteal.checkbox("W") && W.IsReady())
            {
                var kstarget =
                    targets.FirstOrDefault(e => e.IsValidTarget(W.Range) && W.GetDamage(e) >= Prediction.Health.GetPrediction(e, W.CastDelay));

                if (Q.GetDamage(kstarget) >= Prediction.Health.GetPrediction(kstarget, Q.CastDelay))
                {
                    return;
                }

                var Wready = kstarget != null && (W.GetPrediction(kstarget).HitChance >= HitChance.High || kstarget.IsCC()) && Common.Wmana;
                if (Wready)
                {
                    W.Cast(kstarget);
                }
            }

            if (KillSteal.checkbox("E") && E.IsReady())
            {
                var kstarget =
                    targets.FirstOrDefault(e => e.IsValidTarget(E.Range) && E.GetDamage(e) >= Prediction.Health.GetPrediction(e, E.CastDelay));

                if (Q.GetDamage(kstarget) >= Prediction.Health.GetPrediction(kstarget, Q.CastDelay))
                {
                    return;
                }
                if (W.GetDamage(kstarget) >= Prediction.Health.GetPrediction(kstarget, W.CastDelay))
                {
                    return;
                }

                var Eready = kstarget != null && Common.Emana;
                if (Eready)
                {
                    E.Cast(kstarget);
                }
            }

            if (KillSteal.checkbox("R") && R.IsReady())
            {
                var kstarget =
                    targets.FirstOrDefault(e => e.IsValidTarget(R.Range) && R.GetDamage(e) >= Prediction.Health.GetPrediction(e, R.CastDelay));

                if (Q.GetDamage(kstarget) >= Prediction.Health.GetPrediction(kstarget, Q.CastDelay))
                {
                    return;
                }
                if (W.GetDamage(kstarget) >= Prediction.Health.GetPrediction(kstarget, W.CastDelay))
                {
                    return;
                }
                if (E.GetDamage(kstarget) >= Prediction.Health.GetPrediction(kstarget, E.CastDelay))
                {
                    return;
                }

                var Rready = kstarget != null && Common.Rmana;
                if (Rready)
                {
                    R.Cast(kstarget);
                }
            }
        }

        public static void Automated()
        {
            var targets = EntityManager.Heroes.Enemies.Where(e => e.countpassive() >= 2 && e.IsValidTarget() && e.IsKillable());

            if (targets != null)
            {
                if (Auto.checkbox("AutoQ"))
                {
                    var target = targets.FirstOrDefault(e => e.IsValidTarget(Q.Range));
                    if (target != null)
                    {
                        Q.Cast(target);
                    }
                }
                if (Auto.checkbox("AutoW"))
                {
                    var target = targets.FirstOrDefault(e => e.IsValidTarget(W.Range));
                    if (target != null)
                    {
                        W.Cast(target);
                    }
                }
                if (Auto.checkbox("AutoE"))
                {
                    var target = targets.FirstOrDefault(e => e.IsValidTarget(E.Range));
                    if (target != null)
                    {
                        E.Cast(target);
                    }
                }
            }

            var hits = Auto.slider("AutoR");
            var enemies = EntityManager.Heroes.Enemies.Where(e => e.IsValidTarget(R.Range) && e.IsKillable());
            var aoetarget = enemies.OrderByDescending(e => Common.CountEnemeis(400, e)).FirstOrDefault(e => Common.CountEnemeis(400, e) >= hits);

            if (aoetarget != null)
            {
                R.Cast(aoetarget);
            }
        }

        public static void Qlogic(Obj_AI_Base target)
        {
            if (target == null)
            {
                return;
            }
            var Combomode = Common.orbmode(Orbwalker.ActiveModes.Combo);
            var Harassmode = Common.orbmode(Orbwalker.ActiveModes.Harass);
            var LaneClearmode = Common.orbmode(Orbwalker.ActiveModes.LaneClear);
            var JungleClearmode = Common.orbmode(Orbwalker.ActiveModes.JungleClear);

            if (Combomode)
            {
                if (Combo.checkbox("Qp"))
                {
                    if (target.brandpassive())
                    {
                        Q.Cast(target);
                    }
                }
                else
                {
                    Q.Cast(target);
                }
            }

            if (Harassmode)
            {
                Q.Cast(target);
            }

            if (LaneClearmode)
            {
                var minion =
                    EntityManager.MinionsAndMonsters.EnemyMinions.FirstOrDefault(
                        m => Q.GetDamage(m) >= Prediction.Health.GetPrediction(m, Q.CastDelay) && Q.GetPrediction(m).HitChance >= HitChance.High);

                if (minion != null)
                {
                    Q.Cast(minion);
                }
            }

            if (JungleClearmode)
            {
                var minion =
                    EntityManager.MinionsAndMonsters.GetJungleMonsters()
                        .OrderByDescending(m => m.MaxHealth)
                        .FirstOrDefault(m => Q.GetPrediction(m).HitChance >= HitChance.High);

                if (minion != null)
                {
                    Q.Cast(minion);
                }
            }
        }

        public static void Wlogic(Obj_AI_Base target)
        {
            if (target == null)
            {
                return;
            }
            var Combomode = Common.orbmode(Orbwalker.ActiveModes.Combo);
            var Harassmode = Common.orbmode(Orbwalker.ActiveModes.Harass);
            var LaneClearmode = Common.orbmode(Orbwalker.ActiveModes.LaneClear);
            var JungleClearmode = Common.orbmode(Orbwalker.ActiveModes.JungleClear);

            var enemies = EntityManager.Heroes.Enemies.Where(e => e.IsValidTarget(W.Range) && e.IsKillable());
            var pred = Prediction.Position.PredictCircularMissileAoe(
                enemies.Cast<Obj_AI_Base>().ToArray(),
                W.Range,
                W.Width + 50,
                W.CastDelay,
                W.Speed);
            var castpos =
                pred.OrderByDescending(p => p.GetCollisionObjects<AIHeroClient>().Length).FirstOrDefault(p => p.CollisionObjects.Contains(target));
            if (Combomode)
            {
                if (Combo.checkbox("Wp"))
                {
                    if (castpos != null && castpos.CollisionObjects.Length > 1)
                    {
                        W.Cast(castpos.CastPosition);
                    }
                    if (target.brandpassive())
                    {
                        W.Cast(target);
                    }
                    if (target.IsCC())
                    {
                        W.Cast(target);
                    }
                }
                else
                {
                    W.Cast(target);
                }
            }

            if (Harassmode)
            {
                if (castpos != null && castpos.CollisionObjects.Length > 1)
                {
                    W.Cast(castpos.CastPosition);
                }
                W.Cast(target);
            }

            if (LaneClearmode)
            {
                var minions = EntityManager.MinionsAndMonsters.EnemyMinions.Where(e => e.IsValidTarget(W.Range) && e.IsKillable());
                var loc = EntityManager.MinionsAndMonsters.GetCircularFarmLocation(minions.ToArray(), W.Width, (int)W.Range, W.CastDelay, W.Speed);
                var farmpos = loc.CastPosition;

                if (farmpos != null && loc.HitNumber >= 2)
                {
                    W.Cast(farmpos);
                }
            }

            if (JungleClearmode)
            {
                var minions =
                    EntityManager.MinionsAndMonsters.GetJungleMonsters()
                        .OrderByDescending(m => m.MaxHealth)
                        .Where(e => e.IsValidTarget(W.Range) && e.IsKillable());
                var loc = EntityManager.MinionsAndMonsters.GetCircularFarmLocation(minions.ToArray(), W.Width, (int)W.Range, W.CastDelay, W.Speed);
                var farmpos = loc.CastPosition;

                if (farmpos != null)
                {
                    W.Cast(farmpos);
                }
            }
        }

        public static void Elogic(Obj_AI_Base target)
        {
            if (target == null)
            {
                return;
            }

            var Combomode = Common.orbmode(Orbwalker.ActiveModes.Combo);
            var Harassmode = Common.orbmode(Orbwalker.ActiveModes.Harass);
            var LaneClearmode = Common.orbmode(Orbwalker.ActiveModes.LaneClear);
            var JungleClearmode = Common.orbmode(Orbwalker.ActiveModes.JungleClear);

            if (Combomode)
            {
                if (Combo.checkbox("Ep") && target.brandpassive())
                {
                    E.Cast(target);
                }
                else
                {
                    E.Cast(target);
                }
            }

            if (Harassmode)
            {
                E.Cast(target);
            }

            if (LaneClearmode)
            {
                var minions = EntityManager.MinionsAndMonsters.EnemyMinions.Where(e => e.IsValidTarget(W.Range) && e.IsKillable() && e.brandpassive());
                var pred = Prediction.Position.PredictCircularMissileAoe(minions.Cast<Obj_AI_Base>().ToArray(), E.Range, 325, E.CastDelay, 1000);
                var order = pred.OrderByDescending(p => p.GetCollisionObjects<Obj_AI_Minion>().Length).FirstOrDefault();
                var castpos = order?.CollisionObjects.FirstOrDefault(m => m.IsValidTarget(E.Range));

                if (castpos != null && order.CollisionObjects.Length >= 2)
                {
                    E.Cast(castpos);
                }
            }

            if (JungleClearmode)
            {
                var minions =
                    EntityManager.MinionsAndMonsters.GetJungleMonsters().Where(e => e.IsValidTarget(W.Range) && e.IsKillable() && e.brandpassive());
                var pred = Prediction.Position.PredictCircularMissileAoe(minions.Cast<Obj_AI_Base>().ToArray(), E.Range, 325, E.CastDelay, 1000);
                var firstOrDefault = pred.OrderByDescending(p => p.GetCollisionObjects<Obj_AI_Minion>().Length).FirstOrDefault();
                var castpos = firstOrDefault?.CollisionObjects.FirstOrDefault(m => m.IsValidTarget(E.Range));

                if (castpos != null)
                {
                    E.Cast(castpos);
                }
            }
        }

        public static void Rlogic(Obj_AI_Base target)
        {
            if (target == null)
            {
                return;
            }

            var Combomode = Common.orbmode(Orbwalker.ActiveModes.Combo);
            var hits = Combo.slider("Rhit");

            if (Combomode)
            {
                if (Combo.checkbox("RFinisher"))
                {
                    var pred = R.GetDamage(target) >= Prediction.Health.GetPrediction(target, Q.CastDelay);
                    var health = R.GetDamage(target) >= target.TotalShieldHealth();
                    if (pred || health)
                    {
                        R.Cast(target);
                    }
                }
                if (Combo.checkbox("RAoe"))
                {
                    var AoeHit = Common.CountEnemeis(400, target) >= hits;
                    var bestaoe =
                        EntityManager.Heroes.Enemies.OrderByDescending(e => Common.CountEnemeis(400, e))
                            .FirstOrDefault(e => e.IsValidTarget(R.Range) && e.IsKillable() && Common.CountEnemeis(400, e) >= hits);
                    if (AoeHit)
                    {
                        R.Cast(target);
                    }
                    else
                    {
                        if (bestaoe != null)
                        {
                            R.Cast(bestaoe);
                        }
                    }
                }
            }
        }

        private static void Interrupter_OnInterruptableSpell(Obj_AI_Base sender, Interrupter.InterruptableSpellEventArgs e)
        {
            if (!sender.IsEnemy || !Auto.checkbox("Int") || sender == null || e == null)
            {
                return;
            }

            if (e.DangerLevel >= Common.danger() && sender.IsValidTarget(Q.Range))
            {
                if (sender.brandpassive())
                {
                    if (Q.IsReady())
                    {
                        Q.Cast(sender);
                    }
                }
                else
                {
                    if (E.IsReady() && Q.IsReady() && Common.Qmana && Common.Emana)
                    {
                        if (E.Cast(sender))
                        {
                            if (sender.brandpassive())
                            {
                                Q.Cast(sender);
                            }
                        }
                    }
                }
            }
        }

        private static void Gapcloser_OnGapcloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs e)
        {
            if (!sender.IsEnemy || !Auto.checkbox("Gap") || sender == null || e == null || e.End == Vector3.Zero
                || !e.End.IsInRange(Player.Instance, Q.Range))
            {
                return;
            }

            if (Auto.checkbox(e.SpellName) && sender.IsValidTarget(Q.Range))
            {
                if (sender.brandpassive())
                {
                    if (Q.IsReady())
                    {
                        Q.Cast(sender);
                    }
                }
                else
                {
                    if (E.IsReady() && Q.IsReady() && Common.Qmana && Common.Emana)
                    {
                        if (E.Cast(sender))
                        {
                            if (sender.brandpassive())
                            {
                                Q.Cast(sender);
                            }
                        }
                    }
                }
            }
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            var pos = Player.Instance.ServerPosition;

            if (DrawMenu["Q"].Cast<CheckBox>().CurrentValue)
            {
                Circle.Draw(Q.IsReady() ? Colors.Select(Q) : Color.Red, Q.Range, pos);
            }

            if (DrawMenu["W"].Cast<CheckBox>().CurrentValue)
            {
                Circle.Draw(W.IsReady() ? Colors.Select(W) : Color.Red, W.Range, pos);
            }

            if (DrawMenu["E"].Cast<CheckBox>().CurrentValue)
            {
                Circle.Draw(E.IsReady() ? Colors.Select(E) : Color.Red, E.Range, pos);
            }

            if (DrawMenu["R"].Cast<CheckBox>().CurrentValue)
            {
                Circle.Draw(R.IsReady() ? Colors.Select(R) : Color.Red, R.Range, pos);
            }

            if (DrawMenu["damage"].Cast<CheckBox>().CurrentValue)
            {
                foreach (var enemy in EntityManager.Heroes.Enemies.Where(e => e.IsHPBarRendered))
                {
                    if (enemy != null)
                    {
                        var hpx = enemy.HPBarPosition.X;
                        var hpy = enemy.HPBarPosition.Y;
                        var damage = (int)Damagelib.GetDamage(enemy) + " / " + (int)enemy.TotalShieldHealth();
                        var c = System.Drawing.Color.GreenYellow;

                        if (Damagelib.GetDamage(enemy) >= enemy.TotalShieldHealth() / 2)
                        {
                            damage = "Harass for Kill: " + (int)Damagelib.GetDamage(enemy) + "/" + (int)enemy.TotalShieldHealth();
                            c = System.Drawing.Color.Orange;
                        }

                        if (Damagelib.GetDamage(enemy) >= enemy.TotalShieldHealth())
                        {
                            damage = "Killable: " + (int)Damagelib.GetDamage(enemy) + "/" + (int)enemy.TotalShieldHealth();
                            c = System.Drawing.Color.Red;
                        }

                        Drawing.DrawText(hpx + 145, hpy, c, damage, 3);
                    }
                }
            }
        }
    }
}