namespace AutoSteal.Modes
{
    using System.Linq;

    using Genesis.Library;
    using Genesis.Library.Spells;

    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Menu.Values;

    internal class KillSteal
    {
        public static int Playerdamage;

        public static AIHeroClient Targetxdd;

        protected static SpellBase Spells => SpellManager.CurrentSpells;

        public static void Ks()
        {
            Playerdamage = (int)(ObjectManager.Player.BaseAbilityDamage + ObjectManager.Player.BaseAttackDamage);
            var champion = ObjectManager.Player.ChampionName;
            foreach (AIHeroClient target in
                ObjectManager.Get<AIHeroClient>()
                    .Where(
                        hero =>
                        hero != null && hero.IsHPBarRendered && !hero.HasBuffOfType(BuffType.Invulnerability)
                        && hero.IsValid && hero.IsVisible && hero.IsEnemy && !hero.IsDead && !hero.IsZombie
                        && Program.KillStealMenu[champion + "Steal" + hero.BaseSkinName].Cast<CheckBox>().CurrentValue))
            {
                Targetxdd = target;
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
                    if (Spells.QisToggle || Spells.QisDash || Spells.QisCc || Spells.Q == null)
                    {
                        return;
                    }
                    var qtraveltime = target.Distance(ObjectManager.Player) / Spells.Q.Handle.SData.MissileSpeed
                                      + (Spells.Q.CastDelay) + Game.Ping / 2f / 1000;
                    if (Playerdamage + ObjectManager.Player.GetSpellDamage(target, SpellSlot.Q)
                        >= Prediction.Health.GetPrediction(target, (int)qtraveltime) && Spells.Q.IsInRange(target)
                        && Spells.Q.IsReady())
                    {
                        if (Spells.Q.GetType() == typeof(Spell.Skillshot))
                        {
                            var qx = Spells.Q as Spell.Skillshot;
                            qx?.GetPrediction(target);
                            qx?.Cast(target);
                            return;
                        }

                        if (Spells.Q.GetType() == typeof(Spell.Active))
                        {
                            Spells.Q.Cast(target);
                            return;
                        }

                        if (Spells.Q.GetType() == typeof(Spell.Chargeable))
                        {
                            var qx = Spells.Q as Spell.Chargeable;
                            if (qx != null && !qx.IsCharging)
                            {
                                qx.StartCharging();
                            }

                            if (qx != null && qx.Range == qx.MaximumRange)
                            {
                                qx.Cast(target.Position);
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
                    if (Spells.WisToggle || Spells.WisDash || Spells.WisCc || Spells.W == null)
                    {
                        return;
                    }
                    var wtraveltime = target.Distance(ObjectManager.Player) / Spells.W.Handle.SData.MissileSpeed
                                      + Spells.W.CastDelay + Game.Ping / 2f / 1000;
                    if (Playerdamage + ObjectManager.Player.GetSpellDamage(target, SpellSlot.W)
                        >= Prediction.Health.GetPrediction(target, (int)(wtraveltime)) && Spells.W.IsInRange(target)
                        && Spells.W.IsReady())
                    {
                        if (Spells.W.GetType() == typeof(Spell.Skillshot))
                        {
                            var wx = Spells.W as Spell.Skillshot;
                            wx?.GetPrediction(target);
                            wx?.Cast(target);
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
                            var wx = Spells.W as Spell.Chargeable;
                            if (wx != null && !wx.IsCharging)
                            {
                                wx.StartCharging();
                            }

                            if (wx != null && wx.Range == wx.MaximumRange)
                            {
                                wx.Cast(target.Position);
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
                    if (Spells.EisToggle || Spells.EisDash || Spells.EisCc || Spells.E == null)
                    {
                        return;
                    }
                    var etraveltime = target.Distance(ObjectManager.Player) / Spells.E.Handle.SData.MissileSpeed
                                      + Spells.E.CastDelay + Game.Ping / 2f / 1000;
                    if (Playerdamage + ObjectManager.Player.GetSpellDamage(target, SpellSlot.E)
                        >= Prediction.Health.GetPrediction(target, (int)(etraveltime)) && Spells.E.IsInRange(target)
                        && Spells.E.IsReady())
                    {
                        if (Spells.E.GetType() == typeof(Spell.Skillshot))
                        {
                            var ex = Spells.E as Spell.Skillshot;
                            ex?.GetPrediction(target);
                            ex?.Cast(target);
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
                            var ex = Spells.E as Spell.Chargeable;
                            if (ex != null && !ex.IsCharging)
                            {
                                ex.StartCharging();
                            }

                            if (ex != null && ex.Range == ex.MaximumRange)
                            {
                                ex.Cast(target.Position);
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
                    if (Spells.RisToggle || Spells.RisDash || Spells.RisCc)
                    {
                        return;
                    }
                    var rtraveltime = target.Distance(ObjectManager.Player) / Spells.R.Handle.SData.MissileSpeed
                                      + Spells.R.CastDelay + Game.Ping / 2f / 1000;
                    if (Playerdamage + ObjectManager.Player.GetSpellDamage(target, SpellSlot.R)
                        >= Prediction.Health.GetPrediction(target, (int)(rtraveltime)) && Spells.R.IsInRange(target)
                        && Spells.R.IsReady())
                    {
                        if (Spells.R.GetType() == typeof(Spell.Skillshot))
                        {
                            var rx = Spells.R as Spell.Skillshot;
                            rx?.GetPrediction(target);
                            rx?.Cast(target);
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
                            var rx = Spells.R as Spell.Chargeable;
                            if (rx != null && !rx.IsCharging)
                            {
                                rx.StartCharging();
                            }

                            if (rx != null && rx.Range == rx.MaximumRange)
                            {
                                rx.Cast(target.Position);
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

                if (Program.KillStealMenu[champion + "all"].Cast<CheckBox>().CurrentValue
                    && Playerdamage + Misc.Damage.KsCalcDamage(target)
                    >= Prediction.Health.GetPrediction(target, (int)(Misc.Damage.KsTravelTime(target))))
                {

                }
            }
        }
    }
}