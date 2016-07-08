using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Retro_Kingdom
{
    class Intro
    {
        private static Texture2D[] _loadedtextures;

        public Texture2D[] LoadedTextures
        {
            get
            {
                return _loadedtextures;
            }
            set
            {
                _loadedtextures = value;
            }
        }

        public Main MainGameState
        {
            get;
            set;
        }

        public int CountDown
        {
            get;
            set;
        }

        public Intro(Main main)
        {
            this.MainGameState = main;
            this.CountDown = 0;
        }

        public static void LoadContent(ContentManager conman)
        {
            int texturecount = 1;
            _loadedtextures = new Texture2D[texturecount];
            _loadedtextures[1 - 1] = conman.Load<Texture2D>("Resources/textures/startscreen");
        }

        public void Update(KeyboardState okbs, KeyboardState ckbs, MouseState oms, MouseState cms, GamePadState ogps, GamePadState cgps)
        {
            if (this.CountDown > 300 ||
                cms.LeftButton == ButtonState.Pressed && oms.LeftButton != ButtonState.Pressed ||
                ckbs.IsKeyDown(Keys.Enter) == true && okbs.IsKeyDown(Keys.Enter) != true ||
                ckbs.IsKeyDown(Keys.Escape) == true && okbs.IsKeyDown(Keys.Escape) != true ||
                cgps.Buttons.Start == ButtonState.Pressed && ogps.Buttons.Start != ButtonState.Pressed
                )
            {
                this.MainGameState.OpenMenu();
                this.CountDown = 0;
            }
            this.CountDown++;
        }

        public void Draw(SpriteBatch spriteBatch, GraphicsDevice device)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(this.LoadedTextures[0], new Rectangle(0, 0, 1280, 720), null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);
            spriteBatch.End();
        }
    }
}