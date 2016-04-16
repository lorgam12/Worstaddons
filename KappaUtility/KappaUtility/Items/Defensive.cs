namespace KappaUtility.Items
{
    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Menu.Values;

    internal class Defensive
    {
        public static readonly Item Zhonyas = new Item(ItemId.Zhonyas_Hourglass);

        public static readonly Item Seraph = new Item(ItemId.Seraphs_Embrace);

        public static readonly Item FOTM = new Item(ItemId.Face_of_the_Mountain);

        public static readonly Item Solari = new Item(ItemId.Locket_of_the_Iron_Solari);

        public static readonly Item Randuin = new Item(ItemId.Randuins_Omen, 500f);

        public static int Seraphh => DefMenu["Seraphh"].Cast<Slider>().CurrentValue;

        public static int Solarih => DefMenu["Solarih"].Cast<Slider>().CurrentValue;

        public static int FaceOfTheMountainh => DefMenu["FaceOfTheMountainh"].Cast<Slider>().CurrentValue;

        public static int Zhonyash => DefMenu["Zhonyash"].Cast<Slider>().CurrentValue;

        public static bool Seraphc => DefMenu["Seraph"].Cast<CheckBox>().CurrentValue && Seraph.IsOwned() && Seraph.IsReady();

        public static bool Solaric => DefMenu["Solari"].Cast<CheckBox>().CurrentValue && Solari.IsOwned() && Solari.IsReady();

        public static bool FaceOfTheMountainc => DefMenu["FaceOfTheMountain"].Cast<CheckBox>().CurrentValue && FOTM.IsOwned() && FOTM.IsReady();

        public static bool Zhonyasc => DefMenu["Zhonyas"].Cast<CheckBox>().CurrentValue && Zhonyas.IsOwned() && Zhonyas.IsReady();

        public static Menu DefMenu { get; private set; }

        protected static bool loaded = false;

        internal static void OnLoad()
        {
            DefMenu = Load.UtliMenu.AddSubMenu("Defence Items");
            DefMenu.AddGroupLabel("Defence Settings");
            DefMenu.Add("Zhonyas", new CheckBox("Use Zhonyas", false));
            DefMenu.Add("Zhonyash", new Slider("Use Zhonyas health", 25, 0, 100));
            DefMenu.AddSeparator();
            DefMenu.Add("Seraph", new CheckBox("Use Seraph", false));
            DefMenu.Add("Seraphh", new Slider("Use Seraph health", 45, 0, 100));
            DefMenu.AddSeparator();
            DefMenu.Add("FaceOfTheMountain", new CheckBox("Use Face Of The Mountain", false));
            DefMenu.Add("FaceOfTheMountainh", new Slider("Use Face Of The Mountain health", 50, 0, 100));
            DefMenu.AddSeparator();
            DefMenu.Add("Solari", new CheckBox("Use Solari", false));
            DefMenu.Add("Solarih", new Slider("Use Solari health", 30, 0, 100));
            DefMenu.AddSeparator();
            DefMenu.Add("Randuin", new CheckBox("Use Randuin", false));
            DefMenu.Add("Randuinh", new Slider("Use Randuin On X Enemies", 2, 1, 5));
            DefMenu.AddSeparator();
            DefMenu.AddGroupLabel("Zhonya Danger Spells");
            DefMenu.Add("ZhonyasD", new CheckBox("Deny Dangers Spells", false));
            Zhonya.OnLoad();
            loaded = true;
        }

        internal static void Items()
        {
            if (!loaded)
            {
                return;
            }

            if (Randuin.IsReady() && Randuin.IsOwned(Player.Instance)
                && Player.Instance.CountEnemiesInRange(Randuin.Range) >= DefMenu["Randuinh"].Cast<Slider>().CurrentValue
                && DefMenu["Randuin"].Cast<CheckBox>().CurrentValue)
            {
                Randuin.Cast();
            }
        }
    }
}