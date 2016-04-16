namespace KappaUtility.Misc
{
    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Menu.Values;

    internal class AutoLvlUp
    {
        public static Slider level1;

        public static Slider level2;

        public static Slider level3;

        public static Slider level4;

        public static Slider level5;

        public static Slider level6;

        public static Slider level7;

        public static Slider level8;

        public static Slider level9;

        public static Slider level10;

        public static Slider level11;

        public static Slider level12;

        public static Slider level13;

        public static Slider level14;

        public static Slider level15;

        public static Slider level16;

        public static Slider level17;

        public static Slider level18;

        public static int[] LevelSet;

        public static int[] level = { 0, 0, 0, 0 };

        public static int QOff = 0, WOff = 0, EOff = 0, ROff;

        public static Menu LevelMenu { get; private set; }

        protected static bool loaded = false;

        internal static void OnLoad()
        {
            LevelMenu = Load.UtliMenu.AddSubMenu("AutoLvlUP");
            LevelMenu.AddGroupLabel("AutoLeveler Settings");
            LevelMenu.Add(Player.Instance.ChampionName + "enable1", new CheckBox("Enable", false));
            LevelMenu.AddSeparator();
            LevelMenu.AddGroupLabel("Level Sets");
            LevelMenu.AddLabel("Q = 1 | W = 2 | E = 3 | R = 4");
            LevelMenu.AddSeparator();

            level1 = LevelMenu.Add(Player.Instance.ChampionName + "1", new Slider("Level 1", 1, 1, 4));
            level1.OnValueChange += delegate { Getset(); };

            level2 = LevelMenu.Add(Player.Instance.ChampionName + "2", new Slider("Level 2", 2, 1, 4));
            level2.OnValueChange += delegate { Getset(); };

            level3 = LevelMenu.Add(Player.Instance.ChampionName + "3", new Slider("Level 3", 3, 1, 4));
            level3.OnValueChange += delegate { Getset(); };

            level4 = LevelMenu.Add(Player.Instance.ChampionName + "4", new Slider("Level 4", 1, 1, 4));
            level4.OnValueChange += delegate { Getset(); };

            level5 = LevelMenu.Add(Player.Instance.ChampionName + "5", new Slider("Level 5", 1, 1, 4));
            level5.OnValueChange += delegate { Getset(); };

            level6 = LevelMenu.Add(Player.Instance.ChampionName + "6", new Slider("Level 6", 4, 1, 4));
            level5.OnValueChange += delegate { Getset(); };

            level7 = LevelMenu.Add(Player.Instance.ChampionName + "7", new Slider("Level 7", 1, 1, 4));
            level7.OnValueChange += delegate { Getset(); };

            level8 = LevelMenu.Add(Player.Instance.ChampionName + "8", new Slider("Level 8", 2, 1, 4));
            level8.OnValueChange += delegate { Getset(); };

            level9 = LevelMenu.Add(Player.Instance.ChampionName + "9", new Slider("Level 9", 1, 1, 4));
            level9.OnValueChange += delegate { Getset(); };

            level10 = LevelMenu.Add(Player.Instance.ChampionName + "10", new Slider("Level 10", 2, 1, 4));
            level10.OnValueChange += delegate { Getset(); };

            level11 = LevelMenu.Add(Player.Instance.ChampionName + "11", new Slider("Level 11", 4, 1, 4));
            level11.OnValueChange += delegate { Getset(); };

            level12 = LevelMenu.Add(Player.Instance.ChampionName + "12", new Slider("Level 12", 2, 1, 4));
            level12.OnValueChange += delegate { Getset(); };

            level13 = LevelMenu.Add(Player.Instance.ChampionName + "13", new Slider("Level 13", 3, 1, 4));
            level13.OnValueChange += delegate { Getset(); };

            level14 = LevelMenu.Add(Player.Instance.ChampionName + "14", new Slider("Level 14", 2, 1, 4));
            level14.OnValueChange += delegate { Getset(); };

            level15 = LevelMenu.Add(Player.Instance.ChampionName + "15", new Slider("Level 15", 3, 1, 4));
            level15.OnValueChange += delegate { Getset(); };

            level16 = LevelMenu.Add(Player.Instance.ChampionName + "16", new Slider("Level 16", 4, 1, 4));
            level16.OnValueChange += delegate { Getset(); };

            level17 = LevelMenu.Add(Player.Instance.ChampionName + "17", new Slider("Level 17", 3, 1, 4));
            level17.OnValueChange += delegate { Getset(); };

            level18 = LevelMenu.Add(Player.Instance.ChampionName + "18", new Slider("Level 18", 3, 1, 4));
            level18.OnValueChange += delegate { Getset(); };

            LevelMenu.AddSeparator();
            LevelMenu.AddGroupLabel("Leveling Delay");
            LevelMenu.Add("delay", new Slider("LevelUP Delay {0} sec", 5, 0, 15));

            Getset();
            loaded = true;
        }

        internal static void Getset()
        {
            LevelSet = new[]
                           {
                               level1.Cast<Slider>().CurrentValue, level2.Cast<Slider>().CurrentValue, level3.Cast<Slider>().CurrentValue,
                               level4.Cast<Slider>().CurrentValue, level5.Cast<Slider>().CurrentValue, level6.Cast<Slider>().CurrentValue,
                               level7.Cast<Slider>().CurrentValue, level8.Cast<Slider>().CurrentValue, level9.Cast<Slider>().CurrentValue,
                               level10.Cast<Slider>().CurrentValue, level11.Cast<Slider>().CurrentValue, level12.Cast<Slider>().CurrentValue,
                               level13.Cast<Slider>().CurrentValue, level14.Cast<Slider>().CurrentValue, level15.Cast<Slider>().CurrentValue,
                               level16.Cast<Slider>().CurrentValue, level17.Cast<Slider>().CurrentValue, level18.Cast<Slider>().CurrentValue
                           };
        }

        internal static void Levelup()
        {
            if (!loaded)
            {
                return;
            }

            if (!LevelMenu[Player.Instance.ChampionName + "enable1"].Cast<CheckBox>().CurrentValue)
            {
                return;
            }

            int qL = Player.Instance.Spellbook.GetSpell(SpellSlot.Q).Level + QOff;
            int wL = Player.Instance.Spellbook.GetSpell(SpellSlot.W).Level + WOff;
            int eL = Player.Instance.Spellbook.GetSpell(SpellSlot.E).Level + EOff;
            int rL = Player.Instance.Spellbook.GetSpell(SpellSlot.R).Level + ROff;

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
                    levelup(SpellSlot.Q);
                }
                if (wL < level[1])
                {
                    levelup(SpellSlot.W);
                }
                if (eL < level[2])
                {
                    levelup(SpellSlot.E);
                }
                if (rL < level[3])
                {
                    levelup(SpellSlot.R);
                }
            }
        }

        internal static void levelup(SpellSlot slot)
        {
            Core.DelayAction(() => { ObjectManager.Player.Spellbook.LevelSpell(slot); }, LevelMenu["delay"].Cast<Slider>().CurrentValue * 1000);
        }
    }
}