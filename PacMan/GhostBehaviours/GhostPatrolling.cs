using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace PacMan.GhostBehaviours
{
    public class GhostPatrolling : GhostBehaviour
    {

        public GhostPatrolling(Pacman pacman, Level level) : base(pacman, level)
        {

        }

        public override Vector2 CalculateDirection()
        {
            Tile tileInDirection = level.GetAt((ghost.Position + new Vector2(level.TileSize / 2)) + (ghost.Direction * level.TileSize));

            if (tileInDirection == null || tileInDirection.Blocked || ghost.Direction == Vector2.Zero)
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
            return ghost.Direction;
        }

    }
}
