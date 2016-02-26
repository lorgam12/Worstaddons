using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Menu.Values;
using ItemData = EloBuddy.ItemData;
using SharpDX;
using EloBuddy.SDK.Rendering;


namespace Lucian
{
    using System.Collections.Generic;

    public class Program
    {

        public static readonly Item Youmuu = new Item((int)ItemId.Youmuus_Ghostblade);
        public static Menu OrbDrawMenu { get; private set; }
        public static Menu OrbMiscMenu { get; private set; }
        public static Menu ComboMenu { get; private set; }
        public static Menu HarassMenu { get; private set; }
        public static Menu LaneMenu { get; private set; }
        public static Menu KillStealMenu { get; private set; }
        public static Menu AutoMenu { get; private set; }
        public static Menu MiscMenu { get; private set; }
        public static Menu DrawMenu { get; private set; }
        private static Menu menuIni;

        public static Menu orbwalkermenu;
        private static Orbwalking.Orbwalker orbwalker;
        private static AIHeroClient player = ObjectManager.Player;
        private static HpBarIndicator indicator = new HpBarIndicator();
        private static Spell.Skillshot q1, w, e, r;
        private static Spell.Targeted q;
        private static bool aaPassive;
        public static bool LWH => orbwalkermenu["LWH"].Cast<CheckBox>().CurrentValue;
        public static bool MissileCheck => orbwalkermenu["MissileCheck"].Cast<CheckBox>().CurrentValue;
        public static int FarmDelay => orbwalkermenu["FarmDelay"].Cast<Slider>().CurrentValue;
        public static int ExtraWindup => orbwalkermenu["ExtraWindup"].Cast<Slider>().CurrentValue;
        public static int HoldPosRadius => OrbMiscMenu["HoldPosRadius"].Cast<Slider>().CurrentValue;
        public static bool AACircle => OrbDrawMenu["AACircle"].Cast<CheckBox>().CurrentValue;
        public static bool AACircle2 => OrbDrawMenu["AACircle2"].Cast<CheckBox>().CurrentValue;
        public static bool HoldZone => OrbDrawMenu["HoldZone"].Cast<CheckBox>().CurrentValue;
        public static bool AALineWidth => OrbDrawMenu["AALineWidth"].Cast<CheckBox>().CurrentValue;

        private static bool Nocolision => MiscMenu["Nocolision"].Cast<CheckBox>().CurrentValue;
        private static bool Hexq => HarassMenu["HEXQ"].Cast<CheckBox>().CurrentValue;
        private static bool KillstealQ => KillStealMenu["KillstealQ"].Cast<CheckBox>().CurrentValue;
        private static bool Cq => ComboMenu["CQ"].Cast<CheckBox>().CurrentValue;
        private static bool Cw => ComboMenu["CW"].Cast<CheckBox>().CurrentValue;
        private static int Ce => ComboMenu["CE"].Cast<Slider>().CurrentValue;
        private static bool Hq => HarassMenu["HQ"].Cast<CheckBox>().CurrentValue;
        private static bool Hw => HarassMenu["HW"].Cast<CheckBox>().CurrentValue;
        private static int He => HarassMenu["HE"].Cast<Slider>().CurrentValue;
        private static int HMinMana => HarassMenu["HMinMana"].Cast<Slider>().CurrentValue;
        private static bool Jq => LaneMenu["JQ"].Cast<CheckBox>().CurrentValue;
        private static bool Jw => LaneMenu["JW"].Cast<CheckBox>().CurrentValue;
        private static bool Je => LaneMenu["JE"].Cast<CheckBox>().CurrentValue;
        private static bool Lhq => LaneMenu["LHQ"].Cast<CheckBox>().CurrentValue;
        private static int Lq => LaneMenu["LQ"].Cast<Slider>().CurrentValue;
        private static bool Lw => LaneMenu["LW"].Cast<CheckBox>().CurrentValue;
        private static bool Le => LaneMenu["LE"].Cast<CheckBox>().CurrentValue;
        private static int LMinMana => LaneMenu["LMinMana"].Cast<Slider>().CurrentValue;
        private static bool Dind => DrawMenu["Dind"].Cast<CheckBox>().CurrentValue;
        private static bool Deq => DrawMenu["DEQ"].Cast<CheckBox>().CurrentValue;
        private static bool Dq => DrawMenu["DQ"].Cast<CheckBox>().CurrentValue;
        private static bool Dw => DrawMenu["DW"].Cast<CheckBox>().CurrentValue;
        private static bool De => DrawMenu["DE"].Cast<CheckBox>().CurrentValue;
        static bool AutoQ => AutoMenu["AutoQ"].Cast<KeyBind>().CurrentValue;
        private static int MinMana => AutoMenu["MinMana"].Cast<Slider>().CurrentValue;
        private static int HhMinMana => HarassMenu["HHMinMana"].Cast<Slider>().CurrentValue;
        private static int Humanizer => MiscMenu["Humanizer"].Cast<Slider>().CurrentValue;
        static bool ForceR => ComboMenu["ForceR"].Cast<KeyBind>().CurrentValue;
        static bool Lt => LaneMenu["LT"].Cast<KeyBind>().CurrentValue;

