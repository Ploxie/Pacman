using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PacMan.GhostBehaviours;
using System;
using System.Collections.Generic;
using System.Text;

namespace PacMan
{
    public class InGameState : GameState
    {
        private GameWindow window;
        private SpriteSheet tilesheet;
        private SpriteSheet characterSheet;

        private Vector2 levelPosition;

        private HUD hud;
        private Level currentLevel;
        private Pacman pacman;
        private Ghost ghost;

        private List<Ghost> ghosts = new List<Ghost>();
        private List<Powerup> powerups = new List<Powerup>();

        public InGameState(GameWindow window, HUD hud, SpriteSheet tilesheet, SpriteSheet characterSheet)
        {
            this.window = window;
            this.hud = hud;
            this.tilesheet = tilesheet;
            this.characterSheet = characterSheet;
        }

        public void SetLevel(Level level)
        {
            currentLevel = level;
            hud.CurrentLevel = level;

            SpriteSheet pacmanSheet = new SpriteSheet(characterSheet.Texture, Vector2.Zero, new Vector2(80, 16), new Vector2(16, 16));
            pacman = new Pacman(pacmanSheet, level, 5);

            Tile pacmanSpawnTile = level.GetTile(1, 1);
            if(level.PacmanSpawn != null)
            {
                pacmanSpawnTile = level.PacmanSpawn;
            }

            pacman.Position = pacmanSpawnTile.Position + new Vector2(level.TileSize / 2);

            hud.Pacman = pacman;
            this.levelPosition = new Vector2((window.ClientBounds.Width / 2) - (level.PixelWidth / 2), (window.ClientBounds.Height / 2) - (level.PixelHeight / 2));

            SpriteSheet redGhostSheet = new SpriteSheet(characterSheet.Texture, new Vector2(0,16), new Vector2(128, 16), new Vector2(16, 16));

            foreach(Tile spawn in level.GhostSpawns)
            {
                Ghost ghost = new Ghost(redGhostSheet, level, pacman, new GhostPathfinding(pacman, level));
                ghost.Position = spawn.Position + new Vector2(level.TileSize / 2);
                ghosts.Add(ghost);
            }

            ghost = new Ghost(pacmanSheet, level, pacman, new GhostPathfinding(pacman, level));
            ghost.Position = level.GetTile(22, 22).Position + new Vector2(level.TileSize / 2);
        }

        private void PowerupCollision()
        {
            //if pacman and powerup are on the same tile do something...
        }

        public void Update(GameTime gameTime)
        {
            hud.Update(gameTime);
            if (pacman != null)
            {
                pacman.Update(gameTime);
            }
            foreach(Ghost ghost in ghosts)
            {
                ghost.Update(gameTime);
            }
            if (ghost != null)
            {
                //ghost.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            currentLevel.Draw(spriteBatch, levelPosition);
            hud.Draw(spriteBatch);

            if(pacman != null)
            {
                pacman.Draw(spriteBatch, levelPosition);
            }
            foreach (Ghost ghost in ghosts)
            {
                ghost.Draw(spriteBatch, levelPosition);
            }
            if (ghost != null)
            {
                //ghost.Draw(spriteBatch, levelPosition);
            }
        }        
    }
}
