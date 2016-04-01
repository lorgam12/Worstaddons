namespace KappaUtility.Summoners
{
    using System.Linq;

    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Menu.Values;
    using EloBuddy.SDK.Rendering;

    using SharpDX;

    internal class Spells
    {
        public static Spell.Active Heal;

        public static Spell.Active Barrier;

        public static Spell.Targeted Ignite;

        public static Spell.Targeted Smite;

        public static Spell.Targeted Exhaust;

        public static Menu SummMenu { get; private set; }

        internal static void OnLoad()
        {
            Obj_AI_Base.OnProcessSpellCast += OnProcessSpellCast;
            Obj_AI_Base.OnBasicAttack += OnBasicAttack;

            SummMenu = Load.UtliMenu.AddSubMenu("Summoner Spells");
            SummMenu.AddGroupLabel("Summoner Spells Settings");

            if (Player.Spells.FirstOrDefault(o => o.SData.Name.Contains("SummonerDot")) != null)
            {
                SummMenu.AddGroupLabel("Ignite Settings");
                SummMenu.Add(
                    Player.Instance.ChampionName + "EnableIgnite",
                    new KeyBind("Enable Ignite Toggle", false, KeyBind.BindTypes.PressToggle, 'M'));
                SummMenu.Add(
                    Player.Instance.ChampionName + "EnableactiveIgnite",
                    new KeyBind("Enable Ignite Active", false, KeyBind.BindTypes.HoldActive));
                SummMenu.Add("drawIgnite", new CheckBox("Draw Ignite range", false));
                SummMenu.AddGroupLabel("Don't Use Ignite On:");
                foreach (var enemy in ObjectManager.Get<AIHeroClient>())
                {
                    CheckBox cb = new CheckBox(enemy.BaseSkinName) { CurrentValue = false };
                    if (enemy.Team != Player.Instance.Team)
                    {
                        SummMenu.Add("DontIgnite" + enemy.BaseSkinName, cb);
                    }
                }

                SummMenu.AddSeparator();
                Ignite = new Spell.Targeted(Player.Instance.GetSpellSlotFromName("SummonerDot"), 600);
            }

            if (Player.Spells.FirstOrDefault(o => o.SData.Name.Contains("SummonerBarrier")) != null)
            {
                SummMenu.AddGroupLabel("Barrier Settings");
                SummMenu.Add(
                    Player.Instance.ChampionName + "EnableBarrier",
                    new KeyBind("Enable Barrier Toggle", false, KeyBind.BindTypes.PressToggle, 'M'));
                SummMenu.Add(
                    Player.Instance.ChampionName + "EnableactiveBarrier",
                    new KeyBind("Enable Barrier Active", false, KeyBind.BindTypes.HoldActive));
                SummMenu.Add("barrierme", new Slider("Use On My Health %", 30, 0, 100));
                SummMenu.AddSeparator();
                Barrier = new Spell.Active(Player.Instance.GetSpellSlotFromName("SummonerBarrier"));
            }

            if (Player.Spells.FirstOrDefault(o => o.SData.Name.Contains("SummonerHeal")) != null)
            {
                SummMenu.AddGroupLabel("Heal Settings");
                SummMenu.Add(
                    Player.Instance.ChampionName + "EnableHeal",
                    new KeyBind("Enable Heal Toggle", false, KeyBind.BindTypes.PressToggle, 'M'));
                SummMenu.Add(
                    Player.Instance.ChampionName + "EnableactiveHeal",
                    new KeyBind("Enable Heal Active", false, KeyBind.BindTypes.HoldActive));
                SummMenu.Add("drawHeal", new CheckBox("Draw Heal range", false));
                SummMenu.Add("Healally", new Slider("Use On Ally Health %", 25, 0, 100));
                SummMenu.Add("Healme", new Slider("Use On My Health %", 30, 0, 100));
                SummMenu.AddGroupLabel("Don't Use Heal On:");
                foreach (var ally in ObjectManager.Get<AIHeroClient>())
                {
                    CheckBox cb = new CheckBox(ally.BaseSkinName) { CurrentValue = false };
                    if (ally.Team == Player.Instance.Team)
                    {
                        SummMenu.Add("DontHeal" + ally.BaseSkinName, cb);
                    }
                }

                SummMenu.AddSeparator();
                Heal = new Spell.Active(Player.Instance.GetSpellSlotFromName("SummonerHeal"), 850);
            }

            if (Player.Spells.FirstOrDefault(o => o.SData.Name.Contains("SummonerExhaust")) != null)
            {
                SummMenu.AddGroupLabel("Exhaust Settings");
                SummMenu.Add(
                    Player.Instance.ChampionName + "EnableExhaust",
                    new KeyBind("Enable Exhaust Toggle", true, KeyBind.BindTypes.PressToggle, 'M'));
                SummMenu.Add(
                    Player.Instance.ChampionName + "EnableactiveExhaust",
                    new KeyBind("Enable Exhaust Active", false, KeyBind.BindTypes.HoldActive));
                SummMenu.Add("drawExhaust", new CheckBox("Draw Exhaust range", false));
                SummMenu.Add("exhaustally", new Slider("Use When Ally/Self Health %", 35, 0, 100));
                SummMenu.Add("exhaustenemy", new Slider("Use When Enemy Health %", 40, 0, 100));
                SummMenu.AddGroupLabel("Don't Use Exhaust On:");
                foreach (var enemy in ObjectManager.Get<AIHeroClient>())
                {
                    var cb = new CheckBox(enemy.BaseSkinName) { CurrentValue = false };
                    if (enemy.Team != Player.Instance.Team)
                    {
                        SummMenu.Add("DontExhaust" + enemy.BaseSkinName, cb);
                    }
                }

                Exhaust = new Spell.Targeted(Player.Instance.GetSpellSlotFromName("SummonerExhaust"), 650);
            }

            if (Player.Spells.FirstOrDefault(o => o.SData.Name.Contains("SummonerSmite")) != null)
            {
                SummMenu.AddGroupLabel("Smite Settings");
                SummMenu.Add(
                    Player.Instance.ChampionName + "EnableSmite",
                    new KeyBind("Enable Smite Toggle", true, KeyBind.BindTypes.PressToggle, 'M'));
                SummMenu.Add(
                    Player.Instance.ChampionName + "EnableactiveSmite",
                    new KeyBind("Enable Smite Active", false, KeyBind.BindTypes.HoldActive));
                SummMenu.Add("smitemob", new CheckBox("Smite Monsters", false));
                SummMenu.Add("smitecombo", new CheckBox("Smite Combo", false));
                SummMenu.Add("smiteks", new CheckBox("Smite KillSteal", false));
                SummMenu.Add("drawSmite", new CheckBox("Draw Smite range", false));
                SummMenu.AddGroupLabel("Don't Use Smite On:");
                foreach (var enemy in ObjectManager.Get<AIHeroClient>())
                {
                    var cb = new CheckBox(enemy.BaseSkinName) { CurrentValue = false };
                    if (enemy.Team != Player.Instance.Team)
                    {
                        SummMenu.Add("DontSmite" + enemy.BaseSkinName, cb);
                    }
                }

                SummMenu.AddGroupLabel("Use Smite On Monster:");
                SummMenu.Add("blue", new CheckBox(" Blue "));
                SummMenu.Add("red", new CheckBox(" Red "));
                SummMenu.Add("baron", new CheckBox(" Baron "));
                SummMenu.Add("drake", new CheckBox(" Dragon "));
                SummMenu.Add("gromp", new CheckBox(" Gromp "));
                SummMenu.Add("krug", new CheckBox(" Krug "));
                SummMenu.Add("razorbeak", new CheckBox(" Razorbeak "));
                SummMenu.Add("crab", new CheckBox(" Crab "));
                SummMenu.Add("murkwolf", new CheckBox(" Murkwolf "));
                SummMenu.AddSeparator();
                Smite = new Spell.Targeted(Player.Instance.GetSpellSlotFromName("SummonerSmite"), 500);
            }
        }

        internal static void Drawings()
        {
            if (Ignite != null)
            {
                if (SummMenu["drawIgnite"].Cast<CheckBox>().CurrentValue
                    && (SummMenu[Player.Instance.ChampionName + "EnableactiveIgnite"].Cast<KeyBind>().CurrentValue
                        || SummMenu[Player.Instance.ChampionName + "EnableIgnite"].Cast<KeyBind>().CurrentValue))
                {
                    Circle.Draw(Ignite.IsReady() ? Color.LightBlue : Color.Red, Ignite.Range, Player.Instance.Position);
                }
            }
            if (Heal != null)
            {
                if (SummMenu["drawheal"].Cast<CheckBox>().CurrentValue
                    && (SummMenu[Player.Instance.ChampionName + "EnableactiveHeal"].Cast<KeyBind>().CurrentValue
                        || SummMenu[Player.Instance.ChampionName + "EnableHeal"].Cast<KeyBind>().CurrentValue))
                {
                    Circle.Draw(Heal.IsReady() ? Color.LightBlue : Color.Red, Heal.Range, Player.Instance.Position);
                }
            }
            if (Smite != null)
            {
                if (SummMenu["drawSmite"].Cast<CheckBox>().CurrentValue
                    && (SummMenu[Player.Instance.ChampionName + "EnableactiveSmite"].Cast<KeyBind>().CurrentValue
                        || SummMenu[Player.Instance.ChampionName + "EnableSmite"].Cast<KeyBind>().CurrentValue))
                {
                    Circle.Draw(Smite.IsReady() ? Color.LightBlue : Color.Red, Smite.Range, Player.Instance.Position);
                }
            }

            if (Exhaust != null)
            {
                if (SummMenu["drawExhaust"].Cast<CheckBox>().CurrentValue
                    && (SummMenu[Player.Instance.ChampionName + "EnableactiveExhaust"].Cast<KeyBind>().CurrentValue
                        || SummMenu[Player.Instance.ChampionName + "EnableExhaust"].Cast<KeyBind>().CurrentValue))
                {
                    Circle.Draw(
                        Exhaust.IsReady() ? Color.LightBlue : Color.Red,
                        Exhaust.Range,
                        Player.Instance.Position);
                }
            }
        }

        public static void Cast()
        {
            var target =
                ObjectManager.Get<AIHeroClient>()
                    .FirstOrDefault(enemy => enemy.IsValid && enemy.IsEnemy && enemy.IsVisible);

            var ally = ObjectManager.Get<AIHeroClient>().FirstOrDefault(a => a.IsValid && a.IsAlly && a.IsVisible);

            if (Ignite != null)
            {
                var ignitec =
                    (SummMenu[Player.Instance.ChampionName + "EnableactiveIgnite"].Cast<KeyBind>().CurrentValue
                     || SummMenu[Player.Instance.ChampionName + "EnableIgnite"].Cast<KeyBind>().CurrentValue)
                    && Ignite.IsReady();

                if (ignitec && target != null
                    && Player.Instance.GetSummonerSpellDamage(target, DamageLibrary.SummonerSpells.Ignite)
                    >= target.TotalShieldHealth() + (target.HPRegenRate * 3))
                {
                    if (target.IsValidTarget(Ignite.Range) && !target.IsDead
                        && !SummMenu["DontIgnite" + target.BaseSkinName].Cast<CheckBox>().CurrentValue)
                    {
                        Ignite.Cast(target);
                    }
                }
            }

            if (Exhaust != null)
            {
                var exhaustc =
                    (SummMenu[Player.Instance.ChampionName + "EnableactiveExhaust"].Cast<KeyBind>().CurrentValue
                     || SummMenu[Player.Instance.ChampionName + "EnableExhaust"].Cast<KeyBind>().CurrentValue)
                    && Exhaust.IsReady();
                var Exhaustally = SummMenu["exhaustally"].Cast<Slider>().CurrentValue;
                var Exhaustenemy = SummMenu["exhaustenemy"].Cast<Slider>().CurrentValue;

                if (exhaustc && target != null
                    && (target.IsValidTarget(Exhaust.Range)
                        && !SummMenu["DontExhaust" + target.BaseSkinName].Cast<CheckBox>().CurrentValue))
                {
                    if (target.HealthPercent <= Exhaustenemy)
                    {
                        Exhaust.Cast(target);
                    }

                    if (ally != null && ally.HealthPercent <= Exhaustally)
                    {
                        Exhaust.Cast(target);
                    }
                }
            }
        }

        public static void OnBasicAttack(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!(args.Target is AIHeroClient))
            {
                return;
            }

            var caster = sender;
            var target = (AIHeroClient)args.Target;

            if ((caster is AIHeroClient || caster is Obj_AI_Turret) && caster != null && target != null
                && caster.IsEnemy)
            {
                if (target.IsAlly && !target.IsMe)
                {
                    if (Exhaust != null)
                    {
                        var exhaustc =
                            (SummMenu[Player.Instance.ChampionName + "EnableactiveExhaust"].Cast<KeyBind>().CurrentValue
                             || SummMenu[Player.Instance.ChampionName + "EnableExhaust"].Cast<KeyBind>().CurrentValue)
                            && Exhaust.IsReady();
                        var Exhaustally = SummMenu["exhaustally"].Cast<Slider>().CurrentValue;
                        var Exhaustenemy = SummMenu["exhaustenemy"].Cast<Slider>().CurrentValue;

                        if (exhaustc
                            && (target.IsValidTarget(Exhaust.Range)
                                && !SummMenu["DontExhaust" + caster.BaseSkinName].Cast<CheckBox>().CurrentValue))
                        {
                            if (target.HealthPercent <= Exhaustenemy)
                            {
                                Exhaust.Cast(caster);
                            }

                            if (target.HealthPercent <= Exhaustally)
                            {
                                Exhaust.Cast(caster);
                            }
                        }
                    }

                    if (Heal != null && !SummMenu["DontHeal" + target.BaseSkinName].Cast<CheckBox>().CurrentValue)
                    {
                        var healc =
                            (SummMenu[Player.Instance.ChampionName + "EnableactiveHeal"].Cast<KeyBind>().CurrentValue
                             || SummMenu[Player.Instance.ChampionName + "EnableHeal"].Cast<KeyBind>().CurrentValue)
                            && Heal.IsReady();
                        var healally = SummMenu["Healally"].Cast<Slider>().CurrentValue;
                        if (healc)
                        {
                            if (target.IsInRange(Player.Instance, Heal.Range))
                            {
                                if (target.HealthPercent <= healally)
                                {
                                    Heal.Cast();
                                }

                                if (caster.GetAutoAttackDamage(target) > target.TotalShieldHealth())
                                {
                                    Heal.Cast();
                                }
                            }
                        }
                    }
                }

                if (target.IsMe)
                {
                    if (Heal != null && !SummMenu["DontHeal" + target.BaseSkinName].Cast<CheckBox>().CurrentValue)
                    {
                        var healc =
                            (SummMenu[Player.Instance.ChampionName + "EnableactiveHeal"].Cast<KeyBind>().CurrentValue
                             || SummMenu[Player.Instance.ChampionName + "EnableHeal"].Cast<KeyBind>().CurrentValue)
                            && Heal.IsReady();
                        var healme = SummMenu["Healme"].Cast<Slider>().CurrentValue;
                        if (healc)
                        {
                            if (target.HealthPercent <= healme)
                            {
                                Heal.Cast();
                            }

                            if (caster.GetAutoAttackDamage(target) > target.TotalShieldHealth())
                            {
                                Heal.Cast();
                            }
                        }
                    }

                    if (Exhaust != null)
                    {
                        var exhaustc =
                            (SummMenu[Player.Instance.ChampionName + "EnableactiveExhaust"].Cast<KeyBind>().CurrentValue
                             || SummMenu[Player.Instance.ChampionName + "EnableExhaust"].Cast<KeyBind>().CurrentValue)
                            && Exhaust.IsReady();
                        var Exhaustally = SummMenu["exhaustally"].Cast<Slider>().CurrentValue;
                        var Exhaustenemy = SummMenu["exhaustenemy"].Cast<Slider>().CurrentValue;
                        if (exhaustc && !SummMenu["DontExhaust" + caster.BaseSkinName].Cast<CheckBox>().CurrentValue)
                        {
                            if (target.HealthPercent <= Exhaustenemy)
                            {
                                Exhaust.Cast(caster);
                            }

                            if (target.HealthPercent <= Exhaustally)
                            {
                                Exhaust.Cast(caster);
                            }
                        }
                    }

                    if (Barrier != null)
                    {
                        var barrierc =
                            (SummMenu[Player.Instance.ChampionName + "EnableactiveBarrier"].Cast<KeyBind>().CurrentValue
                             || SummMenu[Player.Instance.ChampionName + "EnableBarrier"].Cast<KeyBind>().CurrentValue)
                            && Barrier.IsReady();
                        var barrierme = SummMenu["barrierme"].Cast<Slider>().CurrentValue;
                        if (barrierc)
                        {
                            if (target.HealthPercent <= barrierme)
                            {
                                Barrier.Cast();
                            }

                            if (caster.GetAutoAttackDamage(target) > target.TotalShieldHealth())
                            {
                                Barrier.Cast();
                            }
                        }
                    }
                }
            }
        }

        public static void OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!(args.Target is AIHeroClient))
            {
                return;
            }

            var caster = sender;
            var target = (AIHeroClient)args.Target;

            if ((caster is AIHeroClient || caster is Obj_AI_Turret) && caster != null && target != null
                && caster.IsEnemy)
            {
                if (target.IsAlly && !target.IsMe)
                {
                    if (Heal != null && !SummMenu["DontHeal" + target.BaseSkinName].Cast<CheckBox>().CurrentValue)
                    {
                        var healc =
                            (SummMenu[Player.Instance.ChampionName + "EnableactiveHeal"].Cast<KeyBind>().CurrentValue
                             || SummMenu[Player.Instance.ChampionName + "EnableHeal"].Cast<KeyBind>().CurrentValue)
                            && Heal.IsReady();
                        var healally = SummMenu["Healally"].Cast<Slider>().CurrentValue;
                        if (healc)
                        {
                            if (target.IsInRange(Player.Instance, Heal.Range))
                            {
                                if (target.HealthPercent <= healally)
                                {
                                    Heal.Cast();
                                }

                                if (caster.BaseAttackDamage > target.TotalShieldHealth()
                                    || caster.BaseAbilityDamage > target.TotalShieldHealth())
                                {
                                    Heal.Cast();
                                }
                            }
                        }
                    }

                    if (Exhaust != null)
                    {
                        var exhaustc =
                            (SummMenu[Player.Instance.ChampionName + "EnableactiveExhaust"].Cast<KeyBind>().CurrentValue
                             || SummMenu[Player.Instance.ChampionName + "EnableExhaust"].Cast<KeyBind>().CurrentValue)
                            && Exhaust.IsReady();
                        var Exhaustally = SummMenu["exhaustally"].Cast<Slider>().CurrentValue;
                        var Exhaustenemy = SummMenu["exhaustenemy"].Cast<Slider>().CurrentValue;
                        if (exhaustc && !SummMenu["DontExhaust" + caster.BaseSkinName].Cast<CheckBox>().CurrentValue)
                        {
                            if (target.HealthPercent <= Exhaustenemy)
                            {
                                Exhaust.Cast(caster);
                            }

                            if (target.HealthPercent <= Exhaustally)
                            {
                                Exhaust.Cast(caster);
                            }
                        }
                    }
                }

                if (target.IsMe)
                {
                    if (Heal != null && !SummMenu["DontHeal" + target.BaseSkinName].Cast<CheckBox>().CurrentValue)
                    {
                        var healc =
                            (SummMenu[Player.Instance.ChampionName + "EnableactiveHeal"].Cast<KeyBind>().CurrentValue
                             || SummMenu[Player.Instance.ChampionName + "EnableHeal"].Cast<KeyBind>().CurrentValue)
                            && Heal.IsReady();
                        var healme = SummMenu["Healme"].Cast<Slider>().CurrentValue;
                        if (healc)
                        {
                            if (target.HealthPercent <= healme)
                            {
                                Heal.Cast();
                            }

                            if (caster.BaseAttackDamage > target.TotalShieldHealth()
                                || caster.BaseAbilityDamage > target.TotalShieldHealth())
                            {
                                Heal.Cast();
                            }
                        }

                        if (Exhaust != null)
                        {
                            var exhaustc =
                                (SummMenu[Player.Instance.ChampionName + "EnableactiveExhaust"].Cast<KeyBind>()
                                     .CurrentValue
                                 || SummMenu[Player.Instance.ChampionName + "EnableExhaust"].Cast<KeyBind>()
                                        .CurrentValue) && Exhaust.IsReady();
                            var Exhaustally = SummMenu["exhaustally"].Cast<Slider>().CurrentValue;
                            var Exhaustenemy = SummMenu["exhaustenemy"].Cast<Slider>().CurrentValue;
                            if (exhaustc && !SummMenu["DontExhaust" + caster.BaseSkinName].Cast<CheckBox>().CurrentValue)
                            {
                                if (target.HealthPercent <= Exhaustenemy)
                                {
                                    Exhaust.Cast(caster);
                                }

                                if (target.HealthPercent <= Exhaustally)
                                {
                                    Exhaust.Cast(caster);
                                }
                            }
                        }
                    }

                    if (Barrier != null)
                    {
                        var barrierc =
                            (SummMenu[Player.Instance.ChampionName + "EnableactiveBarrier"].Cast<KeyBind>().CurrentValue
                             || SummMenu[Player.Instance.ChampionName + "EnableBarrier"].Cast<KeyBind>().CurrentValue)
                            && Barrier.IsReady();
                        var barrierme = SummMenu["barrierme"].Cast<Slider>().CurrentValue;
                        if (barrierc)
                        {
                            if (target.HealthPercent <= barrierme)
                            {
                                Barrier.Cast();
                            }

                            if (caster.BaseAttackDamage > target.TotalShieldHealth()
                                || caster.BaseAbilityDamage > target.TotalShieldHealth())
                            {
                                Barrier.Cast();
                            }
                        }
                    }
                }
            }
        }
    }
}