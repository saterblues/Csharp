using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.World;
using Game.System;

namespace Game
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("-------测试开始-------");
            try
            {
                test();
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0},{1}", ex.Message, ex.StackTrace);
            }
            Console.WriteLine("-------测试结束-------");
            Console.ReadLine();
        }

        static void test() {
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

            NameSystem ns = world.GetSystem<NameSystem>();
            Entity e1 = world.CreateEntity();
            Entity e2 = world.CreateEntity();
            ns.RegistEntity(e1, "壹号玩家");
            ns.RegistEntity(e2, "二号玩家");
            PowerSystem ps = world.GetSystem<PowerSystem>();
            ps.RegistEntity(e1, 10,1,5);

            world.Update();

            e1.IsInSystem<NameSystem>();
            e1.GetSystem<NameSystem>().GetName(e1);
            

            ps.SetIntelligence(e1, 6);
            world.Update();
          
        }
    }
}
