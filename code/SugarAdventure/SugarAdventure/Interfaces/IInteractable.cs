using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugarAdventure
{
    interface IInteractable
    {
        Rectangle Hitbox { get; set; }
        IPickupable Key { get; set; }
    }
}
