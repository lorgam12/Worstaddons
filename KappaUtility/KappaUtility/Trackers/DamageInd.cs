namespace KappaUtility.Trackers
{
    using System.Drawing;
    using System.Linq;

    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Menu.Values;

    internal class DamageInd
    {
        private static readonly string[] notsupported = { "TahmKench", "Kindred", "Illaoi", "Jhin" };

        public static Menu DmgMenu { get; private set; }

        public static HpBarIndicator Hpi = new HpBarIndicator();

        private static string champion;

        internal static void OnLoad()
        {
            champion = Player.Instance.ChampionName;
            DmgMenu = Load.UtliMenu.AddSubMenu("DamageIndicator");
            DmgMenu.AddGroupLabel("Damage Indicator Settings");
            if (notsupported.Contains(champion))
            {
                DmgMenu.AddLabel(champion + " Is Not Supported From The DamageLibrary :(");
                DmgMenu.AddSeparator();
            }

            DmgMenu.Add(champion + "dmg", new CheckBox("Draw Damage Indicator", false));
            DmgMenu.Add(champion + "Q", new CheckBox("Draw Q Damage", false));
            DmgMenu.Add(champion + "W", new CheckBox("Draw W Damage", false));
            DmgMenu.Add(champion + "E", new CheckBox("Draw E Damage", false));
            DmgMenu.Add(champion + "R", new CheckBox("Draw R Damage"));
            DmgMenu.Add(champion + "Killable", new CheckBox("Draw Killable", false));
            DmgMenu.AddGroupLabel("Don't Draw On:");
            foreach (var enemy in ObjectManager.Get<AIHeroClient>())
            {
                CheckBox cb = new CheckBox(enemy.BaseSkinName) { CurrentValue = false };
                if (enemy.Team != Player.Instance.Team)
                {
                    DmgMenu.Add("DontDraw" + enemy.BaseSkinName, cb);
                }
            }
        }

        public static void Damage()
        {
            foreach (var enemy in
                ObjectManager.Get<AIHeroClient>()
                    .Where(
                        ene =>
                        ene != null && !ene.IsDead && ene.IsEnemy && ene.IsVisible && ene.IsValid && ene.IsHPBarRendered
                        && !DmgMenu["DontDraw" + ene.BaseSkinName].Cast<CheckBox>().CurrentValue))
            {
                if (DmgMenu[champion + "dmg"].Cast<CheckBox>().CurrentValue)
                {
                    Hpi.unit = enemy;
                    Hpi.drawDmg(CalcDamage(enemy), Color.FromArgb(214, 164, 39));
                }

                if (DmgMenu[champion + "Killable"].Cast<CheckBox>().CurrentValue)
                {
                    var hpPos = enemy.HPBarPosition;
                    if (CalcDamage(enemy) > enemy.TotalShieldHealth())
                    {
                        Drawing.DrawText(hpPos.X + 135f, hpPos.Y, Color.FromArgb(231, 21, 21), "Killable", 2);
                    }
                }
            }
        }

        public static int CalcDamage(Obj_AI_Base target)
        {
            var aa = ObjectManager.Player.GetAutoAttackDamage(target, true);
            var damage = aa;

            if (Player.Instance.Spellbook.GetSpell(SpellSlot.Q).IsReady
                && DmgMenu[champion + "Q"].Cast<CheckBox>().CurrentValue)
            {
                // Q damage
                damage += ObjectManager.Player.GetSpellDamage(target, SpellSlot.Q);
            }

            if (Player.Instance.Spellbook.GetSpell(SpellSlot.W).IsReady
                && DmgMenu[champion + "W"].Cast<CheckBox>().CurrentValue)
            {
                // W damage
                damage += ObjectManager.Player.GetSpellDamage(target, SpellSlot.W);
            }

            if (Player.Instance.Spellbook.GetSpell(SpellSlot.E).IsReady
                && DmgMenu[champion + "E"].Cast<CheckBox>().CurrentValue)
            {
                // E damage
                damage += ObjectManager.Player.GetSpellDamage(target, SpellSlot.E);
            }

            if (Player.Instance.Spellbook.GetSpell(SpellSlot.R).IsReady
                && DmgMenu[champion + "R"].Cast<CheckBox>().CurrentValue)
            {
                // R damage
                damage += ObjectManager.Player.GetSpellDamage(target, SpellSlot.R);
            }

            return (int)damage;
        }
    }
}