        static void Main()
        {
            Loading.OnLoadingComplete += OnGameLoad;
        }

        static void OnGameLoad(EventArgs args)
        {
            if (player.ChampionName != "Lucian") return;
            Chat.Print("Hoola Lucian - Loaded Successfully, Good Luck! :)");
            q = new Spell.Targeted(SpellSlot.Q, 675);
            q1 = new Spell.Skillshot(SpellSlot.Q, 1140, SkillShotType.Linear, 350, int.MaxValue, 75);
            w = new Spell.Skillshot(SpellSlot.W, 1000, SkillShotType.Linear, 250, 1600, 100);
            e = new Spell.Skillshot(SpellSlot.E, 475, SkillShotType.Linear);
            r = new Spell.Skillshot(SpellSlot.R, 1400, SkillShotType.Linear, 500, 2800, 110);

            OnMenuLoad();

            Spellbook.OnCastSpell += Spellbook_OnCastSpell;
            Game.OnUpdate += Game_OnUpdate;
            Drawing.OnEndScene += Drawing_OnEndScene;
            Obj_AI_Base.OnProcessSpellCast += OnDoCast;
            Drawing.OnDraw += OnDraw;
            Obj_AI_Base.OnProcessSpellCast += OnDoCastLc;
        }
        private static void OnMenuLoad()
        {
            orbwalkermenu = MainMenu.AddMenu("Hoola OrbWalker", "Hoola OrbWalker");
            orbwalkermenu.AddGroupLabel("Hoola OrbWalker");
            orbwalkermenu.Add("MissileCheck", new CheckBox("Missile Check"));
            orbwalkermenu.Add("LWH", new CheckBox("Last Hit While Harass"));
            orbwalkermenu.Add("ExtraWindup", new Slider("Extra windup time", 80, 0, 250));
            orbwalkermenu.Add("FarmDelay", new Slider("Farm Delay", 80, 0, 250));
            orbwalkermenu.Add("LastHit", new KeyBind("Last Hit", false, KeyBind.BindTypes.HoldActive));
            orbwalkermenu.Add("Farm", new KeyBind("Mixed", false, KeyBind.BindTypes.HoldActive));
            orbwalkermenu.Add("LaneClear", new KeyBind("Lane Clear", false, KeyBind.BindTypes.HoldActive));
            orbwalkermenu.Add("Orbwalk", new KeyBind("Combo", false, KeyBind.BindTypes.HoldActive));
            orbwalkermenu.Add("StillCombo", new KeyBind("Combo without moving", false, KeyBind.BindTypes.HoldActive));


            OrbMiscMenu = orbwalkermenu.AddSubMenu("Misc");
            OrbMiscMenu.AddGroupLabel("Misc");
            OrbMiscMenu.Add("PriorizeFarm", new CheckBox("Priorize Farm"));
            OrbMiscMenu.Add("AttackWards", new CheckBox("Attack Wards"));
            OrbMiscMenu.Add("AttackPetsnTraps", new CheckBox("Attack Pets and Traps"));
            OrbMiscMenu.Add("AttackBarrel", new CheckBox("Attack Barrel"));
            OrbMiscMenu.Add("Smallminionsprio", new CheckBox("Small minions prio"));
            OrbMiscMenu.Add("FocusMinionsOverTurrets", new CheckBox("Focus Minions Over Turrets"));
            OrbMiscMenu.Add("HoldPosRadius", new Slider("Hold Position Radius", 1, 0, 2));


            OrbDrawMenu = orbwalkermenu.AddSubMenu("Drawings");
            OrbDrawMenu.AddGroupLabel("Drawings");
            OrbDrawMenu.Add("AACircle", new CheckBox("AACircle"));
            OrbDrawMenu.Add("AACircle2", new CheckBox("Enemy AA circle"));
            OrbDrawMenu.Add("HoldZone", new CheckBox("HoldZone"));
            OrbDrawMenu.Add("AALineWidth", new Slider("Line Width", 80, 0, 250));


            menuIni = MainMenu.AddMenu("Hoola Lucian", "Hoola Lucian");
            menuIni.AddGroupLabel("Welcome to the Worst Lucain addon!");
            menuIni.AddGroupLabel("Global Settings");
            menuIni.Add("Combo", new CheckBox("Use Combo?"));
            menuIni.Add("Harass", new CheckBox("Use Harass?"));
            menuIni.Add("LaneClear", new CheckBox("Use Lane Clear?"));
            menuIni.Add("JungleClear", new CheckBox("Use Jungle Clear?"));
            menuIni.Add("KillSteal", new CheckBox("Use Kill Steal?"));
            menuIni.Add("Misc", new CheckBox("Use Misc?"));
            menuIni.Add("Drawings", new CheckBox("Use Drawings?"));


            ComboMenu = menuIni.AddSubMenu("Combo");
            ComboMenu.AddGroupLabel("Combo Settings");
            ComboMenu.Add("CQ", new CheckBox("Use Q"));
            ComboMenu.Add("CW", new CheckBox("Use W"));
            ComboMenu.Add("CE", new CheckBox("Use E"));
            ComboMenu.Add("CES", new Slider("Use E Mode", 1, 0, 2));
            ComboMenu.Add("ForceR", new KeyBind("Force R On Target Selector", false, KeyBind.BindTypes.HoldActive));
            ComboMenu.AddLabel("Modes:");
            ComboMenu.AddLabel("0: Side");
            ComboMenu.AddLabel("1: Cursor");
            ComboMenu.AddLabel("2: Enemy");


            HarassMenu = menuIni.AddSubMenu("Harass");
            HarassMenu.AddGroupLabel("Harass Settings");
            HarassMenu.Add("HEXQ", new CheckBox("Use Extended Q"));
            HarassMenu.Add("HMinMana", new Slider("Extended Q Min Mana (%)", 30, 0, 100));
            HarassMenu.Add("HQ", new CheckBox("Use Q"));
            HarassMenu.Add("HW", new CheckBox("Use W", false));
            HarassMenu.Add("HE", new CheckBox("Use E"));
            HarassMenu.Add("HHMinMana", new Slider("Harass Min Mana (%)", 30, 0, 100));


            LaneMenu = menuIni.AddSubMenu("Farm");
            LaneMenu.AddGroupLabel("LaneClear Settings");
            LaneMenu.Add("LT", new CheckBox("Use Spell LaneClear (Toggle)"));
            LaneMenu.Add("LQ", new Slider("Use Q Min Minions", 3, 1, 10));
            LaneMenu.Add("LHQ", new CheckBox("Use Extended Q For Harass"));
            LaneMenu.Add("LW", new CheckBox("Use Q"));
            LaneMenu.Add("LE", new CheckBox("Use Q"));
            LaneMenu.Add("LMinMana", new Slider("Use Q Mana %", 30, 0, 100));

            LaneMenu.AddSeparator();
            LaneMenu.AddGroupLabel("JungleClear Settings");
            LaneMenu.Add("JQ", new CheckBox("Use Q"));
            LaneMenu.Add("JW", new CheckBox("Use W"));
            LaneMenu.Add("JE", new CheckBox("Use E"));


            AutoMenu = menuIni.AddSubMenu("Automatic");
            AutoMenu.AddGroupLabel("Automatic Settings");
            AutoMenu.Add("AutoQ", new CheckBox("Auto Q"));
            AutoMenu.Add("MinMana", new Slider("Auto Q Mana %", 30, 0, 100));


            KillStealMenu = menuIni.AddSubMenu("Kill Steal");
            KillStealMenu.AddGroupLabel("Kill Steal Settings");
            KillStealMenu.Add("KillstealQ", new CheckBox("Kill Steal Q"));

            MiscMenu = menuIni.AddSubMenu("Misc");
            MiscMenu.AddGroupLabel("Misc Settings");
            MiscMenu.Add("Humanizer", new Slider("Humanizer Delay", 5, 5, 1000));
            MiscMenu.Add("Nocolision", new CheckBox("Nocolision W"));


            DrawMenu = menuIni.AddSubMenu("Drawings");
            DrawMenu.AddGroupLabel("Drawing Settings");
            DrawMenu.Add("Dind", new CheckBox("Draw Damage Incidator"));
            DrawMenu.Add("DEQ", new CheckBox("Draw Extended Q"));
            DrawMenu.Add("DQ", new CheckBox("Draw Q"));
            DrawMenu.Add("DW", new CheckBox("Draw W"));
            DrawMenu.Add("DE", new CheckBox("Draw E"));

            /*
            Menu = new Menu("Hoola Lucian", "hoolalucian", true);

            Menu.AddSubMenu(new Menu("Orbwalking", "Orbwalking"));
            Orbwalker = new Orbwalking.Orbwalker(Menu.SubMenu("Orbwalking"));

            var targetSelectorMenu = new Menu("Target Selector", "Target Selector");
            TargetSelector.AddToMenu(targetSelectorMenu);
            Menu.AddSubMenu(targetSelectorMenu);

            var Combo = new Menu("Combo", "Combo");
            Combo.AddItem(new MenuItem("CQ", "Use Q").SetValue(true));
            Combo.AddItem(new MenuItem("CW", "Use W").SetValue(true));
            Combo.AddItem(new MenuItem("CE", "Use E Mode").SetValue(new StringList(new[] { "Side", "Cursor", "Enemy", "Never" })));
            Combo.AddItem(new MenuItem("ForceR", "Force R On Target Selector").SetValue(new KeyBind('T', KeyBindType.Press)));
            Menu.AddSubMenu(Combo);

            var Misc = new Menu("Misc", "Misc");
            Misc.AddItem(new MenuItem("Humanizer", "Humanizer Delay").SetValue(new Slider(5,5,300)));
            Misc.AddItem(new MenuItem("Nocolision", "Nocolision W").SetValue(true));
            Menu.AddSubMenu(Misc);


            var Harass = new Menu("Harass", "Harass");
            Harass.AddItem(new MenuItem("HEXQ", "Use Extended Q").SetValue(true));
            Harass.AddItem(new MenuItem("HMinMana", "Extended Q Min Mana (%)").SetValue(new Slider(80)));
            Harass.AddItem(new MenuItem("HQ", "Use Q").SetValue(true));
            Harass.AddItem(new MenuItem("HW", "Use W").SetValue(true));
            Harass.AddItem(new MenuItem("HE", "Use E Mode").SetValue(new StringList(new [] {"Side","Cursor","Enemy","Never"})));
            Harass.AddItem(new MenuItem("HHMinMana", "Harass Min Mana (%)").SetValue(new Slider(80)));
            Menu.AddSubMenu(Harass);

            var LC = new Menu("LaneClear", "LaneClear");
            LC.AddItem(new MenuItem("LT", "Use Spell LaneClear (Toggle)").SetValue(new KeyBind('J', KeyBindType.Toggle)));
            LC.AddItem(new MenuItem("LHQ", "Use Extended Q For Harass").SetValue(true));
            LC.AddItem(new MenuItem("LQ", "Use Q (0 = Don't)").SetValue(new Slider(0,0,5)));
            LC.AddItem(new MenuItem("LW", "Use W").SetValue(true));
            LC.AddItem(new MenuItem("LE", "Use E").SetValue(true));
            LC.AddItem(new MenuItem("LMinMana", "Min Mana (%)").SetValue(new Slider(80)));
            Menu.AddSubMenu(LC);

            var JC = new Menu("JungleClear", "JungleClear");
            JC.AddItem(new MenuItem("JQ", "Use Q").SetValue(true));
            JC.AddItem(new MenuItem("JW", "Use W").SetValue(true));
            JC.AddItem(new MenuItem("JE", "Use E").SetValue(true));
            Menu.AddSubMenu(JC);

            var Auto = new Menu("Auto", "Auto");
            Auto.AddItem(new MenuItem("AutoQ", "Auto Extended Q (Toggle)").SetValue(new KeyBind('G', KeyBindType.Toggle)));
            Auto.AddItem(new MenuItem("MinMana", "Min Mana (%)").SetValue(new Slider(80)));
            Menu.AddSubMenu(Auto);

            var Draw = new Menu("Draw", "Draw");
            Draw.AddItem(new MenuItem("Dind", "Draw Damage Incidator").SetValue(true));
            Draw.AddItem(new MenuItem("DEQ", "Draw Extended Q").SetValue(false));
            Draw.AddItem(new MenuItem("DQ", "Draw Q").SetValue(false));
            Draw.AddItem(new MenuItem("DW", "Draw W").SetValue(false));
            Draw.AddItem(new MenuItem("DE", "Draw E").SetValue(false));
            Menu.AddSubMenu(Draw);

            var killsteal = new Menu("killsteal", "Killsteal");
            killsteal.AddItem(new MenuItem("KillstealQ", "Killsteal Q").SetValue(true));
            Menu.AddSubMenu(killsteal);

            Menu.AddToMainMenu();
            */
        }
        

