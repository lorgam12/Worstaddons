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

    using Color = System.Drawing.Color;

    internal class Tracker
    {
        public static TeleportStatus teleport;

        private static Vector2 PingLocation;

        private static int LastPingT;

        public static Menu TrackMenu { get; private set; }

        internal static void OnLoad()
        {
            TrackMenu = Load.UtliMenu.AddSubMenu("Tracker");
            TrackMenu.AddGroupLabel("Surrender Tracker");
            TrackMenu.Add("Trackally", new CheckBox("Track Allies Surrender", false));
            TrackMenu.Add("Trackenemy", new CheckBox("Track Enemies Surrender", false));
            TrackMenu.AddSeparator();
            TrackMenu.AddGroupLabel("Notifications Settings");
            TrackMenu.Add("recallnotify", new CheckBox("Notify On Enemy Recall", false));
            TrackMenu.AddSeparator();
            TrackMenu.AddGroupLabel("Tracker Settings");
            TrackMenu.Add("Track", new CheckBox("Track Enemies Status"));
            TrackMenu.Add("Tracktraps", new CheckBox("Track Enemy Traps [BETA]", false));
            TrackMenu.Add("Trackping", new CheckBox("Ping On Killable Enemies (Local)", false));
            TrackMenu.Add("Trackway", new CheckBox("Track Enemy WayPoints", false));
            TrackMenu.AddSeparator();
            TrackMenu.AddGroupLabel("Don't Track:");
            foreach (var enemy in ObjectManager.Get<AIHeroClient>())
            {
                var cb = new CheckBox(enemy.BaseSkinName) { CurrentValue = false };
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
                        CalcDamage(enemy) >= enemy.TotalShieldHealth()
                        && !TrackMenu["DontTrack" + enemy.BaseSkinName].Cast<CheckBox>().CurrentValue)
                    .Where(
                        enemy =>
                        TrackMenu.Get<CheckBox>("Trackping").CurrentValue && enemy.IsVisible && enemy.IsHPBarRendered))
            {
                Ping(enemy.Position.To2D());
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
            Core.DelayAction(SimplePing, 750);
        }

        private static void SimplePing()
        {
            TacticalMap.ShowPing(PingCategory.Danger, PingLocation, true);
        }

        public static int CalcDamage(Obj_AI_Base target)
        {
            var aa = ObjectManager.Player.GetAutoAttackDamage(target, true);
            var damage = aa;

            if (Player.Instance.Spellbook.GetSpell(SpellSlot.Q).IsReady)
            {
                // Q damage
                damage += ObjectManager.Player.GetSpellDamage(target, SpellSlot.Q);
            }

            if (Player.Instance.Spellbook.GetSpell(SpellSlot.W).IsReady)
            {
                // W damage
                damage += ObjectManager.Player.GetSpellDamage(target, SpellSlot.W);
            }

            if (Player.Instance.Spellbook.GetSpell(SpellSlot.E).IsReady)
            {
                // E damage
                damage += ObjectManager.Player.GetSpellDamage(target, SpellSlot.E);
            }

            if (Player.Instance.Spellbook.GetSpell(SpellSlot.R).IsReady)
            {
                // R damage
                damage += ObjectManager.Player.GetSpellDamage(target, SpellSlot.R);
            }

            return (int)damage;
        }

        internal static void Traps()
        {
            if (!TrackMenu["Tracktraps"].Cast<CheckBox>().CurrentValue)
            {
                return;
            }

            var traps = ObjectManager.Get<Obj_AI_Minion>();
            {
                foreach (var trap in traps.Where(trap => trap != null))
                {
                    switch (trap.Name)
                    {
                        case "Cupcake Trap":
                            Drawing.DrawText(
                                Drawing.WorldToScreen(trap.Position) - new Vector2(30, -30),
                                Color.White,
                                "Caitlyn Trap",
                                2);
                            Circle.Draw(SharpDX.Color.Purple, trap.BoundingRadius + 10, trap.Position);
                            break;

                        case "Noxious Trap":
                            if (trap.BaseSkinName == "NidaleeSpear")
                            {
                                var endTime = Math.Max(0, -Game.Time + 120);
                                Drawing.DrawText(
                                    Drawing.WorldToScreen(trap.Position) - new Vector2(30, -30),
                                    Color.White,
                                    "Nidalee Trap",
                                    2);
                                Circle.Draw(SharpDX.Color.Green, trap.BoundingRadius + 25, trap.Position);
                            }

                            if (trap.BaseSkinName == "TeemoMushroom")
                            {
                                if (trap.GetBuff("BantamTrap") != null)
                                {
                                    var endTime = Math.Max(0, trap.GetBuff("BantamTrap").EndTime - Game.Time);
                                    Drawing.DrawText(
                                        Drawing.WorldToScreen(trap.Position) - new Vector2(30, -30),
                                        Color.White,
                                        "Teemo Mushroom Expire: "
                                        + Convert.ToString(endTime, CultureInfo.InstalledUICulture),
                                        2);
                                }

                                Circle.Draw(SharpDX.Color.Green, trap.BoundingRadius * 3, trap.Position);
                            }
                            break;

                        case "Jack In The Box":
                            if (trap.GetBuff("JackInTheBox") != null)
                            {
                                var endTime = Math.Max(0, trap.GetBuff("JackInTheBox").EndTime - Game.Time);
                                Drawing.DrawText(
                                    Drawing.WorldToScreen(trap.Position) - new Vector2(30, -30),
                                    Color.White,
                                    "Shaco Box Expire: "
                                    + Convert.ToString(endTime, CultureInfo.InvariantCulture),
                                    2);
                            }

                            Circle.Draw(SharpDX.Color.Green, trap.BoundingRadius * 15, trap.Position);
                            break;
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
            if (!(sender is AIHeroClient))
            {
                return;
            }

            if (!TrackMenu["recallnotify"].Cast<CheckBox>().CurrentValue
                || TrackMenu["DontTrack" + sender.BaseSkinName].Cast<CheckBox>().CurrentValue)
            {
                return;
            }

            if (teleport == TeleportStatus.Start)
            {
                Notifications.Show(
                    new SimpleNotification(sender.BaseSkinName + " Is Recalling", sender.BaseSkinName + " Current Health = " + (int)sender.TotalShieldHealth()));
            }

            if (teleport == TeleportStatus.Abort)
            {
                Notifications.Show(
                    new SimpleNotification(sender.BaseSkinName + " Recall Aborted", sender.BaseSkinName + " Current Health = " + (int)sender.TotalShieldHealth()));
            }

            if (teleport == TeleportStatus.Finish)
            {
                Notifications.Show(
                    new SimpleNotification(sender.BaseSkinName + " Recall Finished", sender.BaseSkinName + " Current Health = " + (int)sender.TotalShieldHealth()));
            }
        }

        internal static void HPtrack()
        {
            var trackx = TrackMenu["trackx"].Cast<Slider>().CurrentValue;
            var tracky = TrackMenu["tracky"].Cast<Slider>().CurrentValue;
            float timer = 0;
            float i = 0;
            foreach (var hero in
                EntityManager.Heroes.Enemies.Where(
                    hero =>
                    hero != null && hero.IsEnemy
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
                    var color = Color.FromArgb(194, 194, 194);

                    if (percent > 0)
                    {
                        color = Color.Red;
                    }

                    if (percent > 25)
                    {
                        color = Color.Orange;
                    }

                    if (percent > 50)
                    {
                        color = Color.Yellow;
                    }

                    if (percent > 75)
                    {
                        color = Color.LimeGreen;
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

                    if (hero.Health < CalcDamage(hero) && !hero.IsDead)
                    {
                        Drawing.DrawText(
                            (Drawing.Width * 0.18f) + (trackx * 20),
                            (Drawing.Height * 0.1f) + (tracky * 10) + i,
                            color,
                            "Killable");
                    }

                    i += 20f;
                }


                if (!TrackMenu.Get<CheckBox>("Trackway").CurrentValue
                    || (!(hero.Path.LastOrDefault().Distance(Player.Instance)
                          <= TrackMenu["Distance"].Cast<Slider>().CurrentValue)
                        || hero.IsInRange(hero.Path.LastOrDefault(), 50)))
                {
                    continue;
                }

                Drawing.DrawLine(
                    hero.Position.WorldToScreen(),
                    hero.Path.LastOrDefault().WorldToScreen(),
                    2,
                    Color.White);
                Circle.Draw(SharpDX.Color.White, 50, hero.Path.LastOrDefault());
                timer += hero.Position.Distance(hero.Path.LastOrDefault()) / hero.MoveSpeed;
                Drawing.DrawText(
                    Drawing.WorldToScreen(hero.Path.LastOrDefault()) - new Vector2(25, -20),
                    Color.White,
                    hero.ChampionName + " " + timer.ToString("F"),
                    12);
            }
        }
    }
}