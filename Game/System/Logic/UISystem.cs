using System;
using Game.System.Data;
using Game.Component;
using Game.AttributeExtension;

namespace Game.System.Logic
{
    /// <summary>
    /// UI系统
    /// 负责显示UI
    /// </summary>
    /// 
    public class UISystem : ISystem
    {
        public override IComponent CreateComponent(Entity entity, params object[] args)
        {
            return new BaseComponent();
        }

        public override void Update()
        {
           var entities = World.World.GameWorld.GetEntites();
           foreach (var item in entities)
           {
                if (!item.IsInSystem<PlayerSystem>()) { continue; }
                Console.WriteLine("玩家:{0} {1}",item,GetPowerInfo(item));
           }
        }

        private string GetPowerInfo(Entity entity) {
            if (entity.IsInSystem<PowerSystem>())
            {
                var powerSystem = entity.GetSystem<PowerSystem>();
                string power = string.Format("智力:{0} 力量:{1} 敏捷:{2}",
                        powerSystem.GetIntelligence(entity),
                        powerSystem.GetStrength(entity),
                        powerSystem.GetAgility(entity)
                    );
                return power;
            }
            return "";
        }
    }
}
