using System;
using Game.Component;

namespace Game.System.Data
{
    /// <summary>
    /// 智力
    /// </summary>
    public class IntelligenceSystem : ISystem
    {
        public override IComponent CreateComponent(Entity entity, params object[] args)
        {
            return new SingleComponent(Convert.ToSingle(args[0]));
        }

        public float GetIntelligence(Entity entity)
        {
            return GetComponent<SingleComponent>(entity).value;
        }

        public void SetIntelligence(Entity entity, float intelligence)
        {
            IfNotFoundThrowException(entity);
            _components[entity] = new SingleComponent(intelligence);
        }
    }
}
