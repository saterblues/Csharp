using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Component
{
    /// <summary>
    /// 卡牌
    /// </summary>
    public struct CardComponent : IComponent
    {

        public CardComponent(Action<Entity,Entity> action, float cost) {
            this.action = action;
            this.manaCost = cost;
        }
        /// <summary>
        /// 施法者 和 被施法者
        /// </summary>
        public Action<Entity, Entity> action;

        /// <summary>
        /// 卡牌消耗的法力值
        /// </summary>
        public float manaCost;
    }
}
