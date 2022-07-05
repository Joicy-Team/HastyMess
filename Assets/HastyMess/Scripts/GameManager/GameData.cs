using UnityEngine;

namespace HastyMess.Scripts
{
    [CreateAssetMenu(fileName = "GameData", menuName = "Data/Game")]
    public class GameData : ScriptableObject
    {
        public Texture2D defaultCursor;
        public Texture2D hoverCursor;
    }
}