namespace Malzahar
{
    using System;
    using System.Linq;
    using EloBuddy;
    using EloBuddy.SDK;
    using SharpDX;
    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Menu.Values;
    using EloBuddy.SDK.Enumerations;
    using EloBuddy.SDK.Events;
    using EloBuddy.SDK.Rendering;

    /// <summary>
    ///     A plugin for Malzahar.
    /// </summary>
    internal class Program
    {
        #region Fields

        /// <summary>
        ///     The spell list
        /// </summary>
        /// 
        public static Spell.Skillshot Q { get; private set; }
        public static Spell.Skillshot W { get; private set; }
        public static Spell.Targeted E { get; private set; }
        public static Spell.Targeted R { get; private set; }
        public static Menu UltMenu { get; private set; }
        public static Menu ComboMenu { get; private set; }
        public static Menu HarassMenu { get; private set; }
        public static Menu LaneMenu { get; private set; }
        public static Menu KillStealMenu { get; private set; }
        public static Menu MiscMenu { get; private set; }
        public static Menu ItemsMenu { get; private set; }
        public static Menu DrawMenu { get; private set; }
        private static Menu menuIni;
        #endregion

        #region Constructors and Destructors

        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoad;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Malzahar" /> class.
        /// </summary>
        private static void OnLoad(EventArgs args)
        {
            if (ObjectManager.Player.ChampionName != "Malzahar")
            {
                return;
            }

            // Create spells
            Q = new Spell.Skillshot(SpellSlot.Q, 900, SkillShotType.Circular, 500, int.MaxValue, 50);
            W = new Spell.Skillshot(SpellSlot.W, 800, SkillShotType.Circular, 500, int.MaxValue, 125);
            E = new Spell.Targeted(SpellSlot.E, 650);
            R = new Spell.Targeted(SpellSlot.R, 700);

            // Create Menu

            menuIni = MainMenu.AddMenu("Malzahar ", "Malzahar");
            menuIni.AddGroupLabel("Welcome to the Worst Malzahar addon!");
            menuIni.AddGroupLabel("Global Settings");
            menuIni.Add("Ult", new CheckBox("Use Ultimate?"));
            menuIni.Add("Combo", new CheckBox("Use Combo?"));
            menuIni.Add("Harass", new CheckBox("Use Harass?"));
            menuIni.Add("LaneClear", new CheckBox("Use LaneClear?"));
            menuIni.Add("KillSteal", new CheckBox("Use Kill Steal?"));
            menuIni.Add("Misc", new CheckBox("Use Misc?"));
            menuIni.Add("Drawings", new CheckBox("Use Drawings?"));


            UltMenu = menuIni.AddSubMenu("Ultimate");
            UltMenu.AddGroupLabel("Ultimate Settings");
            UltMenu.Add("gapcloserR", new CheckBox("Use R On GapCloser"));
            UltMenu.Add("interruptR", new CheckBox("Use R Interrupt Spells"));
            UltMenu.Add("tower", new CheckBox("Auto R Under Ally Tower"));
            UltMenu.Add("R", new CheckBox("R Finisher"));
            UltMenu.Add("Rtower", new CheckBox("Don't Use R Under Enemy Turret"));


            ComboMenu = menuIni.AddSubMenu("Combo");
            ComboMenu.AddGroupLabel("Combo Settings");
            ComboMenu.Add("Q", new CheckBox("Use Q"));
            ComboMenu.Add("W", new CheckBox("Use W"));
            ComboMenu.Add("E", new CheckBox("Use E"));


            HarassMenu = menuIni.AddSubMenu("Harass");
            HarassMenu.AddGroupLabel("Harass Settings");
            HarassMenu.Add("Q", new CheckBox("Use Q", false));
            HarassMenu.Add("W", new CheckBox("Use W", false));
            HarassMenu.Add("E", new CheckBox("Use E"));
            HarassMenu.Add("harassmana", new Slider("Harass Mana Manager", 60, 0, 100));


            LaneMenu = menuIni.AddSubMenu("Farm");
            LaneMenu.AddGroupLabel("Farm Settings");
            LaneMenu.Add("Q", new CheckBox("Use Q", false));
            LaneMenu.Add("W", new CheckBox("Use W"));
            LaneMenu.Add("E", new CheckBox("Use E"));
            LaneMenu.Add("lanemana", new Slider("Farm Mana Manager", 80, 0, 100));


            KillStealMenu = menuIni.AddSubMenu("Kill Steal");
            KillStealMenu.AddGroupLabel("Kill Steal Settings");
            KillStealMenu.Add("Q", new CheckBox("Kill Steal Q"));
            KillStealMenu.Add("W", new CheckBox("Kill Steal W"));
            KillStealMenu.Add("E", new CheckBox("Kill Steal E"));


            MiscMenu = menuIni.AddSubMenu("Misc");
            MiscMenu.AddGroupLabel("Misc Settings");
            MiscMenu.Add("gapcloserQ", new CheckBox("Use Q On GapCloser"));
            MiscMenu.Add("interruptQ", new CheckBox("Use Q Interrupt Spells"));


            DrawMenu = menuIni.AddSubMenu("Drawings");
            DrawMenu.AddGroupLabel("Drawing Settings");
            DrawMenu.Add("Q", new CheckBox("Draw Q"));
            DrawMenu.Add("W", new CheckBox("Draw W"));
            DrawMenu.Add("E", new CheckBox("Draw E"));
            DrawMenu.Add("R", new CheckBox("Draw R"));


            Game.OnUpdate += Game_OnGameUpdate;
            Drawing.OnDraw += DrawingOnOnDraw;
            Gapcloser.OnGapcloser += AntiGapcloserOnOnEnemyGapcloser;
            Interrupter.OnInterruptableSpell += InterrupterOnOnPossibleToInterrupt;
        }

