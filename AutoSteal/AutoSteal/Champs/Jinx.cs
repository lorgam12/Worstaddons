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
                        && hero.IsEnemy
                        && !hero.IsDead
                        && !hero.IsZombie))
            {
                if (Jinx.JinxMenu["WC"].Cast<CheckBox>().CurrentValue)
                {
                    if (ObjectManager.Player.GetAutoAttackDamage(target) + ObjectManager.Player.GetSpellDamage(target, SpellSlot.W) > Check.HealthPrediction.GetHealthPrediction(target, (int)(W.CastDelay * 1000))
                        && W.IsInRange(target) && W.IsReady())
                    {
                        var pred = W.GetPrediction(target);
                        W.Cast(pred.CastPosition);
                        return;
                    }
                }

                if (Jinx.JinxMenu["RC"].Cast<CheckBox>().CurrentValue)
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
                        jmob.IsValidTarget(W.Range)
                        && !jmob.HasBuffOfType(BuffType.Invulnerability)
                        && jmob.IsMonster
                        && !jmob.IsDead
                        && !jmob.IsZombie
                        && ((Start.Junglemobs["drake"].Cast<CheckBox>().CurrentValue && jmob.BaseSkinName == "SRU_Dragon")
                        || (Start.Junglemobs["baron"].Cast<CheckBox>().CurrentValue && jmob.BaseSkinName == "SRU_Baron")
                        || (Start.Junglemobs["gromp"].Cast<CheckBox>().CurrentValue && jmob.BaseSkinName == "SRU_Gromp")
                        || (Start.Junglemobs["krug"].Cast<CheckBox>().CurrentValue && jmob.BaseSkinName == "SRU_Krug")
                        || (Start.Junglemobs["razorbeak"].Cast<CheckBox>().CurrentValue && jmob.BaseSkinName == "SRU_Razorbeak")
                        || (Start.Junglemobs["crab"].Cast<CheckBox>().CurrentValue && jmob.BaseSkinName == "Sru_Crab")
                        || (Start.Junglemobs["murkwolf"].Cast<CheckBox>().CurrentValue && jmob.BaseSkinName == "SRU_Murkwolf")
                        || (Start.Junglemobs["blue"].Cast<CheckBox>().CurrentValue && jmob.BaseSkinName == "SRU_Blue")
                        || (Start.Junglemobs["red"].Cast<CheckBox>().CurrentValue && jmob.BaseSkinName == "SRU_Red"))))
            {
                if (Jinx.JinxMenu["WJ"].Cast<CheckBox>().CurrentValue)
                {
                    if (ObjectManager.Player.GetAutoAttackDamage(mob) + ObjectManager.Player.GetSpellDamage(mob, SpellSlot.W) > Check.HealthPrediction.GetHealthPrediction(mob, (int)(W.CastDelay * 1000))
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
