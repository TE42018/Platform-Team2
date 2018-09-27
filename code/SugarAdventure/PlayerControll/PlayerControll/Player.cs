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
        Animation jumpAnim;
        Animation climbAnim;
        Animation standAnim;
        AnimationManager animator;

        Vector2 position;
        Vector2 velocity;

        bool hasJumped;
        bool isClimbing;
        private bool isOnGround;
        private bool canClimb;

        public Action CurrentAction { get; set; }

        public Player(Texture2D newTexture, Vector2 newPosition)
        {
            texture = newTexture;
            position = newPosition;
            hasJumped = true;
            
        }

        public void LoadContent(ContentManager Content)
        {
            standAnim = new Animation(Content.Load<Texture2D>("standPink"), 1);
            animator = new AnimationManager(standAnim);

            walkAnim = new Animation(Content.Load<Texture2D>("walkPink"), 10);
            walkAnim.FrameSpeed = 0.04f;

            climbAnim = new Animation(Content.Load<Texture2D>("climbPink"), 2);
            climbAnim.FrameSpeed = 0.15f;

            jumpAnim = new Animation(Content.Load<Texture2D>("alienPink_jump"), 1);
        }

        public void Update(GameTime gameTime, SoundEffect effect)
        {
            position += velocity;
           
            float i = 1;
            if(!isClimbing)
                velocity.Y += 0.15f * i;

            if (position.Y + texture.Height > 480)
            {
                position.Y = 480 - texture.Height;
                velocity.Y = 0;
                isOnGround = true;
            }

            animator.Position = position;

            if (isClimbing)
            {
                animator.Play(climbAnim);
                switch (CurrentAction)
                {
                    case Action.Up: velocity.Y = -3f; animator.Update(gameTime); break;
                    case Action.Down: velocity.Y = 3f; animator.Update(gameTime); break;
                    default: velocity.Y = 0; break;
                }
            }
            else if (!isOnGround)
            {
                animator.Play(jumpAnim);
            }
            else
            {
                animator.Play(walkAnim);
                switch (CurrentAction)
                {
                    case Action.Left: velocity.X = -3f; animator.Update(gameTime); break;
                    case Action.Right: velocity.X = 3f; animator.Update(gameTime); break;
                    default: velocity.X = 0f; animator.Play(standAnim); animator.Stop(); break;
                }
            }

        }

        public void Jump()
        {
            if (isOnGround)
            {
                velocity.Y = -6f;
                isOnGround = false;
                CurrentAction = Action.Jump;
                animator.Play(jumpAnim);
            }
            isClimbing = false;
        }

        public void MoveLeft()
        {
            CurrentAction = Action.Left;
        }

        public void MoveRight()
        {
            CurrentAction = Action.Right;
        }

        public void Stop()
        {
            CurrentAction = Action.None;
        }

        public void StopClimbing()
        {
            CurrentAction = Action.None;
        }

        public void ClimbUp()
        {
            isClimbing = true;
            CurrentAction = Action.Up;
            if (canClimb)
            {

            }
        }
        public void ClimbDown()
        {
            CurrentAction = Action.Down;
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
                case Action.Up: animator.Draw(spriteBatch); break;
                case Action.Down: animator.Draw(spriteBatch); break;
                case Action.None: animator.Draw(spriteBatch, velocity.X < 0); break;
                default: animator.Draw(spriteBatch); break;
            }
        }
    }
}