        #endregion
        
        #region Methods

        /// <summary>
        ///     Fired on an incoming enemy gapcloser.
        /// </summary>
        /// <param name="gapcloser">The gapcloser.</param>
        private static void AntiGapcloserOnOnEnemyGapcloser(AIHeroClient Sender, Gapcloser.GapcloserEventArgs args)
        {
            if (!Sender.IsValidTarget() && !Sender.IsEnemy && Sender.IsAlly)
            {
                return;
            }

            if (menuIni["Misc"].Cast<CheckBox>().CurrentValue)
            {
                var predq = Q.GetPrediction(Sender);
                if (Sender != null && Q.IsReady() && Sender.IsEnemy && Sender.IsValidTarget(Q.Range)
                    && MiscMenu.Get<CheckBox>("gapcloserQ").CurrentValue)
                {
                    Q.Cast(predq.CastPosition);
                    return;
                }
            }

            if (UltMenu["Rtower"].Cast<CheckBox>().CurrentValue && ObjectManager.Player.IsUnderEnemyturret())
                {
                    return;
                }

                if (Sender != null && R.IsReady() && Sender.IsEnemy && Sender.IsValidTarget(R.Range)
                    && UltMenu.Get<CheckBox>("gapcloserR").CurrentValue)
                {
                    R.Cast(Sender);
                    return;
                }

        }

        private static void InterrupterOnOnPossibleToInterrupt(Obj_AI_Base unit,
            Interrupter.InterruptableSpellEventArgs args)
        {
            var predq = Q.GetPrediction(unit);
            if (unit != null && Q.IsReady() && unit.IsEnemy && unit.IsValidTarget(Q.Range) && MiscMenu.Get<CheckBox>("interruptQ").CurrentValue)
            {
                Q.Cast(predq.CastPosition);
                return;
            }
            
            if (unit != null && R.IsReady() && UltMenu.Get<CheckBox>("interruptR").CurrentValue)
            {
                if (UltMenu["Rtower"].Cast<CheckBox>().CurrentValue && ObjectManager.Player.IsUnderEnemyturret())
                {
                    return;
                }

                if (unit.IsEnemy && unit.IsValidTarget(R.Range))
                {
                    R.Cast(unit);
                }
            }
            

        }


