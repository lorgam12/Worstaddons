namespace AutoSteal.Modes
{
    using System.Linq;
    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Menu.Values;
    using GenesisSpellLibrary;
    using GenesisSpellLibrary.Spells;

    internal class JungleSteal
    {


        protected static SpellBase Spells
        {
            get
            {
                return SpellManager.CurrentSpells;
            }
        }

        public static void JS()
        {
            foreach (Obj_AI_Minion mob in
                ObjectManager.Get<Obj_AI_Minion>()
                    .Where(
                        jmob =>
                        !jmob.HasBuffOfType(BuffType.Invulnerability)
                        && jmob.IsMonster
                        && !jmob.IsDead
                        && !jmob.IsZombie
                        && ((Program.JungleStealMenu["drake"].Cast<CheckBox>().CurrentValue
                             && jmob.BaseSkinName == "SRU_Dragon")
                            || (Program.JungleStealMenu["baron"].Cast<CheckBox>().CurrentValue
                                && jmob.BaseSkinName == "SRU_Baron")
                            || (Program.JungleStealMenu["gromp"].Cast<CheckBox>().CurrentValue
                                && jmob.BaseSkinName == "SRU_Gromp")
                            || (Program.JungleStealMenu["krug"].Cast<CheckBox>().CurrentValue
                                && jmob.BaseSkinName == "SRU_Krug")
                            || (Program.JungleStealMenu["razorbeak"].Cast<CheckBox>().CurrentValue
                                && jmob.BaseSkinName == "SRU_Razorbeak")
                            || (Program.JungleStealMenu["crab"].Cast<CheckBox>().CurrentValue
                                && jmob.BaseSkinName == "Sru_Crab")
                            || (Program.JungleStealMenu["murkwolf"].Cast<CheckBox>().CurrentValue
                                && jmob.BaseSkinName == "SRU_Murkwolf")
                            || (Program.JungleStealMenu["blue"].Cast<CheckBox>().CurrentValue
                                && jmob.BaseSkinName == "SRU_Blue")
                            || (Program.JungleStealMenu["red"].Cast<CheckBox>().CurrentValue && jmob.BaseSkinName == "SRU_Red")))
                )
            {
                if (Program.JungleStealMenu["AAJ"].Cast<CheckBox>().CurrentValue)
                {
                    if (ObjectManager.Player.CanAttack
                        && ObjectManager.Player.GetAutoAttackDamage(mob) > mob.Health
                        && mob.IsInAutoAttackRange(ObjectManager.Player))
                    {
                        Player.IssueOrder(GameObjectOrder.AutoAttack, mob);
                        return;
                    }
                }

                if (Program.JungleStealMenu["QJ"].Cast<CheckBox>().CurrentValue)
                {
                    if (ObjectManager.Player.BaseAbilityDamage
                        + ObjectManager.Player.GetAutoAttackDamage(mob)
                        + ObjectManager.Player.GetSpellDamage(mob, SpellSlot.Q)
                        > Check.HealthPrediction.GetHealthPrediction(mob, (int)(Spells.Q.CastDelay * 1000))
                        && Spells.Q.IsInRange(mob)
                        && Spells.Q.IsReady())
                    {
                        if (Spells.Q.GetType() == typeof(Spell.Skillshot))
                        {
                            Spell.Skillshot Qx = Spells.Q as Spell.Skillshot;
                            Qx.GetPrediction(mob);
                            Qx.Cast(mob);
                            return;
                        }

                        if (Spells.Q.GetType() == typeof(Spell.Active))
                        {
                            Spells.Q.Cast(mob);
                            return;
                        }

                        if (Spells.Q.GetType() == typeof(Spell.Active))
                        {
                            Spells.Q.Cast();
                            return;
                        }

                        if (Spells.Q.GetType() == typeof(Spell.Chargeable))
                        {
                            Spells.Q.Cast(mob);
                            return;
                        }
                    }
                }

                if (Program.JungleStealMenu["WJ"].Cast<CheckBox>().CurrentValue)
                {
                    if (ObjectManager.Player.BaseAbilityDamage
                        + ObjectManager.Player.GetAutoAttackDamage(mob)
                        + ObjectManager.Player.GetSpellDamage(mob, SpellSlot.W)
                        > Check.HealthPrediction.GetHealthPrediction(mob, (int)(Spells.W.CastDelay * 1000))
                        && Spells.W.IsInRange(mob)
                        && Spells.W.IsReady())
                    {
                        if (Spells.W.GetType() == typeof(Spell.Skillshot))
                        {
                            Spell.Skillshot Wx = Spells.W as Spell.Skillshot;
                            Wx.GetPrediction(mob);
                            Wx.Cast(mob);
                            return;
                        }

                        if (Spells.W.GetType() == typeof(Spell.Targeted))
                        {
                            Spells.W.Cast(mob);
                            return;
                        }

                        if (Spells.W.GetType() == typeof(Spell.Active))
                        {
                            Spells.W.Cast();
                            return;
                        }

                        if (Spells.W.GetType() == typeof(Spell.Chargeable))
                        {
                            Spells.W.Cast(mob);
                            return;
                        }
                    }
                }

                if (Program.JungleStealMenu["EJ"].Cast<CheckBox>().CurrentValue)
                {
                    if (ObjectManager.Player.BaseAbilityDamage
                        + ObjectManager.Player.GetAutoAttackDamage(mob)
                        + ObjectManager.Player.GetSpellDamage(mob, SpellSlot.E)
                        > Check.HealthPrediction.GetHealthPrediction(mob, (int)(Spells.E.CastDelay * 1000))
                        && Spells.E.IsInRange(mob)
                        && Spells.E.IsReady())
                    {
                        if (Spells.E.GetType() == typeof(Spell.Skillshot))
                        {
                            Spell.Skillshot Ex = Spells.E as Spell.Skillshot;
                            Ex.GetPrediction(mob);
                            Ex.Cast(mob);
                            return;
                        }

                        if (Spells.E.GetType() == typeof(Spell.Targeted))
                        {
                            Spells.E.Cast(mob);
                            return;
                        }

                        if (Spells.E.GetType() == typeof(Spell.Active))
                        {
                            Spells.E.Cast();
                            return;
                        }

                        if (Spells.E.GetType() == typeof(Spell.Chargeable))
                        {
                            Spells.E.Cast(mob);
                            return;
                        }
                    }
                }

                if (Program.JungleStealMenu["RJ"].Cast<CheckBox>().CurrentValue)
                {
                    if (ObjectManager.Player.BaseAbilityDamage
                        + ObjectManager.Player.GetAutoAttackDamage(mob)
                        + ObjectManager.Player.GetSpellDamage(mob, SpellSlot.R)
                        > Check.HealthPrediction.GetHealthPrediction(mob, (int)(Spells.R.CastDelay * 1000))
                        && Spells.R.IsInRange(mob)
                        && Spells.R.IsReady())
                    {
                        if (Spells.R.GetType() == typeof(Spell.Skillshot))
                        {
                            Spell.Skillshot Rx = Spells.R as Spell.Skillshot;
                            Rx.GetPrediction(mob);
                            Rx.Cast(mob);
                            return;
                        }

                        if (Spells.R.GetType() == typeof(Spell.Targeted))
                        {
                            Spells.R.Cast(mob);
                            return;
                        }

                        if (Spells.R.GetType() == typeof(Spell.Active))
                        {
                            Spells.R.Cast();
                        return;
                        }

                        if (Spells.R.GetType() == typeof(Spell.Chargeable))
                        {
                            Spells.R.Cast(mob);
                            return;
                        }
                    }
                }
            }
        }
    }
}
