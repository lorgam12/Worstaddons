namespace KappaUtilityReborn.Main
{
    using System;

    using EloBuddy;

    public abstract class Base
    {
        public static AIHeroClient user = Player.Instance;

        public abstract void Initialize();

        public abstract void OnTick();

        public abstract void Draw();

        protected Base()
        {
            Game.OnTick += this.Game_OnTick;
            Drawing.OnEndScene += this.Drawing_OnEndScene;
            this.Initialize();
        }

        protected virtual void Drawing_OnEndScene(EventArgs args)
        {
            this.Draw();
        }

        protected virtual void Game_OnTick(EventArgs args)
        {
            this.OnTick();
        }
    }
}
