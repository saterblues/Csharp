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

        public static void RegistToSystem<T>(this Entity entity, params object[] args) where T : ISystem {
            if (false == World.World.GameWorld.IsContainSystem<T>()) { return; }
            entity.GetSystem<T>().RegistEntity(entity, args);
        }

        public static void UnRegistFromSystem<T>(this Entity entity) where T : ISystem {
            if (false == entity.IsInSystem<T>()) { return; }
            entity.GetSystem<T>().UnRegistEntity(entity);
        }

        public static void Destroy(this Entity entity) {
            World.World.GameWorld.DestroyEntity(entity);
        }
    }
}
