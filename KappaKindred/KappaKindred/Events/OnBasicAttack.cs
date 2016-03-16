namespace KappaKindred.Events
{
    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Menu.Values;

    internal class OnBasicAttack
    {
        public static void OnAttack(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!Spells.R.IsReady() || sender.IsAlly || !args.Target.IsValid || sender == null || args.Target.IsEnemy
                || sender is Obj_AI_Minion || args.Target is Obj_AI_Minion
                || args.Target.Distance(Player.Instance) > Spells.R.Range || args.Target == null)
            {
                return;
            }

            var Rally = Menu.UltMenu.Get<CheckBox>("Rally").CurrentValue;
            var Rallyh = Menu.UltMenu.Get<Slider>("Rallyh").CurrentValue;
            var caster = sender;
            var target = (AIHeroClient)args.Target;

            if (!target.IsAlly || !target.IsMe || !caster.IsEnemy || target.IsEnemy || target.IsMinion
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

                if (caster.GetAutoAttackDamage(target) > target.TotalShieldHealth())
                {
                    Spells.R.Cast(target);
                }
            }
        }
    }
}