using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.System
{
    /// <summary>
    /// 力量
    /// </summary>
    public class StrengthSystem : ISystem
    {
        private class StrengthComponent : IComponent
        {
            public StrengthComponent(float strength)
            {
                Strength = strength;
            }
            public float Strength { set; get; }
        }

        public override IComponent CreateComponent(Entity entity, params object[] args)
        {
            return new StrengthComponent(Convert.ToSingle(args[0]));
        }

        public float GetStrength(Entity entity)
        {
            IfNotFoundThrowException(entity);
            return GetComponent<StrengthComponent>(entity).Strength;
        }

        public void SetStrength(Entity entity, float strength)
        {
            IfNotFoundThrowException(entity);
            GetComponent<StrengthComponent>(entity).Strength = strength;
        }
    }
}
