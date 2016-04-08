namespace Khappa_Zix.Modes
{
    using System.Linq;

    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Menu.Values;

    using Load;

    using Misc;

    internal class Combo
    {
        public static bool Jumping;

        public static readonly bool doubleJump = menu.Jump["double"].Cast<CheckBox>().CurrentValue;

        public static readonly int Dis = menu.Combo["dis"].Cast<Slider>().CurrentValue;

        internal static void Start()
        {
            var target =
                EntityManager.Heroes.Enemies.FirstOrDefault(
                    x => !x.IsZombie && !x.IsInvulnerable && !x.IsDead && !x.HasBuffOfType(BuffType.PhysicalImmunity));

            if (target != null)
            {
                var Distance = Load.player.Distance(target);

                if (doubleJump && target.IsValidTarget(Load.Q.Range)
                    && (Load.GetQDamage(target) >= target.Health
                        || Load.player.GetSpellDamage(target, SpellSlot.W) >= target.Health))
                {
                    return;
                }

                if (Load.W.IsReady() && target.IsValidTarget(Load.W.Range)
                    && menu.Combo["W"].Cast<CheckBox>().CurrentValue)
                {
                    var pred = Load.W.GetPrediction(target).HitChance >= Misc.hitchance;
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

                if (Load.E.IsReady() && target.IsValidTarget(Load.E.Range) && Load.Q.IsReady()
                    && !target.IsValidTarget(Load.Q.Range - 50) && menu.Combo["E"].Cast<CheckBox>().CurrentValue)
                {
                    var pred = Load.E.GetPrediction(target).HitChance >= Misc.hitchance;

                    if ((Load.E.GetPrediction(target).CastPosition.IsUnderTurret() && target.IsUnderEnemyturret()
                         && !menu.Combo["Edive"].Cast<CheckBox>().CurrentValue)
                        || target.CountEnemiesInRange(600) >= menu.Combo["safe"].Cast<Slider>().CurrentValue)
                    {
                        return;
                    }

                    if (pred && Load.player.Distance(target) > Dis)
                    {
                        Load.E.Cast(Load.E.GetPrediction(target).CastPosition);
                    }
                }

                if (!menu.Combo["useR"].Cast<CheckBox>().CurrentValue)
                {
                    return;
                }

                if (menu.Combo["R"].Cast<CheckBox>().CurrentValue && Load.R.IsReady() && !Load.Q.IsReady()
                    && !Load.W.IsReady() && !Load.E.IsReady())
                {
                    Load.R.Cast();
                }

                if (menu.Combo["Rmode"].Cast<ComboBox>().CurrentValue == 0 && Load.R.IsReady())
                {
                    if ((Load.Q.IsReady() || Load.W.IsReady() || Load.E.IsReady()) && Load.player.ManaPercent >= 15)
                    {
                        if (Distance <= Load.E.Range + Load.Q.Range - 25 + (Load.player.MoveSpeed * 0.7)
                            && Distance > Load.Q.Range && Load.E.IsReady())
                        {
                            Load.R.Cast();
                        }
                    }
                }

                if (menu.Combo["Rmode"].Cast<ComboBox>().CurrentValue == 1 && Load.R.IsReady()
                    && target.IsValidTarget(Load.Q.Range + 25))
                {
                    Load.R.Cast();
                }

                if (menu.Combo["danger"].Cast<Slider>().CurrentValue >= Load.player.CountEnemiesInRange(550)
                    && Load.R.IsReady())
                {
                    Load.R.Cast();
                }
            }
        }
    }
}