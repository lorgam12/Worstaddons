namespace Khappa_Zix.Modes
{
    using System;
    using System.Linq;

    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Menu.Values;

    using Khappa_Zix.Load;

    using SharpDX;

    internal class JumpsHandler
    {
        public static bool Jumping;

        public static Obj_Shop bases;

        private static Vector3 Jumppoint1, Jumppoint2;

        public static int Edelay = menu.Jump["delay"].Cast<Slider>().CurrentValue;

        public static Vector3 GetJumpPoint(AIHeroClient Qtarget, bool firstjump = true)
        {
            bases = ObjectManager.Get<Obj_Shop>().FirstOrDefault(o => o.IsAlly);
            var target =
                EntityManager.Heroes.Enemies.OrderBy(
                    x =>
                    x.IsValidTarget(Load.E.Range) && !x.IsValidTarget(Load.Q.Range)
                    && x.Health <= Load.GetQDamage(x) + Player.Instance.GetAutoAttackDamage(x)).FirstOrDefault();
            var finalPosition = Player.Instance.Position.Extend(bases.Position, Load.E.Range);
            var collFlags = NavMesh.GetCollisionFlags(finalPosition);

            if (Player.Instance.IsUnderEnemyturret() && collFlags != CollisionFlags.Wall)
            {
                Player.Instance.ServerPosition.Extend(bases.Position, Load.E.Range).To3D();
            }

            if (firstjump && collFlags != CollisionFlags.Wall)
            {
                return Player.Instance.ServerPosition.Extend(bases.Position, Load.E.Range).To3D();
            }

            if (!firstjump && collFlags != CollisionFlags.Wall)
            {
                return Player.Instance.ServerPosition.Extend(bases.Position, Load.E.Range).To3D();
            }

            return Player.Instance.ServerPosition.Extend(bases.Position, Load.E.Range).To3D();
        }

        internal static void JumpLogic(EventArgs args)
        {
            if (!Load.E.IsReady() || !Load.EvolvedE || Player.Instance.IsDead || Player.Instance.IsRecalling())
            {
                return;
            }

            var targets = EntityManager.Heroes.Enemies.Where(x => x.IsValidTarget() && !x.IsInvulnerable && !x.IsZombie);
            var checkQKillable =
                targets.FirstOrDefault(
                    x =>
                    Vector3.Distance(Player.Instance.ServerPosition, x.ServerPosition) < Load.Q.Range - 25
                    && Load.GetQDamage(x) > x.Health);

            if (checkQKillable != null)
            {
                if (Load.Q.IsReady() && Load.E.IsReady())
                {
                    Jumping = true;
                    Jumppoint1 = GetJumpPoint(checkQKillable);
                    Load.E.Cast(Jumppoint1);
                    if (Jumping)
                    {
                        Load.Q.Cast(checkQKillable);
                        Chat.Print("first jump");
                        Core.DelayAction(
                            () =>
                                {
                                    Jumppoint2 = GetJumpPoint(checkQKillable, false);
                                    Load.E.Cast(Jumppoint2);
                                    Chat.Print("second jump");

                                    Jumping = false;
                                },
                            Edelay + Game.Ping);
                    }
                }
            }
        }

        internal static void Spellbook_OnCastSpell(Spellbook sender, SpellbookCastSpellEventArgs args)
        {
            if (!Load.EvolvedE || !Combo.doubleJump || !Load.E.IsReady())
            {
                return;
            }

            if (args.Slot.Equals(SpellSlot.Q) && args.Target is AIHeroClient)
            {
                var target = (AIHeroClient)args.Target;
                var qdmg = Load.GetQDamage(target);
                var dmg = (Player.Instance.GetAutoAttackDamage(target) * 2) + qdmg;
                if (target.Health < dmg && target.Health > qdmg)
                {
                    args.Process = false;
                }
            }
        }

        internal static void Orbwalker_OnPreAttack(AttackableUnit target, Orbwalker.PreAttackArgs args)
        {
            if (!Load.EvolvedE || !Combo.doubleJump || !Load.E.IsReady() || !(args.Target is AIHeroClient)
                || !(target is AIHeroClient))
            {
                return;
            }

            if (args.Target.Health < Load.GetQDamage((AIHeroClient)args.Target) && Player.Instance.ManaPercent > 15)
            {
                args.Process = false;
            }
        }
    }
}