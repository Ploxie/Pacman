﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Emit;

namespace PacMan
{
    public class Game1 : Game
    {
        public static Random random = new Random();

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private Texture2D spritesheetTexture;
        private Texture2D tilesetTexture;
        private Texture2D pacmanBackgroundTexture;
        private Texture2D pacmanLoseScreen;
        private Texture2D pacmanWinScreen;

        private SpriteFont menuFont;

        private SpriteSheet tilesheet;
        private SpriteSheet spritesheet;

        private Editor editor;
        private InGameState game;
        private MenuGameState menu;

        private GameState gameState;
        private KeyboardState lastKeyboardState;

        private HUD hud;
        private int levelIndex;
        private List<string> levelPaths;
        private string currentLevelPath;
        private List<int> highscores;
        private bool savedHighscores;

        public static readonly Vector2 Scale = new Vector2(1.0f, 1.0f);

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";            

            IsMouseVisible = true;
        }
        protected override void LoadContent()
        {
            int hudTop = 70;
            int hudBot = 70;

            graphics.PreferredBackBufferWidth = 768;
            graphics.PreferredBackBufferHeight = 768 + hudTop + hudBot;
            graphics.ApplyChanges();

            spriteBatch = new SpriteBatch(GraphicsDevice);            

            spritesheetTexture = Content.Load<Texture2D>("SpriteSheet");
            tilesetTexture = Content.Load<Texture2D>("Tileset");
            pacmanBackgroundTexture = Content.Load<Texture2D>("pacmanBackground");
            pacmanLoseScreen = Content.Load<Texture2D>("LoseScreen");
            pacmanWinScreen = Content.Load<Texture2D>("WinScreen");
            Texture2D scoreTexture = Content.Load<Texture2D>("score-numbers");

            menuFont = Content.Load<SpriteFont>("menuFont");
            
            Tile.NULL_SPRITE = new SpriteSheet(CreateFilledTexture(32, 32, new Color(0, 0, 64, 128))).Sprite;

            this.spritesheet = new SpriteSheet(spritesheetTexture, Vector2.Zero, new Vector2(135,112), new Vector2(16, 16));
            this.tilesheet = new SpriteSheet(tilesetTexture, Vector2.Zero, new Vector2(128, 128), new Vector2(32, 32), 1);

            this.levelPaths = new List<string>() 
            {
                "Content\\Level1.txt",
                "Content\\Level2.txt",
                "Content\\Level3.txt",
                "Content\\Level4.txt",
                "Content\\Level5.txt"
            };

            this.currentLevelPath = this.levelPaths[0];

            Level level = Level.LoadLevel(tilesheet, spritesheet, currentLevelPath);

            highscores = new List<int>();
            int highscore = LoadHighscore();
            Sprite lifeSprite = spritesheet.GetAt(1, 0);
            hud = new HUD(Window, hudTop, hudBot, scoreTexture, lifeSprite, highscore);
            
            game = new InGameState(Window, hud, spritesheet);
            game.SetLevel(level);
                                   
            editor = new Editor(Window);
            editor.SetLevel(level);
            editor.GridTexture = CreateRectangleTexture(32, 32, new Color(128, 128, 128, 128));
            editor.HighlightTexture = CreateFilledTexture(32, 32, new Color(128,128,128,128));
            editor.FoodSprite = spritesheet.GetAt(3, 0);
            editor.BigFoodSprite = spritesheet.GetAt(4, 0);
            editor.PowerupGhostSprite = spritesheet.GetAt(0, 6);
            editor.PowerupWallEaterSprite = spritesheet.GetAt(1, 6);
            editor.PacmanSpawnSprite = spritesheet.GetAt(1, 0);
            editor.GhostSpawnSprite = spritesheet.GetAt(0, 1);

            menu = new MenuGameState(hud, pacmanBackgroundTexture, pacmanWinScreen, pacmanLoseScreen, menuFont, Window);
            menu.Highscores = highscores;

            gameState = menu;            
        }

        private int LoadHighscore()
        {
            StreamReader sr = new StreamReader("Content\\Highscores.txt");

            highscores = new List<int>();

            string currentLine;
            while ((currentLine = sr.ReadLine()) != null)
            {
                int score = int.Parse(currentLine);
                highscores.Add(score);
            }

            sr.Close();

            if (highscores.Count == 0)
            {
                return 0;
            }

            highscores.Sort((o0, o1) => o1 - o0);
            return highscores[0];
        }

        private void SaveHighscores()
        {

            StreamWriter writer = new StreamWriter(@"Content\\Highscores.txt", false);

            foreach (int score in highscores)
            {
                writer.WriteLine(score);
            }

            writer.Close();
        }

