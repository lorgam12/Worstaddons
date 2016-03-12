namespace AutoSteal.Modes
{
    using System.Linq;

    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Menu.Values;

    using GenesisSpellLibrary;
    using GenesisSpellLibrary.Spells;

    internal class KillSteal
    {
        protected static SpellBase Spells
        {
            get
            {
                return SpellManager.CurrentSpells;
            }
        }

        public static void KS()
        {
            foreach (AIHeroClient target in
                ObjectManager.Get<AIHeroClient>()
                    .Where(
                        hero =>
                        hero != null && hero.IsHPBarRendered && !hero.HasBuffOfType(BuffType.Invulnerability) && hero.IsValid && hero.IsVisible
                        && hero.IsEnemy && !hero.IsDead && !hero.IsZombie
                        && Program.KillStealMenu["Steal" + hero.BaseSkinName].Cast<CheckBox>().CurrentValue))
            {
                if (Program.KillStealMenu["AAC"].Cast<CheckBox>().CurrentValue)
                {
                    if (ObjectManager.Player.CanAttack
                        && ObjectManager.Player.GetAutoAttackDamage(target) > target.Health
                        && target.IsInAutoAttackRange(ObjectManager.Player))
                    {
                        Player.IssueOrder(GameObjectOrder.AutoAttack, target);
                        return;
                    }
                }

                if (Program.KillStealMenu["QC"].Cast<CheckBox>().CurrentValue)
                {
                    if (Spells.QisToggle)
                    {
                        return;
                    }
                    if (ObjectManager.Player.BaseAbilityDamage + ObjectManager.Player.GetAutoAttackDamage(target)
                        + ObjectManager.Player.GetSpellDamage(target, SpellSlot.Q)
                        > Check.HealthPrediction.GetHealthPrediction(target, (int)(Spells.Q.CastDelay * 1000))
                        && Spells.Q.IsInRange(target) && Spells.Q.IsReady())
                    {
                        if (Spells.Q.GetType() == typeof(Spell.Skillshot))
                        {
                            Spell.Skillshot Qx = Spells.Q as Spell.Skillshot;
                            Qx.GetPrediction(target);
                            Qx.Cast(target);
                            return;
                        }

                        if (Spells.Q.GetType() == typeof(Spell.Active))
                        {
                            Spells.Q.Cast(target);
                            return;
                        }

                        if (Spells.Q.GetType() == typeof(Spell.Chargeable))
                        {
                            Spell.Chargeable Qx = Spells.Q as Spell.Chargeable;
                            if (Qx != null && !Qx.IsCharging)
                            {
                                Qx.StartCharging();
                            }

                            if (Qx.Range == Qx.MaximumRange)
                            {
                                Qx.Cast(target.Position);
                            }
                            return;
                        }

                        if (Spells.Q.GetType() == typeof(Spell.Ranged))
                        {
                            Spells.Q.Cast(target);
                            return;
                        }
                    }
                }

                if (Program.KillStealMenu["WC"].Cast<CheckBox>().CurrentValue)
                {
                    if (Spells.WisToggle)
                    {
                        return;
                    }
                    if (ObjectManager.Player.BaseAbilityDamage + ObjectManager.Player.GetAutoAttackDamage(target)
                        + ObjectManager.Player.GetSpellDamage(target, SpellSlot.W)
                        > Check.HealthPrediction.GetHealthPrediction(target, (int)(Spells.W.CastDelay * 1000))
                        && Spells.W.IsInRange(target) && Spells.W.IsReady())
                    {
                        if (Spells.W.GetType() == typeof(Spell.Skillshot))
                        {
                            Spell.Skillshot Wx = Spells.W as Spell.Skillshot;
                            Wx.GetPrediction(target);
                            Wx.Cast(target);
                            return;
                        }

                        if (Spells.W.GetType() == typeof(Spell.Targeted))
                        {
                            Spells.W.Cast(target);
                            return;
                        }

                        if (Spells.W.GetType() == typeof(Spell.Active))
                        {
                            Spells.W.Cast();
                            return;
                        }

                        if (Spells.W.GetType() == typeof(Spell.Chargeable))
                        {
                            Spell.Chargeable Wx = Spells.W as Spell.Chargeable;
                            if (Wx != null && !Wx.IsCharging)
                            {
                                Wx.StartCharging();
                            }

                            if (Wx.Range == Wx.MaximumRange)
                            {
                                Wx.Cast(target.Position);
                            }
                            return;
                        }

                        if (Spells.W.GetType() == typeof(Spell.Ranged))
                        {
                            Spells.W.Cast(target);
                            return;
                        }
                    }
                }

                if (Program.KillStealMenu["EC"].Cast<CheckBox>().CurrentValue)
                {
                    if (Spells.EisToggle)
                    {
                        return;
                    }
                    if (ObjectManager.Player.BaseAbilityDamage + ObjectManager.Player.GetAutoAttackDamage(target)
                        + ObjectManager.Player.GetSpellDamage(target, SpellSlot.E)
                        > Check.HealthPrediction.GetHealthPrediction(target, (int)(Spells.E.CastDelay * 1000))
                        && Spells.E.IsInRange(target) && Spells.E.IsReady())
                    {
                        if (Spells.E.GetType() == typeof(Spell.Skillshot))
                        {
                            Spell.Skillshot Ex = Spells.E as Spell.Skillshot;
                            Ex.GetPrediction(target);
                            Ex.Cast(target);
                            return;
                        }

                        if (Spells.E.GetType() == typeof(Spell.Targeted))
                        {
                            Spells.E.Cast(target);
                            return;
                        }

                        if (Spells.E.GetType() == typeof(Spell.Active))
                        {
                            Spells.E.Cast();
                            return;
                        }

                        if (Spells.E.GetType() == typeof(Spell.Chargeable))
                        {
                            Spell.Chargeable Ex = Spells.E as Spell.Chargeable;
                            if (Ex != null && !Ex.IsCharging)
                            {
                                Ex.StartCharging();
                            }

                            if (Ex.Range == Ex.MaximumRange)
                            {
                                Ex.Cast(target.Position);
                            }
                            return;
                        }

                        if (Spells.E.GetType() == typeof(Spell.Ranged))
                        {
                            Spells.E.Cast(target.Position);
                            return;
                        }
                    }
                }

                if (Program.KillStealMenu["RC"].Cast<CheckBox>().CurrentValue)
                {
                    if (Spells.RisToggle)
                    {
                        return;
                    }
                    if (ObjectManager.Player.BaseAbilityDamage + ObjectManager.Player.GetAutoAttackDamage(target)
                        + ObjectManager.Player.GetSpellDamage(target, SpellSlot.R)
                        > Check.HealthPrediction.GetHealthPrediction(target, (int)(Spells.R.CastDelay * 1000))
                        && Spells.R.IsInRange(target) && Spells.R.IsReady())
                    {
                        if (Spells.R.GetType() == typeof(Spell.Skillshot))
                        {
                            Spell.Skillshot Rx = Spells.R as Spell.Skillshot;
                            Rx.GetPrediction(target);
                            Rx.Cast(target);
                            return;
                        }

                        if (Spells.R.GetType() == typeof(Spell.Targeted))
                        {
                            Spells.R.Cast(target);
                            return;
                        }

                        if (Spells.R.GetType() == typeof(Spell.Active))
                        {
                            Spells.R.Cast();
                            return;
                        }

                        if (Spells.R.GetType() == typeof(Spell.Chargeable))
                        {
                            Spell.Chargeable Rx = Spells.R as Spell.Chargeable;
                            if (Rx != null && !Rx.IsCharging)
                            {
                                Rx.StartCharging();
                            }

                            if (Rx.Range == Rx.MaximumRange)
                            {
                                Rx.Cast(target.Position);
                            }
                            return;
                        }

                        if (Spells.R.GetType() == typeof(Spell.Ranged))
                        {
                            Spells.R.Cast(target.Position);
                            return;
                        }
                    }
                }
            }
        }
    }
}