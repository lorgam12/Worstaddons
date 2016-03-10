namespace KappaLissandra
{
    using System;
    using System.Collections.Generic;
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
        private static bool _eCreated;

        public static List<AIHeroClient> AoeTargetsHit = new List<AIHeroClient>();

        internal static int _aoeTargetsHitCount;

        public static bool jumping;

        private static Vector2 MissilePosition;

        private static MissileClient LissEMissile;

        private static Dictionary<String, Spell.SpellBase> Spells;

        public static Spell.Skillshot Q { get; set; }

        public static Spell.Skillshot Qtest { get; set; }

        public static Spell.Skillshot Q2 { get; set; }

        public static Spell.Active W { get; set; }

        public static Spell.Skillshot E { get; set; }

        public static Spell.Active E2 { get; set; }

        public static Spell.Targeted R { get; set; }

        public static AIHeroClient Player { get; set; }

        public static Menu ComboMenu { get; private set; }

        public static Menu UltMenu { get; private set; }

        public static Menu HarassMenu { get; private set; }

        public static Menu LaneMenu { get; private set; }

        public static Menu MiscMenu { get; private set; }

        public static Menu DrawMenu { get; private set; }

        private static Menu menuIni;

        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoad;
        }

        private static void OnLoad(EventArgs args)
        {
            menuIni = MainMenu.AddMenu("KappaLissandra", "KappaLissandra");
            menuIni.AddGroupLabel("Welcome to the Worst Lissandra addon!");
            menuIni.AddGroupLabel("Global Settings");
            menuIni.Add("Combo", new CheckBox("Use Combo?"));
            menuIni.Add("Harass", new CheckBox("Use Harass?"));
            menuIni.Add("LaneClear", new CheckBox("Use Lane Clear?"));
            menuIni.Add("JungleClear", new CheckBox("Use Jungle Clear?"));
            menuIni.Add("Drawings", new CheckBox("Use Drawings?"));

            UltMenu = menuIni.AddSubMenu("Ultimate");
            UltMenu.AddGroupLabel("Ultimate Settings");
            UltMenu.Add("aoeR", new CheckBox("AoE R Logic"));
            UltMenu.Add("RS", new CheckBox("Use R On Self"));
            UltMenu.Add("RA", new CheckBox("Use R On Ally"));
            UltMenu.Add("RE", new CheckBox("Use R On Enemy"));
            UltMenu.Add("hitR", new Slider("R AoE Hit >=", 2, 1, 5));
            UltMenu.Add("shp", new Slider("On Self Health to use R", 15, 0, 100));
            UltMenu.Add("ahp", new Slider("On Ally Health to use R", 10, 0, 100));

            ComboMenu = menuIni.AddSubMenu("Combo");
            ComboMenu.AddGroupLabel("Combo Settings");
            ComboMenu.Add("Q", new CheckBox("Use Q"));
            ComboMenu.Add("W", new CheckBox("Use W"));
            ComboMenu.Add("E", new CheckBox("Use E"));
            ComboMenu.Add("E2", new CheckBox("Use E Engage", false));

            HarassMenu = menuIni.AddSubMenu("Harass");
            HarassMenu.AddGroupLabel("Harass Settings");
            HarassMenu.Add("Q", new CheckBox("Use Q"));
            HarassMenu.Add("W", new CheckBox("Use W"));
            HarassMenu.Add("E", new CheckBox("Use E"));
            HarassMenu.Add("Mana", new Slider("Save Mana %", 30, 0, 100));

            LaneMenu = menuIni.AddSubMenu("Farm");
            LaneMenu.AddGroupLabel("LaneClear Settings");
            LaneMenu.Add("Q", new CheckBox("Use Q"));
            LaneMenu.Add("W", new CheckBox("Use W"));
            LaneMenu.Add("E", new CheckBox("Use E"));
            LaneMenu.Add("Mana", new Slider("Save Mana %", 30, 0, 100));
            LaneMenu.AddGroupLabel("JungleClear Settings");
            LaneMenu.Add("jQ", new CheckBox("Use Q"));
            LaneMenu.Add("jW", new CheckBox("Use W"));
            LaneMenu.Add("jE", new CheckBox("Use E"));

            MiscMenu = menuIni.AddSubMenu("Misc");
            MiscMenu.AddGroupLabel("Misc Settings");
            MiscMenu.Add("gapcloser", new CheckBox("Anti-GapCloser"));
            MiscMenu.Add("Interrupt", new CheckBox("Interrupt"));
            MiscMenu.Add("gapclosermana", new Slider("Anti-GapCloser Mana", 25, 0, 100));

            DrawMenu = menuIni.AddSubMenu("Drawings");
            DrawMenu.AddGroupLabel("Drawing Settings");
            DrawMenu.Add("Q", new CheckBox("Draw Q"));
            DrawMenu.Add("W", new CheckBox("Draw W"));
            DrawMenu.Add("E", new CheckBox("Draw E"));
            DrawMenu.Add("R", new CheckBox("Draw R"));

            Q = new Spell.Skillshot(SpellSlot.Q, 715, SkillShotType.Linear, 250, 2200, 75);
            Q2 = new Spell.Skillshot(SpellSlot.Q, 825, SkillShotType.Linear, 250, 2200, 90);
            Qtest = new Spell.Skillshot(SpellSlot.Q, 715, SkillShotType.Linear, 250, 2200, 75)
                        { AllowedCollisionCount = int.MaxValue };
            W = new Spell.Active(SpellSlot.W, 450);
            E = new Spell.Skillshot(SpellSlot.E, 1000, SkillShotType.Linear, 250, 850, 125);
            R = new Spell.Targeted(SpellSlot.R, 550);

            Game.OnUpdate += OnUpdate;
            GameObject.OnCreate += OnCreate;
            GameObject.OnDelete += OnDelete;
            Game.OnUpdate += MonitorMissilePosition;
            Drawing.OnDraw += OnDraw;
        }

        private static void OnUpdate(EventArgs args)
        {
            Player = ObjectManager.Player;

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
                if (Player.ManaPercent > LaneMenu["Mana"].Cast<Slider>().CurrentValue)
                {
                    Clear();
                }
            }

            if (flags.HasFlag(Orbwalker.ActiveModes.JungleClear) && menuIni.Get<CheckBox>("JungleClear").CurrentValue)
            {
                if (Player.ManaPercent > LaneMenu["Mana"].Cast<Slider>().CurrentValue)
                {
                    jClear();
                }
            }

            var ally =
                EntityManager.Heroes.Allies.FirstOrDefault(
                    a =>
                    !a.IsDead && !a.IsZombie && !a.IsGhosted && !a.HasBuff("kindredrnodeathbuff")
                    && !a.HasBuff("JudicatorIntervention") && !a.HasBuff("ChronoShift") && !a.HasBuff("UndyingRage"));
            var shp = UltMenu["shp"].Cast<Slider>().CurrentValue;
            var ahp = UltMenu["ahp"].Cast<Slider>().CurrentValue;
            var useRA = UltMenu["RA"].Cast<CheckBox>().CurrentValue && R.IsReady();
            var useRS = UltMenu["RS"].Cast<CheckBox>().CurrentValue && R.IsReady();

            if (useRS && Player.CountEnemiesInRange(750) >= 1 && Player.HealthPercent <= shp
                && (Player.MagicDamageTaken >= 50 || Player.PhysicalDamageTaken >= 50)
                && !Player.HasBuff("kindredrnodeathbuff") && !Player.HasBuff("JudicatorIntervention")
                && !Player.HasBuff("ChronoShift") && !Player.HasBuff("UndyingRage"))
            {
                R.Cast(Player);
            }

            if (useRA && ally.CountEnemiesInRange(R.Range) >= 1 && ally.IsValidTarget(R.Range)
                && ally.HealthPercent <= ahp && (ally.MagicDamageTaken >= 50 || ally.PhysicalDamageTaken >= 50))
            {
                R.Cast(ally);
            }
        }

        private static void MonitorMissilePosition(EventArgs args)
        {
            if (LissEMissile == null || Player.IsDead)
            {
                return;
            }

            MissilePosition = LissEMissile.Position.To2D();
            if (jumping)
            {
                if ((Vector2.Distance(MissilePosition, LissEMissile.EndPosition.To2D()) < 40))
                {
                    E.Cast(Player);
                    jumping = false;
                }
                Core.DelayAction(delegate { jumping = false; }, 2000);
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
                    _eCreated = true;
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
                MissilePosition = new Vector2(0, 0);
                _eCreated = false;
            }
        }

        private static void Combo()
        {
            Player = ObjectManager.Player;
            var useQ = ComboMenu["Q"].Cast<CheckBox>().CurrentValue && Q.IsReady();
            var useW = ComboMenu["W"].Cast<CheckBox>().CurrentValue && W.IsReady();
            var useE = ComboMenu["E"].Cast<CheckBox>().CurrentValue && E.IsReady();
            var useRS = UltMenu["RS"].Cast<CheckBox>().CurrentValue && R.IsReady();
            var useRE = UltMenu["RE"].Cast<CheckBox>().CurrentValue && R.IsReady();

            if (useQ)
            {
                CastQ();
            }

            if (useE)
            {
                CastE();
            }

            if (useW)
            {
                CastW();
            }

            if (useRS || useRE)
            {
                CastR();
            }
        }

        private static void Harass()
        {
            Player = ObjectManager.Player;
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
                        && (Vector3.Distance(h.ServerPosition, Player.ServerPosition) < E.Range * 0.94) && !h.IsZombie);
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
            Player = ObjectManager.Player;
            var useQ = LaneMenu["Q"].Cast<CheckBox>().CurrentValue;
            var useW = LaneMenu["W"].Cast<CheckBox>().CurrentValue;
            var useE = LaneMenu["E"].Cast<CheckBox>().CurrentValue;
        }

        private static void jClear()
        {
            Player = ObjectManager.Player;
            var useQ = LaneMenu["jQ"].Cast<CheckBox>().CurrentValue;
            var useW = LaneMenu["jW"].Cast<CheckBox>().CurrentValue;
            var useE = LaneMenu["jE"].Cast<CheckBox>().CurrentValue;
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
                    (Vector2)Player.ServerPosition,
                    Player.ServerPosition.Extend(minion.ServerPosition, Q2.Range),
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
            if (Vector3.Distance(target.ServerPosition, Player.ServerPosition) <= W.Range && target != null)
            {
                W.Cast();
                return;
            }

            if (
                EntityManager.Heroes.Enemies.Any(
                    h =>
                    h.IsValidTarget() && h != null
                    && (Vector3.Distance(h.ServerPosition, Player.ServerPosition) < W.Range) && !h.IsZombie))
            {
                W.Cast();
            }
        }

        private static void CastE()
        {
            if (!E.IsReady())
            {
                return;
            }

            var target = TargetSelector.GetTarget(E.Range + 100, DamageType.Magical);
            if (LissEMissile == null && !Player.HasBuff("LissandraE") && target != null)
            {
                var pred = E.GetPrediction(target);
                E.Cast(pred.CastPosition);
            }

            if (LissEMissile != null && LissEMissile.Position.CountEnemiesInRange(W.Range - 50) >= 1)
            {
                E.Cast(Game.CursorPos);
            }
        }

        private static void CastR()
        {
            var aoeR = UltMenu["aeoR"].Cast<CheckBox>().CurrentValue;
            var useRA = UltMenu["RA"].Cast<CheckBox>().CurrentValue && R.IsReady();
            var useRS = UltMenu["RS"].Cast<CheckBox>().CurrentValue && R.IsReady();
            var useRE = UltMenu["RE"].Cast<CheckBox>().CurrentValue && R.IsReady();
            var hitR = UltMenu["hitR"].Cast<Slider>().CurrentValue;
            var target =
                EntityManager.Heroes.Enemies.FirstOrDefault(
                    e =>
                    e.CountEnemiesInRange(R.Range - 5) >= 1 && !e.IsZombie && !e.IsDead
                    && !e.HasBuff("kindredrnodeathbuff") && !e.HasBuff("JudicatorIntervention")
                    && !e.HasBuff("ChronoShift") && !e.HasBuff("UndyingRage"));
            var ally = EntityManager.Heroes.Allies.FirstOrDefault(a => a.CountEnemiesInRange(R.Range - 5) >= 1);

            if (target != null && useRE)
            {
                if (aoeR && target.CountEnemiesInRange(R.Range) >= hitR && target.IsValidTarget(R.Range))
                {
                    R.Cast(target);
                }
            }

            if (useRA && ally != null)
            {
                if (aoeR && ally.CountEnemiesInRange(R.Range) >= hitR && ally.IsValidTarget(R.Range))
                {
                    R.Cast(ally);
                }
            }

            if (useRS)
            {
                if (aoeR && Player.CountEnemiesInRange(R.Range) >= hitR)
                {
                    R.Cast(Player);
                }
            }
        }

        private static void OnDraw(EventArgs args)
        {
            if (!menuIni.Get<CheckBox>("Drawings").CurrentValue)
            {
                return;
            }

            if (LissEMissile != null)
            {
                Circle.Draw(Color.MediumPurple, W.Range, LissEMissile.Position);
                Circle.Draw(Color.MediumPurple, W.Range, LissEMissile.EndPosition);
                Circle.Draw(Color.MediumPurple, W.Range, Player.Position);
            }
        }
    }
}