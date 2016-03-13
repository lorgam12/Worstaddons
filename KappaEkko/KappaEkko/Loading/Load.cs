namespace KappaEkko
{
    using System;

    using EloBuddy;
    using EloBuddy.SDK.Events;

    using Events;

    internal class Load
    {
        public static AIHeroClient Player = ObjectManager.Player;

        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            Player = ObjectManager.Player;

            if (Player.ChampionName != "Ekko")
            {
                return;
            }

            Menu.Load();
            Spells.Load();

            Game.OnUpdate += OnUpdate.Update;
            Drawing.OnDraw += OnDraw.Draw;
            Obj_AI_Base.OnProcessSpellCast += OnProcessSpellCast.OnSpell;
            GameObject.OnCreate += OnCreate.Create;
            GameObject.OnDelete += OnDelete.Delete;
            Obj_AI_Base.OnBasicAttack += OnBasicAttack.OnAttack;
        }
    }
}