        private static void UnderTower()
        {
            var Target = TargetSelector.GetTarget(R.Range, DamageType.Physical);

            if (Target != null && R.IsReady() && Target.IsUnderTurret() && R.IsReady())
            {
                R.Cast(Target);
            }
        }

        private static void KillSteal()
        {
            var target =
                ObjectManager.Get<AIHeroClient>()
                    .FirstOrDefault(
                        enemy =>
                            enemy.IsValid && enemy.IsVisible && !enemy.IsDead && enemy != null);

            if (KillStealMenu["Q"].Cast<CheckBox>().CurrentValue && Q.IsReady())
            {
                if (target.IsValidTarget(Q.Range)
                    && ObjectManager.Player.GetSpellDamage(target, SpellSlot.Q) > target.TotalShieldHealth())
                {
                    if (target != null)
                    {
                        Q.Cast(target.Position);
                    }
                }
            }

            if (KillStealMenu["W"].Cast<CheckBox>().CurrentValue && W.IsReady())
            {
                if (target.IsValidTarget(W.Range)
                    && ObjectManager.Player.GetSpellDamage(target, SpellSlot.W) > target.TotalShieldHealth())
                {
                    if (target != null)
                    {
                        W.Cast(target.Position);
                    }
                }
            }

            if (KillStealMenu["E"].Cast<CheckBox>().CurrentValue && E.IsReady())
            {
                if (target.IsValidTarget(E.Range)
                    && ObjectManager.Player.GetSpellDamage(target, SpellSlot.E) > target.TotalShieldHealth())
                {
                    if (target != null)
                    {
                        E.Cast(target);
                    }
                }
            }
        }

        private static void Clear()
        {
        }

        /// <summary>
        ///     Does the combo.
        /// </summary>
        private static void DoCombo()
        {
            var target = TargetSelector.GetTarget(900, DamageType.Magical);
            if (target == null)
            {
                return;
            }

            var predq = Q.GetPrediction(target);
            var predW = W.GetPrediction(target);
            if (ComboMenu["Q"].Cast<CheckBox>().CurrentValue && Q.IsReady() && predq.HitChance >= HitChance.High)
            {
                Q.Cast(predq.CastPosition);
            }

            if (ComboMenu["W"].Cast<CheckBox>().CurrentValue && W.IsReady() && predW.HitChance >= HitChance.High)
            {
                W.Cast(predW.CastPosition);
            }

            if (ComboMenu["E"].Cast<CheckBox>().CurrentValue && E.IsReady())
            {
                E.Cast(target);
            }

            if (UltMenu["R"].Cast<CheckBox>().CurrentValue && menuIni["Ult"].Cast<CheckBox>().CurrentValue && ObjectManager.Player.GetSpellDamage(target, SpellSlot.R) > target.TotalShieldHealth() + 50)
            {
                ShouldUseR(target);
            }
        }

        /// <summary>
        ///     Does the harass.
        /// </summary>
        private static void DoHarass()
        {
            var target = TargetSelector.GetTarget(900, DamageType.Magical);

            if (target == null)
            {
                return;
            }

            var predq = Q.GetPrediction(target);
            var predW = W.GetPrediction(target);

            if (HarassMenu["Q"].Cast<CheckBox>().CurrentValue && Q.IsReady() && predq.HitChance >= HitChance.High)
            {
                Q.Cast(predq.CastPosition);
            }

            if (HarassMenu["W"].Cast<CheckBox>().CurrentValue && W.IsReady() && predW.HitChance >= HitChance.High)
            {
                W.Cast(predW.CastPosition);
            }

            if (HarassMenu["E"].Cast<CheckBox>().CurrentValue && E.IsReady())
            {
                E.Cast(target);
            }
        }
        /*
        private static bool IsCastingR()
        {
            if (ObjectManager.Player.HasBuff("AlZaharNetherGrasp"))
            {
                return true;
            }
            return false;
        }
        */

