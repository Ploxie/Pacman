﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace PacMan
{
    public class HUD
    {

        private GameWindow window;
        private int topHeight;
        private int bottomHeight;

        private SpriteSheet scoresheet;
        private Sprite highscoreSprite;
        private Sprite lifeSprite;

        public HUD(GameWindow window, int topHeight, int bottomHeight, Texture2D numberTexture, Sprite lifeSprite)
        {
            this.window = window;
            this.topHeight = topHeight;
            this.bottomHeight = bottomHeight;

            SpriteSheet highscoreSpriteSheet = new SpriteSheet(numberTexture, new Vector2(51, 1), new Vector2(78, 7), new Vector2(78, 7));
            highscoreSprite = highscoreSpriteSheet.Sprite;

            this.scoresheet = new SpriteSheet(numberTexture, new Vector2(132, 1), new Vector2(35, 14), new Vector2(7, 7));

            this.lifeSprite = lifeSprite;
        }

        public Level CurrentLevel
        {
            get;
            set;
        }

        public Pacman Pacman
        {
            get;
            set;
        }

        private Sprite GetNumberSprite(int number)
        {
            if (number <= 0 || number > 9)
            {
                return scoresheet.GetAt(0, 0);
            }

            int x = number % 5;
            int y = number > 4 ? 1 : 0;

            return scoresheet.GetAt(x, y);
        }

        public void DrawScore(SpriteBatch spriteBatch, int score, Vector2 position, Vector2 scale)
        {
            string valueString = score.ToString();
            int[] valueNumbers = new int[6];

            int numberIndex = valueString.Length - 1;
            for (int i = valueNumbers.Length - 1; i >= valueNumbers.Length - valueString.Length; i--)
            {
                char c = valueString[numberIndex--];
                valueNumbers[i] = (int)Char.GetNumericValue(c);
            }

            for (int i = 0; i < 6; i++)
            {
                Sprite sprite = GetNumberSprite(valueNumbers[i]);
                int size = (int)(sprite.SpriteSize.X * scale.X) + 1;
                int x = i * size;
                sprite.Draw(spriteBatch, new Vector2(position.X + x, position.Y), scale, SpriteEffects.None);
            }
        }

        public void DrawLives(SpriteBatch spriteBatch, int lives, Vector2 position, Vector2 scale)
        {
            for (int i = 0; i < lives; i++)
            {
                int size = (int)(lifeSprite.SpriteSize.X * scale.X) + 1;
                int x = i * size;
                lifeSprite.Draw(spriteBatch, new Vector2(position.X + x, position.Y), scale);
            }
        }

        public void DrawGainedScore(SpriteBatch spriteBatch, int score, Vector2 scale, Vector2 position)
        {
            DrawScore(spriteBatch, score, position, scale);
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {

            float uiScale = Game1.Scale.X * 2.0f;
            Rectangle topRectangle = new Rectangle(0, 0, window.ClientBounds.Width, topHeight);

            Vector2 topHalfPosition = new Vector2((topRectangle.Width / 2) - ((highscoreSprite.SpriteSize.X * uiScale) / 2), topRectangle.X + 15);
            highscoreSprite.Draw(spriteBatch, topHalfPosition, Game1.Scale * uiScale);

            float scoreWidth = 6.0f * 7.0f * uiScale + 6.0f;
            float scoreHeight = 7.0f * uiScale;
            Vector2 highscorePosition = new Vector2(topHalfPosition.X + (highscoreSprite.SpriteSize.X * uiScale) - scoreWidth, topHalfPosition.Y + ((highscoreSprite.SpriteSize.Y * uiScale)) + 10);
            DrawScore(spriteBatch, 1337, highscorePosition, new Vector2(uiScale));

            Vector2 scorePosition = new Vector2(scoreWidth, highscorePosition.Y);
            DrawScore(spriteBatch, 1337, scorePosition, new Vector2(uiScale));


            Rectangle bottomRectangle = new Rectangle(0, window.ClientBounds.Height - bottomHeight, window.ClientBounds.Width, bottomHeight);
            DrawLives(spriteBatch, 5, new Vector2(bottomRectangle.X, bottomRectangle.Y) + new Vector2(14,(bottomRectangle.Height/2) - ((lifeSprite.SpriteSize.Y * uiScale) / 2)), new Vector2(uiScale));
        }

    }
}
