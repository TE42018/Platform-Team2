using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugarAdventure
{
    interface IPickupable
    {
        Rectangle Hitbox { get; set; }
    }
}
