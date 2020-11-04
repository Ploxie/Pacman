using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PacMan
{
    class Editor
    {

        private Level currentLevel;
        private Tile hoveredTile;

        private Tile[,] palette;
        private Tile hoveredPalette;
        private Tile selectedPalette;
        private Vector2 palettePosition;

        public Editor(SpriteSheet palette)
        {
            this.palette = new Tile[palette.Columns, palette.Rows];
            for (int y = 0; y < palette.Rows; y++)
            {
                for (int x = 0; x < palette.Columns; x++)
                {
                    this.palette[x, y] = new Tile(' ',palette.GetAt(x, y), new Vector2(x * 32, y * 32), (int)palette.Sprite.SpriteSize.X);
                }
            }

            this.palettePosition = new Vector2(332, 0);
            this.selectedPalette = this.palette[0, 0];
            CreateNewLevel("", 10, 10);
        }

        public Texture2D DebugTexture
        {
            get;
            set;
        }

        public void CreateNewLevel(string filePath, int columns, int rows)
        {
            Level level = new Level(columns, rows);

            this.currentLevel = level;
        }

        public void LoadLevel()
        {
            Level level = null;


            this.currentLevel = level;        
        }

        public void SaveLevel(Level level, string filePath)
        {
            StreamWriter writer = new StreamWriter(filePath, false);

            writer.WriteLine(level.Width);
            writer.WriteLine(level.Height);

            for (int y = 0; y < level.Height; y++)
            {
                for (int x = 0; x < level.Width; x++)
                {
                    writer.Write(level.TileMap[x, y].Type);
                }
                writer.Write('\n');
            }

            writer.Close();
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
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach(Tile tile in currentLevel.TileMap)
            {

                spriteBatch.Draw(DebugTexture, tile.Position, null, Color.White, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 1.0f);

                if (tile.Sprite == null)
                {
                    continue;
                }
                tile.Sprite.Draw(spriteBatch, tile.Position, Vector2.One, SpriteEffects.None, Color.White);
            }

            foreach(Tile tile in this.palette)
            {
                Color color = Color.White;
                if(tile == hoveredPalette)
                {
                    color = Color.Gray;
                }
                if(tile == selectedPalette)
                {
                    color = Color.Black;
                }
                tile.Sprite.Draw(spriteBatch, palettePosition + tile.Position, Vector2.One, SpriteEffects.None, color);
            }
        }

    }
}
