namespace AutoSteal.Modes
{
    using System.Linq;

    using Genesis.Library;
    using Genesis.Library.Spells;

    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Menu.Values;

    internal class JungleSteal
    {
        public static Obj_AI_Minion Mobxdd;

        protected static SpellBase Spells => SpellManager.CurrentSpells;

        public static void Js()
        {
            var champion = ObjectManager.Player.ChampionName;
            foreach (Obj_AI_Minion mob in
                ObjectManager.Get<Obj_AI_Minion>()
                    .Where(
                        jmob =>
                        !jmob.HasBuffOfType(BuffType.Invulnerability) && jmob.IsMonster && jmob.IsValid
                        && jmob.IsVisible && !jmob.IsDead && !jmob.IsZombie
                        && ((Program.JungleStealMenu[champion + "drake"].Cast<CheckBox>().CurrentValue
                             && jmob.BaseSkinName == "SRU_Dragon")
                            || (Program.JungleStealMenu[champion + "baron"].Cast<CheckBox>().CurrentValue
                                && jmob.BaseSkinName == "SRU_Baron")
                            || (Program.JungleStealMenu[champion + "gromp"].Cast<CheckBox>().CurrentValue
                                && jmob.BaseSkinName == "SRU_Gromp")
                            || (Program.JungleStealMenu[champion + "krug"].Cast<CheckBox>().CurrentValue
                                && jmob.BaseSkinName == "SRU_Krug")
                            || (Program.JungleStealMenu[champion + "razorbeak"].Cast<CheckBox>().CurrentValue
                                && jmob.BaseSkinName == "SRU_Razorbeak")
                            || (Program.JungleStealMenu[champion + "crab"].Cast<CheckBox>().CurrentValue
                                && jmob.BaseSkinName == "Sru_Crab")
                            || (Program.JungleStealMenu[champion + "murkwolf"].Cast<CheckBox>().CurrentValue
                                && jmob.BaseSkinName == "SRU_Murkwolf")
                            || (Program.JungleStealMenu[champion + "blue"].Cast<CheckBox>().CurrentValue
                                && jmob.BaseSkinName == "SRU_Blue")
                            || (Program.JungleStealMenu[champion + "red"].Cast<CheckBox>().CurrentValue
                                && jmob.BaseSkinName == "SRU_Red"))))
            {
                Mobxdd = mob;
                if (Program.JungleStealMenu[champion + "AAJ"].Cast<CheckBox>().CurrentValue)
                {
                    if (ObjectManager.Player.CanAttack && ObjectManager.Player.GetAutoAttackDamage(mob) > mob.Health
                        && mob.IsInAutoAttackRange(ObjectManager.Player))
                    {
                        Player.IssueOrder(GameObjectOrder.AttackUnit, mob);
                        return;
                    }
                }

                if (Program.JungleStealMenu[champion + "QJ"].Cast<CheckBox>().CurrentValue)
                {
                    if (Spells.QisToggle || Spells.QisDash || Spells.QisCc || Spells.Q == null)
                    {
                        return;
                    }

                    var traveltime = mob.Distance(ObjectManager.Player)
                                     / (Spells.Q.Handle.SData.MissileSpeed + Spells.Q.CastDelay) + Game.Ping / 2f / 1000;

                    if (KillSteal.Playerdamage + ObjectManager.Player.GetSpellDamage(mob, SpellSlot.Q)
                        > Prediction.Health.GetPrediction(mob, (int)traveltime) && Spells.Q.IsInRange(mob)
                        && Spells.Q.IsReady())
                    {
                        if (Spells.Q.GetType() == typeof(Spell.Skillshot))
                        {
                            var qx = Spells.Q as Spell.Skillshot;
                            qx?.GetPrediction(mob);
                            qx?.Cast(mob);
                            return;
                        }

                        if (Spells.Q.GetType() == typeof(Spell.Targeted))
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
                            var qx = Spells.Q as Spell.Chargeable;
                            if (qx != null && !qx.IsCharging)
                            {
                                qx.StartCharging();
                            }

                            if (qx != null && qx.Range == qx.MaximumRange)
                            {
                                qx.Cast(mob.Position);
                            }

                            return;
                        }

                        if (Spells.Q.GetType() == typeof(Spell.Ranged))
                        {
                            Spells.Q.Cast(mob.Position);
                            return;
                        }
                    }
                }

                if (Program.JungleStealMenu[champion + "WJ"].Cast<CheckBox>().CurrentValue)
                {
                    if (Spells.WisToggle || Spells.WisDash || Spells.WisCc || Spells.W == null)
                    {
                        return;
                    }

                    var traveltime = mob.Distance(ObjectManager.Player)
                                     / (Spells.W.Handle.SData.MissileSpeed + Spells.W.CastDelay) + Game.Ping / 2f / 1000;

                    if (KillSteal.Playerdamage + ObjectManager.Player.GetSpellDamage(mob, SpellSlot.W)
                        > Prediction.Health.GetPrediction(mob, (int)traveltime) && Spells.W.IsInRange(mob)
                        && Spells.W.IsReady())
                    {
                        if (Spells.W.GetType() == typeof(Spell.Skillshot))
                        {
                            var wx = Spells.W as Spell.Skillshot;
                            wx?.GetPrediction(mob);
                            wx?.Cast(mob);
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
                            var wx = Spells.W as Spell.Chargeable;
                            if (wx != null && !wx.IsCharging)
                            {
                                wx.StartCharging();
                            }

                            if (wx != null && wx.Range == wx.MaximumRange)
                            {
                                wx.Cast(mob.Position);
                            }
                            return;
                        }

                        if (Spells.W.GetType() == typeof(Spell.Ranged))
                        {
                            Spells.W.Cast(mob.Position);
                            return;
                        }
                    }
                }

                if (Program.JungleStealMenu[champion + "EJ"].Cast<CheckBox>().CurrentValue)
                {
                    if (Spells.EisToggle || Spells.EisDash || Spells.EisCc || Spells.E == null)
                    {
                        return;
                    }

                    var traveltime = mob.Distance(ObjectManager.Player)
                                     / (Spells.E.Handle.SData.MissileSpeed + Spells.E.CastDelay) + Game.Ping / 2f / 1000;

                    if (KillSteal.Playerdamage + ObjectManager.Player.GetSpellDamage(mob, SpellSlot.E)
                        > Prediction.Health.GetPrediction(mob, (int)traveltime) && Spells.E.IsInRange(mob)
                        && Spells.E.IsReady())
                    {
                        if (Spells.E.GetType() == typeof(Spell.Skillshot))
                        {
                            var ex = Spells.E as Spell.Skillshot;
                            ex?.GetPrediction(mob);
                            ex?.Cast(mob);
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
                            var ex = Spells.E as Spell.Chargeable;
                            if (ex != null && !ex.IsCharging)
                            {
                                ex.StartCharging();
                            }

                            if (ex != null && ex.Range == ex.MaximumRange)
                            {
                                ex.Cast(mob.Position);
                            }
                            return;
                        }

                        if (Spells.E.GetType() == typeof(Spell.Ranged))
                        {
                            Spells.E.Cast(mob.Position);
                            return;
                        }
                    }
                }

                if (Program.JungleStealMenu[champion + "RJ"].Cast<CheckBox>().CurrentValue)
                {
                    if (Spells.RisToggle || Spells.RisDash || Spells.RisCc || Spells.R == null)
                    {
                        return;
                    }

                    var traveltime = mob.Distance(ObjectManager.Player)
                                     / (Spells.R.Handle.SData.MissileSpeed + Spells.R.CastDelay) + Game.Ping / 2f / 1000;

                    if (KillSteal.Playerdamage + ObjectManager.Player.GetSpellDamage(mob, SpellSlot.R)
                        > Prediction.Health.GetPrediction(mob, (int)traveltime) && Spells.R.IsInRange(mob)
                        && Spells.R.IsReady())
                    {
                        if (Spells.R.GetType() == typeof(Spell.Skillshot))
                        {
                            var rx = Spells.R as Spell.Skillshot;
                            rx?.GetPrediction(mob);
                            rx?.Cast(mob);
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
                            var rx = Spells.R as Spell.Chargeable;
                            if (rx != null && !rx.IsCharging)
                            {
                                rx.StartCharging();
                            }

                            if (rx != null && rx.Range == rx.MaximumRange)
                            {
                                rx.Cast(mob.Position);
                            }
                            return;
                        }

                        if (Spells.R.GetType() == typeof(Spell.Ranged))
                        {
                            Spells.R.Cast(mob.Position);
                            return;
                        }
                    }

                    if (Program.JungleStealMenu[champion + "all"].Cast<CheckBox>().CurrentValue
                        && KillSteal.Playerdamage + Misc.Damage.JsCalcDamage(mob)
                        >= Prediction.Health.GetPrediction(mob, (int)(Misc.Damage.JsTravelTime(mob))))
                    {

                    }
                }
            }
        }
    }
}