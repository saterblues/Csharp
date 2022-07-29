using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.System
{
    /// <summary>
    /// 魔法值
    /// </summary>
    public class ManaSystem : ISystem
    {
        private class ManaComponent : IComponent
        {
            public ManaComponent(float mana)
            {
                Mana = mana;
            }
            public float Mana { set; get; }
        }

        public override IComponent CreateComponent(Entity entity, params object[] args)
        {
            return new ManaComponent(Convert.ToSingle(args[0]));
        }

        public float GetMana(Entity entity)
        {
            IfNotFoundThrowException(entity);
            return GetComponent<ManaComponent>(entity).Mana;
        }

        public void SetMana(Entity entity, float mana)
        {
            IfNotFoundThrowException(entity);
            GetComponent<ManaComponent>(entity).Mana = mana;
        }
    }
}
