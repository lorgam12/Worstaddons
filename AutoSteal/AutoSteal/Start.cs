using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoSteal
{
    using AutoSteal.Champs;
    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Enumerations;
    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Menu.Values;

    class Start
    {
        public static Menu menuIni;
        public static void Startload()
        {

            menuIni = MainMenu.AddMenu("AutoSteal ", "AutoSteal");
            menuIni.AddGroupLabel("Auto Steal Kills");
            menuIni.Add("AAC", new CheckBox("Use AA "));
            menuIni.AddGroupLabel("Auto Steal Jungle Mobs");
            menuIni.Add("AAJ", new CheckBox("Use AA "));
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
                LeeSin.LeeMenu.AddSeparator();
                LeeSin.LeeMenu.AddGroupLabel("Auto Steal Jungle");
                LeeSin.LeeMenu.Add("QJ", new CheckBox("Use Q "));
                LeeSin.LeeMenu.Add("EJ", new CheckBox("Use E "));

                LeeSin.Q = new Spell.Skillshot(SpellSlot.Q, 1100, SkillShotType.Linear, 250, 1800, 50)
                {
                    AllowedCollisionCount = 0
                };
                LeeSin.E = new Spell.Active(SpellSlot.E, 350);
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
        }
    }
}
