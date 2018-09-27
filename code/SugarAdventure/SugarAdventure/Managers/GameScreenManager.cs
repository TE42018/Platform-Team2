using System;
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
    public class GameScreenManager
    {
        private Stack<GameScreen> gamescreens;
        public Stack<GameScreen> Gamescreens
        {
            get
            {
                return gamescreens;
            }
        }

        public GameScreenManager()
        {
            gamescreens = new Stack<GameScreen>();
        }

        public void Push(GameScreen pGameScreen)
        {
            gamescreens.Push(pGameScreen);
        }

        public void Update(GameTime pGameTime)
        {
            bool GameScreenPopped = false;
            do
            {
                GameScreenPopped = false;

                if (gamescreens.Count == 0)
                {
                    SugarGame.Instance.Exit();
                    return;
                }

                var gs = gamescreens.Peek();

                if(gs.Initialized == false)
                {
                    gs.Initialize();
                    gs.LoadContent(SugarGame.Instance.GraphicsDevice);
                }

                gs.Update(pGameTime);

                if (gs.Quit)
                {
                    gamescreens.Pop();
                    GameScreenPopped = true;
                }

            } while (GameScreenPopped);
        }

        public void Draw(SpriteBatch pSpriteBatch)
        {
            GameScreen gameScreen = gamescreens.Peek();
            if(gameScreen.Initialized)
                gameScreen.Draw(pSpriteBatch);
        }
    }
}
