namespace KappaUtility.Common
{
    using EloBuddy;
    using EloBuddy.SDK;

    using Items;

    internal class DamageHandler
    {
        internal static void OnLoad()
        {
            Obj_AI_Base.OnProcessSpellCast += OnProcessSpellCast;
            Obj_AI_Base.OnBasicAttack += OnBasicAttack;
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

                    if (caster.BaseAttackDamage >= target.TotalShieldHealth() || caster.BaseAbilityDamage >= target.TotalShieldHealth())
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

                    if (caster.BaseAttackDamage >= target.TotalShieldHealth() || caster.BaseAbilityDamage >= target.TotalShieldHealth())
                    {
                        Defensive.Solari.Cast();
                    }
                }

                if (target.IsMe)
                {
                    if (Defensive.Seraphc)
                    {
                        if (target.HealthPercent <= Defensive.Seraphh)
                        {
                            Defensive.Seraph.Cast();
                        }

                        if (caster.BaseAttackDamage >= target.TotalShieldHealth() || caster.BaseAbilityDamage >= target.TotalShieldHealth())
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

                        if (caster.BaseAttackDamage >= target.TotalShieldHealth() || caster.BaseAbilityDamage >= target.TotalShieldHealth())
                        {
                            Defensive.Zhonyas.Cast();
                        }
                    }
                }
            }
        }
    }
}