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
    }
    public class EntityManager
    {
        private LevelNumber levelNum;
        private List<Texture2D> textures;
        public List<Texture2D> Textures
        {
            get
            {
                return textures;
            }
        }

        private List<IEntity> originalEntities;
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
            originalEntities = new List<IEntity>();
            textures = new List<Texture2D>();
            
        }

        public void LoadContent()
        {
            textures.Add(SugarGame.contentManager.Load<Texture2D>(@".\Platformer_assets\Items\coinBronze"));
            textures.Add(SugarGame.contentManager.Load<Texture2D>(@".\Platformer_assets\Items\coinSilver"));
            textures.Add(SugarGame.contentManager.Load<Texture2D>(@".\Platformer_assets\Items\coinGold"));
            textures.Add(SugarGame.contentManager.Load<Texture2D>(@".\Platformer_assets\Items\keyGreen"));
            textures.Add(SugarGame.contentManager.Load<Texture2D>(@".\Platformer_assets\Enemies\slimeWalk1"));
            //textures.Add(SugarGame.contentManager.Load<Texture2D>(@".\Platformer assets (1330 assets)\Base pack (360 assets)\PNG\Tiles\lock_green"));
        }

        public void SetLevelNumber(LevelNumber num)
        {
            levelNum = num;
        }

        public void Add(IEntity item)
        {
            entities.Add(item);
            originalEntities.Add(item);
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
                IPickupable item = entities[i - 1] as IPickupable;

                if (item == null || (item as IEntity).LevelNumber == (int)levelNum)
                    continue;

                if (item.Hitbox.Intersects(player.Hitbox))
                {
                    player.Pickup(item);
                    entities.Remove(item as IEntity);
                    originalEntities.Add(item as IEntity);
                }
            }

            for (int i = entities.Count; i > 0; i--)
            {
                IInteractable interactable = entities[i - 1] as IInteractable;

                if (interactable == null || (interactable as IEntity).LevelNumber == (int)levelNum)
                    continue;

                if (interactable.Hitbox.Intersects(player.Hitbox))
                {
                    if (player.HasItem(interactable.Key))
                    {
                        entities.Remove(interactable as IEntity);
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach(IEntity e in entities)
            {
                if(e.LevelNumber == (int)levelNum)
                    e.Draw(spriteBatch);
            }
        }
    }
}
