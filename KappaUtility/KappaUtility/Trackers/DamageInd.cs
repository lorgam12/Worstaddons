namespace KappaUtility.Trackers
{
    using System;
    using System.Linq;

    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Menu.Values;

    internal class DamageInd
    {
        public static Menu DmgMenu { get; private set; }

        public static HpBarIndicator Hpi = new HpBarIndicator();

        internal static void OnLoad()
        {
            DmgMenu = Load.UtliMenu.AddSubMenu("Tracker");
            DmgMenu.AddGroupLabel("Tracker Settings");
            DmgMenu.Add("dmg", new CheckBox("Draw Damage Indicator"));
            DmgMenu.Add("Q", new CheckBox("Draw Q Damage"));
            DmgMenu.Add("W", new CheckBox("Draw W Damage"));
            DmgMenu.Add("E", new CheckBox("Draw E Damage"));
            DmgMenu.Add("R", new CheckBox("Draw R Damage"));
            DmgMenu.Add("Killable", new CheckBox("Draw Killable"));
            DmgMenu.AddGroupLabel("Don't Draw On:");
            foreach (var enemy in ObjectManager.Get<AIHeroClient>())
            {
                CheckBox cb = new CheckBox(enemy.BaseSkinName) { CurrentValue = false };
                if (enemy.Team != Player.Instance.Team)
                {
                    DmgMenu.Add("DontDraw" + enemy.BaseSkinName, cb);
                }
            }

            Drawing.OnEndScene += Drawing_OnEndScene;
        }

        private static void Drawing_OnEndScene(EventArgs args)
        {
            foreach (var enemy in
                ObjectManager.Get<AIHeroClient>()
                    .Where(
                        ene =>
                        ene != null && !ene.IsDead && ene.IsEnemy && ene.IsVisible && ene.IsValid && ene.IsHPBarRendered
                        && !DmgMenu["DontDraw" + ene.BaseSkinName].Cast<CheckBox>().CurrentValue))
            {
                if (DmgMenu["dmg"].Cast<CheckBox>().CurrentValue)
                {
                    Hpi.unit = enemy;
                    Hpi.drawDmg(CalcDamage(enemy), System.Drawing.Color.Goldenrod);
                }
                if (DmgMenu["Killable"].Cast<CheckBox>().CurrentValue)
                {
                    var hpPos = enemy.HPBarPosition;
                    if (CalcDamage(enemy) > enemy.TotalShieldHealth())
                    {
                        Drawing.DrawText(
                            hpPos.X + 135f,
                            hpPos.Y,
                            System.Drawing.Color.FromArgb(231, 21, 21),
                            "Killable",
                            2);
                    }
                }
            }
        }

        private static int CalcDamage(Obj_AI_Base target)
        {
            var aa = ObjectManager.Player.GetAutoAttackDamage(target, true);
            var damage = aa;

            if (Player.Instance.Spellbook.GetSpell(SpellSlot.R).IsReady && DmgMenu["R"].Cast<CheckBox>().CurrentValue)
                // R damage
            {
                damage += ObjectManager.Player.GetSpellDamage(target, SpellSlot.R);
            }

            if (Player.Instance.Spellbook.GetSpell(SpellSlot.Q).IsReady && DmgMenu["Q"].Cast<CheckBox>().CurrentValue)
                // Q damage
            {
                damage += ObjectManager.Player.GetSpellDamage(target, SpellSlot.Q);
            }

            if (Player.Instance.Spellbook.GetSpell(SpellSlot.E).IsReady && DmgMenu["E"].Cast<CheckBox>().CurrentValue)
                // E damage
            {
                damage += ObjectManager.Player.GetSpellDamage(target, SpellSlot.E);
            }

            if (Player.Instance.Spellbook.GetSpell(SpellSlot.W).IsReady && DmgMenu["W"].Cast<CheckBox>().CurrentValue)
                // W damage
            {
                damage += ObjectManager.Player.GetSpellDamage(target, SpellSlot.W);
            }

            return (int)damage;
        }
    }
}