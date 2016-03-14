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

    internal class Clear
    {
        public static void Start()
        {
            var useQ = Menu.LaneMenu["Q"].Cast<CheckBox>().CurrentValue;
            var useE = Menu.LaneMenu["E"].Cast<CheckBox>().CurrentValue;

            var allMinions = EntityManager.MinionsAndMonsters.Get(
                EntityManager.MinionsAndMonsters.EntityType.Minion,
                EntityManager.UnitTeam.Enemy,
                ObjectManager.Player.Position,
                Spells.Q.Range,
                false);
            if (allMinions == null)
            {
                return;
            }

            var objAiMinions = allMinions as IList<Obj_AI_Minion> ?? allMinions.ToList();
            foreach (var minion in objAiMinions)
            {
                objAiMinions.Any();
                {
                    if (useQ)
                    {
                        var fl = EntityManager.MinionsAndMonsters.GetLineFarmLocation(
                            objAiMinions,
                            Spells.Q.Width,
                            (int)Spells.Q.Range);
                        if (fl.HitNumber >= 1)
                        {
                            Spells.Q.Cast(fl.CastPosition);
                        }
                    }

                    if (useE
                        && minion.TotalShieldHealth()
                        <= ObjectManager.Player.GetSpellDamage(minion, SpellSlot.E)
                        + ObjectManager.Player.GetAutoAttackDamage(minion))
                    {
                        Spells.E.Cast(minion.Position);
                        Orbwalker.ResetAutoAttack();
                        Player.IssueOrder(GameObjectOrder.AttackUnit, minion);
                    }
                }
            }
        }
    }
}