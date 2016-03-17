namespace KappaUtility.Items
{
    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Menu.Values;

    internal class Potions
    {
        public static Menu PotMenu { get; private set; }

        public static readonly Item Corrupting = new Item(ItemId.Corrupting_Potion);

        public static readonly Item Health = new Item(ItemId.Health_Potion);

        public static readonly Item Hunters = new Item(ItemId.Hunters_Potion);

        public static readonly Item Refillable = new Item((int)ItemId.Refillable_Potion);

        public static readonly Item Biscuit = new Item((int)ItemId.Total_Biscuit_of_Rejuvenation);

        internal static void OnLoad()
        {
            PotMenu = Load.UtliMenu.AddSubMenu("Potions");
            PotMenu.AddGroupLabel("Potions Settings");
            PotMenu.Add("CP", new CheckBox("Corrupting Potion"));
            PotMenu.Add("CPH", new Slider("Use On Health %", 65, 0, 100));
            PotMenu.Add("HP", new CheckBox("Health Potion"));
            PotMenu.Add("HPH", new Slider("Use On health %", 45, 0, 100));
            PotMenu.Add("HPS", new CheckBox("Hunters Potion"));
            PotMenu.Add("HPSH", new Slider("Use On health %", 75, 0, 100));
            PotMenu.Add("RP", new CheckBox("Refillable Potion"));
            PotMenu.Add("RPH", new Slider("Use On health %", 50, 0, 100));
            PotMenu.Add("BP", new CheckBox("Biscuit"));
            PotMenu.Add("BPH", new Slider("Use On health %", 40, 0, 100));
        }
    }
}