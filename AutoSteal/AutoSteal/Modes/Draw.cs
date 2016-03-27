namespace AutoSteal.Modes
{
    using System.Drawing;

    using Library;
    using Library.Spells;

    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Menu.Values;

    internal class Draw
    {
        protected static SpellBase Spells => SpellManager.CurrentSpells;

        public static void DebugJS()
        {
            var trackx = Program.DrawMenu["trackx"].Cast<Slider>().CurrentValue;
            var tracky = Program.DrawMenu["tracky"].Cast<Slider>().CurrentValue;
            float i = 0;
            i += 20f;
            if (JungleSteal.Mobxdd == null)
            {
                return;
            }

            var qtraveltime = JungleSteal.Mobxdd.Distance(ObjectManager.Player) / Spells.Q.Handle.SData.MissileSpeed
                              + Spells.Q.CastDelay + Game.Ping / 2f / 1000;
            var wtraveltime = JungleSteal.Mobxdd.Distance(ObjectManager.Player) / Spells.W.Handle.SData.MissileSpeed
                              + Spells.W.CastDelay + Game.Ping / 2f / 1000;
            var etraveltime = JungleSteal.Mobxdd.Distance(ObjectManager.Player) / Spells.E.Handle.SData.MissileSpeed
                              + Spells.E.CastDelay + Game.Ping / 2f / 1000;
            var rtraveltime = JungleSteal.Mobxdd.Distance(ObjectManager.Player) / Spells.R.Handle.SData.MissileSpeed
                              + Spells.R.CastDelay + Game.Ping / 2f / 1000;
            var champion = ObjectManager.Player.ChampionName;
            if ((KillSteal.playerdamage
                 + ObjectManager.Player.GetSpellDamage(JungleSteal.Mobxdd, SpellSlot.Q) + 750
                 <= Prediction.Health.GetPrediction(JungleSteal.Mobxdd, (int)qtraveltime)
                 && Spells.Q.IsInRange(JungleSteal.Mobxdd) && Spells.Q.IsReady())
                || (ObjectManager.Player.BaseAbilityDamage
                    + ObjectManager.Player.GetSpellDamage(JungleSteal.Mobxdd, SpellSlot.R) + 750
                    >= Prediction.Health.GetPrediction(JungleSteal.Mobxdd, (int)rtraveltime)
                    && Spells.R.IsInRange(JungleSteal.Mobxdd) && Spells.R.IsReady())
                || (ObjectManager.Player.BaseAbilityDamage
                    + ObjectManager.Player.GetSpellDamage(JungleSteal.Mobxdd, SpellSlot.W) + 750
                    >= Prediction.Health.GetPrediction(JungleSteal.Mobxdd, (int)wtraveltime)
                    && Spells.W.IsInRange(JungleSteal.Mobxdd) && Spells.W.IsReady())
                || (ObjectManager.Player.BaseAbilityDamage
                    + ObjectManager.Player.GetSpellDamage(JungleSteal.Mobxdd, SpellSlot.E) + 750
                    >= Prediction.Health.GetPrediction(JungleSteal.Mobxdd, (int)etraveltime)
                    && Spells.E.IsInRange(JungleSteal.Mobxdd) && Spells.E.IsReady()))
            {
                Drawing.DrawText(
                    (Drawing.Width * 0.1f) + (trackx * 15),
                    (Drawing.Height * 0.1f) + (tracky * 8) + i,
                    Color.Red,
                    JungleSteal.Mobxdd.BaseSkinName + " Health = " + JungleSteal.Mobxdd.Health,
                    2);
            }
        }

        public static void DebugKS()
        {
            var trackx = Program.DrawMenu["trackx"].Cast<Slider>().CurrentValue;
            var tracky = Program.DrawMenu["tracky"].Cast<Slider>().CurrentValue;
            float i = 0;
            i += 20f;

            if (KillSteal.Targetxdd == null)
            {
                return;
            }
            var Qtraveltime = KillSteal.Targetxdd.Distance(ObjectManager.Player)
                              / (Spells.Q.Handle.SData.MissileSpeed + Spells.Q.CastDelay) + Game.Ping / 2f / 1000;
            var Wtraveltime = KillSteal.Targetxdd.Distance(ObjectManager.Player)
                              / (Spells.W.Handle.SData.MissileSpeed + Spells.W.CastDelay) + Game.Ping / 2f / 1000;
            var Etraveltime = KillSteal.Targetxdd.Distance(ObjectManager.Player)
                              / (Spells.E.Handle.SData.MissileSpeed + Spells.E.CastDelay) + Game.Ping / 2f / 1000;
            var Rtraveltime = KillSteal.Targetxdd.Distance(ObjectManager.Player)
                              / (Spells.R.Handle.SData.MissileSpeed + Spells.R.CastDelay) + Game.Ping / 2f / 1000;

            if ((KillSteal.playerdamage
                 + ObjectManager.Player.GetSpellDamage(KillSteal.Targetxdd, SpellSlot.Q) + 750
                 >= Prediction.Health.GetPrediction(KillSteal.Targetxdd, (int)Qtraveltime)
                 && Spells.Q.IsInRange(KillSteal.Targetxdd) && Spells.Q.IsReady())
                || (ObjectManager.Player.BaseAbilityDamage
                    + ObjectManager.Player.GetSpellDamage(KillSteal.Targetxdd, SpellSlot.W) + 750
                    >= Prediction.Health.GetPrediction(KillSteal.Targetxdd, (int)(Wtraveltime))
                    && Spells.W.IsInRange(KillSteal.Targetxdd) && Spells.W.IsReady())
                || (ObjectManager.Player.BaseAbilityDamage
                    + ObjectManager.Player.GetSpellDamage(KillSteal.Targetxdd, SpellSlot.E) + 750
                    >= Prediction.Health.GetPrediction(KillSteal.Targetxdd, (int)(Etraveltime))
                    && Spells.E.IsInRange(KillSteal.Targetxdd) && Spells.E.IsReady())
                || (ObjectManager.Player.BaseAbilityDamage
                    + ObjectManager.Player.GetSpellDamage(KillSteal.Targetxdd, SpellSlot.R) + 750
                    >= Prediction.Health.GetPrediction(KillSteal.Targetxdd, (int)(Rtraveltime))
                    && Spells.R.IsInRange(KillSteal.Targetxdd) && Spells.R.IsReady()))
            {
                Drawing.DrawText(
                    (Drawing.Width * 0.1f) + (trackx * 15),
                    (Drawing.Height * 0.3f) + (tracky * 8) + i,
                    Color.Red,
                    KillSteal.Targetxdd.BaseSkinName + " Health = " + KillSteal.Targetxdd.Health,
                    2);
            }
        }
    }
}