using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Component;

namespace Game.System.Data
{
    public class CardSystem : ISystem
    {
        public override IComponent CreateComponent(Entity entity, params object[] args)
        {
            CardComponent card = new CardComponent((Action<Entity,Entity>)args[0],(float)args[1]);
            return card;
        }
    }
}
