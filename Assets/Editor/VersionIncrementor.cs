using UnityEngine;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using System.Diagnostics;
using System.IO;


[InitializeOnLoad]
public class VersionIncrementor : IPreprocessBuildWithReport
{
    public int callbackOrder => 0;

    static VersionIncrementor()
    {

    }

    public void OnPreprocessBuild(BuildReport report)
    {
        UpdateVersion();
        CommitAndPushToGit(PlayerSettings.bundleVersion);
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
        RunGitCommand("git add ProjectSettings/ProjectSettings.asset");
        RunGitCommand("git commit -m \"Auto commit from Unity Builder.\"");
        RunGitCommand("git tag -a v" + versionParts + " -m \"Auto tag from Unity Builder.\"");
        RunGitCommand("git push origin main");
        RunGitCommand("git push origin v" + versionParts);

        UnityEngine.Debug.Log("Git commit and push done.");
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

    private static void EditProjectSettings(string versionParts)
    {
       // using system io to edit the project settings file
        string filepath = Application.dataPath + "/ProjectSettings/ProjectSettings.asset";
        string[] lines = File.ReadAllLines(filepath);

        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i].Contains("bundleVersion:"))
            {
                lines[i] = "  bundleVersion: " + versionParts;
                break;
            }
        }

        File.WriteAllLines(filepath, lines);

    }
}