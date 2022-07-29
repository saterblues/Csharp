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
        protected List<ISystem> _systems = new List<ISystem>() {  
            new NameSystem(),
            new PowerSystem(),
            new UISystem()
        };
        protected HashSet<Entity> _entities = new HashSet<Entity>();

        public event EventHandler<Entity> OnEntityCreated = (a, b) => { };
        public event EventHandler<Entity> OnEntityDestroy = (a, b) => { };

        public virtual void Update() {
            foreach (var item in _systems)
            {
                item.Update(this);
            }
        }

        public virtual IEnumerable<ISystem> GetSystems()
        {
            return _systems;
        }

        public virtual IEnumerable<Entity> GetEntites()
        {
            return _entities;
        }

        public virtual T GetSystem<T>() where T : ISystem 
        {
            foreach (var item in _systems)
            {
                if (item.GetType() == typeof(T)) {
                    return item as T;
                }
            }
            return default(T);
        }

        public virtual Entity CreateEntity()
        {
            Entity entity = new Entity();
            _entities.Add(entity);
            OnEntityCreated.Invoke(this, entity);

            return entity;
        }

        public virtual void DestroyEntity(Entity entity)
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
