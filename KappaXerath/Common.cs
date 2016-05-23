namespace KappaXerath
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Enumerations;
    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Menu.Values;

    using SharpDX;

    internal abstract class Common
    {
        public static DangerLevel danger()
        {
            switch (Program.MiscMenu["danger"].Cast<ComboBox>().CurrentValue)
            {
                case 0:
                    {
                        return DangerLevel.High;
                    }
                case 1:
                    {
                        return DangerLevel.Medium;
                    }
                case 2:
                    {
                        return DangerLevel.Low;
                    }
            }
            return DangerLevel.Low;
        }

        public static HitChance hitchance(Spell.SpellBase spell, Menu m)
        {
            switch (m[spell.Slot + "hit"].Cast<ComboBox>().CurrentValue)
            {
                case 0:
                    {
                        return HitChance.High;
                    }
                case 1:
                    {
                        return HitChance.Medium;
                    }
                case 2:
                    {
                        return HitChance.Low;
                    }
            }
            return HitChance.Unknown;
        }

        public static void MiniMapCircle(System.Drawing.Color color, float radius, Vector3 center, int thickness = 5, int quality = 30)
        {
            var pointList = new List<Vector3>();
            for (var i = 0; i < quality; i++)
            {
                var angle = i * Math.PI * 2 / quality;
                pointList.Add(new Vector3(center.X + radius * (float)Math.Cos(angle), center.Y + radius * (float)Math.Sin(angle), center.Z));
            }

            for (var i = 0; i < pointList.Count; i++)
            {
                var a = pointList[i];
                var b = pointList[i == pointList.Count - 1 ? 0 : i + 1];

                var aonScreen = Drawing.WorldToMinimap(a);
                var bonScreen = Drawing.WorldToMinimap(b);

                Drawing.DrawLine(aonScreen.X, aonScreen.Y, bonScreen.X, bonScreen.Y, thickness, color);
            }
        }

        public static bool IsCC(Obj_AI_Base target)
        {
            return target.IsStunned || target.IsRooted || target.IsTaunted || target.IsCharmed || target.Spellbook.IsChanneling
                   || target.HasBuffOfType(BuffType.Charm) || target.HasBuffOfType(BuffType.Knockback) || target.HasBuffOfType(BuffType.Knockup)
                   || target.HasBuffOfType(BuffType.Snare) || target.HasBuffOfType(BuffType.Stun) || target.HasBuffOfType(BuffType.Suppression)
                   || target.HasBuffOfType(BuffType.Taunt);
        }

        public static bool CanHarras()
        {
            if (Orbwalker.IsAutoAttacking)
            {
                return false;
            }

            var minions = EntityManager.MinionsAndMonsters.EnemyMinions.Where(m => m.IsValidTarget(Player.Instance.AttackRange + 200));
            if (minions == null || !minions.Any())
            {
                return true;
            }

            var minion = minions.First(minion2 => minion2.IsValidTarget());

            return !(minion.Health < Player.Instance.GetAutoAttackDamage(minion) + 3 * minion.GetAutoAttackDamage(minion));
        }

        public static bool ValidUlt(AIHeroClient target)
        {
            return !target.HasBuff("kindredrnodeathbuff") && !target.HasBuff("JudicatorIntervention") && !target.HasBuff("ChronoShift")
                   && !target.HasBuff("UndyingRage") && !target.IsInvulnerable && !target.IsZombie && !target.HasBuff("bansheesveil")
                   && !target.IsDead && !target.IsPhysicalImmune && target.Health > 0 && !target.HasBuffOfType(BuffType.Invulnerability)
                   && !target.HasBuffOfType(BuffType.PhysicalImmunity) && target.IsValidTarget();
        }

        public static int CountEnemiesInRangeDeley(Vector3 position, float range, int delay)
        {
            return
                EntityManager.Heroes.Enemies.Where(t => t.IsValidTarget())
                    .Select(t => (Vector3)Prediction.Position.PredictUnitPosition(t, delay))
                    .Count(prepos => position.Distance(prepos) < range);
        }

        public static void DrawLineRectangle(Vector3 start2, Vector3 end2, int radius, float width, System.Drawing.Color color)
        {
            Vector2 start = start2.To2D();
            Vector2 end = end2.To2D();
            var dir = (end - start).Normalized();
            var pDir = dir.Perpendicular();

            var rightStartPos = start + pDir * radius;
            var leftStartPos = start - pDir * radius;
            var rightEndPos = end + pDir * radius;
            var leftEndPos = end - pDir * radius;

            var rStartPos = Drawing.WorldToScreen(new Vector3(rightStartPos.X, rightStartPos.Y, ObjectManager.Player.Position.Z));
            var lStartPos = Drawing.WorldToScreen(new Vector3(leftStartPos.X, leftStartPos.Y, ObjectManager.Player.Position.Z));
            var rEndPos = Drawing.WorldToScreen(new Vector3(rightEndPos.X, rightEndPos.Y, ObjectManager.Player.Position.Z));
            var lEndPos = Drawing.WorldToScreen(new Vector3(leftEndPos.X, leftEndPos.Y, ObjectManager.Player.Position.Z));

            Drawing.DrawLine(rStartPos, rEndPos, width, color);
            Drawing.DrawLine(lStartPos, lEndPos, width, color);
            Drawing.DrawLine(rStartPos, lStartPos, width, color);
            Drawing.DrawLine(lEndPos, rEndPos, width, color);
        }

        public static List<Vector3> CirclePoints(float CircleLineSegmentN, float radius, Vector3 position)
        {
            List<Vector3> points = new List<Vector3>();
            for (var i = 1; i <= CircleLineSegmentN; i++)
            {
                var angle = i * 2 * Math.PI / CircleLineSegmentN;
                var point = new Vector3(position.X + radius * (float)Math.Cos(angle), position.Y + radius * (float)Math.Sin(angle), position.Z);
                points.Add(point);
            }
            return points;
        }

        public static int GetBuffCount(Obj_AI_Base target, String buffName)
        {
            return (from buff in target.Buffs where buff.Name == buffName select buff.Count).FirstOrDefault();
        }

        public static float GetPassiveTime(Obj_AI_Base target, String buffName)
        {
            return
                target.Buffs.OrderByDescending(buff => buff.EndTime - Game.Time)
                    .Where(buff => buff.Name == buffName)
                    .Select(buff => buff.EndTime)
                    .FirstOrDefault() - Game.Time;
        }

        public static int WayPointAnalysis(Obj_AI_Base unit, Spell.SpellBase QWER)
        {
            int HC = 0;

            HC = QWER.CastDelay < 0.25f ? 2 : 1;

            if (unit.Path.Count() == 1)
            {
                HC = 2;
            }

            return HC;
        }
    }
}