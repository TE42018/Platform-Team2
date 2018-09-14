using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugarAdventure
{
    interface IEntity : IDrawable
    {
        string Type { get; set; } //Coin, enemy, key
        Vector2 Position { get; set; }
        Texture2D Texture { get; set; }
    }
}

