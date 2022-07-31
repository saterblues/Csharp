using System.Collections.Generic;

using Game.Component;


namespace Game.System.Data
{
    public class BagSystem : ISystem
    {
        public override IComponent CreateComponent(Entity entity, params object[] args)
        {
            BagComponent bag = new BagComponent(new List<Entity>());
            if (args != null) {
                foreach (var item in args)
                {
                    bag.data.Add((Entity)item);
                }
            }
            return bag;
        }
    }
}
