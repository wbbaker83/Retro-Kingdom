using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace Retro_Kingdom
{
    class GameSoundEffect
    {
        private static SoundEffect[] _loadedsoundeffects;

        public SoundEffect[] LoadedSoundEffects
        {
            get
            {
                return _loadedsoundeffects;
            }
            set
            {
                _loadedsoundeffects = value;
            }
        }

        public int SoundEffectType
        {
            get;
            set;
        }

        public SoundEffect GameSound
        {
            get;
            set;
        }

        public GameSoundEffect(int type)
        {
            this.SetSoundType(type);
        }

        public static void LoadContent(ContentManager conman)
        {
            int texturecount = 1;
            _loadedsoundeffects = new SoundEffect[texturecount];
            _loadedsoundeffects[1 - 1] = conman.Load<SoundEffect>("Resources/sounds/menu_move");
        }

        private void SetSoundType(int type)
        {
            this.SoundEffectType = type;

            switch (this.SoundEffectType)
            {
                case 0:
                    this.GameSound = this.LoadedSoundEffects[this.SoundEffectType];
                    this.GameSound.CreateInstance();
                    break;
            }
        }

        public void PlaySoundEffect()
        {
            this.GameSound.Play();
        }
    }
}