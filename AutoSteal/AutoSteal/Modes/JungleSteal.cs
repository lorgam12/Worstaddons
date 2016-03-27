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

        protected static SpellBase Spells
        {
            get
            {
                return SpellManager.CurrentSpells;
            }
        }

        public static void JS()
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
                    var traveltime = mob.Distance(ObjectManager.Player)
                                     / (Spells.Q.Handle.SData.MissileSpeed + Spells.Q.CastDelay) + Game.Ping / 2f / 1000;
                    if (Spells.QisToggle || Spells.QisDash || Spells.QisCC)
                    {
                        return;
                    }

                    if (KillSteal.playerdamage + ObjectManager.Player.GetSpellDamage(mob, SpellSlot.Q)
                        > Prediction.Health.GetPrediction(mob, (int)traveltime) && Spells.Q.IsInRange(mob)
                        && Spells.Q.IsReady())
                    {
                        if (Spells.Q.GetType() == typeof(Spell.Skillshot))
                        {
                            Spell.Skillshot Qx = Spells.Q as Spell.Skillshot;
                            if (Qx != null)
                            {
                                Qx.GetPrediction(mob);
                                Qx.Cast(mob);
                            }
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
                            Spell.Chargeable Qx = Spells.Q as Spell.Chargeable;
                            if (Qx != null && !Qx.IsCharging)
                            {
                                Qx.StartCharging();
                            }

                            if (Qx != null && Qx.Range == Qx.MaximumRange)
                            {
                                Qx.Cast(mob.Position);
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
                    var traveltime = mob.Distance(ObjectManager.Player)
                                     / (Spells.W.Handle.SData.MissileSpeed + Spells.W.CastDelay) + Game.Ping / 2f / 1000;
                    if (Spells.WisToggle || Spells.WisDash || Spells.WisCC)
                    {
                        return;
                    }
                    if (KillSteal.playerdamage + ObjectManager.Player.GetSpellDamage(mob, SpellSlot.W)
                        > Prediction.Health.GetPrediction(mob, (int)traveltime) && Spells.W.IsInRange(mob)
                        && Spells.W.IsReady())
                    {
                        if (Spells.W.GetType() == typeof(Spell.Skillshot))
                        {
                            Spell.Skillshot Wx = Spells.W as Spell.Skillshot;
                            if (Wx != null)
                            {
                                Wx.GetPrediction(mob);
                                Wx.Cast(mob);
                            }
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
                            Spell.Chargeable Wx = Spells.W as Spell.Chargeable;
                            if (Wx != null && !Wx.IsCharging)
                            {
                                Wx.StartCharging();
                            }

                            if (Wx != null && Wx.Range == Wx.MaximumRange)
                            {
                                Wx.Cast(mob.Position);
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
                    var traveltime = mob.Distance(ObjectManager.Player)
                                     / (Spells.E.Handle.SData.MissileSpeed + Spells.E.CastDelay) + Game.Ping / 2f / 1000;
                    if (Spells.EisToggle || Spells.EisDash || Spells.EisCC)
                    {
                        return;
                    }

                    if (KillSteal.playerdamage + ObjectManager.Player.GetSpellDamage(mob, SpellSlot.E)
                        > Prediction.Health.GetPrediction(mob, (int)traveltime) && Spells.E.IsInRange(mob)
                        && Spells.E.IsReady())
                    {
                        if (Spells.E.GetType() == typeof(Spell.Skillshot))
                        {
                            Spell.Skillshot Ex = Spells.E as Spell.Skillshot;
                            if (Ex != null)
                            {
                                Ex.GetPrediction(mob);
                                Ex.Cast(mob);
                            }
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
                            Spell.Chargeable Ex = Spells.E as Spell.Chargeable;
                            if (Ex != null && !Ex.IsCharging)
                            {
                                Ex.StartCharging();
                            }

                            if (Ex != null && Ex.Range == Ex.MaximumRange)
                            {
                                Ex.Cast(mob.Position);
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
                    var traveltime = mob.Distance(ObjectManager.Player)
                                     / (Spells.R.Handle.SData.MissileSpeed + Spells.R.CastDelay) + Game.Ping / 2f / 1000;
                    if (Spells.RisToggle || Spells.RisDash || Spells.RisCC)
                    {
                        return;
                    }
                    if (KillSteal.playerdamage + ObjectManager.Player.GetSpellDamage(mob, SpellSlot.R)
                        > Prediction.Health.GetPrediction(mob, (int)traveltime) && Spells.R.IsInRange(mob)
                        && Spells.R.IsReady())
                    {
                        if (Spells.R.GetType() == typeof(Spell.Skillshot))
                        {
                            Spell.Skillshot Rx = Spells.R as Spell.Skillshot;
                            if (Rx != null)
                            {
                                Rx.GetPrediction(mob);
                                Rx.Cast(mob);
                            }
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
                            Spell.Chargeable Rx = Spells.R as Spell.Chargeable;
                            if (Rx != null && !Rx.IsCharging)
                            {
                                Rx.StartCharging();
                            }

                            if (Rx != null && Rx.Range == Rx.MaximumRange)
                            {
                                Rx.Cast(mob.Position);
                            }
                            return;
                        }

                        if (Spells.R.GetType() == typeof(Spell.Ranged))
                        {
                            Spells.R.Cast(mob.Position);
                            return;
                        }
                    }
                }
            }
        }
    }
}