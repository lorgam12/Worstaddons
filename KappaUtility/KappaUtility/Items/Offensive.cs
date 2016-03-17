namespace KappaUtility
{
    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Menu.Values;

    internal class Offensive
    {
        public static readonly Item Hydra = new Item(ItemId.Ravenous_Hydra_Melee_Only, 250f);

        public static readonly Item Titanic = new Item(ItemId.Titanic_Hydra, Player.Instance.GetAutoAttackRange());

        public static readonly Item Timat = new Item(ItemId.Tiamat_Melee_Only, 250f);

        public static readonly Item Cutlass = new Item((int)ItemId.Bilgewater_Cutlass, 550);

        public static readonly Item Botrk = new Item((int)ItemId.Blade_of_the_Ruined_King, 550);

        public static readonly Item Youmuu = new Item((int)ItemId.Youmuus_Ghostblade);

        public static Menu OffMenu { get; private set; }

        internal static void OnLoad()
        {
            OffMenu = Load.UtliMenu.AddSubMenu("Offense Items");
            OffMenu.AddGroupLabel("Offense Settings");
            OffMenu.Add("Hydra", new CheckBox("Use Hydra / Timat / Titanic"));
            OffMenu.Add("useGhostblade", new CheckBox("Use Youmuu's Ghostblade"));
            OffMenu.Add("UseBOTRK", new CheckBox("Use Blade of the Ruined King"));
            OffMenu.Add("UseBilge", new CheckBox("Use Bilgewater Cutlass"));
            OffMenu.AddSeparator();
            OffMenu.Add("eL", new Slider("Use On Enemy health", 65, 0, 100));
            OffMenu.Add("oL", new Slider("Use On My health", 65, 0, 100));
        }

        internal static void Items()
        {
            var target = TargetSelector.GetTarget(500, DamageType.Physical);
            if (target == null || !target.IsValidTarget())
            {
                return;
            }

            if (Botrk.IsReady() && Botrk.IsOwned(Player.Instance) && target.IsValidTarget(Botrk.Range)
                && target.HealthPercent <= OffMenu["eL"].Cast<Slider>().CurrentValue
                && OffMenu["UseBOTRK"].Cast<CheckBox>().CurrentValue)
            {
                Botrk.Cast(target);
            }

            if (Botrk.IsReady() && Botrk.IsOwned(Player.Instance) && target.IsValidTarget(Botrk.Range)
                && target.HealthPercent <= OffMenu["oL"].Cast<Slider>().CurrentValue
                && OffMenu["UseBOTRK"].Cast<CheckBox>().CurrentValue)
            {
                Botrk.Cast(target);
            }

            if (Cutlass.IsReady() && Cutlass.IsOwned(Player.Instance) && target.IsValidTarget(Cutlass.Range)
                && target.HealthPercent <= OffMenu["eL"].Cast<Slider>().CurrentValue
                && OffMenu["UseBilge"].Cast<CheckBox>().CurrentValue)
            {
                Cutlass.Cast(target);
            }

            if (Youmuu.IsReady() && Youmuu.IsOwned(Player.Instance) && target.IsValidTarget(500)
                && OffMenu["useGhostblade"].Cast<CheckBox>().CurrentValue)
            {
                Youmuu.Cast();
            }
        }
    }
}