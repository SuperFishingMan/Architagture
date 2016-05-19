using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ChangeCanvasScale : MonoBehaviour {

	public CanvasScaler _CanvasScaler;
	public int _StartScale;

	// Use this for initialization
	void Start () {
		#if UNITY_EDITOR
		return;
		#endif

		#if UNITY_ANDROID
		_CanvasScaler.scaleFactor = _StartScale;
		#endif
	}
}
