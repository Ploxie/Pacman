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
        private SpriteSheet tileSheet;
        private int width, height;
        
        public Level(SpriteSheet spritesheet)
        {
            this.tileSheet = spritesheet;
        }
        
        public Level(SpriteSheet spritesheet, int columns, int rows)
        {
            this.tileSheet = spritesheet;
            this.tileMap = new Tile[columns, rows];

            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < columns; x++)
                {
                    tileMap[x, y] = new Tile(false, null, new Vector2(x * 32, y * 32) * Game1.Scale, (int)(32 * Game1.Scale.X));
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

        public void LoadLevel(string filePath)
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
                    tileMap[x, y] = new Tile(c == '0', null, new Vector2(x, y) * tileSize, tileSize);
                    x++;
                }
                y++;
            }
            reader.Close();

            CalculateSprites();
        }

        public void CalculateSprites()
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    bool leftBlocked = true;
                    bool rightBlocked = true;
                    bool topBlocked = true;
                    bool bottomBlocked = true;

                    if(tileMap[x, y].Blocked)
                    {
                        tileMap[x, y].Sprite = Tile.NULL_SPRITE;
                        continue;
                    }

                    if (x - 1 >= 0 && !tileMap[x - 1, y].Blocked)
                    {
                        leftBlocked = false;
                    }

                    if (x + 1 < width && !tileMap[x + 1, y].Blocked)
                    {
                        rightBlocked = false;
                    }

                    if (y - 1 >= 0 && !tileMap[x, y - 1].Blocked)
                    {
                        topBlocked = false;
                    }

                    if (y + 1 < height && !tileMap[x, y + 1].Blocked)
                    {
                        bottomBlocked = false;
                    }

                    if(leftBlocked && rightBlocked && topBlocked && bottomBlocked)
                    {
                        tileMap[x, y].Sprite = tileSheet.GetAt(0, 0);
                    }
                    if (!leftBlocked && rightBlocked && topBlocked && bottomBlocked)
                    {
                        tileMap[x, y].Sprite = tileSheet.GetAt(0, 1);
                    }
                    if (leftBlocked && !rightBlocked && topBlocked && bottomBlocked)
                    {
                        tileMap[x, y].Sprite = tileSheet.GetAt(1, 0);
                    }
                    if (leftBlocked && rightBlocked && !topBlocked && bottomBlocked)
                    {
                        tileMap[x, y].Sprite = tileSheet.GetAt(2, 0);
                    }
                    if (leftBlocked && rightBlocked && topBlocked && !bottomBlocked)
                    {
                        tileMap[x, y].Sprite = tileSheet.GetAt(0, 2);
                    }

                    if (!leftBlocked && !rightBlocked && topBlocked && bottomBlocked)
                    {
                        tileMap[x, y].Sprite = tileSheet.GetAt(1, 1);
                    }
                    if (leftBlocked && rightBlocked && !topBlocked && !bottomBlocked)
                    {
                        tileMap[x, y].Sprite = tileSheet.GetAt(2, 2);
                    }

                    if (!leftBlocked && rightBlocked && !topBlocked && bottomBlocked)
                    {
                        tileMap[x, y].Sprite = tileSheet.GetAt(2, 1);
                    }

                    if (leftBlocked && !rightBlocked && topBlocked && !bottomBlocked)
                    {
                        tileMap[x, y].Sprite = tileSheet.GetAt(1, 2);
                    }

                    if (!leftBlocked && rightBlocked && topBlocked && !bottomBlocked)
                    {
                        tileMap[x, y].Sprite = tileSheet.GetAt(0, 3);
                    }

                    if (leftBlocked && !rightBlocked && !topBlocked && bottomBlocked)
                    {
                        tileMap[x, y].Sprite = tileSheet.GetAt(3, 0);
                    }

                    if (!leftBlocked && !rightBlocked && !topBlocked && !bottomBlocked)
                    {
                        tileMap[x, y].Sprite = tileSheet.GetAt(3, 3);
                    }

                    if (!leftBlocked && !rightBlocked && !topBlocked && bottomBlocked)
                    {
                        tileMap[x, y].Sprite = tileSheet.GetAt(3, 1);
                    }

                    if (!leftBlocked && !rightBlocked && topBlocked && !bottomBlocked)
                    {
                        tileMap[x, y].Sprite = tileSheet.GetAt(1, 3);
                    }

                    if (leftBlocked && !rightBlocked && !topBlocked && !bottomBlocked)
                    {
                        tileMap[x, y].Sprite = tileSheet.GetAt(3, 2);
                    }
                    if (!leftBlocked && rightBlocked && !topBlocked && !bottomBlocked)
                    {
                        tileMap[x, y].Sprite = tileSheet.GetAt(2, 3);
                    }
                }
            }
        }

        public Tile GetAt(int x, int y)
        {
            if(x < 0 || x >= width || y < 0 || y >= height)
            {
                return null;
            }
            return tileMap[x, y];
        }

        public Tile GetAt(Vector2 position)
        {
            return GetAt((int)position.X, (int)position.Y);
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
