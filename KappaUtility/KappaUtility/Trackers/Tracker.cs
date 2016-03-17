using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KappaUtility.Trackers
{
    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Menu.Values;

    internal class Tracker
    {
        public static Menu TrackMenu { get; private set; }

        internal static void OnLoad()
        {
            TrackMenu = Load.UtliMenu.AddSubMenu("Tracker");
            TrackMenu.AddGroupLabel("Tracker Settings");
            TrackMenu.Add("Track", new CheckBox("Track Enemies"));
            TrackMenu.Add("trackx", new Slider("Tracker Position X", 0, 0, 100));
            TrackMenu.Add("tracky", new Slider("Tracker Position Y", 0, 0, 100));
            Drawing.OnDraw += OnDraw;
        }

        private static void OnDraw(EventArgs args)
        {
            if (TrackMenu["Track"].Cast<CheckBox>().CurrentValue)
            {
                var trackx = TrackMenu["trackx"].Cast<Slider>().CurrentValue;
                var tracky = TrackMenu["tracky"].Cast<Slider>().CurrentValue;
                float i = 0;
                foreach (
                    var hero in
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
                            "   Visible ");
                    }
                    else
                    {
                        Drawing.DrawText(
                            (Drawing.Width * 0.13f) + (trackx * 20),
                            (Drawing.Height * 0.1f) + (tracky * 10) + i,
                            color,
                            "   Not Visible ");
                    }

                    i += 20f;
                }
            }
        }
    }
}