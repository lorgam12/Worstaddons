namespace KappaEkko.Logics
{
    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Menu.Values;

    internal class Rlogic
    {
        public static void Combo()
        {
            var Rhit = Menu.ComboMenu["Rhit"].Cast<Slider>().CurrentValue;
            var Enemies = Spells.EkkoREmitter.Position.CountEnemiesInRange(400);

            if (Enemies >= Rhit)
            {
                Spells.R.Cast();
            }
        }

        public static void Aoe()
        {
            var RAoe = Menu.UltMenu["RAoeh"].Cast<Slider>().CurrentValue;
            var Enemies = Spells.EkkoREmitter.Position.CountEnemiesInRange(400);

            if (Enemies >= RAoe && Spells.EkkoREmitter != null)
            {
                Spells.R.Cast();
            }
        }

        public static void Escape()
        {
            var REscapeh = Menu.UltMenu["REscapeh"].Cast<Slider>().CurrentValue;
            var Health = ObjectManager.Player.HealthPercent;
            var Enemies = Spells.EkkoREmitter.Position.CountEnemiesInRange(400);
            var EnemiesMy = ObjectManager.Player.Position.CountEnemiesInRange(800);

            if (REscapeh >= Health && Enemies < EnemiesMy)
            {
                Spells.R.Cast();
            }
        }
    }
}