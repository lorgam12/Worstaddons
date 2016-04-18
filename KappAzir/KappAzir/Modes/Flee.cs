namespace KappAzir.Modes
{
    using System.Linq;

    using EloBuddy;
    using EloBuddy.SDK;
    using static Menus;
    using static SpellsManager;
    using Mario_s_Lib;

    internal class Flee : ModeManager
    {
        public static void Execute()
        {
            var allready = Q.IsReady() && E.IsReady() && W.IsReady();
            if (Orbwalker.AzirSoldiers.Count(s => s.IsAlly) < 1 && allready && ManaCheck(Azir) < Azir.Mana)
            {
                W.Cast(Azir.Position.Extend(Game.CursorPos, W.Range).To3D());
            }

            if (Orbwalker.AzirSoldiers.Count(s => s.IsAlly) > 0)
            {
                if (E.Cast(Azir.Position.Extend(Game.CursorPos, E.Range).To3D()))
                {
                    Core.DelayAction(() => { Q.Cast(Azir.Position.Extend(Game.CursorPos, Q.Range).To3D()); }, FleeMenu.GetSliderValue("delay"));
                }
            }
        }
    }
}