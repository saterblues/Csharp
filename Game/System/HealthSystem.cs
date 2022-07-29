using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.System
{
    /// <summary>
    /// 生命值
    /// </summary>
    public class HealthSystem : ISystem
    {
        private class HealthComponent : IComponent {
            public HealthComponent(float health)
            {
                Health = health;
            }
            public float Health { set; get; }
        }

        public override IComponent CreateComponent(Entity entity,params object[] args)
        {
            return new HealthComponent(Convert.ToSingle(args[0]));
        }

        public float GetHealth(Entity entity) {
            IfNotFoundThrowException(entity);
            return GetComponent<HealthComponent>(entity).Health;
        }

        public void SetHealth(Entity entity,float health) {
            IfNotFoundThrowException(entity);
            GetComponent<HealthComponent>(entity).Health = health;
        }
    }
}