        private static void OnDoCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            var spellName = args.SData.Name;
            if (!sender.IsMe || !Orbwalking.IsAutoAttack(spellName)) return;

            if (args.Target is AIHeroClient)
            {
                var target = (Obj_AI_Base)args.Target;
                if (orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Combo && target.IsValid)
                {
                    Core.DelayAction(() => OnDoCastDelayed(args), Humanizer);
                }
                if (orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Mixed && target.IsValid)
                {
                    Core.DelayAction(() => OnDoCastDelayed(args), Humanizer);
                }
            }
            if (args.Target is Obj_AI_Minion)
            {
                if (orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.LaneClear && args.Target.IsValid)
                {
                    Core.DelayAction(() => OnDoCastDelayed(args), Humanizer);
                }
            }
        }
        private static void OnDoCastLc(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            var spellName = args.SData.Name;
            if (!sender.IsMe || !Orbwalking.IsAutoAttack(spellName)) return;

            if (args.Target is Obj_AI_Minion)
            {
                if (orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.LaneClear && args.Target.IsValid)
                {
                    Core.DelayAction(() => OnDoCastDelayedLc(args), Humanizer);
                }
            }
        }

        static void Killsteal()
        {
            if (KillstealQ && q.IsReady())
            {
                var targets = Orbwalking.HeroManager.Enemies.Where(x => x.IsValidTarget(q.Range) && !x.IsZombie);
                foreach (var target in targets)
                {
                    if (target.Health < player.GetSpellDamage(target, SpellSlot.Q) && (!target.HasBuff("kindrednodeathbuff") && !target.HasBuff("Undying Rage") && !target.HasBuff("JudicatorIntervention")))
                        q.Cast(target);
                }
            }
        }
        private static void OnDoCastDelayedLc(GameObjectProcessSpellCastEventArgs args)
        {
            aaPassive = false;
            if (args.Target is Obj_AI_Minion && args.Target.IsValid)
            {
                if (orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.LaneClear && player.ManaPercent > LMinMana)
                {
                    var minions = EntityManager.MinionsAndMonsters.GetLaneMinions(
                        EntityManager.UnitTeam.Enemy, Player.Instance.ServerPosition, Orbwalking.GetRealAutoAttackRange(player)).ToArray();
                    if (minions[0].IsValid && minions.Count() != 0)
                    {
                        if (!Lt) return;

                        if (e.IsReady() && !aaPassive && Le) e.Cast((Vector3)player.Position.Extend(Game.CursorPos, 70));
                        if (q.IsReady() && (!e.IsReady() || (e.IsReady() && !Le)) && Lq != 0 && !aaPassive)
                        {
                            var qMinions = EntityManager.MinionsAndMonsters.GetLaneMinions(
                                EntityManager.UnitTeam.Enemy, Player.Instance.ServerPosition, q.Range).ToArray();

                            var exminions = EntityManager.MinionsAndMonsters.GetLaneMinions(
                                EntityManager.UnitTeam.Enemy, Player.Instance.ServerPosition, q1.Range).ToArray();

                            foreach (var minion in qMinions)
                            {
                                var qHit = new Geometry.Polygon.Rectangle((Vector2)player.ServerPosition, player.Position.Extend(minion.ServerPosition, q1.Range), q1.Width);
                                if (exminions.Count(x => !qHit.IsOutside(x.Position.To2D())) >= Lq)
                                {
                                    q.Cast((Obj_AI_Base)minion);
                                    break;
                                }
                            }
                        }
                        if ((!e.IsReady() || (e.IsReady() && !Le)) && (!q.IsReady() || (q.IsReady() && Lq == 0)) && Lw && w.IsReady() && !aaPassive) w.Cast(minions[0].ServerPosition);
                    }
                }
            }
        }
        public static Vector2 Deviation(Vector2 point1, Vector2 point2, double angle)
        {
            angle *= Math.PI / 180.0;
            Vector2 temp = Vector2.Subtract(point2, point1);
            Vector2 result = new Vector2(0);
            result.X = (float)(temp.X * Math.Cos(angle) - temp.Y * Math.Sin(angle)) / 4;
            result.Y = (float)(temp.X * Math.Sin(angle) + temp.Y * Math.Cos(angle)) / 4;
            result = Vector2.Add(result, point1);
            return result;
        }
        private static void OnDoCastDelayed(GameObjectProcessSpellCastEventArgs args)
        {
            aaPassive = false;
            if (args.Target is AIHeroClient)
            {
                var target = (Obj_AI_Base)args.Target;
                if (orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Combo && target.IsValid)
                {
                    if (Youmuu.IsReady()) Youmuu.Cast();
                    if (e.IsReady() && !aaPassive && Ce == 0) e.Cast((Deviation(player.Position.To2D(), target.Position.To2D(), 65).To3D()));
                    if (e.IsReady() && !aaPassive && Ce == 1) e.Cast(Game.CursorPos);
                    if (e.IsReady() && !aaPassive && Ce == 2) e.Cast((Vector3)player.Position.Extend(target.Position, 50));
                    if (q.IsReady() && (!e.IsReady() || (e.IsReady() && Ce == 3)) && Cq && !aaPassive) q.Cast(target);
                    if ((!e.IsReady() || (e.IsReady() && Ce == 3)) && (!q.IsReady() || (q.IsReady() && !Cq)) && Cw && w.IsReady() && !aaPassive) w.Cast(target.Position);
                }
                if (orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Mixed && target.IsValid)
                {
                    if (player.ManaPercent < HhMinMana) return;

                    if (e.IsReady() && !aaPassive && He == 0) e.Cast((Deviation(player.Position.To2D(), target.Position.To2D(),65).To3D()));
                    if (e.IsReady() && !aaPassive && He == 1) e.Cast((Vector3)player.Position.Extend(Game.CursorPos, 50));
                    if (e.IsReady() && !aaPassive && He == 2) e.Cast((Vector3)player.Position.Extend(target.Position, 50));
                    if (q.IsReady() && (!e.IsReady() || (e.IsReady() && He == 3)) && Hq && !aaPassive) q.Cast(target);
                    if ((!e.IsReady() || (e.IsReady() && He == 3)) && (!q.IsReady() || (q.IsReady() && !Hq)) && Hw && w.IsReady() && !aaPassive) w.Cast(target.Position);
                }
            }
            if (args.Target is Obj_AI_Minion && args.Target.IsValid)
            {
                if (orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.LaneClear)
                {
                    var mobs = EntityManager.MinionsAndMonsters.GetJungleMonsters(Player.Instance.ServerPosition, Orbwalking.GetRealAutoAttackRange(player)).ToArray();
                    if (mobs[0].IsValid && mobs.Count() != 0)
                    {
                        if (e.IsReady() && !aaPassive && Je) e.Cast((Vector3)player.Position.Extend(Game.CursorPos, 70));
                        if (q.IsReady() && (!e.IsReady() || (e.IsReady() && !Je)) && Jq && !aaPassive) q.Cast(mobs[0]);
                        if ((!e.IsReady() || (e.IsReady() && !Je)) && (!q.IsReady() || (q.IsReady() && !Jq)) && Jw && w.IsReady() && !aaPassive) w.Cast(mobs[0].Position);
                    }
                }
            }
        }

