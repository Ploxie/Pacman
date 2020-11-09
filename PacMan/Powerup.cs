﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace PacMan
{
    public enum PowerUpType 
    {
        Food,
        BigFood,
        WallEater, 
        GhostEater, 
    }
    public class Powerup
    {              
        public Powerup(PowerUpType type, Sprite sprite, int score)
        {
            Type = type;
            Sprite = sprite;
            Score = score;
        }

        public PowerUpType Type
        {
            get;
            private set;
        }

        public Sprite Sprite
        {
            get;
            private set;
        }

        public int Score
        {
            get;
            private set;
        }
    }
}
