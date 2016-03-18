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
        public static Menu TrackMenu { get; private set; }

        internal static void OnLoad()
        {
            TrackMenu = Load.UtliMenu.AddSubMenu("Tracker");
            TrackMenu.AddGroupLabel("Tracker Settings");
            TrackMenu.Add("Trackrecall", new CheckBox("Track Enemies Recall"));
            TrackMenu.Add("Tracktraps", new CheckBox("Track Enemy Traps [BETA]"));
            TrackMenu.AddSeparator();
            TrackMenu.Add("Track", new CheckBox("Track Enemies"));
            TrackMenu.Add("trackx", new Slider("Tracker Position X", 0, 0, 100));
            TrackMenu.Add("tracky", new Slider("Tracker Position Y", 0, 0, 100));
            Drawing.OnDraw += OnDraw;
            Drawing.OnEndScene += Drawing_OnEndScene;
            Teleport.OnTeleport += OnTeleport;
        }

        private static void Drawing_OnEndScene(EventArgs args)
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

            if (sender is AIHeroClient)
            {
                if (TrackMenu["Trackrecall"].Cast<CheckBox>().CurrentValue)
                {
                    if (args.Status == TeleportStatus.Start)
                    {
                        Notifications.Show(
                            new SimpleNotification(sender.BaseSkinName + " Is Recalling", "KappaTracker"));
                    }

                    if (args.Status == TeleportStatus.Abort)
                    {
                        Notifications.Show(
                            new SimpleNotification(sender.BaseSkinName + " Recall Aborted", "KappaTracker"));
                    }

                    if (args.Status == TeleportStatus.Finish)
                    {
                        Notifications.Show(
                            new SimpleNotification(sender.BaseSkinName + " Recall Finished", "KappaTracker"));
                    }
                }
            }
        }

        private static void OnDraw(EventArgs args)
        {
            if (TrackMenu["Track"].Cast<CheckBox>().CurrentValue)
            {
                var trackx = TrackMenu["trackx"].Cast<Slider>().CurrentValue;
                var tracky = TrackMenu["tracky"].Cast<Slider>().CurrentValue;
                float i = 0;
                foreach (var hero in
                    EntityManager.Heroes.Enemies.Where(
                        hero => hero != null && hero.IsEnemy && !hero.IsMe && !hero.IsDead))
                {
                    var champion = hero.ChampionName;
                    if (champion.Length > 12)
                    {
                        champion = champion.Remove(7) + "..";
                    }

                    var percent = (int)hero.HealthPercent;
                    var color = System.Drawing.Color.Red;
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

                    if (hero.IsVisible && hero.IsHPBarRendered)
                    {
                        Drawing.DrawText(
                            (Drawing.Width * 0.13f) + (trackx * 20),
                            (Drawing.Height * 0.1f) + (tracky * 10) + i,
                            color,
                            "    Visible ");
                    }
                    else
                    {
                        Drawing.DrawText(
                            (Drawing.Width * 0.13f) + (trackx * 20),
                            (Drawing.Height * 0.1f) + (tracky * 10) + i,
                            color,
                            "    Not Visible ");
                    }

                    i += 20f;
                }
            }
        }
    }
}