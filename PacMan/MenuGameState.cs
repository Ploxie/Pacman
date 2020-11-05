using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace PacMan
{
    public class MenuGameState : GameState
    {
        private SpriteSheet background;
        private float msSinceLastFrame = 0;
        private float msPerFrame = 100;

        private enum Option { StartGame, SwitchCharacter, EndGame }
        private Option CurrentOption = Option.StartGame;
        private double clickTimer;

        private SpriteFont menuFont;

        private Vector2 startPosistion;
        private Vector2 switchPosition;
        private Vector2 endPosition;

        private GameWindow window;

        public MenuGameState(Texture2D start, SpriteFont font, GameWindow window)
        {
            this.menuFont = font;
            clickTimer = 0;

            this.window = window;

            background = new SpriteSheet(start, new Vector2(0, 0), new Vector2(2510, 5936), new Vector2(502, 742));
        }

        public bool Quit
        {
            get;
            set;
        }

        public void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Down) && clickTimer <= 0)
            {
                if (CurrentOption == Option.StartGame)
                {
                    CurrentOption = Option.SwitchCharacter;
                }
                else if (CurrentOption == Option.SwitchCharacter)
                {
                    CurrentOption = Option.EndGame;
                }
                else if (CurrentOption == Option.EndGame)
                {
                    CurrentOption = Option.StartGame;
                }
                clickTimer = 100;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Up) && clickTimer <= 0)
            {
                if (CurrentOption == Option.StartGame)
                {
                    CurrentOption = Option.EndGame;
                }
                else if (CurrentOption == Option.SwitchCharacter)
                {
                    CurrentOption = Option.StartGame;
                }
                else if (CurrentOption == Option.EndGame)
                {
                    CurrentOption = Option.SwitchCharacter;
                }
                clickTimer = 100;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Enter) && clickTimer <= 0)
            {
                if (CurrentOption == Option.StartGame)
                {
                    
                }
                else if (CurrentOption == Option.SwitchCharacter)
                {
                   
                }
                else if (CurrentOption == Option.EndGame)
                {
                    Quit = true;
                }
                clickTimer = 100;
            }
            if (msSinceLastFrame >= msPerFrame)
            {
                background.XIndex++;
                if (background.XIndex >= 4)
                {
                    background.XIndex = 0;
                    background.YIndex++;
                }
                if (background.YIndex == 7 && background.XIndex == 3)
                {
                    background.YIndex = 0;
                    background.XIndex = 0;
                }
                msSinceLastFrame = 0;
            }
            msSinceLastFrame += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            clickTimer -= gameTime.ElapsedGameTime.TotalMilliseconds;
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            startPosistion = new Vector2(window.ClientBounds.Width / 2.5f, window.ClientBounds.Height / 5.6f);
            switchPosition = new Vector2(window.ClientBounds.Width / 2.5f, window.ClientBounds.Height / 4.6f);
            endPosition = new Vector2(window.ClientBounds.Width / 2.5f, window.ClientBounds.Height / 3.9f);


            background.Sprite.Draw(spriteBatch, Vector2.Zero, new Vector2(2.5f, 1.5f));
            if (CurrentOption == Option.StartGame)
            {
                spriteBatch.DrawString(menuFont, "Start Game", startPosistion, Color.Red);
                spriteBatch.DrawString(menuFont, "Editor", switchPosition, Color.Black);
                spriteBatch.DrawString(menuFont, "End Game", endPosition, Color.Black);
            }
            else if (CurrentOption == Option.SwitchCharacter)
            {
                spriteBatch.DrawString(menuFont, "Start Game", startPosistion, Color.Black);
                spriteBatch.DrawString(menuFont, "Editor", switchPosition, Color.Red);
                spriteBatch.DrawString(menuFont, "End Game", endPosition, Color.Black);
            }
            else if (CurrentOption == Option.EndGame)
            {
                spriteBatch.DrawString(menuFont, "Start Game", startPosistion, Color.Black);
                spriteBatch.DrawString(menuFont, "Editor", switchPosition, Color.Black);
                spriteBatch.DrawString(menuFont, "End Game", endPosition, Color.Red);
            }
        }
    }
}
