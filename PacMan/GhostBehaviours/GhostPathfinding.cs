using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace PacMan.GhostBehaviours
{

    public class GhostPathfinding : GhostBehaviour
    {

        public GhostPathfinding(Ghost ghost, Pacman pacman, Level level) : base(ghost, pacman, level)
        {

        }

        public override Vector2 CalculateDirection()
        {
            float xDist = ghost.Position.X - pacman.Position.X;
            float yDist = ghost.Position.Y - pacman.Position.Y;
            if (yDist > 0 && !level.GetAt((ghost.TilePosition + new Vector2(level.TileSize / 2)) + (new Vector2(0, -1) * level.TileSize)).Blocked)
            {
                return new Vector2(0, -1);
            }
            else if (yDist < 0 && !level.GetAt((ghost.TilePosition + new Vector2(level.TileSize / 2)) + (new Vector2(0, 1) * level.TileSize)).Blocked)
            {
                return new Vector2(0, 1);
            }
            else if (xDist > 0 && !level.GetAt((ghost.TilePosition + new Vector2(level.TileSize / 2)) + (new Vector2(-1, 0) * level.TileSize)).Blocked)
            {
                return new Vector2(-1, 0);
            }
            else if (xDist < 0 && !level.GetAt((ghost.TilePosition + new Vector2(level.TileSize / 2)) + (new Vector2(1, 0) * level.TileSize)).Blocked)
            {
                return new Vector2(1, 0);
            }

            return ghost.Direction;
        }

    }
}
