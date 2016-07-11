using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Retro_Kingdom
{
    public class Sprite
    {
        private static int _spritecount = 1;
        private static Texture2D[] _loadedtextures;

        public int SpriteID
        {
            get;
        }

        public String Name
        {
            get;
            set;
        }

        public int SpriteType
        {
            get;
            set;
        }

        public Random RandomNumber
        {
            get { return new Random(BitConverter.ToInt32(Guid.NewGuid().ToByteArray(), 0)); }
        }

        public Rectangle Box
        {
            get;
            set;
        }

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

        public Texture2D[] Textures
        {
            get;
            set;
        }

        public bool IsFalling
        {
            get;
            set;
        }

        public bool IsJumping
        {
            get;
            set;
        }

        public bool IsFacingLeft
        {
            get;
            set;
        }

        public int CurrentJumpTime
        {
            get;
            set;
        }

        public int MaxJumpTime
        {
            get;
            set;
        }

        public bool IsAttachedToMouse
        {
            get;
            set;
        }

        public bool IsAnimated
        {
            get;
            set;
        }

        public int Width
        {
            get { return this.Box.Width; }
            set
            {
                this.Box = new Rectangle(this.Box.X, this.Box.Y, value, this.Box.Height);
            }
        }

        public int Height
        {
            get { return this.Box.Height; }
            set
            {
                this.Box = new Rectangle(this.Box.X, this.Box.Y, this.Box.Width, value);
            }
        }

        public Sprite(int type, int posx, int posy)
        {
            this.Box = new Rectangle(posx, posy, 0, 0);
            this.SetSpriteType(type);

            this.SpriteID = _spritecount;
            _spritecount++;
        }


        public static void LoadContent(ContentManager conman)
        {
            int texturecount = 5;
            _loadedtextures = new Texture2D[texturecount];
            _loadedtextures[1 - 1] = conman.Load<Texture2D>("Resources/textures/base_rock");
            _loadedtextures[2 - 1] = conman.Load<Texture2D>("Resources/textures/base_paper");
            _loadedtextures[3 - 1] = conman.Load<Texture2D>("Resources/textures/base_scissor");
            _loadedtextures[4 - 1] = conman.Load<Texture2D>("Resources/textures/soldier_rock");
            _loadedtextures[5 - 1] = conman.Load<Texture2D>("Resources/textures/soldier_paper");
        }



        public virtual void Update()
        {

        }

        public virtual void Draw(SpriteBatch spriteBatch, Camera2D camera2D)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera2D.get_transformation());

            if (this.IsFacingLeft == false)
            {
                spriteBatch.Draw(this.Textures[0], this.Box, null, Color.White, 0, new Vector2(0, 0), SpriteEffects.FlipHorizontally, 0);
            }
            else
            {
                spriteBatch.Draw(this.Textures[0], this.Box, null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);
            }
            spriteBatch.End();
        }

        private void SetSpriteType(int type)
        {
            this.SpriteType = type;
            this.Textures = new Texture2D[1];
            this.IsAttachedToMouse = false;
            this.IsFalling = false;
            this.IsJumping = false;
            this.CurrentJumpTime = 0;
            this.MaxJumpTime = 0;

            if (this.SpriteType >= 0)
            {
                switch (this.SpriteType)
                {
                    case 0:
                        this.Name = "Player One";
                        this.Box = new Rectangle(this.Box.X, this.Box.Y, 10, 10);
                        this.Textures[0] = Sprite._loadedtextures[4 - 1];
                        this.IsFacingLeft = false;
                        this.IsAnimated = false;
                        this.IsFalling = true;
                        this.IsJumping = false;
                        this.CurrentJumpTime = 0;
                        this.MaxJumpTime = 30;
                        break;
                    case 1:
                        this.Name = "Rock Base";
                        this.Box = new Rectangle(this.Box.X, this.Box.Y, 50, 50);
                        this.Textures[0] = Sprite._loadedtextures[1 - 1];
                        this.IsFacingLeft = false;
                        this.IsAnimated = false;
                        break;
                    case 2:
                        this.Name = "Paper Base";
                        this.Box = new Rectangle(this.Box.X, this.Box.Y, 50, 50);
                        this.Textures[0] = Sprite._loadedtextures[2 - 1];
                        this.IsFacingLeft = false;
                        this.IsAnimated = false;
                        break;
                    case 3:
                        this.Name = "Scissor Base";
                        this.Box = new Rectangle(this.Box.X, this.Box.Y, 50, 50);
                        this.Textures[0] = Sprite._loadedtextures[3 - 1];
                        this.IsFacingLeft = false;
                        this.IsAnimated = false;
                        break;
                    case 4:
                        this.Name = "Rock Soldier";
                        this.Box = new Rectangle(this.Box.X, this.Box.Y, 10, 10);
                        this.Textures[0] = Sprite._loadedtextures[4 - 1];
                        this.IsFacingLeft = false;
                        this.IsAnimated = false;
                        break;
                    case 5:
                        this.Name = "Paper Soldier";
                        this.Box = new Rectangle(this.Box.X, this.Box.Y, 10, 10);
                        this.Textures[0] = Sprite._loadedtextures[5 - 1];
                        this.IsFacingLeft = false;
                        this.IsAnimated = false;
                        break;
                }
            }
        }
    }
}