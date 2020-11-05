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
        public GhostBehaviour(Ghost ghost, Pacman pacman, Level level)
        {
            this.ghost = ghost;
            this.pacman = pacman;
            this.level = level;
        }

        public abstract Vector2 CalculateDirection();

    }
}
