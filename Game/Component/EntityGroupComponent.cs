using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Component
{
    /// <summary>
    /// Entity集合组件
    /// 用来表示同等地位的Entity集合
    /// </summary>
    public struct EntityGroupComponent : IComponent
    {
        public EntityGroupComponent(List<Entity> entities) { this.entities = entities; }
        public List<Entity> entities;
    }
}
