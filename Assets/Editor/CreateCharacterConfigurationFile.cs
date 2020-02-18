using UnityEditor;
using UnityEngine;

public class CreateCharacterConfigurationFile : MonoBehaviour
{
    [MenuItem("Assets/Create/Generate character configuration file")]
    public static void CreateMyAsset()
    {
        BundleConfiguration asset = ScriptableObject.CreateInstance<BundleConfiguration>();

        AssetDatabase.CreateAsset(asset, "Assets/BundleGeneratorConfig.asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = asset;
    }
}
