using Microsoft.Xna.Framework;
using System;

namespace MonoGame.Meny
{
    internal class SubMenu
    {
        public float X { get; set; }
        public float Y { get; set; }

        public string Text { get; set; }
        public bool Selected { get; set; }

        public Action ClickAction { get; set; }
        public Rectangle HitBox { get; set; }
        public object Choices { get; internal set; }
    }
}