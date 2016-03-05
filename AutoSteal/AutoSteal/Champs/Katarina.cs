namespace AutoSteal.Champs
{
    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Menu.Values;
    using System.Linq;


    class Katarina
    {

        public static Spell.Targeted Q { get; set; }
        public static Spell.Active W { get; set; }
        public static Spell.Targeted E { get; set; }
        public static Menu KataMenu { get; set; }


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

                if (KataMenu["QC"].Cast<CheckBox>().CurrentValue)
                {
                    if (ObjectManager.Player.BaseAbilityDamage + ObjectManager.Player.GetSpellDamage(target, SpellSlot.Q) > target.TotalShieldHealth()
                        && Q.IsInRange(target) && Q.IsReady())
                    {
                        Q.Cast(target);
                        return;
                    }
                }

                if (KataMenu["WC"].Cast<CheckBox>().CurrentValue)
                {
                    if (ObjectManager.Player.BaseAbilityDamage + ObjectManager.Player.GetSpellDamage(target, SpellSlot.W) > target.TotalShieldHealth()
                        && W.IsInRange(target) && W.IsReady())
                    {
                        W.Cast();
                        return;
                    }
                }

                if (KataMenu["EC"].Cast<CheckBox>().CurrentValue)
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

                if (KataMenu["QJ"].Cast<CheckBox>().CurrentValue)
                {
                    if (ObjectManager.Player.BaseAbilityDamage + ObjectManager.Player.GetSpellDamage(mob, SpellSlot.Q) > mob.TotalShieldHealth()
                        && Q.IsInRange(mob) && Q.IsReady())
                    {
                        Q.Cast(mob);
                        return;
                    }
                }

                if (KataMenu["WJ"].Cast<CheckBox>().CurrentValue)
                {
                    if (ObjectManager.Player.BaseAbilityDamage + ObjectManager.Player.GetSpellDamage(mob, SpellSlot.W) > mob.TotalShieldHealth()
                        && W.IsInRange(mob) && W.IsReady())
                    {
                        W.Cast();
                        return;
                    }
                }

                if (KataMenu["EJ"].Cast<CheckBox>().CurrentValue)
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
