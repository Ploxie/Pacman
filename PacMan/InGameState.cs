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

        private List<Powerup> powerups = new List<Powerup>();

        public InGameState(SpriteSheet tilesheet, SpriteSheet characterSheet)
        {
            this.tilesheet = tilesheet;
            this.characterSheet = characterSheet;            
        }

        public void SetLevel(string filePath)
        {
            Level level = new Level(tilesheet);
            level.LoadLevel(filePath);
            currentLevel = level;

            pacman = new Pacman(characterSheet, level, 5);
        }

        private void PowerupCollision()
        {
            //if pacman and powerup are on the same tile do something...
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
