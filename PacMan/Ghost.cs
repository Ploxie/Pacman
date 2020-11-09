﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PacMan.GhostBehaviours;
using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;

namespace PacMan
{
    public class Ghost : Character
    {
        private Pacman pacman;        
        private GhostBehaviour behaviour;

        public Ghost(SpriteSheet spriteSheet, Level level, Pacman pacman, GhostBehaviour behaviour) : base(spriteSheet, level) 
        { 
            this.pacman = pacman;
            this.direction = new Vector2(0, -1);

            this.behaviour = behaviour;
            this.behaviour.Ghost = this;
        }

        protected override void UpdateAnimation() 
        {
            if (direction.X > 0)
            {
                if (spritesheet.XIndex == 0)
                {
                    spritesheet.XIndex = 1;
                }
                else
                {
                    spritesheet.XIndex = 0;
                }
            }
            else if (direction.X < 0)
            {
                if (spritesheet.XIndex == 2)
                {
                    spritesheet.XIndex = 3;
                }
                else
                {
                    spritesheet.XIndex = 2;
                }
            }
            else if (direction.Y < 0)
            {
                if (spritesheet.XIndex == 4)
                {
                    spritesheet.XIndex = 5;
                }
                else
                {
                    spritesheet.XIndex = 4;
                }
            }
            else if (direction.Y > 0)
            {
                if (spritesheet.XIndex == 6)
                {
                    spritesheet.XIndex = 7;
                }
                else
                {
                    spritesheet.XIndex = 6;
                }
            }

        }

        public override void Update(GameTime gameTime)
        {

            ChangeDirection(behaviour.CalculateDirection());

            UpdateMovement(gameTime);
            UpdateAnimationTimer(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            spritesheet.Sprite.Draw(spriteBatch, offset + Position, Game1.Scale * 2, new Vector2(0.5f, 0.5f));
        }
    }

}
