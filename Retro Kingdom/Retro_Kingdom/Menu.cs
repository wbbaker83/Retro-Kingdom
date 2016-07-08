using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Retro_Kingdom
{
    class Menu
    {
        private const int BUTTON_WIDTH = 250;
        private const int BUTTON_HEIGHT = 20;

        private static Texture2D[] _loadedtextures;
        private static SpriteFont[] _loadedspritefonts;

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

        public SpriteFont[] LoadedSpriteFonts
        {
            get
            {
                return _loadedspritefonts;
            }
            set
            {
                _loadedspritefonts = value;
            }
        }

        public int MenuID
        {
            get;
            set;
        }

        public int NeedsToUpdate
        {
            get;
            set;
        }

        public Main MainGameState
        {
            get;
            set;
        }

        public List<string> ButtonNames
        {
            get;
            set;
        }

        public List<Rectangle> ButtonBoxes
        {
            get;
            set;
        }

        public int SelectedButtonIndex
        {
            get;
            set;
        }

        public string SelectedButtonName
        {
            get;
            set;
        }

        public Vector2 NextOpenButtonPosition
        {
            get;
            set;
        }

        public GameSoundEffect MenuMoveSound
        {
            get;
            set;
        }

        public Menu(Main main, int id)
        {
            this.ButtonNames = new List<string>();
            this.ButtonBoxes = new List<Rectangle>();
            this.MainGameState = main;
            this.NextOpenButtonPosition = new Vector2((MainGameState.GraphicsDevice.Viewport.Width / 2) - (BUTTON_WIDTH / 2), (MainGameState.GraphicsDevice.Viewport.Height / 3));
            this.MenuID = id;
            this.NeedsToUpdate = -1;

            this.SetMenu();
        }

        public void Update(KeyboardState okbs, KeyboardState ckbs, MouseState oms, MouseState cms, GamePadState ogps, GamePadState cgps)
        {
            if (NeedsToUpdate > -1)
            {
                this.MenuID = this.NeedsToUpdate;
                this.ConfirmSelection();
                this.NeedsToUpdate = -1;
            }
            if (ckbs.IsKeyDown(Keys.S) == true && okbs.IsKeyDown(Keys.S) != true ||
                cgps.DPad.Down == ButtonState.Pressed && ogps.DPad.Down != ButtonState.Pressed)
            {
                if (this.SelectedButtonIndex + 1 >= this.ButtonNames.Count)
                {
                    this.SelectedButtonIndex = 0;
                    this.SelectedButtonName = this.ButtonNames[this.SelectedButtonIndex];
                    this.MenuMoveSound.PlaySoundEffect();
                }
                else
                {
                    this.SelectedButtonIndex++;
                    this.SelectedButtonName = this.ButtonNames[this.SelectedButtonIndex];
                    this.MenuMoveSound.PlaySoundEffect();
                }
            }
            else if (ckbs.IsKeyDown(Keys.W) == true && okbs.IsKeyDown(Keys.W) != true ||
                cgps.DPad.Up == ButtonState.Pressed && ogps.DPad.Up != ButtonState.Pressed)
            {
                if (this.SelectedButtonIndex - 1 < 0)
                {
                    this.SelectedButtonIndex = this.ButtonNames.Count - 1;
                    this.SelectedButtonName = this.ButtonNames[this.SelectedButtonIndex];
                    this.MenuMoveSound.PlaySoundEffect();
                }
                else
                {
                    this.SelectedButtonIndex--;
                    this.SelectedButtonName = this.ButtonNames[this.SelectedButtonIndex];
                    this.MenuMoveSound.PlaySoundEffect();
                }
            }

            if (ckbs.IsKeyDown(Keys.Escape) == true && okbs.IsKeyDown(Keys.Escape) != true ||
                cgps.Buttons.Back == ButtonState.Pressed && ogps.Buttons.Back != ButtonState.Pressed)
            {
                if (this.MenuID == 0)
                {
                    MainGameState.Exit();
                }
                else if (this.MenuID == 1)
                {
                    this.SelectedButtonIndex = 0;
                    //MainGameState.GameStatus = Main.GAME_STATUS_GAME_RTS_RUNNING;
                    this.MainGameState.OpenMenu();//CurrentGameSelected = 1;
                }

            }

            if (ckbs.IsKeyDown(Keys.Enter) == true && okbs.IsKeyDown(Keys.Enter) != true ||
                cgps.Buttons.Start == ButtonState.Pressed && ogps.Buttons.Start != ButtonState.Pressed ||
                    cgps.Buttons.A == ButtonState.Pressed && ogps.Buttons.A != ButtonState.Pressed)
            {
                this.ConfirmSelection();
                this.SelectedButtonIndex = 0;
            }

            int cnt = 0;

            foreach (Rectangle r in this.ButtonBoxes)
            {
                if (r.Intersects(new Rectangle(cms.X, cms.Y, 1, 1)) == true)// && okbs == ckbs)
                {
                    if (this.SelectedButtonIndex != cnt)
                    {
                        this.SelectedButtonIndex = cnt;
                        this.SelectedButtonName = this.ButtonNames[cnt];
                        this.MenuMoveSound.PlaySoundEffect();
                    }
                    if (cms.LeftButton == ButtonState.Pressed && oms.LeftButton != ButtonState.Pressed)
                    {
                        this.NeedsToUpdate = 3;
                        this.SelectedButtonIndex = 0;
                    }
                }
                cnt++;
            }

        }

        public void Draw(SpriteBatch spriteBatch, GraphicsDevice device)
        {
            spriteBatch.Begin();

            //Background

            if (this.MenuID == 0)
            {
                spriteBatch.Draw(this.LoadedTextures[2], new Rectangle(0, 0, MainGameState.GraphicsDevice.Viewport.Width, MainGameState.GraphicsDevice.Viewport.Height), null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);

            }
            else 
            {
                spriteBatch.Draw(this.LoadedTextures[1], new Rectangle(0, 0, MainGameState.GraphicsDevice.Viewport.Width, MainGameState.GraphicsDevice.Viewport.Height), null, Color.Black, 0, new Vector2(0, 0), SpriteEffects.None, 0);
            }



            Color tmpColor;
            for (int x = 0; x < ButtonNames.Count; x++)
            {
                if (ButtonNames[x] == SelectedButtonName)
                {
                    tmpColor = new Color(new Vector4(0.9f, 1.0f, 0.1f, 1.0f));
                }
                else
                {
                    tmpColor = new Color(new Vector4(0.0f, 0.0f, 0.2f, 0.5f));
                }
                spriteBatch.Draw(this.LoadedTextures[1], this.ButtonBoxes[x], tmpColor);
                spriteBatch.DrawString(this.LoadedSpriteFonts[0], ButtonNames[x], new Vector2(NextOpenButtonPosition.X + 5, this.ButtonBoxes[x].Y), Color.DarkRed);
            }
            spriteBatch.End();
        }

        private void AddButton(string name)
        {
            this.ButtonNames.Add(name);
            this.ButtonBoxes.Add(new Rectangle((int)NextOpenButtonPosition.X, (int)NextOpenButtonPosition.Y, BUTTON_WIDTH, BUTTON_HEIGHT));
            this.NextOpenButtonPosition += new Vector2(0, 30);
        }

        public static void LoadContent(ContentManager conman)
        {
            int contentcount;

            contentcount = 3;
            _loadedtextures = new Texture2D[contentcount];
            _loadedtextures[1 - 1] = conman.Load<Texture2D>("Resources/textures/black_sq");
            _loadedtextures[2 - 1] = conman.Load<Texture2D>("Resources/textures/white_sq");
            _loadedtextures[3 - 1] = conman.Load<Texture2D>("Resources/textures/titlemenu");

            contentcount = 1;
            _loadedspritefonts = new SpriteFont[contentcount];
            _loadedspritefonts[1 - 1] = conman.Load<SpriteFont>("Resources/spritefonts/ArialBlack_10pt");

        }

        private void ConfirmSelection()
        {

            switch (this.SelectedButtonName)
            {
                case "Return To Start Menu":
                    this.MenuID = 0;
                    this.SetMenu();
                    this.MainGameState.CurrentGameSelected = 0;
                    this.MainGameState.GameStatus = Main.GAME_STATUS_MENU;
                    break;
                case "Video":
                    this.MenuID = 3;
                    this.SetMenu();
                    break;
                case "Start RTS":
                    this.MainGameState.GameStatus = Main.GAME_STATUS_GAME_RTS_RUNNING;
                    this.MainGameState.CurrentGameSelected = 1;
                    break;
                case "Resume RTS":
                    this.MainGameState.GameStatus = Main.GAME_STATUS_GAME_RTS_RUNNING;
                    this.MainGameState.CurrentGameSelected = 1;
                    break;
                case "Restart RTS":
                    this.MainGameState.RestartGame();
                    this.MainGameState.GameStatus = Main.GAME_STATUS_GAME_RTS_RUNNING;
                    this.MainGameState.CurrentGameSelected = 1;
                    break;
                case "Start SideScroller":
                    this.MainGameState.GameStatus = Main.GAME_STATUS_GAME_SS_RUNNING;
                    this.MainGameState.CurrentGameSelected = 2;
                    break;
                case "Resume SideScroller":
                    this.MainGameState.GameStatus = Main.GAME_STATUS_GAME_SS_RUNNING;
                    this.MainGameState.CurrentGameSelected = 2;
                    break;
                case "Restart SideScroller":
                    this.MainGameState.RestartGame();
                    this.MainGameState.GameStatus = Main.GAME_STATUS_GAME_SS_RUNNING;
                    this.MainGameState.CurrentGameSelected = 2;
                    break;
                case "FullScreen On/Off":
                    this.MainGameState.ToggleFullScreen();
                    break;
                case "1920 x 1080":
                    this.MainGameState.ChangeResolution(1920, 1080);
                    break;
                case "1280 x 720":
                    this.MainGameState.ChangeResolution(1280, 720);
                    break;
                case "Exit To Desktop":
                    this.MainGameState.Exit();
                    break;


            }
        }

        private void SetMenu()
        {
            this.ButtonBoxes.Clear();
            this.ButtonNames.Clear();
            this.MenuMoveSound = new GameSoundEffect(0);
            this.SelectedButtonIndex = 0;
            this.NextOpenButtonPosition = new Vector2((MainGameState.GraphicsDevice.Viewport.Width / 2) - (BUTTON_WIDTH / 2), (MainGameState.GraphicsDevice.Viewport.Height / 3));

            switch (this.MenuID)
            {
                case 0:// Start Menu
                    AddButton("Start RTS");
                    this.SelectedButtonName = "Start RTS";
                    AddButton("Start SideScroller");
                    AddButton("Video");
                    AddButton("Exit To Desktop");
                    break;
                case 1:// RTS Menu
                    AddButton("Resume RTS");
                    this.SelectedButtonName = "Resume RTS";
                    AddButton("Restart RTS");
                    AddButton("Return To Start Menu");
                    AddButton("Exit To Desktop");
                    break;
                case 2:// SS Menu
                    AddButton("Resume SideScroller");
                    this.SelectedButtonName = "Resume SideScroller";
                    AddButton("Restart SideScroller");
                    AddButton("Return To Start Menu");
                    AddButton("Exit To Desktop");
                    break;
                case 3:// Video
                    AddButton("Return To Start Menu");
                    this.SelectedButtonName = "Return to Start Menu";
                    AddButton("FullScreen On/Off");
                    AddButton("1920 x 1080");
                    AddButton("1280 x 720");
                    break;
            }
        }
    }
}