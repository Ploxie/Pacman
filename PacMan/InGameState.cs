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

        public bool FoodCollected
        {
            get
            {
                foreach (Tile tile in currentLevel.TileMap)
                {
                    if (tile.Powerup == null)
                    {
                        continue;
                    }
                    if (tile.Powerup.Type == PowerUpType.Food || tile.Powerup.Type == PowerUpType.BigFood)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        public void ResetGhosts()
        {
            foreach (Ghost ghost in ghosts)
            {
                ghost.Position = currentLevel.GhostSpawns[Game1.random.Next(currentLevel.GhostSpawns.Count)].Position;
            }
        }

        public void SetLevel(Level level)
        {
            currentLevel = level;
            hud.CurrentLevel = level;


            int oldScore = 0;
            if(pacman != null)
            {
                oldScore = pacman.Score;
            }

            SpriteSheet pacmanSheet = new SpriteSheet(characterSheet.Texture, Vector2.Zero, new Vector2(80, 16), new Vector2(16, 16));
            pacman = new Pacman(pacmanSheet, level, 5);
            pacman.Score = oldScore;

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
            SpriteSheet runGhostSheet = new SpriteSheet(characterSheet.Texture, new Vector2(0, 80), new Vector2(64, 16), new Vector2(16, 16));

            ghosts.Clear();
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
                        ghostSpritesheet = orangeGhostSheet;
                        break;
                    default:
                        behaviour = new GhostFullyRandom(pacman, level);
                        ghostSpritesheet = redGhostSheet;
                        break;
                }

                Ghost ghost = new Ghost(ghostSpritesheet,runGhostSheet, level, pacman, behaviour);
                ghost.Position = spawn.Position + new Vector2(level.TileSize / 2);
                ghosts.Add(ghost);
                ghostBehaviourIndex++;
                ghostBehaviourIndex %= 3;
            }
        }

        private void Collision()
        {
            Tile currentTile = currentLevel.GetAt(pacman.Position);
            if (currentTile.Powerup != null)
            {
                pacman.Score += currentLevel.GetAt(pacman.Position).Powerup.Score;
                

                switch (currentTile.Powerup.Type)
                {
                    case PowerUpType.WallEater:
                        pacman.ActivatePowerup(currentTile.Powerup);
                        hud.AddGainedScore(new GainedScore(currentTile, currentLevel.GetAt(pacman.Position).Powerup.Score, 1000));
                        break;
                    case PowerUpType.GhostEater:
                        pacman.ActivatePowerup(currentTile.Powerup);
                        hud.AddGainedScore(new GainedScore(currentTile, currentLevel.GetAt(pacman.Position).Powerup.Score, 1000));
                        break;
                    case PowerUpType.BigFood:
                        hud.AddGainedScore(new GainedScore(currentTile, currentLevel.GetAt(pacman.Position).Powerup.Score, 1000));
                        break;
                    default:
                        break;
                }
                currentTile.Powerup = null;
            }

            foreach (Ghost ghost in ghosts)
            {
                if (ghost.TilePosition == pacman.TilePosition && pacman.ActivePowerupType != PowerUpType.GhostEater)
                {
                    pacman.LoseLives();
                }
                else if (ghost.TilePosition == pacman.TilePosition && pacman.ActivePowerupType == PowerUpType.GhostEater)
                {
                    ghost.Dead = true;
                    pacman.Score += 200;
                    hud.AddGainedScore(new GainedScore(currentTile, 200, 1000));
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            hud.Update(gameTime);
            if (currentLevel.NeedsReset && pacman != null)
            {
                ResetGhosts();
                pacman.Lives = 3;
                int tileSize = currentLevel.TileSize;
                pacman.Position = currentLevel.PacmanSpawn.Position + new Vector2(tileSize / 2, tileSize / 2);
            }

            if (pacman != null)
            {
                pacman.Update(gameTime);
            }

            foreach(Ghost ghost in ghosts)
            {
                ghost.Update(gameTime);
            }

            Collision();

            
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
