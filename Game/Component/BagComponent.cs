using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Component
{
    public struct BagComponent : IComponent
    {
        public BagComponent(List<Entity> data) { this.data = data; }
        public List<Entity> data;
    }
}
