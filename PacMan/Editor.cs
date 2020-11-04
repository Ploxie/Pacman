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
        private SpriteSheet spritesheet;
        private GameWindow window;

        private Level currentLevel;
        private Tile hoveredTile;

        private Tile[,] palette;
        private Tile hoveredPalette;
        private Tile selectedPalette;

        private Vector2 levelPosition;
        private Vector2 palettePosition;

        public Editor(SpriteSheet palette, GameWindow window)
        {
            this.spritesheet = palette;
            this.window = window;
            this.palette = new Tile[palette.Columns, palette.Rows];

            int paletteIndex = 0;
            for (int y = 0; y < palette.Rows; y++)
            {
                for (int x = 0; x < palette.Columns; x++)
                {
                    char type = paletteIndex++.ToString("X1")[0];
                    this.palette[x, y] = new Tile(type, palette.GetAt(x, y), new Vector2(x * 32 * Game1.Scale.X, y * 32 * Game1.Scale.Y), (int)palette.Sprite.SpriteSize.X);                    
                }
            }
                  
            this.selectedPalette = this.palette[0, 0];
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

        public Texture2D SelectedTexture
        {
            get;
            set;
        }

        public int PaletteWidth
        {
            get { return (int)(palette.GetLength(0) * 32 * Game1.Scale.X); }
        }

        public int PaletteHeight
        {
            get { return (int)(palette.GetLength(1) * 32 * Game1.Scale.Y); }
        }

        public Level CreateNewLevel(string filePath, int columns, int rows)
        {
            Level level = new Level(spritesheet, columns, rows);
            this.palettePosition = new Vector2(level.PixelWidth, 0);
            this.currentLevel = level;

            return currentLevel;
        }

        public Level LoadLevel(string filePath)
        {
            Level level = new Level(spritesheet);
            level.LoadLevel(filePath);
            this.palettePosition = new Vector2(this.levelPosition.X + level.PixelWidth, 0);
            this.currentLevel = level;
            return currentLevel;
        }

        public void SaveLevel(string filePath)
        {
            StreamWriter writer = new StreamWriter(filePath, false);

            writer.WriteLine(currentLevel.Width);
            writer.WriteLine(currentLevel.Height);

            for (int y = 0; y < currentLevel.Height; y++)
            {
                for (int x = 0; x < currentLevel.Width; x++)
                {
                    writer.Write(currentLevel.TileMap[x, y].Type);
                }
                writer.Write('\n');
            }

            writer.Close();

            System.Diagnostics.Debug.WriteLine("Level saved: " + filePath);
        }

        public void Update(GameTime gameTime)
        {
            Vector2 mousePoint = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
            hoveredPalette = null;
            foreach (Tile tile in palette)
            {
                if (tile.Bounds.Contains(mousePoint - palettePosition))
                {
                    hoveredPalette = tile;
                    if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                    {
                        selectedPalette = tile;
                    }
                }
            }

            hoveredTile = null;
            foreach (Tile tile in currentLevel.TileMap)
            {
                if (tile.Bounds.Contains(mousePoint))
                {
                    hoveredTile = tile;



                    if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                    {
                        tile.Sprite = selectedPalette.Sprite;
                        tile.Type = '1';
                        currentLevel.CalculateSprites();
                    }
                    else if (Mouse.GetState().RightButton == ButtonState.Pressed)
                    {
                        tile.Sprite = Tile.NULL_SPRITE;
                        tile.Type = Tile.EMPTY_TYPE;
                        currentLevel.CalculateSprites();
                    }
                }
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach(Tile tile in currentLevel.TileMap)
            {
                //spriteBatch.Draw(GridTexture, levelPosition + tile.Position, null, Color.White, 0f, Vector2.Zero, Game1.Scale, SpriteEffects.None, 1.0f);

                if(hoveredTile == tile)
                {
                    spriteBatch.Draw(HighlightTexture, levelPosition + tile.Position, null, Color.White, 0f, Vector2.Zero, Game1.Scale, SpriteEffects.None, 1.0f);
                }

                if (tile.Sprite == null)
                {
                    continue;
                }
                tile.Sprite.Draw(spriteBatch, levelPosition + tile.Position, Game1.Scale, SpriteEffects.None, Color.White);
            }

            foreach(Tile tile in this.palette)
            {
               
                tile.Sprite.Draw(spriteBatch, palettePosition + tile.Position, Game1.Scale, SpriteEffects.None, Color.White);
                
                if (selectedPalette == tile)
                {
                    spriteBatch.Draw(SelectedTexture, palettePosition + tile.Position, null, Color.White, 0f, Vector2.Zero, Game1.Scale, SpriteEffects.None, 1.0f);
                }
                else if (hoveredPalette == tile)
                {
                    spriteBatch.Draw(HighlightTexture, palettePosition + tile.Position, null, Color.White, 0f, Vector2.Zero, Game1.Scale, SpriteEffects.None, 1.0f);
                }
                spriteBatch.Draw(GridTexture, palettePosition + tile.Position, null, Color.DarkGray, 0f, Vector2.Zero, Game1.Scale, SpriteEffects.None, 1.0f);
            }
        }

    }
}
