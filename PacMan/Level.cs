﻿using Microsoft.Xna.Framework;
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
        private int tileSize;

        private Sprite foodSprite;

        private Level(SpriteSheet tileSheet,SpriteSheet sprites, int columns, int rows, int tileSize)
        {
            this.tileSize = tileSize;
            this.tileSheet = tileSheet;
            this.tileMap = new Tile[columns, rows];

            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < columns; x++)
                {
                    tileMap[x, y] = new Tile(false, null, new Vector2(x * tileSize, y * tileSize) * Game1.Scale, (int)(tileSize * Game1.Scale.X));
                }
            }


            width = columns;
            height = rows;

            this.foodSprite = sprites.GetAt(4, 0);
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

        public int TileSize
        {
            get { return tileSize; }
        }

        public int PixelWidth
        {
            get { return (int)(Width * tileSize * Game1.Scale.X); } 
        }

        public int PixelHeight
        {
            get { return (int)(Height * tileSize * Game1.Scale.Y); } 
        }

        public static Level CreateLevel(SpriteSheet spritesheet,SpriteSheet sprites, int width, int height, int tileSize)
        {
            return new Level(spritesheet, sprites, width, height, tileSize);
        }

        public static Level LoadLevel(SpriteSheet spritesheet,SpriteSheet sprites, string filePath)
        {

            StreamReader reader = new StreamReader(filePath);
            int x = 0;
            int y = 0;
            int width = int.Parse(reader.ReadLine());
            int height = int.Parse(reader.ReadLine());
            int tileSize = int.Parse(reader.ReadLine());

            Level level = new Level(spritesheet,sprites, width, height, tileSize);

            level.tileMap = new Tile[width, height];
            
            string currentLine;
            while ((currentLine = reader.ReadLine()) != null && y < height)
            {
                x = 0;
                foreach (char c in currentLine)
                {
                    int value = int.Parse(c.ToString());
                    bool blocked = value == 1;
                    bool food = value == 2;
                    bool powerupGhost = value == 3;
                    bool powerupWall = value == 4;
                    bool pacspawn = value == 5;
                    bool ghostspawn = value == 6;

                    level.tileMap[x, y] = new Tile(blocked, null, new Vector2(x, y) * tileSize, tileSize);
                    level.tileMap[x, y].HasFood = food;
                    x++;
                }
                y++;
            }
            reader.Close();

            level.CalculateSprites();

            return level;
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

                    if(!tileMap[x, y].Blocked)
                    {
                        tileMap[x, y].Sprite = Tile.NULL_SPRITE;
                        continue;
                    }

                    if (x - 1 >= 0 && tileMap[x - 1, y].Blocked)
                    {
                        leftBlocked = false;
                    }

                    if (x + 1 < width && tileMap[x + 1, y].Blocked)
                    {
                        rightBlocked = false;
                    }

                    if (y - 1 >= 0 && tileMap[x, y - 1].Blocked)
                    {
                        topBlocked = false;
                    }

                    if (y + 1 < height && tileMap[x, y + 1].Blocked)
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

        public Tile GetTile(int x, int y)
        {
            if (x < 0 || x >= width || y < 0 || y >= height)
            {
                return null;
            }
            return tileMap[x, y];
        }

        public Tile GetTile(Vector2 indices)
        {
            return GetTile((int)indices.X, (int)indices.Y);
        }

        public Tile GetAt(float x, float y)
        {
            int xIndex = (int)(x / (tileSize * Game1.Scale.X));
            int yIndex = (int)(y / (tileSize * Game1.Scale.Y));

            return GetTile(xIndex, yIndex);
        }

        public Tile GetAt(Vector2 position)
        {
            return GetAt(position.X, position.Y);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach(Tile tile in tileMap)
            {
                tile.Sprite.Draw(spriteBatch, tile.Position, Game1.Scale, SpriteEffects.None, Color.White);
                
                if(tile.HasFood)
                {
                    foodSprite.Draw(spriteBatch, tile.Position + new Vector2(TileSize / 2), Game1.Scale, new Vector2(0.5f));
                }
            }
        }
               
    }
}
