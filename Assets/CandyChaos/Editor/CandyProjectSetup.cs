using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CandyChaos.Editor
{
    [InitializeOnLoad]
    internal static class CandyProjectSetup
    {
        private const string ScenePath = "Assets/Scenes/CandyChaos.unity";

        static CandyProjectSetup()
        {
            EditorApplication.delayCall += EnsureSetup;
        }

        private static void EnsureSetup()
        {
            if (EditorApplication.isCompiling || EditorApplication.isPlayingOrWillChangePlaymode) return;
            if (!File.Exists(ScenePath))
            {
                if (!Directory.Exists("Assets/Scenes")) Directory.CreateDirectory("Assets/Scenes");
                Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
                EditorSceneManager.SaveScene(scene, ScenePath);
                AssetDatabase.Refresh();
            }
            EditorBuildSettings.scenes = new[] { new EditorBuildSettingsScene(ScenePath, true) };
            PlayerSettings.companyName = "Candy Chaos Studio";
            PlayerSettings.productName = "Candy Chaos";
            PlayerSettings.defaultScreenWidth = 1280;
            PlayerSettings.defaultScreenHeight = 720;
            PlayerSettings.resizableWindow = true;
            PlayerSettings.defaultInterfaceOrientation = UIOrientation.LandscapeLeft;
            if (!EditorApplication.isPlaying && EditorSceneManager.GetActiveScene().path != ScenePath)
                EditorSceneManager.OpenScene(ScenePath);
        }
    }
}
