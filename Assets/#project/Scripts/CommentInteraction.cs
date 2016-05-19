using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CommentInteraction : MonoBehaviour {
	
	[SerializeField] private GameObject button;
	[SerializeField] private Text buttonText;
	[SerializeField] private Image buttonImage;
	[SerializeField] private Image m_BackDrop;
	[SerializeField] private CanvasGroup canvasGroup;
	[SerializeField] private Sprite m_PlaySprite;
	[SerializeField] private Sprite m_PauseSprite;
	[SerializeField] private AudioSource AS;

	private GameObject RecPU;

	private float m_CommentDuration = 20f;
	private GazeCursor gazeCursor;
	private Sprite standardSprite;
	private bool buttonActivated = false;
	private bool hoverOn = false;
	private bool commentTagged = false;
	private Sprite emptySprite;
	private GameObject commentP;
	private bool cardboardTriggerInLastFrame = false;

	// Use this for initialization
	void Start () {
		standardSprite = buttonImage.sprite;
		GameObject player = GameObject.FindGameObjectWithTag("Player"); //locates the player
		gazeCursor = player.GetComponent<GazeCursor>();
	}

	void Update(){
		//check if player clicks mouse (for devellopment only) or taps screen
		if (Input.GetMouseButtonDown (0) || Input.GetButtonDown ("Fire1") || (Cardboard.SDK.CardboardTriggered && !cardboardTriggerInLastFrame)) {
			cardboardTriggerInLastFrame = true; //in cardboard 1, prevents firing trigger twice with one click
			if (hoverOn)
				click ();
		} else {
			cardboardTriggerInLastFrame = false;
		}
		//if the audioclip duration is passed, stop playing
		if (AS.isPlaying && AS.time >= m_CommentDuration) {
			StopPlayback ();
		}

		//update fill
		if (AS.isPlaying) {
			UpdateUIProgress();
		}

	}

	public void HoverOn(){
		//display play button
		if (!buttonActivated) {
			//update buttonsprite
			SetSprites ("play");
			//update cursorsprite (hide)
			foreach (Text text in gazeCursor.cursorText) {
				text.text = "";
			}
			gazeCursor.cursorSprite.sprite = gazeCursor.defaultCursor;
		} else {
			SetSprites ("stop");
		}

		hoverOn = true;
	}

	public void HoverOff(){
		if (!buttonActivated) {
			SetSprites ("default");
		} else {
			SetSprites ("play");
		}

		hoverOn = false;
	}

	private void click(){
		if (!buttonActivated) {
			Debug.Log ("Begin Playback");
			buttonActivated = true;
			SetSprites ("play");
			AS.Play ();
		} else {
			StopPlayback ();
		}
	}

	private void StopPlayback(){
		Debug.Log ("End Playback");
		buttonActivated = false;
		SetSprites ("default");
		AS.Stop ();
		buttonImage.fillAmount = 1f;
	}

	public void SetCommentDuration(float duration){
		m_CommentDuration = duration;
	}

	private void SetSprites(string state){
		switch (state) {
		case "play":
			buttonImage.sprite = m_PlaySprite;
			m_BackDrop.sprite = m_PlaySprite;
			break;

		case "stop":
			buttonImage.sprite = m_PauseSprite;
			m_BackDrop.sprite = m_PauseSprite;
			break;

		default:
			buttonImage.sprite = standardSprite;
			m_BackDrop.sprite = standardSprite;
			break;
		}
	}

	private void UpdateUIProgress(){
		float progress = 1 - (AS.time / m_CommentDuration);
		buttonImage.fillAmount = progress;

		Debug.Log (progress);
	}
}
