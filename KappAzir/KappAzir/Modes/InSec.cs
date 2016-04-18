namespace KappAzir.Modes
{
    using System.Linq;

    using EloBuddy;
    using EloBuddy.SDK;

    using Mario_s_Lib;

    using SharpDX;
    using static Menus;
    using static SpellsManager;

    internal class InSec : ModeManager
    {
        internal static Vector2 insecLoc;

        internal static Vector3 soldposition;

        public static void QCast(Vector3 pos)
        {
            Core.DelayAction(
                () =>
                    {
                        if (Q.Cast(Azir.Position.Extend(pos, Q.Range).To3D()))
                        {
                            if (tower != null)
                            {
                                R.Cast(tower.ServerPosition);
                            }
                            else if (ally != null)
                            {
                                R.Cast(ally.ServerPosition);
                            }
                            else
                            {
                                RCast(insecLoc.To3D());
                            }
                        }
                        ;
                    },
                150);
        }

        public static void RCast(Vector3 pos)
        {
            R.Cast(pos);
        }

        public static void New()
        {
            var Target = TargetSelector.SelectedTarget;
            if (Target != null)
            {
                if (Target.IsValidTarget(825))
                {
                    if (Q.IsReady())
                    {
                        if (R.IsReady())
                        {
                            if (Orbwalker.AzirSoldiers.Count > 0)
                            {
                                insecLoc = Vector2.Zero;
                            }
                            var s =
                                Orbwalker.AzirSoldiers.Where(it => it.Distance(Target) <= (R.Width / 2) - 20)
                                    .OrderByDescending(it => it.Distance(Target))
                                    .First();
                            insecLoc = (Vector2)Azir.ServerPosition;
                            if (Azir.Distance(Target) > 200 && s != null)
                            {
                                if (E.Cast(s.Position))
                                {
                                    if (tower != null && InsecMenu.GetCheckBoxValue("Tower"))
                                    {
                                        QCast(tower.ServerPosition);
                                    }
                                    else if (ally != null && InsecMenu.GetCheckBoxValue("Ally"))
                                    {
                                        QCast(ally.ServerPosition);
                                    }
                                    else
                                    {
                                        QCast(insecLoc.To3D());
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos);
                    }
                }
                else
                {
                    Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos);
                }
            }
            else
            {
                Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos);
            }
        }

        public static void Normal()
        {
            var Target = TargetSelector.SelectedTarget;
            if (Target != null)
            {
                if (Target.IsValidTarget(R.Width) && R.IsReady())
                {
                    if (tower != null && InsecMenu.GetCheckBoxValue("Tower"))
                    {
                        R.Cast(Azir.Position.Extend(tower.ServerPosition, R.Range).To3D());
                    }
                    else if (ally != null && InsecMenu.GetCheckBoxValue("Ally"))
                    {
                        R.Cast(Azir.Position.Extend(ally.ServerPosition, R.Range).To3D());
                    }
                    else
                    {
                        R.Cast(Azir.Position.Extend(insecLoc, R.Range).To3D());
                    }
                }
                else
                {
                    Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos);
                }
                if (Target.IsValidTarget(825))
                {
                    if (Q.IsReady())
                    {
                        if (R.IsReady())
                        {
                            insecLoc = Vector2.Zero;
                            var direction = (TargetSelector.SelectedTarget.ServerPosition - ObjectManager.Player.ServerPosition).To2D().Normalized();
                            var insecPos = TargetSelector.SelectedTarget.ServerPosition.To2D() + (direction * 200f);
                            if (Orbwalker.AzirSoldiers.OrderBy(s => s.Distance(insecPos)).FirstOrDefault() != null)
                            {
                                soldposition = Orbwalker.AzirSoldiers.OrderBy(s => s.Distance(insecPos)).FirstOrDefault().ServerPosition;
                            }
                            insecLoc = (Vector2)Azir.ServerPosition;
                            var time = ((Azir.ServerPosition.Distance(soldposition) / E.Speed) * 1000) - ((Game.Ping + FleeMenu.GetSliderValue("delay")));
                            var allready = Q.IsReady() && E.IsReady() && W.IsReady();
                            if (Orbwalker.AzirSoldiers.Count(s => s.IsAlly) < 1 && allready && ManaCheck(Azir) < Azir.Mana)
                            {
                                W.Cast(Azir.Position.Extend(insecPos, W.Range).To3D());
                            }

                            if (Orbwalker.AzirSoldiers.Count(s => s.IsAlly) > 0)
                            {
                                if (E.Cast(Azir.Position.Extend(insecPos, E.Range).To3D()))
                                {
                                    Core.DelayAction(
                                        () => { Q.Cast(Azir.Position.Extend(insecPos, Q.Range).To3D()); },
                                        (int)time);
                                }
                            }
                        }
                        else
                        {
                            Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos);
                        }
                    }
                    else
                    {
                        Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos);
                    }
                }
                else
                {
                    Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos);
                }
            }
            else
            {
                Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos);
            }
        }
    }
}