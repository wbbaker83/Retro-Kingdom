using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Retro_Kingdom
{

    public class Main : Game
    {
        public const int DEFAULT_RESOLUTION_WIDTH = 1280;
        public const int DEFAULT_RESOLUTION_HEIGHT = 720;

        public const int GAME_STATUS_INTRO = 0;
        public const int GAME_STATUS_STARTMENU = 1;
        public const int GAME_STATUS_GAME_RTS_RUNNING = 10;
        public const int GAME_STATUS_GAME_RTS_MENU = 11;
        public const int GAME_STATUS_GAME_SS_RUNNING = 20;
        public const int GAME_STATUS_GAME_SS_MENU = 21;

        GraphicsDeviceManager graphics;
        GraphicsDevice device;
        SpriteBatch spriteBatch;

        KeyboardState oldKBState, currentKBState;
        MouseState oldMouseState, currentMouseState;
        GamePadState oldGPStateP1, currentGPStateP1;

        Intro mainIntro;
        RTSEngine gameRTS;
        SideScrollerEngine gameSS;
        Menu startMenu, rtsMenu, ssMenu;

        public int GameStatus;

        public Main()
        {
            Window.Title = "ReTro Kingdom";
            this.IsMouseVisible = true;
            graphics = new GraphicsDeviceManager(this);
            device = graphics.GraphicsDevice;
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = DEFAULT_RESOLUTION_WIDTH;
            graphics.PreferredBackBufferHeight = DEFAULT_RESOLUTION_HEIGHT;
            graphics.ApplyChanges();
        }



        protected override void Initialize()
        {
            GameSoundEffect.LoadContent(Content);
            Intro.LoadContent(Content);
            Sprite.LoadContent(Content);
            Menu.LoadContent(Content);

            mainIntro = new Intro(this);
            gameRTS = new RTSEngine("World 1", this);
            gameSS = new SideScrollerEngine("World 1", this);
            startMenu = new Menu(this, 0);
            rtsMenu = new Menu(this, 1);
            ssMenu = new Menu(this, 2);

            GameStatus = GAME_STATUS_INTRO;

            base.Initialize();
        }


        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }


        protected override void UnloadContent()
        {
            this.Content.Unload();
        }


        protected override void Update(GameTime gameTime)
        {
            currentKBState = Keyboard.GetState();
            currentMouseState = Mouse.GetState();
            currentGPStateP1 = GamePad.GetState(PlayerIndex.One);

            switch (this.GameStatus)
            {
                case GAME_STATUS_INTRO:
                    mainIntro.Update(oldKBState, currentKBState, oldMouseState, currentMouseState, oldGPStateP1, currentGPStateP1);
                    break;
                case GAME_STATUS_STARTMENU:
                    startMenu.Update(oldKBState, currentKBState, oldMouseState, currentMouseState, oldGPStateP1, currentGPStateP1);
                    break;
                case GAME_STATUS_GAME_RTS_RUNNING:
                    gameRTS.Update(oldKBState, currentKBState, oldMouseState, currentMouseState, oldGPStateP1, currentGPStateP1);
                    break;
                case GAME_STATUS_GAME_RTS_MENU:
                    rtsMenu.Update(oldKBState, currentKBState, oldMouseState, currentMouseState, oldGPStateP1, currentGPStateP1);
                    break;
                case GAME_STATUS_GAME_SS_RUNNING:
                    gameSS.Update(oldKBState, currentKBState, oldMouseState, currentMouseState, oldGPStateP1, currentGPStateP1);
                    break;
                case GAME_STATUS_GAME_SS_MENU:
                    ssMenu.Update(oldKBState, currentKBState, oldMouseState, currentMouseState, oldGPStateP1, currentGPStateP1);
                    break;
            }

            oldKBState = currentKBState;
            oldMouseState = currentMouseState;
            oldGPStateP1 = currentGPStateP1;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            switch (GameStatus)
            {
                case GAME_STATUS_INTRO:
                    mainIntro.Draw(spriteBatch, device);
                    break;
                case GAME_STATUS_STARTMENU:
                    startMenu.Draw(spriteBatch, device);
                    break;
                case GAME_STATUS_GAME_RTS_RUNNING:
                    gameRTS.Draw(spriteBatch);
                    break;
                case GAME_STATUS_GAME_RTS_MENU:
                    rtsMenu.Draw(spriteBatch, device);
                    break;
                case GAME_STATUS_GAME_SS_RUNNING:
                    gameSS.Draw(spriteBatch);
                    break;
                case GAME_STATUS_GAME_SS_MENU:
                    ssMenu.Draw(spriteBatch, device);
                    break;
            }


            base.Draw(gameTime);
        }

        public void RestartGame()
        {
            mainIntro = new Intro(this);
            startMenu = new Menu(this, 0);

            gameRTS = new RTSEngine("World 1", this);
            rtsMenu = new Menu(this, 1);

            gameSS = new SideScrollerEngine("World 1", this);
            ssMenu = new Menu(this, 2);

            GameStatus = GAME_STATUS_INTRO;
        }

        public void ToggleFullScreen()
        {
            graphics.ToggleFullScreen();
        }

        public void ChangeResolution(int width, int height)
        {
            graphics.PreferredBackBufferWidth = width;
            graphics.PreferredBackBufferHeight = height;
            graphics.ApplyChanges();
        }
    }
}
