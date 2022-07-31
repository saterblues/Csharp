using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
