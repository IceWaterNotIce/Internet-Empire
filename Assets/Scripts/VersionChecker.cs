using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class VersionChecker : MonoBehaviour
{
    private string versionCheckURL;
    private string currentVersion = Application.version;

    void Start()
    {
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
        }
        else
        {
            // 解析 JSON 響應
            var jsonResponse = www.downloadHandler.text;
            var versionInfo = JsonUtility.FromJson<VersionInfo>(jsonResponse);

            if (versionInfo.latestVersion != currentVersion)
            {
                Debug.Log("有新版本可用: " + versionInfo.latestVersion);
                // 可以提示用戶下載新版本
                // Application.OpenURL(versionInfo.downloadURL);
            }
            else
            {
                Debug.Log("您正在運行最新版本.");
            }
        }
    }

    [System.Serializable]
    public class VersionInfo
    {
        public string latestVersion;
        public string downloadURL;
    }
}