namespace Khappa_Zix
{
    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Menu.Values;

    class menu
    {
        public static Menu MenuIni, Combo, HarassMenu, Clear, Jump, Draw;

        internal static void Load()
        {
            MenuIni = MainMenu.AddMenu("Khappa'Zix ", "Khappa'Zix");

            Combo = MenuIni.AddSubMenu("Combo ", "Combo");
            Combo.AddGroupLabel("Combo Settings");
            Combo.Add("Q", new CheckBox("Use Q "));
            Combo.Add("W", new CheckBox("Use W "));
            Combo.Add("E", new CheckBox("Use E "));
            Combo.Add("Edive", new CheckBox("E Dive Towers"));
            Combo.Add("Rmode", new ComboBox("R Mode", 0, "GapCloser", "Always"));
            Combo.Add("R", new CheckBox("Use R "));
            Combo.AddSeparator();
            Combo.AddGroupLabel("E Settings");
            Combo.Add("dis", new Slider("Use if Distance to target is > {0}", 400, 0, 850));
            Combo.Add("delay", new Slider("2nd E Delay {0}", 150, 0, 300));
            Combo.AddSeparator();
            Combo.AddGroupLabel("Extra Settings");
            Combo.Add("hitchance", new ComboBox("HitChance", 0, "High", "Medium", "Low"));

            Combo = MenuIni.AddSubMenu("DoubleJump ", "DoubleJump");
            Jump.AddGroupLabel("E Settings");
            Combo.Add("double", new CheckBox("Use E DoubleJump"));
            Jump.Add("delay", new Slider("2nd E Delay {0}", 150, 0, 300));

            Draw = MenuIni.AddSubMenu("Drawings ", "Drawings");
            Draw.AddGroupLabel("Drawings Settings");
            Draw.Add("Q", new CheckBox("Draw Q "));
            Draw.Add("W", new CheckBox("Draw W "));
            Draw.Add("E", new CheckBox("Draw E "));
            Draw.Add("R", new CheckBox("Draw R "));
        }
    }
}
