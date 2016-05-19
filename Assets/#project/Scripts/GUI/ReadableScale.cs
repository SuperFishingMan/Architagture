using UnityEngine;
using System.Collections;

public class ReadableScale : MonoBehaviour {

	public bool scaleOnStart = true;
	public bool scaleAtRuntime = false;
	public float zeroDistance = 4.5f;	//the distance from whitch the scale is 0

	private Vector3 originalScale;
	private Transform head;

	// Use this for initialization
	void Start() {
		originalScale = transform.localScale;
		GameObject headGO = GameObject.FindGameObjectWithTag("Head"); //locates the player
		head = headGO.transform;

		if (scaleOnStart) {
			scale ();
		}
	}
		
	// Update is called once per frame
	void Update () {
		if (scaleAtRuntime) {
			scale ();
		}
	}

	private void scale()
	{
		float dist = Vector3.Distance (head.position, transform.position);
		//Debug.Log (dist);
		float factor = dist / zeroDistance;
		transform.localScale = originalScale*factor;
	}
}
