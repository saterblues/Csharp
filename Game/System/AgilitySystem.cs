using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.System
{
    
    public class AgilitySystem : ISystem
    {
        public class AgilityComponent : IComponent
        {
            public AgilityComponent(float agility)
            {
                Agility = agility;
            }
            public float Agility {set; get; }
        }

        public override IComponent CreateComponent(Entity entity, params object[] args)
        {
            return new AgilityComponent(Convert.ToSingle(args[0]));
        }

        public float GetAgility(Entity entity)
        {
            IfNotFoundThrowException(entity);
            return GetComponent<AgilityComponent>(entity).Agility;
        }

        public void SetAgility(Entity entity, float agility)
        {
            IfNotFoundThrowException(entity);
            GetComponent<AgilityComponent>(entity).Agility = agility;
        }
    }
}
