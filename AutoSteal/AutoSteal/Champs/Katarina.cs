namespace AutoSteal.Champs
{
    using System.Linq;
    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Menu.Values;

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
                        && hero.IsEnemy
                        && !hero.IsDead 
                        && !hero.IsZombie))
            {

                if (KataMenu["QC"].Cast<CheckBox>().CurrentValue)
                {
                    if (ObjectManager.Player.BaseAbilityDamage + ObjectManager.Player.GetSpellDamage(target, SpellSlot.Q) > Check.HealthPrediction.GetHealthPrediction(target, (int)(Q.CastDelay * 1000))
                        && Q.IsInRange(target) && Q.IsReady())
                    {
                        Q.Cast(target);
                        return;
                    }
                }

                if (KataMenu["WC"].Cast<CheckBox>().CurrentValue)
                {
                    if (ObjectManager.Player.BaseAbilityDamage + ObjectManager.Player.GetSpellDamage(target, SpellSlot.W) > Check.HealthPrediction.GetHealthPrediction(target, (int)(W.CastDelay * 1000))
                        && W.IsInRange(target) && W.IsReady())
                    {
                        W.Cast();
                        return;
                    }
                }

                if (KataMenu["EC"].Cast<CheckBox>().CurrentValue)
                {
                    if (ObjectManager.Player.BaseAbilityDamage + ObjectManager.Player.GetSpellDamage(target, SpellSlot.E) > Check.HealthPrediction.GetHealthPrediction(target, (int)(E.CastDelay * 1000))
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
                        jmob.IsValidTarget(E.Range)
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

                if (KataMenu["QJ"].Cast<CheckBox>().CurrentValue)
                {
                    if (ObjectManager.Player.BaseAbilityDamage + ObjectManager.Player.GetSpellDamage(mob, SpellSlot.Q) > Check.HealthPrediction.GetHealthPrediction(mob, (int)(Q.CastDelay * 1000))
                        && Q.IsInRange(mob) && Q.IsReady())
                    {
                        Q.Cast(mob);
                        return;
                    }
                }

                if (KataMenu["WJ"].Cast<CheckBox>().CurrentValue)
                {
                    if (ObjectManager.Player.BaseAbilityDamage + ObjectManager.Player.GetSpellDamage(mob, SpellSlot.W) > Check.HealthPrediction.GetHealthPrediction(mob, (int)(W.CastDelay * 1000))
                        && W.IsInRange(mob) && W.IsReady())
                    {
                        W.Cast();
                        return;
                    }
                }

                if (KataMenu["EJ"].Cast<CheckBox>().CurrentValue)
                {
                    if (ObjectManager.Player.BaseAbilityDamage + ObjectManager.Player.GetSpellDamage(mob, SpellSlot.E) > Check.HealthPrediction.GetHealthPrediction(mob, (int)(E.CastDelay * 1000))
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
