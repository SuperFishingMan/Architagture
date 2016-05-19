/**
 * Manages Player movement and Interactions
 * */

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class playerControllerOld : MonoBehaviour {

	public GameObject cursor;
	public GameObject walkHereCursor;
	public Transform MainCamera;//the anchor for where we're looking
	public float range = 100f;
	public float speed;
	public float interactionRange = 10f;

	//raycasting
	Ray ray; 
	RaycastHit hit; 
	bool rayHit = false;
	Vector3 focusPoint;
	bool enabled = false;
	//player
	Component collider;
	Rigidbody rigidbody;
	//movement
	Vector3 targetPosition; //the position the player will move to
	Vector3 walkHereCursorPosition;
	bool hitWalkable = false;
	bool isWalking = false;
	Text walkHerePrompt;
	//interaction
	bool hitInteractive = false;
	GameObject hitObject;


	void Start () {
		collider = GetComponent<CapsuleCollider>();
		rigidbody = GetComponent<Rigidbody> ();
		Input.simulateMouseWithTouches = true;
		walkHerePrompt = walkHereCursor.GetComponentInChildren<Text> ();
	}

	void FixedUpdate () {
		//setup raycast
		ray.origin = MainCamera.position;
		ray.direction = MainCamera.forward; 

		rayHit = false;
		hitWalkable = false; 

		if (Physics.Raycast (ray, out hit, range)) {

			rayHit = true;
			focusPoint = hit.point;


			//MOVEMENT - check if hit object has walkable tag
			if(hit.transform.gameObject.tag == "Walkable" && enabled)
			{
				hitWalkable = true;

				//check if player clicks mouse (for devellopment only) or taps screen
				if (Input.GetMouseButtonDown(0) || Input.GetButtonDown("Fire1")){
					isWalking = false;
					targetPosition = new Vector3(hit.point.x, transform.position.y, hit.point.z);
					walkHereCursorPosition = hit.point;
				}
			} 
			//INTERACTION - check if hit object has interactive tag
			else if(hit.transform.gameObject.tag == "Interactive" && enabled)
			{
				hitInteractive = true;
				hitObject = hit.transform.gameObject;
				//check if player clicks mouse (for devellopment only) or taps screen
				if (Input.GetMouseButtonDown(0) || Input.GetButtonDown("Fire1")){
					//call action method, if it exists
					hitObject.SendMessage ("Action", SendMessageOptions.DontRequireReceiver);
				} else{
					//call hover method, if it exists
					hitObject.SendMessage ("Hover", SendMessageOptions.DontRequireReceiver);
				}

				
				//check if screen is touched
				foreach (Touch touch in Input.touches) {
					if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved) {

					}
				}	
			}
			//NOTHING SPECIAL HIT
			else
			{
				rayHit = false;
			}

		} else{
			//set focuPoint at fixed distance when looking at the sky
			focusPoint = MainCamera.forward * interactionRange;
		}

		//INTERACTION - if last frame you had hit something interactive,s end hoveroff message
		if(hitInteractive && (!rayHit || hitWalkable))
		{
			Debug.Log("off");
			hitObject.SendMessage ("HoverOff", SendMessageOptions.DontRequireReceiver);
			hitInteractive = false;
		}

		updateCursors ();
		movePlayer ();
	}

	void updateCursors()
	{
		//set cursor at focuspoint
		cursor.transform.position = focusPoint;

		//set the walkHere cursor
		walkHerePrompt.enabled = (isWalking) ? false : true;
		walkHereCursor.transform.position = (hitWalkable) ? ((isWalking) ? walkHereCursorPosition : focusPoint) : new Vector3(0.0f, -999.0f, 0.0f) ;
	}

	void movePlayer()
	{	
		//move player if the target doesnt match the player position
		if(r1(transform.position.x) != r1(targetPosition.x) && r1(transform.position.z) != r1(targetPosition.z) && targetPosition != Vector3.zero){
			isWalking = true;
			transform.position = Vector3.MoveTowards (transform.position, targetPosition, Time.deltaTime * speed);
		} else {
			isWalking = false;
		}
	}

	//rounds float to one digit
	float r1(float x)
	{
		return Mathf.Round(x * 10f) / 10f;
	}

	public void enable()
	{
		enabled = true;
	}

	public void disable()
	{
		enabled = false;
	}
}
