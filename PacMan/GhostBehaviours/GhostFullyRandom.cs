using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace PacMan.GhostBehaviours
{

    public class GhostFullyRandom : GhostBehaviour
    {

        public GhostFullyRandom(Pacman pacman, Level level) : base(pacman, level)
        {

        }

        public override Vector2 CalculateDirection()
        {
            int Dir = Game1.random.Next(0, 4);
            switch (Dir)
            {
                case 0:
                    return new Vector2(0, -1);
                case 1:
                    return new Vector2(0, 1);
                case 2:
                    return new Vector2(1, 0);
                default:
                    return new Vector2(-1, 0);
            }
        }

    }
}
