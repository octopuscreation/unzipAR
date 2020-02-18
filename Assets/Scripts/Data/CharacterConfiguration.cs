using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AnimationInfos
{
    // Animation name
    public string name;
    // Animation sprite to be displayed in app
    public Sprite sprite;
    // Audio clip player when this animation is played
    public AudioClip audioClip;
}

public class JsonAnimInfos
{
    public string name;
    public string image;
    public string audio;
}

[CreateAssetMenu(fileName = "CharacterConfig", menuName = "BundleConfig/Create Character configuration file", order = 2)]
public class CharacterConfiguration : ScriptableObject
{
    public GameObject prefab;
    public List<AnimationInfos> animationsInfos;
}


