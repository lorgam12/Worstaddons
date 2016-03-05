namespace AutoSteal
{
    using Champs;
    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Enumerations;
    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Menu.Values;

    class Start
    {
        public static Menu menuIni;
        public static Menu Junglemobs;
        public static void Startload()
        {

            menuIni = MainMenu.AddMenu("AutoSteal ", "AutoSteal");
            menuIni.AddGroupLabel("Auto Steal Kills");
            menuIni.Add("AAC", new CheckBox("Use AA "));
            menuIni.AddGroupLabel("Auto Steal Jungle Mobs");
            menuIni.Add("AAJ", new CheckBox("Use AA "));

            Junglemobs = menuIni.AddSubMenu("Jungle Monsters ", "Jungle Monster");
            Junglemobs.AddGroupLabel("Select Jungle Monsters");
            Junglemobs.Add("blue", new CheckBox("Steal Blue "));
            Junglemobs.Add("red", new CheckBox("Steal Red "));
            Junglemobs.Add("baron", new CheckBox("Steal Baron "));
            Junglemobs.Add("drake", new CheckBox("Steal Dragon "));
            Junglemobs.Add("gromp", new CheckBox("Steal Gromp "));
            Junglemobs.Add("krug", new CheckBox("Steal Krug "));
            Junglemobs.Add("razorbeak", new CheckBox("Steal Razorbeak "));
            Junglemobs.Add("crab", new CheckBox("Steal Crab "));
            Junglemobs.Add("murkwolf", new CheckBox("Steal Murkwolf "));
            Junglemobs.AddLabel("If Champion Isn't Supported will only try to AA");


            if (ObjectManager.Player.ChampionName == "Gragas")
            {
                Gragas.GragMenu = menuIni.AddSubMenu("Gragas ");
                Gragas.GragMenu.AddGroupLabel("Auto Steal Kills");
                Gragas.GragMenu.Add("QC", new CheckBox("Use Q "));
                Gragas.GragMenu.Add("EC", new CheckBox("Use E "));
                Gragas.GragMenu.Add("RC", new CheckBox("Use R "));
                Gragas.GragMenu.AddSeparator();
                Gragas.GragMenu.AddGroupLabel("Auto Steal Jungle");
                Gragas.GragMenu.Add("QJ", new CheckBox("Use Q "));
                Gragas.GragMenu.Add("EJ", new CheckBox("Use E "));
                Gragas.GragMenu.Add("RJ", new CheckBox("Use R "));

                Gragas.Q = new Spell.Skillshot(SpellSlot.Q, 775, SkillShotType.Circular, 1, 1000, 100)
                {
                    AllowedCollisionCount = int.MaxValue
                };

                Gragas.E = new Spell.Skillshot(SpellSlot.E, 675, SkillShotType.Linear, 0, 1000, 50)
                {
                    AllowedCollisionCount = 0
                };

                Gragas.R = new Spell.Skillshot(SpellSlot.R, 1100, SkillShotType.Circular, 1, 1000, 500)
                {
                    AllowedCollisionCount = int.MaxValue
                };
            }

            if (ObjectManager.Player.ChampionName == "Jinx")
            {
                Jinx.JinxMenu = menuIni.AddSubMenu("Jinx ");
                Jinx.JinxMenu.AddGroupLabel("Auto Steal Kills");
                Jinx.JinxMenu.Add("WC", new CheckBox("Use W "));
                Jinx.JinxMenu.Add("RC", new CheckBox("Use R "));
                Jinx.JinxMenu.AddSeparator();
                Jinx.JinxMenu.AddGroupLabel("Auto Steal Jungle");
                Jinx.JinxMenu.Add("WJ", new CheckBox("Use W "));

                Jinx.W = new Spell.Skillshot(SpellSlot.W, 1500, SkillShotType.Linear, 500, 3300, 50)
                             {
                                 AllowedCollisionCount = 0 
                             };
                Jinx.R = new Spell.Skillshot(SpellSlot.R, 3000, SkillShotType.Linear, 500, 1500, 100);
            }

            if (ObjectManager.Player.ChampionName == "Katarina")
            {
                Katarina.KataMenu = menuIni.AddSubMenu("Katarina ");
                Katarina.KataMenu.AddGroupLabel("Auto Steal Kills");
                Katarina.KataMenu.Add("QC", new CheckBox("Use Q "));
                Katarina.KataMenu.Add("WC", new CheckBox("Use W "));
                Katarina.KataMenu.Add("EC", new CheckBox("Use E "));
                Katarina.KataMenu.AddSeparator();
                Katarina.KataMenu.AddGroupLabel("Auto Steal Jungle");
                Katarina.KataMenu.Add("QJ", new CheckBox("Use Q "));
                Katarina.KataMenu.Add("WJ", new CheckBox("Use W "));
                Katarina.KataMenu.Add("EJ", new CheckBox("Use E "));

                Katarina.Q = new Spell.Targeted(SpellSlot.Q, 675);
                Katarina.W = new Spell.Active(SpellSlot.W, 375);
                Katarina.E = new Spell.Targeted(SpellSlot.E, 700);
            }

            if (ObjectManager.Player.ChampionName == "LeeSin")
            {
                LeeSin.LeeMenu = menuIni.AddSubMenu("LeeSin ");
                LeeSin.LeeMenu.AddGroupLabel("Auto Steal Kills");
                LeeSin.LeeMenu.Add("QC", new CheckBox("Use Q "));
                LeeSin.LeeMenu.Add("EC", new CheckBox("Use E "));
                LeeSin.LeeMenu.Add("RC", new CheckBox("Use R "));
                LeeSin.LeeMenu.AddSeparator();
                LeeSin.LeeMenu.AddGroupLabel("Auto Steal Jungle");
                LeeSin.LeeMenu.Add("QJ", new CheckBox("Use Q "));
                LeeSin.LeeMenu.Add("EJ", new CheckBox("Use E "));

                LeeSin.Q = new Spell.Skillshot(SpellSlot.Q, 1100, SkillShotType.Linear, 250, 1800, 50)
                {
                    AllowedCollisionCount = 0
                };
                LeeSin.E = new Spell.Active(SpellSlot.E, 350);
                LeeSin.R = new Spell.Targeted(SpellSlot.R, 375);
            }

            if (ObjectManager.Player.ChampionName == "Nidalee")
            {
                Nidalee.NidaMenu = menuIni.AddSubMenu("Nidalee ");
                Nidalee.NidaMenu.AddGroupLabel("Auto Steal Kills");
                Nidalee.NidaMenu.Add("QC", new CheckBox("Use Q "));
                Nidalee.NidaMenu.AddSeparator();
                Nidalee.NidaMenu.AddGroupLabel("Auto Steal Jungle");
                Nidalee.NidaMenu.Add("QJ", new CheckBox("Use Q "));

                Nidalee.Q = new Spell.Skillshot(SpellSlot.Q, 1500, SkillShotType.Linear, 125, 1300, 30)
                                {
                                    AllowedCollisionCount = 0 
                                };
            }

            if (ObjectManager.Player.ChampionName == "Riven")
            {
                Riven.RivenMenu = menuIni.AddSubMenu("Riven ");
                Riven.RivenMenu.AddGroupLabel("Auto Steal Kills");
                Riven.RivenMenu.Add("WC", new CheckBox("Use W "));
                Riven.RivenMenu.Add("RC", new CheckBox("Use R "));
                Riven.RivenMenu.AddSeparator();
                Riven.RivenMenu.AddGroupLabel("Auto Steal Jungle");
                Riven.RivenMenu.Add("WJ", new CheckBox("Use W "));
                Riven.RivenMenu.Add("RJ", new CheckBox("Use R "));

                Riven.W = new Spell.Active(SpellSlot.W, 125);
                Riven.R = new Spell.Skillshot(SpellSlot.R, 875, SkillShotType.Cone, 250, 1600, 150)
                {
                    AllowedCollisionCount = int.MaxValue
                };
            }

            if (ObjectManager.Player.ChampionName == "Sivir")
            {
                Sivir.SivMenu = menuIni.AddSubMenu("Sivir ");
                Sivir.SivMenu.AddGroupLabel("Auto Steal Kills");
                Sivir.SivMenu.Add("QC", new CheckBox("Use Q "));
                Sivir.SivMenu.AddSeparator();
                Sivir.SivMenu.AddGroupLabel("Auto Steal Jungle");
                Sivir.SivMenu.Add("QJ", new CheckBox("Use Q "));

                Sivir.Q = new Spell.Skillshot(SpellSlot.Q, 1245, SkillShotType.Linear, (int)0.25, 1030, 90)
                {
                    AllowedCollisionCount = int.MaxValue
                };
            }

            if (ObjectManager.Player.ChampionName == "Thresh")
            {
                Thresh.ThreshMenu = menuIni.AddSubMenu("Thresh ");
                Thresh.ThreshMenu.AddGroupLabel("Auto Steal Kills");
                Thresh.ThreshMenu.Add("QC", new CheckBox("Use Q "));
                Thresh.ThreshMenu.Add("EC", new CheckBox("Use E "));
                Thresh.ThreshMenu.AddSeparator();
                Thresh.ThreshMenu.AddGroupLabel("Auto Steal Jungle");
                Thresh.ThreshMenu.Add("QJ", new CheckBox("Use Q "));
                Thresh.ThreshMenu.Add("EJ", new CheckBox("Use E "));

                Thresh.Q = new Spell.Skillshot(SpellSlot.Q, 1040, SkillShotType.Linear, 500, 1900, 60)
                {
                    AllowedCollisionCount = 0
                };
                Thresh.E = new Spell.Skillshot(SpellSlot.E, 480, SkillShotType.Linear, 0, 2000, 110)
                {
                    AllowedCollisionCount = int.MaxValue
                };
            }
        }
    }
}
