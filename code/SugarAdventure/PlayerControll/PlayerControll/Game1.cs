using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace PlayerControll
{

    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        InputManager input;

        private List<Sprite> _sprites;

        Player Character;

        SoundEffect effect;
        Song song; 

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            input = new InputManager();
            Content.RootDirectory = "Content";
        }
        
        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
           
            spriteBatch = new SpriteBatch(GraphicsDevice);

            var animations = new Dictionary<string, Animation>()
            {
                {"alien_walk1", new Animation(Content.Load<Texture2D>("Player/alienPink_walk1"), 1) },
                {"alien_walk2", new Animation(Content.Load<Texture2D>("Player/alienPink_walk2"), 1) },
            };

            //_sprites = new List<Sprite>()
            //{
            //    new Sprite(animations)
            //    {
            //        Position = new Vector2(100,100),
            //        Input = new InputManager()
            //        {
            //            Right = Keys.Right,
            //            Left = Keys.Left
            //        },
            //    },
            //};
            Character = new Player(Content.Load<Texture2D>("p3_front"), new Vector2(50, 50));
            Character.LoadContent(Content);
            
            effect = Content.Load<SoundEffect>("Mario_Jumping-Mike_Koenig-989896458");
            song = Content.Load<Song>("Seinfeld");

            MediaPlayer.Play(song);
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            input.Update();

            //foreach (var sprite in _sprites)
                //sprite.Update(gameTime, _sprites);

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (input.IsTriggered(Action.Jump))
            {
                Character.Jump();
                effect.Play();
            }
            if (input.IsPressed(Action.Right))
            {
                Character.MoveRight();
            }
            else if (input.IsPressed(Action.Left))
            {
                Character.MoveRight();
            }

            else
            {
                Character.CurrentAction = Action.None;
            }

            Character.Update(gameTime, effect);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue); 

            spriteBatch.Begin();

            //foreach (var sprite in _sprites)
                //sprite.Draw(spriteBatch);
            Character.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
