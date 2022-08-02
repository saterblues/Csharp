

namespace Game.Component
{
    public struct SingleComponent : IComponent
    {
        public SingleComponent(float value) { this.value = value; }
        public float value;
    }
}
