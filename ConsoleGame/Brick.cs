using System;

namespace ConsoleGame
{
    class Brick
    {
        private int x;
        private int y;
        private bool isBroken;
        private ConsoleColor color;
        public static Type type = typeof(ConsoleColor);

        public bool IsBroken
        {
            get { return isBroken; }
            set { isBroken = value; }
        }

        public Brick(int x, int y, ConsoleColor color)
        {
            this.x = x;
            this.y = y;
            this.isBroken = false;
            this.color = color;
        }

        public void Draw(char symbol)
        {
            Console.ForegroundColor = this.color;
            Console.SetCursorPosition(this.x, this.y);
            Console.Write(symbol);
        }
    }
}
