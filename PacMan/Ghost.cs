using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PacMan.GhostBehaviours;
using System;
using System.Collections.Generic;
using System.Text;

namespace PacMan
{
    public class Ghost : Character
    {
        private Pacman pacman;

        public enum AItype {PathFinding, FullyRandom, Patrolling};
        private AItype type;

        private GhostBehaviour behaviour;

        public Ghost(SpriteSheet spriteSheet, Level level, Pacman pacman, GhostBehaviour behaviour) : base(spriteSheet, level) 
        { 
            this.pacman = pacman;
            direction = new Vector2(0, -1);

            this.behaviour = behaviour;
            this.behaviour.Ghost = this;
        }

        protected override void UpdateAnimation() { }

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
