using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
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

        public int CurrentSpriteCount
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

            if (ckbs.IsKeyDown(Keys.Escape) == true && okbs.IsKeyDown(Keys.Escape) != true ||
                cgps.Buttons.Back == ButtonState.Pressed && ogps.Buttons.Back != ButtonState.Pressed)
            {
                MainGameState.OpenMenu();
            }


            if (this.SpriteLayers.Count > 0)
            {
                foreach (KeyValuePair<string, List<Sprite>> kp in SpriteLayers)
                {
                    if (kp.Value.Count > 0)
                    {
                        foreach (Sprite s in kp.Value)
                        {
                            switch (kp.Key)
                            {
                                case "Dynamic":

                                    if (s.Box.Contains((int)this.Camera.GetMouseWorldPosition().X, (int)this.Camera.GetMouseWorldPosition().Y) == true && Mouse.GetState().LeftButton == ButtonState.Pressed)
                                    {
                                        s.Box = new Rectangle(s.Box.X + 3, s.Box.Y, s.Width, s.Height);
                                    }

                                    switch (s.Name)
                                    {
                                        case "Rock Soldier":
                                            AssignClosestEnemyToSprite(s);
                                            if (s.Box.X >= s.TargetedSprite.Box.X)
                                            {
                                                s.Box = new Rectangle(s.Box.X - 1, s.Box.Y, s.Box.Width, s.Box.Height);
                                            }
                                            else if (s.Box.X < s.TargetedSprite.Box.X)
                                            {
                                                s.Box = new Rectangle(s.Box.X + 1, s.Box.Y, s.Box.Width, s.Box.Height);
                                            }

                                            if (s.Box.Y >= s.TargetedSprite.Box.Y)
                                            {
                                                s.Box = new Rectangle(s.Box.X, s.Box.Y - 1, s.Box.Width, s.Box.Height);
                                            }
                                            else if (s.Box.Y < s.TargetedSprite.Box.Y)
                                            {
                                                s.Box = new Rectangle(s.Box.X, s.Box.Y + 1, s.Box.Width, s.Box.Height);
                                            }
                                            break;
                                        case "Paper Soldier":                                            
                                                AssignClosestEnemyToSprite(s);
                                                if (s.Box.X >= s.TargetedSprite.Box.X)
                                                {
                                                    s.Box = new Rectangle(s.Box.X - 1, s.Box.Y, s.Box.Width, s.Box.Height);
                                                }
                                                else if (s.Box.X < s.TargetedSprite.Box.X)
                                                {
                                                    s.Box = new Rectangle(s.Box.X + 1, s.Box.Y, s.Box.Width, s.Box.Height);
                                                }

                                                if (s.Box.Y >= s.TargetedSprite.Box.Y)
                                                {
                                                    s.Box = new Rectangle(s.Box.X, s.Box.Y - 1, s.Box.Width, s.Box.Height);
                                                }
                                                else if (s.Box.Y < s.TargetedSprite.Box.Y)
                                                {
                                                    s.Box = new Rectangle(s.Box.X, s.Box.Y + 1, s.Box.Width, s.Box.Height);
                                                }                                 
                                            break;
                                        case "Scissor Soldier":
                                            AssignClosestEnemyToSprite(s);
                                            if (s.Box.X >= s.TargetedSprite.Box.X)
                                            {
                                                s.Box = new Rectangle(s.Box.X - 1, s.Box.Y, s.Box.Width, s.Box.Height);
                                            }
                                            else if (s.Box.X < s.TargetedSprite.Box.X)
                                            {
                                                s.Box = new Rectangle(s.Box.X + 1, s.Box.Y, s.Box.Width, s.Box.Height);
                                            }

                                            if (s.Box.Y >= s.TargetedSprite.Box.Y)
                                            {
                                                s.Box = new Rectangle(s.Box.X, s.Box.Y - 1, s.Box.Width, s.Box.Height);
                                            }
                                            else if (s.Box.Y < s.TargetedSprite.Box.Y)
                                            {
                                                s.Box = new Rectangle(s.Box.X, s.Box.Y + 1, s.Box.Width, s.Box.Height);
                                            }
                                            break;
                                    }
                                    break;
                                case "Static":

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

                                    switch (s.Name)
                                    {
                                        case "Rock Base":
                                            if (s.Box.Contains((int)this.Camera.GetMouseWorldPosition().X, (int)this.Camera.GetMouseWorldPosition().Y) == true && Mouse.GetState().RightButton == ButtonState.Pressed && oms != cms && s.IsAttachedToMouse == false)
                                            {
                                                this.AddSpriteToLayer("Dynamic", new Sprite(4, s.Box.X + (s.Box.Width / 2), s.Box.Y - 10));
                                            }
                                            break;
                                        case "Paper Base":
                                            if (s.Box.Contains((int)this.Camera.GetMouseWorldPosition().X, (int)this.Camera.GetMouseWorldPosition().Y) == true && Mouse.GetState().RightButton == ButtonState.Pressed && oms != cms && s.IsAttachedToMouse == false)
                                            {
                                                this.AddSpriteToLayer("Dynamic", new Sprite(5, s.Box.X + (s.Box.Width / 2), s.Box.Y - 10));
                                            }
                                            break;
                                        case "Scissor Base":
                                            if (s.Box.Contains((int)this.Camera.GetMouseWorldPosition().X, (int)this.Camera.GetMouseWorldPosition().Y) == true && Mouse.GetState().RightButton == ButtonState.Pressed && oms != cms && s.IsAttachedToMouse == false)
                                            {
                                                this.AddSpriteToLayer("Dynamic", new Sprite(6, s.Box.X + (s.Box.Width / 2), s.Box.Y - 10));
                                            }
                                            break;
                                    }
                                    break;
                            }
                        }
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
                this.CurrentSpriteCount++;
            }
            else
            {
                List<Sprite> tmp = new List<Sprite>();
                tmp.Add(s);
                SpriteLayers.Add(layername, tmp);
                this.CurrentSpriteCount++;
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
                        this.AddSpriteToLayer("Dynamic", new Sprite(6, -285, -285));
                        this.AddSpriteToLayer("Dynamic", new Sprite(6, -280, -280));
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

        private bool IsSpriteColliding(Sprite s)
        {
            if (SpriteLayers.ContainsKey("Static") == true)
            {
                foreach (Sprite s2 in SpriteLayers["Static"])
                {
                    if (s2.Box.Intersects(s.Box) == true && s2.IsAttachedToMouse == false)
                    {
                        return true;
                    }
                }
            }
            return false;
        }


        private double GetDistance(Sprite sprite1, Sprite sprite2)
        {
            double a = (double)(sprite2.Box.X - sprite1.Box.X);
            double b = (double)(sprite2.Box.Y - sprite1.Box.Y);

            return Math.Sqrt(a * a + b * b);
        }

        private void AssignClosestEnemyToSprite(Sprite sprite1)
        {
            //Check if layers exist
            if (this.SpriteLayers.Count > 0)
            {
                //For each of the layers the world
                foreach (KeyValuePair<string, List<Sprite>> kp in SpriteLayers)
                {
                    //Make sure the layer has sprites
                    if (kp.Value.Count > 0)
                    {
                        //Foreach sprite in the layer
                        foreach (Sprite s in kp.Value)
                        {
                            //Check of original sprite is named correct
                            if (sprite1.Name == "Paper Soldier")
                            {
                                    //Check if other sprite is named correct 
                                    if (s.Name == "Rock Soldier")
                                    {
                                        //check if original sprite has a target
                                        if (sprite1.TargetedSprite == null)
                                        {
                                            //assign other sprite to tmp sprite
                                            sprite1.TargetedSprite = s;
                                        }
                                        //check if distance between original sprite and other sprite is less than or equal to
                                        //the distance between original sprite and original sprite's target
                                        else if (this.GetDistance(sprite1,s) <= this.GetDistance(sprite1, sprite1.TargetedSprite))
                                        {
                                        //assign other sprite to tmp sprite
                                        sprite1.TargetedSprite = s;
                                        }
                                    }                                    
                            }
                            else if (sprite1.Name == "Rock Soldier")
                            {
                                //Check if other sprite is named correct 
                                if (s.Name == "Scissor Soldier")
                                {
                                    //check if original sprite has a target
                                    if (sprite1.TargetedSprite == null)
                                    {
                                        //assign other sprite to tmp sprite
                                        sprite1.TargetedSprite = s;
                                    }
                                    //check if distance between original sprite and other sprite is less than or equal to
                                    //the distance between original sprite and original sprite's target
                                    else if (this.GetDistance(sprite1, s) <= this.GetDistance(sprite1, sprite1.TargetedSprite))
                                    {
                                        //assign other sprite to tmp sprite
                                        sprite1.TargetedSprite = s;
                                    }
                                }
                            }
                            else if (sprite1.Name == "Scissor Soldier")
                            {
                                //Check if other sprite is named correct 
                                if (s.Name == "Paper Soldier")
                                {
                                    //check if original sprite has a target
                                    if (sprite1.TargetedSprite == null)
                                    {
                                        //assign other sprite to tmp sprite
                                        sprite1.TargetedSprite = s;
                                    }
                                    //check if distance between original sprite and other sprite is less than or equal to
                                    //the distance between original sprite and original sprite's target
                                    else if (this.GetDistance(sprite1, s) <= this.GetDistance(sprite1, sprite1.TargetedSprite))
                                    {
                                        //assign other sprite to tmp sprite
                                        sprite1.TargetedSprite = s;
                                    }
                                }
                            }
                        }
                    }
                }
            }                           
        }
    }
}

