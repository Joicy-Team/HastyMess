using Unity.Entities;

namespace HastyMess.Scripts.Data
{
    [GenerateAuthoringComponent]
    public struct BillComponent : IComponentData
    {
        public float MoveSpeed;
    }
}