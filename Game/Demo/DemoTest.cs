using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.System.Data;
using Game.System.Logic;
using Game.Component;

namespace Game.Demo
{
    public class DemoTest
    {
        public void Test() {
            World.World world = World.World.GameWorld;
            world.RegistSystem<NameSystem>();
            world.RegistSystem<HealthSystem>();
            world.RegistSystem<ManaSystem>();
            world.RegistSystem<IntelligenceSystem>();
            world.RegistSystem<StrengthSystem>();
            world.RegistSystem<AgilitySystem>();
            world.RegistSystem<PowerSystem>();
            world.RegistSystem<UISystem>();
            world.UnRegistSystem<ManaSystem>();


            Entity e1 = world.CreateEntity();
            Entity e2 = world.CreateEntity();

            e1.RegistToSystem<NameSystem>("壹号玩家");
            e2.RegistToSystem<NameSystem>("二号玩家");
            e1.RegistToSystem<PowerSystem>(10, 1, 5);

            world.Update();

            e1.GetSystem<PowerSystem>().SetAgility(e1, 888);
            world.Update();

            e2.Destroy();
            world.Update();

            var x1 = e1.GetSystem<AgilitySystem>().GetComponent<SingleComponent>(e1).value;
            Console.WriteLine(x1);
        }
    }
}
