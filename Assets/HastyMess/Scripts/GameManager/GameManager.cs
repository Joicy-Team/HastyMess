using UnityEngine;

namespace HastyMess.Scripts
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        
        public GameData data;
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(this);
            else
                Instance = this;
            
            Cursor.SetCursor(data.defaultCursor, Vector2.zero, CursorMode.Auto);
        }
    }
}