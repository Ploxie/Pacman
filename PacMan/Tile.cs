using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace PacMan
{
    public class Tile
    {
        public static Sprite NULL_SPRITE;

        private Sprite sprite;
        private Vector2 position;
        private int size;
        private bool blocked;

        public Tile(bool blocked, Sprite sprite, Vector2 position, int size)
        {
            this.blocked = blocked;
            this.sprite = sprite;
            this.position = position;
            this.size = size;
        }
        
        public bool Blocked
        {
            get { return blocked; }
            set { blocked = value; }
        }

        public Sprite Sprite
        {
            get { return this.sprite; }
            set { this.sprite = value; }
        }

        public Vector2 Position
        {
            get { return this.position; }
        }

        public Rectangle Bounds
        {
            get { return new Rectangle((int)position.X, (int)position.Y, size, size); }
        }

        public bool HasFood
        {
            get;
            set;
        }

    }
}