        private static void Harass()
        {
            if (player.ManaPercent < HMinMana) return;

            if (q.IsReady() && Hexq)
            {
                var target = TargetSelector.GetTarget(q1.Range, DamageType.Physical);
                var minions = EntityManager.MinionsAndMonsters.GetLaneMinions(
                        EntityManager.UnitTeam.Enemy, Player.Instance.ServerPosition, q.Range).ToArray();
                foreach (var minion in (IEnumerable<Obj_AI_Base>)minions)
                {
                    var qHit = new Geometry.Polygon.Rectangle((Vector2)player.ServerPosition, player.ServerPosition.Extend(minion.Position, q1.Range),q1.Width);
                    var qPred = q1.GetPrediction(target);
                    if (!qHit.IsOutside(qPred.UnitPosition.To2D()) && qPred.HitChance == HitChance.High)
                    {
                        q.Cast(minion);
                        break;
                    }
                }
            }
        }
        static void LaneClear()
        {
            if (player.ManaPercent < LMinMana) return;

            if (q.IsReady() && Lhq)
            {
                var extarget = TargetSelector.GetTarget(q1.Range, DamageType.Physical);
                var minions = EntityManager.MinionsAndMonsters.GetLaneMinions(
                        EntityManager.UnitTeam.Enemy, Player.Instance.ServerPosition, q.Range).ToArray();
                foreach (var minion in (IEnumerable<Obj_AI_Base>)minions)
                {
                    var qHit = new Geometry.Polygon.Rectangle((Vector2)player.Position, player.Position.Extend(minion.ServerPosition, q1.Range), q1.Width);
                    var qPred = q1.GetPrediction(extarget);
                    if (!qHit.IsOutside(qPred.UnitPosition.To2D()) && qPred.HitChance == HitChance.High)
                    {
                        q.Cast(minion);
                        break;
                    }
                }
            }
        }
        static void AutoUseQ()
        {
            if (q.IsReady() && AutoQ && player.ManaPercent > MinMana)
            {
                var extarget = TargetSelector.GetTarget(q1.Range, DamageType.Physical);
                var minions = EntityManager.MinionsAndMonsters.GetLaneMinions(
                        EntityManager.UnitTeam.Enemy, Player.Instance.ServerPosition, q.Range).ToArray();
                foreach (var minion in (IEnumerable<Obj_AI_Base>)minions)
                {
                    var qHit = new Geometry.Polygon.Rectangle((Vector2)player.Position, player.Position.Extend(minion.ServerPosition, q1.Range), q1.Width);
                    var qPred = q1.GetPrediction(extarget);
                    if (!qHit.IsOutside(qPred.UnitPosition.To2D()) && qPred.HitChance == HitChance.High)
                    {
                        q.Cast(minion);
                        break;
                    }
                }
            }
        }

