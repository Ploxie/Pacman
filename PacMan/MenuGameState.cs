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
        private HUD hud;

        public MenuGameState(HUD hud, Texture2D start, SpriteFont font, GameWindow window)
        {
            this.hud = hud;
            this.menuFont = font;
            clickTimer = 0;

            this.window = window;

            background = start;
            this.Highscores = new List<int>();
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

        public List<int> Highscores
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

        private void DrawHighscores(SpriteBatch spriteBatch, Vector2 position)
        {
            float x = position.X;
            float y = position.Y;

            hud.HighscoreSprite.Draw(spriteBatch, new Vector2(x - (hud.HighscoreSprite.SpriteSize.X * Game1.Scale.X), y), Game1.Scale * 2);
            y += (hud.HighscoreSprite.SpriteSize.Y * Game1.Scale.Y) + 25;

            int scoreLength = (int)(8 * Game1.Scale.X * 2 * 6);

            if (Highscores.Count == 0)
            {
                Vector2 highscorePosition = new Vector2(x - (scoreLength / 2), y);
                hud.DrawScore(spriteBatch, 0, highscorePosition, Game1.Scale * 2.0f, Color.White);
                return;
            }

            bool hasShownYourScore = false;
            for(int i = 0; i < 7; i++)
            {                
                if(i >= Highscores.Count)
                {
                    break;
                }

                Vector2 highscorePosition = new Vector2(x - (scoreLength / 2), y);

                hud.DrawScore(spriteBatch, Highscores[i], highscorePosition, Game1.Scale * 2.0f, (Highscores[i] == hud.Pacman.Score && !hasShownYourScore) ? Color.Yellow : Color.White);
                if (Highscores[i] == hud.Pacman.Score)
                {
                    hasShownYourScore = true;
                }
                y  +=  25;
            }     
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            startPosistion = new Vector2(window.ClientBounds.Width / 2.5f, window.ClientBounds.Height / 1.7f + 100);
            editorPosition = new Vector2(window.ClientBounds.Width / 2.5f, window.ClientBounds.Height / 1.6f + 100);
            endPosition = new Vector2(window.ClientBounds.Width / 2.5f, window.ClientBounds.Height / 1.5f + 100);


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

            Vector2 highscorePosition = new Vector2(window.ClientBounds.Width / 2, window.ClientBounds.Height / 2 - 50);
            DrawHighscores(spriteBatch, highscorePosition);
        }
    }
}
