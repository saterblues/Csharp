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



        public override void Update()
        {
           var entities = World.World.GameWorld.GetEntites();
           foreach (var item in entities)
           {
               string name = "";
               
               if (World.World.GameWorld.IsEntityInSystem<NameSystem>(item)) {
                   name = World.World.GameWorld.GetSystem<NameSystem>().GetName(item);
               }
               string power = "";
               var powerSystem = World.World.GameWorld.GetSystem<PowerSystem>();
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
