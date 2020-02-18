using LitJson;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class AssetBundleManager : MonoBehaviour
{
    static private string characterRootFolder = Application.dataPath + "/Characters";
    static private string characterOutputPath = Application.dataPath + "/Characters_bundles";

    [MenuItem("AssetBundle/1. Generate configuration file (Optional)")]
    static void GenerateConfigurationFile()
    {
        BundleConfiguration bundleConfiguration = (BundleConfiguration)Resources.Load("BundleConfig");

        if (!bundleConfiguration)
        {
            Debug.LogError("Failed to find 'BundleConfig' file in Assets/Resources/");
            return;
        }

        CharacterConfiguration configuration = (CharacterConfiguration)Resources.Load("CharacterConfig");

        if (!configuration)
        {
            Debug.LogError("Failed to find 'CharacterConfig' file in Assets/Resources/");
            return;
        }

        StringBuilder sb = new StringBuilder();
        JsonWriter writer = new JsonWriter(sb);
        writer.PrettyPrint = true;

        writer.WriteObjectStart();
        writer.WritePropertyName("Prefab");
        writer.Write(configuration.prefab.name);

        writer.WritePropertyName("Animations");
        writer.WriteArrayStart();

        foreach (AnimationInfos info in configuration.animationsInfos)
        {
            JsonAnimInfos animInfo = new JsonAnimInfos();

            animInfo.name = info.name;
            if (info.sprite)
                animInfo.image = info.sprite.name;
            if (info.audioClip)
                animInfo.audio = info.audioClip.name;

            writer.Write(JsonMapper.ToJson(animInfo));
        }

        writer.WriteArrayEnd();
        writer.WriteObjectEnd();

        string path = "Assets/Characters/" + bundleConfiguration.targetModelFolder + "/" + bundleConfiguration.bundleName + "_config.json";
        StreamWriter fileWriter = new StreamWriter(path);
        fileWriter.WriteLine(sb.ToString());
        fileWriter.Close();
    }

    [MenuItem("AssetBundle/2. Generate bundle")]
    static void GenerateBundle()
    {
        BundleConfiguration configuration = (BundleConfiguration)Resources.Load("BundleConfig");

        if (!configuration)
        {
            Debug.LogError("Failed to find 'BundleConfig' file in Assets/Resources/");
            return;
        }

        if (configuration)
        {
            Debug.Log("Configuration file found.");
            Debug.Log("Bundle name: " + configuration.bundleName);
            Debug.Log("Target model folder: " + configuration.targetModelFolder);
            Debug.Log("Bundle platform: " + configuration.bundlePlatform);

            if (!Directory.Exists(characterRootFolder + "/" + configuration.targetModelFolder))
            {
                Debug.LogError(characterRootFolder + "/" + configuration.targetModelFolder + " doesn't exist...");
                return;
            }

            AssetBundleBuild[] buildMap = new AssetBundleBuild[1];
            buildMap[0].assetBundleName = configuration.bundleName + ".unity3d";

            List<string> fileList = new List<string>();
            string[] allfiles = Directory.GetFiles(characterRootFolder + "/" + configuration.targetModelFolder, "*.*", SearchOption.AllDirectories);

            foreach (string file in allfiles)
            {
                string filename = Path.GetFileName(file);

                if (!filename.Contains(".meta"))
                {
                    int startIndex = file.IndexOf("Assets/");
                    int endIndex = file.Length - startIndex;

                    string relativePath = file.Substring(startIndex, endIndex).Replace("\\", "/");

                    fileList.Add(relativePath);
                }
            }

            buildMap[0].assetNames = fileList.ToArray();

            if (configuration.bundleName.Contains(".unity3d"))
                configuration.bundleName = configuration.bundleName.Substring(0, configuration.bundleName.IndexOf(".unity3d"));

            buildMap[0].assetBundleName = configuration.bundlePlatform == TargetPlatform.Android ? configuration.bundleName + "_android.unity3d" : configuration.bundleName + "_ios.unity3d";

            if (!Directory.Exists(characterOutputPath))
            {
                Debug.Log("Creating folder: " + characterOutputPath);
                Directory.CreateDirectory(characterOutputPath);
            }

            if (!Directory.Exists(characterOutputPath + "/" + configuration.bundleName))
            {
                Debug.Log("Creating folder: " + characterOutputPath + "/" + configuration.bundleName);
                Directory.CreateDirectory(characterOutputPath + "/" + configuration.bundleName);
            }

            switch (configuration.bundlePlatform)
            {
                case TargetPlatform.Android:
                    Debug.Log("Building bundle for Android");
                    BuildPipeline.BuildAssetBundles(characterOutputPath + "/" + configuration.bundleName, buildMap, BuildAssetBundleOptions.None, BuildTarget.Android);
                    break;
                case TargetPlatform.iOS:
                    Debug.Log("Building bundle for iOS");
                    BuildPipeline.BuildAssetBundles(characterOutputPath + "/" + configuration.bundleName, buildMap, BuildAssetBundleOptions.None, BuildTarget.iOS);
                    break;
            }
        }
        else
            Debug.LogError("Configuration file not found.");
    }

    [MenuItem("AssetBundle/3. Check bundle")]
    static void CheckBundle()
    {
        BundleConfiguration configuration = (BundleConfiguration)Resources.Load("BundleConfig");

        if (configuration.bundleName.Contains(".unity3d"))
            configuration.bundleName = configuration.bundleName.Substring(0, configuration.bundleName.IndexOf(".unity3d"));

        string file = configuration.bundlePlatform == TargetPlatform.Android ? configuration.bundleName + "_android.unity3d" : configuration.bundleName + "_ios.unity3d";

        string filePath = Path.Combine(characterOutputPath, configuration.bundleName + "/" + file);

        if (!File.Exists(filePath))
        {
            Debug.LogError("Failed to find bundle: " + filePath);
            return;
        }

        AssetBundle assetBundle = AssetBundle.LoadFromFile(filePath);
        if (assetBundle == null)
        {
            Debug.Log("Failed to load AssetBundle!");
            return;
        }

        string[] assetNameList = assetBundle.GetAllAssetNames();
        foreach (string name in assetNameList)
            Debug.Log(name);

        TextAsset[] configurationFiles = assetBundle.LoadAllAssets<TextAsset>();
        if (configurationFiles.Length == 1)
            Debug.Log("YOUR BUNDLE IS VALID !");
        else
        {
            if (configurationFiles.Length == 0)
            {
                Debug.LogError("Configuration file not found. Please create a configuration file name_config.json");
            }
            else
            {
                Debug.LogError("Too much configuration files found... Just one supported actually");
                foreach (TextAsset txt in configurationFiles)
                    Debug.Log("File: " + txt.name);
            }
        }
        assetBundle.Unload(false);
    }
}
