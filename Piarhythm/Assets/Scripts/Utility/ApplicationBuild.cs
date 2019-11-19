using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationBuild
{
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	static void OnRuntimeInitializeOnLoadMethod()
	{
		Screen.fullScreen = false;
	}
}
