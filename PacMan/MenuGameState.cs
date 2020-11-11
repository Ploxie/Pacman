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
        private Texture2D winScreen;
        private Texture2D loseScreen;

        public enum Option { StartGame, Editor, EndGame, RestartGame }
        public Option CurrentOption = Option.StartGame;
        private double clickTimer;

        private SpriteFont menuFont;

        private Vector2 startPosition;
        private Vector2 editorPosition;
        private Vector2 endPosition;
        private Vector2 restartPosition;

        private GameWindow window;
        private HUD hud;

        public MenuGameState(HUD hud, Texture2D background, Texture2D winScreen, Texture2D loseScreen, SpriteFont font, GameWindow window)
        {
            this.hud = hud;
            this.menuFont = font;
            clickTimer = 0;

            this.window = window;

            this.background = background;
            this.winScreen = winScreen;
            this.loseScreen = loseScreen;

            this.Highscores = new List<int>();
        }

        public bool Restart
        {
            get;
            set;
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

        public bool LoseScreen
        {
            get { return hud.Pacman.Lives <= 0; }
        }

        public bool WinScreen
        {
            get;
            set;
        }

        public List<int> Highscores
        {
            get;
            set;
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
            for (int i = 0; i < 7; i++)
            {
                if (i >= Highscores.Count)
                {
                    break;
                }

                Vector2 highscorePosition = new Vector2(x - (scoreLength / 2), y);

                hud.DrawScore(spriteBatch, Highscores[i], highscorePosition, Game1.Scale * 2.0f, (Highscores[i] == hud.Pacman.Score && !hasShownYourScore) ? Color.Yellow : Color.White);
                if (Highscores[i] == hud.Pacman.Score)
                {
                    hasShownYourScore = true;
                }
                y += 25;
            }
        }

        public void MenuBaseUpdate(GameTime gameTime) 
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

        public void GameOverUpdate(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Down) || Keyboard.GetState().IsKeyDown(Keys.Up) && clickTimer <= 0)
            {
                if (CurrentOption == Option.EndGame)
                {
                    CurrentOption = Option.RestartGame;
                }
                else if (CurrentOption == Option.RestartGame)
                {
                    CurrentOption = Option.EndGame;
                }
                clickTimer = 100;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Enter) && clickTimer <= 0)
            {
                if (CurrentOption == Option.RestartGame)
                {
                    WinScreen = false;
                    Restart = true;
                }
                else if (CurrentOption == Option.EndGame)
                {
                    Quit = true;
                }
                clickTimer = 100;
            }
            clickTimer -= gameTime.ElapsedGameTime.TotalMilliseconds;
        }

        public void Update(GameTime gameTime)
        {
            if (!WinScreen && !LoseScreen)
            {
                MenuBaseUpdate(gameTime);
            }
            else
            {
                GameOverUpdate(gameTime);
            }
        }

        public void MenuBaseDraw(SpriteBatch spriteBatch)
        {
            startPosition = new Vector2(window.ClientBounds.Width / 2.5f, window.ClientBounds.Height / 1.7f + 100);
            editorPosition = new Vector2(window.ClientBounds.Width / 2.5f, window.ClientBounds.Height / 1.6f + 100);
            endPosition = new Vector2(window.ClientBounds.Width / 2.5f, window.ClientBounds.Height / 1.5f + 100);


            spriteBatch.Draw(background, new Vector2(50, 100), Color.White);
            if (CurrentOption == Option.StartGame)
            {
                spriteBatch.DrawString(menuFont, "Start Game", startPosition, Color.Red);
                spriteBatch.DrawString(menuFont, "Level Editor", editorPosition, Color.White);
                spriteBatch.DrawString(menuFont, "End Game", endPosition, Color.White);
            }
            else if (CurrentOption == Option.Editor)
            {
                spriteBatch.DrawString(menuFont, "Start Game", startPosition, Color.White);
                spriteBatch.DrawString(menuFont, "Level Editor", editorPosition, Color.Red);
                spriteBatch.DrawString(menuFont, "End Game", endPosition, Color.White);
            }
            else if (CurrentOption == Option.EndGame)
            {
                spriteBatch.DrawString(menuFont, "Start Game", startPosition, Color.White);
                spriteBatch.DrawString(menuFont, "Level Editor", editorPosition, Color.White);
                spriteBatch.DrawString(menuFont, "End Game", endPosition, Color.Red);
            }
        }

        public void GameOverDraw(SpriteBatch spriteBatch)
        {
            restartPosition = new Vector2(window.ClientBounds.Width / 2.5f, window.ClientBounds.Height / 1.6f + 100);
            endPosition = new Vector2(window.ClientBounds.Width / 2.5f, window.ClientBounds.Height / 1.5f + 100);

            if (WinScreen)
            {
                spriteBatch.Draw(winScreen, new Vector2(190, 80), null, Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 1.0f);
            }
            if (LoseScreen)
            {
                spriteBatch.Draw(loseScreen, new Vector2(50, 100), Color.White);
            }
            if (CurrentOption == Option.RestartGame)
            {
                spriteBatch.DrawString(menuFont, "Restart Game", restartPosition, Color.Red);
                spriteBatch.DrawString(menuFont, "End Game", endPosition, Color.White);
                
            }
            else if (CurrentOption == Option.EndGame)
            {
                spriteBatch.DrawString(menuFont, "Restart Game", restartPosition, Color.White);
                spriteBatch.DrawString(menuFont, "End Game", endPosition, Color.Red);
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!LoseScreen && !WinScreen)
            {
                MenuBaseDraw(spriteBatch);
            }
            else
            {
                GameOverDraw(spriteBatch);
            }

            Vector2 highscorePosition = new Vector2(window.ClientBounds.Width / 2, window.ClientBounds.Height / 2 - 50);
            DrawHighscores(spriteBatch, highscorePosition);
        }
    }
}
