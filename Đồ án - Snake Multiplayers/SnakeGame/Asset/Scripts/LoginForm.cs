using TMPro;
using UnityEngine;

namespace Assets.Scripts
{
    public class LoginForm : MonoBehaviour
    {
        [SerializeField] private TMP_InputField usernameText;
        [SerializeField] private GameObject listRanking;

        private void Start()
        {
            listRanking.SetActive(false);
        }

        private void Update()
        {
            if (GameCacheData.Instance.isRetry && Client.Instance != null && Client.Instance.connected)
            {
                Debug.Log("Retry");
                GameCacheData.Instance.isRetry = false;
                listRanking.SetActive(true);
                Client.Instance.Login(GameCacheData.Instance.retryUsername);
                gameObject.SetActive(false);
            }
        }

        public void Login()
        {
            string username = usernameText.text.Trim();
            if(string.IsNullOrEmpty(username))
            {
                return;
            }
            listRanking.SetActive(true);
            Client.Instance.Login(username);
            gameObject.SetActive(false);
            Debug.Log("Login");
        }
    }
}
