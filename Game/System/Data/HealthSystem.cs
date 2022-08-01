using System;
using Game.Component;

namespace Game.System.Data
{
    /// <summary>
    /// 生命值
    /// </summary>
    public class HealthSystem : ISystem
    {
        public override IComponent CreateComponent(Entity entity,params object[] args)
        {
            return new SingleComponent(Convert.ToSingle(args[0]));
        }

        public float GetHealth(Entity entity) {
            return GetComponent<SingleComponent>(entity).value;
        }

        public void SetHealth(Entity entity,float health) {
            IfNotFoundThrowException(entity);
            _components[entity] = new SingleComponent(health);
        }
    }
}
