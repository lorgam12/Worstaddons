namespace Khappa_Zix.Load
{
    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Menu.Values;

    internal class menu
    {
        public static Menu MenuIni, Combo, Harass, Misc, Clear, KillSteal, Mana, Jump, Draw;

        internal static void Load()
        {
            MenuIni = MainMenu.AddMenu("Khappa'Zix", "Khappa'Zix");

            Jump = MenuIni.AddSubMenu("JumpsHandler ", "JumpsHandler");
            Jump.AddGroupLabel("E Settings");
            Jump.Add("double", new CheckBox("Use E DoubleJump", false));
            Jump.Add("block", new CheckBox("Block if will land on a wall"));
            Jump.Add("delay", new Slider("2nd E Delay {0}", 150, 0, 300));
            Jump.AddGroupLabel("1st Jump");
            Jump.Add("1jump", new ComboBox("1st Jump", 0, "To Base", "To Ally", "To Mouse", "To Next Target"));
            Jump.AddGroupLabel("2nd Jump");
            Jump.Add("2jump", new ComboBox("2nd Jump", 0, "To Base", "To Ally", "To Mouse", "To Next Target"));
            Jump.AddSeparator();
            Jump.AddGroupLabel("Extra Settings");
            Jump.AddLabel("Escape Towers");
            Jump.Add("save", new CheckBox("Jump Out of Enemy Turrets Range"));
            Jump.Add("saveh", new Slider("Use Under {0}% Health", 15));

            Combo = MenuIni.AddSubMenu("Combo ", "Combo");
            Combo.AddGroupLabel("Combo Settings");
            Combo.Add("Q", new CheckBox("Use Q "));
            Combo.Add("W", new CheckBox("Use W "));
            Combo.Add("E", new CheckBox("Use E "));
            Combo.AddSeparator();
            Combo.AddGroupLabel("E Settings");
            Combo.Add("Edive", new CheckBox("E Dive Towers"));
            Combo.Add("safe", new Slider("Dont E if Enemies Near target Are more than {0}", 3, 0, 5));
            Combo.Add("dis", new Slider("Use if Distance to target is more than {0}", 385, 0, 850));
            Combo.AddSeparator();
            Combo.AddGroupLabel("R Settings");
            Combo.Add("useR", new CheckBox("Use R"));
            Combo.Add("R", new CheckBox("Use R When No Spells Are Ready"));
            Combo.Add("NoAA", new CheckBox("No AA While R Active"));
            Combo.Add("Rmode", new ComboBox("R Mode", 0, "GapClose For Combo", "Always"));
            Combo.Add("danger", new Slider("Use if Enemies Near me are more than {0}", 3, 1, 5));

            Harass = MenuIni.AddSubMenu("Harass ", "Harass");
            Harass.AddGroupLabel("Harass Settings");
            Harass.Add("Q", new CheckBox("Use Q "));
            Harass.Add("W", new CheckBox("Use W "));
            Harass.Add("E", new CheckBox("Use E "));
            Harass.Add("Edive", new CheckBox("E Dive Towers"));

            Clear = MenuIni.AddSubMenu("Clear ", "Clear");
            Clear.AddGroupLabel("LaneClear Settings");
            Clear.Add("Qc", new CheckBox("Use Q "));
            Clear.Add("Wc", new CheckBox("Use W "));
            Clear.Add("Ec", new CheckBox("Use E ", false));
            Clear.AddSeparator();
            Clear.AddGroupLabel("LastHit Settings");
            Clear.Add("Qh", new CheckBox("Use Q "));
            Clear.Add("Wh", new CheckBox("Use W "));
            Clear.Add("Eh", new CheckBox("Use E ", false));
            Clear.AddSeparator();
            Clear.AddGroupLabel("JungleClear Settings");
            Clear.Add("Qj", new CheckBox("Use Q "));
            Clear.Add("Wj", new CheckBox("Use W "));
            Clear.Add("Ej", new CheckBox("Use E ", false));

            Mana = MenuIni.AddSubMenu("ManaManager ", "ManaManager");
            Mana.AddGroupLabel("Harass Mana");
            Mana.Add("harass", new Slider("Save {0}% Mana", 60));
            Mana.AddSeparator();
            Mana.AddGroupLabel("LaneClear Mana");
            Mana.Add("lane", new Slider("Save {0}% Mana", 75));
            Mana.AddSeparator();
            Mana.AddGroupLabel("LastHit Mana");
            Mana.Add("last", new Slider("Save {0}% Mana", 50));
            Mana.AddSeparator();
            Mana.AddGroupLabel("JungleClear Mana");
            Mana.Add("jungle", new Slider("Save {0}% Mana", 30));

            KillSteal = MenuIni.AddSubMenu("KillSteal ", "KillSteal");
            KillSteal.AddGroupLabel("KillSteal Settings");
            KillSteal.Add("Q", new CheckBox("Use Q "));
            KillSteal.Add("W", new CheckBox("Use W "));
            KillSteal.Add("E", new CheckBox("Use E "));

            Draw = MenuIni.AddSubMenu("Drawings ", "Drawings");
            Draw.AddGroupLabel("Drawings Settings");
            Draw.Add("Q", new CheckBox("Draw Q "));
            Draw.Add("W", new CheckBox("Draw W "));
            Draw.Add("E", new CheckBox("Draw E "));

            Misc = MenuIni.AddSubMenu("Misc ", "Misc");
            Misc.AddGroupLabel("Spells HitChance");
            Misc.Add("hitChance", new ComboBox("HitChance", 0, "High", "Medium", "Low"));
        }
    }
}