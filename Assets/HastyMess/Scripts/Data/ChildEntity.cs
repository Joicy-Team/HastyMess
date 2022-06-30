using Unity.Entities;
using UnityEngine;

namespace HastyMess.Scripts.Data
{
    [GenerateAuthoringComponent]
    public class ChildEntity : IComponentData
    {
        public GameObject Parent;
    }
}