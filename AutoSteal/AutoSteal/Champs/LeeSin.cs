namespace AutoSteal.Champs
{
    using System.Linq;
    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Menu.Values;

    class LeeSin
    {
        public static Spell.Skillshot Q { get; set; }
        public static Spell.Active E { get; set; }
        public static Spell.Targeted R { get; set; }
        public static Menu LeeMenu { get; set; }


        public static void KS()
        {
            foreach (AIHeroClient target in
                ObjectManager.Get<AIHeroClient>()
                    .Where(
                        hero =>
                        hero.IsValidTarget(Q.Range)
                        && !hero.HasBuffOfType(BuffType.Invulnerability)
                        && hero.IsEnemy
                        && !hero.IsDead
                        && !hero.IsZombie))
            {

                if (LeeMenu["QC"].Cast<CheckBox>().CurrentValue)
                {
                    if (Player.GetSpell(SpellSlot.Q).Name == "SonicWave" && ObjectManager.Player.GetAutoAttackDamage(target) + ObjectManager.Player.GetSpellDamage(target, SpellSlot.Q) > Check.HealthPrediction.GetHealthPrediction(target, (int)(Q.CastDelay * 1000))
                        && Q.IsInRange(target) && Q.IsReady())
                    {
                        var pred = Q.GetPrediction(target);
                        Q.Cast(pred.CastPosition);
                        return;
                    }
                }

                if (LeeMenu["EC"].Cast<CheckBox>().CurrentValue)
                {
                    if (ObjectManager.Player.GetAutoAttackDamage(target) + ObjectManager.Player.GetSpellDamage(target, SpellSlot.E) > Check.HealthPrediction.GetHealthPrediction(target, (int)(E.CastDelay * 1000))
                        && E.IsInRange(target) && E.IsReady())
                    {
                        E.Cast();
                        return;
                    }
                }

                if (LeeMenu["RC"].Cast<CheckBox>().CurrentValue)
                {
                    if (ObjectManager.Player.GetAutoAttackDamage(target) + ObjectManager.Player.GetSpellDamage(target, SpellSlot.R) > Check.HealthPrediction.GetHealthPrediction(target, (int)(R.CastDelay * 1000))
                        && R.IsInRange(target) && R.IsReady())
                    {
                        R.Cast(target);
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
                        jmob.IsValidTarget(Q.Range)
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
                if (LeeMenu["QJ"].Cast<CheckBox>().CurrentValue)
                {
                    if (Player.GetSpell(SpellSlot.Q).Name == "SonicWave" && ObjectManager.Player.GetAutoAttackDamage(mob) + ObjectManager.Player.GetSpellDamage(mob, SpellSlot.Q) > Check.HealthPrediction.GetHealthPrediction(mob, (int)(Q.CastDelay * 1000))
                        && Q.IsInRange(mob) && Q.IsReady())
                    {
                        Q.Cast(mob.Position);
                        return;
                    }
                }

                if (LeeMenu["EJ"].Cast<CheckBox>().CurrentValue)
                {
                    if (ObjectManager.Player.GetAutoAttackDamage(mob) + ObjectManager.Player.GetSpellDamage(mob, SpellSlot.E) > Check.HealthPrediction.GetHealthPrediction(mob, (int)(E.CastDelay * 1000))
                        && E.IsInRange(mob) && E.IsReady())
                    {
                        E.Cast();
                        return;
                    }
                }
            }
        }
    }
}
