namespace KappaKindred.Events
{
    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Menu.Values;

    internal class OnBasicAttack
    {
        public static void OnAttack(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
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

            var Rally = Menu.UltMenu.Get<CheckBox>("Rally").CurrentValue && Spells.R.IsReady();
            var Rallyh = Menu.UltMenu.Get<Slider>("Rallyh").CurrentValue;

            if (!target.IsAlly || !target.IsMe || !caster.IsEnemy || target.IsEnemy || target.IsMinion
                || Menu.UltMenu["DontUlt" + target.BaseSkinName].Cast<CheckBox>().CurrentValue)
            {
                return;
            }

            if (target.IsValidTarget(Spells.R.Range) && Rally)
            {
                if (target.HealthPercent <= Rallyh)
                {
                    Spells.R.Cast(target);
                }

                if (caster.GetAutoAttackDamage(target) >= target.TotalShieldHealth())
                {
                    Spells.R.Cast(target);
                }
            }
        }
    }
}