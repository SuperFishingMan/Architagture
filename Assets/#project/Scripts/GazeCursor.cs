using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GazeCursor : MonoBehaviour {

	public Transform MainCamera;		//the anchor for where we're looking
	public float range = 100f;
	public Sprite defaultCursor = null;
	public Sprite emptyCursor = null;
	public Sprite interactiveSprite = null;
	public Sprite interactiveSpriteClick = null;

	//raycasting
	Ray ray; 
	RaycastHit hit; 
	[HideInInspector] public bool rayHit = false;
	[HideInInspector] public bool rayHitInteractive = false;
	[HideInInspector] public GameObject hitObject;
	[HideInInspector] public string hitTag;
	[HideInInspector] public float hitDistance;
	[HideInInspector] public Vector3 hitNormal; //the normal of the ray
	[HideInInspector] public Vector3 focusPoint; //the ray hitpoint

	//cursor
	[HideInInspector] public Text[] cursorText;
	GameObject cursor;
	[HideInInspector] public SpriteRenderer cursorSprite;

	public bool lockSize = false; 				//scales reticle relative to distance
	public float sizeDistOrigin = 4f;			//distance fomr player from where the size is 0
	private Vector3 originalScale;				//original scale of cursor
	private float tempDistForStandardCursor = 4f;
	private Transform head;

	// Use this for initialization
	void Start () {
		cursor = GameObject.FindWithTag("Cursor");
		cursorText = cursor.GetComponentsInChildren <Text>();
		cursorSprite = cursor.GetComponent <SpriteRenderer>();

		GameObject headGO = GameObject.FindGameObjectWithTag("Head"); //locates the player
		head = headGO.transform;
		originalScale = transform.localScale;
		
		hitTag = "Untagged";
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		//setup raycast
		ray.origin = MainCamera.position;
		ray.direction = MainCamera.forward; 
		
		rayHit = false;
		rayHitInteractive = false;
		
		if (Physics.Raycast (ray, out hit, range)) {
			rayHit = true;
			hitObject = hit.transform.gameObject;
			hitTag = hitObject.tag;
			hitDistance = hit.distance;
			
			if(hitTag!="Untagged")
				rayHitInteractive = true;
			//Debug.Log (rayHitInteractive +"-"+hit.transform.gameObject.tag );
			focusPoint = hit.point;

			hitNormal = hit.normal;
		} else{
			//set focuPoint at fixed distance when looking at the sky
			focusPoint = MainCamera.forward * 5f;
		}

		if (lockSize) {
			
		}
		scale ();

		//set cursor at focuspoint
		cursor.transform.position = focusPoint;
	}

	private void scale()
	{
		float dist = Vector3.Distance (head.position, cursor.transform.position);
		//Debug.Log (dist);
		float factor = dist / sizeDistOrigin;
		cursor.transform.localScale = originalScale*factor*0.7f;
	}
}
