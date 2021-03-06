﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Retro_Kingdom
{
    public class BarMeter
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

        public int Current { get; set; } = 0;
        public int Max { get; set; } = 0;
        public int Width { get; set; } = 0;
        public int Height { get; set; } = 0;
        public Vector2 Location { get; set; } = new Vector2(0, 0);

        private static Texture2D[] _loadedtextures;

        public BarMeter()
        {

        }

        public static void LoadContent(ContentManager conman)
        {
            int contentcount;

            contentcount = 1;
            _loadedtextures = new Texture2D[contentcount];
            _loadedtextures[1 - 1] = conman.Load<Texture2D>("Resources/textures/white_sq");
        }

        public void Update()
        {

        }

        public void Draw(SpriteBatch spriteBatch, Camera2D camera2D)
        {
            decimal tmpFrontWidth = (this.Width - 2);
            decimal tmpHealth = (decimal)this.Current / (decimal)this.Max;
            decimal frontWidth = tmpFrontWidth * tmpHealth;

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera2D.get_transformation());
            spriteBatch.Draw(this.LoadedTextures[0], new Rectangle((int)this.Location.X, (int)(this.Location.Y - this.Height), this.Width, this.Height), null, Color.Black, 0, new Vector2(0,0), SpriteEffects.None, 0);
            spriteBatch.Draw(this.LoadedTextures[0], new Rectangle((int)this.Location.X + 1, (int)(this.Location.Y - this.Height) + 1, (int)frontWidth, this.Height - 2), null, Color.Green, 0, new Vector2(0, 0), SpriteEffects.None, 0);
            spriteBatch.End();
        }
    }
}