namespace Khappa_Zix.Modes
{
    using System.Linq;

    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Enumerations;
    using EloBuddy.SDK.Menu.Values;

    using Khappa_Zix.Load;

    internal class Combo
    {
        public static HitChance hitchance;

        public static bool Jumping;

        public static readonly bool doubleJump = menu.Jump["double"].Cast<CheckBox>().CurrentValue;

        public static readonly int Dis = menu.Combo["dis"].Cast<Slider>().CurrentValue;

        internal static void Start()
        {
            switch (menu.Combo["HitChance"].Cast<ComboBox>().CurrentValue)
            {
                case 0:
                    {
                        hitchance = HitChance.High;
                    }
                    break;

                case 1:
                    {
                        hitchance = HitChance.Medium;
                    }
                    break;

                case 2:
                    {
                        hitchance = HitChance.Low;
                    }
                    break;
            }
            var target =
                EntityManager.Heroes.Enemies.FirstOrDefault(
                    x =>
                    x.IsValidTarget(Load.W.Range) && !x.IsZombie && !x.IsInvulnerable
                    && !x.HasBuffOfType(BuffType.PhysicalImmunity));

            if (target != null)
            {
                if (doubleJump && target.IsValidTarget(Load.Q.Range)
                    && (Load.GetQDamage(target) >= target.Health
                        || Player.Instance.GetSpellDamage(target, SpellSlot.W) >= target.Health))
                {
                    return;
                }

                if (Load.W.IsReady() && target.IsValidTarget(Load.W.Range)
                    && menu.Combo["W"].Cast<CheckBox>().CurrentValue)
                {
                    var pred = Load.W.GetPrediction(target).HitChance >= hitchance;
                    if (pred)
                    {
                        Load.W.Cast(Load.W.GetPrediction(target).CastPosition);
                    }
                }

                if (Load.Q.IsReady() && target.IsValidTarget(Load.Q.Range)
                    && menu.Combo["Q"].Cast<CheckBox>().CurrentValue)
                {
                    Load.Q.Cast(target);
                }

                if (Load.E.IsReady() && target.IsValidTarget(Load.W.Range) && Load.Q.IsReady()
                    && !target.IsValidTarget(Load.Q.Range - 50) && menu.Combo["E"].Cast<CheckBox>().CurrentValue)
                {
                    var pred = Load.E.GetPrediction(target).HitChance >= hitchance;

                    if (Load.E.GetPrediction(target).CastPosition.IsUnderTurret() && target.IsUnderEnemyturret()
                        && !menu.Combo["Edive"].Cast<CheckBox>().CurrentValue)
                    {
                        return;
                    }

                    if (pred && Player.Instance.Distance(target) > Dis)
                    {
                        Load.E.Cast(Load.E.GetPrediction(target).CastPosition);
                    }
                }
            }
        }
    }
}