using System.Linq;

namespace KappAzir.Modes
{
    using EloBuddy;
    using EloBuddy.SDK;

    using Mario_s_Lib;

    using SharpDX;
    using static Menus;
    using static SpellsManager;

    class Jumper : ModeManager
    {
        internal static Vector3 soldposition;

        public static Obj_AI_Minion sold;

        public static void jump(Vector3 pos, Vector3 qpos)
        {
            var allready = Q.IsReady() && E.IsReady() && W.IsReady();

            if (Orbwalker.AzirSoldiers.Count(s => s.IsAlly) > 0 && allready && ManaCheck(Azir) < Azir.Mana)
            {
                sold = Orbwalker.AzirSoldiers.OrderBy(s => s.Distance(pos)).FirstOrDefault(s => s != null);
                soldposition = sold.ServerPosition;
                if (E.Cast(Azir.Position.Extend(sold, E.Range).To3D()))
                {
                    var time = ((Azir.ServerPosition.Distance(soldposition) / E.Speed) * 1000) - FleeMenu.GetSliderValue("delay");
                    Core.DelayAction(() => { Q.Cast(Azir.Position.Extend(qpos, Q.Range).To3D()); }, (int)time);
                }
            }
            if (Orbwalker.AzirSoldiers.Count(s => s.IsAlly) < 1 && allready && ManaCheck(Azir) < Azir.Mana)
            {
                if (W.Cast(Azir.Position.Extend(pos, W.Range).To3D()))
                {
                        if (E.Cast(Azir.Position.Extend(Game.CursorPos, E.Range).To3D()))
                        {
                            Core.DelayAction(() => { Q.Cast(Azir.Position.Extend(qpos, Q.Range).To3D()); }, FleeMenu.GetSliderValue("delay"));
                        }
                }
            }

            /*
            if (Orbwalker.AzirSoldiers.Count(s => s.IsAlly) > 0)
            {
                if (Orbwalker.AzirSoldiers.OrderBy(s => s.Distance(pos)).FirstOrDefault() != null)
                {
                    sold = Orbwalker.AzirSoldiers.OrderBy(s => s.Distance(pos)).FirstOrDefault();
                    soldposition = sold.ServerPosition;
                }
                if (E.Cast(Azir.Position.Extend(sold, E.Range).To3D()))
                {
                    var time = ((Azir.ServerPosition.Distance(soldposition) / E.Speed) * (1000 - FleeMenu.GetSliderValue("delay"))) - Game.Ping;
                    Core.DelayAction(() => { Q.Cast(Azir.Position.Extend(qpos, Q.Range).To3D()); }, (int)time);
                }
            }
            */
        }
    }
}
