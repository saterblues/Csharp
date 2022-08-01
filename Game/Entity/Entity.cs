using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.System.Data;

namespace Game
{
    public class Entity
    {
        public Entity() {
            _guid = Guid.NewGuid();
        }
        private Guid _guid;
        public Guid Uid
        {
            set {
                _guid = value;
            }
            get {
                return _guid;
            }
        }

        public override string ToString()
        {
            if (this.IsInSystem<NameSystem>()) {
                return this.GetSystem<NameSystem>().GetName(this);
            }
            return base.ToString();
        }
    }
}
