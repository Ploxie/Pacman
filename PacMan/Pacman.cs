using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

            // check movement
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spritesheet.Sprite.Draw(spriteBatch, Position, Game1.Scale, SpriteEffects.None, Color.White);
        }
        
    }
}
