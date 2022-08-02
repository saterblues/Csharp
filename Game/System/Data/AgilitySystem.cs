using System;
using Game.Component;

namespace Game.System.Data
{
    
    public class AgilitySystem : ISystem
    {
        public override IComponent CreateComponent(Entity entity, params object[] args)
        {
            return new SingleComponent(Convert.ToSingle(args[0]));
        }

        public float GetAgility(Entity entity)
        {
            IfNotFoundThrowException(entity);
            return GetComponent<SingleComponent>(entity).value;
        }

        public void SetAgility(Entity entity, float agility)
        {
            IfNotFoundThrowException(entity);
            _components[entity] =  new SingleComponent(agility);
        }
    }
}
