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

            Chat.Print(caster.BaseSkinName);
            if (caster.IsValid && args.Target.IsMe && Spells.R.IsReady())
            {
                if (sender.GetAutoAttackDamage(ObjectManager.Player) >= ObjectManager.Player.Health)
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

        /*
        public static void Spell(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            var Rsave = Menu.UltMenu["Rsave"].Cast<CheckBox>().CurrentValue;
            var Rsaveh = Menu.UltMenu["Rsaveh"].Cast<Slider>().CurrentValue;
            var Health = ObjectManager.Player.HealthPercent;
            if ((sender.IsValid && (sender is AIHeroClient || sender is Obj_AI_Turret)) && sender.IsEnemy && args.Target.IsMe)
            {
                if (sender.GetAutoAttackDamage(ObjectManager.Player) >= ObjectManager.Player.Health
                     && args.SData.IsAutoAttack())
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
        */
    }
}