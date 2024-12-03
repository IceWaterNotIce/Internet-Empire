using UnityEngine;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

[InitializeOnLoad]
public class VersionIncrementor : IPreprocessBuildWithReport
{
    public int callbackOrder => 0;
    public void OnPreprocessBuild(BuildReport report)
    {
        UpdateVersion();
    }

    private static void UpdateVersion()
    {
        string[] versionParts = PlayerSettings.bundleVersion.Split('.');
        if (versionParts.Length == 3)
        {
            if (int.TryParse(versionParts[2], out int patchVersion))
            {
                patchVersion++;
                PlayerSettings.bundleVersion = $"{versionParts[0]}.{versionParts[1]}.{patchVersion}";
                Debug.Log($"版本已更新为 {PlayerSettings.bundleVersion}");
            }
            else
            {
                Debug.LogError("解析补丁版本失败。");
            }
        }
        else
        {
            Debug.LogError("版本格式不正确。预期格式为: x.x.x");
        }
    }
}