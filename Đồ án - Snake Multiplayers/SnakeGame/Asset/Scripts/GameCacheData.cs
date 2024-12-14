using UnityEngine;

namespace Assets.Scripts
{
    public class GameCacheData : MonoBehaviour
    {
        public bool isRetry { get; set; } = false;
        public string retryUsername { get; set; }

        private static GameCacheData instance;
        public static GameCacheData Instance
        {
            get
            {
                return instance;
            }
        }

        private void Awake()
        {
            if (Instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
