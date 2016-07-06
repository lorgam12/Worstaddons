namespace KappaUtilityReborn.Main.Common.Utility
{
    using System;

    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Menu.Values;

    internal static class Misc
    {
        public static int Health(this GameObject obj, bool percent = false)
        {
            switch (obj.Type)
            {
                case GameObjectType.obj_AI_Turret:
                    return percent ? (int)(obj as Obj_AI_Turret).HealthPercent : (int)(obj as Obj_AI_Turret).Health;
                case GameObjectType.obj_BarracksDampener:
                    return percent ? (int)(obj as Obj_BarracksDampener).HealthPercent : (int)(obj as Obj_BarracksDampener).Health;
                case GameObjectType.obj_HQ:
                    return percent ? (int)(obj as Obj_HQ).HealthPercent : (int)(obj as Obj_HQ).Health;
                default:
                    return 0;
            }
        }

        public static bool Enabled(this GameObjectType obj, Menu m)
        {
            return m[obj.ToString()].Cast<CheckBox>().CurrentValue;
        }

        public static bool Team(this GameObject obj, int value)
        {
            switch (value)
            {
                case 0:
                    return true;
                case 1:
                    return Game.MapId.Equals(GameMapId.SummonersRift) ? obj.Distance(ObjectsManager.Fturret) > 12000 : obj.Distance(ObjectsManager.Fturret) > 8000;
                case 2:
                    return Game.MapId.Equals(GameMapId.SummonersRift) ? obj.Distance(ObjectsManager.Fturret) < 12000 : obj.Distance(ObjectsManager.Fturret) < 8000;
                default:
                    return true;
            }
        }

        public static string RealName(this GameObject obj)
        {
            if (Game.MapId != GameMapId.SummonersRift)
            {
                return string.Empty;
            }

            if (obj.Name.ToLower().Contains("hq"))
            {
                return "NEXUES";
            }

            if (obj.Name.ToLower().Contains("_r1"))
            {
                return "BOT Inhibitor";
            }

            if (obj.Name.ToLower().Contains("_c1"))
            {
                return "MID Inhibitor";
            }

            if (obj.Name.ToLower().Contains("_l1"))
            {
                return "TOP Inhibitor";
            }

            if (obj.Name.ToLower().Contains("c_02_a") || obj.Name.ToLower().Contains("c_01_a"))
            {
                return "NEXUES LASER Turret";
            }

            if (obj.Name.ToLower().Contains("r_01_a") || obj.Name.ToLower().Contains("c_07_a"))
            {
                return "BOT BASE Turret";
            }

            if (obj.Name.ToLower().Contains("r_02_a"))
            {
                return "BOT T1 Outer Turret";
            }

            if (obj.Name.ToLower().Contains("r_03_a"))
            {
                return "BOT T2 Outer Turret";
            }

            if (obj.Name.ToLower().Contains("c_03_a"))
            {
                return "MID BASE Turret";
            }

            if (obj.Name.ToLower().Contains("c_04_a"))
            {
                return "MID T1 Outer Turret";
            }

            if (obj.Name.ToLower().Contains("c_05_a"))
            {
                return "MID T2 Outer Turret";
            }

            if (obj.Name.ToLower().Contains("l_01_a") || obj.Name.ToLower().Contains("c_06_a"))
            {
                return "TOP BASE Turret";
            }

            if (obj.Name.ToLower().Contains("l_02_a"))
            {
                return "TOP T1 Outer Turret";
            }

            return obj.Name.ToLower().Contains("l_03_a") ? "TOP T2 Outer Turret" : string.Empty;
        }

        public static bool DrawPercent(this Menu m, string str)
        {
            return m[str].Cast<ComboBox>().CurrentValue == 0;
        }

        public static SpellDataInst SlotToSpell(this AIHeroClient hero, SpellSlot slot)
        {
            return hero.Spellbook.GetSpell(slot);
        }

        public static string CoolDown(this SpellDataInst spell)
        {
            var t = spell.CooldownExpires - Game.Time;
            var ts = TimeSpan.FromSeconds(t);
            var s = t > 60 ? string.Format("{0}:{1:D2}", ts.Minutes, ts.Seconds) : string.Format("{0:0}", t);
            return s;
        }

        public static int Timer(this SpellDataInst spell)
        {
            var t = spell.CooldownExpires - Game.Time;
            var ts = TimeSpan.FromSeconds(t);
            var s = t > 60 ? string.Format("{0}:{1:D2}", ts.Minutes, ts.Seconds) : string.Format("{0:0}", t);
            return (int)t;
        }

        public static int DeathTimer(this AIHeroClient hero)
        {
            var spawntime = 0;

            var BRW = hero.Level * 2.5 + 7.5;
            if (Game.Time > 900 && Game.Time < 1800)
            {
                spawntime = (int)(BRW + ((BRW / 100) * ((Game.Time / 60) - 15) * 2 * 0.425));
            }

            if (Game.Time > 1800 && Game.Time < 2700)
            {
                spawntime = (int)(BRW + ((BRW / 100) * ((Game.Time / 60) - 15) * 2 * 0.425) + ((BRW / 100) * ((Game.Time / 60) - 30) * 2 * 0.30) + ((BRW / 100) * ((Game.Time / 60) - 45) * 2 * 1.45));
            }

            if (Game.Time > 3210)
            {
                spawntime =
                    (int)((BRW + ((BRW / 100) * ((Game.Time / 60) - 15) * 2 * 0.425) + ((BRW / 100) * ((Game.Time / 60) - 30) * 2 * 0.30) + ((BRW / 100) * ((Game.Time / 60) - 45) * 2 * 1.45)) * 1.5f);
            }

            if (spawntime == 0)
            {
                spawntime = (int)BRW;
            }

            return spawntime;
        }
    }
}
