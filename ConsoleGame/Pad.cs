using System;

namespace ConsoleGame
{
    class Pad
    {
        private int x;
        private readonly int y;
        private ConsoleColor color;
        private int width;

        public Pad(int x, int y, ConsoleColor color)
        {
            this.width = 3;
            this.x = x;
            this.y = y;
            this.color = color;
        }

        public void Draw(char symbol)
        {
            Console.ForegroundColor = this.color;
            Console.SetCursorPosition(this.x, this.y);
            Console.Write(new string(symbol, this.width));
        }
    }
}
