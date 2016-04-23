namespace KappAzir.Modes
{
    using EloBuddy;
    using EloBuddy.SDK;

    internal class Flee : ModeManager
    {
        public static void Execute()
        {
            Jumper.jump(Game.CursorPos);
        }
    }
}