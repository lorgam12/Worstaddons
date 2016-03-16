namespace KappaKindred.Events
{
    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Menu.Values;

    internal class OnDamage
    {
        public static void Damage(AttackableUnit sender, AttackableUnitDamageEventArgs args)
        {
            if (!Spells.R.IsReady() || sender.IsAlly || sender == null || args.Target.IsEnemy || sender is Obj_AI_Minion
                || args.Target == null || args.Target is Obj_AI_Minion)
            {
                return;
            }
            var Rally = Menu.UltMenu.Get<CheckBox>("Rally").CurrentValue;
            var Rallyh = Menu.UltMenu.Get<Slider>("Rallyh").CurrentValue;
            var caster = sender;
            var target = (AIHeroClient)args.Target;

            if (!target.IsAlly || !target.IsMe || !caster.IsEnemy || target.IsEnemy
                || Menu.UltMenu["DontUlt" + target.BaseSkinName].Cast<CheckBox>().CurrentValue)
            {
                return;
            }

            if (target.IsValidTarget(Spells.R.Range))
            {
                if (Rally && target.HealthPercent < Rallyh)
                {
                    Spells.R.Cast(target);
                }
            }
        }
    }
}