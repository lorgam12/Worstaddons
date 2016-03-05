namespace AutoSteal.Champs
{
    using System.Linq;

    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Menu.Values;

    class Jinx
    {
        public static Spell.Skillshot W { get; set; }
        public static Spell.Skillshot R { get; set; }
        public static Menu JinxMenu { get; set; }


        public static void KS()
        {
            foreach (AIHeroClient target in
                ObjectManager.Get<AIHeroClient>()
                    .Where(
                        hero =>
                        hero.IsValidTarget(R.Range)
                        && !hero.HasBuffOfType(BuffType.Invulnerability)
                        && hero.IsEnemy))
            {

                if (Jinx.JinxMenu["WC"].Cast<CheckBox>().CurrentValue)
                {
                    if (ObjectManager.Player.GetAutoAttackDamage(target) + ObjectManager.Player.GetSpellDamage(target, SpellSlot.W) > target.TotalShieldHealth()
                        && W.IsInRange(target) && W.IsReady())
                    {
                        var pred = W.GetPrediction(target);
                        W.Cast(pred.CastPosition);
                        return;
                    }
                }

                if (Jinx.JinxMenu["RC"].Cast<CheckBox>().CurrentValue)
                {
                    if (ObjectManager.Player.GetAutoAttackDamage(target) + ObjectManager.Player.GetSpellDamage(target, SpellSlot.R) > target.TotalShieldHealth()
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
                        W.IsInRange(jmob)
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

                if (Jinx.JinxMenu["WJ"].Cast<CheckBox>().CurrentValue)
                {
                    if (ObjectManager.Player.GetAutoAttackDamage(mob) + ObjectManager.Player.GetSpellDamage(mob, SpellSlot.W) > mob.TotalShieldHealth()
                        && W.IsInRange(mob))
                    {
                        W.Cast(mob.Position);
                        return;
                    }
                }
            }
        }
    }
}
