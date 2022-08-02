using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Component;
using Game.Util;

namespace Game.System.Data
{
    /// <summary>
    /// 新成员默认添加在末尾
    /// </summary>
    public class RoundSystem : ISystem
    {
        public override Component.IComponent CreateComponent(Entity entity, params object[] args)
        {
            RoundComponent component = new RoundComponent(new RoundQueue<Entity>());
            if (args != null) {
                foreach (var item in args) { component.round.Add((Entity)item); }
            }
            return component;
        }

        public Entity Pop(Entity roundId) {
            return GetComponent<RoundComponent>(roundId).round.Pop();
        }

        public Entity PeekCurrent(Entity roundId)
        {
            return GetComponent<RoundComponent>(roundId).round.PeekCurrent(); 
        }

        public IEnumerable<Entity> PeekNextRange(Entity roundId,int count)
        {
            return GetComponent<RoundComponent>(roundId).round.PeekNextRange(count);
        }

        public Entity PeekNext(Entity roundId)
        {
            return GetComponent<RoundComponent>(roundId).round.PeekNext();
        }

        public void Remove(Entity roundId,Entity entity)
        {
            GetComponent<RoundComponent>(roundId).round.Remove(entity);
        }

        public void Add(Entity roundId,Entity entity)
        {
            GetComponent<RoundComponent>(roundId).round.Add(entity);
        }
    }
}
