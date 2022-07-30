using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.System
{
    /// <summary>
    /// 系统基类
    /// 管理所有注册的Entity,生成对应的组件并放入数组
    /// </summary>
    public abstract class ISystem{
        public struct RegistInfo {
            public Entity entity;
            public object[] args;

            public RegistInfo(Entity entity, object[] args)
            {
                this.entity = entity;
                this.args = args;
            }
        }

        public event EventHandler<RegistInfo> OnRegisting = (sender, registInfo) => { };
        public event EventHandler<RegistInfo> OnRegisted = (sender, registInfo) => { };
        public event EventHandler<Entity> OnUnRegisting = (sender, entity) => { };
        public event EventHandler<Entity> OnUnRegisted = (sender, entity) => { };

        protected Dictionary<Entity, IComponent> _components = new Dictionary<Entity, IComponent>();

        protected virtual void IfNotFoundThrowException(Entity entity)
        {
            if (false == ContainsEntity(entity))
            {
                string msg = string.Format("Entity Hash:{0} Uid:{1} Not In System:{2}",
                    entity.GetHashCode(),
                    entity.Uid,
                    GetType().FullName);

                throw new KeyNotFoundException(msg);
            }
        }

        public virtual IComponent GetComponent(Entity entity)
        {
            IComponent com;
            _components.TryGetValue(entity, out com);
            return com;
        }

        public virtual T GetComponent<T>(Entity entity) where T : IComponent
        {
            if (GetComponent(entity) == null) { return null; }
            return GetComponent(entity) as T;
        }

        public virtual IEnumerable<Entity> GetEntities() {
            return _components.Keys;
        }

        public virtual void RegistEntity(Entity entity, params object[] args) {

            RegistInfo info = new RegistInfo(entity, args);
            OnRegisting.Invoke(this, info);

            IComponent com = CreateComponent(entity, args);
            _components.Add(entity, com);

            OnRegisted.Invoke(this, info);
        }

        public virtual void UnRegistEntity(Entity entity)
        {
            OnUnRegisting.Invoke(this, entity);

            _components.Remove(entity);

            OnUnRegisted.Invoke(this, entity);
        }
  
        public virtual bool ContainsEntity(Entity entity) {
            return _components.ContainsKey(entity);
        }

        public virtual void Update() { }

        public virtual void SetData(Entity entity, IComponent component){ }

        public abstract IComponent CreateComponent(Entity entity,params object[] args);
    }
}
