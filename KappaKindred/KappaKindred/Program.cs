namespace KappaKindred
{
    using System;

    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Events;

    using Events;

    internal class Program
    {
        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            Menu.Load();
            Spells.Load();

            Game.OnUpdate += OnUpdate.Update;
            Drawing.OnDraw += OnDraw.Draw;
            Orbwalker.OnPostAttack += OnPostAttack.PostAttack;
            AIHeroClient.OnProcessSpellCast += OnProcessSpellCast.OnSpell;
            AIHeroClient.OnBasicAttack += OnBasicAttack.OnAttack;
            AttackableUnit.OnDamage += OnDamage.Damage;
        }
    }
}