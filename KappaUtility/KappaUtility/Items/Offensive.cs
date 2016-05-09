namespace KappaUtility.Items
{
    using System;
    using System.Linq;

    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Menu.Values;

    using KappaUtility.Common;

    internal class Offensive
    {
        public static readonly Item Hydra = new Item(ItemId.Ravenous_Hydra_Melee_Only, 250f);

        public static readonly Item Titanic = new Item(ItemId.Titanic_Hydra, Player.Instance.GetAutoAttackRange());

        public static readonly Item Timat = new Item(ItemId.Tiamat_Melee_Only, 250f);

        public static readonly Item Cutlass = new Item((int)ItemId.Bilgewater_Cutlass, 550);

        public static readonly Item Botrk = new Item((int)ItemId.Blade_of_the_Ruined_King, 550);

        public static readonly Item Youmuu = new Item((int)ItemId.Youmuus_Ghostblade);

        protected static readonly Item Gunblade = new Item(ItemId.Hextech_Gunblade, 700f);

        protected static readonly Item ProtoBelt = new Item(ItemId.Will_of_the_Ancients, 600);

        protected static readonly Item GLP = new Item(3030, 600);

        public static Menu OffMenu { get; private set; }

        protected static bool loaded = false;

        internal static void OnLoad()
        {
            OffMenu = Load.UtliMenu.AddSubMenu("Offense Items");
            OffMenu.AddGroupLabel("Offense Settings");
            OffMenu.Add("Hydra", new CheckBox("Use Hydra / Timat / Titanic", false));
            OffMenu.Add("useGhostblade", new CheckBox("Use Youmuu's Ghostblade", false));
            OffMenu.Add("UseBOTRK", new CheckBox("Use Blade of the Ruined King", false));
            OffMenu.Add("UseBilge", new CheckBox("Use Bilgewater Cutlass", false));
            OffMenu.Add("UseGunblade", new CheckBox("Use Hextech Gunblade", false));
            OffMenu.Add("UseBelt", new CheckBox("Use Hextech ProtoBelt-01", false));
            OffMenu.Add("UseGLP", new CheckBox("Use Hextech GLP-800", false));
            OffMenu.AddSeparator();
            OffMenu.AddGroupLabel("Settings");
            OffMenu.Add("UseKS", new CheckBox("Use for KillSteal", false));
            OffMenu.Add("UseCombo", new CheckBox("Use In Combo", false));
            OffMenu.Add("eL", new Slider("Use On Enemy health", 65, 0, 100));
            OffMenu.Add("oL", new Slider("Use On My health", 65, 0, 100));

            Orbwalker.OnPostAttack += Orbwalker_OnPostAttack;
            loaded = true;
        }

        private static void Orbwalker_OnPostAttack(AttackableUnit target, EventArgs args)
        {
            if (!loaded)
            {
                return;
            }
            if (!target.IsEnemy || !(target is AIHeroClient))
            {
                return;
            }

            var useHydra = OffMenu["Hydra"].Cast<CheckBox>().CurrentValue
                           && ((Hydra.IsOwned(Player.Instance) && Hydra.IsReady()) || (Timat.IsOwned(Player.Instance) && Timat.IsReady())
                               || (Titanic.IsOwned(Player.Instance) && Titanic.IsReady()));
            var flags = Orbwalker.ActiveModesFlags;
            if (flags.HasFlag(Orbwalker.ActiveModes.Combo) && useHydra)
            {
                if (Hydra.IsOwned() && Hydra.IsReady() && Hydra != null)
                {
                    if (Hydra.Cast())
                    {
                        Orbwalker.ResetAutoAttack();
                    }
                }

                if (Timat.IsOwned() && Timat.IsReady() && Timat != null)
                {
                    if (Timat.Cast())
                    {
                        Orbwalker.ResetAutoAttack();
                    }
                }
                if (Titanic.IsOwned() && Titanic.IsReady() && Titanic != null)
                {
                    if (Titanic.Cast())
                    {
                        Orbwalker.ResetAutoAttack();
                    }
                }
            }
        }

        internal static void Items()
        {
            if (!loaded)
            {
                return;
            }

            if (OffMenu["UseKS"].Cast<CheckBox>().CurrentValue)
            {
                foreach (var enemy in EntityManager.Heroes.Enemies)
                {
                    if (enemy != null && enemy.IsKillable() && enemy.IsValidTarget(600))
                    {
                        if (OffMenu["UseGunblade"].Cast<CheckBox>().CurrentValue
                            && Player.Instance.GetItemDamage(enemy, ItemId.Hextech_Gunblade) >= enemy.Health)
                        {
                            Gunblade.Cast(enemy);
                        }
                        if (OffMenu["UseBOTRK"].Cast<CheckBox>().CurrentValue
                            && Player.Instance.GetItemDamage(enemy, ItemId.Blade_of_the_Ruined_King) >= enemy.Health)
                        {
                            Botrk.Cast(enemy);
                        }
                        if (OffMenu["UseBilge"].Cast<CheckBox>().CurrentValue
                            && Player.Instance.GetItemDamage(enemy, ItemId.Bilgewater_Cutlass) >= enemy.Health)
                        {
                            Cutlass.Cast(enemy);
                        }
                        if (OffMenu["UseBelt"].Cast<CheckBox>().CurrentValue
                            && Player.Instance.GetItemDamage(enemy, ItemId.Will_of_the_Ancients) >= enemy.Health)
                        {
                            ProtoBelt.Cast(enemy.ServerPosition);
                        }
                        if (OffMenu["UseGLP"].Cast<CheckBox>().CurrentValue && Player.Instance.GetItemDamage(enemy, (ItemId)3030) >= enemy.Health)
                        {
                            GLP.Cast(enemy.ServerPosition);
                        }
                        if (OffMenu["Hydra"].Cast<CheckBox>().CurrentValue)
                        {
                            if (Hydra.IsOwned(Player.Instance) && Hydra.IsReady())
                            {
                                Hydra.Cast();
                            }
                            if (Timat.IsOwned(Player.Instance) && Timat.IsReady())
                            {
                                Timat.Cast();
                            }
                            if (Titanic.IsOwned(Player.Instance) && Titanic.IsReady())
                            {
                                Titanic.Cast();
                            }
                        }
                    }
                }
            }

            var target = TargetSelector.GetTarget(600, DamageType.Physical);
            if (target == null || !target.IsValidTarget() || !OffMenu["UseCombo"].Cast<CheckBox>().CurrentValue)
            {
                return;
            }

            if (target.HealthPercent <= OffMenu["eL"].Cast<Slider>().CurrentValue
                || Player.Instance.HealthPercent <= OffMenu["oL"].Cast<Slider>().CurrentValue)
            {
                if (Gunblade.IsReady() && Gunblade.IsOwned(Player.Instance) && target.IsValidTarget(Gunblade.Range)
                    && OffMenu["UseGunblade"].Cast<CheckBox>().CurrentValue)
                {
                    Gunblade.Cast(target);
                }

                if (Botrk.IsReady() && Botrk.IsOwned(Player.Instance) && target.IsValidTarget(Botrk.Range)
                    && (target.HealthPercent <= OffMenu["eL"].Cast<Slider>().CurrentValue
                        || Player.Instance.HealthPercent <= OffMenu["oL"].Cast<Slider>().CurrentValue)
                    && OffMenu["UseBOTRK"].Cast<CheckBox>().CurrentValue)
                {
                    Botrk.Cast(target);
                }

                if (Cutlass.IsReady() && Cutlass.IsOwned(Player.Instance) && target.IsValidTarget(Cutlass.Range)
                    && (target.HealthPercent <= OffMenu["eL"].Cast<Slider>().CurrentValue
                        || Player.Instance.HealthPercent <= OffMenu["oL"].Cast<Slider>().CurrentValue)
                    && OffMenu["UseBilge"].Cast<CheckBox>().CurrentValue)
                {
                    Cutlass.Cast(target);
                }

                if (ProtoBelt.IsOwned(Player.Instance) && ProtoBelt.IsReady() && OffMenu["UseBelt"].Cast<CheckBox>().CurrentValue)
                {
                    ProtoBelt.Cast(target.ServerPosition);
                }

                if (GLP.IsOwned(Player.Instance) && GLP.IsReady() && OffMenu["UseGLP"].Cast<CheckBox>().CurrentValue)
                {
                    GLP.Cast(target.ServerPosition);
                }
            }

            if (Youmuu.IsReady() && Youmuu.IsOwned(Player.Instance) && target.IsValidTarget(500)
                && OffMenu["useGhostblade"].Cast<CheckBox>().CurrentValue)
            {
                Youmuu.Cast();
            }
        }
    }
}