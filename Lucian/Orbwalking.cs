#region

using System;
using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using SharpDX;

#endregion

namespace Lucian
{
    
    /// <summary>
    ///     This class offers everything related to auto-attacks and orbwalking.
    /// </summary>
    public static class Orbwalking
    {
        /// <summary>
        /// Delegate AfterAttackEvenH
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <param name="target">The target.</param>
        public delegate void AfterAttackEvenH(AttackableUnit unit, AttackableUnit target);

        /// <summary>
        /// Delegate BeforeAttackEvenH
        /// </summary>
        /// <param name="args">The <see cref="BeforeAttackEventArgs"/> instance containing the event data.</param>
        public delegate void BeforeAttackEvenH(BeforeAttackEventArgs args);

        /// <summary>
        /// Delegate OnAttackEvenH
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <param name="target">The target.</param>
        public delegate void OnAttackEvenH(AttackableUnit unit, AttackableUnit target);

        /// <summary>
        /// Delegate OnNonKillableMinionH
        /// </summary>
        /// <param name="minion">The minion.</param>
        public delegate void OnNonKillableMinionH(AttackableUnit minion);

        /// <summary>
        /// Delegate OnTargetChangeH
        /// </summary>
        /// <param name="oldTarget">The old target.</param>
        /// <param name="newTarget">The new target.</param>
        public delegate void OnTargetChangeH(AttackableUnit oldTarget, AttackableUnit newTarget);

        /// <summary>
        /// The orbwalking mode.
        /// </summary>
        public enum OrbwalkingMode
        {
            LastHit,
            Mixed,
            LaneClear,
            Combo,
            CustomMode,
            None
        }
        /// <summary>
        /// Spells that are not attacks even if they have the "attack" word in their name.
        /// </summary>
        private static readonly string[] NoAttacks =
        {
            "volleyattack", "volleyattackwithsound", "jarvanivcataclysmattack",
            "monkeykingdoubleattack", "shyvanadoubleattack",
            "shyvanadoubleattackdragon", "zyragraspingplantattack",
            "zyragraspingplantattack2", "zyragraspingplantattackfire",
            "zyragraspingplantattack2fire", "viktorpowertransfer",
            "sivirwattackbounce", "asheqattacknoonhit",
            "elisespiderlingbasicattack", "heimertyellowbasicattack",
            "heimertyellowbasicattack2", "heimertbluebasicattack",
            "annietibbersbasicattack", "annietibbersbasicattack2",
            "yorickdecayedghoulbasicattack", "yorickravenousghoulbasicattack",
            "yorickspectralghoulbasicattack", "malzaharvoidlingbasicattack",
            "malzaharvoidlingbasicattack2", "malzaharvoidlingbasicattack3",
            "kindredwolfbasicattack", "kindredbasicattackoverridelightbombfinal"
        };


        /// <summary>
        /// Spells that are attacks even if they dont have the "attack" word in their name.
        /// </summary>
        private static readonly string[] Attacks =
        {
            "caitlynheadshotmissile", "frostarrow", "garenslash2",
            "kennenmegaproc", "lucianpassiveattack", "masteryidoublestrike", "quinnwenhanced", "renektonexecute",
            "renektonsuperexecute", "rengarnewpassivebuffdash", "trundleq", "xenzhaothrust", "xenzhaothrust2",
            "xenzhaothrust3", "viktorqbuff"
        };

        /// <summary>
        /// The last auto attack tick
        /// </summary>
        public static int LastAATick;

        /// <summary>
        /// <c>true</c> if the orbwalker will attack.
        /// </summary>
        public static bool Attack = true;

        /// <summary>
        /// <c>true</c> if the orbwalker will skip the next attack.
        /// </summary>
        public static bool DisableNextAttack;

        /// <summary>
        /// <c>true</c> if the orbwalker will move.
        /// </summary>
        public static bool Move = true;

        /// <summary>
        /// The tick the most recent attack command was sent.
        /// </summary>
        public static int LastAttackCommandT;

        /// <summary>
        /// The tick the most recent move command was sent.
        /// </summary>
        public static int LastMoveCommandT;

        /// <summary>
        /// The last move command position
        /// </summary>
        public static Vector3 LastMoveCommandPosition = Vector3.Zero;

        /// <summary>
        /// The last target
        /// </summary>
        private static AttackableUnit _lastTarget;

        /// <summary>
        /// The player
        /// </summary>
        private static readonly AIHeroClient Players;

        /// <summary>
        /// The delay
        /// </summary>
        private static int _delay;

        /// <summary>
        /// The minimum distance
        /// </summary>
        private static float _minDistance = 400;

        /// <summary>
        /// <c>true</c> if the auto attack missile was launched from the player.
        /// </summary>
        private static bool _missileLaunched;

        /// <summary>
        /// The champion name
        /// </summary>
        private static string _championName;

        /// <summary>
        /// The random
        /// </summary>
        private static readonly Random _random = new Random(DateTime.Now.Millisecond);

        /// <summary>
        /// Initializes static members of the <see cref="Orbwalking"/> class.
        /// </summary>
        static Orbwalking()
        {
            Players = ObjectManager.Player;
            _championName = Players.ChampionName;
            Obj_AI_Base.OnProcessSpellCast += OnProcessSpell;
            Obj_AI_Base.OnSpellCast += Obj_AI_Base_OnDoCast;
            Spellbook.OnStopCast += SpellbookOnStopCast;
        }

        /// <summary>
        /// This event is fired before the player auto attacks.
        /// </summary>
        public static event BeforeAttackEvenH BeforeAttack;

        /// <summary>
        /// This event is fired when a unit is about to auto-attack another unit.
        /// </summary>
        public static event OnAttackEvenH OnAttack;

        /// <summary>
        /// This event is fired after a unit finishes auto-attacking another unit (Only works with player for now).
        /// </summary>
        public static event AfterAttackEvenH AfterAttack;

        /// <summary>
        /// Gets called on target changes
        /// </summary>
        public static event OnTargetChangeH OnTargetChange;

        ///<summary>
        /// Occurs when a minion is not killable by an auto attack.
        /// </summary>
        public static event OnNonKillableMinionH OnNonKillableMinion;

        /// <summary>
        /// Fires the before attack event.
        /// </summary>
        /// <param name="target">The target.</param>
        private static void FireBeforeAttack(AttackableUnit target)
        {
            if (BeforeAttack != null)
            {
                BeforeAttack(new BeforeAttackEventArgs { Target = target });
            }
            else
            {
                DisableNextAttack = false;
            }
        }

        /// <summary>
        /// Fires the on attack event.
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <param name="target">The target.</param>
        private static void FireOnAttack(AttackableUnit unit, AttackableUnit target)
        {
            OnAttack?.Invoke(unit, target);
        }

        /// <summary>
        /// Fires the after attack event.
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <param name="target">The target.</param>
        private static void FireAfterAttack(AttackableUnit unit, AttackableUnit target)
        {
            if (AfterAttack != null && target.IsValidTarget())
            {
                AfterAttack(unit, target);
            }
        }

        /// <summary>
        /// Fires the on target switch event.
        /// </summary>
        /// <param name="newTarget">The new target.</param>
        private static void FireOnTargetSwitch(AttackableUnit newTarget)
        {
            if (OnTargetChange != null && (!_lastTarget.IsValidTarget() || _lastTarget != newTarget))
            {
                OnTargetChange(_lastTarget, newTarget);
            }
        }

        /// <summary>
        /// Fires the on non killable minion event.
        /// </summary>
        /// <param name="minion">The minion.</param>
        private static void FireOnNonKillableMinion(AttackableUnit minion)
        {
            OnNonKillableMinion?.Invoke(minion);
        }

