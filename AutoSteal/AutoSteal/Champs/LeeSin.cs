using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoSteal.Champs
{
    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Menu.Values;

    class LeeSin
    {


        public static Spell.Skillshot Q { get; set; }
        public static Spell.Active E { get; set; }
        public static Menu LeeMenu { get; set; }


        public static void KS()
        {
            foreach (AIHeroClient target in
                ObjectManager.Get<AIHeroClient>()
                    .Where(
                        hero =>
                        hero.IsValidTarget(E.Range)
                        && !hero.HasBuffOfType(BuffType.Invulnerability)
                        && hero.IsEnemy))
            {

                if (LeeMenu["QC"].Cast<CheckBox>().CurrentValue)
                {
                    if (ObjectManager.Player.BaseAbilityDamage + ObjectManager.Player.GetSpellDamage(target, SpellSlot.Q) > target.TotalShieldHealth()
                        && Q.IsInRange(target) && Q.IsReady())
                    {
                        Q.Cast(target);
                        return;
                    }
                }

                if (LeeMenu["EC"].Cast<CheckBox>().CurrentValue)
                {
                    if (ObjectManager.Player.BaseAbilityDamage + ObjectManager.Player.GetSpellDamage(target, SpellSlot.E) > target.TotalShieldHealth()
                        && E.IsInRange(target) && E.IsReady())
                    {
                        E.Cast(target);
                        return;
                    }
                }
            }
        }

        public static void JKS()
        {

            foreach (Obj_AI_Minion mob in
                ObjectManager.Get<Obj_AI_Minion>()
                    .Where(
                        jmob =>
                        E.IsInRange(jmob)
                        && !jmob.HasBuffOfType(BuffType.Invulnerability)
                        && jmob.IsMonster
                        && (jmob.BaseSkinName == "SRU_Dragon"
                        || jmob.BaseSkinName == "SRU_Baron"
                        || jmob.BaseSkinName == "SRU_Gromp"
                        || jmob.BaseSkinName == "SRU_Krug"
                        || jmob.BaseSkinName == "SRU_Razorbeak"
                        || jmob.BaseSkinName == "Sru_Crab"
                        || jmob.BaseSkinName == "SRU_Murkwolf"
                        || jmob.BaseSkinName == "SRU_Blue"
                        || jmob.BaseSkinName == "SRU_Red")))
            {

                if (LeeMenu["QJ"].Cast<CheckBox>().CurrentValue)
                {
                    if (ObjectManager.Player.BaseAbilityDamage + ObjectManager.Player.GetSpellDamage(mob, SpellSlot.Q) > mob.TotalShieldHealth()
                        && Q.IsInRange(mob) && Q.IsReady())
                    {
                        Q.Cast(mob);
                        return;
                    }
                }

                if (LeeMenu["EJ"].Cast<CheckBox>().CurrentValue)
                {
                    if (ObjectManager.Player.BaseAbilityDamage + ObjectManager.Player.GetSpellDamage(mob, SpellSlot.E) > mob.TotalShieldHealth()
                        && E.IsInRange(mob) && E.IsReady())
                    {
                        E.Cast(mob);
                        return;
                    }
                }
            }
        }
    }
}
