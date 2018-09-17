using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugarAdventure
{
    public class EntityManager
    {
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
        }

        public void LoadContent()
        {

        }

        public void Add(IEntity item)
        {
            entities.Add(item);
        }
    }
}
