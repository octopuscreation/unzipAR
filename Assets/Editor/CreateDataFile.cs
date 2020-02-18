using UnityEditor;
using UnityEngine;

public class CreateDataFile : MonoBehaviour
{
    [MenuItem("Assets/Create/Generate bundle configuration file")]
    public static void CreateMyAsset()
    {
        BundleConfiguration asset = ScriptableObject.CreateInstance<BundleConfiguration>();

        AssetDatabase.CreateAsset(asset, "Assets/BundleGeneratorConfig.asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = asset;
    }
}
