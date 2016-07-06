namespace KappaUtilityReborn.Main.Common.Utility
{
    using System.Collections.Generic;
    using System.Linq;

    using EloBuddy;
    using EloBuddy.SDK;

    internal class ObjectsManager
    {
        public static List<GameObject> AllObjects = new List<GameObject>();

        public static IEnumerable<Obj_AI_Turret> AllTurrets
        {
            get
            {
                return EntityManager.Turrets.AllTurrets.Where(t => !t.IsDead && t.Health > 0 && t.Health < 9999);
            }
        }

        public static IEnumerable<Obj_BarracksDampener> AllInhb
        {
            get
            {
                return ObjectManager.Get<Obj_BarracksDampener>().Where(i => !i.IsDead && i.Health > 0);
            }
        }

        public static IEnumerable<Obj_HQ> AllNexues
        {
            get
            {
                return ObjectManager.Get<Obj_HQ>().Where(i => !i.IsDead && i.Health > 0);
            }
        }

        public static Obj_AI_Turret Fturret
        {
            get
            {
                return EntityManager.Turrets.Allies.FirstOrDefault(t => !t.IsDead && t.Health > 9000);
            }
        }
    }
}
