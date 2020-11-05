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
            timePerFrame = 100.0f;
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

        protected override void UpdateAnimation()
        {
            if(spritesheet.XIndex == 0)
            {
                spritesheet.XIndex = 1;
            }
            /*else if(spritesheet.XIndex == 1)
            {
                spritesheet.XIndex = 2;
            }*/
            else
            {
                spritesheet.XIndex = 0;
            }
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
            UpdateAnimationTimer(gameTime);
            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {

            float rotation = 0.0f;
            rotation = facing == Direction.Down ? (float)(Math.PI / 2) : rotation;
            rotation = facing == Direction.Left ? (float)(Math.PI / 2) * 2.0f : rotation;
            rotation = facing == Direction.Up ? (float)(Math.PI / 2) * 3.0f : rotation;
            spritesheet.Sprite.Draw(spriteBatch, Position, Game1.Scale * 2, new Vector2(0.5f, 0.5f), rotation, SpriteEffects.None);
        }
        
    }
}
