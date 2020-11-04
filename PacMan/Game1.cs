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

        private Level level;
        private Editor editor;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }


        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            spritesheetTexture = Content.Load<Texture2D>("SpriteSheet");
            tilesetTexture = Content.Load<Texture2D>("Tileset");

            //level = new Level(new SpriteSheet(tilesetTexture, Vector2.Zero, new Vector2(128, 128), new Vector2(32,32), 1));
            //level.LoadLevel("Content\\Level1.txt");

            Texture2D pixelTexture = new Texture2D(GraphicsDevice, 3,3);
            Color pixelColor = Color.White;
            Color[] pixelColors = new Color[9] { Color.Black, Color.Black, Color.Black, Color.Black, Color.White, Color.Black, Color.Black, Color.Black, Color.Black };
            //pixelTexture.SetData(new Color[]{pixelColor});
            pixelTexture.SetData(pixelColors);


            editor = new Editor(new SpriteSheet(tilesetTexture, Vector2.Zero, new Vector2(128, 128), new Vector2(32, 32), 1));
            editor.DebugTexture = CreateRectangleTexture(32,32);
        }

        private Texture2D CreateRectangleTexture(int width, int height)
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
                        colors[index] = Color.Black;
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

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            editor.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
            //level.Draw(spriteBatch);
            editor.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
