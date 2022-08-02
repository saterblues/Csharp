using Game.Component;

namespace Game.System.Data
{
   /// <summary>
   /// 名称系统: 
   /// Name: 名字
   /// </summary>
    public class NameSystem : ISystem
    {
        public class NameComponent : IComponent
        {
            public string Name { get; set; }
        }

        public override IComponent CreateComponent(Entity entity, params object[] args)
        {
            NameComponent com = new NameComponent();
            com.Name = args[0].ToString();
            return com;
        }

        public string GetName(Entity entity) {
            NameComponent c = GetComponent<NameComponent>(entity);
            if (null == c) { return null; }
            return c.Name;
        }
    }
}
