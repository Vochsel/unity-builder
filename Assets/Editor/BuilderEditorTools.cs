using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;

public class BuilderEditorTools : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	[MenuItem("Builder/Create/BuildPreferences")]
	public static void CreateBuildPreferences()
	{
		BuildPreferences asset = ScriptableObject.CreateInstance<BuildPreferences>();

		AssetDatabase.CreateFolder ("Assets", "Resources");
		AssetDatabase.CreateFolder ("Assets/Resources", "Preferences");
		AssetDatabase.CreateAsset(asset, "Assets/Resources/Preferences/BuildPrefs.asset");
		AssetDatabase.SaveAssets();

		EditorUtility.FocusProjectWindow();

		Selection.activeObject = asset;
	}

	[MenuItem("Builder/Build/Android")]
	public static void BuildAndroid() {
		BuildAny (BuildTarget.Android);
	}

	[MenuItem("Builder/Build/All Project Targets", false, 0)]
	public static void BuildAllProjectTargets() {
		BuildPreferences prefs = Resources.Load<BuildPreferences>("Preferences/BuildPrefs");

		foreach (BuildTarget target in prefs.Targets) {
			BuildAny (target);
		}
	}

	public static void BuildAny(BuildTarget a_platform){

		string platformName = a_platform.ToString ();

		Debug.LogFormat ("Building for {0}", platformName);

		// Build the player
		BuildPreferences prefs = Resources.Load<BuildPreferences>("Preferences/BuildPrefs");

		BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
		buildPlayerOptions.scenes = prefs.ScenePaths;

		Debug.Log (buildPlayerOptions.scenes);

		buildPlayerOptions.locationPathName = string.Format("/{0}/{3}/{2}/{1}_{2}_Android.apk", Application.dataPath + "/../" + prefs.BuildFolder, prefs.ProjectName, prefs.ProjectVersion, platformName);
		Debug.Log (buildPlayerOptions.locationPathName);
		buildPlayerOptions.target = a_platform;

		BuildOptions bo = new BuildOptions();

		for (int i = 0; i < prefs.Options.Length; i++) {
			bo |= prefs.Options [i];
		}

		buildPlayerOptions.options = bo;

		BuildPipeline.BuildPlayer(buildPlayerOptions);
	}

}
