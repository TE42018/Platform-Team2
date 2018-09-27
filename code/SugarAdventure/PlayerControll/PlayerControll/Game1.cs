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
            Character = new Player(Content.Load<Texture2D>("standPink"), new Vector2(50, 50));
            Character.LoadContent(Content);
            
            effect = Content.Load<SoundEffect>("Mario_Jumping-Mike_Koenig-989896458");
            //song = Content.Load<Song>("Seinfeld");

            MediaPlayer.Play(song);
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            input.Update();

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
                Character.MoveLeft();
            }
            else
            {
                Character.Stop();
            }

            if (input.IsPressed(Action.Up))
            {
                Character.ClimbUp();
            }
            else if (input.IsPressed(Action.Down))
            {
                Character.ClimbDown();
            }

            /*else
            {
                Character.CurrentAction = Action.None;
            }*/

            Character.Update(gameTime, effect);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue); 

            spriteBatch.Begin();

            Character.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
