using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Meny
{
    class MenuChoice
    {
        public float X { get; set; }
        public float Y { get; set; }

        public string Text { get; set; }
        public bool Selected { get; set; }
        public bool Active { get; set; }

        public Action ClickAction { get; set; }
        public Rectangle HitBox { get; set; }

        public virtual void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            spriteBatch.DrawString(font,
                Text, new Vector2(X, Y), Color.White);
        }
    }
}