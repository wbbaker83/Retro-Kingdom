using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;

namespace Retro_Kingdom
{

    public class Main : Game
    {
        public const int DEFAULT_RESOLUTION_WIDTH = 1280;
        public const int DEFAULT_RESOLUTION_HEIGHT = 720;

        public const int GAME_STATUS_INTRO = 0;
        public const int GAME_STATUS_MENU = 1;
        public const int GAME_STATUS_GAME_RTS_RUNNING = 10;
        public const int GAME_STATUS_GAME_SS_RUNNING = 20;

        public int GameStatus;
        public int CurrentGameSelected;

        GraphicsDeviceManager graphics;
        GraphicsDevice device;
        SpriteBatch spriteBatch;

        KeyboardState oldKBState, currentKBState;
        MouseState oldMouseState, currentMouseState;
        GamePadState oldGPStateP1, currentGPStateP1;

        Intro mainIntro;
        RTSEngine gameRTS;
        SideScrollerEngine gameSS;
        Menu mainMenu;        

        public Main()
        {
            Window.Title = "ReTro Kingdom";
            this.IsMouseVisible = true;
            graphics = new GraphicsDeviceManager(this);
            device = graphics.GraphicsDevice;
            Content.RootDirectory = "Content";


            this.ChangeResolution(DEFAULT_RESOLUTION_WIDTH, DEFAULT_RESOLUTION_HEIGHT);
            this.LoadOptions();

            
        }



        protected override void Initialize()
        {
            GameSoundEffect.LoadContent(Content);
            Intro.LoadContent(Content);
            Sprite.LoadContent(Content);
            Menu.LoadContent(Content);
            BarMeter.LoadContent(Content);
            SideScrollerEngine.LoadContent(Content);

            mainIntro = new Intro(this);
            gameRTS = new RTSEngine("World 1", this);
            gameSS = new SideScrollerEngine("World 1", this);
            mainMenu = new Menu(this, 0);

            GameStatus = GAME_STATUS_INTRO;
            CurrentGameSelected = 0;

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
                case GAME_STATUS_MENU:
                    mainMenu.Update(oldKBState, currentKBState, oldMouseState, currentMouseState, oldGPStateP1, currentGPStateP1);
                    break;
                case GAME_STATUS_GAME_RTS_RUNNING:
                    gameRTS.Update(oldKBState, currentKBState, oldMouseState, currentMouseState, oldGPStateP1, currentGPStateP1);
                    break;
                case GAME_STATUS_GAME_SS_RUNNING:
                    gameSS.Update(oldKBState, currentKBState, oldMouseState, currentMouseState, oldGPStateP1, currentGPStateP1);
                    break;
            }

            oldKBState = currentKBState;
            oldMouseState = currentMouseState;
            oldGPStateP1 = currentGPStateP1;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            //GraphicsDevice.Clear(Color.CornflowerBlue);
            switch (GameStatus)
            {
                case GAME_STATUS_INTRO:
                    mainIntro.Draw(spriteBatch);
                    break;
                case GAME_STATUS_MENU:
                    mainMenu.Draw(spriteBatch);
                    break;
                case GAME_STATUS_GAME_RTS_RUNNING:
                    gameRTS.Draw(spriteBatch);
                    break;
                case GAME_STATUS_GAME_SS_RUNNING:
                    gameSS.Draw(spriteBatch);
                    break;
            }
            base.Draw(gameTime);
        }

        public void OpenMenu()
        {
            mainMenu = new Menu(this, this.CurrentGameSelected);
            this.GameStatus = GAME_STATUS_MENU;
        }

        public void RestartGame()
        {
            mainIntro = new Intro(this);
            mainMenu = new Menu(this, 0);

            gameRTS = new RTSEngine("World 1", this);
            gameSS = new SideScrollerEngine("World 1", this);

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

        private void CreateDefaultOptions()
        {
            string tmpFile = "options.ini";

            using (StreamWriter sw = File.CreateText(tmpFile))
            {
                sw.WriteLine(string.Format("fullscreen={0}", graphics.IsFullScreen.ToString().ToLower()));
                sw.WriteLine(string.Format("height={0}", graphics.PreferredBackBufferHeight));
                sw.WriteLine(string.Format("width={0}", graphics.PreferredBackBufferWidth));
            }
        }

        private void LoadOptions()
        {
            string tmpFile = "options.ini";
            List<string> tmpLines = new List<string>();
            if (File.Exists(tmpFile))
            {
                using (StreamReader reader = new StreamReader(tmpFile))
                {
                    string tmpLine;

                    while ((tmpLine = reader.ReadLine()) != null)
                    {
                        string left = tmpLine.Substring(0, tmpLine.IndexOf("="));
                        string right = tmpLine.Substring(tmpLine.LastIndexOf('=') + 1);

                        switch (left)
                        {
                            case "fullscreen":
                                graphics.IsFullScreen = Boolean.Parse(right);                                
                            break;
                            case "height":
                                graphics.PreferredBackBufferHeight = int.Parse(right);
                                break;
                            case "width":
                                graphics.PreferredBackBufferWidth = int.Parse(right);
                                break;
                        }
                        graphics.ApplyChanges();
                    }
                }
            }
            else
            {
                this.CreateDefaultOptions();
            }
        }

        public void SaveOptions()
        {
            string tmpFile = "options.ini";

            using (StreamWriter sw = File.CreateText(tmpFile))
            {
                sw.WriteLine(string.Format("fullscreen={0}", graphics.IsFullScreen.ToString().ToLower()));
                sw.WriteLine(string.Format("height={0}", graphics.PreferredBackBufferHeight));
                sw.WriteLine(string.Format("width={0}", graphics.PreferredBackBufferWidth));
            }
        }
    }
}
