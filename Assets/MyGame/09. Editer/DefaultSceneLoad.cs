using UnityEditor;
using UnityEditor.SceneManagement;

[InitializeOnLoad]
public class DefaultSceneLoad
{
    static DefaultSceneLoad() // 로딩창 로드
    {
        var pathOfFirstScene = EditorBuildSettings.scenes[0].path;
        var sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(pathOfFirstScene);
        EditorSceneManager.playModeStartScene = sceneAsset;
    }
}
