using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.System
{
    /// <summary>
    /// 智力
    /// </summary>
    public class IntelligenceSystem : ISystem
    {
        private class IntelligenceComponent : IComponent
        {
            public IntelligenceComponent(float intelligence)
            {
                Intelligence = intelligence;
            }
            public float Intelligence { set; get; }
        }

        public override IComponent CreateComponent(Entity entity, params object[] args)
        {
            return new IntelligenceComponent(Convert.ToSingle(args[0]));
        }

        public float GetIntelligence(Entity entity)
        {
            IfNotFoundThrowException(entity);
            return GetComponent<IntelligenceComponent>(entity).Intelligence;
        }

        public void SetIntelligence(Entity entity, float intelligence)
        {
            IfNotFoundThrowException(entity);
            GetComponent<IntelligenceComponent>(entity).Intelligence = intelligence;
        }
    }
}
