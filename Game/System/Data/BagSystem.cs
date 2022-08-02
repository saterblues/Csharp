using System;
using System.Collections.Generic;

using Game.Component;


namespace Game.System.Data
{
    public class BagSystem : ISystem
    {
        public override IComponent CreateComponent(Entity entity, params object[] args)
        {
            EntityGroupComponent bag = new EntityGroupComponent(new List<Entity>());
            if (args != null) {
                foreach (var item in args)
                {
                    bag.entities.Add((Entity)item);
                }
            }
            return bag;
        }

        public virtual IEnumerable<Entity> GetBagData(Entity bagId)
        {
            return GetComponent<EntityGroupComponent>(bagId).entities;
        }

        public virtual void AddBagData(Entity bagId,Entity thingId) {
            GetComponent<EntityGroupComponent>(bagId).entities.Add(thingId);
        }

        public virtual void RemoveBagData(Entity bagId, Entity thingId)
        {
            GetComponent<EntityGroupComponent>(bagId).entities.Remove(thingId);
        }

        public virtual int GetBagCount(Entity bagId)
        {
            return GetComponent<EntityGroupComponent>(bagId).entities.Count;
        }

        public virtual Entity PopFirst(Entity bagId) {
            if (GetBagCount(bagId) == 0) { return null; }
            Entity first = GetComponent<EntityGroupComponent>(bagId).entities[0];
            RemoveBagData(bagId, first);
            return first;
        }

        public virtual Entity PopLast(Entity bagId) {
            if (GetBagCount(bagId) == 0) { return null; }
            Entity last = GetComponent<EntityGroupComponent>(bagId).entities[GetBagCount(bagId) - 1];
            RemoveBagData(bagId, last);
            return last;
        }

        public virtual IEnumerable<Entity> PopLastRange(Entity bagId, int count) {
            if (GetBagCount(bagId) == 0) { return null; }
            if(GetBagCount(bagId) <= count) { return GetBagData(bagId);}
            EntityGroupComponent things = (EntityGroupComponent)_components[bagId];
            IEnumerable<Entity> result = things.entities.GetRange(things.entities.Count - count, count);
            things.entities.RemoveRange(things.entities.Count - count, count);
            return result;
        }
    }
}
