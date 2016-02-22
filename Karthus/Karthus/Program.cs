using System;
using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using SharpDX;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Rendering;

namespace Karthus
{

    class Program
    {
        public static Check Check;

        public static Vector2[] g_MinMaxCorners;
        public static RectangleF g_MinMaxBox;
        public static Vector2[] g_NonCulledPoints;

        private static bool cz = false;
        private static float czx = 0, czy = 0, czx2 = 0, czy2 = 0;

        private static AIHeroClient player = ObjectManager.Player;
        public static Spell.Skillshot Q { get; private set; }
        public static Spell.Skillshot W { get; private set; }
        public static Spell.Active E { get; private set; }
        public static Spell.Skillshot R { get; private set; }


        public static Menu ComboMenu { get; private set; }
        public static Menu HarassMenu { get; private set; }
        public static Menu LaneMenu { get; private set; }
        public static Menu LHMenu { get; private set; }
        public static Menu MiscMenu { get; private set; }
        public static Menu DrawMenu { get; private set; }
        private static Menu MenuIni;
        private static AIHeroClient QTarget;
        private static AIHeroClient WTarget;
        private static AIHeroClient ETarget;
        private static bool NowE = false;

        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Game_OnGameLoad;
        }

        private static void Game_OnGameLoad(EventArgs args)
        {
            if (player.ChampionName != "Karthus") return;

            Check = new Check();

            Q = new Spell.Skillshot(SpellSlot.Q, 875, SkillShotType.Circular, 1000, int.MaxValue, 160);
            W = new Spell.Skillshot(SpellSlot.W, 1000, SkillShotType.Circular, 500, int.MaxValue, 70);
            E = new Spell.Active(SpellSlot.E, 505);
            R = new Spell.Skillshot(SpellSlot.R, 25000, SkillShotType.Circular, 3000, int.MaxValue, int.MaxValue);

            MenuIni = MainMenu.AddMenu("Karthus", "Karthus");
            MenuIni.AddGroupLabel("Welcome to the worst karthus addon!");

            ComboMenu = MenuIni.AddSubMenu("Combo");
            ComboMenu.Add("CUse_Q", new CheckBox("CUse_Q"));
            ComboMenu.Add("CUse_W", new CheckBox("CUse_W"));
            ComboMenu.Add("CUse_E", new CheckBox("CUse_E"));
            ComboMenu.Add("CUse_AA", new CheckBox("CUse_AA"));
            ComboMenu.Add("CEPercent", new Slider("Use E Mana %", 100, 30, 0));
            ComboMenu.AddSeparator();
            ComboMenu.Add("CE_Auto_False", new CheckBox("CE_Auto_False"));
            ComboMenu.AddLabel("E auto false when target isn't valid");

            HarassMenu = MenuIni.AddSubMenu("Harass");
            HarassMenu.Add("HUse_Q", new CheckBox("HUse_Q"));
            HarassMenu.Add("HUse_E", new CheckBox("HUse_E"));
            HarassMenu.Add("HEPercent", new Slider("Use E Mana %", 100, 30, 0));
            HarassMenu.Add("HUse_AA", new CheckBox("HUse_AA"));
            HarassMenu.Add("HUse_AA_to_minion", new CheckBox("HUse_AA_to_minion"));
            HarassMenu.Add("E_LastHit", new CheckBox("E_LastHit"));
            HarassMenu.AddSeparator();
            HarassMenu.Add("HE_Auto_False", new CheckBox("HE_Auto_False"));
            HarassMenu.AddLabel("E auto false when target isn't valid");

            LaneMenu = MenuIni.AddSubMenu("Farm");
            LaneMenu.Add("FUse_Q", new CheckBox("FUse_Q"));
            LaneMenu.Add("FQPercent", new Slider("Use Q Mana %", 100, 30, 0));
            LaneMenu.Add("FUse_E", new CheckBox("FUse_E"));
            LaneMenu.Add("FEPercent", new Slider("Use E Mana %", 100, 30, 0));

            LHMenu = MenuIni.AddSubMenu("LastHit");
            LHMenu.Add("LUse_Q", new CheckBox("LUse_Q"));

            MiscMenu = MenuIni.AddSubMenu("Misc");
            MiscMenu.Add("NotifyUlt", new CheckBox("Ult_notify_text"));
            MiscMenu.Add("DeadCast", new CheckBox("DeadCast"));

            DrawMenu = MenuIni.AddSubMenu("Draw");
            DrawMenu.Add("Enabled", new CheckBox("Enabled"));
            DrawMenu.Add("Draw_Q", new CheckBox("Draw_Q"));

            Game.OnUpdate += zigzag;
            Game.OnUpdate += OnUpdate;
            Drawing.OnDraw += OnDraw;
        }

