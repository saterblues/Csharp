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
    public abstract partial class ISystem{
        protected Dictionary<Entity, IComponent> _components = new Dictionary<Entity, IComponent>();

        protected virtual Dictionary<Entity, IComponent>.Enumerator GetEnumerator() {
            return _components.GetEnumerator();
        }
        protected virtual IComponent GetComponent(Entity entity)
        {
            IComponent com = null;
            _components.TryGetValue(entity, out com);
            return com;
        }

        protected virtual T GetComponent<T>(Entity entity) where T : IComponent
        {
            if (GetComponent(entity) == null) { return null; }
            return GetComponent(entity) as T;
        }

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

        public virtual void RegistEntity(Entity entity,object obj) {
            IComponent com = CreateComponent(obj);
            _components.Add(entity, com);
        }

        public virtual void UnRegistEntity(Entity entity)
        {
            _components.Remove(entity);
        }
  
        public virtual bool ContainsEntity(Entity entity) {
            return _components.ContainsKey(entity);
        }

        public virtual void Update(World.World world) { }

        public abstract IComponent CreateComponent(object obj);
    }
}