        private Texture2D CreateRectangleTexture(int width, int height, Color lineColor)
        {
            Texture2D texture = new Texture2D(GraphicsDevice, width, height);

            Color[] colors = new Color[width * height];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int index = y * width + x;
                    if(x == 0 || x == width-1 || y == 0 || y == height-1)
                    {
                        colors[index] = lineColor;
                    }
                    else
                    {
                        colors[index] = Color.Transparent;
                    }
                }
            }

            texture.SetData(colors);
            

            return texture;
        }

        private Texture2D CreateFilledTexture(int width, int height, Color color)
        {
            Texture2D texture = new Texture2D(GraphicsDevice, width, height);

            Color[] colors = new Color[width * height];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int index = y * width + x;
                    colors[index] = color;
                }
            }

            texture.SetData(colors);


            return texture;
        }

        private void SetEditorLevel(string levelPath)
        {
            this.currentLevelPath = levelPath;
            Level level = Level.LoadLevel(tilesheet, spritesheet, levelPath);
            editor.SetLevel(level);
        }

        private void HandleEditorInput(KeyboardState keyboardState)
        {
            if(gameState != editor)
            {
                return;
            }

            if (keyboardState.IsKeyDown(Keys.LeftControl) && (keyboardState.IsKeyDown(Keys.S) && !lastKeyboardState.IsKeyDown(Keys.S)))
            {
                editor.SaveLevel(currentLevelPath);
                game.SetLevel(Level.LoadLevel(tilesheet, spritesheet, currentLevelPath));
            }

            if (keyboardState.IsKeyDown(Keys.F1) && !lastKeyboardState.IsKeyDown(Keys.F1))
            {
                SetEditorLevel(levelPaths[0]);
            }
            else if (keyboardState.IsKeyDown(Keys.F2) && !lastKeyboardState.IsKeyDown(Keys.F2))
            {
                SetEditorLevel(levelPaths[1]);
            }
            else if (keyboardState.IsKeyDown(Keys.F3) && !lastKeyboardState.IsKeyDown(Keys.F3))
            {
                SetEditorLevel(levelPaths[2]);
            }
            else if (keyboardState.IsKeyDown(Keys.F4) && !lastKeyboardState.IsKeyDown(Keys.F4))
            {
                SetEditorLevel(levelPaths[3]);
            }
            else if (keyboardState.IsKeyDown(Keys.F5) && !lastKeyboardState.IsKeyDown(Keys.F5))
            {
                SetEditorLevel(levelPaths[4]);
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if(lastKeyboardState == null)
            {
                lastKeyboardState = Keyboard.GetState();
            }

            if (menu.Play)
            {
                gameState = game;
                menu.Play = false;
                savedHighscores = false;
            }
            else if (menu.Editor)
            {
                gameState = editor;
                menu.Editor = false;
            }
            

            KeyboardState keyboardState = Keyboard.GetState();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || keyboardState.IsKeyDown(Keys.Escape) || menu.Quit)
                Exit();

            HandleEditorInput(keyboardState);


            if (keyboardState.IsKeyDown(Keys.F12) && !lastKeyboardState.IsKeyDown(Keys.F12))
            {
                if(gameState == game)
                {
                    gameState = editor;
                }
                else
                {
                    gameState = game;
                }
            }

            
            gameState.Update(gameTime);



            if (game.FoodCollected && !savedHighscores)
            {
                levelIndex++;
                if(levelIndex >= levelPaths.Count)
                {
                    // No more levels, you won the game!
                    menu.WinScreen = true;
                    gameState = menu;
                    menu.CurrentOption = MenuGameState.Option.RestartGame;

                    highscores.Add(hud.Pacman.Score);
                    highscores.Sort((o0, o1) => o1 - o0);
                    menu.Highscores = highscores;
                    SaveHighscores();
                    savedHighscores = true;
                }
                else
                {
                    // Go to next level
                    game.SetLevel(Level.LoadLevel(tilesheet, spritesheet, levelPaths[levelIndex]));
                }                
            }
            if (hud.Pacman.Lives <= 0 && !savedHighscores)
            {
                highscores.Add(hud.Pacman.Score);
                highscores.Sort((o0, o1) => o1 - o0);
                menu.Highscores = highscores;
                SaveHighscores();
                gameState = menu;
                menu.CurrentOption = MenuGameState.Option.RestartGame;
                savedHighscores = true;
            }  
            if (menu.Restart)
            {
                gameState = game;
                hud.Pacman.Score = 0;
                levelIndex = 0;
                game.SetLevel(Level.LoadLevel(tilesheet, spritesheet, levelPaths[levelIndex]));
                savedHighscores = false;
                menu.Restart = false;
                
            }
            lastKeyboardState = Keyboard.GetState();
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp);
            gameState.Draw(spriteBatch);
            spriteBatch.End();
        }
    }
}
