using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using GameServer.Entities;

namespace GameServer.Managers
{
    class EntityManager:Singleton<EntityManager>
    {
        private int idx = 0;
        public List<Entity> AllEntity = new List<Entity>();
        public Dictionary<int, List<Entity>> MapEntitites = new Dictionary<int, List<Entity>>();

        internal void AddEntity(int mapID, Entity entity)
        {
            AllEntity.Add(entity);
            //加入管理器生成唯一ID
            entity.EntityData.Id = ++this.idx;

            List<Entity> entities = null;
            if (!MapEntitites.TryGetValue(mapID,out entities))
            {
                entities = new List<Entity>();
                MapEntitites[mapID] = entities;
            }
            entities.Add(entity);
        }

        internal void RemoveEntity(int mapID, Entity entity)
        {
            AllEntity.Remove(entity);
            MapEntitites[mapID].Remove(entity);
        }
    }
}
