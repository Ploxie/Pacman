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

            SpriteSheet redGhostSheet = new SpriteSheet(characterSheet.Texture, new Vector2(0, 16), new Vector2(128, 16), new Vector2(16, 16));
            SpriteSheet pinkGhostSheet = new SpriteSheet(characterSheet.Texture, new Vector2(0, 32), new Vector2(128, 16), new Vector2(16, 16));
            SpriteSheet blueGhostSheet = new SpriteSheet(characterSheet.Texture, new Vector2(0, 48), new Vector2(128, 16), new Vector2(16, 16));
            SpriteSheet orangeGhostSheet = new SpriteSheet(characterSheet.Texture, new Vector2(0, 64), new Vector2(128, 16), new Vector2(16, 16));

            int ghostBehaviourIndex = 0;
            foreach(Tile spawn in level.GhostSpawns)
            {
                GhostBehaviour behaviour = null;
                SpriteSheet ghostSpritesheet = null;
                switch(ghostBehaviourIndex)
                {
                    case 0:
                        behaviour = new GhostPatrolling(pacman, level);
                        ghostSpritesheet = blueGhostSheet;
                        break;
                    case 1:
                        behaviour = new GhostPathfinding(pacman, level);
                        ghostSpritesheet = pinkGhostSheet;
                        break;
                    default:
                        behaviour = new GhostFullyRandom(pacman, level);
                        ghostSpritesheet = redGhostSheet;
                        break;
                }

                Ghost ghost = new Ghost(ghostSpritesheet, level, pacman, behaviour);
                ghost.Position = spawn.Position + new Vector2(level.TileSize / 2);
                ghosts.Add(ghost);
                ghostBehaviourIndex++;
                ghostBehaviourIndex %= 3;
            }

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
        }        
    }
}
