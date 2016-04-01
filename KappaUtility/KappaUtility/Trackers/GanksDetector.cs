namespace KappaUtility.Trackers
{
    using System.Linq;

    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Menu.Values;

    using Color = System.Drawing.Color;

    internal class GanksDetector
    {
        private static readonly AIHeroClient _Hero;

        private static float DrawDuration;

        private static float EnterTime;

        private static int LastPing;

        public static Menu GankMenu { get; private set; }

        internal static void OnLoad()
        {
            GankMenu = Load.UtliMenu.AddSubMenu("GanksDetector");
            GankMenu.AddGroupLabel("Ganks Detector");
            GankMenu.Add("enable", new CheckBox("Enable", false));
            GankMenu.Add("ping", new CheckBox("Ping On Champion (Local)", false));
            GankMenu.Add("cd", new Slider("Detect CoolDown (Sec)", 15));
            GankMenu.Add("range", new Slider("Detect Range", 2500, 500, 6500));
            GankMenu.AddSeparator();
            GankMenu.AddGroupLabel("Detect Ganks From -");
            GankMenu.AddGroupLabel("Allies:");
            foreach (var hero in ObjectManager.Get<AIHeroClient>())
            {
                var cb = new CheckBox(hero.BaseSkinName) { CurrentValue = true };
                if (hero.Team == Player.Instance.Team && !hero.IsMe)
                {
                    GankMenu.Add("AllyGank" + hero.BaseSkinName, cb);
                }
            }

            GankMenu.AddGroupLabel("Enemies:");
            foreach (var hero in ObjectManager.Get<AIHeroClient>())
            {
                var cb = new CheckBox(hero.BaseSkinName) { CurrentValue = true };
                if (hero.Team != Player.Instance.Team)
                {
                    GankMenu.Add("EnemyGank" + hero.BaseSkinName, cb);
                }
            }
        }

        internal static void OnEndScene()
        {
            if (!GankMenu["enable"].Cast<CheckBox>().CurrentValue)
            {
                return;
            }

            var range = GankMenu["range"].Cast<Slider>().CurrentValue;
            var cd = GankMenu["cd"].Cast<Slider>().CurrentValue;
            var heros =
                EntityManager.Heroes.AllHeroes.Where(
                    x =>
                    x.IsInRange(Player.Instance.Position, range) && !x.IsDead && !x.IsInvulnerable && Detect(x)
                    && !x.IsMe);
            foreach (var hero in heros.Where(hero => hero != null && Game.Time - DrawDuration < cd))
            {
                var c = hero.IsAlly ? Color.FromArgb(125, 0, 255, 0) : Color.FromArgb(125, 255, 0, 0);
                Drawing.DrawLine(
                    Drawing.WorldToMinimap(Player.Instance.Position),
                    Drawing.WorldToMinimap(hero.Position),
                    5,
                    c);
            }
        }

        internal static void OnUpdate()
        {
            if (Player.Instance.IsDead || !GankMenu["enable"].Cast<CheckBox>().CurrentValue)
            {
                return;
            }

            var range = GankMenu["range"].Cast<Slider>().CurrentValue;
            var cd = GankMenu["cd"].Cast<Slider>().CurrentValue;
            var heros =
                EntityManager.Heroes.AllHeroes.Where(
                    x => x.IsInRange(Player.Instance.Position, range) && !x.IsDead && !x.IsInvulnerable && !x.IsMe);
            foreach (var hero in
                heros.Where(hero => hero != null && Detect(hero) && hero.IsInRange(Player.Instance.Position, range)))
            {
                if (Game.Time - EnterTime > cd)
                {
                    DrawDuration = Game.Time;
                    if (GankMenu["ping"].Cast<CheckBox>().CurrentValue)
                    {
                        if (hero.IsAlly)
                        {
                            TacticalMap.ShowPing(PingCategory.OnMyWay, hero, true);
                        }

                        if (hero.IsEnemy)
                        {
                            TacticalMap.ShowPing(PingCategory.Danger, hero, true);
                        }
                    }
                }

                EnterTime = Game.Time;
            }
        }

        internal static void OnDraw()
        {
            if (!GankMenu["enable"].Cast<CheckBox>().CurrentValue)
            {
                return;
            }

            var range = GankMenu["range"].Cast<Slider>().CurrentValue;
            var cd = GankMenu["cd"].Cast<Slider>().CurrentValue;
            var heros =
                EntityManager.Heroes.AllHeroes.Where(
                    x =>
                    x.IsInRange(Player.Instance.Position, range) && !x.IsDead && !x.IsInvulnerable && Detect(x)
                    && !x.IsMe);
            foreach (var hero in heros.Where(hero => hero != null && Game.Time - DrawDuration < cd))
            {
                var c = hero.IsAlly ? Color.FromArgb(125, 0, 255, 0) : Color.FromArgb(125, 255, 0, 0);
                Drawing.DrawLine(
                    Drawing.WorldToScreen(Player.Instance.Position),
                    Drawing.WorldToScreen(hero.Position),
                    5,
                    c);
                Drawing.DrawText(
                    Drawing.WorldToScreen(Player.Instance.ServerPosition.To2D().Extend(hero, 300f).To3D()),
                    Color.White,
                    hero.ChampionName,
                    40);
            }
        }

        internal static bool Detect(AIHeroClient hero)
        {
            if (!GankMenu["enable"].Cast<CheckBox>().CurrentValue || hero.IsMe)
            {
                return false;
            }

            return hero.IsEnemy
                       ? GankMenu["EnemyGank" + hero.BaseSkinName].Cast<CheckBox>().CurrentValue
                       : GankMenu["AllyGank" + hero.BaseSkinName].Cast<CheckBox>().CurrentValue;
        }
    }
}