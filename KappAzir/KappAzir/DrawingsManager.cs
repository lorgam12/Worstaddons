namespace KappAzir
{
    using System;
    using System.Linq;

    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Rendering;

    using Mario_s_Lib;

    using SharpDX;
    using static SpellsManager;
    using static Menus;

    internal class DrawingsManager
    {
        public static void InitializeDrawings()
        {
            Drawing.OnDraw += Drawing_OnDraw;
            Drawing.OnEndScene += Drawing_OnEndScene;
            DamageIndicator.Init();
        }

        /// <summary>
        /// Normal drawings
        /// </summary>
        /// <param name="args"></param>
        private static void Drawing_OnDraw(EventArgs args)
        {
            var readyDraw = DrawingsMenu.GetCheckBoxValue("readyDraw");

            var rect = new Geometry.Polygon.Rectangle(Player.Instance.Front(350), Player.Instance.Back(300), R.Width);
            var Target = TargetSelector.GetTarget(R.Range, DamageType.Magical);
            var tower = EntityManager.Turrets.Allies.FirstOrDefault(it => it.IsValidTarget(1000));
            ;
            if (DrawingsMenu.GetCheckBoxValue("qDraw") && readyDraw ? Q.IsReady() : DrawingsMenu.GetCheckBoxValue("qDraw"))
            {
                Circle.Draw(QColorSlide.GetSharpColor(), Q.Range, 1f, Player.Instance);
            }

            if (DrawingsMenu.GetCheckBoxValue("wDraw") && readyDraw ? W.IsReady() : DrawingsMenu.GetCheckBoxValue("wDraw"))
            {
                Circle.Draw(WColorSlide.GetSharpColor(), W.Range, 1f, Player.Instance);
            }

            if (DrawingsMenu.GetCheckBoxValue("eDraw") && readyDraw ? E.IsReady() : DrawingsMenu.GetCheckBoxValue("eDraw"))
            {
                Circle.Draw(EColorSlide.GetSharpColor(), E.Range, 1f, Player.Instance);
            }

            if (DrawingsMenu.GetCheckBoxValue("rDraw") && readyDraw ? R.IsReady() : DrawingsMenu.GetCheckBoxValue("rDraw"))
            {
                Circle.Draw(RColorSlide.GetSharpColor(), R.Range, 1f, Player.Instance);
            }
        }

        /// <summary>
        /// This drawing will override some of the lol`s, like healthbars menus and atc
        /// </summary>
        /// <param name="args"></param>
        private static void Drawing_OnEndScene(EventArgs args)
        {
        }
    }
}