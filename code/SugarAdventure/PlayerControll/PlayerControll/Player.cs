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
using Microsoft.Xna.Framework.Content;

namespace PlayerControll
{
    class Player 
    {
        Texture2D texture;
        Animation walkAnim;
        AnimationManager animator;

        Vector2 position;
        Vector2 velocity;

        bool hasJumped;
        bool isClimbing;
        bool isOnGround;
        bool canClimb;
       

        public Action CurrentAction { get; set; }

        public Player(Texture2D newTexture, Vector2 newPosition)
        {
            texture = newTexture;
            position = newPosition;
            hasJumped = true;
            
        }

        public void LoadContent(ContentManager Content)
        {
            walkAnim = new Animation(Content.Load<Texture2D>("walkPink"), 10);
            walkAnim.FrameSpeed = 0.01f;
            animator = new AnimationManager(walkAnim);
        }

        public void Update(GameTime gameTime, SoundEffect effect)
        {
            position += velocity;
           
            float i = 1;
            velocity.Y += 0.15f * i;

            if (position.Y + texture.Height >= 450)
            {
                position.Y = 450 - texture.Height;
                velocity.Y = 0;
                isOnGround = true;
            }

            animator.Position = position;
            

            switch (CurrentAction)
            {
                case Action.Left: velocity.X = -3f; animator.Update(gameTime); break;
                case Action.Right: velocity.X = 3f; animator.Update(gameTime); break;
                case Action.None: velocity.X = 0f; animator.Stop(); break;
            }
            
        }

        public void Jump()
        {
            if (isOnGround)
            velocity.Y = -6f;
            isOnGround = false;
        }

        public void MoveLeft()
        {
            CurrentAction = Action.Left;
        }

        public void MoveRight()
        {
            CurrentAction = Action.Right;
        }

        public void ClimbUp()
        {
            if (canClimb)
            {

            }
        }
        public void ClimbDown()
        {
            if (canClimb && !isOnGround)
            {

            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            switch (CurrentAction)
            {
                case Action.Right: animator.Draw(spriteBatch); break;
                case Action.Left: animator.Draw(spriteBatch, true); break;
                default: animator.Draw(spriteBatch); break;
            }
            //spriteBatch.Draw(texture, position, Color.White);
        }
    }
}
