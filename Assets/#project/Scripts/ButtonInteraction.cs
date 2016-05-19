using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonInteraction : MonoBehaviour {

	[SerializeField] private GameObject button;
	[SerializeField] private Text buttonText;
	[SerializeField] private GameObject emptyButton;
	[SerializeField] private Image buttonImage;
	[SerializeField] private CanvasGroup canvasGroup;
	[SerializeField] private Sprite hoverSprite;
	[SerializeField] private Sprite clickSprite;
	[SerializeField] private PlayerMovement playerMovement;
	[SerializeField] private GameObject commentPointer;
	[SerializeField] private SingleMicrophoneCapture SMC;
	[SerializeField] private float m_DoubleClickTime = 0.5f;

	[Header("Messages")]
	[SerializeField] private string s_record;
	[SerializeField] private string s_tag;
	[SerializeField] private string s_cancel;

	[Header("Record")]
	[SerializeField] private GameObject RecPopUp;
	private GameObject RecPU;

	private GazeCursor gazeCursor;
	private Sprite standardSprite;
	private bool buttonActivated = false;
	private bool hoverOn = false;
	private bool commentTagged = false;
	private Sprite emptySprite;
	private GameObject commentP;
	private float m_CommentRecordStartTime = 0f;
	private bool cardboardTriggerInLastFrame = false;	//for cardboard 1 users, this prevents firing trigger twice in same frame
	private float m_LastClickTime = 1f;
	private bool m_DoubleClicked = false;

	// Use this for initialization
	void Start () {
		standardSprite = buttonImage.sprite;
		GameObject player = GameObject.FindGameObjectWithTag("Player"); //locates the player
		gazeCursor = player.GetComponent<GazeCursor>();
	}

	void Update(){

		switch (UserPrefs.cardboardType) {
		case "Gen1":
			PostCommentGen1 ();
			break;

		case "Gen2":
			PostCommentGen2 ();
			break;
		}
			
	}

	private void PostCommentGen1 (){
		//check if player clicks mouse (for devellopment only) or taps screen
		if (Input.GetMouseButtonDown (0) || Input.GetButtonDown ("Fire1") || (Cardboard.SDK.Triggered && !cardboardTriggerInLastFrame)) {

			cardboardTriggerInLastFrame = true; 	//in cardboard 1, prevents trigger from fireing twice in one click

			//4. stop recording - upload comment - close commentPointer - spawn comment
			if (commentTagged) {
				if (commentP != null) {
					Debug.Log ("stopped recording by clicking");
					SMC.stopRec ();
					Destroy (RecPU);
					StopRecording ();
				}
			} else if (hoverOn) {
				//2. cancel tagging
				if (buttonActivated) {
					buttonActivated = false;		//reset bools

					playerMovement.enabled = true;	//enable movement
					playerMovement.m_TurnOff = false;

					canvasGroup.alpha = 1f;			//show original comment button

					buttonText.text = s_record;		//set instructions

					Destroy (commentP);
				} 
				//1. activate tag
				else {
					buttonActivated = true;			//the button is pressed, and the user drags it to the tag position

					gazeCursor.cursorSprite.sprite = gazeCursor.interactiveSpriteClick;

					playerMovement.m_TurnOff = true;	//disactivate player movement

					canvasGroup.alpha = 0f;			//disable image

					buttonText.text = s_tag;		//change buttontext

					commentP = Instantiate (commentPointer, gazeCursor.focusPoint, Quaternion.identity) as GameObject; 	//instantiate CommentPointer
				}
			} 
			//3. pin the tag
			else if (buttonActivated && !commentTagged) {
				StopAllCoroutines ();//make sure non OffMeshLink the previous recordings is still running

				NormalHitPosition nhp = commentP.GetComponent<NormalHitPosition> ();	//disable script that's moving the commentpointer
				nhp.enabled = false;

				commentTagged = true;		//set bools

				buttonText.text = "";		//remove instructions on button

				RecPU = Instantiate (RecPopUp, new Vector3 (commentP.transform.position.x, commentP.transform.position.y + 0.5f, commentP.transform.position.z), Quaternion.identity) as GameObject; //spawn record popup

				SMC.SetRecParams (commentP.transform.position, commentP.transform.rotation);
			}

		} else {
			cardboardTriggerInLastFrame = false;
		}
	}

	private void PostCommentGen2 (){

		if (InputState.tap) {
			if (commentP != null) {
				Debug.Log ("stopped recording by clicking");
				SMC.stopRec ();
				Destroy (RecPU);
				StopRecording ();
			}
		}

		if (InputState.doubleTap) {
			if (!commentTagged) {
				StopAllCoroutines ();//make sure non OffMeshLink the previous recordings is still running

				commentTagged = true;//set bools
				playerMovement.enabled = false;	//disactivate player movement
				m_CommentRecordStartTime = Time.time;
				commentP = Instantiate (commentPointer, RayCast.Hitpoint, Quaternion.identity) as GameObject; 	//instantiate CommentPointer

				//rotate sprite perpendic to geom.
				commentP.transform.position = RayCast.Hitpoint;												//position cursor on hitlocation
				commentP.transform.LookAt(GameObject.FindGameObjectWithTag("Player").transform); 																//rotate towards player
				commentP.transform.rotation = Quaternion.Euler( Quaternion.FromToRotation (Vector3.forward, RayCast.Hitnormal).eulerAngles.x,  Quaternion.FromToRotation (Vector3.forward, RayCast.Hitnormal).eulerAngles.y,  commentP.transform.rotation.eulerAngles.z); //rotate from cursors foreward direction to the hit normal

				//rotate image so it faced player - sort of
				if (commentP.transform.forward == new Vector3 (0, 1, 0)) { 
					float angle = GameObject.FindGameObjectWithTag("Head").transform.eulerAngles.y;
					transform.Rotate (0f, 0f, angle - 180f);
				}
				commentP.GetComponent<NormalHitPosition> ().enabled = false;
				RecPU = Instantiate (RecPopUp, new Vector3 (RayCast.Hitpoint.x, RayCast.Hitpoint.y + 0.5f, RayCast.Hitpoint.z), Quaternion.identity) as GameObject; //spawn record popup
				SMC.SetRecParams (commentP.transform.position, commentP.transform.rotation);
			} 
		}
	}

	public void HoverOn(){
		if (!commentTagged) {
			if (!buttonActivated) {
				//update buttonsprite
				buttonImage.sprite = hoverSprite;
				//update cursorsprite (hide)
				foreach (Text text in gazeCursor.cursorText) {
					text.text = "";
				}
				gazeCursor.cursorSprite.sprite = gazeCursor.interactiveSprite;
			} else {
				//change buttontext
				buttonText.text = s_cancel;
			}
		}
		hoverOn = true;
	}

	public void HoverOff(){
		if (!commentTagged) {
			if (!buttonActivated) {
				buttonImage.sprite = standardSprite;
			} else {
				buttonText.text = s_tag;
			}
		}
		hoverOn = false;
	}
		
	public void StopRecording(){
		Destroy (commentP);				//destroy commentPointer

		buttonActivated = false;		//reset bools
		commentTagged = false;
		hoverOn = false;

		Destroy (RecPU);				//destroy rec popup

		playerMovement.enabled = true;	//enable walking
		playerMovement.m_TurnOff = false;

		canvasGroup.alpha = 1f;			//show original comment button

		buttonText.text = s_record;		//set instructions

		m_DoubleClicked = false;
	}
}
