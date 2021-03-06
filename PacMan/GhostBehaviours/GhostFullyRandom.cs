﻿using Microsoft.Xna.Framework;
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
            return Dir switch
            {
                0 => new Vector2(0, -1),
                1 => new Vector2(0, 1),
                2 => new Vector2(1, 0),
                _ => new Vector2(-1, 0),
            };
        }

    }
}
