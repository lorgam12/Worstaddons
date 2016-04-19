using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public static void jump(Vector2 pos)
        {
            var allready = Q.IsReady() && E.IsReady() && W.IsReady();
            if (Orbwalker.AzirSoldiers.Count(s => s.IsAlly) < 1 && allready && ManaCheck(Azir) < Azir.Mana)
            {
                W.Cast(Azir.Position.Extend(pos, W.Range).To3D());
            }

            if (Orbwalker.AzirSoldiers.Count(s => s.IsAlly) > 0)
            {
                if (Orbwalker.AzirSoldiers.OrderBy(s => s.Distance(pos)).FirstOrDefault() != null)
                {
                    sold = Orbwalker.AzirSoldiers.OrderBy(s => s.Distance(pos)).FirstOrDefault();
                    soldposition = sold.ServerPosition;
                }
                if (E.Cast(Azir.Position.Extend(sold, E.Range).To3D()))
                {
                    var time = ((Azir.ServerPosition.Distance(soldposition) / E.Speed) * 995) - (Game.Ping + FleeMenu.GetSliderValue("delay"));
                    Core.DelayAction(() => { Q.Cast(Azir.Position.Extend(pos, Q.Range).To3D()); }, (int)time);
                }
            }
        }
    }
}
