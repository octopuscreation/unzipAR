using System;
using LitJson;
using System.IO;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BundleAnimationInfo
{
    public string name;
    public string image;
    public string audio;
}

public class BundleInfo
{
    public string prefab;
    public List<BundleAnimationInfo> animationInfoList = new List<BundleAnimationInfo>();
}

public class CharacterInfo
{
    public string name;
    public Sprite image;
    public AudioClip audioClip;
}

public class Character
{
    public GameObject prefab;
    public List<CharacterInfo> characterInfos = new List<CharacterInfo>();
}

public class LoadBundle : MonoBehaviour
{
    public string characterOutputPath = "Assets/Characters_bundles";

    private void Start()
    {
        //characterOutputPath = characterOutputPath = Application.dataPath + "/Characters_bundles";
        StartCoroutine(LoadingBundle());
    }

    private IEnumerator LoadingBundle()
    {
        BundleConfiguration configuration = (BundleConfiguration)Resources.Load("BundleConfig");

        if (string.IsNullOrEmpty(configuration.bundleName))
        {
            Debug.LogError("Please provide a bundle name in configuration file.");
            yield break;
        }

        BundleInfo bundleInfo = new BundleInfo();

        if (configuration.bundleName.Contains(".unity3d"))
            configuration.bundleName = configuration.bundleName.Substring(0, configuration.bundleName.IndexOf(".unity3d"));

        string file = configuration.bundlePlatform == TargetPlatform.Android ? configuration.bundleName + "_android.unity3d" : configuration.bundleName + "_ios.unity3d";

        string filePath = Path.Combine(characterOutputPath, configuration.bundleName + "/" + file);

        if (!File.Exists(filePath))
        {
            Debug.LogError("Failed to find bundle: " + filePath);
            yield break;
        }

        AssetBundle assetBundle = AssetBundle.LoadFromFile(filePath);
        if (assetBundle == null)
        {
            Debug.Log("Failed to load AssetBundle!");
            yield break;
        }

        TextAsset[] characterConfigurations = assetBundle.LoadAllAssets<TextAsset>();

        if (characterConfigurations.Length != 1)
        {
            if (characterConfigurations.Length == 0)
            {
                Debug.LogError("Configuration file not found. Please create a configuration file name_config.json");
                yield break;
            }
            else
            {
                Debug.LogError("Too much configuration files found... Just one supported actually");
                foreach (TextAsset txt in characterConfigurations)
                    Debug.Log("File: " + txt.name);
                yield break;
            }
        }

        TextAsset configFile = characterConfigurations[0];

        JsonData data = JsonMapper.ToObject(configFile.text);
        try
        {
            bundleInfo.prefab = (string)data["Prefab"];

            Debug.Log("Prefab: " + bundleInfo.prefab);
            Debug.Log("Animations : " + data["Animations"].Count);
            for (int i = 0; i < data["Animations"].Count; i++)
            {
                BundleAnimationInfo info = new BundleAnimationInfo();
                info.name = (string)data["Animations"][i][0];
                info.image = (string)data["Animations"][i][1];
                info.audio = (string)data["Animations"][i][2];

                bundleInfo.animationInfoList.Add(info);
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Exception message: " + e.Message);
        }

        InstantiateBundle(bundleInfo, assetBundle);

        assetBundle.Unload(false);
        yield return null;
    }

    private void InstantiateBundle(BundleInfo bundleInfo, AssetBundle assetBundle)
    {
        Character newCharacter = new Character();

        GameObject[] obj = assetBundle.LoadAllAssets<GameObject>();

        foreach (GameObject o in obj)
        {
            if (bundleInfo.prefab.Contains(o.name))
                newCharacter.prefab = o;
        }

        newCharacter.characterInfos = LoadAnimationFromInfo(bundleInfo, assetBundle);

        // Instantiate prefab
        GameObject character = Instantiate(newCharacter.prefab);

        // Initialize animation buttons
        UIManager.instance.InitAnimationPanel(character, newCharacter);
    }

    private List<CharacterInfo> LoadAnimationFromInfo(BundleInfo bundleInfo, AssetBundle assetBundle)
    {
        Sprite[] sprites = assetBundle.LoadAllAssets<Sprite>();
        Texture2D[] textures = assetBundle.LoadAllAssets<Texture2D>();
        AudioClip[] audioClip = assetBundle.LoadAllAssets<AudioClip>();

        List<CharacterInfo> animationInfoList = new List<CharacterInfo>();
        foreach (BundleAnimationInfo b in bundleInfo.animationInfoList)
        {
            CharacterInfo characterInfo = new CharacterInfo();

            if (b.name != bundleInfo.prefab)
            {
                characterInfo.name = b.name;

                if (sprites.Length > 0)
                {
                    foreach (Sprite tx in sprites)
                    {
                        if (tx.name == b.image)
                            characterInfo.image = tx;
                    }
                }
                else if (textures.Length > 0)
                {
                    foreach (Texture2D tx in textures)
                    {
                        if (tx.name == b.image)
                        {
                            Sprite sp = Sprite.Create(tx, new Rect(0, 0, 500, 500), new Vector2(0.5f, 0.5f));
                            characterInfo.image = sp;
                        }
                    }
                }

                foreach (AudioClip clip in audioClip)
                {
                    if (clip.name == b.audio)
                        characterInfo.audioClip = clip;
                }

                animationInfoList.Add(characterInfo);
            }
        }

        return animationInfoList;
    }
}
