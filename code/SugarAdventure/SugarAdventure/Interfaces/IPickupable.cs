using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugarAdventure
{
    public interface IPickupable
    {
        Rectangle Hitbox { get; set; }
        SoundEffect Sound { get; set; }
    }
}
