using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugarAdventure
{
    public interface IEnemy
    {
        Rectangle Hitbox { get; set; }
        int AttackDamage { get; set; }

        void Update(GameTime gameTime, Level level);
        void Walk();
    }
}
