namespace KappAzir.Modes
{
    using System;
    using System.Linq;

    using EloBuddy;
    using EloBuddy.SDK;

    using Mario_s_Lib;

    using SharpDX;
    using static Menus;
    using static SpellsManager;

    internal class Jumper : ModeManager
    {
        public static void jump(Vector3 qpos, Vector3 pos)
        {
            var range = FleeMenu.GetSliderValue("range");
            var delay = FleeMenu.GetSliderValue("delay");
            var allready = Q.IsReady() && E.IsReady() && ManaCheck(Azir) < Azir.Mana;
            if (Orbwalker.AzirSoldiers.Count(s => s.Distance(Azir) < range) < 1 && allready)
            {
                if (W.Cast(Azir.ServerPosition.Extend(pos, W.Range).To3D()))
                {
                    Console.WriteLine("W1 Casted");
                                if (E.IsReady())
                                {
                                    if (E.Cast(Azir.ServerPosition.Extend(pos, W.Range).To3D()))
                                    {
                                        Console.WriteLine("E1 Casted");
                                        if (Q.IsReady())
                                        {
                                            Core.DelayAction(
                                                () =>
                                                {
                                                    Q.Cast(Azir.ServerPosition.Extend(qpos, Q.Range).To3D());
                                                    Console.WriteLine("Q1 Casted");
                                                }, delay);
                                        }
                                    }
                                }
                }
                return;
            }
            if (Orbwalker.AzirSoldiers.Count(s => s.Distance(Azir) < range) > 0 && allready)
            {
                if (E.IsReady())
                {
                                if (E.Cast(Azir.ServerPosition.Extend(pos, W.Range).To3D()))
                                {
                                    Console.WriteLine("E2 Casted");
                                    if (Q.IsReady())
                                    {
                                        Core.DelayAction(
                                            () =>
                                                {
                                                    Q.Cast(Azir.ServerPosition.Extend(qpos, Q.Range).To3D());
                                                    Console.WriteLine("Q2 Casted");
                                                },
                                            delay);
                                    }
                                }
                }

                return;
            }
        }
    }
}