namespace AutoSteal
{
    using System;

    using AutoSteal.Library;

    using EloBuddy;
    using EloBuddy.SDK.Events;
    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Menu.Values;

    public class Program
    {
        public static Menu menuIni;

        public static Menu KillStealMenu;

        public static Menu DrawMenu;

        public static Menu JungleStealMenu;

        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoad;
        }

        public static void OnLoad(EventArgs args)
        {
            var champion = ObjectManager.Player.ChampionName;
            menuIni = MainMenu.AddMenu("Auto Steal ", "Auto Steal");

            KillStealMenu = menuIni.AddSubMenu("Kill Steal ", "Kill Steal");
            KillStealMenu.AddGroupLabel("Kill Steal Settings");
            KillStealMenu.Add(
                champion + "EnableKST",
                new KeyBind("Enable Kill Steal Toggle", true, KeyBind.BindTypes.PressToggle, 'M'));
            KillStealMenu.Add(
                champion + "EnableKSA",
                new KeyBind("Enable Kill Steal Active", false, KeyBind.BindTypes.HoldActive));
            KillStealMenu.AddSeparator();
            KillStealMenu.AddGroupLabel(champion + " Kill Steal Spells");
            KillStealMenu.Add(champion + "AAC", new CheckBox("Use AA "));
            KillStealMenu.Add(champion + "QC", new CheckBox("Use Q "));
            KillStealMenu.Add(champion + "WC", new CheckBox("Use W "));
            KillStealMenu.Add(champion + "EC", new CheckBox("Use E "));
            KillStealMenu.Add(champion + "RC", new CheckBox("Use R "));
            KillStealMenu.AddSeparator();
            KillStealMenu.AddGroupLabel("Select Champions");
            foreach (var enemy in ObjectManager.Get<AIHeroClient>())
            {
                CheckBox cb = new CheckBox(enemy.BaseSkinName);
                cb.CurrentValue = true;
                if (enemy.Team != Player.Instance.Team)
                {
                    KillStealMenu.Add(champion + "Steal" + enemy.BaseSkinName, cb);
                }
            }

            JungleStealMenu = menuIni.AddSubMenu("Jungle Steal ", "Jungle Steal");
            JungleStealMenu.AddGroupLabel("Jungle Steal Settings");
            JungleStealMenu.Add(
                champion + "EnableJST",
                new KeyBind("Enable Jungle Steal Toggle", true, KeyBind.BindTypes.PressToggle, 'M'));
            JungleStealMenu.Add(
                champion + "EnableJSA",
                new KeyBind("Enable Jungle Steal Active", false, KeyBind.BindTypes.HoldActive));
            JungleStealMenu.AddSeparator();
            JungleStealMenu.AddGroupLabel(champion + " Jungle Steal Spells");
            JungleStealMenu.Add(champion + "AAJ", new CheckBox("Use AA "));
            JungleStealMenu.Add(champion + "QJ", new CheckBox("Use Q "));
            JungleStealMenu.Add(champion + "WJ", new CheckBox("Use W "));
            JungleStealMenu.Add(champion + "EJ", new CheckBox("Use E "));
            JungleStealMenu.Add(champion + "RJ", new CheckBox("Use R "));

            JungleStealMenu.AddSeparator();
            JungleStealMenu.AddGroupLabel("Select Jungle Monsters");
            JungleStealMenu.Add(champion + "blue", new CheckBox("Steal Blue "));
            JungleStealMenu.Add(champion + "red", new CheckBox("Steal Red "));
            JungleStealMenu.Add(champion + "baron", new CheckBox("Steal Baron "));
            JungleStealMenu.Add(champion + "drake", new CheckBox("Steal Dragon "));
            JungleStealMenu.Add(champion + "gromp", new CheckBox("Steal Gromp "));
            JungleStealMenu.Add(champion + "krug", new CheckBox("Steal Krug "));
            JungleStealMenu.Add(champion + "razorbeak", new CheckBox("Steal Razorbeak "));
            JungleStealMenu.Add(champion + "crab", new CheckBox("Steal Crab "));
            JungleStealMenu.Add(champion + "murkwolf", new CheckBox("Steal Murkwolf "));

            DrawMenu = menuIni.AddSubMenu("Debug", "Debug");
            DrawMenu.AddGroupLabel("Debug Settings");
            DrawMenu.Add(champion + "debug", new CheckBox("Enable Debug Drawings", false));
            DrawMenu.AddGroupLabel("Position");
            DrawMenu.Add("trackx", new Slider("Debug Position X", 0, 0, 100));
            DrawMenu.Add("tracky", new Slider("Debug Position Y", 0, 0, 100));

            SpellManager.Initialize();
            SpellLibrary.Initialize();

            Drawing.OnEndScene += Drawing_OnDraw;
            Game.OnUpdate += OnUpdate;
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            if (DrawMenu[Player.Instance.ChampionName + "debug"].Cast<CheckBox>().CurrentValue)
            {
                Modes.Draw.DebugKS();
                Modes.Draw.DebugJS();
            }
        }

        private static void OnUpdate(EventArgs args)
        {
            var champion = ObjectManager.Player.ChampionName;
            if (KillStealMenu[champion + "EnableKST"].Cast<KeyBind>().CurrentValue
                || KillStealMenu[champion + "EnableKSA"].Cast<KeyBind>().CurrentValue)
            {
                Modes.KillSteal.KS();
            }

            if (JungleStealMenu[champion + "EnableJST"].Cast<KeyBind>().CurrentValue
                || JungleStealMenu[champion + "EnableJSA"].Cast<KeyBind>().CurrentValue)
            {
                Modes.JungleSteal.JS();
            }
        }
    }
}