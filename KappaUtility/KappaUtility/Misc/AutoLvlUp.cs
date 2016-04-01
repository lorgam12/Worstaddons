namespace KappaUtility.Misc
{
    using System;

    using EloBuddy;
    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Menu.Values;

    internal class AutoLvlUp
    {
        public static ComboBox levels;

        public static int[] LevelSet;

        public static int[] level = { 0, 0, 0, 0 };

        public static int QOff = 0, WOff = 0, EOff = 0, ROff;

        public static Menu LevelMenu { get; private set; }

        internal static void OnLoad()
        {
            LevelMenu = Load.UtliMenu.AddSubMenu("AutoLvlUP");
            LevelMenu.AddGroupLabel("AutoLeveler Settings");
            LevelMenu.Add(Player.Instance.ChampionName + "enable", new CheckBox("Enable", false));
            LevelMenu.AddSeparator();
            LevelMenu.AddGroupLabel("Level Sets");
            levels = LevelMenu.Add(
                Player.Instance.ChampionName + "sets",
                new ComboBox(
                    "Level Sets",
                    0,
                    "R > Q > W > E",
                    "R > Q > E > W",
                    "R > W > Q > E",
                    "R > W > E > Q",
                    "R > E > Q > W",
                    "R > E > W > Q"));
            levels.OnValueChange += delegate
                {
                    switch (levels.Cast<ComboBox>().CurrentValue)
                    {
                        case 0:
                            {
                                LevelSet = new[] { 1, 2, 3, 1, 1, 4, 1, 2, 1, 2, 4, 2, 3, 2, 3, 4, 3, 3 };
                            }
                            break;

                        case 1:
                            {
                                LevelSet = new[] { 1, 3, 2, 1, 1, 4, 1, 3, 1, 3, 4, 3, 2, 3, 2, 4, 2, 2 };
                            }
                            break;

                        case 2:
                            {
                                LevelSet = new[] { 2, 1, 3, 2, 2, 4, 2, 1, 2, 1, 4, 1, 3, 1, 3, 4, 3, 3 };
                            }
                            break;

                        case 3:
                            {
                                LevelSet = new[] { 2, 3, 1, 2, 2, 4, 2, 3, 2, 3, 4, 3, 1, 3, 1, 4, 1, 1 };
                            }
                            break;

                        case 4:
                            {
                                LevelSet = new[] { 3, 1, 2, 3, 3, 4, 3, 1, 3, 1, 4, 1, 2, 1, 2, 4, 2, 2 };
                            }
                            break;

                        case 5:
                            {
                                LevelSet = new[] { 3, 2, 1, 3, 3, 4, 3, 2, 3, 2, 4, 2, 1, 2, 1, 4, 1, 1 };
                            }
                            break;
                    }
                };
            LevelMenu.AddSeparator();
            LevelMenu.AddGroupLabel("Humanizer Delay [Not Implemented yet]");
            LevelMenu.Add("min", new Slider("Min LevelUP Delay", 1000, 0, 2500));
            LevelMenu.Add("max", new Slider("Max LevelUP Delay", 2501, 2501, 5000));

            loadfirstset();
        }

        internal static void loadfirstset()
        {
            switch (levels.Cast<ComboBox>().CurrentValue)
            {
                case 0:
                    {
                        LevelSet = new[] { 1, 2, 3, 1, 1, 4, 1, 2, 1, 2, 4, 2, 3, 2, 3, 4, 3, 3 };
                    }
                    break;

                case 1:
                    {
                        LevelSet = new[] { 1, 3, 2, 1, 1, 4, 1, 3, 1, 3, 4, 3, 2, 3, 2, 4, 2, 2 };
                    }
                    break;

                case 2:
                    {
                        LevelSet = new[] { 2, 1, 3, 2, 2, 4, 2, 1, 2, 1, 4, 1, 3, 1, 3, 4, 3, 3 };
                    }
                    break;

                case 3:
                    {
                        LevelSet = new[] { 2, 3, 1, 2, 2, 4, 2, 3, 2, 3, 4, 3, 1, 3, 1, 4, 1, 1 };
                    }
                    break;

                case 4:
                    {
                        LevelSet = new[] { 3, 1, 2, 3, 3, 4, 3, 1, 3, 1, 4, 1, 2, 1, 2, 4, 2, 2 };
                    }
                    break;

                case 5:
                    {
                        LevelSet = new[] { 3, 2, 1, 3, 3, 4, 3, 2, 3, 2, 4, 2, 1, 2, 1, 4, 1, 1 };
                    }
                    break;
            }
        }

        internal static void Levelup()
        {
            if (!LevelMenu[Player.Instance.ChampionName + "enable"].Cast<CheckBox>().CurrentValue)
            {
                return;
            }

            int qL = Player.Instance.Spellbook.GetSpell(SpellSlot.Q).Level + QOff;
            int wL = Player.Instance.Spellbook.GetSpell(SpellSlot.W).Level + WOff;
            int eL = Player.Instance.Spellbook.GetSpell(SpellSlot.E).Level + EOff;
            int rL = Player.Instance.Spellbook.GetSpell(SpellSlot.R).Level + ROff;

            var min = LevelMenu["min"].Cast<Slider>().CurrentValue;
            var max = LevelMenu["max"].Cast<Slider>().CurrentValue;

            Random rnd = new Random();
            int random = rnd.Next(min, max);

            level = new[] { 0, 0, 0, 0 };
            if (qL + wL + eL + rL < Player.Instance.Level)
            {
                for (int i = 0; i < Player.Instance.Level; i++)
                {
                    if (LevelSet != null)
                    {
                        level[LevelSet[i] - 1] = level[LevelSet[i] - 1] + 1;
                    }
                }
                if (qL < level[0])
                {
                    ObjectManager.Player.Spellbook.LevelSpell(SpellSlot.Q);
                }
                if (wL < level[1])
                {
                    ObjectManager.Player.Spellbook.LevelSpell(SpellSlot.W);
                }
                if (eL < level[2])
                {
                    ObjectManager.Player.Spellbook.LevelSpell(SpellSlot.E);
                }
                if (rL < level[3])
                {
                    ObjectManager.Player.Spellbook.LevelSpell(SpellSlot.R);
                }
            }
        }
    }
}