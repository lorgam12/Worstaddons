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
        public static void jump(Vector3 qpos)
        {
            var allready = Q.IsReady() && E.IsReady();
            if (allready && ManaCheck(Azir) < Azir.Mana)
            {
                if (Orbwalker.AzirSoldiers.Count(s => s.Distance(Azir) <= FleeMenu.GetSliderValue("range") && s != null && s.IsAlly) < 1)
                {
                    if (W.Cast(Azir.ServerPosition.Extend(Game.CursorPos, W.Range).To3D()))
                    {
                        Core.DelayAction(
                            () =>
                                {
                                    if (E.Cast(Azir.ServerPosition.Extend(Game.CursorPos, E.Range).To3D()))
                                    {
                                        Core.DelayAction(
                                            () =>
                                                { Q.Cast(Azir.ServerPosition.Extend(qpos, Q.Range).To3D()); }, FleeMenu.GetSliderValue("delay"));
                                    }
                                }, 150);
                    }
                }
                else if (Orbwalker.AzirSoldiers.Count(s => s.Distance(Azir) <= FleeMenu.GetSliderValue("range") && s != null && s.IsAlly) > 0)
                {
                    Core.DelayAction(
                        () =>
                            {
                                if (E.Cast(Azir.ServerPosition.Extend(Game.CursorPos, E.Range).To3D()))
                                {
                                    Core.DelayAction(
                                        () =>
                                            {
                                                Q.Cast(Azir.ServerPosition.Extend(qpos, Q.Range).To3D());
                                            },
                                        100);
                                }
                            },
                        150);
                }
                else if (Orbwalker.AzirSoldiers.Count(s => s.Distance(Azir) <= FleeMenu.GetSliderValue("range") && s != null && s.IsAlly) < 1)
                {
                    if (W.Cast(Azir.ServerPosition.Extend(Game.CursorPos, W.Range).To3D()))
                    {
                        Core.DelayAction(
                        () =>
                        { E.Cast(Azir.ServerPosition.Extend(Game.CursorPos, E.Range).To3D()); },
                        150);
                    }
                }else if(E.Cast(Azir.ServerPosition.Extend(Game.CursorPos, E.Range).To3D()))
                {
                    Core.DelayAction(
                        () =>
                        {
                            Q.Cast(Azir.ServerPosition.Extend(qpos, Q.Range).To3D());
                        },
                        150);
                }
                else if (Orbwalker.AzirSoldiers.Count(s => s.Distance(Azir) <= FleeMenu.GetSliderValue("range") && s != null && s.IsAlly) > 1 && !E.IsReady())
                {
                    Core.DelayAction(
                        () =>
                        {
                            Q.Cast(Azir.ServerPosition.Extend(qpos, Q.Range).To3D());
                        },
                        100);
                }
            }
        }
    }
}
