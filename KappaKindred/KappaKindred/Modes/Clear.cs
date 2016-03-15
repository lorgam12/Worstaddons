using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KappaKindred.Modes
{
    using EloBuddy.SDK.Menu.Values;

    internal class Clear
    {
        public static void Start()
        {
            var useQ = Menu.LaneMenu["Q"].Cast<CheckBox>().CurrentValue && Spells.Q.IsReady();
            var useW = Menu.LaneMenu["W"].Cast<CheckBox>().CurrentValue && Spells.W.IsReady();
            var useE = Menu.LaneMenu["E"].Cast<CheckBox>().CurrentValue && Spells.E.IsReady();
        }
    }
}