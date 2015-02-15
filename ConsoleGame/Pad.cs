using System;

namespace ConsoleGame
{
    class Pad
    {
        private int x;
        private readonly int y;
        private ConsoleColor color;
        private int width;

        public int X
        {
            get { return x; }
            set { x = value; }
        }

        public int Width
        {
            get { return width; }
            set { width = value; }
        }

        public Pad(int x, int y, ConsoleColor color)
        {
            this.width = 3;
            this.x = x;
            this.y = y;
            this.color = color;
        }

        public void ChangePosition(ConsoleKeyInfo key)
        {
            if (key.Key == ConsoleKey.LeftArrow && this.x > 0)
            {
                this.x--;
            }

            if (key.Key == ConsoleKey.RightArrow && this.x + this.width < ConsoleGame.windowWidth - 1)
            {
                this.x++;
            }
        }

        public void Draw(char symbol)
        {
            Console.ForegroundColor = this.color;

            //Clear old pad if any
            Console.SetCursorPosition(0, this.y);
            Console.Write(new string(' ', ConsoleGame.windowWidth - 1));

            //Draw new pad
            Console.SetCursorPosition(this.x, this.y);
            Console.Write(new string(symbol, this.width));
        }
    }
}
