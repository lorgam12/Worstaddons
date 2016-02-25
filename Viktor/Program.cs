using System;
using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using SharpDX;


public class Program
    {
        public const string ChampName = "Viktor";
        private static readonly AIHeroClient player = ObjectManager.Player;
    public static List<Spell.SpellBase> SpellList = new List<Spell.SpellBase>();

    // Spells

    private static bool UseQCombo
    {
        get { return ComboMenu["comboUseQ"].Cast<CheckBox>().CurrentValue; }
    }
    private static bool UseWCombo
    {
        get { return ComboMenu["comboUseW"].Cast<CheckBox>().CurrentValue; }
    }
    private static bool UseECombo
    {
        get { return ComboMenu["comboUseE"].Cast<CheckBox>().CurrentValue; }
    }
    private static bool UseRCombo
    {
        get { return ComboMenu["comboUseR"].Cast<CheckBox>().CurrentValue; }
    }
    private static int RTicks
    {
        get { return ComboMenu["rTicks"].Cast<Slider>().CurrentValue; }
    }
    private static Spell.Skillshot W, E, R;
        private static Spell.Targeted Q;
        private static readonly int maxRangeE = 1225;
        private static readonly int lengthE = 700;
        private static readonly int speedE = 1200;
        private static readonly int rangeE = 525;
        private static int lasttick;
        private static Vector3 gapCloserPos;
        private static bool AttacksEnabled
        {
            get
            {
                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo) || Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass))
                {
                    return (!Q.IsReady() || player.Mana < Q.Handle.SData.Mana)
                            && (!E.IsReady() || player.Mana < E.Handle.SData.Mana);
                }

                return true;
            }
        }
    // Menu
    public static Menu menuIni;
    private static object gapcloser;

    public static Menu ComboMenu { get; private set; }
    public static Menu HarassMenu { get; private set; }
    public static Menu LaneMenu { get; private set; }
    public static Menu LhMenu { get; private set; }
    public static Menu MiscMenu { get; private set; }
    public static Menu DrawMenu { get; private set; }

    // Menu links

        public static void Main(string[] args)
        {
            // Register events
            Loading.OnLoadingComplete += Game_OnGameLoad;

        }

        private static void OrbwalkingOnBeforeAttack(AttackableUnit target, Orbwalker.PreAttackArgs args)
        {
            if (target.Type == GameObjectType.AIHeroClient)
            {
                args.Process = AttacksEnabled;
            }
            else
            {
                args.Process = true;
            }
        }

        private static void Game_OnGameLoad(EventArgs args)
        {
            // Champ validation
            if (player.ChampionName != ChampName)
            {
                return;
            }
            // Define spells

            Q = new Spell.Targeted(SpellSlot.Q, 600);
            W = new Spell.Skillshot(SpellSlot.W, 700, SkillShotType.Circular, 500, int.MaxValue, 300)
                    {
                        AllowedCollisionCount = int.MaxValue 
                    };
            E = new Spell.Skillshot(SpellSlot.E, 525, SkillShotType.Linear, 250, int.MaxValue, 100)
                    {
                        AllowedCollisionCount = int.MaxValue 
                    };
            R = new Spell.Skillshot(SpellSlot.R, 700, SkillShotType.Circular, 250, int.MaxValue, 450)
                    {
                        AllowedCollisionCount = int.MaxValue 
                    };

        // Create menu
        CreateMenu();

        // Register events
        Game.OnUpdate += Game_OnGameUpdate;
        /*
            Drawing.OnDraw += Drawing_OnDraw;
            */
            Gapcloser.OnGapcloser += Gapcloser_OnGapcloser;
            Obj_AI_Base.OnProcessSpellCast += Obj_AI_Base_OnProcessSpellCast;
        }
    
        private static void Game_OnGameUpdate(EventArgs args)
        {
            // Combo
            if (Orbwalker.ActiveModesFlags == Orbwalker.ActiveModes.Combo)
                OnCombo();
            // Harass�
            if (Orbwalker.ActiveModesFlags == Orbwalker.ActiveModes.Harass)
                OnHarass();
            // WaveClear
            if (Orbwalker.ActiveModesFlags == Orbwalker.ActiveModes.LaneClear)
                OnWaveClear();

            if (Orbwalker.ActiveModesFlags == Orbwalker.ActiveModes.JungleClear)
                OnJungleClear();

            // Ultimate follow
            if (R.Name != "ViktorChaosStorm" && ComboMenu.Get<CheckBox>("AutoFollowR").CurrentValue && Environment.TickCount - lasttick > 0)
            {
                var stormT = TargetSelector.GetTarget(1100, DamageType.Magical);
                if (stormT != null)
                {
                    R.Cast(stormT.ServerPosition);
                    lasttick = Environment.TickCount + 500;
                }
            }
            AutoW();
        }
        private static bool KillableWithAA(Obj_AI_Base target)
        {
            var qaaDmg = new Double[] { 20, 25, 30, 35, 40, 45, 50, 55, 60, 70, 80, 90, 110, 130, 150, 170, 190, 210 };
            if (player.HasBuff("viktorpowertransferreturn") && Orbwalker.CanAutoAttack
                && (player.GetSpellDamage(
                    target,
                    (SpellSlot)DamageType.Magical, (DamageLibrary.SpellStages)(qaaDmg[player.Level >= 18 ? 18 - 1 : player.Level - 1] + (player.TotalMagicalDamage * .5)
                                                                 + player.TotalAttackDamage)) > target.Health))
            {
                Console.WriteLine("killable with aa");
                return true;
            }
            return false;
        }
        private static void OnCombo()
        {
            bool useQ = UseQCombo && Q.IsReady();
            bool useW = UseWCombo && W.IsReady();
            bool useE = UseECombo && E.IsReady();
            bool useR = UseRCombo && R.IsReady();
            bool killpriority = ComboMenu.Get < CheckBox >("spPriority").CurrentValue && R.IsReady();
            bool rKillSteal = ComboMenu.Get<CheckBox>("rLastHit").CurrentValue;
            var etarget = TargetSelector.GetTarget(maxRangeE, DamageType.Magical);
            var qtarget = TargetSelector.GetTarget(Q.Range, DamageType.Magical);
            var rTarget = TargetSelector.GetTarget(W.Range, DamageType.Magical);
            if (killpriority && qtarget != null & etarget != null && etarget != qtarget
                && ((etarget.Health > TotalDmg(etarget, false, true, false, false))
                    || (etarget.Health > TotalDmg(etarget, false, true, true, false) && etarget == rTarget))
                && qtarget.Health < TotalDmg(qtarget, true, true, false, false))
            {
                etarget = qtarget;
            }

            if (rTarget != null && rKillSteal && useR)
            {
                if (TotalDmg(rTarget, true, true, false, false) < rTarget.Health && TotalDmg(rTarget, true, true, true, true) > rTarget.Health)
                {
                    R.Cast(rTarget.ServerPosition);
                }
            }


            if (useE)
            {
                if (etarget != null)
                    PredictCastE(etarget);
            }
            if (useQ)
            {
                if (qtarget != null)
                {
                    Q.Cast(qtarget);
                }
            }
            if (useW)
            {
                var t = TargetSelector.GetTarget(W.Range, DamageType.Magical);

                if (t.Path.Count() < 2)
                {
                    if (t.HasBuffOfType(BuffType.Slow))
                    {
                        if (W.GetPrediction(t).HitChance >= HitChance.High)
                        {
                            W.Cast(t.ServerPosition);
                        }
                    }
                    if (t.CountEnemiesInRange(250) > 2)
                    {
                        var predW = W.GetPrediction(t);
                        if (predW.HitChance >= HitChance.High)
                        {
                            W.Cast(t.ServerPosition);
                        }
                    }
                }
            }
            if (useR && R.Name == "ViktorChaosStorm" && player.CanCast && !player.Spellbook.IsCastingSpell)
            {
                var target = TargetSelector.GetTarget(R.Range, DamageType.Magical);
                if (target != null && target.IsEnemy && !target.IsZombie && target.CountEnemiesInRange(R.Width) >= ComboMenu.Get<Slider>("HitR").CurrentValue && R.Name == "ViktorChaosStorm")
                {
                    var prediction = E.GetPrediction(target);
                    var predictDmg = PredictDamage(target);
                    //Chat.Print("Target Health: " + target.Health + "Predict Dmg: " + predictDmg);
                    if (target.HealthPercent > 5 && UseRCombo && prediction.HitChance >= HitChance.High)
                    {
                        if (target.Health <= predictDmg)
                        {
                            R.Cast(target);
                        }
                    }
                    else if (target.HealthPercent > 5 && !UseRCombo && prediction.HitChance >= HitChance.High)
                    {
                        R.Cast(target);
                    }
                }
            }

        }

        private static void OnHarass()
        {
            // Mana check
            if (player.ManaPercent < HarassMenu.Get<Slider>("harassMana").CurrentValue)
            {
                return;
            }
            bool useE = HarassMenu.Get<CheckBox>("harassUseE").CurrentValue && E.IsReady();
            bool useQ = HarassMenu.Get < CheckBox >("harassUseQ").CurrentValue && Q.IsReady();
            if (useQ)
            {
                var qtarget = TargetSelector.GetTarget(Q.Range, DamageType.Magical);
                if (qtarget != null)
                {
                    Q.Cast(qtarget);
                }
            }
            if (useE)
            {
                var target = TargetSelector.GetTarget(maxRangeE, DamageType.Magical);

                if (target != null)
                {
                    PredictCastE(target);
                }
            }
        }
    
    private static void OnWaveClear()
        {
            // Mana check
        if (player.ManaPercent < LaneMenu.Get<Slider>("waveMana").CurrentValue)
        {
            return;
        }

        bool useQ = LaneMenu.Get < CheckBox >("waveUseQ").CurrentValue && Q.IsReady();
            bool useE = LaneMenu.Get < CheckBox >("waveUseE").CurrentValue && E.IsReady();

            if (useQ)
            {

            var minion =
                EntityManager.MinionsAndMonsters.GetLaneMinions()
                    .OrderBy(m => m.Health)
                    .FirstOrDefault(
                        mi =>
                            mi.IsValidTarget(Q.Range) &&
                            Prediction.Health.GetPrediction(mi, Q.CastDelay) <= player.GetSpellDamage(mi, SpellSlot.Q) &&
                            Prediction.Health.GetPrediction(mi, Q.CastDelay) > 20);
            if (minion != null && !minion.IsInRange(Player.Instance, Player.Instance.GetAutoAttackRange()))
            {
                        QLastHit(minion);
                    }
        }
        if (useE)
        {
            PredictCastMinionE();
        }
        }

    private static void QLastHit(Obj_AI_Base minion)
    {
        bool castQ = (LaneMenu.Get<CheckBox>("waveUseQ").CurrentValue && Q.IsReady());
        if (castQ)
            {
                Q.Cast(minion);
            }
    }

    private static void OnJungleClear()
        {
            // Mana check
        if (player.ManaPercent < LaneMenu.Get<Slider>("waveMana").CurrentValue)
        {
            return;
        }

        bool useQ = LaneMenu.Get<CheckBox>("waveUseQ").CurrentValue && Q.IsReady();
            bool useE = LaneMenu.Get<CheckBox>("waveUseE").CurrentValue && E.IsReady();

            if (useQ)
            {
                foreach (var minion in EntityManager.MinionsAndMonsters.GetJungleMonsters())
                {
                    Q.Cast(minion);
                }
            }

        if (useE)
        {
            var junglemob = EntityManager.MinionsAndMonsters.Get(
                EntityManager.MinionsAndMonsters.EntityType.Monster,
                EntityManager.UnitTeam.Both,
                player.ServerPosition,
                maxRangeE,
                false);

            var objAiMinions = junglemob as IList<Obj_AI_Minion> ?? junglemob.ToList();
            foreach (var minion in objAiMinions)
            {
                {
                    var loc = EntityManager.MinionsAndMonsters.GetLineFarmLocation(objAiMinions, E.Width, maxRangeE);
                    Player.CastSpell(SpellSlot.E, loc.CastPosition, minion.ServerPosition);
                }
            }
        }
        }
    
    private static bool PredictCastMinionE(int requiredHitNumber = -1)
    {
        int hitNum = 0;
        Vector2 startPos = new Vector2(0, 0);
        Vector2 endPos = new Vector2(0, 0);
        var minions = EntityManager.MinionsAndMonsters.Get(
            EntityManager.MinionsAndMonsters.EntityType.Minion,
            EntityManager.UnitTeam.Enemy,
            player.Position,
            maxRangeE,
            false);


        foreach (var minion in minions)
        {
            var farmLocation = EntityManager.MinionsAndMonsters.GetLineFarmLocation(minions, E.Width, maxRangeE);
            if (farmLocation.HitNumber > hitNum)
        {
            hitNum = farmLocation.HitNumber;
            startPos = minion.ServerPosition.To2D();
            endPos = farmLocation.CastPosition.To2D();
        }
    }

    if (startPos.X != 0 && startPos.Y != 0)
            return PredictCastMinionE(startPos, requiredHitNumber);
        return false;
    }

    private static bool PredictCastMinionE(Vector2 fromPosition, int requiredHitNumber = 1)
    {
        var minions = EntityManager.MinionsAndMonsters.Get(
            EntityManager.MinionsAndMonsters.EntityType.Minion,
            EntityManager.UnitTeam.Enemy,
            player.Position,
            maxRangeE,
            false);


        foreach (var minion in minions)
        {
            var farmLocation = EntityManager.MinionsAndMonsters.GetLineFarmLocation(minions, E.Width, maxRangeE);

            if (farmLocation.HitNumber >= requiredHitNumber)
            {
                CastE(fromPosition, farmLocation.CastPosition.To2D());
                return true;
            }
        }
        return false;
    }


    private static void CastE(Vector3 source, Vector3 destination)
    {
        Player.CastSpell(SpellSlot.E, source, destination);
    }

        private static void CastE(Vector2 source, Vector2 destination)
    {
        Player.CastSpell(SpellSlot.E, (Vector3)source, true);
    }

        private static void Obj_AI_Base_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
    {
        if (sender.IsMe && args.SData.Name.ToLower().Contains("viktorpowertransferreturn"))
            Core.DelayAction(Orbwalker.ResetAutoAttack, 230);

        // Console.WriteLine("Enemy casted skill: " + args.SData.Name);
        var start = InterruptSkills(args.SData.Name);
        if (start && sender.IsEnemy && MiscMenu.Get<CheckBox>("miscGapcloser").CurrentValue && W.IsReady())
        {

            if (args.End.Distance(player.Position) <= 100)
            {
                //Console.WriteLine("Self Cast. Skill: " + args.SData.Name);
                W.Cast(player);
            }

            else if (MiscMenu.Get<CheckBox>("wInterrupt").CurrentValue)
            {
                // Console.WriteLine("Enemy Cast. Skill: " + args.SData.Name);
                W.Cast(sender.ServerPosition);
            }
        }
    }

    private static void Gapcloser_OnGapcloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs e)
    {
        if (sender.IsAlly || !MiscMenu.Get<CheckBox>("miscGapcloser").CurrentValue) return;
        if (e.End.Distance(sender) <= 170)
            W.Cast(sender);
    }

    private static void AutoW()
        {
            if (!W.IsReady() || !MiscMenu.Get<CheckBox>("autoW").CurrentValue)
                return;
            Obj_AI_Base tPanth = ObjectManager.Get<Obj_AI_Base>().FirstOrDefault(x => x.IsEnemy && W.IsInRange(x) && (x.HasBuff("teleport_target") || x.HasBuff("Pantheon_GrandSkyfall_Jump")));
            if (tPanth != null)
            {
                if (W.Cast(tPanth.ServerPosition))
                    return;
            }
        /*
            if (Target.HasBuff("rocketgrab2"))
            {
                var t = HeroManager.Allies.Find(h => h.BaseSkinName.ToLower() == "blitzcrank" && h.Distance((AttackableUnit)player) < W.Range);
                if (t != null)
                {
                    if (W.Cast(t) == Spell.CastStates.SuccessfullyCasted)
                        return;
                }
            }
            */
        var wtarget = TargetSelector.GetTarget(W.Range, DamageType.Magical);
        if (Player.HasBuffOfType(BuffType.Stun) || Player.HasBuffOfType(BuffType.Snare) ||
                         Player.HasBuffOfType(BuffType.Charm) || Player.HasBuffOfType(BuffType.Fear) ||
                         Player.HasBuffOfType(BuffType.Taunt) || Player.HasBuffOfType(BuffType.Suppression) ||
                         Player.HasBuffOfType(BuffType.Stun))
                {
                    if (W.Cast(wtarget))
                        return;
                }
                if (W.GetPrediction(wtarget).HitChance == HitChance.Immobile)
                {
                    if (W.Cast(wtarget))
                    {
                        return;
                    }
                }
            }
    /*
        private static void Drawing_OnDraw(EventArgs args)
        {
            // All circles
            foreach (var circle in circleLinks.Values.Select(link => link.Value))
            {
                if (circle.Active)
                    Render.Circle.DrawCircle(player.Position, circle.Radius, circle.Color);
            }
        }
        */
        
        private static float TotalDmg(Obj_AI_Base enemy, bool useQ, bool useE, bool useR, bool qRange)
        {
            var qaaDmg = new Double[] { 20, 25, 30, 35, 40, 45, 50, 55, 60, 70, 80, 90, 110, 130, 150, 170, 190, 210 };
            var damage = 0d;
            var rTicks = RTicks;
            bool inQRange = ((qRange && player.IsInAutoAttackRange(enemy)) || qRange == false);
            //Base Q damage
            if (useQ && Q.IsReady() && inQRange)
            {
                damage += player.GetSpellDamage(enemy, SpellSlot.Q);
                damage += player.GetSpellDamage(enemy, (SpellSlot)DamageType.Magical, (DamageLibrary.SpellStages)(qaaDmg[player.Level >= 18 ? 18 - 1 : player.Level - 1] + (player.TotalMagicalDamage * .5) + player.TotalAttackDamage));
            }

            // Q damage on AA
            if (useQ && !Q.IsReady() && player.HasBuff("viktorpowertransferreturn") && inQRange)
            {
                damage += player.GetSpellDamage(enemy, (SpellSlot)DamageType.Magical, (DamageLibrary.SpellStages)(qaaDmg[player.Level >= 18 ? 18 - 1 : player.Level - 1] +
                                                                                                           (player.TotalMagicalDamage * .5) + player.TotalAttackDamage));
            }

            //E damage
            if (useE && E.IsReady())
            {
                if (player.HasBuff("viktoreaug") || player.HasBuff("viktorqeaug") || player.HasBuff("viktorqweaug"))
                    damage += player.GetSpellDamage(enemy, SpellSlot.E);
                else
                    damage += player.GetSpellDamage(enemy, SpellSlot.E, 0);
            }

            //R damage + 2 ticks
            if (useR && R.Level > 0 && R.IsReady() && R.Name == "ViktorChaosStorm")
            {
                damage += player.GetSpellDamage(enemy, SpellSlot.R) * rTicks;
                damage += player.GetSpellDamage(enemy, SpellSlot.R);
            }

            // Ludens Echo damage
            if (player.HasItem(3285))
                damage += player.GetSpellDamage(enemy, (SpellSlot)DamageType.Magical, (DamageLibrary.SpellStages)(100 + player.FlatMagicDamageMod * 0.1));

            //sheen damage
            if (player.HasItem(3057))
                damage += player.GetSpellDamage(enemy, (SpellSlot)DamageType.Physical, (DamageLibrary.SpellStages)(0.5 * player.BaseAttackDamage));

            //lich bane dmg
            if (player.HasItem(3100))
                damage += player.GetSpellDamage(enemy, (SpellSlot)DamageType.Magical, (DamageLibrary.SpellStages)(0.5 * player.FlatMagicDamageMod + 0.75 * player.BaseAttackDamage));

            return (float)damage;
        }
        private static float GetComboDamage(Obj_AI_Base enemy)
        {

            return TotalDmg(enemy, true, true, true, false);
        }

        private static void CreateMenu()
        {

        menuIni = MainMenu.AddMenu("Viktor", "Viktor");
        menuIni.AddGroupLabel("Welcome to the worst Viktor addon!");

        ComboMenu = menuIni.AddSubMenu("Combo");
        ComboMenu.Add("comboUseQ", new CheckBox("Use Q"));
        ComboMenu.Add("comboUseW", new CheckBox("Use W"));
        ComboMenu.Add("comboUseE", new CheckBox("Use E"));
        ComboMenu.Add("comboUseR", new CheckBox("Use R"));
        ComboMenu.Add("HitR", new Slider("Ultimate to hit", 3, 1, 5));
        ComboMenu.AddSeparator();
        ComboMenu.Add("rLastHit", new CheckBox("1 target ulti"));
        ComboMenu.Add("AutoFollowR", new CheckBox("Auto Follow R"));
        ComboMenu.Add("rTicks", new Slider("Ultimate ticks to count", 2, 1, 14));

        HarassMenu = menuIni.AddSubMenu("Harass");
        HarassMenu.Add("harassUseQ", new CheckBox("Use Q"));
        HarassMenu.Add("harassUseE", new CheckBox("Use E"));
        HarassMenu.Add("harassMana", new Slider("Mana usage in percent (%)", 30, 0, 100));

        LaneMenu = menuIni.AddSubMenu("Farm");
        LaneMenu.Add("waveUseQ", new CheckBox("Use Q"));
        LaneMenu.Add("waveUseE", new CheckBox("Use E"));
        LaneMenu.Add("waveMana", new Slider("Mana usage in percent (%)", 30, 0, 100));

        LhMenu = menuIni.AddSubMenu("LastHit");
        LhMenu.Add("waveUseQLH", new CheckBox("Use Q"));

        MiscMenu = menuIni.AddSubMenu("Misc");
        MiscMenu.Add("spPriority", new CheckBox("Prioritize kill over dmg"));
        MiscMenu.Add("rInterrupt", new CheckBox("Use R to interrupt dangerous spells"));
        MiscMenu.Add("wInterrupt", new CheckBox("Use W to interrupt dangerous spells"));
        MiscMenu.Add("miscGapcloser", new CheckBox("Use W against gapclosers"));
        MiscMenu.Add("autoW", new CheckBox("Use W to continue CC"));

        DrawMenu = menuIni.AddSubMenu("Draw");
        DrawMenu.Add("Enabled", new CheckBox("Enabled"));
        DrawMenu.Add("drawRangeQ", new CheckBox("Q range"));
        DrawMenu.Add("drawRangeW", new CheckBox("W rangE"));
        DrawMenu.Add("drawRangeE", new CheckBox("E range"));
        DrawMenu.Add("drawRangeEMax", new CheckBox("E Max range"));
        DrawMenu.Add("drawRangeR", new CheckBox("R range"));

        }
        private static float PredictDamage(AIHeroClient t)
        {
            float dmg = 0f;
            if (UseQCombo && Q.IsReady() && player.IsInAutoAttackRange(t))
            {
                dmg += player.GetSpellDamage(t, SpellSlot.Q);
                dmg += (float)CalculateAADmg();
            }

            if (UseECombo && E.IsReady() && player.ServerPosition.Distance(t.ServerPosition) <= maxRangeE)
            {
                dmg += player.GetSpellDamage(t, SpellSlot.E);
            }

            if (UseRCombo && R.IsReady() && R.IsInRange(t))
            {
                dmg += player.GetSpellDamage(t, SpellSlot.R);
                dmg += (float)CalculateRTickDmg(t, RTicks);
            }
            return dmg;
        }

        private static double CalculateAADmg()
        {
            double[] AAdmg = new double[] { 20, 25, 30, 35, 40, 45, 50, 55, 60, 70, 80, 90, 110, 130, 150, 170, 190, 210 };

            return (double)AAdmg[player.Level - 1] + player.TotalMagicalDamage * 0.5 + player.TotalAttackDamage;
        }

        private static double CalculateRTickDmg(AIHeroClient t, int ticks)
        {
            if (R.Level == 0) return 0;
            double dmg = 0;
            if (R.Level == 1)
                dmg += (15 + player.TotalMagicalDamage * 0.10) * ticks;
            else if (R.Level == 2)
                dmg += (30 + player.TotalMagicalDamage * 0.10) * ticks;
            else if (R.Level == 3) //No point for that,  just testing if that was the error..
                dmg += (45 + player.TotalMagicalDamage * 0.10) * ticks;

            return dmg;
        }

    private static bool InterruptSkills(string skillName)
    {
        switch (skillName)
        {
            case "BandageToss":
                return 
                    true;
            case "KhazixE":
                return
                    true;
            case "LissandraE":
                return
                    true;
            case "ThreshQ":
                return
                    true;
            case "MissFortuneBulletTime":
                return
                    true;
            case "RocketGrab":
                return
                    true;
        }
        return false;
    }

    private static void PredictCastE(AIHeroClient target)
    {
        // Helpers
        bool inRange = Vector2.DistanceSquared(target.ServerPosition.To2D(), player.Position.To2D()) < E.Range * E.Range;
        PredictionResult prediction;
        bool spellCasted = false;

        // Positions
        Vector3 pos1;

        // Champs
        var nearChamps = (from champ in ObjectManager.Get<AIHeroClient>() where champ.IsValidTarget(maxRangeE) && target != champ select champ).ToList();
        var innerChamps = new List<AIHeroClient>();
        var outerChamps = new List<AIHeroClient>();
        foreach (var champ in nearChamps)
        {
            if (Vector2.DistanceSquared(champ.ServerPosition.To2D(), player.Position.To2D()) < E.Range * E.Range)
                innerChamps.Add(champ);
            else
                outerChamps.Add(champ);
        }

        // Minions
        var nearMinions =
            EntityManager.MinionsAndMonsters.EnemyMinions.Where(x => x.Distance(Player.Instance) <= maxRangeE);
        var innerMinions = new List<Obj_AI_Base>();
        var outerMinions = new List<Obj_AI_Base>();
        foreach (var minion in nearMinions)
        {
            if (Vector2.DistanceSquared(minion.ServerPosition.To2D(), player.Position.To2D()) < E.Range * E.Range)
                innerMinions.Add(minion);
            else
                outerMinions.Add(minion);
        }

        // Main target in close range
        if (inRange)
        {
            // Get prediction reduced speed, adjusted sourcePosition
            E.Speed = (int)(speedE * 0.9f);
            E.SourcePosition = target.ServerPosition + (Vector3.Normalize(player.Position - target.ServerPosition) * (lengthE * 0.1f));
            prediction = E.GetPrediction(target);
            E.RangeCheckSource = player.Position;

            // Prediction in range, go on
            if (prediction.CastPosition.Distance(player.Position) < E.Range)
                pos1 = prediction.CastPosition;
            // Prediction not in range, use exact position
            else
            {
                pos1 = target.ServerPosition;
                E.Speed = speedE;
            }

            // Set new sourcePosition
            E.SourcePosition = pos1;
            E.RangeCheckSource = pos1;

            // Set new range
            E.Range = (uint)lengthE;

            // Get next target
            if (nearChamps.Count > 0)
            {
                // Get best champion around
                var closeToPrediction = new List<AIHeroClient>();
                foreach (var enemy in nearChamps)
                {
                    // Get prediction
                    prediction = E.GetPrediction(enemy);
                    // Validate target
                    if (prediction.HitChance >= HitChance.High && Vector2.DistanceSquared(pos1.To2D(), prediction.CastPosition.To2D()) < (E.Range * E.Range) * 0.8)
                        closeToPrediction.Add(enemy);
                }

                // Champ found
                if (closeToPrediction.Count > 0)
                {
                    // Sort table by health DEC
                    if (closeToPrediction.Count > 1)
                        closeToPrediction.Sort((enemy1, enemy2) => enemy2.Health.CompareTo(enemy1.Health));

                    // Set destination
                    prediction = E.GetPrediction(closeToPrediction[0]);
                    var pos2 = prediction.CastPosition;

                    // Cast spell
                    CastE(pos1, pos2);
                    spellCasted = true;
                }
            }

            // Spell not casted
            if (!spellCasted)
                // Try casting on minion
                if (!PredictCastMinionE(pos1.To2D()))
                    // Cast it directly
                    CastE(pos1, E.GetPrediction(target).CastPosition);

            // Reset spell
            E.Speed = speedE;
            E.Range = (uint)rangeE;
            E.SourcePosition = player.Position;
            E.RangeCheckSource = player.Position;
        }

        // Main target in extended range
        else
        {
            // Radius of the start point to search enemies in
            float startPointRadius = 150;

            // Get initial start point at the border of cast radius
            Vector3 startPoint = player.Position + Vector3.Normalize(target.ServerPosition - player.Position) * rangeE;

            // Potential start from postitions
            var targets = (from champ in nearChamps where Vector2.DistanceSquared(champ.ServerPosition.To2D(), startPoint.To2D()) < startPointRadius * startPointRadius && Vector2.DistanceSquared(player.Position.To2D(), champ.ServerPosition.To2D()) < rangeE * rangeE select champ).ToList();
            if (targets.Count > 0)
            {
                // Sort table by health DEC
                if (targets.Count > 1)
                    targets.Sort((enemy1, enemy2) => enemy2.Health.CompareTo(enemy1.Health));

                // Set target
                pos1 = targets[0].ServerPosition;
            }
            else
            {
                var minionTargets = (nearMinions.Where(
                    minion =>
                    Vector2.DistanceSquared(minion.ServerPosition.To2D(), startPoint.To2D())
                    < startPointRadius * startPointRadius
                    && Vector2.DistanceSquared(player.Position.To2D(), minion.ServerPosition.To2D()) < rangeE * rangeE)).ToList();
                if (minionTargets.Count > 0)
                {
                    // Sort table by health DEC
                    if (minionTargets.Count > 1)
                        minionTargets.Sort((enemy1, enemy2) => enemy2.Health.CompareTo(enemy1.Health));

                    // Set target
                    pos1 = minionTargets[0].ServerPosition;
                }
                else
                    // Just the regular, calculated start pos
                    pos1 = startPoint;
            }

            // Predict target position
            E.SourcePosition = pos1;
            E.Range = (uint)lengthE;
            E.RangeCheckSource = pos1;
            prediction = E.GetPrediction(target);

            // Cast the E
            if (prediction.HitChance >= HitChance.High)
                CastE(pos1, prediction.CastPosition);

            // Reset spell
            E.Range = (uint)rangeE;
            E.SourcePosition = player.Position;
            E.RangeCheckSource = player.Position;
        }

    }
}