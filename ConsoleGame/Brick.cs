using System;

namespace ConsoleGame
{
    class Brick
    {
        private int x;
        private int y;
        private ConsoleColor color;

        public Brick(int x, int y, ConsoleColor color)
        {
            this.x = x;
            this.y = y;
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