        /// <summary>
        /// Returns true if the spellname is an auto-attack.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns><c>true</c> if the name is an auto attack; otherwise, <c>false</c>.</returns>
        public static bool IsAutoAttack(string name)
        {
            return (name.ToLower().Contains("attack") && !NoAttacks.Contains(name.ToLower())) ||
                   Attacks.Contains(name.ToLower());
        }

        /// <summary>
        /// Returns the auto-attack range of local player with respect to the target.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <returns>System.Single.</returns>
        public static float GetRealAutoAttackRange(AttackableUnit target)
        {
            var result = Players.AttackRange + Players.BoundingRadius;
            if (target.IsValidTarget())
            {
                var aiBase = target as AIHeroClient;
                if (aiBase != null && Players.ChampionName == "Caitlyn")
                {
                    if (aiBase.HasBuff("caitlynyordletrapinternal"))
                    {
                        result += 650;
                    }
                }

                return result + target.BoundingRadius;
            }

            return result;
        }

        /// <summary>
        /// Returns the auto-attack range of the target.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <returns>System.Single.</returns>
        public static float GetAttackRange(AIHeroClient target)
        {
            var result = target.AttackRange + target.BoundingRadius;
            return result;
        }

        /// <summary>
        /// Returns true if the target is in auto-attack range.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool InAutoAttackRange(AttackableUnit target)
        {
            if (!target.IsValidTarget())
            {
                return false;
            }
            var myRange = GetRealAutoAttackRange(target);
            return
                Vector2.DistanceSquared(
                    (target as AIHeroClient)?.ServerPosition.To2D() ?? target.Position.To2D(),
                    Players.ServerPosition.To2D()) <= myRange * myRange;
        }

        /// <summary>
        /// Returns if the player's auto-attack is ready.
        /// </summary>
        /// <returns><c>true</c> if this instance can attack; otherwise, <c>false</c>.</returns>
        public static bool CanAttack()
        {

            return Orbwalker.GameTimeTickCount >= LastAATick + Players.AttackDelay * 1000 && Attack;
        }

        /// <summary>
        /// Returns true if moving won't cancel the auto-attack.
        /// </summary>
        /// <param name="extraWindup">The extra windup.</param>
        /// <returns><c>true</c> if this instance can move the specified extra windup; otherwise, <c>false</c>.</returns>
        public static bool CanMove(float extraWindup)
        {
            if (!Move)
            {
                return false;
            }

            if (_missileLaunched && Program.MissileCheck)
            {
                return true;
            }

            return Orbwalker.GameTimeTickCount >= LastAATick + Players.AttackCastDelay * 1000 + extraWindup;
        }

        /// <summary>
        /// Sets the movement delay.
        /// </summary>
        /// <param name="delay">The delay.</param>
        public static void SetMovementDelay(int delay)
        {
            _delay = delay;
        }

        /// <summary>
        /// Sets the minimum orbwalk distance.
        /// </summary>
        /// <param name="d">The d.</param>
        public static void SetMinimumOrbwalkDistance(float d)
        {
            _minDistance = d;
        }

        /// <summary>
        /// Gets the last move time.
        /// </summary>
        /// <returns>System.Single.</returns>
        public static float GetLastMoveTime()
        {
            return LastMoveCommandT;
        }

        /// <summary>
        /// Gets the last move position.
        /// </summary>
        /// <returns>Vector3.</returns>
        public static Vector3 GetLastMovePosition()
        {
            return LastMoveCommandPosition;
        }

        /// <summary>
        /// Moves to the position.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="holdAreaRadius">The hold area radius.</param>
        /// <param name="overrideTimer">if set to <c>true</c> [override timer].</param>
        /// <param name="useFixedDistance">if set to <c>true</c> [use fixed distance].</param>
        /// <param name="randomizeMinDistance">if set to <c>true</c> [randomize minimum distance].</param>
        /// 
        /// 
        /// 
        /// 
        public static List<Vector2> CutPath(this List<Vector2> path, float distance)
        {
            var result = new List<Vector2>();
            var Distance = distance;
            if (distance < 0)
            {
                path[0] = path[0] + distance * (path[1] - path[0]).Normalized();
                return path;
            }

            for (var i = 0; i < path.Count - 1; i++)
            {
                var dist = path[i].Distance(path[i + 1]);
                if (dist > Distance)
                {
                    result.Add(path[i] + Distance * (path[i + 1] - path[i]).Normalized());
                    for (var j = i + 1; j < path.Count; j++)
                    {
                        result.Add(path[j]);
                    }

                    break;
                }
                Distance -= dist;
            }
            return result.Count > 0 ? result : new List<Vector2> { path.Last() };
        }
        public static float PathLength(this List<Vector2> path)
        {
            var distance = 0f;
            for (var i = 0; i < path.Count - 1; i++)
            {
                distance += path[i].Distance(path[i + 1]);
            }
            return distance;
        }
        internal static class WaypointTracker
        {
            public static readonly Dictionary<int, List<Vector2>> StoredPaths = new Dictionary<int, List<Vector2>>();
            public static readonly Dictionary<int, int> StoredTick = new Dictionary<int, int>();
        }
    public static List<Vector2> GetWaypoints(this Obj_AI_Base unit)
        {
            var result = new List<Vector2>();

            if (unit.IsVisible)
            {
                result.Add(unit.ServerPosition.To2D());
                var path = unit.Path;
                if (path.Length > 0)
                {
                    var first = path[0].To2D();
                    if (first.Distance(result[0], true) > 40)
                    {
                        result.Add(first);
                    }

                    for (int i = 1; i < path.Length; i++)
                    {
                        result.Add(path[i].To2D());
                    }
                }
            }
            else if (WaypointTracker.StoredPaths.ContainsKey(unit.NetworkId))
            {
                var path = WaypointTracker.StoredPaths[unit.NetworkId];
                var timePassed = (Orbwalker.GameTimeTickCount - WaypointTracker.StoredTick[unit.NetworkId]) / 1000f;
                if (path.PathLength() >= unit.MoveSpeed * timePassed)
                {
                    result = CutPath(path, (int)(unit.MoveSpeed * timePassed));
                }
            }

            return result;
        }
        public static void MoveTo(
            Vector3 position,
            float holdAreaRadius = 0,
            bool overrideTimer = false,
            bool useFixedDistance = true,
            bool randomizeMinDistance = true)
        {
            var playerPosition = ObjectManager.Player.ServerPosition;

            if (playerPosition.Distance(position, true) < holdAreaRadius * holdAreaRadius)
            {
                if (ObjectManager.Player.Path.Length > 0)
                {
                    Player.IssueOrder(GameObjectOrder.Stop, playerPosition);
                    LastMoveCommandPosition = playerPosition;
                    LastMoveCommandT = (int)(Orbwalker.GameTimeTickCount - 70);
                }
                return;
            }

            var point = position;

            if (ObjectManager.Player.Distance(point, true) < 150 * 150)
            {
                point = (Vector3)playerPosition.Extend(position, (randomizeMinDistance ? (_random.NextFloat(0.6f, 1) + 0.2f) * _minDistance : _minDistance));
            }
            var angle = 0f;
            var currentPath = ObjectManager.Player.GetWaypoints();
            if (currentPath.Count > 1 && currentPath.PathLength() > 100)
            {
                var movePath = ObjectManager.Player.GetPath(point);

                if (movePath.Length > 1)
                {
                    var v1 = currentPath[1] - currentPath[0];
                    var v2 = movePath[1] - movePath[0];
                    angle = v1.AngleBetween(v2.To2D());
                    var distance = movePath.Last().To2D().Distance(currentPath.Last(), true);

                    if ((angle < 10 && distance < 500 * 500) || distance < 50 * 50)
                    {
                        return;
                    }
                }
            }

            if (Orbwalker.GameTimeTickCount - LastMoveCommandT < (70 + Math.Min(60, Game.Ping)) && !overrideTimer && angle < 60)
            {
                return;
            }

            if (angle >= 60 && Orbwalker.GameTimeTickCount - LastMoveCommandT < 60)
            {
                return;
            }

            Player.IssueOrder(GameObjectOrder.MoveTo, point);
            LastMoveCommandPosition = point;
            LastMoveCommandT = (int)Orbwalker.GameTimeTickCount;
        }

