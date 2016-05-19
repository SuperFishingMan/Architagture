using UnityEngine;
using System.Collections;

public class SetScreenOrientation : MonoBehaviour {

	public bool _LandScapeLeft;
	public bool _Portrait;

	// Use this for initialization
	void Start () {
		if (_LandScapeLeft) {
			Screen.orientation = ScreenOrientation.LandscapeLeft;
		} 

		if (_Portrait) {
			Screen.orientation = ScreenOrientation.Portrait;
		}
	}
}
