using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Game.System;

namespace Game.AttributeExtension
{
    /// <summary>
    /// 用于表示依赖的系统,World在注册有此Attibute类时,会将其依赖的系统也一并进行注册
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class RequireSystemAttribute : Attribute
    {
        
        public RequireSystemAttribute(Type type) {

            if (false == type.IsSubclassOf(typeof(ISystem)))
            {
                throw new NotSupportedException("RequireSystemAttribute Not Support:" + type.FullName);
            }
            GameSystem = type;
        }

        public Type GameSystem
        {
            set;get;
        }

        public void RegistRequire(World.World world) {
            Type type = world.GetType();
            var registfnc = type.GetMethod("RegistSystem");
            var gMethod =  registfnc.MakeGenericMethod(GameSystem);
            gMethod.Invoke(world, null);
        }
    }
}
