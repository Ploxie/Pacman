using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PacMan
{
    
    public class Level
    {

        private Tile[,] tileMap;
        private int width, height;
        
        public Level()
        {

        }
        
        public Level(int columns, int rows)
        {
            this.tileMap = new Tile[columns, rows];

            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < columns; x++)
                {
                    tileMap[x, y] = new Tile(Tile.EMPTY_TYPE, null, new Vector2(x * 32, y * 32) * Game1.Scale, (int)(32 * Game1.Scale.X));
                }
            }


            width = columns;
            height = rows;
        }

        public Tile[,] TileMap
        {
            get { return this.tileMap; }
        }

        public int Width
        {
            get { return width; }
        }

        public int Height
        {
            get { return height; }
        }

        public int PixelWidth
        {
            get { return (int)(Width * 32 * Game1.Scale.X); } 
        }

        public int PixelHeight
        {
            get { return (int)(Height * 32 * Game1.Scale.Y); } 
        }

        public void LoadLevel(SpriteSheet spritesheet, string filePath)
        {
            StreamReader reader = new StreamReader(filePath);
            int x = 0;
            int y = 0;
            width = int.Parse(reader.ReadLine());
            height = int.Parse(reader.ReadLine());
            tileMap = new Tile[width, height];

            int tileSize = (int)(32 * Game1.Scale.X);

            string currentLine;
            while ((currentLine = reader.ReadLine()) != null && y < height)
            {
                x = 0;
                foreach (char c in currentLine)
                {
                    switch (c)
                    {
                        case '-':
                            tileMap[x, y] = new Tile(c, null, new Vector2(x, y) * tileSize, tileSize);
                            break;
                        case '0':
                            tileMap[x, y] = new Tile(c, spritesheet.GetAt(0, 0), new Vector2(x, y) * tileSize, tileSize);
                            break;
                        case '1':
                            tileMap[x, y] = new Tile(c, spritesheet.GetAt(1, 0), new Vector2(x, y) * tileSize, tileSize);
                            break;
                        case '2':
                            tileMap[x, y] = new Tile(c, spritesheet.GetAt(2, 0), new Vector2(x, y) * tileSize, tileSize);
                            break;
                        case '3':
                            tileMap[x, y] = new Tile(c, spritesheet.GetAt(3, 0), new Vector2(x, y) * tileSize, tileSize);
                            break;
                        case '4':
                            tileMap[x, y] = new Tile(c, spritesheet.GetAt(0, 1), new Vector2(x, y) * tileSize, tileSize);
                            break;
                        case '5':
                            tileMap[x, y] = new Tile(c, spritesheet.GetAt(1, 1), new Vector2(x, y) * tileSize, tileSize);
                            break;
                        case '6':
                            tileMap[x, y] = new Tile(c, spritesheet.GetAt(2, 1), new Vector2(x, y) * tileSize, tileSize);
                            break;
                        case '7':
                            tileMap[x, y] = new Tile(c, spritesheet.GetAt(3, 1), new Vector2(x, y) * tileSize, tileSize);
                            break;
                        case '8':
                            tileMap[x, y] = new Tile(c, spritesheet.GetAt(0, 2), new Vector2(x, y) * tileSize, tileSize);
                            break;
                        case '9':
                            tileMap[x, y] = new Tile(c, spritesheet.GetAt(1, 2), new Vector2(x, y) * tileSize, tileSize);
                            break;
                        case 'A':
                            tileMap[x, y] = new Tile(c, spritesheet.GetAt(2, 2), new Vector2(x, y) * tileSize, tileSize);
                            break;
                        case 'B':
                            tileMap[x, y] = new Tile(c, spritesheet.GetAt(3, 2), new Vector2(x, y) * tileSize, tileSize);
                            break;
                        case 'C':
                            tileMap[x, y] = new Tile(c, spritesheet.GetAt(0, 3), new Vector2(x, y) * tileSize, tileSize);
                            break;
                        case 'D':
                            tileMap[x, y] = new Tile(c, spritesheet.GetAt(1, 3), new Vector2(x, y) * tileSize, tileSize);
                            break;
                        case 'E':
                            tileMap[x, y] = new Tile(c, spritesheet.GetAt(2, 3), new Vector2(x, y) * tileSize, tileSize);
                            break;
                        case 'F':
                            tileMap[x, y] = new Tile(c, spritesheet.GetAt(3, 3), new Vector2(x, y) * tileSize, tileSize);
                            break;
                        default:
                            tileMap[x, y] = new Tile(c, null, new Vector2(x, y) * tileSize, tileSize);
                            break;
                    }

                    x++;
                }
                y++;
            }
            reader.Close();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach(Tile tile in tileMap)
            {
                tile.Sprite.Draw(spriteBatch, tile.Position, Game1.Scale, SpriteEffects.None, Color.White);
            }
        }
               
    }
}
