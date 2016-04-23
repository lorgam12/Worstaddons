namespace KappAzir.Modes
{
    using System.Linq;

    using EloBuddy;
    using EloBuddy.SDK;

    using Mario_s_Lib;

    using SharpDX;
    using static Menus;
    using static SpellsManager;

    class Jumper : ModeManager
    {
        public static void jump(Vector3 qpos, Vector3 pos)
        {
            if(W.IsReady() && E.IsReady() && Q.IsReady() && Orbwalker.AzirSoldiers.Count(s => s.Distance(Azir) <= FleeMenu.GetSliderValue("range")) < 1 && Azir.Mana > ManaCheck(Azir))
            {
                if (W.Cast(Azir.ServerPosition.Extend(pos, W.Range).To3D()))
                {
                    Core.DelayAction(
                        () =>
                            {
                                if (E.Cast())
                                {
                                    Core.DelayAction(
                                        () =>
                                            { Q.Cast(Azir.ServerPosition.Extend(qpos, Q.Range).To3D()); }, FleeMenu.GetSliderValue("delay"));
                                }
                            }, 150);
                }
            }else if (Orbwalker.AzirSoldiers.Count(s => s.Distance(Azir) <= FleeMenu.GetSliderValue("range")) > 0 && Q.IsReady())
            {
                Core.DelayAction(
                    () =>
                    {
                        if (E.Cast())
                        {
                            Core.DelayAction(
                                    () =>
                                    { Q.Cast(Azir.ServerPosition.Extend(qpos, Q.Range).To3D()); }, FleeMenu.GetSliderValue("delay"));
                        }
                    }, 150);
            }else if (Orbwalker.AzirSoldiers.Count(s => s.Distance(Azir) <= FleeMenu.GetSliderValue("range")) > 0 && E.IsReady())
            {
                Core.DelayAction(
                    () =>
                        {
                            if (Q.Cast(Azir.ServerPosition.Extend(qpos, Q.Range).To3D()))
                            {
                                Core.DelayAction(
                                    () =>
                                        {
                                            E.Cast();
                                        },
                                    FleeMenu.GetSliderValue("delay"));
                            }
                        },
                    150);
            }else if (E.Cast() && Q.IsReady())
            {
                Core.DelayAction(
                        () =>
                        { Q.Cast(Azir.ServerPosition.Extend(qpos, Q.Range).To3D()); }, FleeMenu.GetSliderValue("delay"));

            }else if (Q.Cast(Azir.ServerPosition.Extend(qpos, Q.Range).To3D()) && E.IsReady())
            {
                Core.DelayAction(
                        () =>
                        { E.Cast(); }, FleeMenu.GetSliderValue("delay"));
            }
            else if (Orbwalker.AzirSoldiers.Any(s => s != null))
            {
                if (E.Cast() && Q.IsReady())
                {
                    Core.DelayAction(
                        () =>
                            {
                                Q.Cast(Azir.ServerPosition.Extend(qpos, Q.Range).To3D());
                            },
                        FleeMenu.GetSliderValue("delay"));
                }
            }
        }
    }
}
