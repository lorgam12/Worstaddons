namespace AutoFarmer
{
    using System;

    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Events;
    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Menu.Values;
    using EloBuddy.SDK.Rendering;

    using SharpDX;

    using Genesis.Library;
    using Genesis.Library.Spells;

    internal class AutoFarm
    {
        public static Menu MenuIni, SpellsMenu, ManaMenu, DrawMenu;

        public static string Champion = Player.Instance.ChampionName;

        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        protected static SpellBase Spells => SpellManager.CurrentSpells;

        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            OnLoad();
            SpellManager.Initialize();
            SpellLibrary.Initialize();
        }

        private static void OnLoad()
        {
            MenuIni = MainMenu.AddMenu("Auto Farm ", "Auto Farm");

            SpellsMenu = MenuIni.AddSubMenu("Spells ", "Spells");
            SpellsMenu.AddGroupLabel("Spells for UnkillableMinion");
            SpellsMenu.Add(Champion + "Q", new CheckBox("Use Q ", false));
            SpellsMenu.Add(Champion + "W", new CheckBox("Use W ", false));
            SpellsMenu.Add(Champion + "E", new CheckBox("Use E ", false));
            SpellsMenu.Add(Champion + "R", new CheckBox("Use R ", false));

            ManaMenu = MenuIni.AddSubMenu("Mana Manager ", "Mana Manager");
            ManaMenu.AddGroupLabel("Only Cast if Mana >= %");
            ManaMenu.Add(Champion + "Q", new Slider(" Q "));
            ManaMenu.Add(Champion + "W", new Slider(" W "));
            ManaMenu.Add(Champion + "E", new Slider(" E "));
            ManaMenu.Add(Champion + "R", new Slider(" R "));

            DrawMenu = MenuIni.AddSubMenu("Drawings ", "Drawings");
            DrawMenu.AddGroupLabel("Drawings Settings");
            DrawMenu.Add(Champion + "Q", new CheckBox("Draw Q ", false));
            DrawMenu.Add(Champion + "W", new CheckBox("Draw W ", false));
            DrawMenu.Add(Champion + "E", new CheckBox("Draw E ", false));
            DrawMenu.Add(Champion + "R", new CheckBox("Draw R ", false));

            Orbwalker.OnUnkillableMinion += Orbwalker_OnUnkillableMinion;
            Drawing.OnDraw += Drawing_OnDraw;
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            if (DrawMenu[Champion + "Q"].Cast<CheckBox>().CurrentValue && (!Spells.QisToggle || !Spells.QisDash || !Spells.QisCc || Spells.Q.Range >= 20000 || Spells.Q == null))
            {
                Circle.Draw(Color.White, Spells.Q.Range, ObjectManager.Player.Position);
            }

            if (DrawMenu[Champion + "W"].Cast<CheckBox>().CurrentValue && (!Spells.WisToggle || !Spells.WisDash || !Spells.WisCc || Spells.W.Range >= 20000 || Spells.W == null))
            {
                Circle.Draw(Color.White, Spells.W.Range, ObjectManager.Player.Position);
            }

            if (DrawMenu[Champion + "E"].Cast<CheckBox>().CurrentValue && (!Spells.EisToggle || !Spells.EisDash || !Spells.EisCc || Spells.E.Range >= 20000 || Spells.E == null))
            {
                Circle.Draw(Color.White, Spells.E.Range, ObjectManager.Player.Position);
            }

            if (DrawMenu[Champion + "R"].Cast<CheckBox>().CurrentValue && (!Spells.RisToggle || !Spells.RisDash || !Spells.RisCc || Spells.R.Range >= 20000 || Spells.R == null))
            {
                Circle.Draw(Color.White, Spells.R.Range, ObjectManager.Player.Position);
            }
        }

        private static void Orbwalker_OnUnkillableMinion(Obj_AI_Base target, Orbwalker.UnkillableMinionArgs args)
        {
            if (target == null || target.IsAlly)
            {
                return;
            }

            if (SpellsMenu[Champion + "Q"].Cast<CheckBox>().CurrentValue
                && Player.Instance.ManaPercent >= ManaMenu[Champion + "Q"].Cast<Slider>().CurrentValue
                && Spells.Q.IsReady())
            {
                if (Spells.QisToggle || Spells.QisDash || Spells.QisCc || Spells.Q == null)
                {
                    return;
                }

                if (ObjectManager.Player.GetSpellDamage(target, SpellSlot.Q)
                    >= Prediction.Health.GetPrediction(target, (int)Spells.Q.CastDelay * 1000)
                    && Spells.Q.IsInRange(target))
                {
                    if (Spells.Q.GetType() == typeof(Spell.Skillshot))
                    {
                        var Qx = Spells.Q as Spell.Skillshot;
                        Qx?.GetPrediction(target);
                        Qx?.Cast(target);
                        return;
                    }

                    if (Spells.Q.GetType() == typeof(Spell.Active))
                    {
                        Spells.Q.Cast(target);
                        return;
                    }

                    if (Spells.Q.GetType() == typeof(Spell.Chargeable))
                    {
                        var Qx = Spells.Q as Spell.Chargeable;
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
                        Spells.Q.Cast(target.Position);
                        return;
                    }
                }
            }

            if (SpellsMenu[Champion + "W"].Cast<CheckBox>().CurrentValue
                && Player.Instance.ManaPercent >= ManaMenu[Champion + "W"].Cast<Slider>().CurrentValue
                && Spells.W.IsReady())
            {
                if (Spells.WisToggle || Spells.WisDash || Spells.WisCc || Spells.W == null)
                {
                    return;
                }
                if (ObjectManager.Player.GetSpellDamage(target, SpellSlot.W)
                    > Prediction.Health.GetPrediction(target, (int)(Spells.W.CastDelay)) && Spells.W.IsInRange(target))
                {
                    if (Spells.W.GetType() == typeof(Spell.Skillshot))
                    {
                        var Wx = Spells.W as Spell.Skillshot;
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
                        var Wx = Spells.W as Spell.Chargeable;
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
                        Spells.W.Cast(target.Position);
                        return;
                    }
                }
            }

            if (SpellsMenu[Champion + "E"].Cast<CheckBox>().CurrentValue
                && Player.Instance.ManaPercent >= ManaMenu[Champion + "E"].Cast<Slider>().CurrentValue
                && Spells.E.IsReady())
            {
                if (Spells.EisToggle || Spells.EisDash || Spells.EisCc || Spells.E == null)
                {
                    return;
                }
                if (ObjectManager.Player.GetSpellDamage(target, SpellSlot.E)
                    > Prediction.Health.GetPrediction(target, (int)(Spells.E.CastDelay * 1000))
                    && Spells.E.IsInRange(target))
                {
                    if (Spells.E.GetType() == typeof(Spell.Skillshot))
                    {
                        var Ex = Spells.E as Spell.Skillshot;
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

            if (SpellsMenu[Champion + "R"].Cast<CheckBox>().CurrentValue
                && Player.Instance.ManaPercent >= ManaMenu[Champion + "R"].Cast<Slider>().CurrentValue)
            {
                if (Spells.RisToggle || Spells.RisDash || Spells.RisCc || Spells.R == null)
                {
                    return;
                }
                if (ObjectManager.Player.GetSpellDamage(target, SpellSlot.R)
                    > Prediction.Health.GetPrediction(target, (int)(Spells.R.CastDelay * 1000))
                    && Spells.R.IsInRange(target))
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