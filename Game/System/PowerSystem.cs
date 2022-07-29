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

        public override IComponent CreateComponent(Entity entity, params object[] args)
        {
            World.World.GameWorld.GetSystem<IntelligenceSystem>().RegistEntity(entity,args[0]);
            World.World.GameWorld.GetSystem<StrengthSystem>().RegistEntity(entity, args[1]);
            World.World.GameWorld.GetSystem<AgilitySystem>().RegistEntity(entity, args[2]);

            return new IComponent();
        }

        public float GetIntelligence(Entity entity) {
            IfNotFoundThrowException(entity);
            return World.World.GameWorld.GetSystem<IntelligenceSystem>().GetIntelligence(entity);
        }

        public float GetStrength(Entity entity) {
            IfNotFoundThrowException(entity);
            return World.World.GameWorld.GetSystem<StrengthSystem>().GetStrength(entity);
        }

        public float GetAgility(Entity entity) {
            IfNotFoundThrowException(entity);
            return World.World.GameWorld.GetSystem<AgilitySystem>().GetAgility(entity);
        }

        public void SetIntelligence(Entity entity,float intelligence) {
            IfNotFoundThrowException(entity);
            World.World.GameWorld.GetSystem<IntelligenceSystem>().SetIntelligence(entity, intelligence);
        }

        public void SetStrength(Entity entity, float strength) {
            IfNotFoundThrowException(entity);
            World.World.GameWorld.GetSystem<StrengthSystem>().SetStrength(entity, strength);
        }

        public void SetAgility(Entity entity, float agility) {
            IfNotFoundThrowException(entity);
            World.World.GameWorld.GetSystem<AgilitySystem>().SetAgility(entity, agility);
        }

        public override void UnRegistEntity(Entity entity)
        {
            base.UnRegistEntity(entity);
            World.World.GameWorld.GetSystem<IntelligenceSystem>().UnRegistEntity(entity);
            World.World.GameWorld.GetSystem<StrengthSystem>().UnRegistEntity(entity);
            World.World.GameWorld.GetSystem<AgilitySystem>().UnRegistEntity(entity);
        }
    }
}
