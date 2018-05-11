using Game_Engine.Components;
using Game_Engine.Entities;
using Game_Engine.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Engine.Helpers
{
    /// <summary>
    /// This class contains methods for getting entities of certain types 
    /// </summary>
    public class EntityHelper
    {
        public static Entity GetPlayer(string playertype)
        {
            Entity result = null;
            var playerEntities = ComponentManager.Instance.GetDictionary<PlayerComponent>().Keys;
            foreach (var player in playerEntities)
            {
                if (player.EntityTypeName.Equals(playertype)) result = player;
            }
            return result;
        }
    }
}
