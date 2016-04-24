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
        public static int delay = delay = FleeMenu.GetSliderValue("delay");
        public static int range = delay = FleeMenu.GetSliderValue("range");

        public static Vector3 castpos;

        public static void jump(Vector3 qpos, Vector3 pos)
        {
            castpos = qpos;
            var allready = Q.IsReady() && E.IsReady() && ManaCheck(Azir) < Azir.Mana;
            if (Orbwalker.AzirSoldiers.Count(s => s.Distance(Azir) < range) < 1 && allready)
            {
                if (W.Cast(Azir.ServerPosition.Extend(pos, W.Range).To3D()))
                {
                    if (E.IsReady())
                    {
                        if (E.Cast(Azir.ServerPosition.Extend(pos, W.Range).To3D()))
                        {
                            if (Q.IsReady())
                            {
                                Core.DelayAction(() => { Q.Cast(Azir.ServerPosition.Extend(qpos, Q.Range).To3D()); }, delay);
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
                        if (Q.IsReady())
                        {
                            Core.DelayAction(() => { Q.Cast(Azir.ServerPosition.Extend(qpos, Q.Range).To3D()); }, delay);
                        }
                    }
                }

                return;
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
                if (args.Slot == SpellSlot.E)
                {
                    if (Q.IsReady())
                    {
                        Core.DelayAction(() => { Q.Cast(Azir.ServerPosition.Extend(castpos, Q.Range).To3D()); }, delay);
                    }
                }
                if (args.Slot == SpellSlot.Q)
                {
                    if (E.IsReady())
                    {
                        Core.DelayAction(() => { E.Cast(Azir.ServerPosition.Extend(castpos, W.Range).To3D()); }, delay);
                    }
                }
            }
        }
    }
}