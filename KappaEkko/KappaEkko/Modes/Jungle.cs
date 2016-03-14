using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KappaEkko.Modes
{
    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Menu.Values;

    internal class Jungle
    {
        public static void Start()
        {
            var useQ = Menu.JungleMenu["Q"].Cast<CheckBox>().CurrentValue;
            var useE = Menu.JungleMenu["E"].Cast<CheckBox>().CurrentValue;

            var jmobs =
                ObjectManager.Get<Obj_AI_Minion>()
                    .OrderBy(m => m.CampNumber)
                    .Where(m => m.IsMonster && m.IsEnemy && !m.IsDead);
            var objAiMinions = jmobs as IList<Obj_AI_Minion> ?? jmobs.ToList();
            foreach (var jmob in objAiMinions)
            {
                if (useQ && jmob.IsValidTarget(Spells.Q.Range) && objAiMinions.Count() > 1)
                {
                    Spells.Q.Cast(jmob);
                }

                if (useE && jmob.IsValidTarget(Spells.E.Range))
                {
                    Spells.E.Cast(jmob);
                    Orbwalker.ResetAutoAttack();
                    Player.IssueOrder(GameObjectOrder.AttackUnit, jmob);
                }
            }
        }
    }
}