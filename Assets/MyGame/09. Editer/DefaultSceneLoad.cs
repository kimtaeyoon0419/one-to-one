using UnityEditor;
using UnityEditor.SceneManagement;

[InitializeOnLoad]
public class DefaultSceneLoad
{
    static DefaultSceneLoad()
    {
        var pathOfFirstScene = EditorBuildSettings.scenes[0].path;
        var sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(pathOfFirstScene);
        EditorSceneManager.playModeStartScene = sceneAsset;
    }
}
