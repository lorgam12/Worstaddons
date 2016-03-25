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
        protected static SpellBase Spells => SpellManager.CurrentSpells;

        public static void KS()
        {
            var champion = ObjectManager.Player.ChampionName;
            foreach (AIHeroClient target in
                ObjectManager.Get<AIHeroClient>()
                    .Where(
                        hero =>
                        hero != null && hero.IsHPBarRendered && !hero.HasBuffOfType(BuffType.Invulnerability)
                        && hero.IsValid && hero.IsVisible && hero.IsEnemy && !hero.IsDead && !hero.IsZombie
                        && Program.KillStealMenu[champion + "Steal" + hero.BaseSkinName].Cast<CheckBox>().CurrentValue))
            {
                if (Program.KillStealMenu[champion + "AAC"].Cast<CheckBox>().CurrentValue)
                {
                    if (ObjectManager.Player.CanAttack
                        && ObjectManager.Player.GetAutoAttackDamage(target) > target.Health
                        && target.IsInAutoAttackRange(ObjectManager.Player))
                    {
                        Player.IssueOrder(GameObjectOrder.AttackUnit, target);
                        return;
                    }
                }

                if (Program.KillStealMenu[champion + "QC"].Cast<CheckBox>().CurrentValue)
                {
                    var qtraveltime = target.Distance(ObjectManager.Player) / Spells.Q.Handle.SData.MissileSpeed + Spells.Q.CastDelay + Game.Ping / 2f / 1000;
                    if (Spells.QisToggle)
                    {
                        return;
                    }
                    if (ObjectManager.Player.BaseAbilityDamage + ObjectManager.Player.GetAutoAttackDamage(target)
                        + ObjectManager.Player.GetSpellDamage(target, SpellSlot.Q)
                        > Prediction.Health.GetPrediction(target, (int)qtraveltime)
                        && Spells.Q.IsInRange(target) && Spells.Q.IsReady())
                    {
                        if (Spells.Q.GetType() == typeof(Spell.Skillshot))
                        {
                            Spell.Skillshot Qx = Spells.Q as Spell.Skillshot;
                            if (Qx != null)
                            {
                                Qx.GetPrediction(target);
                                Qx.Cast(target);
                            }
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

                if (Program.KillStealMenu[champion + "WC"].Cast<CheckBox>().CurrentValue)
                {
                    var wtraveltime = target.Distance(ObjectManager.Player) / Spells.W.Handle.SData.MissileSpeed + Spells.W.CastDelay + Game.Ping / 2f / 1000;
                    if (Spells.WisToggle)
                    {
                        return;
                    }
                    if (ObjectManager.Player.BaseAbilityDamage + ObjectManager.Player.GetAutoAttackDamage(target)
                        + ObjectManager.Player.GetSpellDamage(target, SpellSlot.W)
                        > Prediction.Health.GetPrediction(target, (int)(wtraveltime))
                        && Spells.W.IsInRange(target) && Spells.W.IsReady())
                    {
                        if (Spells.W.GetType() == typeof(Spell.Skillshot))
                        {
                            Spell.Skillshot Wx = Spells.W as Spell.Skillshot;
                            if (Wx != null)
                            {
                                Wx.GetPrediction(target);
                                Wx.Cast(target);
                            }
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

                            if (Wx != null && Wx.Range == Wx.MaximumRange)
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

                if (Program.KillStealMenu[champion + "EC"].Cast<CheckBox>().CurrentValue)
                {
                    var etraveltime = target.Distance(ObjectManager.Player) / Spells.E.Handle.SData.MissileSpeed + Spells.E.CastDelay + Game.Ping / 2f / 1000;
                    if (Spells.EisToggle)
                    {
                        return;
                    }
                    if (ObjectManager.Player.BaseAbilityDamage + ObjectManager.Player.GetAutoAttackDamage(target)
                        + ObjectManager.Player.GetSpellDamage(target, SpellSlot.E)
                        > Prediction.Health.GetPrediction(target, (int)(etraveltime))
                        && Spells.E.IsInRange(target) && Spells.E.IsReady())
                    {
                        if (Spells.E.GetType() == typeof(Spell.Skillshot))
                        {
                            Spell.Skillshot Ex = Spells.E as Spell.Skillshot;
                            if (Ex != null)
                            {
                                Ex.GetPrediction(target);
                                Ex.Cast(target);
                            }
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

                            if (Ex != null && Ex.Range == Ex.MaximumRange)
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

                if (Program.KillStealMenu[champion + "RC"].Cast<CheckBox>().CurrentValue)
                {
                    var rtraveltime = target.Distance(ObjectManager.Player) / Spells.R.Handle.SData.MissileSpeed + Spells.R.CastDelay + Game.Ping / 2f / 1000;
                    if (Spells.RisToggle)
                    {
                        return;
                    }
                    if (ObjectManager.Player.BaseAbilityDamage + ObjectManager.Player.GetAutoAttackDamage(target)
                        + ObjectManager.Player.GetSpellDamage(target, SpellSlot.R)
                        > Prediction.Health.GetPrediction(target, (int)(rtraveltime))
                        && Spells.R.IsInRange(target) && Spells.R.IsReady())
                    {
                        if (Spells.R.GetType() == typeof(Spell.Skillshot))
                        {
                            Spell.Skillshot rx = Spells.R as Spell.Skillshot;
                            if (rx != null)
                            {
                                rx.GetPrediction(target);
                                rx.Cast(target);
                            }
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

                            if (Rx != null && Rx.Range == Rx.MaximumRange)
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