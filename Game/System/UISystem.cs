using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.System
{
    /// <summary>
    /// UI系统
    /// 负责显示UI
    /// </summary>
    public class UISystem : ISystem
    {
        public override IComponent CreateComponent(Entity entity, params object[] args)
        {
            return new IComponent();
        }

        public override void Update(World.World world)
        {
           var entities = world.GetEntites();
           foreach (var item in entities)
           {
               string name = "";
               
               if (world.IsEntityInSystem<NameSystem>(item)) {
                   name = world.GetSystem<NameSystem>().GetName(item);
               }
               string power = "";
               var powerSystem = world.GetSystem<PowerSystem>();
               if (powerSystem != null && powerSystem.ContainsEntity(item)) {
                   power = string.Format("智力:{0} 力量:{1} 敏捷:{2}",
                           powerSystem.GetIntelligence(item),
                           powerSystem.GetStrength(item),
                           powerSystem.GetAgility(item)
                       );
               }

               Console.WriteLine("玩家:{0} {1}",name,power);
           }
        }
    }
}
