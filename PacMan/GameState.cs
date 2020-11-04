using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace PacMan
{
    public interface GameState
    {

        public void Update(GameTime gameTime);

        public void Draw(SpriteBatch spriteBatch);
    }
}
