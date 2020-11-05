using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace PacMan
{
    public abstract class GhostBehaviour
    {
        protected Ghost ghost;
        protected Pacman pacman;
        protected Level level;
        public GhostBehaviour(Pacman pacman, Level level)
        {
            this.pacman = pacman;
            this.level = level;
        }

        public Ghost Ghost
        {
            get { return ghost; }
            set { ghost = value; }
        }

        public abstract Vector2 CalculateDirection();

    }
}
