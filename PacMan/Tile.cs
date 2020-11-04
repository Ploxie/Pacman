using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace PacMan
{
    class Tile
    {
        public static readonly char EMPTY_TYPE = '-';

        private Sprite sprite;
        private Vector2 position;
        private int size;
        private char type;
        private byte collisionData;

        public Tile(char type, Sprite sprite, Vector2 position, int size)
        {
            this.type = type;
            this.sprite = sprite;
            this.position = position;
            this.size = size;

            this.collisionData = GetCollisionData(type);

        }
        
        public static byte GetCollisionData(char c)
        {

            // 0001 = block north
            // 0010 = block south
            // 0100 = block west
            // 1000 = block east
            switch (c)
            {
                case '0':
                    return 0b00001111;
                case '1':
                    return 0b00000111;
                case '2':
                    return 0b00001110;
                case '3':
                    return 0b00000110;
                case '4':
                    return 0b00001011;
                case '5':
                    return 0b00000011;
                case '6':
                    return 0b00001010;
                case '7':
                    return 0b00000010;
                case '8':
                    return 0b00001101;
                case '9':
                    return 0b00000101;
                case 'A':
                    return 0b00001100;
                case 'B':
                    return 0b00000100;
                case 'C':
                    return 0b00001001;
                case 'D':
                    return 0b00000001;
                case 'E':
                    return 0b00001000;
                case 'F':
                    return 0b00000000;

            }
            
            return 0;
        }

        public bool BlockedNorth()
        {
            return (collisionData & 1) != 1;
        }

        public bool BlockedSouth()
        {
            return (collisionData & 2) != 2;
        }

        public bool BlockedWest()
        {
            return (collisionData & 4) != 4;
        }

        public bool BlockedEast()
        {
            return (collisionData & 8) != 8;           
        }
        
        public char Type
        {
            get { return type; }
            set { this.type = value; }
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

    }
}
