namespace KappaUtility.Items
{
    using System.Linq;

    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Menu.Values;

    using KappaUtility.Summoners;

    internal class AutoTear
    {
        public static readonly Item Tear = new Item(ItemId.Tear_of_the_Goddess);

        public static readonly Item Tearcs = new Item(ItemId.Tear_of_the_Goddess_Crystal_Scar);

        public static readonly Item Arch = new Item(ItemId.Archangels_Staff);

        public static readonly Item Archcs = new Item(ItemId.Archangels_Staff_Crystal_Scar);

        public static readonly Item Mana = new Item(ItemId.Manamune);

        public static readonly Item Manacs = new Item(ItemId.Manamune_Crystal_Scar);

        public static readonly Item Mura = new Item(ItemId.Muramana);

        public static readonly Item Sera = new Item(ItemId.Seraphs_Embrace);

        public static Menu TearMenu { get; private set; }

        internal static void OnLoad()
        {
            TearMenu = Load.UtliMenu.AddSubMenu("AutoTearStacker");
            TearMenu.AddGroupLabel("AutoTear Settings");
            TearMenu.Add(
                Player.Instance.ChampionName + "enable",
                new KeyBind("Enable Toggle", false, KeyBind.BindTypes.PressToggle, 'M'));
            TearMenu.Add(Player.Instance.ChampionName + "shop", new CheckBox("Stack Only In Shop Range", false));
            TearMenu.Add(Player.Instance.ChampionName + "enemy", new CheckBox("Stop Stacking if Enemies Near"));
            TearMenu.AddSeparator();
            TearMenu.AddGroupLabel("Mana Manager");
            TearMenu.Add(Player.Instance.ChampionName + "manasave", new Slider("Save Mana %", 85));
            TearMenu.AddSeparator();
            TearMenu.AddGroupLabel("Item Settings");
            TearMenu.Add(Player.Instance.ChampionName + "tear", new CheckBox("Stack Tear"));
            TearMenu.Add(Player.Instance.ChampionName + "arch", new CheckBox("Stack Archangels Staff"));
            TearMenu.Add(Player.Instance.ChampionName + "mana", new CheckBox("Stack Manamune"));
            TearMenu.AddSeparator();
            TearMenu.AddGroupLabel("Stacking Spell");
            TearMenu.Add(Player.Instance.ChampionName + "Q", new CheckBox("Use Q", false));
            TearMenu.Add(Player.Instance.ChampionName + "W", new CheckBox("Use W", false));
            TearMenu.Add(Player.Instance.ChampionName + "E", new CheckBox("Use E", false));
            TearMenu.Add(Player.Instance.ChampionName + "R", new CheckBox("Use R", false));
        }

        internal static void OnUpdate()
        {
            if (TearMenu[Player.Instance.ChampionName + "enable"].Cast<KeyBind>().CurrentValue)
            {
                var items = ((Tearcs.IsOwned() || Tear.IsOwned())
                             && TearMenu[Player.Instance.ChampionName + "tear"].Cast<CheckBox>().CurrentValue)
                            || ((Arch.IsOwned() || Archcs.IsOwned())
                                && TearMenu[Player.Instance.ChampionName + "arch"].Cast<CheckBox>().CurrentValue)
                            || ((Manacs.IsOwned() || Mana.IsOwned())
                                && TearMenu[Player.Instance.ChampionName + "mana"].Cast<CheckBox>().CurrentValue);
                var items2 = Sera.IsOwned() || Mura.IsOwned();
                var minions = EntityManager.MinionsAndMonsters.EnemyMinions;

                foreach (var minion in
                    minions.Where(
                        minion =>
                        items && !items2
                        && Player.Instance.ManaPercent
                        > TearMenu[Player.Instance.ChampionName + "manasave"].Cast<Slider>().CurrentValue))
                {
                    if (TearMenu[Player.Instance.ChampionName + "enemy"].Cast<CheckBox>().CurrentValue
                        && Player.Instance.CountEnemiesInRange(1250) >= 1)
                    {
                        return;
                    }

                    if (TearMenu[Player.Instance.ChampionName + "shop"].Cast<CheckBox>().CurrentValue
                        && !Player.Instance.IsInShopRange())
                    {
                        return;
                    }

                    if (!Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.None)
                        || Player.Instance.Spellbook.IsChanneling)
                    {
                        return;
                    }

                    Cast(minion);
                }
            }
        }

        internal static void Cast(Obj_AI_Base target)
        {
            var useQ = Player.GetSpell(SpellSlot.Q).IsReady
                       && TearMenu[Player.Instance.ChampionName + "Q"].Cast<CheckBox>().CurrentValue;

            var useW = Player.GetSpell(SpellSlot.W).IsReady
                       && TearMenu[Player.Instance.ChampionName + "W"].Cast<CheckBox>().CurrentValue;

            var useE = Player.GetSpell(SpellSlot.E).IsReady
                       && TearMenu[Player.Instance.ChampionName + "E"].Cast<CheckBox>().CurrentValue;

            var useR = Player.GetSpell(SpellSlot.R).IsReady
                       && TearMenu[Player.Instance.ChampionName + "R"].Cast<CheckBox>().CurrentValue;
            if (useQ)
            {
                Player.CastSpell(SpellSlot.Q, Game.CursorPos);
            }

            if (useW)
            {
                Player.CastSpell(SpellSlot.W, Game.CursorPos);
            }

            if (useE)
            {
                Player.CastSpell(SpellSlot.E, Game.CursorPos);
            }

            if (useR)
            {
                Player.CastSpell(SpellSlot.R, Game.CursorPos);
            }
        }
    }
}