using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGame
{
    class Player
    {
        private int score;
        private int lives;

        public int Score
        {
            get { return score; }
            set { score = value; }
        }

        public int Lives
        {
            get { return lives; }
            set { lives = value; }
        }

        public Player(int score, int lives)
        {
            this.score = score;
            this.lives = lives;
        }
    }
}
