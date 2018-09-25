using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayerControll
{
    class Sprite
    {
        protected AnimationManager _animationManager;
        protected Dictionary<string, Animation> _animations;
        protected Vector2 _position;
        protected Texture2D _texture;
        public InputManager Input;

        public Vector2 Position
        {
            get { return _position; }
            set
            {
                _position = value;
                if (_animationManager != null)
                    _animationManager.Position = _position;
            }
        }

        public float speed = 1f;
        public Vector2 Velocity;

        public virtual void Draw(SpriteBatch spritBatch)
        {
            if (_texture != null)  
                spritBatch.Draw(_texture, Position, Color.White);
            else if (_animationManager != null)
                _animationManager.Draw(spritBatch);
            else throw new Exception("This ain`t right..!");
        }

        protected virtual void Move()
        {
            if (Keyboard.GetState().IsKeyDown(Input.Right))
                Velocity.Y = speed;
            else if (Keyboard.GetState().IsKeyDown(Input.Left))
                Velocity.Y = -speed;
        }

        protected virtual void SetAnimations()
        {
            if (Velocity.X > 0)
                _animationManager.Play(_animations["alienPink_walk1"]);
            else  if (Velocity.X < 0)
                  _animationManager.Play(_animations["alienPink_walk2"]);
        }

        public Sprite(Dictionary<string, Animation> animations)
        {
            _animations = animations;
            _animationManager = new AnimationManager(_animations.First().Value);
        }

        public Sprite(Texture2D texture)
        {
            _texture = texture;
        }

        public virtual void Update(GameTime gameTime, List<Sprite> sprites)
        {
            Move();
            SetAnimations();
            _animationManager.Update(gameTime);
            Position += Velocity;
            Velocity = Vector2.Zero;
        }
    }
}
