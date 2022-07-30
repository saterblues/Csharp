using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.System;

namespace Game
{
    public static partial class EntityExtension
    {
        public static bool IsInSystem<T>(this Entity entity) where T : ISystem
        {
            if (false == World.World.GameWorld.IsContainSystem<T>())
            {
                return false;
            }
            T t = World.World.GameWorld.GetSystem<T>();
            return t.ContainsEntity(entity);
        }

        public static T GetSystem<T>(this Entity entity) where T : ISystem {
            return World.World.GameWorld.GetSystem<T>();
        }
    }
}
