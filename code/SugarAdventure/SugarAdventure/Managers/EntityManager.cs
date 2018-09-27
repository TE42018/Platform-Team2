using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugarAdventure
{
    public enum EntityTexture
    {
        Coin_bronze = 0,
        Coin_silver = 1,
        Coin_gold = 2,
        Key_green = 3,
        //Lock_green = 5,
        Slime_walk1 = 4,
        Snail_walk1 = 5,
    }
    public class EntityManager
    {
        private List<Texture2D> textures;
        public List<Texture2D> Textures
        {
            get
            {
                return textures;
            }
        }
        
        private List<IEntity> entities;
        public List<IEntity> Entities
        {
            get
            {
                return entities;
            }
        }

        public EntityManager()
        {
            entities = new List<IEntity>();
            textures = new List<Texture2D>();
            
        }

        public void LoadContent()
        {
            textures.Add(SugarGame.contentManager.Load<Texture2D>(@".\Platformer_assets\Items\coinBronze"));
            textures.Add(SugarGame.contentManager.Load<Texture2D>(@".\Platformer_assets\Items\coinSilver"));
            textures.Add(SugarGame.contentManager.Load<Texture2D>(@".\Platformer_assets\Items\coinGold"));
            textures.Add(SugarGame.contentManager.Load<Texture2D>(@".\Platformer_assets\Items\keyGreen"));
            textures.Add(SugarGame.contentManager.Load<Texture2D>(@".\Platformer_assets\Enemies\slimeWalk1"));
            textures.Add(SugarGame.contentManager.Load<Texture2D>(@".\Platformer_assets\Enemies\snailWalk1"));
        }


        public void Add(IEntity item)
        {
            entities.Add(item);
        }

        public IPickupable GetEntity(string entityName)
        {
            foreach (IEntity item in entities)
            {
                if(item.Type == entityName)
                {
                    return item as IPickupable;
                }
            }
            return null;
        }

        public void Reset()
        {
            //entities = originalEntities;
        }

        public void Update(Player player, GameTime gameTime)
        {
            for (int i = entities.Count; i > 0; i--)
            {

                if (entities[i - 1] is IPickupable item)
                {
                    if (item.Hitbox.Intersects(player.Hitbox))
                    {
                        player.Pickup(item);
                        entities.Remove(item as IEntity);
                    }
                }
                else if(entities[i - 1] is IEnemy enemy)
                {
                    enemy.Update(gameTime, player.Level);
                    if (enemy.Hitbox.Intersects(player.Hitbox))
                    {
                        player.Damage(enemy.AttackDamage);
                    }
                }


            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach(IEntity e in entities)
            {
                e.Draw(spriteBatch);
            }
        }
    }
}
