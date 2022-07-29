using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.System
{

    /// <summary>
    /// 基础属性系统:
    /// 力量 
    /// 敏捷 
    /// 智力
    /// </summary>
    public partial class PowerSystem : ISystem
    {

        private class PowerComponent : IComponent
        {
            public PowerComponent(Number Intelligence, Number Strength, Number Agility)
            {
                this.Intelligence = Intelligence;
                this.Strength = Strength;
                this.Agility = Agility;
            }
            public Number Intelligence { get; set; }
            public Number Strength { set; get; }
            public Number Agility { set; get; }
        }

        public override IComponent CreateComponent(object obj)
        {
            dynamic power = obj;
            PowerComponent com = new PowerComponent(power.Intelligence, power.Strength, power.Agility);
            return com;
        }

        public Number GetIntelligence(Entity entity) {
            IfNotFoundThrowException(entity);
            return GetComponent<PowerComponent>(entity).Intelligence;
        }

        public Number GetStrength(Entity entity) {
            IfNotFoundThrowException(entity);
            return GetComponent<PowerComponent>(entity).Strength;
        }

        public Number GetAgility(Entity entity) {
            IfNotFoundThrowException(entity);
            return GetComponent<PowerComponent>(entity).Agility;
        }

        public void SetIntelligence(Entity entity,float intelligence) {
            IfNotFoundThrowException(entity);
            GetComponent<PowerComponent>(entity).Intelligence = intelligence;
        }

        public void SetStrength(Entity entity, float strength) {
            IfNotFoundThrowException(entity);
            GetComponent<PowerComponent>(entity).Intelligence = strength;
        }

        public void SetAgility(Entity entity, float agility) {
            IfNotFoundThrowException(entity);
            GetComponent<PowerComponent>(entity).Intelligence = agility;
        }
    }
}
