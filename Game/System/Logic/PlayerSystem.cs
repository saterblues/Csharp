using Game.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.System.Logic
{
    public class PlayerSystem : ISystem
    {
        public override IComponent CreateComponent(Entity entity, params object[] args)
        {
            return new BaseComponent();
        }
    }
}
