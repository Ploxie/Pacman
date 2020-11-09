using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace PacMan
{
    public abstract class Character
    {

        protected enum FaceDirection
        {
            Up,
            Down,
            Left,
            Right,
        }

        protected SpriteSheet spritesheet;
        protected Level level;

        protected Vector2 position;
        protected Vector2 destination;
        protected Vector2 direction;
        protected float speed;

        protected FaceDirection facing;

        protected double animationTimer;
        protected double timePerFrame = 200.0f;

        public Character(SpriteSheet spritesheet, Level level)
        {
            this.spritesheet = spritesheet;
            this.level = level;
            this.speed = 3.0f;
            this.facing = FaceDirection.Up;
        }

        public Vector2 Position
        {
            get { return position; }
            set { this.position = value; }
        }

        public Vector2 TilePosition
        {
            get { return level.GetAt(Position).Position; }
        }

        public Vector2 Destination
        {
            get;
        }

        public Vector2 Direction
        {
            get { return direction; }
        }
        
        protected virtual bool CanGoDirection(Vector2 direction)
        {
            Vector2 destinationTilePosition = (TilePosition + new Vector2(level.TileSize / 2)) + (direction * level.TileSize);
            
            if (!level.GetAt(destinationTilePosition).Blocked)
            {
                return true;
            }

            return false;
        }

        public void ChangeDirection(Vector2 direction)
        {
            Vector2 destinationTilePosition = (TilePosition + new Vector2(level.TileSize / 2)) + (direction * level.TileSize) ;
            Vector2 currentTileMiddle = level.GetAt(position).Position + new Vector2(level.TileSize / 2);

            if (Vector2.Distance(position,currentTileMiddle) <= 0.1 && CanGoDirection(direction))
            {
                this.position = currentTileMiddle;
                this.destination = destinationTilePosition;                
                this.direction = direction;

                if (direction.X > 0)
                {
                    this.facing = FaceDirection.Right;
                }
                else if (direction.X < 0)
                {
                    this.facing = FaceDirection.Left;
                }
                else if (direction.Y < 0)
                {
                    this.facing = FaceDirection.Up;
                }
                else if (direction.Y > 0)
                {
                    this.facing = FaceDirection.Down;
                }
            }
        }

        public void ChangeDirection(int x, int y)
        {
            ChangeDirection(new Vector2(x, y));
        }

        private void HandleOutOfBounds()
        {
            if (position.X < 0)
            {
                position.X += level.Width * level.TileSize;
            }
            else if (position.X >= level.Width * level.TileSize)
            {
                position.X -= level.Width * level.TileSize;
            }
            else if (position.Y < 0)
            {
                position.Y += level.Height * level.TileSize;
            }
            else if (position.Y >= level.Height * level.TileSize)
            {
                position.Y -= level.Height * level.TileSize;
            }
        }
        
        protected void UpdateMovement(GameTime gameTime)
        {
            Vector2 currentTileMiddle = level.GetAt(position).Position + new Vector2(level.TileSize / 2);

            if (Vector2.Distance(position, currentTileMiddle) <= 0.1 && !CanGoDirection(direction))
            {
                direction = Vector2.Zero;
            }

            position += direction * (speed * level.TileSize) * (float)gameTime.ElapsedGameTime.TotalSeconds;

            HandleOutOfBounds();

            if (Vector2.Distance(position, destination) < 1)
            {
                position = destination;            
            }
        }

        protected abstract void UpdateAnimation();

        protected void UpdateAnimationTimer(GameTime gameTime)
        {
            animationTimer -= gameTime.ElapsedGameTime.TotalMilliseconds;
            if(animationTimer <= 0)
            {
                UpdateAnimation();
                animationTimer = timePerFrame;
            }
        }        

        public abstract void Update(GameTime gameTime);

        public abstract void Draw(SpriteBatch spriteBatch, Vector2 offset);

    }
}
