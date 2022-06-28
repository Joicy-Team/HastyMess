using Unity.Entities;
using UnityEngine;

namespace HastyMess.Scripts.Data
{
    [GenerateAuthoringComponent]
    public class ChildEntityComponent : IComponentData
    {
        public GameObject Parent;
    }
}