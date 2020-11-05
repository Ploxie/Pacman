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
        }

        public Vector2 Position
        {
            get { return position; }
            private set { this.position = value; }
        }

        public Vector2 Destination
        {
            get;
        }
        
        public void ChangeDirection(Vector2 direction)
        {
            Vector2 newPosition = position + direction;

            if (!level.GetAt(newPosition).Blocked)
            {
                this.destination = newPosition;                
                this.direction = direction;
            }
        }
        
        protected void UpdateMovement(GameTime gameTime)
        {
            position += direction * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (Vector2.Distance(position, destination) < 1)
            {
                position = destination;
            }
        }

        public abstract void Update(GameTime gameTime);

        public abstract void Draw(SpriteBatch spriteBatch);

    }
}
