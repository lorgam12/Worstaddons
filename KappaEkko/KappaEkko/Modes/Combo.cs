namespace KappaEkko.Modes
{
    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Enumerations;
    using EloBuddy.SDK.Menu.Values;

    using Logics;

    internal class Combo
    {
        public static void Start()
        {
            var useQ = Menu.ComboMenu["Q"].Cast<CheckBox>().CurrentValue;
            var useW = Menu.ComboMenu["W"].Cast<CheckBox>().CurrentValue;
            var useWpred = Menu.ComboMenu["Wpred"].Cast<CheckBox>().CurrentValue;
            var useE = Menu.ComboMenu["E"].Cast<CheckBox>().CurrentValue;
            var useR = Menu.ComboMenu["R"].Cast<CheckBox>().CurrentValue;
            var Qtarget = TargetSelector.GetTarget(Spells.Q.Range, DamageType.Magical);
            var Wtarget = TargetSelector.GetTarget(Spells.W.Range, DamageType.Magical);

            if (useR && Spells.R.IsReady())
            {
                Rlogic.Combo();
            }

            if (Wtarget != null && Wtarget.IsValidTarget(Spells.W.Range) && Spells.W.IsReady())
            {
                if (useWpred)
                {
                    var pred = Spells.W.GetPrediction(Wtarget);
                    if (pred.HitChance >= HitChance.High)
                    {
                        Spells.W.Cast(pred.CastPosition);
                    }
                }

                if (useW)
                {
                    if (useWpred)
                    {
                        return;
                    }

                    Spells.W.Cast(Wtarget.Position);
                }
            }

            if (Qtarget != null && Qtarget.IsValidTarget(Spells.Q.Range))
            {
                if (useE && Spells.E.IsReady())
                {
                    Spells.E.Cast(Qtarget.Position);
                    Orbwalker.ResetAutoAttack();
                }

                if (useQ && Spells.Q.IsReady())
                {
                    var pred = Spells.Q.GetPrediction(Qtarget).CastPosition;
                    Spells.Q.Cast(pred);
                }
            }
        }
    }
}