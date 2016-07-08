using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Retro_Kingdom
{
    class RTSEngine
    {
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

        public Main MainGameState
        {
            get;
        }

        public RTSEngine(string name, Main main)
        {
            SpriteLayers = new Dictionary<string, List<Sprite>>();
            this.Camera = new Camera2D(1280, 720);
            this.Name = name;
            this.MainGameState = main;
            this.LoadMap();
        }

        public void Update(KeyboardState okbs, KeyboardState ckbs, MouseState oms, MouseState cms, GamePadState ogps, GamePadState cgps)
        {
            if (ckbs.IsKeyDown(Keys.W) == true)
            {
                this.Camera.Move(new Vector2(0, -1));
            }
            else if (ckbs.IsKeyDown(Keys.S) == true)
            {
                this.Camera.Move(new Vector2(0, 1));
            }

            if (ckbs.IsKeyDown(Keys.D) == true)
            {
                this.Camera.Move(new Vector2(1, 0));
            }
            else if (ckbs.IsKeyDown(Keys.A) == true)
            {
                this.Camera.Move(new Vector2(-1, 0));
            }

            if (oms.ScrollWheelValue < cms.ScrollWheelValue)
            {
                this.Camera.Zoom += 0.1f;
            }
            else if (oms.ScrollWheelValue > cms.ScrollWheelValue)
            {
                this.Camera.Zoom -= 0.1f;
            }

            if (ckbs.IsKeyDown(Keys.Escape) == true && okbs.IsKeyDown(Keys.Escape) != true)
            {
                MainGameState.OpenMenu();
            }


            if (SpriteLayers.ContainsKey("Dynamic") == true)
            {
                foreach (Sprite s in SpriteLayers["Dynamic"])
                {
                    if (s.Name == "Paper Soldier" | s.Name == "Rock Soldier")
                    {
                        s.Box = new Rectangle(s.Box.X + s.RandomNumber.Next(-3, 4), s.Box.Y + s.RandomNumber.Next(-1, 2), s.Box.Width, s.Box.Height);
                    }

                    if (s.Box.Contains((int)this.Camera.GetMouseWorldPosition().X, (int)this.Camera.GetMouseWorldPosition().Y) == true && Mouse.GetState().LeftButton == ButtonState.Pressed)
                    {
                        s.Box = new Rectangle(s.Box.X + 1, s.Box.Y, s.Width, s.Height);
                    }
                }
            }

            if (SpriteLayers.ContainsKey("Static") == true)
            {
                foreach (Sprite s in SpriteLayers["Static"])
                {
                    if (s.Box.Contains((int)this.Camera.GetMouseWorldPosition().X, (int)this.Camera.GetMouseWorldPosition().Y) == true && Mouse.GetState().LeftButton == ButtonState.Pressed && oms != cms)
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
                    }
                }
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
                            s.Draw(spriteBatch, this.Camera);
                        }
                    }
                }
            }
        }



        public void AddSpriteToLayer(string layername, Sprite s)
        {
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
            if (this.Name != null && this.Name != "")
            {
                switch (this.Name)
                {
                    case "World 1":
                        this.AddSpriteToLayer("Static", new Sprite(1, 0, 0));
                        this.AddSpriteToLayer("Static", new Sprite(2, 100, 100));
                        this.AddSpriteToLayer("Static", new Sprite(3, 150, 150));
                        this.AddSpriteToLayer("Dynamic", new Sprite(4, -200, -200));
                        this.AddSpriteToLayer("Dynamic", new Sprite(5, -230, -230));
                        this.AddSpriteToLayer("Dynamic", new Sprite(4, -250, -250));
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
    }
}
