using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Util;

namespace Game.Component
{
    public struct RoundComponent : IComponent
    {
        public RoundComponent(RoundQueue<Entity> round) {
            this.round = round;
        }
        public RoundQueue<Entity> round;
    }
}
