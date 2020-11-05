using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PacMan
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private Texture2D spritesheetTexture;
        private Texture2D tilesetTexture;

        private Editor editor;
        private InGameState game;

        private GameState gameState;

        public static Vector2 Scale = new Vector2(1.0f, 1.0f);

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            

            IsMouseVisible = true;
        }
        protected override void LoadContent()
        {

            //TODO:
            // Fråga lärare om Manager/Handler
            // 

            graphics.PreferredBackBufferWidth = 768;
            graphics.PreferredBackBufferHeight = 768;
            graphics.ApplyChanges();

            spriteBatch = new SpriteBatch(GraphicsDevice);            

            spritesheetTexture = Content.Load<Texture2D>("SpriteSheet");
            tilesetTexture = Content.Load<Texture2D>("Tileset");

            //level = new Level(new SpriteSheet(tilesetTexture, Vector2.Zero, new Vector2(128, 128), new Vector2(32,32), 1));
            //level.LoadLevel("Content\\Level1.txt");

            Tile.NULL_SPRITE = new SpriteSheet(CreateFilledTexture(32, 32, new Color(0, 0, 64, 128))).Sprite;

            SpriteSheet spritesheet = new SpriteSheet(spritesheetTexture, Vector2.Zero, new Vector2(135,112), new Vector2(16, 16));

            SpriteSheet tilesheet = new SpriteSheet(tilesetTexture, Vector2.Zero, new Vector2(128, 128), new Vector2(32, 32), 1);

            Level level = Level.LoadLevel(tilesheet, spritesheet, "Content\\Level1.txt");

            game = new InGameState(tilesheet, spritesheet);
            game.SetLevel(level);
                                   
            editor = new Editor(tilesheet,spritesheet, Window);
            editor.SetLevel(level);
            editor.GridTexture = CreateRectangleTexture(32, 32, new Color(128, 128, 128, 128));
            editor.HighlightTexture = CreateFilledTexture(32, 32, new Color(128,128,128,128));
            editor.FoodSprite = spritesheet.GetAt(4, 0);
                       

            gameState = editor;
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

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if(Keyboard.GetState().IsKeyDown(Keys.F1))
            {
                editor.SaveLevel("Content\\Level1.txt");
            }


            if (Keyboard.GetState().IsKeyDown(Keys.F12))
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

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp);
            gameState.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
