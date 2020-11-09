using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace PacMan.GhostBehaviours
{

    public class GhostRunAwayBehaviour : GhostBehaviour
    {

        public GhostRunAwayBehaviour(Pacman pacman, Level level) : base(pacman, level)
        {

        }

        private Tile GetTileInDirection(float x, float y)
        {
            return level.GetAt((ghost.TilePosition + new Vector2(level.TileSize / 2)) + (new Vector2(x, y) * level.TileSize));
        }

        public override Vector2 CalculateDirection()
        {
            float xDist = ghost.Position.X - pacman.Position.X;
            float yDist = ghost.Position.Y - pacman.Position.Y;

            

            if (yDist > 0 && GetTileInDirection(0, 1) != null && !GetTileInDirection(0, 1).Blocked)
            {
                return new Vector2(0, 1);
            }
            else if (yDist < 0 && GetTileInDirection(0, -1) != null && !GetTileInDirection(0, -1).Blocked)
            {
                return new Vector2(0, -1);
            }
            else if (xDist > 0 && GetTileInDirection(1, 0) != null && !GetTileInDirection(1, 0).Blocked)
            {
                return new Vector2(1, 0);
            }
            else if (xDist < 0 && GetTileInDirection(-1, 0) != null && !GetTileInDirection(-1, 0).Blocked)
            {
                return new Vector2(-1, 0);
            }

            if (GetTileInDirection(ghost.Direction.X, ghost.Direction.Y).Blocked)
            {
                Vector2 counterClockWise = new Vector2(-ghost.Direction.Y, ghost.Direction.X);
                if (!GetTileInDirection(counterClockWise.X, counterClockWise.Y).Blocked)
                {
                    return counterClockWise;
                }
                Vector2 clockWise = new Vector2(ghost.Direction.Y, -ghost.Direction.X);
                if (!GetTileInDirection(clockWise.X, clockWise.Y).Blocked)
                {
                    return clockWise;
                }
            }
            return ghost.Direction;
        }

    }
}
