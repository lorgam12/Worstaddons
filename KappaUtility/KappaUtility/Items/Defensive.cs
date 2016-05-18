namespace KappaUtility.Items
{
    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Menu;

    using Common;

    internal class Defensive
    {
        public static readonly Item Zhonyas = new Item(ItemId.Zhonyas_Hourglass);

        public static readonly Item Seraph = new Item(ItemId.Seraphs_Embrace);

        public static readonly Item FOTM = new Item(ItemId.Face_of_the_Mountain, 600);

        public static readonly Item Solari = new Item(ItemId.Locket_of_the_Iron_Solari, 600);

        public static readonly Item Randuin = new Item(ItemId.Randuins_Omen, 500f);

        public static int Seraphh => DefMenu.GetSlider("Seraphh");

        public static int Solarih => DefMenu.GetSlider("Solarih");

        public static int FaceOfTheMountainh => DefMenu.GetSlider("FaceOfTheMountainh");

        public static int Zhonyash => DefMenu.GetSlider("Zhonyash");

        public static int Seraphn => DefMenu.GetSlider("Seraphn");

        public static int Solarin => DefMenu.GetSlider("Solarin");

        public static int FaceOfTheMountainn => DefMenu.GetSlider("FaceOfTheMountainn");

        public static int Zhonyasn => DefMenu.GetSlider("Zhonyasn");

        public static bool Seraphc => DefMenu.GetCheckbox("Seraph") && Seraph.IsOwned(Player.Instance) && Seraph.IsReady();

        public static bool Solaric => DefMenu.GetCheckbox("Solari") && Solari.IsOwned(Player.Instance) && Solari.IsReady();

        public static bool FaceOfTheMountainc => DefMenu.GetCheckbox("FaceOfTheMountain") && FOTM.IsOwned(Player.Instance) && FOTM.IsReady();

        public static bool Zhonyasc => DefMenu.GetCheckbox("Zhonyas") && Zhonyas.IsOwned(Player.Instance) && Zhonyas.IsReady();

        public static Menu DefMenu { get; private set; }

        protected static bool loaded = false;

        internal static void OnLoad()
        {
            DefMenu = Load.UtliMenu.AddSubMenu("Defence Items");
            DefMenu.AddGroupLabel("Defence Settings");
            DefMenu.Checkbox("Zhonyas", "Use Zhonyas");
            DefMenu.Slider("Zhonyash", "Use Zhonyas health [{0}%]", 35);
            DefMenu.Slider("Zhonyasn", "Use Zhonyas if incoming Damage more than [{0}%]", 50);
            DefMenu.AddSeparator();
            DefMenu.Checkbox("Seraph", "Use Seraph");
            DefMenu.Slider("Seraphh", "Use Seraph health [{0}%]", 45);
            DefMenu.Slider("Seraphn", "Use Seraph if incoming Damage more than [{0}%]", 45);
            DefMenu.AddSeparator();
            DefMenu.Checkbox("FaceOfTheMountain", "Use Face Of The Mountain");
            DefMenu.Slider("FaceOfTheMountainh", "Use FOTM health [{0}%]", 50);
            DefMenu.Slider("FaceOfTheMountainn", "Use FOTM if incoming Damage more than [{0}%]", 50);
            DefMenu.AddSeparator();
            DefMenu.Checkbox("Solari", "Use Solari");
            DefMenu.Slider("Solarih", "Use Solari health [{0}%]", 30);
            DefMenu.Slider("Solarin", "Use Solari if incoming Damage more than [{0}%]", 35);
            DefMenu.AddSeparator();
            DefMenu.Checkbox("Randuin", "Use Randuin (Omen)");
            DefMenu.Slider("Randuinh", "Use Randuin On [{0}] Enemies", 2, 1, 5);
            DefMenu.AddSeparator();
            DefMenu.AddGroupLabel("Zhonya Danger Spells");
            DefMenu.Checkbox("ZhonyasD", "Deny Dangers Spells");
            DamageHandler.OnLoad();
            Zhonya.OnLoad();
            loaded = true;
        }

        internal static void Items()
        {
            if (!loaded)
            {
                return;
            }

            if (Randuin.IsReady() && Randuin.IsOwned(Player.Instance) && Helpers.CountEnemies((int)Randuin.Range) >= DefMenu.GetSlider("Randuinh")
                && DefMenu.GetCheckbox("Randuin"))
            {
                Randuin.Cast();
            }
        }
    }
}