using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InteractionGen2 : MonoBehaviour {

	[SerializeField] private PlayerMovement playerMovement;
	[SerializeField] private GameObject commentPointer;
	[SerializeField] private SingleMicrophoneCapture SMC;

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
		GameObject player = GameObject.FindGameObjectWithTag("Player"); //locates the player
		gazeCursor = player.GetComponent<GazeCursor>();
	}

	void Update(){
		PostCommentGen2 ();
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
					commentP.transform.Rotate (0f, 0f, angle - 180f);
				}
				commentP.GetComponent<NormalHitPosition> ().enabled = false;
				RecPU = Instantiate (RecPopUp, new Vector3 (RayCast.Hitpoint.x, RayCast.Hitpoint.y + 0.5f, RayCast.Hitpoint.z), Quaternion.identity) as GameObject; //spawn record popup
				SMC.SetRecParams (commentP.transform.position, commentP.transform.rotation);
			} 
		}
	}
		
	public void StopRecording(){
		Destroy (commentP);				//destroy commentPointer

		buttonActivated = false;		//reset bools
		commentTagged = false;
		hoverOn = false;

		Destroy (RecPU);				//destroy rec popup

		playerMovement.enabled = true;	//enable walking
		playerMovement.m_TurnOff = false;

		m_DoubleClicked = false;
	}
}
