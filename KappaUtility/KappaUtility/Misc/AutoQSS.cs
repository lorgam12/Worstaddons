﻿namespace KappaUtility.Misc
{
    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Menu.Values;

    internal class AutoQSS
    {
        protected static readonly Item Mercurial_Scimitar = new Item(ItemId.Mercurial_Scimitar);

        protected static readonly Item Quicksilver_Sash = new Item(ItemId.Quicksilver_Sash);

        public static Menu QssMenu { get; private set; }

        internal static void OnLoad()
        {
            QssMenu = Load.UtliMenu.AddSubMenu("AutoQSS");
            QssMenu.AddGroupLabel("AutoQSS Settings");
            QssMenu.Add("enable", new CheckBox("Enable", false));
            QssMenu.Add("Mercurial", new CheckBox("Use Mercurial Scimitar"));
            QssMenu.Add("Quicksilver", new CheckBox("Use Quicksilver Sash"));
            QssMenu.AddLabel("Cleanse Settings:");
            QssMenu.Add("blind", new CheckBox("Use On Blinds?", false));
            QssMenu.Add("charm", new CheckBox("Use On Charms?"));
            QssMenu.Add("disarm", new CheckBox("Use On Disarm?", false));
            QssMenu.Add("fear", new CheckBox("Use On Fear?"));
            QssMenu.Add("frenzy", new CheckBox("Use On Frenzy?", false));
            QssMenu.Add("silence", new CheckBox("Use On Silence?", false));
            QssMenu.Add("snare", new CheckBox("Use On Snare?"));
            QssMenu.Add("sleep", new CheckBox("Use On Sleep?"));
            QssMenu.Add("stun", new CheckBox("Use On Stuns?"));
            QssMenu.Add("supperss", new CheckBox("Use On Supperss?"));
            QssMenu.Add("slow", new CheckBox("Use On Slows?", false));
            QssMenu.Add("knockup", new CheckBox("Use On Knock Ups?"));
            QssMenu.Add("knockback", new CheckBox("Use On Knock Backs?"));
            QssMenu.Add("nearsight", new CheckBox("Use On NearSight?", false));
            QssMenu.Add("root", new CheckBox("Use On Roots?"));
            QssMenu.Add("tunt", new CheckBox("Use On Taunts?"));
            QssMenu.Add("poly", new CheckBox("Use On Polymorph?"));
            QssMenu.Add("poison", new CheckBox("Use On Poisons?", false));
            QssMenu.Add("hp", new Slider("Use Only When HP is Under %", 25, 0, 100));
            QssMenu.Add("human", new Slider("Humanizer Delay", 150, 0, 1500));
            QssMenu.Add("Rene", new Slider("Enemies Near to Cast", 1, 0, 5));
            QssMenu.Add("enemydetect", new Slider("Enemies Detect Range", 1000, 0, 2000));
        }

        public static void Clean()
        {
            if (QssMenu["enable"].Cast<CheckBox>().CurrentValue)
            {
                var debuff = (QssMenu["charm"].Cast<CheckBox>().CurrentValue && Player.Instance.IsCharmed)
                             || (QssMenu["root"].Cast<CheckBox>().CurrentValue && Player.Instance.IsRooted)
                             || (QssMenu["tunt"].Cast<CheckBox>().CurrentValue && Player.Instance.IsTaunted)
                             || (QssMenu["stun"].Cast<CheckBox>().CurrentValue && Player.Instance.IsStunned)
                             || (QssMenu["fear"].Cast<CheckBox>().CurrentValue
                                 && Player.Instance.HasBuffOfType(BuffType.Fear))
                             || (QssMenu["silence"].Cast<CheckBox>().CurrentValue
                                 && Player.Instance.HasBuffOfType(BuffType.Silence))
                             || (QssMenu["snare"].Cast<CheckBox>().CurrentValue
                                 && Player.Instance.HasBuffOfType(BuffType.Snare))
                             || (QssMenu["supperss"].Cast<CheckBox>().CurrentValue
                                 && Player.Instance.HasBuffOfType(BuffType.Suppression))
                             || (QssMenu["sleep"].Cast<CheckBox>().CurrentValue
                                 && Player.Instance.HasBuffOfType(BuffType.Sleep))
                             || (QssMenu["poly"].Cast<CheckBox>().CurrentValue
                                 && Player.Instance.HasBuffOfType(BuffType.Polymorph))
                             || (QssMenu["frenzy"].Cast<CheckBox>().CurrentValue
                                 && Player.Instance.HasBuffOfType(BuffType.Frenzy))
                             || (QssMenu["disarm"].Cast<CheckBox>().CurrentValue
                                 && Player.Instance.HasBuffOfType(BuffType.Disarm))
                             || (QssMenu["nearsight"].Cast<CheckBox>().CurrentValue
                                 && Player.Instance.HasBuffOfType(BuffType.NearSight))
                             || (QssMenu["knockback"].Cast<CheckBox>().CurrentValue
                                 && Player.Instance.HasBuffOfType(BuffType.Knockback))
                             || (QssMenu["knockup"].Cast<CheckBox>().CurrentValue
                                 && Player.Instance.HasBuffOfType(BuffType.Knockup))
                             || (QssMenu["slow"].Cast<CheckBox>().CurrentValue
                                 && Player.Instance.HasBuffOfType(BuffType.Slow))
                             || (QssMenu["poison"].Cast<CheckBox>().CurrentValue
                                 && Player.Instance.HasBuffOfType(BuffType.Poison))
                             || (QssMenu["blind"].Cast<CheckBox>().CurrentValue
                                 && Player.Instance.HasBuffOfType(BuffType.Blind));
                var enemys = QssMenu["Rene"].Cast<Slider>().CurrentValue;
                var hp = QssMenu["hp"].Cast<Slider>().CurrentValue;
                var enemysrange = QssMenu["enemydetect"].Cast<Slider>().CurrentValue;
                if (debuff && ObjectManager.Player.HealthPercent <= hp
                    && enemys >= ObjectManager.Player.Position.CountEnemiesInRange(enemysrange))
                {
                    Core.DelayAction(QssCast, QssMenu["human"].Cast<Slider>().CurrentValue);
                }
            }
        }

        public static void QssCast()
        {
            if (Mercurial_Scimitar.IsOwned() && Mercurial_Scimitar.IsReady()
                && QssMenu["Mercurial"].Cast<CheckBox>().CurrentValue)
            {
                if (Mercurial_Scimitar.Cast())
                {
                    return;
                }
            }

            if (Quicksilver_Sash.IsOwned() && Quicksilver_Sash.IsReady()
                && QssMenu["Quicksilver"].Cast<CheckBox>().CurrentValue)
            {
                Quicksilver_Sash.Cast();
            }
        }
    }
}