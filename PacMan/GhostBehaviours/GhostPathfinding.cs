using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace PacMan.GhostBehaviours
{

    public class GhostPathfinding : GhostBehaviour
    {

        public GhostPathfinding(Pacman pacman, Level level) : base(pacman, level)
        {

        }

        private Tile GetTileInDirection(int x, int y)
        {
            return level.GetAt((ghost.TilePosition + new Vector2(level.TileSize / 2)) + (new Vector2(x, y) * level.TileSize));
        }

        public override Vector2 CalculateDirection()
        {
            float xDist = ghost.Position.X - pacman.Position.X;
            float yDist = ghost.Position.Y - pacman.Position.Y;

            if (yDist > 0 && GetTileInDirection(0, -1) != null && !GetTileInDirection(0, -1).Blocked)
            {
                return new Vector2(0, -1);
            }
            else if (yDist < 0 && GetTileInDirection(0, 1) != null && !GetTileInDirection(0, 1).Blocked)
            {
                return new Vector2(0, 1);
            }
            else if (xDist > 0 && GetTileInDirection(-1, 0) != null && !GetTileInDirection(-1, 0).Blocked)
            {
                return new Vector2(-1, 0);
            }
            else if (xDist < 0 && GetTileInDirection(1, 0) != null && !GetTileInDirection(1, 0).Blocked)
            {
                return new Vector2(1, 0);
            }

            return ghost.Direction;
        }

    }
}