        /// <summary>
        /// Orbwalks a target while moving to Position.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="position">The position.</param>
        /// <param name="extraWindup">The extra windup.</param>
        /// <param name="holdAreaRadius">The hold area radius.</param>
        /// <param name="useFixedDistance">if set to <c>true</c> [use fixed distance].</param>
        /// <param name="randomizeMinDistance">if set to <c>true</c> [randomize minimum distance].</param>
        public static void Orbwalk(AttackableUnit target,
            Vector3 position,
            float extraWindup = 90,
            float holdAreaRadius = 0,
            bool useFixedDistance = true,
            bool randomizeMinDistance = true)
        {
            if (Orbwalker.GameTimeTickCount - LastAttackCommandT < (70 + Math.Min(60, Game.Ping)))
            {
                return;
            }

            try
            {
                if (target.IsValidTarget() && CanAttack())
                {
                    DisableNextAttack = false;
                    FireBeforeAttack(target);
                    Player.IssueOrder(GameObjectOrder.AttackUnit, target);
                    if (!DisableNextAttack)
                    {
                        if (Player.IssueOrder(GameObjectOrder.AttackUnit, target))
                        {
                            LastAttackCommandT = (int)Orbwalker.GameTimeTickCount;
                            _lastTarget = target;
                        }
                    }
                }

                else if (CanMove(extraWindup))
                {
                    MoveTo(position, holdAreaRadius, false, useFixedDistance, randomizeMinDistance);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        /// <summary>
        /// Resets the Auto-Attack timer.
        /// </summary>
        public static void ResetAutoAttackTimer()
        {
            LastAATick = 0;
        }

        /// <summary>
        /// Fired when the spellbook stops casting a spell.
        /// </summary>
        /// <param name="spellbook">The spellbook.</param>
        /// <param name="args">The <see cref="SpellbookStopCastEventArgs"/> instance containing the event data.</param>
        private static void SpellbookOnStopCast(Obj_AI_Base sender, SpellbookStopCastEventArgs args)
        {
            if (ObjectManager.Player.Spellbook.Owner.IsValid && ObjectManager.Player.Spellbook.Owner.IsMe && args.DestroyMissile && args.StopAnimation)
            {
                ResetAutoAttackTimer();
            }
        }

        /// <summary>
        /// Fired when an auto attack is fired.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="GameObjectProcessSpellCastEventArgs"/> instance containing the event data.</param>
        private static void Obj_AI_Base_OnDoCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsMe)
            {
                if (Game.Ping <= 30) //First world problems kappa
                {
                    Core.DelayAction(() => Obj_AI_Base_OnDoCast_Delayed(sender, args), 30);
                    return;
                }

                Obj_AI_Base_OnDoCast_Delayed(sender, args);
            }
        }

        /// <summary>
        /// Fired 30ms after an auto attack is launched.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="GameObjectProcessSpellCastEventArgs"/> instance containing the event data.</param>
        private static void Obj_AI_Base_OnDoCast_Delayed(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (IsAutoAttack(args.SData.Name))
            {
                FireAfterAttack(sender, args.Target as AttackableUnit);
                _missileLaunched = true;
            }
        }

        /// <summary>
        /// Handles the <see cref="E:ProcessSpell" /> event.
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <param name="spell">The <see cref="GameObjectProcessSpellCastEventArgs"/> instance containing the event data.</param>
        private static void OnProcessSpell(Obj_AI_Base unit, GameObjectProcessSpellCastEventArgs spell)
        {
            try
            {
                var spellName = spell.SData.Name;

                if (!IsAutoAttack(spellName))
                {
                    return;
                }

                if (unit.IsMe &&
                    (spell.Target is Obj_AI_Base || spell.Target is Obj_BarracksDampener || spell.Target is Obj_HQ))
                {
                    LastAATick = (int)(Orbwalker.GameTimeTickCount - Game.Ping / 2);
                    _missileLaunched = false;
                    LastMoveCommandT = 0;

                    if (spell.Target is Obj_AI_Base)
                    {
                        var target = (Obj_AI_Base)spell.Target;
                        if (target.IsValid)
                        {
                            FireOnTargetSwitch(target);
                            _lastTarget = target;
                        }
                    }
                }

                FireOnAttack(unit, _lastTarget);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        /// <summary>
        /// The before attack event arguments.
        /// </summary>
        public class BeforeAttackEventArgs : EventArgs
        {
            /// <summary>
            /// <c>true</c> if the orbwalker should continue with the attack.
            /// </summary>
            private bool process = true;

            /// <summary>
            /// The target
            /// </summary>
            public AttackableUnit Target;

            /// <summary>
            /// The unit
            /// </summary>
            public Obj_AI_Base Unit = ObjectManager.Player;

            /// <summary>
            /// Gets or sets a value indicating whether this <see cref="BeforeAttackEventArgs"/> should continue with the attack.
            /// </summary>
            /// <value><c>true</c> if the orbwalker should continue with the attack; otherwise, <c>false</c>.</value>
            public bool Process
            {
                get { return this.process; }
                set
                {
                    DisableNextAttack = !value;
                    this.process = value;
                }
            }
        }

        /// <summary>
        /// This class allows you to add an instance of "Orbwalker" to your assembly in order to control the orbwalking in an
        /// easy way.
        /// </summary>
        public class Orbwalker
        {
            /// <summary>
            /// The lane clear wait time modifier.
            /// </summary>
            private const float LaneClearWaitTimeMod = 2f;

            /// <summary>
            /// The configuration
            /// </summary>
            private static Menu _config;

            /// <summary>
            /// The player
            /// </summary>
            private readonly AIHeroClient Player;

            /// <summary>
            /// The forced target
            /// </summary>
            private Obj_AI_Base _forcedTarget;

            /// <summary>
            /// The orbalker mode
            /// </summary>
            private OrbwalkingMode _mode = OrbwalkingMode.None;

            /// <summary>
            /// The orbwalking point
            /// </summary>
            private Vector3 _orbwalkingPoint;

            /// <summary>
            /// The previous minion the orbwalker was targeting.
            /// </summary>
            private Obj_AI_Minion _prevMinion;

            /// <summary>
            /// The instances of the orbwalker.
            /// </summary>
            public static List<Orbwalker> Instances = new List<Orbwalker>();

            /// <summary>
            /// The name of the CustomMode if it is set.
            /// </summary>
            private string CustomModeName;
            public static Menu drawings { get; private set; }
            public static Menu ComboMenu { get; private set; }
            public static Menu HarassMenu { get; private set; }
            public static Menu LaneMenu { get; private set; }
            public static Menu LhMenu { get; private set; }
            public static Menu KillStealMenu { get; private set; }
            public static Menu MiscMenu { get; private set; }
            public static Menu DrawMenu { get; private set; }
            private static Menu menuIni;

            /// <summary>

            /// Initializes a new instance of the <see cref="Orbwalker"/> class.
            /// </summary>
            /// <param name="attachToMenu">The menu the orbwalker should attach to.</param>
            public Orbwalker(Menu attachToMenu)
            {
                /*
                _config = attachToMenu;
                drawings = attachToMenu.AddSubMenu("drawings");
                drawings.Add("Ultimate", new CheckBox("Use Ultimate?"));
                drawings.Add("Ultimate", new CheckBox("Use Ultimate?"));
                drawings.Add("Ultimate", new CheckBox("Use Ultimate?"));
                drawings.Add("Ultimate", new CheckBox("Use Ultimate?"));

                drawings.AddItem(
                    new MenuItem("AACircle", "AACircle").SetShared()
                        .SetValue(new Circle(true, Color.FromArgb(155, 255, 255, 0))));
                drawings.AddItem(
                    new MenuItem("AACircle2", "Enemy AA circle").SetShared()
                        .SetValue(new Circle(false, Color.FromArgb(155, 255, 255, 0))));
                drawings.AddItem(
                    new MenuItem("HoldZone", "HoldZone").SetShared()
                        .SetValue(new Circle(false, Color.FromArgb(155, 255, 255, 0))));
                drawings.AddItem(
                    new MenuItem("AALineWidth", "Line Width")).SetShared()
                        .SetValue(new Slider(2, 1, 6));
                _config.AddSubMenu(drawings);
                
                var misc = new Menu("Misc", "Misc");
                misc.AddItem(
                    new MenuItem("HoldPosRadius", "Hold Position Radius").SetShared().SetValue(new Slider(0, 0, 250)));
                misc.AddItem(new MenuItem("PriorizeFarm", "Priorize farm over harass").SetShared().SetValue(true));
                misc.AddItem(new MenuItem("AttackWards", "Auto attack wards").SetShared().SetValue(false));
                misc.AddItem(new MenuItem("AttackPetsnTraps", "Auto attack pets & traps").SetShared().SetValue(true));
                misc.AddItem(new MenuItem("AttackBarrel", "Auto attack gangplank barrel").SetShared().SetValue(true));
                misc.AddItem(new MenuItem("Smallminionsprio", "Jungle clear small first").SetShared().SetValue(false));
                misc.AddItem(new MenuItem("FocusMinionsOverTurrets", "Focus minions over objectives").SetShared().SetValue(new KeyBind('M', KeyBindType.Toggle)));

                _config.AddSubMenu(misc);
                
                _config.AddItem(new MenuItem("MissileCheck", "Use Missile Check").SetShared().SetValue(true));
                
                _config.AddItem(
                    new MenuItem("ExtraWindup", "Extra windup time").SetShared().SetValue(new Slider(35)));
                _config.AddItem(new MenuItem("FarmDelay", "Farm delay").SetShared().SetValue(new Slider(0)));
                
                _config.AddItem(
                    new MenuItem("LastHit", "Last hit").SetShared().SetValue(new KeyBind('X', KeyBindType.Press)));

                _config.AddItem(
                    new MenuItem("Farm", "Mixed").SetShared().SetValue(new KeyBind('C', KeyBindType.Press)));

                _config.AddItem(
                    new MenuItem("LWH", "Last Hit While Harass").SetShared().SetValue(false));

                _config.AddItem(
                    new MenuItem("LaneClear", "LaneClear").SetShared().SetValue(new KeyBind('V', KeyBindType.Press)));

                _config.AddItem(
                    new MenuItem("Orbwalk", "Combo").SetShared().SetValue(new KeyBind(32, KeyBindType.Press)));

                _config.AddItem(
                    new MenuItem("StillCombo", "Combo without moving").SetShared().SetValue(new KeyBind('N', KeyBindType.Press)));
                    */
                this.Player = ObjectManager.Player;
                Game.OnUpdate += GameOnOnGameUpdate;
                Drawing.OnDraw += DrawingOnOnDraw;
                Instances.Add(this);
            }

            /// <summary>
            /// Determines if a target is in auto attack range.
            /// </summary>
            /// <param name="target">The target.</param>
            /// <returns><c>true</c> if a target is in auto attack range, <c>false</c> otherwise.</returns>
            public virtual bool InAutoAttackRange(AttackableUnit target)
            {
                return Orbwalking.InAutoAttackRange(target);
            }

            /// <summary>
            /// Registers the Custom Mode of the Orbwalker. Useful for adding a flee mode and such.
            /// </summary>
            /// <param name="name">The name of the mode Ex. "Myassembly.FleeMode" </param>
            /// <param name="displayname">The name of the mode in the menu. Ex. Flee</param>
            /// <param name="key">The default key for this mode.</param>
            /// <summary>
            /// Gets or sets the active mode.
            /// </summary>
            /// <value>The active mode.</value>
            public OrbwalkingMode ActiveMode
            {
                get
                {
                    if (_mode != OrbwalkingMode.None)
                    {
                        return _mode;
                    }

                    if (Program.orbwalkermenu["Orbwalk"].Cast<KeyBind>().CurrentValue)
                    {
                        return OrbwalkingMode.Combo;
                    }

                    if (Program.orbwalkermenu["StillCombo"].Cast<KeyBind>().CurrentValue)
                    {
                        return OrbwalkingMode.Combo;
                    }

                    if (Program.orbwalkermenu["LaneClear"].Cast<KeyBind>().CurrentValue)
                    {
                        return OrbwalkingMode.LaneClear;
                    }

                    if (Program.orbwalkermenu["Farm"].Cast<KeyBind>().CurrentValue)
                    {
                        return OrbwalkingMode.Mixed;
                    }

                    if (Program.orbwalkermenu["LastHit"].Cast<KeyBind>().CurrentValue)
                    {
                        return OrbwalkingMode.LastHit;
                    }

                    return OrbwalkingMode.None;
                }
                set { _mode = value; }
            }

            /// <summary>
            /// Enables or disables the auto-attacks.
            /// </summary>
            /// <param name="b">if set to <c>true</c> the orbwalker will attack units.</param>
            public void SetAttack(bool b)
            {
                Attack = b;
            }

            /// <summary>
            /// Enables or disables the movement.
            /// </summary>
            /// <param name="b">if set to <c>true</c> the orbwalker will move.</param>
            public void SetMovement(bool b)
            {
                Move = b;
            }

            /// <summary>
            /// Forces the orbwalker to attack the set target if valid and in range.
            /// </summary>
            /// <param name="target">The target.</param>
            public void ForceTarget(Obj_AI_Base target)
            {
                _forcedTarget = target;
            }

            /// <summary>
            /// Forces the orbwalker to move to that point while orbwalking (Game.CursorPos by default).
            /// </summary>
            /// <param name="point">The point.</param>
            public void SetOrbwalkingPoint(Vector3 point)
            {
                _orbwalkingPoint = point;
            }

            /// <summary>
            /// Determines if the orbwalker should wait before attacking a minion.
            /// </summary>
            /// <returns><c>true</c> if the orbwalker should wait before attacking a minion, <c>false</c> otherwise.</returns>
            /// 
            /// 
            public enum Type
            {
                Attackable,
                Enemy,
                Neutral,
                Friendly,
                All
            }
            internal static IEnumerable<Obj_AI_Minion> GetCreeps(float range, Type type = Type.All)
            {
                switch (type)
                {
                    case Type.All:
                        return EntityManager.MinionsAndMonsters.Combined.Where(
                                o => ObjectManager.Player.Distance(o) <= range && o.IsValid && !o.IsDead);
                    case Type.Attackable:
                        return EntityManager.MinionsAndMonsters.Combined.Where(
                                o => ObjectManager.Player.Distance(o) <= range && o.IsValid && !o.IsDead && !o.IsAlly);
                    case Type.Enemy:
                        return EntityManager.MinionsAndMonsters.Combined.Where(
                                o => ObjectManager.Player.Distance(o) <= range && o.IsValid && !o.IsDead && o.IsEnemy);
                    case Type.Neutral:
                        return EntityManager.MinionsAndMonsters.Combined.Where(
                                o => ObjectManager.Player.Distance(o) <= range && o.IsValid && !o.IsDead && o.IsMonster);
                    case Type.Friendly:
                        return EntityManager.MinionsAndMonsters.Combined.Where(
                                o => ObjectManager.Player.Distance(o) <= range && o.IsValid && !o.IsDead && o.IsAlly);

                }
                return EntityManager.MinionsAndMonsters.Combined.Where(
                                o => ObjectManager.Player.Distance(o) <= range && o.IsValid && !o.IsDead);
            }
            private bool ShouldWait()
            {
                return
                    GetCreeps(GetRealAutoAttackRange(ObjectManager.Player), Type.Attackable)
                        .Any(
                            minion =>
                                minion.IsValidTarget() && minion.Team != GameObjectTeam.Neutral &&
                                ObjectManager.Player.IsInAutoAttackRange(minion) &&
                                Prediction.Health.GetPrediction(minion, (int)((ObjectManager.Player.AttackDelay * 1000) * 2)) <=
                                ObjectManager.Player.GetAutoAttackDamage(minion));
            }

            /// <summary>
            /// Gets the target.
            /// </summary>
            /// <returns>AttackableUnit.</returns>
            /// 
            /// 
            public enum MinionTeam
            {
                /// <summary>
                /// The minion is not on either team.
                /// </summary>
                Neutral,

                /// <summary>
                /// The minions is an ally
                /// </summary>
                Ally,

                /// <summary>
                /// The minions is an enemy
                /// </summary>
                Enemy,

                /// <summary>
                /// The minion is not an ally
                /// </summary>
                NotAlly,

                /// <summary>
                /// The minions is not an ally for the enemy
                /// </summary>
                NotAllyForEnemy,

                /// <summary>
                /// Any minion.
                /// </summary>
                All
            }

            /// <summary>
            /// The type of minion.
            /// </summary>
            public enum MinionTypes
            {
                /// <summary>
                /// Ranged minions.
                /// </summary>
                Ranged,

                /// <summary>
                /// Melee minions.
                /// </summary>
                Melee,

                /// <summary>
                /// Any minion
                /// </summary>
                All
                
            }

            public enum MinionOrderTypes
            {
                /// <summary>
                /// No order.
                /// </summary>
                None,

                /// <summary>
                /// Ordered by the current health of the minion. (Least to greatest)
                /// </summary>
                Health,

                /// <summary>
                /// Ordered by the maximum health of the minions. (Greatest to least)
                /// </summary>
                MaxHealth
            }

            public static T MinOrDefault<T, TR>(IEnumerable<T> container, Func<T, TR> valuingFoo)
                where TR : IComparable
            {
                var enumerator = container.GetEnumerator();
                if (!enumerator.MoveNext())
                {
                    return default(T);
                }

                var minElem = enumerator.Current;
                var minVal = valuingFoo(minElem);

                while (enumerator.MoveNext())
                {
                    var currVal = valuingFoo(enumerator.Current);

                    if (currVal.CompareTo(minVal) < 0)
                    {
                        minVal = currVal;
                        minElem = enumerator.Current;
                    }
                }

                return minElem;
            }
            public static bool IsMinion(Obj_AI_Minion minion, bool includeWards = false)
            {
                return minion.Name.Contains("Minion");
            }
            public static List<Obj_AI_Base> GetMinions(Vector3 from,
                float range,
                MinionTypes type = MinionTypes.All,
                MinionTeam team = MinionTeam.Enemy,
                MinionOrderTypes order = MinionOrderTypes.Health)
            {
                var result = ObjectManager.Get<Obj_AI_Minion>()
                    .Where(minion => minion.IsValidTarget(range, false, @from))
                    .Select(minion => new { minion, minionTeam = minion.Team })
                    .Where(
                        @t =>
                        team == MinionTeam.Neutral && @t.minionTeam == GameObjectTeam.Neutral
                        || team == MinionTeam.Ally
                        && @t.minionTeam
                        == (ObjectManager.Player.Team == GameObjectTeam.Chaos
                                ? GameObjectTeam.Chaos
                                : GameObjectTeam.Order)
                        || team == MinionTeam.Enemy
                        && @t.minionTeam
                        == (ObjectManager.Player.Team == GameObjectTeam.Chaos
                                ? GameObjectTeam.Order
                                : GameObjectTeam.Chaos)
                        || team == MinionTeam.NotAlly && @t.minionTeam != ObjectManager.Player.Team
                        || team == MinionTeam.NotAllyForEnemy
                        && (@t.minionTeam == ObjectManager.Player.Team || @t.minionTeam == GameObjectTeam.Neutral)
                        || team == MinionTeam.All)
                    .Where(
                        @t =>
                        @t.minion.IsMelee && type == MinionTypes.Melee
                        || !@t.minion.IsMelee && type == MinionTypes.Ranged || type == MinionTypes.All)
                    .Where(
                        @t =>
                        IsMinion(@t.minion)
                        || @t.minionTeam == GameObjectTeam.Neutral && @t.minion.MaxHealth > 5
                        && @t.minion.IsHPBarRendered)
                    .Select(@t => @t.minion).Cast<Obj_AI_Base>().ToList();

                switch (order)
                {
                    case MinionOrderTypes.Health:
                        result = result.OrderBy(o => o.Health).ToList();
                        break;
                    case MinionOrderTypes.MaxHealth:
                        result = result.OrderByDescending(o => o.MaxHealth).ToList();
                        break;
                }

                return result;
            }
            public virtual AttackableUnit GetTarget()
            {
                AttackableUnit result = null;

                if ((ActiveMode == OrbwalkingMode.Mixed || ActiveMode == OrbwalkingMode.LaneClear) &&
                    !Program.OrbMiscMenu["PriorizeFarm"].Cast<CheckBox>().CurrentValue)
                {
                    var target = TargetSelector.GetTarget(-1, DamageType.Physical);
                    if (target != null && InAutoAttackRange(target))
                    {
                        return target;
                    }
                }

                /*Killable Minion*/
                if (ActiveMode == OrbwalkingMode.LaneClear || (ActiveMode == OrbwalkingMode.Mixed && Program.LWH) ||
                    ActiveMode == OrbwalkingMode.LastHit)
                {
                    var MinionList =
                        ObjectManager.Get<Obj_AI_Minion>()
                            .Where(
                                minion =>
                                    minion.IsValidTarget() && InAutoAttackRange(minion))
                                    .OrderByDescending(minion => minion.CharData.BaseSkinName.Contains("Siege"))
                                    .ThenBy(minion => minion.CharData.BaseSkinName.Contains("Super"))
                                    .ThenBy(minion => minion.Health)
                                    .ThenByDescending(minion => minion.MaxHealth);

                    foreach (var minion in MinionList)
                    {
                        var t = (int)(this.Player.AttackCastDelay * 1000) - 100 + Game.Ping / 2 +
                                1000 * (int)Math.Max(0, this.Player.Distance(minion) - this.Player.BoundingRadius) / int.MaxValue;
                        var predHealth = HealthPrediction.GetHealthPrediction(minion, t, Program.FarmDelay);

                        if (minion.Team != GameObjectTeam.Neutral && (Program.OrbMiscMenu["AttackPetsnTraps"].Cast<CheckBox>().CurrentValue) &&
                            minion.CharData.BaseSkinName != "jarvanivstandard" || IsMinion(minion, Program.OrbMiscMenu["AttackWards"].Cast<CheckBox>().CurrentValue))
                        {
                            if (predHealth <= 0)
                            {
                                FireOnNonKillableMinion(minion);
                            }

                            if (predHealth > 0 && predHealth <= this.Player.GetAutoAttackDamage(minion, true))
                            {
                                return minion;
                            }
                        }

                        if (minion.Team == GameObjectTeam.Neutral && (Program.OrbMiscMenu["AttackBarrel"].Cast<CheckBox>().CurrentValue &&
                            minion.CharData.BaseSkinName == "gangplankbarrel" && minion.IsHPBarRendered))
                        {
                            if (minion.Health < 2)
                            {
                                return minion;
                            }
                        }
                    }
                }

                //Forced target
                if (_forcedTarget.IsValidTarget() && InAutoAttackRange(_forcedTarget))
                {
                    return _forcedTarget;
                }

                /* turrets / inhibitors / nexus */
                if (ActiveMode == OrbwalkingMode.LaneClear && (!Program.OrbMiscMenu["FocusMinionsOverTurrets"].Cast<CheckBox>().CurrentValue || !GetMinions(ObjectManager.Player.Position, GetRealAutoAttackRange(ObjectManager.Player)).Any()))
                {
                    /* turrets */
                    foreach (var turret in
                        ObjectManager.Get<Obj_AI_Turret>().Where(t => t.IsValidTarget() && InAutoAttackRange(t)))
                    {
                        return turret;
                    }

                    /* inhibitor */
                    foreach (var turret in
                        ObjectManager.Get<Obj_BarracksDampener>().Where(t => t.IsValidTarget() && InAutoAttackRange(t)))
                    {
                        return turret;
                    }

                    /* nexus */
                    foreach (var nexus in
                        ObjectManager.Get<Obj_HQ>().Where(t => t.IsValidTarget() && InAutoAttackRange(t)))
                    {
                        return nexus;
                    }
                }

                /*Champions*/
                if (ActiveMode != OrbwalkingMode.LastHit && !this.Player.HasBuff("LucianR"))
                {
                    var target = TargetSelector.GetTarget(-1, DamageType.Physical);
                    if (target.IsValidTarget() && InAutoAttackRange(target))
                    {
                        return target;
                    }
                }

                /*Jungle minions*/
                if (ActiveMode == OrbwalkingMode.LaneClear || ActiveMode == OrbwalkingMode.Mixed)
                {
                    EntityManager.MinionsAndMonsters.GetLaneMinions(
                        EntityManager.UnitTeam.Enemy, Player.ServerPosition, Player.GetAutoAttackRange()).ToArray();
                    var jminions =
                        ObjectManager.Get<Obj_AI_Minion>().Where(
                                mob =>
                                mob.IsValidTarget() && mob.Team == GameObjectTeam.Neutral && InAutoAttackRange(mob)
                                && mob.CharData.BaseSkinName != "gangplankbarrel");

                    result = Program.OrbMiscMenu["Smallminionsprio"].Cast<CheckBox>().CurrentValue
                                 ? jminions.OrderBy(it => it.MaxHealth).FirstOrDefault()
                                 : jminions.OrderBy(it => it.MaxHealth).LastOrDefault();

                    if (result != null)
                    {
                        return result;
                    }
                }

                /*Lane Clear minions*/
                if (ActiveMode == OrbwalkingMode.LaneClear)
                {
                    if (!ShouldWait())
                    {
                        if (_prevMinion.IsValidTarget() && InAutoAttackRange(_prevMinion))
                        {
                            var predHealth = HealthPrediction.LaneClearHealthPrediction(
                                _prevMinion, (int)((this.Player.AttackDelay * 1000) * LaneClearWaitTimeMod), Program.FarmDelay);
                            if (predHealth >= 2 * this.Player.GetAutoAttackDamage(_prevMinion) ||
                                Math.Abs(predHealth - _prevMinion.Health) < float.Epsilon)
                            {
                                return _prevMinion;
                            }
                        }

                        result = (from minion in
                                      ObjectManager.Get<Obj_AI_Minion>()
                                          .Where(minion => minion.IsValidTarget() && InAutoAttackRange(minion) &&
                                          (Program.OrbMiscMenu["AttackWards"].Cast<CheckBox>().CurrentValue &&
                                          (Program.OrbMiscMenu["AttackPetsnTraps"].Cast<CheckBox>().CurrentValue && minion.CharData.BaseSkinName != "jarvanivstandard" || IsMinion(minion, Program.OrbMiscMenu["AttackWards"].Cast<CheckBox>().CurrentValue)) &&
                                          minion.CharData.BaseSkinName != "gangplankbarrel"))
                                  let predHealth =
                                      HealthPrediction.LaneClearHealthPrediction(
                                          minion, (int)((this.Player.AttackDelay * 1000) * LaneClearWaitTimeMod), Program.FarmDelay)
                                  where
                                      predHealth >= 2 * Player.GetAutoAttackDamage(minion) ||
                                      Math.Abs(predHealth - minion.Health) < float.Epsilon
                                  select minion).OrderBy(it => !IsMinion(it, true)
                                  ? float.MaxValue
                                  : it.Health);


                        if (result != null)
                        {
                            _prevMinion = (Obj_AI_Minion)result;
                        }
                    }
                }

                return result;
            }

            /// <summary>
            /// Fired when the game is updated.
            /// </summary>
            /// <param name="args">The <see cref="EventArgs"/> instance containing the event data.</param>
            
        private void GameOnOnGameUpdate(EventArgs args)
            {
                try
                {
                    if (ActiveMode == OrbwalkingMode.None)
                    {
                        return;
                    }

                    //Block movement if StillCombo is used
                    Move = !Program.orbwalkermenu["StillCombo"].Cast<KeyBind>().CurrentValue;

                    //Prevent canceling important spells
                    if (Interrupt.IsCastingInterruptableSpell(ObjectManager.Player) != Interrupt.State.None)
                    {
                        return;
                    }

                    var target = this.GetTarget();
                    Orbwalk(
                        target, (this._orbwalkingPoint.To2D().IsValid()) ? _orbwalkingPoint : Game.CursorPos,
                        Program.ExtraWindup,
                        Math.Max(Program.HoldPosRadius, 30));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            /// <summary>
            /// Fired when the game is drawn.
            /// </summary>
            /// <param name="args">The <see cref="EventArgs"/> instance containing the event data.</param>
            private void DrawingOnOnDraw(EventArgs args)
            {
                if (Program.AACircle)
                {
                    Circle.Draw(Color.White, Player.GetAutoAttackRange(), Player.ServerPosition);
                }

                if (Program.AACircle2)
                {
                    foreach (var target in
                        HeroManager.Enemies.FindAll(target => target.IsValidTarget(1175)))
                    {
                        Circle.Draw(Color.White, GetAttackRange(target), target.ServerPosition);
                    }
                }

                if (Program.HoldZone)
                {

                    Circle.Draw(Color.White, Program.HoldPosRadius, Player.ServerPosition);
                }
                /*
                Program.OrbMiscMenu["FocusMinionsOverTurrets"].Cast<CheckBox>().CurrentValue
                _config.Item("FocusMinionsOverTurrets").Permashow(_config.Item("FocusMinionsOverTurrets").GetValue<KeyBind>().Active);
                */
            }

            public static int GameTimeTickCount
            {
                get
                {
                    return (int)(GameTimeTickCount * 1000);
                }
            }
        }

        public class HeroManager
        {

            /// <summary>
            /// Gets all heroes.
            /// </summary>
            /// <value>
            /// All heroes.
            /// </value>
            public static List<AIHeroClient> AllHeroes { get; private set; }

            /// <summary>
            /// Gets the allies.
            /// </summary>
            /// <value>
            /// The allies.
            /// </value>
            public static List<AIHeroClient> Allies { get; private set; }

            /// <summary>
            /// Gets the enemies.
            /// </summary>
            /// <value>
            /// The enemies.
            /// </value>
            public static List<AIHeroClient> Enemies { get; private set; }

            /// <summary>
            /// Gets the Local Player
            /// </summary>
            public static AIHeroClient Player { get; private set; }

            /// <summary>
            /// Initializes static members of the <see cref="HeroManager"/> class. 
            /// </summary>
            static HeroManager()
            {
                if (Game.Mode == GameMode.Running)
                {
                    Game_OnStart(new EventArgs());
                }
                Game.OnLoad += Game_OnStart;
            }

            /// <summary>
            /// Fired when the game starts.
            /// </summary>
            /// <param name="args">The <see cref="EventArgs"/> instance containing the event data.</param>
            static void Game_OnStart(EventArgs args)
            {
                AllHeroes = ObjectManager.Get<AIHeroClient>().ToList();
                Allies = AllHeroes.FindAll(o => o.IsAlly);
                Enemies = AllHeroes.FindAll(o => o.IsEnemy);
                Player = AllHeroes.Find(x => x.IsMe);
            }
        }

        public class HealthPrediction
        {
            /// <summary>
            /// The active attacks
            /// </summary>
            private static readonly Dictionary<int, PredictedDamage> ActiveAttacks = new Dictionary<int, PredictedDamage>();

            /// <summary>
            /// Initializes static members of the <see cref="HealthPrediction"/> class. 
            /// </summary>
            static HealthPrediction()
            {
                Obj_AI_Base.OnProcessSpellCast += ObjAiBaseOnOnProcessSpellCast;
                Game.OnUpdate += Game_OnGameUpdate;
                Spellbook.OnStopCast += SpellbookOnStopCast1;
                MissileClient.OnDelete += MissileClient_OnDelete;
                Obj_AI_Base.OnProcessSpellCast += Obj_AI_Base_OnDoCast;
            }
            
            /// <summary>
            /// Fired when a unit does an auto attack.
            /// </summary>
            /// <param name="sender">The sender.</param>
            /// <param name="args">The <see cref="GameObjectProcessSpellCastEventArgs"/> instance containing the event data.</param>
            private static void Obj_AI_Base_OnDoCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
            {
                if (ActiveAttacks.ContainsKey(sender.NetworkId) && sender.IsMelee)
                {
                    ActiveAttacks[sender.NetworkId].Processed = true;
                }
            }

            /// <summary>
            /// Fired when a <see cref="MissileClient"/> is deleted from the game.
            /// </summary>
            /// <param name="sender">The sender.</param>
            /// <param name="args">The <see cref="EventArgs"/> instance containing the event data.</param>
            static void MissileClient_OnDelete(GameObject sender, EventArgs args)
            {
                var missile = sender as MissileClient;
                if (missile != null && missile.SpellCaster != null)
                {
                    var casterNetworkId = missile.SpellCaster.NetworkId;
                    foreach (var activeAttack in ActiveAttacks)
                    {
                        if (activeAttack.Key == casterNetworkId)
                        {
                            ActiveAttacks[casterNetworkId].Processed = true;
                        }
                    }
                }
            }

            /// <summary>
            /// Fired when the game is updated.
            /// </summary>
            /// <param name="args">The <see cref="EventArgs"/> instance containing the event data.</param>
            private static void Game_OnGameUpdate(EventArgs args)
            {
                ActiveAttacks.ToList()
                    .Where(pair => pair.Value.StartTick < Orbwalker.GameTimeTickCount - 3000)
                    .ToList()
                    .ForEach(pair => ActiveAttacks.Remove(pair.Key));
            }

            /// <summary>
            /// Fired when the spellbooks stops a cast.
            /// </summary>
            /// <param name="spellbook">The spellbook.</param>
            /// <param name="args">The <see cref="SpellbookStopCastEventArgs"/> instance containing the event data.</param>
            private static void SpellbookOnStopCast1(Obj_AI_Base sender, SpellbookStopCastEventArgs args)
            {
                if (sender.IsValid && args.StopAnimation)
                {
                    if (ActiveAttacks.ContainsKey(sender.NetworkId))
                    {
                        ActiveAttacks.Remove(sender.NetworkId);
                    }
                }
            }

            /// <summary>
            /// Fired when the game processes a spell cast.
            /// </summary>
            /// <param name="sender">The sender.</param>
            /// <param name="args">The <see cref="GameObjectProcessSpellCastEventArgs"/> instance containing the event data.</param>
            private static void ObjAiBaseOnOnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
            {
                if (!sender.IsValidTarget(3000, false) || sender.Team != ObjectManager.Player.Team || sender is AIHeroClient
                    || !Orbwalking.IsAutoAttack(args.SData.Name) || !(args.Target is Obj_AI_Base))
                {
                    return;
                }

                var target = (Obj_AI_Base)args.Target;
                ActiveAttacks.Remove(sender.NetworkId);

                var attackData = new PredictedDamage(
                    sender,
                    target,
                    Orbwalker.GameTimeTickCount - Game.Ping / 2,
                    sender.AttackCastDelay * 1000,
                    sender.AttackDelay * 1000 - (sender is Obj_AI_Turret ? 70 : 0),
                    sender.IsMelee ? int.MaxValue : (int)args.SData.MissileSpeed,
                    (float)sender.GetAutoAttackDamage(target, true));
                ActiveAttacks.Add(sender.NetworkId, attackData);
            }

            /// <summary>
            /// Returns the unit health after a set time milliseconds.
            /// </summary>
            /// <param name="unit">The unit.</param>
            /// <param name="time">The time.</param>
            /// <param name="delay">The delay.</param>
            /// <returns></returns>
            public static float GetHealthPrediction(Obj_AI_Base unit, int time, int delay = 70)
            {
                var predictedDamage = 0f;

                foreach (var attack in ActiveAttacks.Values)
                {
                    var attackDamage = 0f;
                    if (!attack.Processed && attack.Source.IsValidTarget(float.MaxValue, false) &&
                        attack.Target.IsValidTarget(float.MaxValue, false) && attack.Target.NetworkId == unit.NetworkId)
                    {
                        var landTime = attack.StartTick + attack.Delay +
                                       1000 * Math.Max(0, unit.Distance(attack.Source) - attack.Source.BoundingRadius) / attack.ProjectileSpeed + delay;

                        if (/*Orbwalker.GameTimeTickCount < landTime - delay &&*/ landTime < Orbwalker.GameTimeTickCount + time)
                        {
                            attackDamage = attack.Damage;
                        }
                    }

                    predictedDamage += attackDamage;
                }

                return unit.Health - predictedDamage;
            }

            /// <summary>
            /// Returns the unit health after time milliseconds assuming that the past auto-attacks are periodic.
            /// </summary>
            /// <param name="unit">The unit.</param>
            /// <param name="time">The time.</param>
            /// <param name="delay">The delay.</param>
            /// <returns></returns>
            public static float LaneClearHealthPrediction(Obj_AI_Base unit, int time, int delay = 70)
            {
                var predictedDamage = 0f;

                foreach (var attack in ActiveAttacks.Values)
                {
                    var n = 0;
                    if (Orbwalker.GameTimeTickCount - 100 <= attack.StartTick + attack.AnimationTime &&
                        attack.Target.IsValidTarget(float.MaxValue, false) &&
                        attack.Source.IsValidTarget(float.MaxValue, false) && attack.Target.NetworkId == unit.NetworkId)
                    {
                        var fromT = attack.StartTick;
                        var toT = Orbwalker.GameTimeTickCount + time;

                        while (fromT < toT)
                        {
                            if (fromT >= Orbwalker.GameTimeTickCount &&
                                (fromT + attack.Delay + Math.Max(0, unit.Distance(attack.Source) - attack.Source.BoundingRadius) / attack.ProjectileSpeed < toT))
                            {
                                n++;
                            }
                            fromT += (int)attack.AnimationTime;
                        }
                    }
                    predictedDamage += n * attack.Damage;
                }

                return unit.Health - predictedDamage;
            }

            /// <summary>
            /// Determines whether the specified minion has minion aggro.
            /// </summary>
            /// <param name="minion">The minion.</param>
            /// <returns></returns>
            public static bool HasMinionAggro(Obj_AI_Minion minion)
            {
                return ActiveAttacks.Values.Any(m => (m.Source is Obj_AI_Minion) && m.Target.NetworkId == minion.NetworkId);
            }
            /// <summary>
            /// Determines whether the specified minion has turret aggro.
            /// </summary>
            /// <param name="minion">The minion</param>
            /// <returns></returns>
            public static bool HasTurretAggro(Obj_AI_Minion minion)
            {
                return ActiveAttacks.Values.Any(m => (m.Source is Obj_AI_Turret) && m.Target.NetworkId == minion.NetworkId);
            }
            /// <summary>
            /// Return the starttick of the attacking turret.
            /// </summary>
            /// <param name="minion"></param>
            /// <returns></returns>
            public static int TurretAggroStartTick(Obj_AI_Minion minion)
            {
                var ActiveTurret = ActiveAttacks.Values
                    .FirstOrDefault(m => (m.Source is Obj_AI_Turret) && m.Target.NetworkId == minion.NetworkId);
                return ActiveTurret != null ? ActiveTurret.StartTick : 0;
            }
            /// <summary>
            /// Return the Attacking turret.
            /// </summary>
            /// <param name="minion"></param>
            /// <returns></returns>
            public static Obj_AI_Base GetAggroTurret(Obj_AI_Minion minion)
            {
                var ActiveTurret = ActiveAttacks.Values
                    .FirstOrDefault(m => (m.Source is Obj_AI_Turret) && m.Target.NetworkId == minion.NetworkId);
                return ActiveTurret != null ? ActiveTurret.Source : null;
            }
            /// <summary>
            /// Represetns predicted damage.
            /// </summary>
            private class PredictedDamage
            {
                /// <summary>
                /// The animation time
                /// </summary>
                public readonly float AnimationTime;

                /// <summary>
                /// Gets or sets the damage.
                /// </summary>
                /// <value>
                /// The damage.
                /// </value>
                public float Damage { get; private set; }

                /// <summary>
                /// Gets or sets the delay.
                /// </summary>
                /// <value>
                /// The delay.
                /// </value>
                public float Delay { get; private set; }

                /// <summary>
                /// Gets or sets the projectile speed.
                /// </summary>
                /// <value>
                /// The projectile speed.
                /// </value>
                public int ProjectileSpeed { get; private set; }

                /// <summary>
                /// Gets or sets the source.
                /// </summary>
                /// <value>
                /// The source.
                /// </value>
                public Obj_AI_Base Source { get; private set; }

                /// <summary>
                /// Gets or sets the start tick.
                /// </summary>
                /// <value>
                /// The start tick.
                /// </value>
                public int StartTick { get; internal set; }

                /// <summary>
                /// Gets or sets the target.
                /// </summary>
                /// <value>
                /// The target.
                /// </value>
                public Obj_AI_Base Target { get; private set; }

                /// <summary>
                /// Gets or sets a value indicating whether this <see cref="PredictedDamage"/> is processed.
                /// </summary>
                /// <value>
                ///   <c>true</c> if processed; otherwise, <c>false</c>.
                /// </value>
                public bool Processed { get; internal set; }

                /// <summary>
                /// Initializes a new instance of the <see cref="PredictedDamage"/> class.
                /// </summary>
                /// <param name="source">The source.</param>
                /// <param name="target">The target.</param>
                /// <param name="startTick">The start tick.</param>
                /// <param name="delay">The delay.</param>
                /// <param name="animationTime">The animation time.</param>
                /// <param name="projectileSpeed">The projectile speed.</param>
                /// <param name="damage">The damage.</param>
                public PredictedDamage(Obj_AI_Base source,
                    Obj_AI_Base target,
                    int startTick,
                    float delay,
                    float animationTime,
                    int projectileSpeed,
                    float damage)
                {
                    Source = source;
                    Target = target;
                    StartTick = startTick;
                    Delay = delay;
                    ProjectileSpeed = projectileSpeed;
                    Damage = damage;
                    AnimationTime = animationTime;
                }
            }
        }
    }

    class Interrupt
    {
        public enum State
        {
            None,
            CanMove,
            CanNothing
        }
        public enum DangerLevel
        {
            Low,
            Medium,
            High
        }

        public static List<InterruptableSpells> InterruptableSpellList = GetList();

        private static List<InterruptableSpells> GetList()
        {
            if (InterruptableSpellList != null) return InterruptableSpellList;
            var list = new List<InterruptableSpells>();
            list.Add(new InterruptableSpells("Caitlyn", SpellSlot.R, DangerLevel.High));
            list.Add(new InterruptableSpells("FiddleSticks", SpellSlot.W, DangerLevel.Medium));
            list.Add(new InterruptableSpells("FiddleSticks", SpellSlot.R, DangerLevel.High));
            list.Add(new InterruptableSpells("Galio", SpellSlot.R, DangerLevel.High));
            list.Add(new InterruptableSpells("Janna", SpellSlot.R, DangerLevel.Low));
            list.Add(new InterruptableSpells("Karthus", SpellSlot.R, DangerLevel.High));
            list.Add(new InterruptableSpells("Katarina", SpellSlot.R, DangerLevel.High));
            list.Add(new InterruptableSpells("Lucian", SpellSlot.R, DangerLevel.High, true));
            list.Add(new InterruptableSpells("Malzahar", SpellSlot.R, DangerLevel.High));
            list.Add(new InterruptableSpells("MasterYi", SpellSlot.W, DangerLevel.Low));
            list.Add(new InterruptableSpells("MissFortune", SpellSlot.R, DangerLevel.High));
            list.Add(new InterruptableSpells("Nunu", SpellSlot.R, DangerLevel.High));
            list.Add(new InterruptableSpells("Pantheon", SpellSlot.E, DangerLevel.Low));
            list.Add(new InterruptableSpells("Pantheon", SpellSlot.R, DangerLevel.High));
            list.Add(new InterruptableSpells("RekSai", SpellSlot.R, DangerLevel.High));
            list.Add(new InterruptableSpells("Sion", SpellSlot.R, DangerLevel.Low));
            list.Add(new InterruptableSpells("Shen", SpellSlot.R, DangerLevel.Low));
            list.Add(new InterruptableSpells("TwistedFate", SpellSlot.R, DangerLevel.Medium));
            list.Add(new InterruptableSpells("Urgot", SpellSlot.R, DangerLevel.High));
            list.Add(new InterruptableSpells("Varus", SpellSlot.Q, DangerLevel.Low, true));
            list.Add(new InterruptableSpells("Velkoz", SpellSlot.R, DangerLevel.High));
            list.Add(new InterruptableSpells("Warwick", SpellSlot.R, DangerLevel.High));
            list.Add(new InterruptableSpells("Xerath", SpellSlot.R, DangerLevel.High));
            InterruptableSpellList = list;
            return list;
        }

        internal static State IsCastingInterruptableSpell(AIHeroClient hero)
        {
            return (from interruptableSpell in InterruptableSpellList
                    where interruptableSpell.ChampionName == hero.ChampionName
                    where hero.Spellbook.IsCastingSpell || hero.Spellbook.IsChanneling || hero.Spellbook.IsCharging
                    where hero.Spellbook.ActiveSpellSlot == interruptableSpell.Spellslot
                    where hero.Spellbook.CastEndTime < Game.Time
                    select interruptableSpell).Select(interruptableSpell => interruptableSpell.CanMove ? State.CanMove : State.CanNothing).FirstOrDefault();
        }
    }

    class InterruptableSpells
    {
        public string ChampionName;
        public SpellSlot Spellslot;
        public Interrupt.DangerLevel DangerLevel;
        public bool CanMove;
        public InterruptableSpells(string champname, SpellSlot spellSlot, Interrupt.DangerLevel dangerlevel, bool canmove = false)
        {
            ChampionName = champname;
            Spellslot = spellSlot;
            DangerLevel = dangerlevel;
            CanMove = canmove;
        }

    }
}
