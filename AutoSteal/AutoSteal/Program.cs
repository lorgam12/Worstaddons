namespace AutoSteal
{
    using System;

    using EloBuddy;
    using EloBuddy.SDK.Events;
    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Menu.Values;

    using GenesisSpellLibrary;

    public class Program
    {
        public static Menu menuIni;

        public static Menu KillStealMenu;

        public static Menu JungleStealMenu;

        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoad;
        }

        public static void OnLoad(EventArgs args)
        {
            menuIni = MainMenu.AddMenu("Auto Steal ", "Auto Steal");

            KillStealMenu = menuIni.AddSubMenu("Kill Steal ", "Kill Steal");
            KillStealMenu.AddGroupLabel("Kill Steal Settings");
            KillStealMenu.Add("EnableKST", new KeyBind("Enable Kill Steal Toggle", true, KeyBind.BindTypes.PressToggle, 'M'));
            KillStealMenu.Add("EnableKSA", new KeyBind("Enable Kill Steal Active", false, KeyBind.BindTypes.HoldActive));
            KillStealMenu.AddSeparator();
            KillStealMenu.AddGroupLabel(ObjectManager.Player.ChampionName + " Kill Steal Spells");
            KillStealMenu.Add("AAC", new CheckBox("Use AA "));
            KillStealMenu.Add("QC", new CheckBox("Use Q "));
            KillStealMenu.Add("WC", new CheckBox("Use W "));
            KillStealMenu.Add("EC", new CheckBox("Use E "));
            KillStealMenu.Add("RC", new CheckBox("Use R "));
            KillStealMenu.AddSeparator();
            KillStealMenu.AddGroupLabel("Select Champions");
            foreach (var enemy in ObjectManager.Get<AIHeroClient>())
            {
                CheckBox cb = new CheckBox(enemy.BaseSkinName);
                cb.CurrentValue = true;
                if (enemy.Team != Player.Instance.Team)
                {
                    KillStealMenu.Add("Steal" + enemy.BaseSkinName, cb);
                }
            }

            JungleStealMenu = menuIni.AddSubMenu("Jungle Steal ", "Jungle Steal");
            JungleStealMenu.AddGroupLabel("Jungle Steal Settings");
            JungleStealMenu.Add("EnableJST", new KeyBind("Enable Jungle Steal Toggle", true, KeyBind.BindTypes.PressToggle, 'M'));
            JungleStealMenu.Add("EnableJSA", new KeyBind("Enable Jungle Steal Active", false, KeyBind.BindTypes.HoldActive));
            JungleStealMenu.AddSeparator();
            JungleStealMenu.AddGroupLabel(ObjectManager.Player.ChampionName + " Jungle Steal Spells");
            JungleStealMenu.Add("AAJ", new CheckBox("Use AA "));
            JungleStealMenu.Add("QJ", new CheckBox("Use Q "));
            JungleStealMenu.Add("WJ", new CheckBox("Use W "));
            JungleStealMenu.Add("EJ", new CheckBox("Use E "));
            JungleStealMenu.Add("RJ", new CheckBox("Use R "));

            JungleStealMenu.AddSeparator();
            JungleStealMenu.AddGroupLabel("Select Jungle Monsters");
            JungleStealMenu.Add("blue", new CheckBox("Steal Blue "));
            JungleStealMenu.Add("red", new CheckBox("Steal Red "));
            JungleStealMenu.Add("baron", new CheckBox("Steal Baron "));
            JungleStealMenu.Add("drake", new CheckBox("Steal Dragon "));
            JungleStealMenu.Add("gromp", new CheckBox("Steal Gromp "));
            JungleStealMenu.Add("krug", new CheckBox("Steal Krug "));
            JungleStealMenu.Add("razorbeak", new CheckBox("Steal Razorbeak "));
            JungleStealMenu.Add("crab", new CheckBox("Steal Crab "));
            JungleStealMenu.Add("murkwolf", new CheckBox("Steal Murkwolf "));

            SpellManager.Initialize();
            SpellLibrary.Initialize();

            Game.OnUpdate += OnUpdate;
        }

        private static void OnUpdate(EventArgs args)
        {
            if (KillStealMenu["EnableKST"].Cast<KeyBind>().CurrentValue || KillStealMenu["EnableKSA"].Cast<KeyBind>().CurrentValue)
            {
                Modes.KillSteal.KS();
            }

            if (JungleStealMenu["EnableJST"].Cast<KeyBind>().CurrentValue || JungleStealMenu["EnableJSA"].Cast<KeyBind>().CurrentValue)
            {
                Modes.JungleSteal.JS();
            }
        }
    }
}