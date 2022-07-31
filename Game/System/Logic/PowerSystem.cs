using Game.System.Data;
using Game.Component;

namespace Game.System.Logic
{
    /// <summary>
    /// 基础属性系统:
    /// 力量 
    /// 敏捷 
    /// 智力
    /// </summary>
    public partial class PowerSystem : ISystem
    {
        public PowerSystem():base() {
            OnRegisted += (sender, info ) => {
                World.World.GameWorld.GetSystem<IntelligenceSystem>().RegistEntity(info.entity, info.args[0]);
                World.World.GameWorld.GetSystem<StrengthSystem>().RegistEntity(info.entity, info.args[1]);
                World.World.GameWorld.GetSystem<AgilitySystem>().RegistEntity(info.entity, info.args[2]);
            };

            OnUnRegisted += (sender,entity) => {
                entity.GetSystem<IntelligenceSystem>().UnRegistEntity(entity);
                entity.GetSystem<StrengthSystem>().UnRegistEntity(entity);
                entity.GetSystem<AgilitySystem>().UnRegistEntity(entity);
            };
        }

        public override IComponent CreateComponent(Entity entity, params object[] args)
        {
            return new Component.BaseComponent();
        }

        public float GetIntelligence(Entity entity) {
            IfNotFoundThrowException(entity);
            return entity.GetSystem<IntelligenceSystem>().GetIntelligence(entity);
        }

        public float GetStrength(Entity entity) {
            IfNotFoundThrowException(entity);
            return entity.GetSystem<StrengthSystem>().GetStrength(entity);
        }

        public float GetAgility(Entity entity) {
            IfNotFoundThrowException(entity);
            return entity.GetSystem<AgilitySystem>().GetAgility(entity);
        }

        public void SetIntelligence(Entity entity,float intelligence) {
            IfNotFoundThrowException(entity);
            entity.GetSystem<IntelligenceSystem>().SetIntelligence(entity, intelligence);
        }

        public void SetStrength(Entity entity, float strength) {
            IfNotFoundThrowException(entity);
            entity.GetSystem<StrengthSystem>().SetStrength(entity, strength);
        }

        public void SetAgility(Entity entity, float agility) {
            IfNotFoundThrowException(entity);
            entity.GetSystem<AgilitySystem>().SetAgility(entity, agility);
        }
      
    }
}
