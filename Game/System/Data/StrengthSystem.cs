using System;
using Game.Component;

namespace Game.System.Data
{
    /// <summary>
    /// 力量
    /// </summary>
    public class StrengthSystem : ISystem
    {
        

        public override IComponent CreateComponent(Entity entity, params object[] args)
        {
            return new SingleComponent(Convert.ToSingle(args[0]));
        }

        public float GetStrength(Entity entity)
        {
            IfNotFoundThrowException(entity);
            return GetComponent<SingleComponent>(entity).value;
        }

        public void SetStrength(Entity entity, float strength)
        {
            IfNotFoundThrowException(entity);
            _components[entity] = new SingleComponent(strength);
        }
    }
}