        private static void zigzag(EventArgs args)
        {
            if (QTarget == null)
            {
                czx = 0;
                czx2 = 0;
                czy = 0;
                czy2 = 0;
                return;
            }

            if (czx < czx2)
            {
                if (czx2 >= QTarget.ServerPosition.X)
                    cz = true;
                else
                    cz = false;
            }
            else if (czx == czx2)
            {
                cz = false;
                czx = czx2;
                czx2 = QTarget.ServerPosition.X;
                return;
            }
            else
            {
                if (czx2 <= QTarget.ServerPosition.X)
                    cz = true;
                else
                    cz = false;
            }
            czx = czx2;
            czx2 = QTarget.ServerPosition.X;

            if (czy < czy2)
            {
                if (czy2 >= QTarget.ServerPosition.Y)
                    cz = true;
                else
                    cz = false;
            }
            else if (czy == czy2)
                cz = false;
            else
            {
                if (czy2 <= QTarget.ServerPosition.Y)
                    cz = true;
                else
                    cz = false;
            }
            czy = czy2;
            czy2 = QTarget.ServerPosition.Y;
        }

        private static void OnUpdate(EventArgs args)
        {
            if (player.IsDead) return;

            QTarget = TargetSelector.GetTarget(Q.Range, DamageType.Magical);
            WTarget = TargetSelector.GetTarget(W.Range, DamageType.Magical);
            ETarget = TargetSelector.GetTarget(E.Range, DamageType.Magical);
            
            if (Orbwalker.ActiveModesFlags == Orbwalker.ActiveModes.Combo)
            {
                Combo();
                if (!ComboMenu.Get<CheckBox>("CUse_AA").CurrentValue || player.Mana < Q.Handle.SData.Mana * 3)
                {
                    Orbwalker.DisableAttacking = true;
                }
                else
                {
                    Orbwalker.DisableAttacking = false;
                }
            }
            if (Orbwalker.ActiveModesFlags == Orbwalker.ActiveModes.LaneClear)
            {
                Farm();
            }
            if (Orbwalker.ActiveModesFlags == Orbwalker.ActiveModes.Harass)
            {
                Harass();
                if (!HarassMenu.Get<CheckBox>("HUse_AA").CurrentValue || player.Mana < Q.Handle.SData.Mana * 3)
                {
                    Orbwalker.DisableAttacking = true;
                }
                else
                {
                    Orbwalker.DisableAttacking = false;
                }
            }
            if (Orbwalker.ActiveModesFlags == Orbwalker.ActiveModes.LastHit)
            {
                LastHit();
            }
            if (Orbwalker.ActiveModesFlags == Orbwalker.ActiveModes.LastHit)
            {
                LastHit();
            }
            if (MiscMenu.Get<CheckBox>("DeadCast").CurrentValue)
                if (player.IsZombie)
                    if (!Combo())
                        Farm(true);
        }

