using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KappaKindred.Events
{
    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Menu.Values;

    using Modes;

    internal class OnUpdate
    {
        public static void Update(EventArgs args)
        {
            var lanemana = Menu.ManaMenu["lanemana"].Cast<Slider>().CurrentValue;
            var harassmana = Menu.ManaMenu["harassmana"].Cast<Slider>().CurrentValue;
            if (Player.Instance.IsDead || MenuGUI.IsChatOpen || Player.Instance.IsRecalling())
            {
                return;
            }

            var flags = Orbwalker.ActiveModesFlags;
            if (flags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                Combo.Start();
            }

            if (flags.HasFlag(Orbwalker.ActiveModes.LaneClear) && Player.Instance.ManaPercent >= lanemana)
            {
                Clear.Start();
            }

            if (flags.HasFlag(Orbwalker.ActiveModes.Harass) && Player.Instance.ManaPercent >= harassmana)
            {
                Harass.Start();
            }

            if (flags.HasFlag(Orbwalker.ActiveModes.Flee))
            {
                Flee.Start();
            }
        }
    }
}