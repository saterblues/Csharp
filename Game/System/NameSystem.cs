using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Game.System
{
   /// <summary>
   /// 名称系统: 
   /// Name: 名字
   /// </summary>
    public class NameSystem : ISystem
    {
        private class NameComponent : IComponent
        {
            public string Name { get; set; }
        }

        public override IComponent CreateComponent(object obj)
        {
            NameComponent com = new NameComponent();
            com.Name = obj.ToString();
            return com;
        }

        public string GetName(Entity entity) {
            NameComponent c = GetComponent<NameComponent>(entity);
            if (null == c) { return null; }
            return c.Name;
        }
    }
}
