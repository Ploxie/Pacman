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

        public void SetLevel(Level level)
        {
            currentLevel = level;

            SpriteSheet pacmanSheet = new SpriteSheet(characterSheet.Texture, Vector2.Zero, new Vector2(80, 16), new Vector2(16, 16));
            pacman = new Pacman(pacmanSheet, level, 5);
            pacman.Position = level.GetTile(1, 1).Position + new Vector2(level.TileSize / 2);
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
