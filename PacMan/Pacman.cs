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

        private Vector2 currentTilePosition;
        private Vector2 preferredDirection;
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
            else
            {
                spritesheet.XIndex = 0;
            }
        }

        public override void Update(GameTime gameTime)
        {           
            if(Keyboard.GetState().IsKeyDown(Keys.W))
            {
                preferredDirection = new Vector2(0, -1);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                preferredDirection = new Vector2(-1, 0);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                preferredDirection = new Vector2(0, 1);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                preferredDirection = new Vector2(1, 0);                
            }

            ChangeDirection(preferredDirection);

            UpdateMovement(gameTime);            
            UpdateAnimationTimer(gameTime);

            if(currentTilePosition != TilePosition)
            {
                currentTilePosition = TilePosition;
            }
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {

            float rotation = 0.0f;
            rotation = facing == FaceDirection.Down ? (float)(Math.PI / 2) : rotation;
            rotation = facing == FaceDirection.Left ? (float)(Math.PI / 2) * 2.0f : rotation;
            rotation = facing == FaceDirection.Up ? (float)(Math.PI / 2) * 3.0f : rotation;
            spritesheet.Sprite.Draw(spriteBatch, offset + Position, Game1.Scale * 2, new Vector2(0.5f, 0.5f), rotation, SpriteEffects.None);
        }
        
    }
}
