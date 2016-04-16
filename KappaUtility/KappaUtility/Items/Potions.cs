namespace KappaUtility.Items
{
    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Menu.Values;

    internal class Potions
    {
        public static Menu PotMenu { get; private set; }

        public static readonly Item Corrupting = new Item(ItemId.Corrupting_Potion);

        public static readonly Item Health = new Item(ItemId.Health_Potion);

        public static readonly Item Hunters = new Item(ItemId.Hunters_Potion);

        public static readonly Item Refillable = new Item((int)ItemId.Refillable_Potion);

        public static readonly Item Biscuit = new Item((int)ItemId.Total_Biscuit_of_Rejuvenation);

        public static int Corruptingh => PotMenu["CPH"].Cast<Slider>().CurrentValue;

        public static int Healthh => PotMenu["HPH"].Cast<Slider>().CurrentValue;

        public static int Huntersh => PotMenu["HPSH"].Cast<Slider>().CurrentValue;

        public static int Refillableh => PotMenu["RPH"].Cast<Slider>().CurrentValue;

        public static int Biscuith => PotMenu["BPH"].Cast<Slider>().CurrentValue;

        public static bool Corruptingc
            =>
                PotMenu["CP"].Cast<CheckBox>().CurrentValue && Corrupting.IsOwned() && Corrupting.IsReady()
                && !Player.Instance.HasBuff(Corrupting.ItemInfo.Name);

        public static bool Healthc
            => PotMenu["HP"].Cast<CheckBox>().CurrentValue && Health.IsOwned() && Health.IsReady() && !Player.Instance.HasBuff(Health.ItemInfo.Name);

        public static bool Huntersc
            =>
                PotMenu["HPS"].Cast<CheckBox>().CurrentValue && Hunters.IsOwned() && Hunters.IsReady()
                && !Player.Instance.HasBuff(Hunters.ItemInfo.Name);

        public static bool Refillablec
            =>
                PotMenu["RP"].Cast<CheckBox>().CurrentValue && Refillable.IsOwned() && Refillable.IsReady()
                && !Player.Instance.HasBuff(Refillable.ItemInfo.Name);

        public static bool Biscuitc
            =>
                PotMenu["BP"].Cast<CheckBox>().CurrentValue && Biscuit.IsOwned() && Biscuit.IsReady()
                && !Player.Instance.HasBuff(Biscuit.ItemInfo.Name);

        internal static void OnLoad()
        {
            PotMenu = Load.UtliMenu.AddSubMenu("Potions");
            PotMenu.AddGroupLabel("General Settings");
            PotMenu.Add("mob", new CheckBox("Use on Minions", false));
            PotMenu.Add("jmob", new CheckBox("Use on Jungle Monsters"));
            PotMenu.Add("champ", new CheckBox("Use On Champions"));
            PotMenu.Add("tower", new CheckBox("Use On Turrets"));
            PotMenu.AddGroupLabel("Potions Settings");
            PotMenu.Add("CP", new CheckBox("Corrupting Potion", false));
            PotMenu.Add("CPH", new Slider("Use On Health %", 65, 0, 100));
            PotMenu.Add("HP", new CheckBox("Health Potion", false));
            PotMenu.Add("HPH", new Slider("Use On health %", 45, 0, 100));
            PotMenu.Add("HPS", new CheckBox("Hunters Potion", false));
            PotMenu.Add("HPSH", new Slider("Use On health %", 75, 0, 100));
            PotMenu.Add("RP", new CheckBox("Refillable Potion", false));
            PotMenu.Add("RPH", new Slider("Use On health %", 50, 0, 100));
            PotMenu.Add("BP", new CheckBox("Biscuit", false));
            PotMenu.Add("BPH", new Slider("Use On health %", 40, 0, 100));

            Obj_AI_Base.OnBasicAttack += Obj_AI_Base_OnBasicAttack;
            Obj_AI_Base.OnProcessSpellCast += Obj_AI_Base_OnProcessSpellCast;
        }

        private static void Obj_AI_Base_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!(args.Target is AIHeroClient))
            {
                return;
            }

            var caster = sender;
            var target = (AIHeroClient)args.Target;
            if (((caster is AIHeroClient && PotMenu["champ"].Cast<CheckBox>().CurrentValue)
                 || (caster is Obj_AI_Minion && caster.IsMinion && PotMenu["mob"].Cast<CheckBox>().CurrentValue)
                 || (caster is Obj_AI_Minion && caster.IsMonster && PotMenu["jmob"].Cast<CheckBox>().CurrentValue)
                 || (caster is Obj_AI_Turret && PotMenu["tower"].Cast<CheckBox>().CurrentValue)) && caster.IsEnemy && target != null && target.IsMe)
            {
                if (!Player.Instance.IsRecalling())
                {
                    if (Refillablec)
                    {
                        if (target.HealthPercent <= Refillableh)
                        {
                            Refillable.Cast();
                        }

                        if (caster.GetAutoAttackDamage(target) >= target.TotalShieldHealth())
                        {
                            Refillable.Cast();
                        }
                    }

                    if (Healthc)
                    {
                        if (target.HealthPercent <= Healthh)
                        {
                            Health.Cast();
                        }

                        if (caster.GetAutoAttackDamage(target) >= target.TotalShieldHealth())
                        {
                            Health.Cast();
                        }
                    }

                    if (Huntersc)
                    {
                        if (target.HealthPercent <= Huntersh)
                        {
                            Hunters.Cast();
                        }

                        if (caster.GetAutoAttackDamage(target) >= target.TotalShieldHealth())
                        {
                            Hunters.Cast();
                        }
                    }

                    if (Biscuitc)
                    {
                        if (target.HealthPercent <= Biscuith)
                        {
                            Biscuit.Cast();
                        }

                        if (caster.GetAutoAttackDamage(target) >= target.TotalShieldHealth())
                        {
                            Biscuit.Cast();
                        }
                    }

                    if (Corruptingc)
                    {
                        if (target.HealthPercent <= Corruptingh)
                        {
                            Corrupting.Cast();
                        }

                        if (caster.GetAutoAttackDamage(target) >= target.TotalShieldHealth())
                        {
                            Corrupting.Cast();
                        }
                    }
                }
            }
        }

        private static void Obj_AI_Base_OnBasicAttack(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!(args.Target is AIHeroClient))
            {
                return;
            }

            var caster = sender;
            var target = (AIHeroClient)args.Target;
            if (((caster is AIHeroClient && PotMenu["champ"].Cast<CheckBox>().CurrentValue)
                 || (caster is Obj_AI_Minion && caster.IsMinion && PotMenu["mob"].Cast<CheckBox>().CurrentValue)
                 || (caster is Obj_AI_Minion && caster.IsMonster && PotMenu["jmob"].Cast<CheckBox>().CurrentValue)
                 || (caster is Obj_AI_Turret && PotMenu["tower"].Cast<CheckBox>().CurrentValue)) && caster.IsEnemy && target != null && target.IsMe)
            {
                if (!Player.Instance.IsRecalling())
                {
                    if (Refillablec)
                    {
                        if (target.HealthPercent <= Refillableh)
                        {
                            Refillable.Cast();
                        }

                        if (caster.GetAutoAttackDamage(target) >= target.TotalShieldHealth())
                        {
                            Refillable.Cast();
                        }
                    }

                    if (Healthc)
                    {
                        if (target.HealthPercent <= Healthh)
                        {
                            Health.Cast();
                        }

                        if (caster.GetAutoAttackDamage(target) >= target.TotalShieldHealth())
                        {
                            Health.Cast();
                        }
                    }

                    if (Huntersc)
                    {
                        if (target.HealthPercent <= Huntersh)
                        {
                            Hunters.Cast();
                        }

                        if (caster.GetAutoAttackDamage(target) >= target.TotalShieldHealth())
                        {
                            Hunters.Cast();
                        }
                    }

                    if (Biscuitc)
                    {
                        if (target.HealthPercent <= Biscuith)
                        {
                            Biscuit.Cast();
                        }

                        if (caster.GetAutoAttackDamage(target) >= target.TotalShieldHealth())
                        {
                            Biscuit.Cast();
                        }
                    }

                    if (Corruptingc)
                    {
                        if (target.HealthPercent <= Corruptingh)
                        {
                            Corrupting.Cast();
                        }

                        if (caster.GetAutoAttackDamage(target) >= target.TotalShieldHealth())
                        {
                            Corrupting.Cast();
                        }
                    }
                }
            }
        }
    }
}