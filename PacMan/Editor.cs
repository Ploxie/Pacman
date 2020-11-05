using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PacMan
{
    public class Editor : GameState
    {
        private enum BrushType
        {
            Block,
            Food,
            PowerupGhost,
            PowerupWall,
            PacmanSpawn,
            GhostSpawn,
        }

        private SpriteSheet tilesheet;
        private SpriteSheet spritesheet;
        private GameWindow window;

        private Level currentLevel;
        private Tile hoveredTile;

        private BrushType brushType = BrushType.Block;
        
        private Vector2 levelPosition;

        public Editor(SpriteSheet tilesheet,SpriteSheet spritesheet, GameWindow window)
        {
            this.tilesheet = tilesheet;
            this.spritesheet = spritesheet;
            this.window = window;     
        }

        public Texture2D GridTexture
        {
            get;
            set;
        }

        public Texture2D HighlightTexture
        {
            get;
            set;
        }

        public Sprite FoodSprite
        {
            get;
            set;
        }

        public Sprite PowerupGhostSprite
        {
            get;
            set;
        }
        
        public Level CreateNewLevel(string filePath, int columns, int rows, int tileSize)
        {
            Level level = Level.CreateLevel(tilesheet,spritesheet, columns, rows, tileSize);
            this.levelPosition = new Vector2((window.ClientBounds.Width / 2) - (level.PixelWidth / 2), (window.ClientBounds.Height / 2) - (level.PixelHeight / 2));
            this.currentLevel = level;

            return currentLevel;
        }

        public void SetLevel(Level level)
        {
            this.levelPosition = new Vector2((window.ClientBounds.Width / 2) - (level.PixelWidth / 2), (window.ClientBounds.Height / 2) - (level.PixelHeight / 2));
            this.currentLevel = level;
        }

        public void SaveLevel(string filePath)
        {
            StreamWriter writer = new StreamWriter(filePath, false);

            writer.WriteLine(currentLevel.Width);
            writer.WriteLine(currentLevel.Height);
            writer.WriteLine(currentLevel.TileSize);

            for (int y = 0; y < currentLevel.Height; y++)
            {
                for (int x = 0; x < currentLevel.Width; x++)
                {
                    int value = currentLevel.TileMap[x, y].Blocked ? 1 : 0;
                    value = currentLevel.TileMap[x, y].Powerup != null && currentLevel.TileMap[x, y].Powerup.Type == PowerUpType.Food ? 2 : value;
                    writer.Write(value);
                }
                writer.Write('\n');
            }

            writer.Close();

            System.Diagnostics.Debug.WriteLine("Level saved: " + filePath);
        }

        private void BrushAdd(Tile tile)
        {
            switch(brushType)
            {
                case BrushType.Block:
                    tile.Blocked = true;
                    tile.Powerup = null;
                    currentLevel.CalculateSprites();
                    break;
                case BrushType.Food:
                    if (!tile.Blocked)
                    {
                        tile.Powerup = new Powerup(PowerUpType.Food, FoodSprite, 100);
                    }
                    break;
                case BrushType.PowerupGhost:
                    if (!tile.Blocked)
                    {
                        tile.Powerup = new Powerup(PowerUpType.GhostEater, PowerupGhostSprite , 100);
                    }
                    break;
            }
        }

        public void BrushRemove(Tile tile)
        {
            switch (brushType)
            {
                case BrushType.Block:
                    tile.Blocked = false;                    
                    currentLevel.CalculateSprites();
                    break;
                case BrushType.Food: case BrushType.PowerupGhost:
                    tile.Powerup = null;
                    break;
            }
        }

        public void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.D1))
            {
                brushType = BrushType.Block;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.D2))
            {
                brushType = BrushType.Food;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.D3))
            {
                brushType = BrushType.PowerupGhost;
            }

            Vector2 mousePoint = new Vector2(Mouse.GetState().X, Mouse.GetState().Y) - levelPosition;
           
            hoveredTile = null;
            foreach (Tile tile in currentLevel.TileMap)
            {
                if (tile.Bounds.Contains(mousePoint))
                {
                    hoveredTile = tile;
                    if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                    {
                        BrushAdd(tile);
                    }
                    else if (Mouse.GetState().RightButton == ButtonState.Pressed)
                    {
                        BrushRemove(tile);
                    }
                }
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach(Tile tile in currentLevel.TileMap)
            {
                spriteBatch.Draw(GridTexture, levelPosition + tile.Position, null, Color.White, 0f, Vector2.Zero, Game1.Scale, SpriteEffects.None, 1.0f);

                if(hoveredTile == tile)
                {
                    spriteBatch.Draw(HighlightTexture, levelPosition + tile.Position, null, Color.White, 0f, Vector2.Zero, Game1.Scale, SpriteEffects.None, 1.0f);
                    if(!hoveredTile.Blocked)
                    {
                        if (brushType == BrushType.Food)
                        {
                            FoodSprite.Draw(spriteBatch, levelPosition + tile.Position + new Vector2(currentLevel.TileSize / 2), Game1.Scale, new Vector2(0.5f));
                        }
                        else if (brushType == BrushType.PowerupGhost)
                        {
                            PowerupGhostSprite.Draw(spriteBatch, levelPosition + tile.Position + new Vector2(currentLevel.TileSize / 2), Game1.Scale, new Vector2(0.5f));
                        }
                    }                    
                }
            }
            currentLevel.Draw(spriteBatch, levelPosition);
        }

    }
}
