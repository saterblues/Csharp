using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Component;

namespace Game.System.Data
{
    /// <summary>
    /// 魔法值
    /// </summary>
    public class ManaSystem : ISystem
    {
        public override IComponent CreateComponent(Entity entity, params object[] args)
        {
            return new SingleComponent(Convert.ToSingle(args[0]));
        }

        public float GetMana(Entity entity)
        {
            return GetComponent<SingleComponent>(entity).value;
        }

        public void SetMana(Entity entity, float mana)
        {
            IfNotFoundThrowException(entity);
            _components[entity] =  new SingleComponent(mana);
        }
    }
}
