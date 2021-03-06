﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Retro_Kingdom
{
    public class Sprite
    {
        public enum SpriteTypes
        {
            RockBase = 1,
            PaperBase,
            ScissorBase,
            RockSoldier,
            PaperSoldier,
            ScissorSoldier
        };

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

        public Color SpriteColor
        {
            get;
            set;
        }

        public float SpriteRotation
        {
            get;
            set;
        }

        public bool IsAnimated
        {
            get;
            set;
        }

        public int CurrentHealth
        {
            get;
            set;
        }

        public int MaxHealth
        {
            get;
            set;
        }

        public BarMeter HealthBar
        {
            get;
            set;
        }

        public bool DrawHealthBar
        {
            get;
            set;
        }

        public Sprite TargetedSprite
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
            this.TargetedSprite = null;

            this.SpriteID = _spritecount;
            _spritecount++;
        }


        public static void LoadContent(ContentManager conman)
        {
            int texturecount = 6;
            _loadedtextures = new Texture2D[texturecount];
            _loadedtextures[1 - 1] = conman.Load<Texture2D>("Resources/textures/base_rock");
            _loadedtextures[2 - 1] = conman.Load<Texture2D>("Resources/textures/base_paper");
            _loadedtextures[3 - 1] = conman.Load<Texture2D>("Resources/textures/base_scissor");
            _loadedtextures[4 - 1] = conman.Load<Texture2D>("Resources/textures/soldier_rock");
            _loadedtextures[5 - 1] = conman.Load<Texture2D>("Resources/textures/soldier_paper");
            _loadedtextures[6 - 1] = conman.Load<Texture2D>("Resources/textures/soldier_scissor");
        }



        public virtual void Update()
        {

        }

        public virtual void Draw(SpriteBatch spriteBatch, Camera2D camera2D)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera2D.get_transformation());

            if (this.IsFacingLeft == false)
            {
                spriteBatch.Draw(this.Textures[0], this.Box, null, this.SpriteColor, this.SpriteRotation, new Vector2(0, 0), SpriteEffects.FlipHorizontally, 0);
            }
            else
            {
                spriteBatch.Draw(this.Textures[0], this.Box, null, this.SpriteColor, this.SpriteRotation, new Vector2(0, 0), SpriteEffects.None, 0);
            }
            spriteBatch.End();

            if (this.DrawHealthBar == true)
            {
                this.HealthBar.Height = 6;
                this.HealthBar.Width = 18;
                this.HealthBar.Location = new Vector2((this.Box.Location.X + (this.Box.Width / 2)) - (this.HealthBar.Width / 2), this.Box.Location.Y);
                this.HealthBar.Location.Normalize();
                
                this.HealthBar.Current = this.CurrentHealth;
                this.HealthBar.Max = this.MaxHealth;
                this.HealthBar.Draw(spriteBatch, camera2D);
            }
           
        }

        public void SetSpriteType(int type)
        {
            this.SpriteType = type;
            this.Textures = new Texture2D[1];
            this.IsAttachedToMouse = false;
            this.IsFalling = false;
            this.IsJumping = false;
            this.CurrentJumpTime = 0;
            this.MaxJumpTime = 0;
            this.HealthBar = new BarMeter();
            this.CurrentHealth = 100;
            this.MaxHealth = 100;
            this.DrawHealthBar = true;
            this.SpriteColor = Color.White;

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
                        this.MaxJumpTime = 10;
                        break;
                    case (int)SpriteTypes.RockBase:
                        this.Name = "Rock Base";
                        this.Box = new Rectangle(this.Box.X, this.Box.Y, 50, 50);
                        this.Textures[0] = Sprite._loadedtextures[1 - 1];
                        this.IsFacingLeft = false;
                        this.IsAnimated = false;
                        break;
                    case (int)SpriteTypes.PaperBase:
                        this.Name = "Paper Base";
                        this.Box = new Rectangle(this.Box.X, this.Box.Y, 50, 50);
                        this.Textures[0] = Sprite._loadedtextures[2 - 1];
                        this.IsFacingLeft = false;
                        this.IsAnimated = false;
                        break;
                    case (int)SpriteTypes.ScissorBase:
                        this.Name = "Scissor Base";
                        this.Box = new Rectangle(this.Box.X, this.Box.Y, 50, 50);
                        this.Textures[0] = Sprite._loadedtextures[3 - 1];
                        this.IsFacingLeft = false;
                        this.IsAnimated = false;
                        break;
                    case (int)SpriteTypes.RockSoldier:
                        this.Name = "Rock Soldier";
                        this.Box = new Rectangle(this.Box.X, this.Box.Y, 10, 10);
                        this.Textures[0] = Sprite._loadedtextures[4 - 1];
                        this.IsFacingLeft = false;
                        this.IsAnimated = false;
                        break;
                    case (int)SpriteTypes.PaperSoldier:
                        this.Name = "Paper Soldier";
                        this.Box = new Rectangle(this.Box.X, this.Box.Y, 10, 10);
                        this.Textures[0] = Sprite._loadedtextures[5 - 1];
                        this.IsFacingLeft = false;
                        this.IsAnimated = false;
                        break;
                    case (int)SpriteTypes.ScissorSoldier:
                        this.Name = "Scissor Soldier";
                        this.Box = new Rectangle(this.Box.X, this.Box.Y, 10, 10);
                        this.Textures[0] = Sprite._loadedtextures[6 - 1];
                        this.IsFacingLeft = false;
                        this.IsAnimated = false;
                        break;
                }
            }
        }
    }
}