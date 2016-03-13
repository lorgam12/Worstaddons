namespace KappaEkko
{
    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Menu.Values;

    internal class Menu
    {
        public static EloBuddy.SDK.Menu.Menu UltMenu { get; private set; }

        public static EloBuddy.SDK.Menu.Menu ComboMenu { get; private set; }

        public static EloBuddy.SDK.Menu.Menu HarassMenu { get; private set; }

        public static EloBuddy.SDK.Menu.Menu LaneMenu { get; private set; }

        public static EloBuddy.SDK.Menu.Menu MiscMenu { get; private set; }

        public static EloBuddy.SDK.Menu.Menu DrawMenu { get; private set; }

        public static EloBuddy.SDK.Menu.Menu ManaMenu { get; private set; }

        private static EloBuddy.SDK.Menu.Menu menuIni;

        public static void Load()
        {
            menuIni = MainMenu.AddMenu("Ekko", "Ekko");
            menuIni.AddGroupLabel("Welcome to the Worst Ekko addon!");

            UltMenu = menuIni.AddSubMenu("Ultimate");
            UltMenu.AddGroupLabel("Ultimate Settings");
            UltMenu.Add("Rsave", new CheckBox("Use R Saver"));
            UltMenu.Add("Rsaveh", new Slider("R Saver Health", 15, 0, 100));
            UltMenu.Add("RAoe", new CheckBox("Auto R AoE"));
            UltMenu.Add("RAoeh", new Slider("R AoE Hit", 3, 1, 5));
            UltMenu.Add("REscape", new CheckBox("R Escape"));
            UltMenu.Add("REscapeh", new Slider("R Escape On Health", 10, 1, 100));

            ComboMenu = menuIni.AddSubMenu("Combo");
            ComboMenu.AddGroupLabel("Combo Settings");
            ComboMenu.Add("Q", new CheckBox("Use Q"));
            ComboMenu.Add("W", new CheckBox("Use W No Prediction"));
            ComboMenu.Add("Wpred", new CheckBox("Use W With Prediction", false));
            ComboMenu.Add("Whit", new Slider("W On Hit X Enemies", 1, 1, 5));
            ComboMenu.Add("E", new CheckBox("Use E"));
            ComboMenu.Add("R", new CheckBox("Use R"));
            ComboMenu.Add("Rhit", new Slider("Use R Hit", 2, 1, 5));

            HarassMenu = menuIni.AddSubMenu("Harass");
            HarassMenu.AddGroupLabel("Harass Settings");
            HarassMenu.Add("Q", new CheckBox("Use Q"));
            HarassMenu.Add("W", new CheckBox("Use W"));
            HarassMenu.Add("E", new CheckBox("Use E"));

            LaneMenu = menuIni.AddSubMenu("Farm");
            LaneMenu.AddGroupLabel("LaneClear Settings");
            LaneMenu.Add("Q", new CheckBox("Use Q"));
            LaneMenu.Add("E", new CheckBox("Use E"));

            ManaMenu = menuIni.AddSubMenu("Mana Manager");
            ManaMenu.AddGroupLabel("Harass");
            ManaMenu.Add("harassmana", new Slider("Harass Mana %", 75, 0, 100));
            ManaMenu.AddGroupLabel("Lane Clear");
            ManaMenu.Add("lanemana", new Slider("Lane Clear Mana %", 60, 0, 100));

            DrawMenu = menuIni.AddSubMenu("Drawings");
            DrawMenu.AddGroupLabel("Drawing Settings");
            DrawMenu.Add("Q", new CheckBox("Draw Q"));
            DrawMenu.Add("W", new CheckBox("Draw W"));
            DrawMenu.Add("E", new CheckBox("Draw E"));
            DrawMenu.Add("R", new CheckBox("Draw R"));
        }
    }
}