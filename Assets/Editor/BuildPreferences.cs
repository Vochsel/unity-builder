using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using UnityEditor;

public class BuildPreferences : ScriptableObject {
	[Header("Project")]
	public string ProjectName = "Untitled";
	public string ProjectVersion = "0.0.1";

	public string[] ScenePaths = {"Assets/Scenes/TestScene.unity"};

	[Header("Build")]
	public string BuildFolder = "Builds/";

	public BuildOptions[] Options;

	public BuildTarget[] Targets;


}
