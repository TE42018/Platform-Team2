using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace SugarAdventure
{
    public class Game1 : Game
    {
        private static Game1 instance;
        public static Game1 Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new Game1();
                }
                return instance;
            }
        }

        public static GraphicsDeviceManager graphics;
        public static ContentManager contentManager;
        public static EntityManager entityManager;
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
            entityManager = new EntityManager();

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
            entityManager.LoadContent();
            levelManager.LoadContent();
            level = levelManager.GetLevel(LevelNumber.level1);


            cam.SetBoundingLevel(level);

            player = new Player(new Vector2(70 * 1, level.GetLayer("Ground").LayerHeight/2 + 70*4), level);
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
                instance.Exit();

            if (inputManager.IsPressed(Action.Up))
            {
                player.ClimbUp();
            }
            if (inputManager.IsPressed(Action.Down))
            {
                player.ClimbDown();
            }
            if(!inputManager.IsPressed(Action.Up) && !inputManager.IsPressed(Action.Down))
            {
                player.StopClimbing();
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
            if (inputManager.IsKeyPressed(Keys.R))
            {
                player.Reset();
            }
            player.Update(gameTime);
            entityManager.Update(player, gameTime);
            cam.SetTarget(player.Hitbox);
            cam.Update(gameTime);

            if (inputManager.IsTriggered(Action.Fullscreen))
            {
                if (!graphics.IsFullScreen)
                {
                    graphics.PreferredBackBufferWidth = 1920;
                    graphics.PreferredBackBufferHeight = 1080;
                    graphics.ApplyChanges();
                }
                else
                {
                    graphics.PreferredBackBufferWidth = 800;
                    graphics.PreferredBackBufferHeight = 480;
                    graphics.ApplyChanges();
                }
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
            entityManager.Draw(spriteBatch);
            player.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
