using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinesweeperExitExam.GameObjects
{
    public class GameObject
    {
        public Dictionary<string, SoundEffectInstance> SoundEffects;
        protected Texture2D _texture;
        public bool isHovered;
        public Vector2 Position;
        public float Rotation;
        public Vector2 Scale;

        public Vector2 Velocity;

        public bool IsActive;
        public Rectangle Viewport;

        


        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, Viewport.Width, Viewport.Height);
            }
        }

        public GameObject(Texture2D texture)
        {
            isHovered = false;
            _texture = texture;
            Position = Vector2.Zero;
            Scale = Vector2.One;
            Rotation = 0f;
            IsActive = true;

        }


        public virtual void Update(GameTime gameTime, List<GameObject> gameObjects)
        {
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
        }

        public virtual void Reset()
        {

        }
    }
}
