using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace PacMan
{
    class Ghost : Character
    {
        private Pacman pacman;

        public enum AItype {PathFinding, FullyRandom, Patrolling};
        private AItype type;
        float randomTimer;

        public Ghost(SpriteSheet spriteSheet, Level level, Pacman pacman, AItype type) : base(spriteSheet, level) 
        { 
            this.pacman = pacman;
            direction = new Vector2(0, -1);
            this.type = type;
        
        }

        public Vector2 PathFinding(Vector2 direction)
        {
            float xDist = position.X - pacman.Position.X;
            float yDist = position.Y - pacman.Position.Y;
            if (yDist > 0 && !level.GetAt((TilePosition + new Vector2(level.TileSize / 2)) + (new Vector2(0, -1) * level.TileSize)).Blocked)
            {
                return new Vector2(0, -1);
            }
            else if (yDist < 0 && !level.GetAt((TilePosition + new Vector2(level.TileSize / 2)) + (new Vector2(0, 1) * level.TileSize)).Blocked)
            {
                return new Vector2(0, 1);
            }
            else if (xDist > 0 && !level.GetAt((TilePosition + new Vector2(level.TileSize / 2)) + (new Vector2(-1, 0) * level.TileSize)).Blocked)
            {
                return new Vector2(-1, 0);
            }
            else if (xDist < 0 && !level.GetAt((TilePosition + new Vector2(level.TileSize / 2)) + (new Vector2(1, 0) * level.TileSize)).Blocked)
            {
                return new Vector2(1, 0);
            }

            return direction;
        }

        protected override void UpdateAnimation() { }

        public override void Update(GameTime gameTime)
        {
            switch (type)
            {
                case AItype.PathFinding:
                    ChangeDirection(PathFinding(direction));
                    break;
                case AItype.FullyRandom:
                    int tempDir = Game1.random.Next(0, 4);
                    switch (tempDir)
                    {
                        case 0:
                            ChangeDirection(new Vector2(0, 1));
                            break;
                        case 1:
                            ChangeDirection(new Vector2(0, -1));
                            break;
                        case 2:
                            ChangeDirection(new Vector2(1, 0));
                            break;
                        default:
                            ChangeDirection(new Vector2(-1, 0));
                            break;
                    }
                    break;
                case AItype.Patrolling:
                    if (level.GetAt((TilePosition + new Vector2(level.TileSize / 2)) + (direction * level.TileSize)).Blocked || direction == Vector2.Zero)
                    {
                        int Dir = Game1.random.Next(0, 4);
                        switch (Dir)
                        {
                            case 0:
                                ChangeDirection(new Vector2(0, 1));
                                break;
                            case 1:
                                ChangeDirection(new Vector2(0, -1));
                                break;
                            case 2:
                                ChangeDirection(new Vector2(1, 0));
                                break;
                            default:
                                ChangeDirection(new Vector2(-1, 0));
                                break;

                        }
                    }
                    break;
                default:
                    break;
            }
            

            UpdateMovement(gameTime);
            UpdateAnimationTimer(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spritesheet.Sprite.Draw(spriteBatch, Position, Game1.Scale * 2, new Vector2(0.5f, 0.5f));
        }
    }

}
