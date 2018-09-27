using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace SugarAdventure
{
    public class SugarGame : Game
    {
        private static SugarGame instance;
        public static SugarGame Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new SugarGame();
                }
                return instance;
            }
        }

        private GameScreenManager gsm;
        public GameScreenManager GSM
        {
            get
            {
                return gsm;
            }
        }

        public static GraphicsDeviceManager graphics;
        public static ContentManager contentManager;
        public static InputManager inputManager;
        public static LevelManager levelManager;
        public static EntityManager entityManager;
        SpriteBatch spriteBatch;
        
        

        public SugarGame()
        {
            IsMouseVisible = true;
            graphics = new GraphicsDeviceManager(this);
            inputManager = new InputManager();
            levelManager = new LevelManager();
            levelManager.SetSource(@"data\");
            gsm = new GameScreenManager();

            gsm.Push(new StartupGameScreen());
            //gsm.Push(new MainGameScreen());

            Content.RootDirectory = "Content";
            contentManager = Content;
        }
        
        protected override void Initialize()
        {
            entityManager = new EntityManager();
            //Components.Add(new MenuComponent(this));
            base.Initialize();
        }
        
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            entityManager.LoadContent();
            levelManager.LoadContent();
        }
        
        protected override void UnloadContent()
        {

        }
        
        protected override void Update(GameTime gameTime)
        {
            //cam.Update(gameTime);
            inputManager.Update();
            //if (inputManager.IsPressed(Actions.Back))
                //instance.Exit();

            gsm.Update(gameTime);

            

            

            base.Update(gameTime);
        }
        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            
            //spriteBatch.Begin();

            gsm.Draw(spriteBatch);
            //spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, matrix);

            //level.Draw(spriteBatch);
            //entityManager.Draw(spriteBatch);
            //player.Draw(spriteBatch);
            //spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
