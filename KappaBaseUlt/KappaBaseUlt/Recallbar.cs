namespace KappaBaseUlt
{
    using EloBuddy;
    using EloBuddy.SDK;

    using SharpDX;

    using Color = System.Drawing.Color;

    internal static class Recallbar
    {
        private static readonly float X = Drawing.Width * 0.425f;

        private static readonly float Y = Drawing.Height * 0.80f;

        private static readonly int Width = (int)(Drawing.Width - 2 * X);

        private static readonly int Height = 6;

        private static readonly float Scale = (float)Width / 8000;

        public static void RecallBarDraw(this Program.EnemyInfo enemy)
        {
            Rect(X, Y, Width, Height, 1, Color.White);
            var c = Color.White;
            if (enemy.CountDown() >= enemy.Enemy.traveltime())
            {
                c = Color.White;
                if (enemy.Enemy.Killable())
                {
                    c = Color.Red;
                    Drawing.DrawLine(
                        (X + 450) + Scale * enemy.Enemy.traveltime() - 1,
                        Y + 7,
                        (X + 450) + Scale * enemy.Enemy.traveltime(),
                        Y - 11,
                        3,
                        c);
                }
            }
            Drawing.DrawText(
                (X + 450) + Scale * enemy.CountDown() - 1,
                Y - 30,
                c,
                "(" + (int)enemy.Enemy.HealthPercent + "%)" + enemy.Enemy.BaseSkinName);
            Drawing.DrawLine((X + 450) + Scale * enemy.CountDown() - 1, Y + 7, (X + 450) + Scale * enemy.CountDown(), Y - 11, 3, c);
        }

        public static void Rect(float x, float y, int width, int height, float bold, Color color)
        {
            var x2 = x + 450;
            for (int i = 0; i < height; i++)
            {
                Drawing.DrawLine(x2, y + i, x2 + width, y + i, bold, color);
            }
        }
    }
}