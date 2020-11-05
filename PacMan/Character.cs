using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace PacMan
{
    public abstract class Character
    {

        protected SpriteSheet spritesheet;
        protected Level level;

        protected Vector2 position;
        protected Vector2 destination;
        protected Vector2 direction;
        protected float speed;

        public Character(SpriteSheet spritesheet, Level level)
        {
            this.spritesheet = spritesheet;
            this.level = level;
            this.speed = 3.0f;
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
        
        public void ChangeDirection(Vector2 direction)
        {
            Vector2 destinationTilePosition = (TilePosition + new Vector2(level.TileSize / 2)) + (direction * level.TileSize) ;
            Vector2 currentTileMiddle = level.GetAt(position).Position + new Vector2(level.TileSize / 2);

            if (Vector2.Distance(position,currentTileMiddle) <= 0.1 && !level.GetAt(destinationTilePosition).Blocked)
            {
                this.position = currentTileMiddle;
                this.destination = destinationTilePosition;                
                this.direction = direction;
            }
        }

        public void ChangeDirection(int x, int y)
        {
            ChangeDirection(new Vector2(x, y));
        }
        
        protected void UpdateMovement(GameTime gameTime)
        {
            Vector2 destinationTilePosition = (TilePosition + new Vector2(level.TileSize / 2)) + (direction * level.TileSize);
            Vector2 currentTileMiddle = level.GetAt(position).Position + new Vector2(level.TileSize / 2);
            if (Vector2.Distance(position, currentTileMiddle) <= 0.1 && level.GetAt(destinationTilePosition).Blocked)
            {
                direction = Vector2.Zero;
            }

            position += direction * (speed * level.TileSize) * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (Vector2.Distance(position, destination) < 1)
            {
                position = destination;            
            }
        }

        public abstract void Update(GameTime gameTime);

        public abstract void Draw(SpriteBatch spriteBatch);

    }
}