        /// <summary>
        ///     Fired when the game updates.
        /// </summary>
        /// <param name="args">The <see cref="EventArgs" /> instance containing the event data.</param>
        private static void DrawingOnOnDraw(EventArgs args)
        {
            if (menuIni["Drawings"].Cast<CheckBox>().CurrentValue)
            {
                if (DrawMenu["Q"].Cast<CheckBox>().CurrentValue && Q.IsReady())
                {
                    Circle.Draw(Color.Aqua, Q.Range, Player.Instance.Position);
                }
                else
                {
                    if (DrawMenu["Q"].Cast<CheckBox>().CurrentValue)
                        Circle.Draw(Color.Red, Q.Range, Player.Instance.Position);
                }

                if (DrawMenu["W"].Cast<CheckBox>().CurrentValue && W.IsReady())
                {
                    Circle.Draw(Color.BlueViolet, W.Range, Player.Instance.Position);
                }
                else
                {
                    if (DrawMenu["W"].Cast<CheckBox>().CurrentValue)
                        Circle.Draw(Color.Red, W.Range, Player.Instance.Position);
                }

                if (DrawMenu["E"].Cast<CheckBox>().CurrentValue && E.IsReady())
                {
                    Circle.Draw(Color.Chartreuse, E.Range, Player.Instance.Position);
                }
                else
                {
                    if (DrawMenu["E"].Cast<CheckBox>().CurrentValue)
                        Circle.Draw(Color.Red, E.Range, Player.Instance.Position);
                }

                if (DrawMenu["R"].Cast<CheckBox>().CurrentValue && R.IsReady())
                {
                    Circle.Draw(Color.Purple, R.Range, Player.Instance.Position);
                }
                else
                {
                    if(DrawMenu["R"].Cast<CheckBox>().CurrentValue)
                    Circle.Draw(Color.Red, R.Range, Player.Instance.Position);
                }
            }
        }

        /// <summary>
        ///     Fired when the game updates.
        /// </summary>
        /// <param name="args">The <see cref="EventArgs" /> instance containing the event data.</param>
        private static void Game_OnGameUpdate(EventArgs args)
        {
            var flags = Orbwalker.ActiveModesFlags;
            if (flags.HasFlag(Orbwalker.ActiveModes.Combo) && menuIni.Get<CheckBox>("Combo").CurrentValue)
            {
                DoCombo();
            }

            if (ObjectManager.Player.ManaPercent > LaneMenu["lanemana"].Cast<Slider>().CurrentValue)
            {
                if (flags.HasFlag(Orbwalker.ActiveModes.LaneClear) && menuIni.Get<CheckBox>("LaneClear").CurrentValue)
                {
                    Clear();
                }
            }

            if (ObjectManager.Player.ManaPercent > HarassMenu["harassmana"].Cast<Slider>().CurrentValue)
            {
                if (flags.HasFlag(Orbwalker.ActiveModes.Harass) && menuIni.Get<CheckBox>("Harass").CurrentValue)
                {
                    DoHarass();
                }
            }

            if (UltMenu["tower"].Cast<CheckBox>().CurrentValue && menuIni["Ult"].Cast<CheckBox>().CurrentValue && R.IsReady())
            {
                UnderTower();
            }

            if (menuIni["KillSteal"].Cast<CheckBox>().CurrentValue && (Q.IsReady() || W.IsReady() || E.IsReady()))
            {
                KillSteal();
            }
            Chat.Print(IsCastingR());
        }
        
        /// <summary>
        ///     Shoulds the use r.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        private static void ShouldUseR(Obj_AI_Base target)
        {
            if (UltMenu["Rtower"].Cast<CheckBox>().CurrentValue && ObjectManager.Player.IsUnderEnemyturret())
            {
                return;
            }

            if (target != null && ObjectManager.Player.GetSpellDamage(target, SpellSlot.R) > target.TotalShieldHealth() + 50 && R.IsReady())
                {
                    R.Cast(target);
                }
        }

        #endregion
    }
}