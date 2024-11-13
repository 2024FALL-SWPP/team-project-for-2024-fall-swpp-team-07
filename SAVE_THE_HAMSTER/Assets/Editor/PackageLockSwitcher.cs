// Assets/Editor/PackageLockSwitcher.cs
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using System.IO;

public class PackageLockSwitcher : AssetPostprocessor
{
    private static string GetPlatformSpecificFile()
    {
#if UNITY_EDITOR_WIN
        return "Packages/packages-lock.windows.json";
#elif UNITY_EDITOR_OSX
        return "Packages/packages-lock.mac.json";
#else
        return "Packages/packages-lock.json";
#endif
    }

    [InitializeOnLoadMethod]
    static void OnEditorStartup()
    {
        // 에디터 시작 시 플랫폼별 파일을 packages-lock.json으로 복사
        string sourceFile = GetPlatformSpecificFile();
        string destFile = "Packages/packages-lock.json";

        if (File.Exists(sourceFile))
        {
            File.Copy(sourceFile, destFile, true);
            AssetDatabase.Refresh();
            Debug.Log("packages-lock.json has been updated from platform-specific file.");
        }
        else
        {
            Debug.LogWarning($"Source file not found: {sourceFile}");
        }
    }

    static void OnPackageLockChanged()
    {
        // packages-lock.json이 변경되었을 때 플랫폼별 파일로 복사
        string sourceFile = "Packages/packages-lock.json";
        string destFile = GetPlatformSpecificFile();

        if (File.Exists(sourceFile))
        {
            File.Copy(sourceFile, destFile, true);
            Debug.Log($"Platform-specific packages-lock file has been updated: {destFile}");
        }
    }

    // AssetPostprocessor의 콜백 메서드
    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets,
                                       string[] movedAssets, string[] movedFromAssetPaths)
    {
        foreach (string asset in importedAssets)
        {
            if (asset == "Packages/packages-lock.json")
            {
                OnPackageLockChanged();
                break;
            }
        }
    }
}