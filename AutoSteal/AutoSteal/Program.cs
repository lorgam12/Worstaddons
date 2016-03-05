namespace AutoSteal
{
    using System;
    using System.Linq;
    using Champs;
    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Events;
    using EloBuddy.SDK.Menu.Values;

    internal class Program
    {

        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoad;
        }


        private static void OnLoad(EventArgs args)
        {
            Game.OnUpdate += OnUpdate;
            Start.Startload();
        }

        private static void OnUpdate(EventArgs args)
        {
            if (Start.menuIni["AAJ"].Cast<CheckBox>().CurrentValue || Start.menuIni["AAC"].Cast<CheckBox>().CurrentValue)
            {
                AA();
            }

            if (ObjectManager.Player.ChampionName == "Gragas")
            {
                if (Gragas.GragMenu["QC"].Cast<CheckBox>().CurrentValue
                    || Gragas.GragMenu["EC"].Cast<CheckBox>().CurrentValue
                    || Gragas.GragMenu["RC"].Cast<CheckBox>().CurrentValue)
                {
                    Gragas.KS();
                }

                if (Gragas.GragMenu["QJ"].Cast<CheckBox>().CurrentValue
                    || Gragas.GragMenu["EJ"].Cast<CheckBox>().CurrentValue
                    || Gragas.GragMenu["RJ"].Cast<CheckBox>().CurrentValue)
                {
                    Gragas.JKS();
                }
            }

            if (ObjectManager.Player.ChampionName == "Jinx")
            {
                if (Jinx.JinxMenu["WC"].Cast<CheckBox>().CurrentValue
                    || Jinx.JinxMenu["RC"].Cast<CheckBox>().CurrentValue)
                {
                    Jinx.KS();
                }

                if (Jinx.JinxMenu["WJ"].Cast<CheckBox>().CurrentValue)
                {
                    Jinx.JKS();
                }
            }

            if (ObjectManager.Player.ChampionName == "Katarina")
            {
                if (Katarina.KataMenu["QC"].Cast<CheckBox>().CurrentValue || Katarina.KataMenu["WC"].Cast<CheckBox>().CurrentValue || Katarina.KataMenu["EC"].Cast<CheckBox>().CurrentValue)
                {
                    Katarina.KS();
                }

                if (Katarina.KataMenu["QJ"].Cast<CheckBox>().CurrentValue || Katarina.KataMenu["WJ"].Cast<CheckBox>().CurrentValue || Katarina.KataMenu["EJ"].Cast<CheckBox>().CurrentValue)
                {
                    Katarina.JKS();
                }
            }

            if (ObjectManager.Player.ChampionName == "LeeSin")
            {
                if (LeeSin.LeeMenu["QC"].Cast<CheckBox>().CurrentValue || LeeSin.LeeMenu["EC"].Cast<CheckBox>().CurrentValue)
                {
                    LeeSin.KS();
                }

                if (LeeSin.LeeMenu["QJ"].Cast<CheckBox>().CurrentValue || LeeSin.LeeMenu["EJ"].Cast<CheckBox>().CurrentValue)
                {
                    LeeSin.JKS();
                }
            }

            if (ObjectManager.Player.ChampionName == "Nidalee")
            {
                if (Nidalee.NidaMenu["QC"].Cast<CheckBox>().CurrentValue)
                {
                    Nidalee.KS();
                }

                if (Nidalee.NidaMenu["QJ"].Cast<CheckBox>().CurrentValue)
                {
                    Nidalee.JKS();
                }
            }

            if (ObjectManager.Player.ChampionName == "Riven")
            {
                if (Riven.RivenMenu["WC"].Cast<CheckBox>().CurrentValue || Riven.RivenMenu["RC"].Cast<CheckBox>().CurrentValue)
                {
                    Riven.KS();
                }

                if (Riven.RivenMenu["WJ"].Cast<CheckBox>().CurrentValue || Riven.RivenMenu["RJ"].Cast<CheckBox>().CurrentValue)
                {
                    Riven.JKS();
                }
            }

            if (ObjectManager.Player.ChampionName == "Sivir")
            {
                if (Sivir.SivMenu["QC"].Cast<CheckBox>().CurrentValue)
                {
                    Sivir.KS();
                }

                if (Sivir.SivMenu["QJ"].Cast<CheckBox>().CurrentValue)
                {
                    Sivir.JKS();
                }
            }

            if (ObjectManager.Player.ChampionName == "Thresh")
            {
                if (Thresh.ThreshMenu["QC"].Cast<CheckBox>().CurrentValue || Thresh.ThreshMenu["EC"].Cast<CheckBox>().CurrentValue)
                {
                    Thresh.KS();
                }

                if (Thresh.ThreshMenu["QJ"].Cast<CheckBox>().CurrentValue || Thresh.ThreshMenu["EJ"].Cast<CheckBox>().CurrentValue)
                {
                    Thresh.JKS();
                }
            }
        }

        private static void AA()
        {
            if (Start.menuIni["AAC"].Cast<CheckBox>().CurrentValue)
            {
                foreach (AIHeroClient target in
                    ObjectManager.Get<AIHeroClient>()
                        .Where(
                            hero =>
                            hero.IsValidTarget(ObjectManager.Player.GetAutoAttackRange())
                            && hero.IsInAutoAttackRange(ObjectManager.Player)
                            && !hero.HasBuffOfType(BuffType.Invulnerability)
                            && hero.IsEnemy
                            && !hero.IsDead
                            && !hero.IsZombie))
                {
                    if (ObjectManager.Player.GetAutoAttackDamage(target) > target.TotalShieldHealth()
                        && target.IsInAutoAttackRange(ObjectManager.Player))
                    {
                        Player.IssueOrder(GameObjectOrder.AttackUnit, target);
                        return;
                    }
                }
            }

            if (Start.menuIni["AAJ"].Cast<CheckBox>().CurrentValue)
            {
                foreach (Obj_AI_Minion mob in
                    ObjectManager.Get<Obj_AI_Minion>()
                        .Where(
                            jmob =>
                            jmob.IsValidTarget(ObjectManager.Player.GetAutoAttackRange())
                            && jmob.IsInAutoAttackRange(ObjectManager.Player)
                            && !jmob.HasBuffOfType(BuffType.Invulnerability)
                            && jmob.IsMonster
                            && !jmob.IsDead
                            && !jmob.IsZombie
                            && (jmob.BaseSkinName == "SRU_Dragon"
                            || jmob.BaseSkinName == "SRU_Baron"
                            || jmob.BaseSkinName == "SRU_Gromp"
                            || jmob.BaseSkinName == "SRU_Krug"
                            || jmob.BaseSkinName == "SRU_Razorbeak"
                            || jmob.BaseSkinName == "Sru_Crab"
                            || jmob.BaseSkinName == "SRU_Murkwolf"
                            || jmob.BaseSkinName == "SRU_Blue"
                            || jmob.BaseSkinName == "SRU_Red")))
                {
                    if (ObjectManager.Player.GetAutoAttackDamage(mob) > mob.Health
                        && mob.IsInAutoAttackRange(ObjectManager.Player))
                    {
                        Player.IssueOrder(GameObjectOrder.AttackUnit, mob);
                        return;
                    }
                }
            }
        }
    }
}
