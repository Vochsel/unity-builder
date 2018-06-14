using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;
using System.IO;

public class BuilderEditorTools : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    [MenuItem("Builder/Utils/Create Build Preferences", priority = 0)]
    public static void CreateBuildPreferences()
    {
        BuildPreferences asset = ScriptableObject.CreateInstance<BuildPreferences>();

        if (Resources.Load<BuildPreferences>("Preferences/BuildPrefs"))
        {
            Debug.Log("BuildPrefs already exists in Resources folder!");
            return;
        }

        if (!AssetDatabase.IsValidFolder("Assets/unity-builder"))
            AssetDatabase.CreateFolder("Assets", "unity-builder");
        if (!AssetDatabase.IsValidFolder("Assets/unity-builder/Resources"))
            AssetDatabase.CreateFolder ("Assets/unity-builder", "Resources");
        if (!AssetDatabase.IsValidFolder("Assets/unity-builder/Resources/Preferences"))
            AssetDatabase.CreateFolder ("Assets/unity-builder/Resources", "Preferences");

		AssetDatabase.CreateAsset(asset, "Assets/unity-builder/Resources/Preferences/BuildPrefs.asset");
		AssetDatabase.SaveAssets();

		EditorUtility.FocusProjectWindow();

		Selection.activeObject = asset;
	}
    
	[MenuItem("Builder/Build/Build Development Targets", priority = 1)]
	public static void BuildAllProjectTargets() {
		BuildPreferences prefs = Resources.Load<BuildPreferences>("Preferences/BuildPrefs");

		foreach (BuildTarget target in prefs.Targets) {
			BuildAny (target);
		}
	}

    [MenuItem("Builder/Utils/Open Build Folder")]
    public static void OpenBuildFolder()
    {
        BuildPreferences prefs = Resources.Load<BuildPreferences>("Preferences/BuildPrefs");

        System.Diagnostics.Process.Start(@prefs.BuildFolder);
    }


    public static void BuildAny(BuildTarget a_platform){

		string platformName = a_platform.ToString ();

		Debug.LogFormat ("Building for {0}", platformName);

		// Build the player
		BuildPreferences prefs = Resources.Load<BuildPreferences>("Preferences/BuildPrefs");

		BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();

        //Get scenes from build settings
        buildPlayerOptions.scenes = new string[EditorBuildSettings.scenes.Length];
        for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
            buildPlayerOptions.scenes[i] = EditorBuildSettings.scenes[i].path;

        Debug.Log (buildPlayerOptions.scenes);
        
        buildPlayerOptions.locationPathName = string.Format("{0}\\{3}\\{2}\\{3}_{2}_{1}{4}",
            Path.GetFullPath(Application.dataPath + "/../" + prefs.BuildFolder), 
            prefs.ProjectName,
            prefs.ProjectVersion,
            platformName,
            ResolveExtension(a_platform)
        );

        Debug.Log(buildPlayerOptions.locationPathName);
        //{1}_{2}_Android.apk
        Debug.Log (buildPlayerOptions.locationPathName);
		buildPlayerOptions.target = a_platform;

		BuildOptions bo = new BuildOptions();

		for (int i = 0; i < prefs.Options.Length; i++) {
			bo |= prefs.Options [i];
		}

		buildPlayerOptions.options = bo;

		BuildPipeline.BuildPlayer(buildPlayerOptions);
	}

    static string ResolveExtension(BuildTarget a_target)
    {
        switch(a_target) {
            case BuildTarget.StandaloneOSX:
                return ".app";

            case BuildTarget.StandaloneWindows:
            case BuildTarget.StandaloneWindows64:
                return ".exe";

            case BuildTarget.iOS:
                return ".xcode";
                    
            case BuildTarget.Android:
                return ".apk";

            case BuildTarget.WebGL:
                return ".html";
        }

        return "";
    }

}
