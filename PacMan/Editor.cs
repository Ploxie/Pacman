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
        
        private Vector2 levelPosition;

        public Editor(SpriteSheet spritesheet, GameWindow window)
        {
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

        public Texture2D SelectedTexture
        {
            get;
            set;
        }
        
        public Level CreateNewLevel(string filePath, int columns, int rows, int tileSize)
        {
            Level level = Level.CreateLevel(spritesheet, columns, rows, tileSize);
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
                    writer.Write(currentLevel.TileMap[x, y].Blocked ? '1' : '0');
                }
                writer.Write('\n');
            }

            writer.Close();

            System.Diagnostics.Debug.WriteLine("Level saved: " + filePath);
        }

        public void Update(GameTime gameTime)
        {
            Vector2 mousePoint = new Vector2(Mouse.GetState().X, Mouse.GetState().Y) - levelPosition;
           
            hoveredTile = null;
            foreach (Tile tile in currentLevel.TileMap)
            {
                if (tile.Bounds.Contains(mousePoint))
                {
                    hoveredTile = tile;
                    if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                    {
                        tile.Blocked = true;
                        currentLevel.CalculateSprites();
                    }
                    else if (Mouse.GetState().RightButton == ButtonState.Pressed)
                    {
                        tile.Blocked = false;
                        currentLevel.CalculateSprites();
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
                }

                if (tile.Sprite == null)
                {
                    continue;
                }
                tile.Sprite.Draw(spriteBatch, levelPosition + tile.Position, Game1.Scale, SpriteEffects.None, Color.White);
            }
        }

    }
}
