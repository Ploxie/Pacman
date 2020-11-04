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

        protected Vector2 position;
        protected Vector2 direction;

        public Character(SpriteSheet spritesheet)
        {
            this.spritesheet = spritesheet;
        }

        public Vector2 Position
        {
            get { return position; }
            private set { this.position = value; }
        }

        
        
        public void ChangeDirection(Vector2 direction)
        {
            this.direction = direction;
        }

        public abstract void Update(GameTime gameTime);

        public abstract void Draw(SpriteBatch spriteBatch);

    }
}
