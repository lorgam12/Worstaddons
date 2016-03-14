namespace KappaEkko.Events
{
    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Menu.Values;

    internal class OnProcessSpellCast
    {
        public static void OnSpell(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender == null || args == null || sender.IsMe || sender.IsAlly)
            {
                return;
            }

            var Rsave = Menu.UltMenu["Rsave"].Cast<CheckBox>().CurrentValue;
            var Rsaveh = Menu.UltMenu["Rsaveh"].Cast<Slider>().CurrentValue;
            var Health = ObjectManager.Player.HealthPercent;
            var caster = sender;
            var target = (AIHeroClient)args.Target;

            if (!target.IsMe || !caster.IsEnemy || caster is Obj_AI_Minion)
            {
                return;
            }

            if (caster.IsValid && args.Target.IsMe && Spells.R.IsReady()
                && ObjectManager.Player.CountEnemiesInRange(1000) >= 1)
            {
                if (Rsave)
                {
                    if (Rsaveh >= Health)
                    {
                        Spells.R.Cast();
                    }
                }
            }
        }
    }
}