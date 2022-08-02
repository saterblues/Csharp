using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.System;
using Game.AttributeExtension;

namespace Game.World
{
    /// <summary>
    /// 万物都由世界掌管
    /// </summary>
    public class World
    {
        public static World GameWorld = new World();
        private World() { }

        protected List<ISystem> _systems = new List<ISystem>();
        protected HashSet<Entity> _entities = new HashSet<Entity>();

        public event EventHandler<Entity> OnEntityCreated = (sender, entity) => { };
        public event EventHandler<Entity> OnEntityDestroy = (sender, entity) => {  };
        public event EventHandler<ISystem> OnSystemRegist = (sender, system) => {  };
        public event EventHandler<ISystem> OnSystemUnRegist = (sender, system) => {  };

        /// <summary>
        ///查找System的RequireSystemAttribute,将其需要的System进行注册
        /// </summary>
        /// <param name="t"></param>
        private void RegistRequireSystem(Type system) {
            var attrs = system.GetCustomAttributes(typeof(RequireSystemAttribute), true);
            foreach (var item in attrs)
            {
                RequireSystemAttribute require = (RequireSystemAttribute)item;
                require.RegistRequire(this);
            }
        }

        public void Update() {
            foreach (var item in _systems)
            {
                item.Update();
            }
        }

        public IEnumerable<ISystem> GetSystems()
        {
            return _systems;
        }

        public IEnumerable<Entity> GetEntites()
        {
            return _entities;
        }

        public bool IsContainSystem<T>() where T : ISystem {
            return null != GetSystem<T>();
        }

        public World RegistSystem<T>() 
            where T : ISystem , new()
        {
            if (IsContainSystem<T>()) {return this; }
            T t = new T();
            _systems.Add(t);

            OnSystemRegist(this, t);

            RegistRequireSystem(t.GetType());

            return this;
        }

        public World UnRegistSystem<T>() where T : ISystem {
            if (!IsContainSystem<T>()) { return this; }
            T t = GetSystem<T>();
            _systems.Remove(t);

            OnSystemUnRegist(this, t);
            return this;
        }

        public bool IsEntityInSystem<T>(Entity entity) where T : ISystem {
            T t = GetSystem<T>();
            if(t == null) { return false; }
            return t.ContainsEntity(entity);
        }

        public T GetSystem<T>() where T : ISystem 
        {
            foreach (var item in _systems)
            {
                if (item.GetType() == typeof(T)) {
                    return item as T;
                }
            }
            return default(T);
        }

        public Entity CreateEntity()
        {
            Entity entity = new Entity();
            _entities.Add(entity);

            OnEntityCreated.Invoke(this, entity);

            return entity;
        }

        public void DestroyEntity(Entity entity)
        {
            foreach (var item in _systems)
            {
                item.UnRegistEntity(entity);
            }
            _entities.Remove(entity);

            OnEntityDestroy.Invoke(this, entity);
        }
    }
}