        static void UseRTarget()
        {
            var target = TargetSelector.GetTarget(r.Range, DamageType.Physical);
            if (ForceR && r.IsReady() && target.IsValid && !player.HasBuff("LucianR")) r.Cast(target.ServerPosition);
        }
        static void Game_OnUpdate(EventArgs args)
        {
            AutoUseQ();

            if (ForceR) UseRTarget();
            Killsteal();
            if (orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Mixed) Harass();
            if (orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.LaneClear) LaneClear();
        }
        static void Spellbook_OnCastSpell(Spellbook sender, SpellbookCastSpellEventArgs args)
        {
            if (args.Slot == SpellSlot.Q || args.Slot == SpellSlot.W || args.Slot == SpellSlot.E) aaPassive = true;
            if (args.Slot == SpellSlot.E) Orbwalking.ResetAutoAttackTimer();
            if (args.Slot == SpellSlot.R) Youmuu.Cast();
        }

        static float GetComboDamage(Obj_AI_Base enemy)
        {
            if (enemy != null)
            {
                float damage = 0;
                if (e.IsReady()) damage = damage + (float)player.GetAutoAttackDamage(enemy) * 2;
                if (w.IsReady()) damage = damage + player.GetSpellDamage(enemy, SpellSlot.W) + (float)player.GetAutoAttackDamage(enemy);
                if (q.IsReady())
                {
                    damage = damage + player.GetSpellDamage(enemy, SpellSlot.Q) + (float)player.GetAutoAttackDamage(enemy);
                }
                damage = damage + (float)player.GetAutoAttackDamage(enemy);

                return damage;
            }
            return 0;
        }

