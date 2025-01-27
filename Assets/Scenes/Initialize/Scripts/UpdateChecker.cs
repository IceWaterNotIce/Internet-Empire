using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Windows;
using System.IO;


namespace InternetEmpire
{
    public class UpdateChecker : MonoBehaviour
    {
        private string versionCheckURL;
        private string currentVersion;
        public VersionConfig.VersionInfo versionInfo;



        void Start()
        {
            Debug.Log("Checking for update.");
            currentVersion = Application.version;
            versionCheckURL = "https://raw.githubusercontent.com/IceWaterNotIce/Internet-Empire/main/Assets/StreamingAssets/version.json";
            StartCoroutine(CheckForUpdate());
        }

        public IEnumerator CheckForUpdate()
        {
            Debug.Log("Checking for update.");
            UnityWebRequest www = UnityWebRequest.Get(versionCheckURL);
            yield return www.SendWebRequest();


            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Connect to server failed.");
                Debug.LogError(www.error);
            }
            else
            {
                // 解析 JSON 響應
                var jsonResponse = www.downloadHandler.text;
                VersionConfig versionData = JsonUtility.FromJson<VersionConfig>(jsonResponse);

                string platform = new string("");
                string platformFilePath = new string("");
                if (Application.platform == RuntimePlatform.Android)
                {
                    platformFilePath = Path.Combine(Application.persistentDataPath, "platform.txt");
                }
                else
                {
                    platformFilePath = Path.Combine(Application.streamingAssetsPath, "platform.txt");
                }

                if (Application.platform == RuntimePlatform.Android)
                {
                    // if file does not exist, create it
                    if (!System.IO.File.Exists(platformFilePath))
                    {
                        System.IO.File.Create(platformFilePath).Close();
                        System.IO.File.WriteAllText(platformFilePath, "android");
                    }
                }

                // Check the local version config file exists
                if (System.IO.File.Exists(platformFilePath))
                {
                    platform = System.IO.File.ReadAllText(platformFilePath);
                }


                foreach (VersionConfig.VersionInfo info in versionData.platforms)
                {
                    Debug.Log("info.name: " + info.name);
                    Debug.Log("platform: " + platform);
                    Debug.Log(info.name == platform);
                    if (info.name == platform)
                    {
                        versionInfo = info;
                        //compare version// greater or equal to or less than
                        Debug.Log(new System.Version(currentVersion) < new System.Version(info.latestVersion));
                        if (new System.Version(currentVersion) < new System.Version(info.latestVersion))
                        {
                            Debug.Log("Version is not up to date.");
                            InitializeSceneManager initializeSceneManager = GameObject.FindFirstObjectByType<InitializeSceneManager>();
                            initializeSceneManager.SetVersionChecked(false);
                        }
                        else
                        {
                            Debug.Log("Version is up to date.");
                            InitializeSceneManager initializeSceneManager = GameObject.FindFirstObjectByType<InitializeSceneManager>();
                            initializeSceneManager.SetVersionChecked(true);
                        }
                    }
                }
            }

            Debug.Log("Update check completed.");
        }

        [System.Serializable]
        public class VersionConfig
        {
            [System.Serializable]
            public class VersionInfo
            {
                public string name;
                public string latestVersion;
                public string downloadURL;
            }
            public List<VersionInfo> platforms;
        }


    }
}