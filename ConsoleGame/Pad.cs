using System;
//using System.Threading;

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
            this.width = 5;
            this.x = x;
            this.y = y;
            this.color = color;
        }

        public void ChangePosition(ConsoleKeyInfo key)
        {

            if (key.Key == ConsoleKey.LeftArrow && this.x > 0)
            {
                this.x-=2;
            }

            if (key.Key == ConsoleKey.RightArrow && this.x + this.width < ConsoleGame.WindowWidth - 1)
            {
                this.x+=2;
            }

            if (key.Key == ConsoleKey.P)
            {
                bool cont = false;
                while (!cont)
                {
                    key = Console.ReadKey(true);
                    if (key.Key == ConsoleKey.P)
                    {
                        cont = true;
                        break;
                    }

                }
            }
        }

        public void Draw(char symbol)
        {
            Console.ForegroundColor = this.color;

            //Clear old pad if any
            Console.SetCursorPosition(0, this.y);
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write(new string(' ', ConsoleGame.WindowWidth - 1));

            //Draw new pad
            Console.SetCursorPosition(this.x, this.y);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(new string(symbol, this.width));
        }
    }
}
