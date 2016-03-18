namespace KappaUtility.Misc
{
    using System;

    using EloBuddy;
    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Menu.Values;

    internal class SkinHax
    {
        public static Menu SkinMenu { get; private set; }

        internal static void OnLoad()
        {
            SkinMenu = Load.UtliMenu.AddSubMenu("Skins Hax");
            SkinMenu.AddGroupLabel("Skin Settings");
            SkinMenu.Add(Player.Instance.ChampionName + "skin", new CheckBox("Enable", false));
            SkinMenu.Add(Player.Instance.ChampionName + "skins", new Slider("Select Skin", 0, 0, 15));

            Game.OnUpdate += Game_OnUpdate;
        }

        private static void Game_OnUpdate(EventArgs args)
        {
            if (SkinMenu[Player.Instance.ChampionName + "skin"].Cast<CheckBox>().CurrentValue && !Player.Instance.IsDead)
            {
                Player.Instance.SetSkinId(SkinMenu[Player.Instance.ChampionName + "skins"].Cast<Slider>().CurrentValue);
            }
        }
    }
}