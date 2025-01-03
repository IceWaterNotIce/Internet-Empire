using UnityEngine;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using System.Diagnostics;
using System.IO;
using System;

[InitializeOnLoad]
public class VersionIncrementor : IPreprocessBuildWithReport
{
    public int callbackOrder => 0;
    public void OnPreprocessBuild(BuildReport report)
    {
        UpdateVersion();
        CommitAndPushToGit(PlayerSettings.bundleVersion);
    }

    public void OnPostprocessBuild(BuildReport report)
    {

    }

    private static void UpdateVersion()
    {
        string versionFilePath = Path.Combine(Application.streamingAssetsPath, "version.json");
        if (File.Exists(versionFilePath))
        {
            string json = File.ReadAllText(versionFilePath);
            VersionInfo versionInfo = JsonUtility.FromJson<VersionInfo>(json);
            // versionInfo version + 1
            string[] versionInfoParts = PlayerSettings.bundleVersion.Split('.');
            if (versionInfoParts.Length == 3)
            {
                if (int.TryParse(versionInfoParts[2], out int patchVersion))
                {
                    patchVersion++;
                    versionInfo.latestVersion = $"{versionInfoParts[0]}.{versionInfoParts[1]}.{patchVersion}";
                    UnityEngine.Debug.Log($"Version updated to {versionInfo.latestVersion}");
                }
                else
                {
                    UnityEngine.Debug.LogError("Patch version is not a number.");
                }
            }
            else
            {
                UnityEngine.Debug.LogError("Version format is not correct. It should be like 1.0.0");
            }

            File.WriteAllText(versionFilePath, JsonUtility.ToJson(versionInfo));
        }

        string[] versionParts = PlayerSettings.bundleVersion.Split('.');
        if (versionParts.Length == 3)
        {
            if (int.TryParse(versionParts[2], out int patchVersion))
            {
                patchVersion++;
                PlayerSettings.bundleVersion = $"{versionParts[0]}.{versionParts[1]}.{patchVersion}";
                UnityEngine.Debug.Log($"Version updated to {PlayerSettings.bundleVersion}");
            }
            else
            {
                UnityEngine.Debug.LogError("Patch version is not a number.");
            }
        }
        else
        {
            UnityEngine.Debug.LogError("Version format is not correct. It should be like 1.0.0");
        }
    }

    private static void CommitAndPushToGit(string versionParts)
    {
        RunGitCommand("git add .");
        RunGitCommand("git commit -m \"Auto commit from Unity Builder. \"");
        RunGitCommand("git tag -a v" + versionParts + " -m \"Auto tag from Unity Builder. \"");
        RunGitCommand("git push origin main");
        RunGitCommand("git push origin v" + versionParts);

        UnityEngine.Debug.Log("Git commit and push done");
    }

    private static void RunGitCommand(string command)
    {
        ProcessStartInfo processStartInfo = new ProcessStartInfo("cmd.exe", "/c " + command);
        processStartInfo.WorkingDirectory = Application.dataPath;
        processStartInfo.RedirectStandardOutput = true;
        processStartInfo.UseShellExecute = false;
        processStartInfo.CreateNoWindow = false;

        using (Process process = Process.Start(processStartInfo))
        {
            process.WaitForExit();
            UnityEngine.Debug.Log(process.StandardOutput.ReadToEnd());
        }
    }

    [System.Serializable]
    public class VersionInfo
    {
        public string latestVersion;
        public string downloadURL;
    }
}