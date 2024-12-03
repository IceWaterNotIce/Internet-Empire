using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class Testfasdf
{
    static Testfasdf()
    {
        // 注册构建事件
        BuildPlayerWindow.RegisterBuildPlayerHandler(OnBuildStart);
    }

    private static void OnBuildStart(BuildPlayerOptions options)
    {
        Debug.Log("构建开始！");

        // 调用原始的构建处理程序
        BuildPipeline.BuildPlayer(options);
        
        Debug.Log("构建结束！");
    }
}