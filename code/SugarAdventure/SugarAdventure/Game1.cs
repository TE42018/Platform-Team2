using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace SugarAdventure
{
    public class Game1 : Game
    {
        public static GraphicsDeviceManager graphics;
        public static ContentManager contentManager;
        InputManager inputManager;
        LevelManager levelManager;
        SpriteBatch spriteBatch;
        Level level;
        Player player;
        Camera cam;
        

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            inputManager = new InputManager();
            levelManager = new LevelManager();

            Content.RootDirectory = "Content";
            contentManager = Content;
        }
        
        protected override void Initialize()
        {
            cam = new Camera(Vector2.Zero, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            base.Initialize();
        }
        
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            levelManager.LoadContent();
            level = levelManager.GetLevel(LevelNumber.level1);


            cam.SetBoundingLevel(level);

            player = new Player(new Vector2(0, level.GetLayer("Ground").LayerHeight/2), level);
            player.LoadContent();
            player.SetBoundingLevel(level);
        }
        
        protected override void UnloadContent()
        {

        }
        
        protected override void Update(GameTime gameTime)
        {
            //cam.Update(gameTime);
            inputManager.Update();
            if (inputManager.IsPressed(Action.Exit))
                Exit();

            if (inputManager.IsPressed(Action.Up))
            {
                player.ClimbUp();
            }
            if (inputManager.IsPressed(Action.Down))
            {
                player.ClimbDown();
            }
            if (inputManager.IsPressed(Action.Left))
            {
                player.MoveLeft();
            }
            if (inputManager.IsPressed(Action.Right))
            {
                player.MoveRight();
            }
            if (!inputManager.IsPressed(Action.Right) && !inputManager.IsPressed(Action.Left))
            {
                player.Stop();
            }
            if (inputManager.IsTriggered(Action.Jump))
            {
                player.Jump();
            }
            player.Update(gameTime);
            cam.SetTarget(player.HitBox);
            cam.Update(gameTime);

            if (inputManager.IsTriggered(Action.Fullscreen))
            {
                graphics.PreferredBackBufferWidth = 1920;
                graphics.PreferredBackBufferHeight = 1080;
                graphics.ApplyChanges();
                cam.UpdateViewport(GraphicsDevice.Viewport.Bounds);
                graphics.ToggleFullScreen();
            }

            base.Update(gameTime);
        }
        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            float mScaleX = graphics.PreferredBackBufferWidth / cam.ViewportWidth;
            float mScaleY = graphics.PreferredBackBufferHeight / cam.ViewportHeight;
            float mOffestX = -cam.Pos.X * Camera.ZoomFactor;
            float mOffestY = -cam.Pos.Y * Camera.ZoomFactor;
            
            Matrix matrix = new Matrix(
                mScaleX, 0, 0, 0,
                0, mScaleY, 0, 0,
                0, 0, 1, 0,
                mOffestX, mOffestY, 0, 1
                );
            
            spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, matrix);
            
            level.Draw(spriteBatch);
            player.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
