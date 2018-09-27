using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugarAdventure
{
    class MainGameScreen : GameScreen
    {
        Level level;
        Player player;
        Camera cam;

        public MainGameScreen()
        {
            if(cam == null)
                cam = new Camera(Vector2.Zero, SugarGame.graphics.GraphicsDevice.Viewport.Width, SugarGame.graphics.GraphicsDevice.Viewport.Height, 0.6f);
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void LoadContent(GraphicsDevice pGraphicsDevice)
        {
            level = SugarGame.levelManager.LoadLevel(LevelNumber.level3);
            
            //SugarGame.entityManager.LoadContent();

            Camera.SetBoundingLevel(level);

            player = new Player(level, 4);
            player.LoadContent();
            player.SetBoundingLevel(level);

            base.LoadContent(pGraphicsDevice);
        }

        public void Exit()
        {
            quit = true;
        }

        public override void Update(GameTime pGameTime)
        {
            if (SugarGame.inputManager.IsKeyPressed(Keys.Escape))
            {
                quit = true;
                SugarGame.Instance.Components.Add(new MenuComponent(SugarGame.Instance));
            }
            if (SugarGame.inputManager.IsPressed(Actions.Up))
            {
                player.ClimbUp();
            }
            else if (SugarGame.inputManager.IsPressed(Actions.Down))
            {
                player.ClimbDown();
            }
            if (!SugarGame.inputManager.IsPressed(Actions.Up) && !SugarGame.inputManager.IsPressed(Actions.Down))
            {
                player.StopClimbing();
            }
            if (SugarGame.inputManager.IsPressed(Actions.Left))
            {
                player.MoveLeft();
            }
            else if (SugarGame.inputManager.IsPressed(Actions.Right))
            {
                player.MoveRight();
            }
            if (!SugarGame.inputManager.IsPressed(Actions.Right) && !SugarGame.inputManager.IsPressed(Actions.Left))
            {
                player.Stop();
            }

            if (SugarGame.inputManager.IsTriggered(Actions.Activate))
            {
                player.CurrentAction = Actions.Activate;
            }
            if (SugarGame.inputManager.IsTriggered(Actions.Jump))
            {
                player.Jump();
            }
            if (SugarGame.inputManager.IsKeyPressed(Keys.R))
            {
                player.Reset();
            }
            if (SugarGame.inputManager.IsKeyTriggered(Keys.F))
            {
                SugarGame.graphics.PreferredBackBufferWidth = 1920;
                SugarGame.graphics.PreferredBackBufferHeight = 1080;
                SugarGame.graphics.ApplyChanges();
                SugarGame.graphics.ToggleFullScreen();
                cam.UpdateViewport(SugarGame.Instance.GraphicsDevice.Viewport.Bounds);
            }
            player.Update(pGameTime);
            SugarGame.entityManager.Update(player, pGameTime);
            cam.SetTarget(player.Hitbox);
            cam.Update(pGameTime);
            base.Update(pGameTime);
        }

        public override void Draw(SpriteBatch pSpriteBatch)
        {
            pSpriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, cam.ViewMatrix);

            if (player != null)
            {
                player.Level.Draw(pSpriteBatch);
                player.Draw(pSpriteBatch);
                SugarGame.entityManager.Draw(pSpriteBatch);
            }
            base.Draw(pSpriteBatch);
            pSpriteBatch.End();

            pSpriteBatch.Begin();
                //Draw UI elements
            pSpriteBatch.End();
        }
    }
}
