using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Meny
{
    internal class MenuChoiceValue : MenuChoice
    {
        private readonly int _min;
        private readonly int _max;
        private readonly string _suffix;

        public int Value { get; set; }

        public MenuChoiceValue(int value, int min, int max, string suffix)
        {
            Value = value;
            _min = min;
            _max = max;
            _suffix = suffix;
        }

        public MenuChoiceValue(int value) : this(value, 0, 100, "%")
        {
            
        }

        public override void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            var text = Text + " " + + Value + _suffix;
            Vector2 size = font.MeasureString(text);
            X = spriteBatch.GraphicsDevice.Viewport.Width / 2.0f - size.X / 2;

            spriteBatch.DrawString(font,
                text, new Vector2(X, Y), Color.White);
        }
    }
}