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



        void Start()
        {
            currentVersion = Application.version;
            versionCheckURL = "https://raw.githubusercontent.com/IceWaterNotIce/Internet-Empire/main/Assets/StreamingAssets/version.json";
        }

        public IEnumerator CheckForUpdate()
        {
            UnityWebRequest www = UnityWebRequest.Get(versionCheckURL);
            var operation = www.SendWebRequest();

            float timer = 0;
            while (!operation.isDone)
            {
                timer += Time.deltaTime;
                if (timer > 10)
                {
                    Debug.Log("錯誤: 請求超時");
                    yield break;
                }
                yield return null;
            }

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log("錯誤: " + www.error);
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
                    MessageManager.Instance.CreateYesNoMessage("There is a new version available. This game needs to run on the latest version. Do you want to update now?", UpdateGame, QuitGame);
                }
                else
                {
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

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}