        static void OnDraw(EventArgs args)
        {
            if (Deq && q.IsLearned && q.IsReady())
            {
                Circle.Draw(Color.LimeGreen, q1.Range, Player.Instance.ServerPosition);
            }
            if (Deq && q.IsLearned && !q.IsReady())
            {
                Circle.Draw(Color.IndianRed, q1.Range, Player.Instance.ServerPosition);
            }

            if (Dq && q.IsLearned && q.IsReady())
            {
                Circle.Draw(Color.LimeGreen, q.Range, Player.Instance.ServerPosition);
            }
            if (Dq && q.IsLearned && !q.IsReady())
            {
                Circle.Draw(Color.IndianRed, q.Range, Player.Instance.ServerPosition);
            }

            if (Dw && w.IsLearned && w.IsReady())
            {
                Circle.Draw(Color.LimeGreen, w.Range, Player.Instance.ServerPosition);
            }
            if (Dw && w.IsLearned && !w.IsReady())
            {
                Circle.Draw(Color.IndianRed, w.Range, Player.Instance.ServerPosition);
            }

            if (De && e.IsLearned && e.IsReady())
            {
                Circle.Draw(Color.LimeGreen, e.Range, Player.Instance.ServerPosition);
            }
            if (De && e.IsLearned && !e.IsReady())
            {
                Circle.Draw(Color.IndianRed, e.Range, Player.Instance.ServerPosition);
            }
        }
        static void Drawing_OnEndScene(EventArgs args)
        {
            if (Dind)
            {
                foreach (
                    var enemy in
                        ObjectManager.Get<AIHeroClient>()
                            .Where(ene => ene.IsValidTarget() && !ene.IsZombie))
                {
                    indicator.unit = enemy;
                    indicator.drawDmg(GetComboDamage(enemy), new ColorBGRA(255, 204, 0, 160));

                }
            }
        }
    }
}