        private static void OnDraw(EventArgs args)
        {
            if (!player.IsDead)
            {
                if (DrawMenu.Get<CheckBox>("Draw_Q").CurrentValue)
                {
                    Circle.Draw(Color.DarkRed, Q.Range, Player.Instance.Position);
                }
            }

            if (player.Spellbook.GetSpell(SpellSlot.R).Level > 0)
            {
                var killable = "";

                var time = Game.Time;

                foreach (TI target in Check.TI.Where(x => x.Player.IsValid && !x.Player.IsDead && x.Player.IsEnemy && (Check.recalltc(x) /*|| (x.Player.IsVisible && Utility.IsValidTarget(x.Player))*/) && player.GetSpellDamage(x.Player, SpellSlot.R) >= Program.Check.GetTargetHealth(x, (int)(R.CastDelay * 1000f))))
                {
                    killable += target.Player.ChampionName + " ";
                }

                if (killable != "" && MiscMenu.Get<CheckBox>("NotifyUlt").CurrentValue)
                {
                    Drawing.DrawText(Drawing.Width * 0.44f, Drawing.Height * 0.7f, System.Drawing.Color.Red, "Killable by ult: " + killable);
                }
            }
        }


        private static void BeforeAttack(AttackableUnit target, Orbwalker.PreAttackArgs args)
        {
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo) && ComboMenu.Get<CheckBox>("CUse_AA").CurrentValue)
            {
                if (Player.Instance.Mana > 44)
                {
                    if (target.Type == GameObjectType.AIHeroClient)
                    {
                        args.Process = false;
                    }
                }
            }
        }
        


        private static void calcE(bool TC = false)
        {
            if (!E.IsReady() || player.IsZombie || player.Spellbook.GetSpell(SpellSlot.E).ToggleState != 2)
            {
                return;
            }

            var minions = EntityManager.MinionsAndMonsters.GetLaneMinions(
                EntityManager.UnitTeam.Enemy, Player.Instance.Position, E.Range).ToArray();

            if (!TC && (ETarget != null || (!NowE && minions.Count() != 0)))
            {
                return;
            }

            E.Cast();
            NowE = false;
        }
        
        private static void Harass()
        {
            if (QTarget != null)
            {
                var predQ = Q.GetPrediction(QTarget);
                if (HarassMenu.Get<CheckBox>("HUse_Q").CurrentValue && (Q.IsReady() && QTarget.IsValidTarget(Q.Range)))
                {
                    if (!cz)
                    {
                        Q.Cast(predQ.CastPosition);
                    }
                    else
                    {
                        Q.Cast(QTarget.ServerPosition);
                    }
                }
            }

            if (HarassMenu.Get<CheckBox>("HUse_E").CurrentValue && HarassMenu.Get<CheckBox>("E_LastHit").CurrentValue && E.IsReady() && !player.IsZombie)
            {
                if (!E.IsReady() || player.IsZombie)
                {
                    return;
                }

                NowE = false;
                List<Obj_AI_Base> minions;
                minions = new List<Obj_AI_Base>(EntityManager.MinionsAndMonsters.GetLaneMinions(
                    EntityManager.UnitTeam.Enemy, Player.Instance.Position, E.Range).ToArray());
                minions.RemoveAll(x => x.Health <= 5);
                minions.RemoveAll(x => player.Distance(x.ServerPosition) > E.Range || x.Health > QTarget.GetSpellDamage(player,  SpellSlot.E));
                var jgm = minions.Any(x => x.Team == GameObjectTeam.Neutral);

                if ((player.Spellbook.GetSpell(SpellSlot.E).ToggleState == 1 && (minions.Count >= 1 || jgm))
                    && (((player.Mana / player.MaxMana) * 100f)
                        >= HarassMenu.Get<Slider>("HEPercent").CurrentValue)) E.Cast();
                else if ((player.Spellbook.GetSpell(SpellSlot.E).ToggleState == 2 && (minions.Count == 0 && !jgm))
                         || !(((player.Mana / player.MaxMana) * 100f)
                              >= HarassMenu.Get<Slider>("HEPercent").CurrentValue)) calcE(true);
            }

            if (HarassMenu.Get<CheckBox>("HUse_E").CurrentValue && E.IsReady() && !player.IsZombie)
            {
                if (HarassMenu.Get<CheckBox>("HE_Auto_False").CurrentValue)
                {
                    if (ETarget != null)
                    {
                        if (player.Spellbook.GetSpell(SpellSlot.E).ToggleState == 1)
                        {
                            if (player.Distance(ETarget.ServerPosition) <= E.Range && (((player.Mana / player.MaxMana) * 100f) >= HarassMenu.Get<Slider>("HEPercent").CurrentValue))
                            {
                                NowE = true;
                                E.Cast();
                            }
                        }
                        else if (player.Distance(ETarget.ServerPosition) >= E.Range || (((player.Mana / player.MaxMana) * 100f) <= HarassMenu.Get<Slider>("HEPercent").CurrentValue))
                        {
                            calcE(true);
                        }
                    }
                    else calcE();
                }
                else
                {
                    if (ETarget != null)
                    {
                        if (player.Spellbook.GetSpell(SpellSlot.E).ToggleState == 1)
                        {
                            if (player.Distance(ETarget.ServerPosition) <= E.Range && (((player.Mana / player.MaxMana) * 100f) >= HarassMenu.Get<Slider>("HEPercent").CurrentValue))
                            {
                                NowE = true;
                                E.Cast();
                            }
                        }
                        else if (((player.Mana / player.MaxMana) * 100f) <= HarassMenu.Get<Slider>("HEPercent").CurrentValue)
                        {
                            calcE(true);
                        }
                    }
                }

            }
        }

        private static bool Combo()
        {
            bool Qtarget = false;

            var Qm = ComboMenu.Get<CheckBox>("CUse_Q").CurrentValue;
            var Wm = ComboMenu.Get<CheckBox>("CUse_W").CurrentValue;
            var Em = ComboMenu.Get<CheckBox>("CUse_E").CurrentValue;
            var EFm = ComboMenu.Get<CheckBox>("CE_Auto_False").CurrentValue;

            if (WTarget == null)
            {
                return false;
            }

            if (Wm && W.IsReady() && WTarget.IsValid)
            {
                double DS = 0;
                double countmana = W.Handle.SData.Mana;

                if (R.IsReady())
                {
                    DS += QTarget.GetSpellDamage(player, SpellSlot.R);
                    countmana += R.Handle.SData.Mana;
                }

                while (DS < QTarget.MaxHealth)
                {
                    var qd = QTarget.GetSpellDamage(player, SpellSlot.Q);

                    DS += qd;
                    countmana += Q.Handle.SData.Mana;
                }

                var predW = W.GetPrediction(WTarget);
                if (player.MaxMana > countmana || QTarget.CountAlliesInRange(W.Range) > 1 || player.IsZombie)
                {
                    W.Cast(predW.CastPosition);
                }
            }

            if (ETarget != null)
            {
                if (Em && E.IsReady() && !player.IsZombie)
                {
                    if (EFm)
                    {
                        if (ETarget != null)
                        {
                            if (player.Spellbook.GetSpell(SpellSlot.E).ToggleState == 1)
                            {
                                if (player.Distance(ETarget.ServerPosition) <= E.Range && (((player.Mana / player.MaxMana) * 100f) >= ComboMenu.Get<Slider>("CEPercent").CurrentValue))
                                {
                                    NowE = true;
                                    E.Cast();
                                }
                            }
                            else if (player.Distance(ETarget.ServerPosition) >= E.Range || (((player.Mana / player.MaxMana) * 100f) <= ComboMenu.Get<Slider>("CEPercent").CurrentValue))
                            {
                                calcE(true);
                            }
                        }
                        else calcE();
                    }
                    else
                    {
                        if (player.Spellbook.GetSpell(SpellSlot.E).ToggleState == 1)
                        {
                            if (player.Distance(ETarget.ServerPosition) <= E.Range && (((player.Mana / player.MaxMana) * 100f) >= ComboMenu.Get<Slider>("CEPercent").CurrentValue))
                            {
                                NowE = true;
                                E.Cast();
                            }
                        }
                        else if (((player.Mana / player.MaxMana) * 100f) <= ComboMenu.Get<Slider>("CEPercent").CurrentValue)
                        {
                            calcE(true);
                        }
                    }
                }
            }

            if (QTarget != null && (Qm && Q.IsReady() && QTarget.IsValid))
            {
                var predQ = Q.GetPrediction(QTarget);
                Qtarget = true;
                if (!cz)
                {
                    Q.Cast(predQ.CastPosition);
                }
                else
                {
                    Q.Cast(QTarget);
                }
            }

            return Qtarget;
        }

        private static void Farm(bool Can = false)
        {
            var canQ = Can || LaneMenu.Get<CheckBox>("FUse_Q").CurrentValue;
            var canE = Can || LaneMenu.Get<CheckBox>("FUse_E").CurrentValue;
            //bool QtoOne = MenuIni.SubMenu("Farm").Item("Q_to_One").GetValue<bool>();
            List<Obj_AI_Base> minions;

            if (canQ && Q.IsReady() && (((player.Mana / player.MaxMana) * 100f) >= LaneMenu.Get<Slider>("FQPercent").CurrentValue))
            {
               minions = new List<Obj_AI_Base>(EntityManager.MinionsAndMonsters.GetLaneMinions(
                   EntityManager.UnitTeam.Enemy, Player.Instance.Position, E.Range).ToArray());
                minions.RemoveAll(x => x.MaxHealth <= 5);
                var location =
                GetBestCircularFarmLocation(
                    EntityManager.MinionsAndMonsters.EnemyMinions.Where(x => x.Distance(Player.Instance) <= Q.Range)
                        .Select(xm => xm.ServerPosition.To2D())
                        .ToList(),
                    Q.Width,
                    Q.Range);
                if (location.MinionsHit >= 1)
                {
                    Q.Cast(location.Position.To3D());
                }
            }

            if (!canE || !E.IsReady() || player.IsZombie)
                return;
            NowE = false;

            minions = new List<Obj_AI_Base>(EntityManager.MinionsAndMonsters.GetLaneMinions(
                EntityManager.UnitTeam.Enemy, Player.Instance.Position, E.Range).ToArray());
            minions.RemoveAll(x => x.MaxHealth <= 5);
            var jgm = minions.Any(x => x.Team == GameObjectTeam.Neutral);

            if (player.Spellbook != null
                && ((player.Spellbook.GetSpell(SpellSlot.E).ToggleState == 1 && (minions.Count >= 3 || jgm))
                    && (((player.Mana / player.MaxMana) * 100f) >= LaneMenu.Get<Slider>("FEPercent").CurrentValue))) E.Cast();
            else if (player.Spellbook != null
                     && ((player.Spellbook.GetSpell(SpellSlot.E).ToggleState == 2 && (minions.Count <= 2 && !jgm))
                         || !(((player.Mana / player.MaxMana) * 100f) >= LaneMenu.Get<Slider>("FEPercent").CurrentValue)))
            {
                calcE();
            }
        }

        private static void LastHit()
        {
            if (LHMenu.Get<CheckBox>("LUse_Q").CurrentValue)
            {
                /*
                List<Obj_AI_Base> minions, minions2;

                minions = new List<Obj_AI_Base>(EntityManager.MinionsAndMonsters.GetLaneMinions(
                    EntityManager.UnitTeam.Enemy, Player.Instance.Position, E.Range).ToArray());

                minions2 = new List<Obj_AI_Base>(EntityManager.MinionsAndMonsters.GetLaneMinions(
                    EntityManager.UnitTeam.Enemy, Player.Instance.Position, E.Range).ToArray());
                minions.RemoveAll(x => x.MaxHealth <= 5);
                minions.RemoveAll(x => x.Health > QTarget.GetSpellDamage(player, SpellSlot.Q));
                var i = new List<int>() { -100, -70, 0, 70, 100 };
                var j = new List<int>() { -100, -70, 0, 70, 100 };

                foreach (var minion in minions)
                {
                    foreach (int xi in i)
                    {
                        foreach (int yj in j)
                        {
                            int cnt = 0;
                            Vector3 temp = new Vector3(Prediction.GetPrediction(minion, 250f).UnitPosition.X + xi, y: Prediction.GetPrediction(minion, 250f).UnitPosition.Y + yj, z: Prediction.GetPrediction(minion, 250f).UnitPosition.Z);
                            foreach (var minion2 in minions2.Where(x => Vector3.Distance(temp, Prediction.GetPrediction(x, 250f).UnitPosition) < 200))
                            {
                                cnt++;
                            }

                            if (cnt == 1 && minion.Health < QTarget.GetSpellDamage(player, SpellSlot.Q))
                            {
                                Q.Cast(temp);
                                break;
                            }
                        }
                    }
                }
                */
            }
        }


        // For debugging.

        // Find the points nearest the upper left, upper right,
        // lower left, and lower right corners.
        private static void GetMinMaxCorners(List<Vector2> points,
            ref Vector2 ul,
            ref Vector2 ur,
            ref Vector2 ll,
            ref Vector2 lr)
        {
            // Start with the first point as the solution.
            ul = points[0];
            ur = ul;
            ll = ul;
            lr = ul;

            // Search the other points.
            foreach (var pt in points)
            {
                if (-pt.X - pt.Y > -ul.X - ul.Y)
                {
                    ul = pt;
                }
                if (pt.X - pt.Y > ur.X - ur.Y)
                {
                    ur = pt;
                }
                if (-pt.X + pt.Y > -ll.X + ll.Y)
                {
                    ll = pt;
                }
                if (pt.X + pt.Y > lr.X + lr.Y)
                {
                    lr = pt;
                }
            }

            g_MinMaxCorners = new[] { ul, ur, lr, ll }; // For debugging.
        }

        // Find a box that fits inside the MinMax quadrilateral.
        private static RectangleF GetMinMaxBox(List<Vector2> points)
        {
            // Find the MinMax quadrilateral.
            Vector2 ul = new Vector2(0, 0), ur = ul, ll = ul, lr = ul;
            GetMinMaxCorners(points, ref ul, ref ur, ref ll, ref lr);

            // Get the coordinates of a box that lies inside this quadrilateral.
            var xmin = ul.X;
            var ymin = ul.Y;

            var xmax = ur.X;
            if (ymin < ur.Y)
            {
                ymin = ur.Y;
            }

            if (xmax > lr.X)
            {
                xmax = lr.X;
            }
            var ymax = lr.Y;

            if (xmin < ll.X)
            {
                xmin = ll.X;
            }
            if (ymax > ll.Y)
            {
                ymax = ll.Y;
            }

            var result = new RectangleF(xmin, ymin, xmax - xmin, ymax - ymin);
            g_MinMaxBox = result; // For debugging.
            return result;
        }

        // Cull points out of the convex hull that lie inside the
        // trapezoid defined by the vertices with smallest and
        // largest X and Y coordinates.
        // Return the points that are not culled.
        private static List<Vector2> HullCull(List<Vector2> points)
        {
            // Find a culling box.
            var culling_box = GetMinMaxBox(points);

            // Cull the points.
            var results =
                points.Where(
                    pt =>
                        pt.X <= culling_box.Left || pt.X >= culling_box.Right || pt.Y <= culling_box.Top ||
                        pt.Y >= culling_box.Bottom).ToList();

            g_NonCulledPoints = new Vector2[results.Count]; // For debugging.
            results.CopyTo(g_NonCulledPoints); // For debugging.
            return results;
        }

        // Return the points that make up a polygon's convex hull.
        // This method leaves the points list unchanged.
        public static List<Vector2> MakeConvexHull(List<Vector2> points)
        {
            // Cull.
            points = HullCull(points);

            // Find the remaining point with the smallest Y value.
            // if (there's a tie, take the one with the smaller X value.
            Vector2[] best_pt = { points[0] };
            foreach (
                var pt in points.Where(pt => (pt.Y < best_pt[0].Y) || ((pt.Y == best_pt[0].Y) && (pt.X < best_pt[0].X)))
                )
            {
                best_pt[0] = pt;
            }

            // Move this point to the convex hull.
            var hull = new List<Vector2> { best_pt[0] };
            points.Remove(best_pt[0]);

            // Start wrapping up the other points.
            float sweep_angle = 0;
            for (;;)
            {
                // If all of the points are on the hull, we're done.
                if (points.Count == 0)
                {
                    break;
                }

                // Find the point with smallest AngleValue
                // from the last point.
                var X = hull[hull.Count - 1].X;
                var Y = hull[hull.Count - 1].Y;
                best_pt[0] = points[0];
                float best_angle = 3600;

                // Search the rest of the points.
                foreach (var pt in points)
                {
                    var test_angle = AngleValue(X, Y, pt.X, pt.Y);
                    if ((test_angle >= sweep_angle) && (best_angle > test_angle))
                    {
                        best_angle = test_angle;
                        best_pt[0] = pt;
                    }
                }

                // See if the first point is better.
                // If so, we are done.
                var first_angle = AngleValue(X, Y, hull[0].X, hull[0].Y);
                if ((first_angle >= sweep_angle) && (best_angle >= first_angle))
                {
                    // The first point is better. We're done.
                    break;
                }

                // Add the best point to the convex hull.
                hull.Add(best_pt[0]);
                points.Remove(best_pt[0]);

                sweep_angle = best_angle;
            }

            return hull;
        }

        // Return a number that gives the ordering of angles
        // WRST horizontal from the point (x1, y1) to (x2, y2).
        // In other words, AngleValue(x1, y1, x2, y2) is not
        // the angle, but if:
        //   Angle(x1, y1, x2, y2) > Angle(x1, y1, x2, y2)
        // then
        //   AngleValue(x1, y1, x2, y2) > AngleValue(x1, y1, x2, y2)
        // this angle is greater than the angle for another set
        // of points,) this number for
        //
        // This function is dy / (dy + dx).
        private static float AngleValue(float x1, float y1, float x2, float y2)
        {
            float t;

            var dx = x2 - x1;
            var ax = Math.Abs(dx);
            var dy = y2 - y1;
            var ay = Math.Abs(dy);
            if (ax + ay == 0)
            {
                // if (the two points are the same, return 360.
                t = 360f / 9f;
            }
            else
            {
                t = dy / (ax + ay);
            }
            if (dx < 0)
            {
                t = 2 - t;
            }
            else if (dy < 0)
            {
                t = 4 + t;
            }
            return t * 90;
        }

        // Find a minimal bounding circle.
        public static void FindMinimalBoundingCircle(List<Vector2> points, out Vector2 center, out float radius)
        {
            // Find the convex hull.
            var hull = MakeConvexHull(points);

            // The best solution so far.
            var best_center = points[0];
            var best_radius2 = float.MaxValue;

            // Look at pairs of hull points.
            for (var i = 0; i < hull.Count - 1; i++)
            {
                for (var j = i + 1; j < hull.Count; j++)
                {
                    // Find the circle through these two points.
                    var test_center = new Vector2((hull[i].X + hull[j].X) / 2f, (hull[i].Y + hull[j].Y) / 2f);
                    var dx = test_center.X - hull[i].X;
                    var dy = test_center.Y - hull[i].Y;
                    var test_radius2 = dx * dx + dy * dy;

                    // See if this circle would be an improvement.
                    if (test_radius2 < best_radius2)
                    {
                        // See if this circle encloses all of the points.
                        if (CircleEnclosesPoints(test_center, test_radius2, points, i, j, -1))
                        {
                            // Save this solution.
                            best_center = test_center;
                            best_radius2 = test_radius2;
                        }
                    }
                } // for i
            } // for j

            // Look at triples of hull points.
            for (var i = 0; i < hull.Count - 2; i++)
            {
                for (var j = i + 1; j < hull.Count - 1; j++)
                {
                    for (var k = j + 1; k < hull.Count; k++)
                    {
                        // Find the circle through these three points.
                        Vector2 test_center;
                        float test_radius2;
                        FindCircle(hull[i], hull[j], hull[k], out test_center, out test_radius2);

                        // See if this circle would be an improvement.
                        if (test_radius2 < best_radius2)
                        {
                            // See if this circle encloses all of the points.
                            if (CircleEnclosesPoints(test_center, test_radius2, points, i, j, k))
                            {
                                // Save this solution.
                                best_center = test_center;
                                best_radius2 = test_radius2;
                            }
                        }
                    } // for k
                } // for i
            } // for j

            center = best_center;
            if (best_radius2 == float.MaxValue)
            {
                radius = 0;
            }
            else
            {
                radius = (float)Math.Sqrt(best_radius2);
            }
        }

        // Return true if the indicated circle encloses all of the points.
        private static bool CircleEnclosesPoints(Vector2 center,
            float radius2,
            List<Vector2> points,
            int skip1,
            int skip2,
            int skip3)
        {
            return (from point in points.Where((t, i) => (i != skip1) && (i != skip2) && (i != skip3))
                    let dx = center.X - point.X
                    let dy = center.Y - point.Y
                    select dx * dx + dy * dy).All(test_radius2 => !(test_radius2 > radius2));
        }

        // Find a circle through the three points.
        private static void FindCircle(Vector2 a, Vector2 b, Vector2 c, out Vector2 center, out float radius2)
        {
            // Get the perpendicular bisector of (x1, y1) and (x2, y2).
            var x1 = (b.X + a.X) / 2;
            var y1 = (b.Y + a.Y) / 2;
            var dy1 = b.X - a.X;
            var dx1 = -(b.Y - a.Y);

            // Get the perpendicular bisector of (x2, y2) and (x3, y3).
            var x2 = (c.X + b.X) / 2;
            var y2 = (c.Y + b.Y) / 2;
            var dy2 = c.X - b.X;
            var dx2 = -(c.Y - b.Y);

            // See where the lines intersect.
            var cx = (y1 * dx1 * dx2 + x2 * dx1 * dy2 - x1 * dy1 * dx2 - y2 * dx1 * dx2) / (dx1 * dy2 - dy1 * dx2);
            var cy = (cx - x1) * dy1 / dx1 + y1;
            center = new Vector2(cx, cy);

            var dx = cx - a.X;
            var dy = cy - a.Y;
            radius2 = dx * dx + dy * dy;
        }

        public struct MecCircle
        {
            public Vector2 Center;
            public float Radius;

            public MecCircle(Vector2 center, float radius)
            {
                Center = center;
                Radius = radius;
            }
        }

        public static MecCircle GetMec(List<Vector2> points)
        {
            var center = new Vector2();
            float radius;

            var ConvexHull = MakeConvexHull(points);
            FindMinimalBoundingCircle(ConvexHull, out center, out radius);
            return new MecCircle(center, radius);
        }
        public static FarmLocation GetBestCircularFarmLocation(List<Vector2> minionPositions,
            float width,
            float range,
            int useMECMax = 9)
        {
            var result = new Vector2();
            var minionCount = 0;
            var startPos = ObjectManager.Player.ServerPosition.To2D();

            range = range * range;

            if (minionPositions.Count == 0)
            {
                return new FarmLocation(result, minionCount);
            }

            /* Use MEC to get the best positions only when there are less than 9 positions because it causes lag with more. */
            if (minionPositions.Count <= useMECMax)
            {
                var subGroups = GetCombinations(minionPositions);
                foreach (var subGroup in subGroups)
                {
                    if (subGroup.Count > 0)
                    {
                        var circle = GetMec(subGroup);

                        if (circle.Radius <= width && circle.Center.Distance(startPos, true) <= range)
                        {
                            minionCount = subGroup.Count;
                            return new FarmLocation(circle.Center, minionCount);
                        }
                    }
                }
            }
            else
            {
                foreach (var pos in minionPositions)
                {
                    if (pos.Distance(startPos, true) <= range)
                    {
                        var count = minionPositions.Count(pos2 => pos.Distance(pos2, true) <= width * width);

                        if (count >= minionCount)
                        {
                            result = pos;
                            minionCount = count;
                        }
                    }
                }
            }

            return new FarmLocation(result, minionCount);
        }

        private static List<List<Vector2>> GetCombinations(List<Vector2> allValues)
        {
            var collection = new List<List<Vector2>>();
            for (var counter = 0; counter < (1 << allValues.Count); ++counter)
            {
                var combination = allValues.Where((t, i) => (counter & (1 << i)) == 0).ToList();

                collection.Add(combination);
            }
            return collection;
        }

        public struct FarmLocation
        {
            public int MinionsHit;
            public Vector2 Position;

            public FarmLocation(Vector2 position, int minionsHit)
            {
                Position = position;
                MinionsHit = minionsHit;
            }
        }

    }
}
