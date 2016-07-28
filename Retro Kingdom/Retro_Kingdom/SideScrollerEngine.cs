using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Retro_Kingdom
{
    class SideScrollerEngine
    {
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

        public string Name
        {
            get;
            set;
        }

        public Dictionary<string, List<Sprite>> SpriteLayers
        {
            set;
            get;
        }

        public Camera2D Camera
        {
            get;
            set;
        }

        public bool IsEditingEnabled
        {
            get;
            set;
        }

        public Sprite PlayerOne
        {
            get;
            set;
        }

        public Main MainGameState
        {
            get;
        }

        

        private static Texture2D[] _loadedtextures;

        public SideScrollerEngine(string name, Main main)
        {
            SpriteLayers = new Dictionary<string, List<Sprite>>();
            this.IsEditingEnabled = false;
            this.Camera = new Camera2D(1280, 720);
            this.PlayerOne = new Sprite(0, 100, 290);
            this.Name = name;
            this.MainGameState = main;
            this.LoadMap();
        }

        public static void LoadContent(ContentManager conman)
        {
            int contentcount;

            contentcount = 1;
            _loadedtextures = new Texture2D[contentcount];
            _loadedtextures[1 - 1] = conman.Load<Texture2D>("Resources/textures/ss_editor_overlay");
        }

        public void Update(KeyboardState okbs, KeyboardState ckbs, MouseState oms, MouseState cms, GamePadState ogps, GamePadState cgps)
        {
            if (ckbs.IsKeyDown(Keys.D) == true ||
                cgps.DPad.Right == ButtonState.Pressed)
            {
                this.PlayerOne.Box = new Rectangle(this.PlayerOne.Box.X + 1, this.PlayerOne.Box.Y, this.PlayerOne.Box.Width, this.PlayerOne.Box.Height);
                if (this.IsPlayerCollidingWithSolidSprite(this.PlayerOne))
                {
                    this.PlayerOne.Box = new Rectangle(this.PlayerOne.Box.X - 1, this.PlayerOne.Box.Y, this.PlayerOne.Box.Width, this.PlayerOne.Box.Height);
                }
            }
            else if (ckbs.IsKeyDown(Keys.A) == true ||
                cgps.DPad.Left == ButtonState.Pressed)
            {
                this.PlayerOne.Box = new Rectangle(this.PlayerOne.Box.X - 1, this.PlayerOne.Box.Y, this.PlayerOne.Box.Width, this.PlayerOne.Box.Height);
                if (this.IsPlayerCollidingWithSolidSprite(this.PlayerOne))
                {
                    this.PlayerOne.Box = new Rectangle(this.PlayerOne.Box.X + 1, this.PlayerOne.Box.Y, this.PlayerOne.Box.Width, this.PlayerOne.Box.Height);
                }
            }

            

                if (ckbs.IsKeyDown(Keys.Escape) == true && okbs.IsKeyDown(Keys.Escape) != true ||
                cgps.Buttons.Back == ButtonState.Pressed && ogps.Buttons.Back != ButtonState.Pressed)
            {
                MainGameState.OpenMenu();
            }

            if (ckbs.IsKeyDown(Keys.Space) == true ||
                cgps.Buttons.A == ButtonState.Pressed)
            {
                if (this.PlayerOne.IsJumping == true && this.PlayerOne.IsFalling == false)
                {
                    if (this.PlayerOne.CurrentJumpTime <= this.PlayerOne.MaxJumpTime)
                    {                        
                        this.PlayerOne.Box = new Rectangle(this.PlayerOne.Box.X, this.PlayerOne.Box.Y - 2, this.PlayerOne.Box.Width, this.PlayerOne.Box.Height);
                    }
                    else if (this.PlayerOne.CurrentJumpTime > this.PlayerOne.MaxJumpTime)
                    {
                        this.PlayerOne.IsJumping = false;
                        this.PlayerOne.IsFalling = true;
                    }
                }
                else if (this.PlayerOne.IsJumping == false && this.PlayerOne.IsFalling == false && okbs.IsKeyDown(Keys.Space) == false && ogps.Buttons.A == ButtonState.Released)
                {
                    this.PlayerOne.IsJumping = true;
                    this.PlayerOne.CurrentJumpTime++;
                    this.PlayerOne.Box = new Rectangle(this.PlayerOne.Box.X, this.PlayerOne.Box.Y - 2, this.PlayerOne.Box.Width, this.PlayerOne.Box.Height);
                }
                this.PlayerOne.CurrentJumpTime++;
            }
            else
            {
                this.PlayerOne.IsJumping = false;
                this.PlayerOne.IsFalling = true;
            }

            this.Gravity(this.PlayerOne);


            //Editor
            if (ckbs.IsKeyDown(Keys.E) == true && ckbs != okbs)
            {
                if (this.IsEditingEnabled == false)
                {
                    this.IsEditingEnabled = true;
                }
                else
                {
                    this.IsEditingEnabled = false;
                }
            }

            if (this.IsEditingEnabled == true)
            {
                if (oms.ScrollWheelValue < cms.ScrollWheelValue)
                {
                    this.Camera.Zoom += 0.1f;
                }
                else if (oms.ScrollWheelValue > cms.ScrollWheelValue)
                {
                    this.Camera.Zoom -= 0.1f;
                }

                if (ckbs.IsKeyDown(Keys.Up) == true)
                {
                    this.Camera.Move(new Vector2(0, -2));
                }
                else if (ckbs.IsKeyDown(Keys.Down) == true)
                {
                    this.Camera.Move(new Vector2(0, 2));
                }

                if (ckbs.IsKeyDown(Keys.Right) == true)
                {
                    this.Camera.Move(new Vector2(2, 0));
                }
                else if (ckbs.IsKeyDown(Keys.Left) == true)
                {
                    this.Camera.Move(new Vector2(-2, 0));
                }

                

                if (this.SpriteLayers.Count > 0)
                {
                    foreach (KeyValuePair<string, List<Sprite>> kp in SpriteLayers)
                    {
                        if (kp.Value.Count > 0)
                        {
                            foreach (Sprite s in kp.Value)
                            {
                                if (s.IsAttachedToMouse == true)
                                {
                                    s.SpriteColor = Color.Yellow;
                                    if (ckbs.IsKeyDown(Keys.NumPad5) == true && okbs.IsKeyDown(Keys.NumPad5) == false)
                                    {
                                        if (s.SpriteType == (int)Sprite.SpriteTypes.ScissorSoldier)
                                        {
                                            s.SetSpriteType(1);
                                        }
                                        else
                                        {
                                            s.SetSpriteType(s.SpriteType + 1);                                            
                                        }
                                        s.IsAttachedToMouse = true;
                                        s.DrawHealthBar = false;
                                    }
                                }
                                else
                                {
                                    s.SpriteColor = Color.White;
                                }

                                

                                switch (kp.Key)
                                {
                                    case "Dynamic":
                                        break;
                                    case "Static":

                                        if (s.Box.Contains((int)this.Camera.GetMouseWorldPosition().X, (int)this.Camera.GetMouseWorldPosition().Y) == true && cms.LeftButton == ButtonState.Pressed && oms.LeftButton != ButtonState.Pressed)
                                        {
                                            if (s.IsAttachedToMouse == false)
                                            {
                                                s.IsAttachedToMouse = true;
                                            }
                                            else
                                            {
                                                s.IsAttachedToMouse = false;
                                            }
                                        }

                                        if (s.IsAttachedToMouse == true)
                                        {
                                            s.Box = new Rectangle((int)this.Camera.GetMouseWorldPosition().X - s.Box.Width / 2, (int)this.Camera.GetMouseWorldPosition().Y - s.Box.Height / 2, s.Box.Width, s.Box.Height);

                                            if (ckbs.IsKeyDown(Keys.Add) == true)
                                            {
                                                s.Height++;
                                                s.Width++;
                                            }
                                            else if (ckbs.IsKeyDown(Keys.Subtract) == true)
                                            {
                                                s.Height--;
                                                s.Width--;
                                            }

                                            if (ckbs.IsKeyDown(Keys.NumPad8) == true)
                                            {
                                                s.Height++;
                                            }
                                            else if (ckbs.IsKeyDown(Keys.NumPad2) == true)
                                            {
                                                s.Height--;
                                            }

                                            if (ckbs.IsKeyDown(Keys.NumPad4) == true)
                                            {
                                                s.Width--;
                                            }
                                            else if (ckbs.IsKeyDown(Keys.NumPad6) == true)
                                            {
                                                s.Width++;
                                            }

                                            

                                            if (ckbs.IsKeyDown(Keys.R) == true)
                                            {
                                                s.SpriteRotation++;
                                            }
                                        }
                                        break;
                                }
                            }
                        }
                    }
                }
            }

            //Debug
            if (ckbs.IsKeyDown(Keys.P) == true)
            {
                this.PlayerOne.CurrentHealth--;
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (this.SpriteLayers.Count > 0)
            {
                foreach (KeyValuePair<string, List<Sprite>> kp in SpriteLayers)
                {
                    if (kp.Value.Count > 0)
                    {
                        foreach (Sprite s in kp.Value)
                        {
                            if (this.IsEditingEnabled == true)
                            {

                                    spriteBatch.Begin();
                                    spriteBatch.Draw(this.LoadedTextures[0], new Rectangle(0, 0, MainGameState.GraphicsDevice.Viewport.Width, MainGameState.GraphicsDevice.Viewport.Height), null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);
                                    spriteBatch.End();

                            }
                            s.Draw(spriteBatch, this.Camera);
                        }
                    }
                }
            }
        }



        public void AddSpriteToLayer(string layername, Sprite s)
        {
            if (layername != "Players")
            {
                s.DrawHealthBar = false;
            }

            if (SpriteLayers.ContainsKey(layername) == true)
            {
                SpriteLayers[layername].Add(s);
            }
            else
            {
                List<Sprite> tmp = new List<Sprite>();
                tmp.Add(s);
                SpriteLayers.Add(layername, tmp);
            }

        }

        public void RemoveSpriteFromLayer(string layername, Sprite s)
        {
            if (SpriteLayers.ContainsKey(layername) == true)
            {
                SpriteLayers[layername].Remove(s);
            }
        }

        private void LoadMap()
        {
            this.SpriteLayers.Clear();

            if (this.Name != null && this.Name != "")
            {
                switch (this.Name)
                {
                    case "World 1":
                        this.AddSpriteToLayer("Static", new Sprite(1, 0, 310));
                        this.AddSpriteToLayer("Static", new Sprite(1, 50, 310));
                        this.AddSpriteToLayer("Static", new Sprite(1, 100, 310));
                        this.AddSpriteToLayer("Static", new Sprite(1, 175, 310));
                        this.AddSpriteToLayer("Static", new Sprite(1, 250, 305));
                        this.AddSpriteToLayer("Static", new Sprite(1, 325, 300));
                        this.AddSpriteToLayer("Static", new Sprite(1, 400, 295));
                        this.AddSpriteToLayer("Static", new Sprite(1, 475, 290));
                        this.AddSpriteToLayer("Players", this.PlayerOne);
                        break;
                    case "World 2":
                        this.AddSpriteToLayer("Static", new Sprite(1, 550, 550));
                        this.AddSpriteToLayer("Static", new Sprite(2, 650, 650));
                        this.AddSpriteToLayer("Static", new Sprite(3, 750, 700));
                        this.AddSpriteToLayer("Dynamic", new Sprite(4, 800, 700));
                        this.AddSpriteToLayer("Dynamic", new Sprite(5, 830, 700));
                        this.AddSpriteToLayer("Dynamic", new Sprite(4, 850, 700));
                        break;
                }
            }
        }

        private void Gravity(Sprite s)
        {
            if (SpriteLayers.ContainsKey("Static") == true)
            {
                foreach (Sprite s2 in SpriteLayers["Static"])
                {
                    if (s2.Box.Intersects(s.Box) == true)                    
                    {
                        return;
                    }
                }
                if (s.IsFalling == true &&
                   s.IsJumping == false)
                { 
                    s.Box = new Rectangle(s.Box.X, s.Box.Y + 1, s.Box.Width, s.Box.Height);
                    
                    if (this.IsPlayerCollidingWithSolidSprite(this.PlayerOne))
                    {
                        s.IsFalling = false;
                        this.PlayerOne.CurrentJumpTime = 0;
                        this.PlayerOne.Box = new Rectangle(this.PlayerOne.Box.X, this.PlayerOne.Box.Y - 1, this.PlayerOne.Box.Width, this.PlayerOne.Box.Height);
                    }
                }
            }
        }

        private bool IsPlayerCollidingWithSolidSprite(Sprite s)
        {
            if (SpriteLayers.ContainsKey("Static") == true)
            {
                foreach (Sprite s2 in SpriteLayers["Static"])
                {
                    if (s2.Box.Intersects(s.Box) == true)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}