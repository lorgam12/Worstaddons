namespace KappaUtility
{
    using System;

    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Events;
    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Menu.Values;

    using Items;

    using Misc;

    using Summoners;

    using Trackers;

    internal class Load
    {
        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoad;
        }

        public static Menu UtliMenu;

        private static void OnLoad(EventArgs args)
        {
                UtliMenu = MainMenu.AddMenu("KappaUtility", "KappaUtility");
                AutoLvlUp.OnLoad();
                AutoQSS.OnLoad();
                AutoTear.OnLoad();
                AutoReveal.OnLoad();
                GanksDetector.OnLoad();
                Tracker.OnLoad();
                Surrender.OnLoad();
                SkinHax.OnLoad();
                Spells.OnLoad();
                Potions.OnLoad();
                Offensive.OnLoad();
                Defensive.OnLoad();

                Game.OnTick += GameOnTick;
                Drawing.OnEndScene += OnEndScene;
                Drawing.OnDraw += DrawingOnDraw;
                Obj_AI_Base.OnProcessSpellCast += OnProcessSpellCast;
                Obj_AI_Base.OnBasicAttack += OnBasicAttack;
        }

        private static void DrawingOnDraw(EventArgs args)
        {
            try
            {
                Spells.Drawings();
                GanksDetector.OnDraw();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private static void OnEndScene(EventArgs args)
        {
            try
            {
                AutoReveal.Drawings();
                Traps.Draw();
                Tracker.HPtrack();
                Tracker.track();
                GanksDetector.OnEndScene();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private static void GameOnTick(EventArgs args)
        {
            try
            {
                var flags = Orbwalker.ActiveModesFlags;
                if (flags.HasFlag(Orbwalker.ActiveModes.Combo))
                {
                    Offensive.Items();
                    Defensive.Items();
                }

                AutoReveal.Reveal();
                AutoLvlUp.Levelup();
                AutoTear.OnUpdate();
                GanksDetector.OnUpdate();
                Smite.Smiteopepi();
                Spells.Cast();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
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
            if ((caster is AIHeroClient || caster is Obj_AI_Turret) && caster.IsEnemy && target != null && target.IsAlly)
            {
                if (target.IsValidTarget(Defensive.FOTM.Range) && Defensive.FaceOfTheMountainc)
                {
                    if (target.HealthPercent <= Defensive.FaceOfTheMountainh)
                    {
                        Defensive.FOTM.Cast(target);
                    }

                    if (caster.GetAutoAttackDamage(target) >= target.TotalShieldHealth())
                    {
                        Defensive.FOTM.Cast(target);
                    }
                }

                if (target.IsValidTarget(Defensive.Solari.Range) && Defensive.Solaric)
                {
                    if (target.HealthPercent <= Defensive.Solarih)
                    {
                        Defensive.Solari.Cast();
                    }

                    if (caster.GetAutoAttackDamage(target) >= target.TotalShieldHealth())
                    {
                        Defensive.Solari.Cast();
                    }
                }

                if (target.IsMe)
                {
                    if (!Player.Instance.IsRecalling())
                    {
                        if (Potions.Refillablec)
                        {
                            if (target.HealthPercent <= Potions.Refillableh)
                            {
                                Potions.Refillable.Cast();
                            }

                            if (caster.GetAutoAttackDamage(target) >= target.TotalShieldHealth())
                            {
                                Potions.Refillable.Cast();
                            }
                        }

                        if (Potions.Healthc)
                        {
                            if (target.HealthPercent <= Potions.Healthh)
                            {
                                Potions.Health.Cast();
                            }

                            if (caster.GetAutoAttackDamage(target) >= target.TotalShieldHealth())
                            {
                                Potions.Health.Cast();
                            }
                        }

                        if (Potions.Huntersc)
                        {
                            if (target.HealthPercent <= Potions.Huntersh)
                            {
                                Potions.Hunters.Cast();
                            }

                            if (caster.GetAutoAttackDamage(target) >= target.TotalShieldHealth())
                            {
                                Potions.Hunters.Cast();
                            }
                        }

                        if (Potions.Biscuitc)
                        {
                            if (target.HealthPercent <= Potions.Biscuith)
                            {
                                Potions.Biscuit.Cast();
                            }

                            if (caster.GetAutoAttackDamage(target) >= target.TotalShieldHealth())
                            {
                                Potions.Biscuit.Cast();
                            }
                        }

                        if (Potions.Corruptingc)
                        {
                            if (target.HealthPercent <= Potions.Corruptingh)
                            {
                                Potions.Corrupting.Cast();
                            }

                            if (caster.GetAutoAttackDamage(target) >= target.TotalShieldHealth())
                            {
                                Potions.Corrupting.Cast();
                            }
                        }
                    }

                    if (Defensive.Seraphc)
                    {
                        if (target.HealthPercent <= Defensive.Seraphh)
                        {
                            Defensive.Seraph.Cast();
                        }

                        if (caster.GetAutoAttackDamage(target) >= target.TotalShieldHealth())
                        {
                            Defensive.Seraph.Cast();
                        }
                    }

                    if (Defensive.Zhonyasc)
                    {
                        if (target.HealthPercent <= Defensive.Zhonyash)
                        {
                            Defensive.Zhonyas.Cast();
                        }

                        if (caster.GetAutoAttackDamage(target) >= target.TotalShieldHealth())
                        {
                            Defensive.Zhonyas.Cast();
                        }
                    }
                }
            }
        }

        public static void OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!(args.Target is AIHeroClient))
            {
                return;
            }

            var caster = sender;
            var target = (AIHeroClient)args.Target;

            if ((caster is AIHeroClient || caster is Obj_AI_Turret) && caster.IsEnemy && target != null && target.IsAlly)
            {
                if (target.IsValidTarget(Defensive.FOTM.Range))
                {
                    if (Defensive.FaceOfTheMountainc && target.HealthPercent <= Defensive.FaceOfTheMountainh)
                    {
                        Defensive.FOTM.Cast(target);
                    }

                    if (caster.BaseAttackDamage >= target.TotalShieldHealth()
                        || caster.BaseAbilityDamage >= target.TotalShieldHealth())
                    {
                        Defensive.FOTM.Cast(target);
                    }
                }

                if (target.IsValidTarget(Defensive.Solari.Range) && Defensive.Solaric)
                {
                    if (target.HealthPercent <= Defensive.Solarih)
                    {
                        Defensive.Solari.Cast();
                    }

                    if (caster.BaseAttackDamage >= target.TotalShieldHealth()
                        || caster.BaseAbilityDamage >= target.TotalShieldHealth())
                    {
                        Defensive.Solari.Cast();
                    }
                }

                if (target.IsMe)
                {
                    if (!Player.Instance.IsRecalling())
                    {
                        if (Potions.Refillablec)
                        {
                            if (target.HealthPercent <= Potions.Refillableh)
                            {
                                Potions.Refillable.Cast();
                            }

                            if (caster.GetAutoAttackDamage(target) >= target.TotalShieldHealth())
                            {
                                Potions.Refillable.Cast();
                            }
                        }

                        if (Potions.Healthc)
                        {
                            if (target.HealthPercent <= Potions.Healthh)
                            {
                                Potions.Health.Cast();
                            }

                            if (caster.GetAutoAttackDamage(target) >= target.TotalShieldHealth())
                            {
                                Potions.Health.Cast();
                            }
                        }

                        if (Potions.Huntersc)
                        {
                            if (target.HealthPercent <= Potions.Huntersh)
                            {
                                Potions.Hunters.Cast();
                            }

                            if (caster.GetAutoAttackDamage(target) >= target.TotalShieldHealth())
                            {
                                Potions.Hunters.Cast();
                            }
                        }

                        if (Potions.Biscuitc)
                        {
                            if (target.HealthPercent <= Potions.Biscuith)
                            {
                                Potions.Biscuit.Cast();
                            }

                            if (caster.GetAutoAttackDamage(target) >= target.TotalShieldHealth())
                            {
                                Potions.Biscuit.Cast();
                            }
                        }

                        if (Potions.Corruptingc)
                        {
                            if (target.HealthPercent <= Potions.Corruptingh)
                            {
                                Potions.Corrupting.Cast();
                            }

                            if (caster.GetAutoAttackDamage(target) >= target.TotalShieldHealth())
                            {
                                Potions.Corrupting.Cast();
                            }
                        }
                    }

                    if (Defensive.Seraphc)
                    {
                        if (target.HealthPercent <= Defensive.Seraphh)
                        {
                            Defensive.Seraph.Cast();
                        }

                        if (caster.BaseAttackDamage >= target.TotalShieldHealth()
                            || caster.BaseAbilityDamage >= target.TotalShieldHealth())
                        {
                            Defensive.Seraph.Cast();
                        }
                    }

                    if (Defensive.Zhonyasc)
                    {
                        if (target.HealthPercent <= Defensive.Zhonyash)
                        {
                            Defensive.Zhonyas.Cast();
                        }

                        if (caster.BaseAttackDamage >= target.TotalShieldHealth()
                            || caster.BaseAbilityDamage >= target.TotalShieldHealth())
                        {
                            Defensive.Zhonyas.Cast();
                        }
                    }
                }
            }
        }
    }
}