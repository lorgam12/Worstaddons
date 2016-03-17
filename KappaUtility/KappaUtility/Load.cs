namespace KappaUtility
{
    using System;

    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Events;
    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Menu.Values;

    using Items;

    using Summoners;

    using Trackers;

    internal class Load
    {
        public static Menu UtliMenu;

        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoad;
        }

        private static void OnLoad(EventArgs args)
        {
            UtliMenu = MainMenu.AddMenu("KappaUtility", "KappaUtility");
            Tracker.OnLoad();
            Spells.OnLoad();
            Potions.OnLoad();
            Offensive.OnLoad();
            Defensive.OnLoad();

            Game.OnUpdate += Game_OnUpdate;
            Obj_AI_Base.OnProcessSpellCast += OnProcessSpellCast;
            Obj_AI_Base.OnBasicAttack += OnBasicAttack;
        }

        private static void Game_OnUpdate(EventArgs args)
        {
            var flags = Orbwalker.ActiveModesFlags;
            if (flags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                Offensive.Items();
                Defensive.Items();
            }
            Smite.Smiteopepi();
        }

        public static void OnBasicAttack(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsAlly || !args.Target.IsValid || args.Target.IsEnemy || sender is Obj_AI_Minion
                || args.Target is Obj_AI_Minion || args.Target == null || args.Target is Obj_AI_Turret)
            {
                return;
            }

            var Seraph = Defensive.Seraph;
            var Solari = Defensive.Solari;
            var FOTM = Defensive.FOTM;
            var Zhonyas = Defensive.Zhonyas;

            var Corrupting = Potions.Corrupting;
            var Health = Potions.Health;
            var Hunters = Potions.Hunters;
            var Refillable = Potions.Refillable;
            var Biscuit = Potions.Biscuit;

            var Corruptingc = Potions.PotMenu["CP"].Cast<CheckBox>().CurrentValue && Corrupting.IsOwned()
                              && Corrupting.IsReady();
            var Corruptingh = Potions.PotMenu["CPH"].Cast<Slider>().CurrentValue;

            var Healthc = Potions.PotMenu["HP"].Cast<CheckBox>().CurrentValue && Health.IsOwned() && Health.IsReady();
            var Healthh = Potions.PotMenu["HPH"].Cast<Slider>().CurrentValue;

            var Huntersc = Potions.PotMenu["HPS"].Cast<CheckBox>().CurrentValue && Hunters.IsOwned()
                           && Hunters.IsReady();
            var Huntersh = Potions.PotMenu["HPSH"].Cast<Slider>().CurrentValue;

            var Refillablec = Potions.PotMenu["RP"].Cast<CheckBox>().CurrentValue && Refillable.IsOwned()
                              && Refillable.IsReady();
            var Refillableh = Potions.PotMenu["RPH"].Cast<Slider>().CurrentValue;

            var Biscuitc = Potions.PotMenu["BP"].Cast<CheckBox>().CurrentValue && Biscuit.IsOwned() && Biscuit.IsReady();
            var Biscuith = Potions.PotMenu["BPH"].Cast<Slider>().CurrentValue;

            var Seraphc = Defensive.DefMenu["Seraph"].Cast<CheckBox>().CurrentValue && Seraph.IsOwned()
                          && Seraph.IsReady();
            var Seraphh = Defensive.DefMenu["Seraphh"].Cast<Slider>().CurrentValue;

            var Solaric = Defensive.DefMenu["Solari"].Cast<CheckBox>().CurrentValue && Solari.IsOwned()
                          && Solari.IsReady();
            var Solarih = Defensive.DefMenu["Solarih"].Cast<Slider>().CurrentValue;

            var FaceOfTheMountainc = Defensive.DefMenu["FaceOfTheMountain"].Cast<CheckBox>().CurrentValue
                                     && FOTM.IsOwned() && FOTM.IsReady();
            var FaceOfTheMountainh = Defensive.DefMenu["FaceOfTheMountainh"].Cast<Slider>().CurrentValue;

            var Zhonyasc = Defensive.DefMenu["Zhonyas"].Cast<CheckBox>().CurrentValue && Zhonyas.IsOwned()
                           && Zhonyas.IsReady();
            var Zhonyash = Defensive.DefMenu["Zhonyash"].Cast<Slider>().CurrentValue;

            var caster = sender;
            var target = (AIHeroClient)args.Target;

            if (!target.IsAlly || !target.IsMe || !caster.IsEnemy || target.IsEnemy || target.IsMinion)
            {
                return;
            }

            if (target.IsValidTarget(FOTM.Range) && FaceOfTheMountainc)
            {
                if (target.HealthPercent < FaceOfTheMountainh)
                {
                    FOTM.Cast(target);
                }

                if (caster.GetAutoAttackDamage(target) > target.TotalShieldHealth())
                {
                    FOTM.Cast(target);
                }
            }

            if (target.IsValidTarget(Solari.Range) && Solaric)
            {
                if (target.HealthPercent < Solarih)
                {
                    Solari.Cast();
                }

                if (caster.GetAutoAttackDamage(target) > target.TotalShieldHealth())
                {
                    Solari.Cast();
                }
            }

            if (target.IsMe)
            {
                if (Refillablec)
                {
                    if (target.HealthPercent < Refillableh)
                    {
                        Refillable.Cast();
                    }

                    if (caster.GetAutoAttackDamage(target) > target.TotalShieldHealth())
                    {
                        Refillable.Cast();
                    }
                }

                if (Healthc)
                {
                    if (target.HealthPercent < Healthh)
                    {
                        Health.Cast();
                    }

                    if (caster.GetAutoAttackDamage(target) > target.TotalShieldHealth())
                    {
                        Health.Cast();
                    }
                }

                if (Huntersc)
                {
                    if (target.HealthPercent < Huntersh)
                    {
                        Hunters.Cast();
                    }

                    if (caster.GetAutoAttackDamage(target) > target.TotalShieldHealth())
                    {
                        Hunters.Cast();
                    }
                }

                if (Biscuitc)
                {
                    if (target.HealthPercent < Biscuith)
                    {
                        Biscuit.Cast();
                    }

                    if (caster.GetAutoAttackDamage(target) > target.TotalShieldHealth())
                    {
                        Biscuit.Cast();
                    }
                }

                if (Corruptingc)
                {
                    if (target.HealthPercent < Corruptingh)
                    {
                        Corrupting.Cast();
                    }

                    if (caster.GetAutoAttackDamage(target) > target.TotalShieldHealth())
                    {
                        Corrupting.Cast();
                    }
                }

                if (Seraphc)
                {
                    if (target.HealthPercent < Seraphh)
                    {
                        Seraph.Cast();
                    }

                    if (caster.GetAutoAttackDamage(target) > target.TotalShieldHealth())
                    {
                        Seraph.Cast();
                    }
                }

                if (Zhonyasc)
                {
                    if (target.HealthPercent < Zhonyash)
                    {
                        Zhonyas.Cast();
                    }

                    if (caster.GetAutoAttackDamage(target) > target.TotalShieldHealth())
                    {
                        Zhonyas.Cast();
                    }
                }
            }
        }

        public static void OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsAlly || !args.Target.IsValid || args.Target.IsEnemy || sender is Obj_AI_Minion
                || args.Target is Obj_AI_Minion || args.Target == null || args.Target is Obj_AI_Turret)
            {
                return;
            }

            var Corrupting = Potions.Corrupting;
            var Health = Potions.Health;
            var Hunters = Potions.Hunters;
            var Refillable = Potions.Refillable;
            var Biscuit = Potions.Biscuit;

            var Corruptingc = Potions.PotMenu["CP"].Cast<CheckBox>().CurrentValue && Corrupting.IsOwned()
                              && Corrupting.IsReady();
            var Corruptingh = Potions.PotMenu["CPH"].Cast<Slider>().CurrentValue;

            var Healthc = Potions.PotMenu["HP"].Cast<CheckBox>().CurrentValue && Health.IsOwned() && Health.IsReady();
            var Healthh = Potions.PotMenu["HPH"].Cast<Slider>().CurrentValue;

            var Huntersc = Potions.PotMenu["HPS"].Cast<CheckBox>().CurrentValue && Hunters.IsOwned()
                           && Hunters.IsReady();
            var Huntersh = Potions.PotMenu["HPSH"].Cast<Slider>().CurrentValue;

            var Refillablec = Potions.PotMenu["RP"].Cast<CheckBox>().CurrentValue && Refillable.IsOwned()
                              && Refillable.IsReady();
            var Refillableh = Potions.PotMenu["RPH"].Cast<Slider>().CurrentValue;

            var Biscuitc = Potions.PotMenu["BP"].Cast<CheckBox>().CurrentValue && Biscuit.IsOwned() && Biscuit.IsReady();
            var Biscuith = Potions.PotMenu["BPH"].Cast<Slider>().CurrentValue;

            var Seraph = Defensive.Seraph;
            var Seraphc = Defensive.DefMenu["Seraph"].Cast<CheckBox>().CurrentValue && Seraph.IsOwned()
                          && Seraph.IsReady();
            var Seraphh = Defensive.DefMenu["Seraphh"].Cast<Slider>().CurrentValue;

            var Solari = Defensive.Solari;
            var Solaric = Defensive.DefMenu["Solari"].Cast<CheckBox>().CurrentValue && Solari.IsOwned()
                          && Solari.IsReady();
            var Solarih = Defensive.DefMenu["Solarih"].Cast<Slider>().CurrentValue;

            var FOTM = Defensive.FOTM;
            var FaceOfTheMountainc = Defensive.DefMenu["FaceOfTheMountain"].Cast<CheckBox>().CurrentValue
                                     && FOTM.IsOwned() && FOTM.IsReady();
            var FaceOfTheMountainh = Defensive.DefMenu["FaceOfTheMountainh"].Cast<Slider>().CurrentValue;

            var Zhonyas = Defensive.Zhonyas;
            var Zhonyasc = Defensive.DefMenu["Zhonyas"].Cast<CheckBox>().CurrentValue && Zhonyas.IsOwned()
                           && Zhonyas.IsReady();
            var Zhonyash = Defensive.DefMenu["Zhonyash"].Cast<Slider>().CurrentValue;

            var caster = sender;
            var target = (AIHeroClient)args.Target;

            if (!target.IsAlly || !target.IsMe || !caster.IsEnemy || target.IsEnemy || target.IsMinion)
            {
                return;
            }

            if (target.IsValidTarget(FOTM.Range))
            {
                if (FaceOfTheMountainc && target.HealthPercent < FaceOfTheMountainh)
                {
                    FOTM.Cast(target);
                }

                if (caster.BaseAttackDamage > target.TotalShieldHealth()
                    || caster.BaseAbilityDamage > target.TotalShieldHealth())
                {
                    FOTM.Cast(target);
                }
            }

            if (target.IsValidTarget(Solari.Range) && Solaric)
            {
                if (target.HealthPercent < Solarih)
                {
                    Solari.Cast();
                }

                if (caster.BaseAttackDamage > target.TotalShieldHealth()
                    || caster.BaseAbilityDamage > target.TotalShieldHealth())
                {
                    Solari.Cast();
                }
            }

            if (target.IsMe)
            {
                if (Refillablec)
                {
                    if (target.HealthPercent < Refillableh)
                    {
                        Refillable.Cast();
                    }

                    if (caster.BaseAttackDamage > target.TotalShieldHealth()
                        || caster.BaseAbilityDamage > target.TotalShieldHealth())
                    {
                        Refillable.Cast();
                    }
                }

                if (Healthc)
                {
                    if (target.HealthPercent < Healthh)
                    {
                        Health.Cast();
                    }

                    if (caster.BaseAttackDamage > target.TotalShieldHealth()
                        || caster.BaseAbilityDamage > target.TotalShieldHealth())
                    {
                        Health.Cast();
                    }
                }

                if (Huntersc)
                {
                    if (target.HealthPercent < Huntersh)
                    {
                        Hunters.Cast();
                    }

                    if (caster.BaseAttackDamage > target.TotalShieldHealth()
                        || caster.BaseAbilityDamage > target.TotalShieldHealth())
                    {
                        Hunters.Cast();
                    }
                }

                if (Biscuitc)
                {
                    if (target.HealthPercent < Biscuith)
                    {
                        Biscuit.Cast();
                    }

                    if (caster.BaseAttackDamage > target.TotalShieldHealth()
                        || caster.BaseAbilityDamage > target.TotalShieldHealth())
                    {
                        Biscuit.Cast();
                    }
                }

                if (Corruptingc)
                {
                    if (target.HealthPercent < Corruptingh)
                    {
                        Corrupting.Cast();
                    }

                    if (caster.BaseAttackDamage > target.TotalShieldHealth()
                        || caster.BaseAbilityDamage > target.TotalShieldHealth())
                    {
                        Corrupting.Cast();
                    }
                }

                if (Seraphc)
                {
                    if (target.HealthPercent < Seraphh)
                    {
                        Seraph.Cast();
                    }

                    if (caster.BaseAttackDamage > target.TotalShieldHealth()
                        || caster.BaseAbilityDamage > target.TotalShieldHealth())
                    {
                        Seraph.Cast();
                    }
                }

                if (Zhonyasc)
                {
                    if (target.HealthPercent < Zhonyash)
                    {
                        Zhonyas.Cast();
                    }

                    if (caster.BaseAttackDamage > target.TotalShieldHealth()
                        || caster.BaseAbilityDamage > target.TotalShieldHealth())
                    {
                        Zhonyas.Cast();
                    }
                }
            }
        }
    }
}