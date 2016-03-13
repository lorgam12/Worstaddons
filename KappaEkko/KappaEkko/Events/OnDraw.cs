namespace KappaEkko.Events
{
    using System;

    using EloBuddy;
    using EloBuddy.SDK.Menu.Values;
    using EloBuddy.SDK.Rendering;

    using SharpDX;

    internal class OnDraw
    {
        public static void Draw(EventArgs args)
        {
            if (Menu.DrawMenu.Get<CheckBox>("Q").CurrentValue && Spells.Q.IsLearned)
            {
                Circle.Draw(Color.Purple, Spells.Q.Range, ObjectManager.Player.Position);
            }

            if (Menu.DrawMenu.Get<CheckBox>("W").CurrentValue && Spells.W.IsLearned)
            {
                Circle.Draw(Color.Purple, Spells.W.Range, ObjectManager.Player.Position);
            }

            if (Menu.DrawMenu.Get<CheckBox>("E").CurrentValue && Spells.E.IsLearned)
            {
                Circle.Draw(Color.Purple, Spells.E.Range, ObjectManager.Player.Position);
            }

            if (Menu.DrawMenu.Get<CheckBox>("R").CurrentValue && Spells.R.IsLearned && Spells.EkkoREmitter != null)
            {
                Circle.Draw(Color.Purple, Spells.R.Range, Spells.EkkoREmitter.Position);
            }
        }
    }
}