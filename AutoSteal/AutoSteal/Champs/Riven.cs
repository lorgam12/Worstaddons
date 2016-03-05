namespace AutoSteal.Champs
{
    using System.Linq;
    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Menu.Values;

    internal class Riven
    {

        public static Spell.Active W { get; set; }

        public static Spell.Skillshot R { get; set; }

        public static Menu RivenMenu { get; set; }


        public static void KS()
        {
            foreach (AIHeroClient target in
                ObjectManager.Get<AIHeroClient>()
                    .Where(
                        hero =>
                        hero.IsValidTarget(R.Range)
                        && !hero.HasBuffOfType(BuffType.Invulnerability)
                        && hero.IsEnemy
                        && !hero.IsDead
                        && !hero.IsZombie))
            {
                if (RivenMenu["WC"].Cast<CheckBox>().CurrentValue)
                {
                    if (ObjectManager.Player.GetAutoAttackDamage(target)
                        + ObjectManager.Player.GetSpellDamage(target, SpellSlot.W) > Check.HealthPrediction.GetHealthPrediction(target, (int)(W.CastDelay * 1000))
                        && W.IsInRange(target) && W.IsReady())
                    {
                        W.Cast();
                        return;
                    }
                }

                if (RivenMenu["RC"].Cast<CheckBox>().CurrentValue)
                {
                    if (ObjectManager.Player.GetAutoAttackDamage(target) + ObjectManager.Player.GetSpellDamage(target, SpellSlot.R) > Check.HealthPrediction.GetHealthPrediction(target, (int)(R.CastDelay * 1000))
                        && R.IsInRange(target) && R.IsReady())
                    {
                        var pred = R.GetPrediction(target);
                        R.Cast(pred.CastPosition);
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
                        jmob.IsValidTarget(R.Range)
                        && !jmob.HasBuffOfType(BuffType.Invulnerability)
                        && jmob.IsMonster
                        && !jmob.IsDead
                        && !jmob.IsZombie
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
                if (RivenMenu["WJ"].Cast<CheckBox>().CurrentValue)
                {
                    if (ObjectManager.Player.GetAutoAttackDamage(mob) + ObjectManager.Player.GetSpellDamage(mob, SpellSlot.W) > Check.HealthPrediction.GetHealthPrediction(mob, (int)(W.CastDelay * 1000))
                        && W.IsInRange(mob))
                    {
                        W.Cast();
                        return;
                    }
                }
                if (RivenMenu["RJ"].Cast<CheckBox>().CurrentValue)
                {
                    if (ObjectManager.Player.GetAutoAttackDamage(mob) + ObjectManager.Player.GetSpellDamage(mob, SpellSlot.R) > Check.HealthPrediction.GetHealthPrediction(mob, (int)(R.CastDelay * 1000))
                        && R.IsInRange(mob))
                    {
                        R.Cast(mob.Position);
                        return;
                    }
                }
            }
        }
    }
}
