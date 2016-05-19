using UnityEngine;
using System.Collections;

public class RayCast : MonoBehaviour {

	public float _RayRange;
	public static Vector3 Hitpoint;
	public static Vector3 Hitnormal;
	public static Transform Hittransform;
	public static bool HitObject = false;
	public static string Hittag;
	public static int Hitlayernumb;
	public static string Hitlayer = "";
	public static float HitDistance;

	private Ray _Ray;
	private RaycastHit _Hit;

	
	// Update is called once per frame
	void FixedUpdate () {

		_Ray.origin = transform.position;
		_Ray.direction = transform.forward;

		Debug.DrawRay (_Ray.origin, _Ray.direction, Color.green);

		// Bit shift the index of the layer (8) to get a bit mask. Mask of everything but Grid layer
		int layerMask = 1 << LayerMask.NameToLayer("Trigger");

		// This would cast rays only against colliders in layer 'Grid'.
		// But instead we want to collide against everything except 'Grid'. The ~ operator does this, it inverts a bitmask.
		layerMask = ~layerMask;

		bool hit = false;

		if (Physics.Raycast (_Ray, out _Hit, _RayRange, layerMask)) {
			hit = true;
			HitObject = true;
		}
		//nothing at all was hit somehow
		else {
			HitObject = false;
		}

		if (hit) {
			Hitpoint = _Hit.point;
			Hitnormal =_Hit.normal;
			Hittransform =_Hit.transform;
			Hittag = _Hit.transform.tag;
			Hitlayernumb = _Hit.transform.gameObject.layer;
			Hitlayer = LayerMask.LayerToName(_Hit.transform.gameObject.layer);
			HitDistance = Vector3.Distance (_Ray.origin, Hitpoint);
		}
	}
}
