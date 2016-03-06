namespace AutoSteal
{
    using EloBuddy;
    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Menu.Values;
    using GenesisSpellLibrary;

    class Start
    {
        public static Menu menuIni;
        public static Menu KillSteal;
        public static Menu JungleSteal;
        public static void Startload()
        {
            menuIni = MainMenu.AddMenu("Auto Steal ", "Auto Steal");

            KillSteal = menuIni.AddSubMenu("Kill Steal ", "Kill Steal");
            KillSteal.AddGroupLabel("Kill Steal Settings");
            KillSteal.Add("EnableKS", new CheckBox("Enable Kill Steal "));
            KillSteal.AddGroupLabel(ObjectManager.Player.ChampionName + " Kill Steal Spells");
            KillSteal.Add("AAC", new CheckBox("Use AA "));
            KillSteal.Add("QC", new CheckBox("Use Q "));
            KillSteal.Add("WC", new CheckBox("Use W "));
            KillSteal.Add("EC", new CheckBox("Use E "));
            KillSteal.Add("RC", new CheckBox("Use R "));
            KillSteal.AddSeparator();
            KillSteal.AddGroupLabel("Select Champions");
            foreach (var enemy in ObjectManager.Get<AIHeroClient>())
            {
                CheckBox cb = new CheckBox(enemy.BaseSkinName);
                cb.CurrentValue = false;
                if (enemy.Team != Player.Instance.Team)
                {
                    KillSteal.Add("DontSteal" + enemy.BaseSkinName, cb);
                }
            }


            JungleSteal = menuIni.AddSubMenu("Jungle Steal ", "Jungle Steal");
            JungleSteal.AddGroupLabel("Jungle Steal Settings");
            JungleSteal.Add("EnableJS", new CheckBox("Enable Jungle Steal "));
            JungleSteal.AddSeparator();
            JungleSteal.AddGroupLabel(ObjectManager.Player.ChampionName + " Jungle Steal Spells");
            JungleSteal.Add("AAJ", new CheckBox("Use AA "));
            JungleSteal.Add("QJ", new CheckBox("Use Q "));
            JungleSteal.Add("WJ", new CheckBox("Use W "));
            JungleSteal.Add("EJ", new CheckBox("Use E "));
            JungleSteal.Add("RJ", new CheckBox("Use R "));

            JungleSteal.AddSeparator();
            JungleSteal.AddGroupLabel("Select Jungle Monsters");
            JungleSteal.Add("blue", new CheckBox("Steal Blue "));
            JungleSteal.Add("red", new CheckBox("Steal Red "));
            JungleSteal.Add("baron", new CheckBox("Steal Baron "));
            JungleSteal.Add("drake", new CheckBox("Steal Dragon "));
            JungleSteal.Add("gromp", new CheckBox("Steal Gromp "));
            JungleSteal.Add("krug", new CheckBox("Steal Krug "));
            JungleSteal.Add("razorbeak", new CheckBox("Steal Razorbeak "));
            JungleSteal.Add("crab", new CheckBox("Steal Crab "));
            JungleSteal.Add("murkwolf", new CheckBox("Steal Murkwolf "));


            SpellManager.Initialize();
            SpellLibrary.Initialize();
        }
    }
}
