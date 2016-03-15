using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KappaKindred.Events
{
    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Menu.Values;

    internal class OnProcessSpellCast
    {
        public static void OnSpell(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsValid && !Spells.R.IsReady() || sender.IsAlly || !args.Target.IsValid
                || args.Target.IsEnemy || sender is Obj_AI_Minion || args.Target is Obj_AI_Minion || args.Target == null)
            {
                return;
            }
            var Rally = Menu.UltMenu.Get<CheckBox>("Rally").CurrentValue;
            var Rallyh = Menu.UltMenu.Get<Slider>("Rallyh").CurrentValue;
            var caster = sender;
            var target = (AIHeroClient)args.Target;

            if (!target.IsAlly || !target.IsMe || !caster.IsEnemy)
            {
                return;
            }

            if(target.IsValidTarget(Spells.R.Range))
            {
                if (Rally && target.HealthPercent < Rallyh)
                {
                    Spells.R.Cast(target);
                }
            }
        }
    }
}