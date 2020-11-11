using Microsoft.Xna.Framework;
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
        private GhostBehaviour runBehaviour;
        private SpriteSheet defaultSpriteSheet;
        private SpriteSheet runSpriteSheet;

        public Ghost(SpriteSheet spriteSheet,SpriteSheet runSpriteSheet, Level level, Pacman pacman, GhostBehaviour behaviour) : base(spriteSheet, level) 
        { 
            this.pacman = pacman;
            this.direction = new Vector2(0, -1);

            this.behaviour = behaviour;
            this.behaviour.Ghost = this;
            this.runBehaviour = new GhostRunAwayBehaviour(pacman, level);
            this.runBehaviour.Ghost = this;

            this.defaultSpriteSheet = spriteSheet;
            this.runSpriteSheet = runSpriteSheet;
        }

        public bool Dead
        {
            get;
            set;
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
            if (pacman.ActivePowerupType != PowerUpType.GhostEater)
            {
                this.spritesheet = defaultSpriteSheet;
                ChangeDirection(behaviour.CalculateDirection());
            }
            else
            {
                this.spritesheet = runSpriteSheet;
                ChangeDirection(runBehaviour.CalculateDirection());
            }
            

            UpdateMovement(gameTime);
            UpdateAnimationTimer(gameTime);

            if (Dead)
            {
                position = level.GhostSpawns[0].Position + new Vector2(level.TileSize/2, level.TileSize/2);
                Dead = false;
            }
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            spritesheet.Sprite.Draw(spriteBatch, offset + Position, Game1.Scale * 2, new Vector2(0.5f, 0.5f));
        }
    }

}
