namespace KappAzir.Modes
{
    using System.Linq;

    using EloBuddy;
    using EloBuddy.SDK;

    using SharpDX;
    using static Menus;
    using static SpellsManager;
    using Mario_s_Lib;

    internal class Flee : ModeManager
    {
        internal static Vector3 soldposition;

        public static void Execute()
        {
        var allready = Q.IsReady() && E.IsReady() && W.IsReady();
            if (Orbwalker.AzirSoldiers.Count(s => s.IsAlly) < 1 && allready && ManaCheck(Azir) < Azir.Mana)
            {
                W.Cast(Azir.Position.Extend(Game.CursorPos, W.Range).To3D());
            }

            if (Orbwalker.AzirSoldiers.Count(s => s.IsAlly) > 0)
            {
                var time = ((Azir.ServerPosition.Distance(soldposition) / E.Speed) * 1000) - ((Game.Ping + FleeMenu.GetSliderValue("delay")));
                if (Orbwalker.AzirSoldiers.OrderBy(s => s.Distance(Game.CursorPos)).FirstOrDefault() != null)
                {
                    soldposition = Orbwalker.AzirSoldiers.OrderBy(s => s.Distance(Game.CursorPos)).FirstOrDefault().ServerPosition;
                }
                if (E.Cast(Azir.Position.Extend(Game.CursorPos, E.Range).To3D()))
                {
                    Core.DelayAction(() => { Q.Cast(Azir.Position.Extend(Game.CursorPos, Q.Range).To3D()); }, (int)time);
                }
            }
        }
    }
}