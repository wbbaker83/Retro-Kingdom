using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Retro_Kingdom
{
    class SideScrollerEngine
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

        public Sprite PlayerOne
        {
            get;
            set;
        }

        public Main MainGameState
        {
            get;
        }

        public SideScrollerEngine(string name, Main main)
        {
            SpriteLayers = new Dictionary<string, List<Sprite>>();
            this.Camera = new Camera2D(1280, 720);
            this.PlayerOne = new Sprite(0, 100, 290);
            this.Name = name;
            this.MainGameState = main;
            this.LoadMap();
        }

        public void Update(KeyboardState okbs, KeyboardState ckbs, MouseState oms, MouseState cms, GamePadState ogps, GamePadState cgps)
        {
            if (ckbs.IsKeyDown(Keys.D) == true)
            {
                this.PlayerOne.Box = new Rectangle(this.PlayerOne.Box.X + 1, this.PlayerOne.Box.Y, this.PlayerOne.Box.Width, this.PlayerOne.Box.Height);
                if (this.IsPlayerCollidingWithSolidSprite(this.PlayerOne))
                {
                    this.PlayerOne.Box = new Rectangle(this.PlayerOne.Box.X - 1, this.PlayerOne.Box.Y, this.PlayerOne.Box.Width, this.PlayerOne.Box.Height);
                }
            }
            else if (ckbs.IsKeyDown(Keys.A) == true)
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

            if (ckbs.IsKeyDown(Keys.Space) == true)
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
                else if (this.PlayerOne.IsJumping == false && this.PlayerOne.IsFalling == false && okbs.IsKeyDown(Keys.Space) == false)
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

            if (SpriteLayers.ContainsKey("Players") == true)
            {
                foreach (Sprite s in SpriteLayers["Players"])
                {

                }
            }

            if (SpriteLayers.ContainsKey("Static") == true)
            {
                foreach (Sprite s in SpriteLayers["Static"])
                {

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
            this.SpriteLayers.Clear();

            if (this.Name != null && this.Name != "")
            {
                switch (this.Name)
                {
                    case "World 1":
                        this.AddSpriteToLayer("Static", new Sprite(1, 0, 310));
                        this.AddSpriteToLayer("Static", new Sprite(1, 50, 310));
                        this.AddSpriteToLayer("Static", new Sprite(1, 100, 310));
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