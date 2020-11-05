using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace PacMan
{
    public class InGameState : GameState
    {
        private SpriteSheet tilesheet;
        private SpriteSheet characterSheet;

        private Level currentLevel;
        private Pacman pacman;

        public InGameState(SpriteSheet tilesheet, SpriteSheet characterSheet)
        {
            this.tilesheet = tilesheet;
            this.characterSheet = characterSheet;            
        }

        public void SetLevel(Level level)
        {
            currentLevel = level;

            pacman = new Pacman(characterSheet, level, 5);
            pacman.Position = level.GetAt(1, 1).Position;
        }

        public void Update(GameTime gameTime)
        {
            if (pacman != null)
            {
                pacman.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            currentLevel.Draw(spriteBatch);

            if(pacman != null)
            {
                pacman.Draw(spriteBatch);
            }
        }        
    }
}
