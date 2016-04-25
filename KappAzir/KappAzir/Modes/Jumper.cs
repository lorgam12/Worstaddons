namespace KappAzir.Modes
{
    using System;
    using System.Linq;

    using EloBuddy;
    using EloBuddy.SDK;

    using Mario_s_Lib;

    using SharpDX;
    using
    

    static
Menus;
    using
    static
SpellsManager;

    internal class Jumper : ModeManager
    {
        public static int delay = delay = FleeMenu.GetSliderValue("delay");

        public static int range = delay = FleeMenu.GetSliderValue("range");

        public static Vector3 castpos;

        public static void jump(Vector3 qpos, Vector3 pos)
        {
            castpos = qpos;
            if (Orbwalker.AzirSoldiers.Count(s => s.Distance(Azir) < range) < 1)
            {
                if (E.IsReady() && Q2.IsReady())
                {
                    if (W.Cast(Azir.ServerPosition.Extend(pos, W.Range).To3D()))
                    {
                        Core.DelayAction(
                            () =>
                                {
                                    if (E.Cast(Azir.ServerPosition.Extend(pos, E.Range).To3D()))
                                    {
                                        Core.DelayAction(() => Q2.Cast(Azir.ServerPosition.Extend(qpos, Q2.Range).To3D()), delay);
                                    }
                                },
                            150);
                    }
                }
            }
            else
            {
                if (E.IsReady() && Q2.IsReady())
                {
                    Core.DelayAction(
                        () =>
                            {
                                if (E.Cast(Azir.ServerPosition.Extend(pos, E.Range).To3D()))
                                {
                                    Core.DelayAction(() => Q2.Cast(Azir.ServerPosition.Extend(qpos, Q2.Range).To3D()), delay);
                                }
                            },
                        150);

                    Core.DelayAction(
                        () =>
                            {
                                if (Q2.Cast(Azir.ServerPosition.Extend(qpos, Q2.Range).To3D()))
                                {
                                    Core.DelayAction(() => E.Cast(Azir.ServerPosition.Extend(pos, E.Range).To3D()), delay);
                                }
                            },
                        250);
                }
            }
        }

        internal static void OnLoad()
        {
            Obj_AI_Base.OnProcessSpellCast += Obj_AI_Base_OnProcessSpellCast1;
        }

        private static void Obj_AI_Base_OnProcessSpellCast1(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsMe && (FleeMenu.GetKeyBindValue("flee") || FleeMenu.GetKeyBindValue("insect") || FleeMenu.GetKeyBindValue("insected")))
            {
                if (args.SData.Name == "AzirE" && Q2.IsReady())
                {
                    Q2.Cast(Azir.ServerPosition.Extend(castpos, Q2.Range).To3D());
                }
                if (args.SData.Name == "AzirQ" && E.IsReady())
                {
                    E.Cast(Azir.ServerPosition.Extend(castpos, E.Range).To3D());
                }
            }
        }
    }
}