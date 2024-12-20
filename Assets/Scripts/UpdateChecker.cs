using UnityEngine;
using UnityEngine.Networking;
using System.Collections;


namespace InternetEmpire
{
    public class UpdateChecker : MonoBehaviour
    {
        private string versionCheckURL;
        private string currentVersion;

        public VersionInfo versionInfo;

        [SerializeField] private GameObject networkpanel;

        [SerializeField] private GameObject updatepanel;

        void Start()
        {
            currentVersion = Application.version;
            versionCheckURL = "https://raw.githubusercontent.com/IceWaterNotIce/Internet-Empire/main/Assets/StreamingAssets/version.json";
            StartCoroutine(CheckForUpdate());
        }

        private IEnumerator CheckForUpdate()
        {
            UnityWebRequest www = UnityWebRequest.Get(versionCheckURL);
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log("錯誤: " + www.error);
                networkpanel.SetActive(true);
                
            }
            else
            {
                // 解析 JSON 響應
                var jsonResponse = www.downloadHandler.text;
                versionInfo = JsonUtility.FromJson<VersionInfo>(jsonResponse);

                if (versionInfo.latestVersion != currentVersion)
                {
                    Debug.Log("有新版本可用: " + versionInfo.latestVersion);
                    // 可以提示用戶下載新版本
                    updatepanel.SetActive(true);

                }
                else
                {
                    Debug.Log("您正在運行最新版本.");
                    InitializeSceneManager initializeSceneManager = GameObject.FindFirstObjectByType<InitializeSceneManager>();
                    initializeSceneManager.isVersionChecked = true;
                    initializeSceneManager.Check();
                }
            }

        }

        [System.Serializable]
        public class VersionInfo
        {
            public string latestVersion;
            public string downloadURL;
        }

        public void UpdateGame()
        {
            Application.OpenURL(versionInfo.downloadURL);
            Application.Quit();
        }

        
    }
}