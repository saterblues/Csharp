using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.System;

namespace Game.World
{
    /// <summary>
    /// 万物都由世界掌管
    /// </summary>
    public class World
    {
        public static World GameWorld = new World();
        private World() { }

        protected List<ISystem> _systems = new List<ISystem>() {  
            new NameSystem(),
            new HealthSystem(),
            new ManaSystem(),
            new IntelligenceSystem(),
            new StrengthSystem(),
            new AgilitySystem(),
            new PowerSystem(),
            new UISystem()
        };

        protected HashSet<Entity> _entities = new HashSet<Entity>();

        public event EventHandler<Entity> OnEntityCreated = (a, b) => { };
        public event EventHandler<Entity> OnEntityDestroy = (a, b) => { };

        public void Update() {
            foreach (var item in _systems)
            {
                item.Update(this);
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
