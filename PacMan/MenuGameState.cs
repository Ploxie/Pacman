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
        private Texture2D background;
        private float msSinceLastFrame = 0;
        private float msPerFrame = 100;

        private enum Option { StartGame, Editor, EndGame }
        private Option CurrentOption = Option.StartGame;
        private double clickTimer;

        private SpriteFont menuFont;

        private Vector2 startPosistion;
        private Vector2 editorPosition;
        private Vector2 endPosition;

        private GameWindow window;

        public MenuGameState(Texture2D start, SpriteFont font, GameWindow window)
        {
            this.menuFont = font;
            clickTimer = 0;

            this.window = window;

            background = start;
        }

        public bool Quit
        {
            get;
            set;
        }

        public bool Play
        {
            get;
            set;
        }

        public bool Editor
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
                    CurrentOption = Option.Editor;
                }
                else if (CurrentOption == Option.Editor)
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
                else if (CurrentOption == Option.Editor)
                {
                    CurrentOption = Option.StartGame;
                }
                else if (CurrentOption == Option.EndGame)
                {
                    CurrentOption = Option.Editor;
                }
                clickTimer = 100;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Enter) && clickTimer <= 0)
            {
                if (CurrentOption == Option.StartGame)
                {
                    Play = true;
                }
                else if (CurrentOption == Option.Editor)
                {
                    Editor = true;
                }
                else if (CurrentOption == Option.EndGame)
                {
                    Quit = true;
                }
                clickTimer = 100;
            }
            clickTimer -= gameTime.ElapsedGameTime.TotalMilliseconds;
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            startPosistion = new Vector2(window.ClientBounds.Width / 2.5f, window.ClientBounds.Height / 1.7f);
            editorPosition = new Vector2(window.ClientBounds.Width / 2.5f, window.ClientBounds.Height / 1.6f);
            endPosition = new Vector2(window.ClientBounds.Width / 2.5f, window.ClientBounds.Height / 1.5f);


            spriteBatch.Draw(background, new Vector2(50,100), Color.White);
            if (CurrentOption == Option.StartGame)
            {
                spriteBatch.DrawString(menuFont, "Start Game", startPosistion, Color.Red);
                spriteBatch.DrawString(menuFont, "Level Editor", editorPosition, Color.White);
                spriteBatch.DrawString(menuFont, "End Game", endPosition, Color.White);
            }
            else if (CurrentOption == Option.Editor)
            {
                spriteBatch.DrawString(menuFont, "Start Game", startPosistion, Color.White);
                spriteBatch.DrawString(menuFont, "Level Editor", editorPosition, Color.Red);
                spriteBatch.DrawString(menuFont, "End Game", endPosition, Color.White);
            }
            else if (CurrentOption == Option.EndGame)
            {
                spriteBatch.DrawString(menuFont, "Start Game", startPosistion, Color.White);
                spriteBatch.DrawString(menuFont, "Level Editor", editorPosition, Color.White);
                spriteBatch.DrawString(menuFont, "End Game", endPosition, Color.Red);
            }
        }
    }
}
