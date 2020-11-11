using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace PacMan
{
    public class GainedScore
    {

        public GainedScore(Tile tile, int score, double timer)
        {
            this.Tile = tile;
            this.Score = score;
            this.Timer = timer;
        }

        public Tile Tile
        {
            get;
            private set;
        }

        public int Score
        {
            get;
            private set;
        }

        public double Timer
        {
            get;
            set;
        }
    }

    public class HUD
    {

        private GameWindow window;
        private int topHeight;
        private int bottomHeight;

        private SpriteSheet scoresheet;
        private Sprite lifeSprite;

        private List<GainedScore> gainedScores = new List<GainedScore>();

        public HUD(GameWindow window, int topHeight, int bottomHeight, Texture2D numberTexture, Sprite lifeSprite)
        {
            this.window = window;
            this.topHeight = topHeight;
            this.bottomHeight = bottomHeight;

            SpriteSheet highscoreSpriteSheet = new SpriteSheet(numberTexture, new Vector2(51, 1), new Vector2(78, 7), new Vector2(78, 7));
            HighscoreSprite = highscoreSpriteSheet.Sprite;

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

        public Sprite HighscoreSprite
        {
            get;
        }

        public void AddGainedScore(GainedScore gainedScore)
        {
            this.gainedScores.Add(gainedScore);
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

        public void DrawScore(SpriteBatch spriteBatch, int score, Vector2 position, Vector2 scale, Color? color = null, int numbers = 6)
        {
            string valueString = score.ToString();
            numbers = Math.Max(valueString.Length, numbers);
            int[] valueNumbers = new int[numbers];

            int numberIndex = valueString.Length - 1;
            for (int i = valueNumbers.Length - 1; i >= valueNumbers.Length - valueString.Length; i--)
            {
                char c = valueString[numberIndex--];
                valueNumbers[i] = (int)Char.GetNumericValue(c);
            }

            for (int i = 0; i < numbers; i++)
            {
                Sprite sprite = GetNumberSprite(valueNumbers[i]);
                int size = (int)(sprite.SpriteSize.X * scale.X) + 1;
                int x = i * size;
                sprite.Draw(spriteBatch, new Vector2(position.X + x, position.Y), scale, SpriteEffects.None, color);
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

        public void DrawGainedScore(SpriteBatch spriteBatch, int score, Vector2 scale, Vector2 position, int numbers = 6)
        {
            DrawScore(spriteBatch, score, position, scale, null, numbers);
        }
        
        public void Update(GameTime gameTime)
        {
            for(int i = gainedScores.Count - 1; i >= 0;i--)
            {
                gainedScores[i].Timer -= gameTime.ElapsedGameTime.TotalMilliseconds;
                if(gainedScores[i].Timer <= 0)
                {
                    gainedScores.RemoveAt(i);
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {

            float uiScale = Game1.Scale.X * 2.0f;
            Rectangle topRectangle = new Rectangle(0, 0, window.ClientBounds.Width, topHeight);

            Vector2 topHalfPosition = new Vector2((topRectangle.Width / 2) - ((HighscoreSprite.SpriteSize.X * uiScale) / 2), topRectangle.X + 15);
            HighscoreSprite.Draw(spriteBatch, topHalfPosition, Game1.Scale * uiScale);

            float scoreWidth = 6.0f * 7.0f * uiScale + 6.0f;
            float scoreHeight = 7.0f * uiScale;
            Vector2 highscorePosition = new Vector2(topHalfPosition.X + (HighscoreSprite.SpriteSize.X * uiScale) - scoreWidth, topHalfPosition.Y + ((HighscoreSprite.SpriteSize.Y * uiScale)) + 10);
            DrawScore(spriteBatch, 1337, highscorePosition, new Vector2(uiScale));

            Vector2 scorePosition = new Vector2(scoreWidth, highscorePosition.Y);
            DrawScore(spriteBatch, Pacman.Score, scorePosition, new Vector2(uiScale));


            Rectangle bottomRectangle = new Rectangle(0, window.ClientBounds.Height - bottomHeight, window.ClientBounds.Width, bottomHeight);
            DrawLives(spriteBatch, Pacman.Lives, new Vector2(bottomRectangle.X, bottomRectangle.Y) + new Vector2(14,(bottomRectangle.Height/2) - ((lifeSprite.SpriteSize.Y * uiScale) / 2)), new Vector2(uiScale));
        
            foreach(GainedScore gainedScore in gainedScores)
            {
                string scoreString = gainedScore.Score.ToString();
                float gainedScoreWidth = scoreString.Length * 7.0f * Game1.Scale.X + scoreString.Length;
                float gainedScoreHeight = 7.0f * Game1.Scale.X;
                Vector2 position = new Vector2(0, topRectangle.Height) + gainedScore.Tile.Position + (new Vector2(gainedScore.Tile.Size) * 0.5f)- (new Vector2(gainedScoreWidth, gainedScoreHeight) * 0.5f);
                DrawGainedScore(spriteBatch, gainedScore.Score, Game1.Scale, position, 0);
            }
        }

    }
}
