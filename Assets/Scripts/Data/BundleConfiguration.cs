using UnityEngine;

public enum TargetPlatform
{
    Android,
    iOS
}

[CreateAssetMenu(fileName = "BundleConfig", menuName = "BundleConfig/Create Configuration file", order = 1)]
public class BundleConfiguration : ScriptableObject
{
    [Tooltip("Output bundle .unity3d")]
    public string bundleName = "";

    [Tooltip("Target folder in Assets/Characters/")]
    public string targetModelFolder = "";
    
    [Tooltip("Target bundle platform")]
    public TargetPlatform bundlePlatform = TargetPlatform.Android;
}
