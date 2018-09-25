﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;

namespace SugarAdventure
{
    public abstract class GameScreen
    {
        protected bool initialized;
        public bool Initialized
        {
            get
            {
                return initialized;
            }
        }

        protected bool quit;
        public bool Quit
        {
            get
            {
                return quit;
            }
        }

        public virtual void Initialize()
        {
            initialized = true;
        }

        public virtual void LoadContent(GraphicsDevice pGraphicsDevice)
        {

        }

        public virtual void Update(GameTime pGameTime)
        {

        }

        public virtual void Draw(SpriteBatch pSpriteBatch)
        {

        }
    }
}
