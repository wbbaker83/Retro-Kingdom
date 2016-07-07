using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Retro_Kingdom
{
    public class Camera2D
    {
        protected float _zoom;
        public Matrix _transform;

        public int ResolutionWidth
        {
            get;
            set;
        }

        public int ResolutionHeight
        {
            get;
            set;
        }

        public float Zoom
        {
            get { return _zoom; }
            set { _zoom = value; if (_zoom < 0.1f) _zoom = 0.1f; } // Negative zoom will flip image
        }

        public Vector2 Position
        {
            get;
            set;
        }

        public float Rotation
        {
            get;
            set;
        }

        public Camera2D(int reswidth, int resheight)
        {
            _zoom = 1.0f;
            this.Rotation = 0.0f;
            this.Position = new Vector2(0, 0);

            this.ResolutionHeight = resheight;
            this.ResolutionWidth = reswidth;
        }

        public void Move(Vector2 amount)
        {
            this.Position += amount;
        }

        public Vector2 GetMouseScreenPosition()
        {
            return Vector2.Transform(new Vector2(Mouse.GetState().X, Mouse.GetState().Y), this._transform);
        }

        public Vector2 GetMouseWorldPosition()
        {
            return Vector2.Transform(new Vector2(Mouse.GetState().X, Mouse.GetState().Y), Matrix.Invert(this._transform));
        }

        public Vector2 GetScreenPosition(Vector2 worldPosition)
        {
            return Vector2.Transform(worldPosition, this._transform);
        }

        public Vector2 GetWorldPosition(Vector2 screenPosition)
        {
            return Vector2.Transform(screenPosition, Matrix.Invert(this._transform));
        }

        public Matrix get_transformation()
        {
            _transform =
              Matrix.CreateTranslation(
                  new Vector3(-this.Position.X, -this.Position.Y, 0)) *
                  Matrix.CreateRotationZ(Rotation) *
                  Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
                  Matrix.CreateTranslation(
                      new Vector3(this.ResolutionWidth * 0.5f, this.ResolutionHeight * 0.5f, 0));
            return _transform;
        }
    }
}
