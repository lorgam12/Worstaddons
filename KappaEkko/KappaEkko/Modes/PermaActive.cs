namespace KappaEkko.Modes
{
    using EloBuddy.SDK.Menu.Values;

    using Logics;

    internal class PermaActive
    {
        public static void Active()
        {
            var RAoe = Menu.UltMenu["RAoe"].Cast<CheckBox>().CurrentValue;
            var REscape = Menu.UltMenu["REscape"].Cast<CheckBox>().CurrentValue;

            if (Spells.EkkoREmitter == null)
            {
                return;
            }

            if (Spells.R.IsReady())
            {
                if (RAoe)
                {
                    Rlogic.Aoe();
                }

                if (REscape)
                {
                    Rlogic.Escape();
                }
            }
        }
    }
}