using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OpenDoor : Interactable {

	public bool open = false;
	public string promptOpen = "OPEN DOOR";
	public string promptClose = "CLOSE DOOR";
	public float openEulerAngle = 90f;
	public bool openInward = true;
	public float openTime;

	bool isMoving = false;
	float passedTime = 0f;

	void Start()
	{
		if (open) {
			//set door in open position

			promptText= promptClose;
			float mult = (openInward) ? 1f : -1f;
			transform.Rotate(0,mult*openEulerAngle,0);
		} else {
			//set door in closed position
			promptText = promptOpen;
			transform.Rotate(0,0,0);
		}
	}

	void Update()
	{
		if (isMoving) 
		{
			
			if (open) {
				//close door
				transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, 0), passedTime+=(Time.deltaTime / openTime) );
				if(transform.rotation == Quaternion.Euler(0, 0, 0))
				{
					passedTime = 0f;
					open = false;
					promptText = promptOpen;
					isMoving = false;
				}
			} else {
				//open door


				float mult = (openInward) ? 1f : -1f;
				transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, mult*openEulerAngle, 0), passedTime+=(Time.deltaTime / openTime));
				if(transform.rotation == Quaternion.Euler(0, mult*openEulerAngle, 0))
				{
					passedTime = 0f;
					open = true;
					promptText= promptClose;
					isMoving = false;
				}
			}
		}
	}

	public void Action()
	{
		if (!isMoving) 
		{
			isMoving = true;
		}
	}

	public void Open()
	{
		if (!isMoving && !open) 
		{
			isMoving = true;
		}
	}

	public void Close()
	{
		if (!isMoving && open) 
		{
			isMoving = true;
		}
	}

	//add open and close public methods
}
