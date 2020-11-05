using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace PacMan
{
    class Powerup
    {
        public enum PowerUpType { WallEater, GhostEater, Food }
        PowerUpType type;

        Vector2 position;
        Sprite texture;
        Tile myTile;
        int score;


        public Powerup(Vector2 position, Sprite texture, Tile tile, int score, PowerUpType type)
        {
            this.position = position;
            this.texture = texture;
            this.myTile = tile;
            this.score = score;
            this.type = type;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            texture.Draw(spriteBatch, position, Game1.Scale, SpriteEffects.None, Color.White);
        }
    }
}
