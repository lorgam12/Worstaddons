namespace AutoSteal.Champs
{
    using System.Linq;
    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Menu.Values;

    class Nidalee
    {
        public static Spell.Skillshot Q { get; set; }
        public static Menu NidaMenu { get; set; }

        public static void KS()
        {
            foreach (AIHeroClient target in
                ObjectManager.Get<AIHeroClient>()
                    .Where(
                        hero =>
                        hero.IsValidTarget(Q.Range)
                        && !hero.HasBuffOfType(BuffType.Invulnerability)
                        && hero.IsEnemy))
            {
                if (Nidalee.NidaMenu["QC"].Cast<CheckBox>().CurrentValue)
                {
                    if (Player.GetSpell(SpellSlot.Q).Name == "JavelinToss"  && ObjectManager.Player.BaseAbilityDamage + ObjectManager.Player.GetSpellDamage(target, SpellSlot.Q) > target.TotalShieldHealth()
                        && Q.IsInRange(target) && Q.IsReady())
                    {
                        var pred = Q.GetPrediction(target);
                        Q.Cast(pred.CastPosition);
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
                        Q.IsInRange(jmob)
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

                if (Nidalee.NidaMenu["QJ"].Cast<CheckBox>().CurrentValue)
                {
                    if (Player.GetSpell(SpellSlot.Q).Name == "JavelinToss" && ObjectManager.Player.BaseAbilityDamage + ObjectManager.Player.GetSpellDamage(mob, SpellSlot.Q) > mob.TotalShieldHealth()
                        && Q.IsInRange(mob))
                    {
                        Q.Cast(mob.Position);
                        return;
                    }
                }
            }
        }
    }
}
