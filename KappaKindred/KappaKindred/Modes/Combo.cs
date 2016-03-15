namespace KappaKindred.Modes
{
    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Menu.Values;

    internal class Combo
    {
        public static void Start()
        {
            var useQ = Menu.ComboMenu["Q"].Cast<CheckBox>().CurrentValue && Spells.Q.IsReady();
            var qmode = Menu.ComboMenu["Qmode"].Cast<ComboBox>().CurrentValue;
            var useW = Menu.ComboMenu["W"].Cast<CheckBox>().CurrentValue && Spells.W.IsReady();
            var useE = Menu.ComboMenu["E"].Cast<CheckBox>().CurrentValue && Spells.E.IsReady();
            var Qaf = Menu.ComboMenu["Qaf"].Cast<CheckBox>().CurrentValue && Spells.Q.IsReady();
            var target = TargetSelector.GetTarget(Spells.E.Range, DamageType.Physical);
            var qtarget = TargetSelector.GetTarget(Spells.Q.Range, DamageType.Physical);

            if (target != null)
            {
                if (useE)
                {
                    Spells.E.Cast(target);
                }

                if (useW)
                {
                    Spells.W.Cast();
                }
            }

            if (qtarget != null)
            {
                if (useQ && !Qaf)
                {
                    Spells.Q.Cast(qmode == 0 ? qtarget.Position : Game.CursorPos);
                }
            }
        }
    }
}