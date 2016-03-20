namespace KappaUtility.Trackers
{
    using System;
    using System.Globalization;
    using System.Linq;

    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Enumerations;
    using EloBuddy.SDK.Events;
    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Menu.Values;
    using EloBuddy.SDK.Notifications;
    using EloBuddy.SDK.Rendering;

    using SharpDX;

    internal class Tracker
    {
        public static TeleportStatus teleport;

        private static Vector2 PingLocation;

        private static int LastPingT = 0;

        public static Menu TrackMenu { get; private set; }

        internal static void OnLoad()
        {
            TrackMenu = Load.UtliMenu.AddSubMenu("Tracker");
            TrackMenu.AddGroupLabel("Surrender Tracker");
            TrackMenu.Add("Trackally", new CheckBox("Track Allies Surrender"));
            TrackMenu.Add("Trackenemy", new CheckBox("Track Enemies Surrender"));
            TrackMenu.AddSeparator();
            TrackMenu.AddGroupLabel("Notifications Settings");
            TrackMenu.Add("recallnotify", new CheckBox("Notify On Enemy Recall"));
            TrackMenu.AddSeparator();
            TrackMenu.AddGroupLabel("Tracker Settings");
            TrackMenu.Add("Track", new CheckBox("Track Enemies Status"));
            TrackMenu.Add("Tracktraps", new CheckBox("Track Enemy Traps [BETA]"));
            TrackMenu.Add("Trackping", new CheckBox("Ping On Killable Enemies (Local)"));
            TrackMenu.Add("Trackway", new CheckBox("Track Enemy WayPoints"));
            TrackMenu.AddSeparator();
            TrackMenu.AddGroupLabel("Don't Track:");
            foreach (var enemy in ObjectManager.Get<AIHeroClient>())
            {
                CheckBox cb = new CheckBox(enemy.BaseSkinName) { CurrentValue = false };
                if (enemy.Team != Player.Instance.Team)
                {
                    TrackMenu.Add("DontTrack" + enemy.BaseSkinName, cb);
                }
            }

            TrackMenu.Add("Distance", new Slider("WayPoints Detection Range", 1000, 0, 5000));
            TrackMenu.AddGroupLabel("Drawings Settings");
            TrackMenu.Add("trackx", new Slider("Tracker Position X", 0, 0, 100));
            TrackMenu.Add("tracky", new Slider("Tracker Position Y", 0, 0, 100));

            Teleport.OnTeleport += OnTeleport;
        }

        public static void track()
        {
            foreach (var enemy in
                ObjectManager.Get<AIHeroClient>()
                    .Where(ene => ene != null && !ene.IsDead && ene.IsEnemy && ene.IsValid)
                    .Where(
                        enemy =>
                        DamageInd.CalcDamage(enemy) >= enemy.TotalShieldHealth()
                        && !TrackMenu["DontTrack" + enemy.BaseSkinName].Cast<CheckBox>().CurrentValue))
            {
                if (TrackMenu.Get<CheckBox>("Trackping").CurrentValue && enemy.IsVisible && enemy.IsHPBarRendered)
                {
                    Ping(enemy.Position.To2D());
                }
            }
        }

        private static void Ping(Vector2 position)
        {
            if (Environment.TickCount - LastPingT < 30 * 1000)
            {
                return;
            }

            LastPingT = Environment.TickCount;
            PingLocation = position;
            SimplePing();
            Core.DelayAction(SimplePing, 150);
            Core.DelayAction(SimplePing, 450);
            Core.DelayAction(SimplePing, 950);
        }

        private static void SimplePing()
        {
            TacticalMap.ShowPing(PingCategory.Danger, PingLocation, true);
        }

        internal static void Traps()
        {
            if (TrackMenu["Tracktraps"].Cast<CheckBox>().CurrentValue)
            {
                var traps = ObjectManager.Get<Obj_AI_Minion>();
                {
                    foreach (var trap in traps)
                    {
                        if (trap != null)
                        {
                            if (trap.Name == "Cupcake Trap")
                            {
                                Drawing.DrawText(
                                    Drawing.WorldToScreen(trap.Position) - new Vector2(30, -30),
                                    System.Drawing.Color.White,
                                    "Caitlyn Trap",
                                    2);
                                Circle.Draw(Color.Purple, trap.BoundingRadius + 10, trap.Position);
                            }

                            if (trap.Name == "Noxious Trap")
                            {
                                if (trap.BaseSkinName == "NidaleeSpear")
                                {
                                    var endTime = Math.Max(0, -Game.Time + 120);
                                    Drawing.DrawText(
                                        Drawing.WorldToScreen(trap.Position) - new Vector2(30, -30),
                                        System.Drawing.Color.White,
                                        "Nidalee Trap",
                                        2);
                                    Circle.Draw(Color.Green, trap.BoundingRadius + 25, trap.Position);
                                }

                                if (trap.BaseSkinName == "TeemoMushroom")
                                {
                                    if (trap.GetBuff("BantamTrap") != null)
                                    {
                                        var endTime = Math.Max(0, trap.GetBuff("BantamTrap").EndTime - Game.Time);
                                        Drawing.DrawText(
                                            Drawing.WorldToScreen(trap.Position) - new Vector2(30, -30),
                                            System.Drawing.Color.White,
                                            "Teemo Mushroom Expire: "
                                            + Convert.ToString(endTime, CultureInfo.InstalledUICulture),
                                            2);
                                    }

                                    Circle.Draw(Color.Green, trap.BoundingRadius * 3, trap.Position);
                                }
                            }

                            if (trap.Name == "Jack In The Box")
                            {
                                if (trap.GetBuff("JackInTheBox") != null)
                                {
                                    var endTime = Math.Max(0, trap.GetBuff("JackInTheBox").EndTime - Game.Time);
                                    Drawing.DrawText(
                                        Drawing.WorldToScreen(trap.Position) - new Vector2(30, -30),
                                        System.Drawing.Color.White,
                                        "Shaco Box Expire: " + Convert.ToString(endTime, CultureInfo.InvariantCulture),
                                        2);
                                }

                                Circle.Draw(Color.Green, trap.BoundingRadius * 15, trap.Position);
                            }
                        }
                    }
                }
            }
        }

        private static void OnTeleport(Obj_AI_Base sender, Teleport.TeleportEventArgs args)
        {
            if (sender.IsAlly || sender.IsMe)
            {
                return;
            }

            teleport = args.Status;
            if (sender is AIHeroClient)
            {
                if (TrackMenu["recallnotify"].Cast<CheckBox>().CurrentValue
                    && !TrackMenu["DontTrack" + sender.BaseSkinName].Cast<CheckBox>().CurrentValue)
                {
                    if (teleport == TeleportStatus.Start)
                    {
                        Notifications.Show(
                            new SimpleNotification(sender.BaseSkinName + " Is Recalling", "KappaTracker"));
                    }

                    if (teleport == TeleportStatus.Abort)
                    {
                        Notifications.Show(
                            new SimpleNotification(sender.BaseSkinName + " Recall Aborted", "KappaTracker"));
                    }

                    if (teleport == TeleportStatus.Finish)
                    {
                        Notifications.Show(
                            new SimpleNotification(sender.BaseSkinName + " Recall Finished", "KappaTracker"));
                    }
                }
            }
        }

        internal static void HPtrack()
        {
            float timer = 0;
            var trackx = TrackMenu["trackx"].Cast<Slider>().CurrentValue;
            var tracky = TrackMenu["tracky"].Cast<Slider>().CurrentValue;
            float i = 0;
            foreach (var hero in
                EntityManager.Heroes.Enemies.Where(
                    hero =>
                    hero != null && hero.IsEnemy && !hero.IsMe
                    && !TrackMenu["DontTrack" + hero.BaseSkinName].Cast<CheckBox>().CurrentValue))
            {
                if (TrackMenu["Track"].Cast<CheckBox>().CurrentValue)
                {
                    var champion = hero.ChampionName;
                    if (champion.Length > 12)
                    {
                        champion = champion.Remove(7) + "..";
                    }

                    var percent = (int)hero.HealthPercent;
                    var color = System.Drawing.Color.FromArgb(206, 206, 206);

                    if (percent > 0)
                    {
                        color = System.Drawing.Color.Red;
                    }

                    if (percent > 25)
                    {
                        color = System.Drawing.Color.Orange;
                    }

                    if (percent > 50)
                    {
                        color = System.Drawing.Color.Yellow;
                    }

                    if (percent > 75)
                    {
                        color = System.Drawing.Color.LimeGreen;
                    }

                    Drawing.DrawText(
                        (Drawing.Width * 0.01f) + (trackx * 20),
                        (Drawing.Height * 0.1f) + (tracky * 10) + i,
                        color,
                        champion);
                    Drawing.DrawText(
                        (Drawing.Width * 0.06f) + (trackx * 20),
                        (Drawing.Height * 0.1f) + (tracky * 10) + i,
                        color,
                        (" ( " + (int)hero.TotalShieldHealth()) + " / " + (int)hero.MaxHealth + " | " + percent + "% ) ");

                    if (hero.IsVisible && hero.IsHPBarRendered && !hero.IsDead)
                    {
                        Drawing.DrawText(
                            (Drawing.Width * 0.13f) + (trackx * 20),
                            (Drawing.Height * 0.1f) + (tracky * 10) + i,
                            color,
                            "     Visible ");
                    }
                    else
                    {
                        if (!hero.IsDead)
                        {
                            Drawing.DrawText(
                                (Drawing.Width * 0.13f) + (trackx * 20),
                                (Drawing.Height * 0.1f) + (tracky * 10) + i,
                                color,
                                "     Not Visible ");
                        }
                    }
                    if (hero.IsDead)
                    {
                        Drawing.DrawText(
                            (Drawing.Width * 0.13f) + (trackx * 20),
                            (Drawing.Height * 0.1f) + (tracky * 10) + i,
                            color,
                            "     Dead ");
                    }

                    if (hero.Health < DamageInd.CalcDamage(hero))
                    {
                        Drawing.DrawText(
                            (Drawing.Width * 0.18f) + (trackx * 20),
                            (Drawing.Height * 0.1f) + (tracky * 10) + i,
                            color,
                            "Killable");
                    }

                    i += 20f;
                }

                if (TrackMenu.Get<CheckBox>("Trackway").CurrentValue)
                {
                    if (hero.Path.LastOrDefault().Distance(Player.Instance)
                        <= TrackMenu["Distance"].Cast<Slider>().CurrentValue)
                    {
                        if (!hero.IsInRange(hero.Path.LastOrDefault(), 50))
                        {
                            Drawing.DrawLine(
                                hero.Position.WorldToScreen(),
                                hero.Path.LastOrDefault().WorldToScreen(),
                                2,
                                System.Drawing.Color.White);
                            Circle.Draw(Color.White, 50, hero.Path.LastOrDefault());
                            if (hero != null && hero.Position != null && hero.Path.LastOrDefault() != null)
                            {
                                timer += hero.Position.Distance(hero.Path.LastOrDefault()) / hero.MoveSpeed;
                                Drawing.DrawText(
                                    Drawing.WorldToScreen(hero.Path.LastOrDefault()) - new Vector2(15, -15),
                                    System.Drawing.Color.White,
                                    hero.ChampionName + " " + timer.ToString("F"),
                                    12);
                            }
                        }
                    }
                }
            }
        }
    }
}