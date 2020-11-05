using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace PacMan
{
    public class Pacman : Character
    {
        public Pacman(SpriteSheet spritesheet, Level level, int lives) : base(spritesheet, level)
        {
            Lives = lives;
        }

        public int Lives
        {
            get;
            set;
        }

        public int Score
        {
            get;
            set;
        }

        public override void Update(GameTime gameTime)
        {
            if(Keyboard.GetState().IsKeyDown(Keys.W))
            {
                ChangeDirection(0, -1);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                ChangeDirection(-1, 0);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                ChangeDirection(0, 1);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                ChangeDirection(1, 0);
            }

            UpdateMovement(gameTime);
            // check movement
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spritesheet.Sprite.Draw(spriteBatch, Position, Game1.Scale * 2, new Vector2(0.5f, 0.5f), SpriteEffects.None);
        }
        
    }
}
