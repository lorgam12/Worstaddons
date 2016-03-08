namespace AurelionSol
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
    
    internal class Program
    {
        private static AIHeroClient player = ObjectManager.Player;
        
        public static Spell.Skillshot Q { get; private set; }
        
        public static Spell.Active W { get; private set; }
        
        public static Spell.Skillshot R { get; private set; }
        
        public static Menu ComboMenu { get; private set; }
        
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
            Q = new Spell.Skillshot(SpellSlot.Q, 875, SkillShotType.Circular, 1000, int.MaxValue, 160);
            W = new Spell.Active(SpellSlot.W, 1000);
            R = new Spell.Skillshot(SpellSlot.R, 1300, SkillShotType.Linear, 250, 1300, 115);

            menuIni = MainMenu.AddMenu("Karthus", "Karthus");
            menuIni.AddGroupLabel("Welcome to the Worst Karthus addon!");
            menuIni.AddGroupLabel("Global Settings");
            menuIni.Add("Combo", new CheckBox("Use Combo?"));
            menuIni.Add("Harass", new CheckBox("Use Harass?"));
            menuIni.Add("Clear", new CheckBox("Use Lane Clear?"));
            menuIni.Add("Drawings", new CheckBox("Use Drawings?"));

            ComboMenu = menuIni.AddSubMenu("Combo");
            ComboMenu.AddGroupLabel("Combo Settings");
            ComboMenu.Add("Q", new CheckBox("Use Q"));
            ComboMenu.Add("W", new CheckBox("Use W"));
            ComboMenu.Add("R", new CheckBox("Use R"));
            ComboMenu.Add("Rhit", new Slider("Use R Hit", 2, 1, 5));

            HarassMenu = menuIni.AddSubMenu("Harass");
            HarassMenu.AddGroupLabel("Harass Settings");
            HarassMenu.Add("Q", new CheckBox("Use Q"));
            HarassMenu.Add("W", new CheckBox("Use W"));
            HarassMenu.Add("Mana", new Slider("Save Mana %", 30, 0, 100));

            LaneMenu = menuIni.AddSubMenu("Farm");
            LaneMenu.AddGroupLabel("LaneClear Settings");
            LaneMenu.Add("Q", new CheckBox("Use Q"));
            LaneMenu.Add("W", new CheckBox("Use W"));
            LaneMenu.Add("Mana", new Slider("Save Mana %", 30, 0, 100));

            MiscMenu = menuIni.AddSubMenu("Misc");
            MiscMenu.AddGroupLabel("Misc Settings");
            MiscMenu.Add("gapcloser", new CheckBox("Anti-GapCloser"));
            MiscMenu.Add("gapclosermana", new Slider("Anti-GapCloser Mana", 25, 0, 100));

            DrawMenu = menuIni.AddSubMenu("Drawings");
            DrawMenu.AddGroupLabel("Drawing Settings");
            DrawMenu.Add("Q", new CheckBox("Draw Q"));
            DrawMenu.Add("W", new CheckBox("Draw W"));
            DrawMenu.Add("E", new CheckBox("Draw E"));
            DrawMenu.Add("R", new CheckBox("Draw R"));

            Game.OnUpdate += OnUpdate;
            Drawing.OnDraw += OnDraw;
            Gapcloser.OnGapcloser += Gapcloser_OnGap;
        }
        
        private static void OnUpdate(EventArgs args)
        {
            var flags = Orbwalker.ActiveModesFlags;
            if (flags.HasFlag(Orbwalker.ActiveModes.Combo) && menuIni.Get<CheckBox>("Combo").CurrentValue)
            {
                Combo();
            }

            if (flags.HasFlag(Orbwalker.ActiveModes.Harass) && menuIni.Get<CheckBox>("Harass").CurrentValue)
            {
                Harass();
            }

            if (flags.HasFlag(Orbwalker.ActiveModes.LaneClear) && menuIni.Get<CheckBox>("Clear").CurrentValue)
            {
                Clear();
            }
        }
        
        private static void Gapcloser_OnGap(AIHeroClient Sender, Gapcloser.GapcloserEventArgs args)
        {
            if (!menuIni.Get<CheckBox>("Misc").CurrentValue || !MiscMenu.Get<CheckBox>("gapcloser").CurrentValue
                || ObjectManager.Player.ManaPercent < MiscMenu.Get<Slider>("gapclosermana").CurrentValue
                || Sender == null)
            {
                return;
            }

            var pred = Q.GetPrediction(Sender);
            if (Sender.IsValidTarget(W.Range) && W.IsReady() && !Sender.IsAlly && !Sender.IsMe)
            {
                Q.Cast(pred.CastPosition);
            }
        }
        
        private static void OnDraw(EventArgs args)
        {
            if (!player.IsDead && menuIni.Get<CheckBox>("Drawings").CurrentValue)
            {
                if (DrawMenu.Get<CheckBox>("Q").CurrentValue)
                {
                    Circle.Draw(Color.DarkRed, Q.Range, Player.Instance.Position);
                }

                if (DrawMenu.Get<CheckBox>("W").CurrentValue)
                {
                    Circle.Draw(Color.DarkRed, W.Range, Player.Instance.Position);
                }

                if (DrawMenu.Get<CheckBox>("E").CurrentValue)
                {
                    Circle.Draw(Color.DarkRed, 5000, Player.Instance.Position);
                }

                if (DrawMenu.Get<CheckBox>("R").CurrentValue)
                {
                    Circle.Draw(Color.DarkRed, R.Range, Player.Instance.Position);
                }
            }
        }
        
        private static void Combo()
        {
            var useQ = ComboMenu["Q"].Cast<CheckBox>().CurrentValue && Q.IsReady();
            var useW = ComboMenu["W"].Cast<CheckBox>().CurrentValue && W.IsReady();
            var useR = ComboMenu["R"].Cast<CheckBox>().CurrentValue && R.IsReady();
            var Rhit = ComboMenu["Rhit"].Cast<Slider>().CurrentValue;
            var Qtarget = TargetSelector.GetTarget(Q.Range, DamageType.Magical);
            var Wtarget = TargetSelector.GetTarget(W.Range, DamageType.Magical);
            var Rtarget = TargetSelector.GetTarget(R.Range, DamageType.Magical);

            if (useQ && Qtarget != null && Qtarget.IsValidTarget(Q.Range))
            {
                var pred = Q.GetPrediction(Qtarget);
                if (pred.HitChance >= HitChance.High)
                {
                    Q.Cast(pred.CastPosition);
                }
            }

            if (useW && Wtarget != null && Wtarget.IsValidTarget(W.Range))
            {
                W.Cast();
            }

            if (useR && Rtarget != null && Rtarget.IsValidTarget(R.Range))
            {
                // credits centilmen
                foreach (AIHeroClient enemie in EntityManager.Heroes.Enemies)
                {
                    if (Rhit > 0
                        && EntityManager.Heroes.Enemies.Where(
                            enemy => enemy != player && enemy.Distance(player) <= 1200).Count() >= Rhit
                        && !enemie.IsDead && !enemie.IsZombie)
                    {
                        R.Cast(enemie);
                    }
                }
            }
        }
        
        private static void Harass()
        {
            var useQ = HarassMenu["Q"].Cast<CheckBox>().CurrentValue && Q.IsReady();
            var useW = HarassMenu["W"].Cast<CheckBox>().CurrentValue && W.IsReady();
            var Qtarget = TargetSelector.GetTarget(Q.Range, DamageType.Magical);
            var Wtarget = TargetSelector.GetTarget(W.Range, DamageType.Magical);

            if (useQ && Qtarget != null && Qtarget.IsValidTarget(Q.Range))
            {
                var pred = Q.GetPrediction(Qtarget);
                Q.Cast(pred.CastPosition);
            }

            if (useW && Wtarget != null && Wtarget.IsValidTarget(W.Range))
            {
                W.Cast();
            }
        }
        
        private static void Clear()
        {
            var useQ = LaneMenu["Q"].Cast<CheckBox>().CurrentValue && Q.IsReady();
            var useW = LaneMenu["W"].Cast<CheckBox>().CurrentValue && W.IsReady();

            if (useQ)
            {
                // Credits stefsot
                var minions = EntityManager.MinionsAndMonsters.GetLaneMinions(
                    EntityManager.UnitTeam.Enemy, 
                    Player.Instance.Position, 
                    1500, 
                    false);

                var predictResult =
                    Prediction.Position.PredictCircularMissileAoe(
                        minions.Cast<Obj_AI_Base>().ToArray(), 
                        Q.Range, 
                        Q.Radius, 
                        Q.CastDelay, 
                        Q.Speed).OrderByDescending(r => r.GetCollisionObjects<Obj_AI_Minion>().Length).FirstOrDefault();

                if (predictResult != null && predictResult.CollisionObjects.Length >= 2)
                {
                    Q.Cast(predictResult.CastPosition);
                }
            }

            if (useW)
            {
                var minions = EntityManager.MinionsAndMonsters.GetLaneMinions(
                    EntityManager.UnitTeam.Enemy, 
                    Player.Instance.Position, 
                    W.Range, 
                    false);

                if (minions.Count() >= 2)
                {
                    W.Cast();
                }
            }
        }
    }
}