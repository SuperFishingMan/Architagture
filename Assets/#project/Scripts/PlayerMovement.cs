using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour {

	[Header ("Player Movement Params")]
	public float m_Speed;
	public float m_InteractionRangeMax;
	public float m_InteractionRangeMin;
	public Sprite m_CursorSprite;
	public GameObject m_WalkHereTarget;
	public GameObject m_WalkhereCursor;
	public Transform m_PlayerForewardAnchor;

	//movement
	private Vector3 m_TargetPosition; //the position the m_Player will move to
	private bool m_IsWalking = false;
	
	//states	
	private bool m_HitWalkable = false;
	private bool m_HoverOnWalkable = false; 			//true for first frame after cursor entering a walkable surface
	private bool m_HoverOffWalkable = false; 			//true for first frame after cursor leaving a walkable surface
	[HideInInspector] public bool m_TurnOff = false;	//movement will be disabled as soon as m_Player reaches last target
	private bool m_CursorSpawned = false;

	//general
	private GameObject m_Player;
	private GazeCursor m_GazeCursor; //raycast input script on m_Player
	
	//walkhere Target
	private Vector3 m_WalkHereTargetPosition;
	private bool m_WalkHereTargetSpawned = false;
	private GameObject m_Target;
	private GameObject m_Cursor;

	//BAD CODE
	private float  m_TempDistForStandardCursor = 0.5f;

	[Header ("Player Height Params")]
	public float m_PlayerHeight = 1.65f;

    /**
    * Runs at start
    * 
    * @param NULL
    * @return NULL
    */
	void Start () 
	{
		m_Player = GameObject.FindGameObjectWithTag ("Player"); //locates the m_Player
		m_GazeCursor = m_Player.GetComponent <GazeCursor>();
	}

    /**
    * Main Update function, checks if surface was hit withing range, calls necessary methods accordingly
    * Updates curor GUI
    *
    * @param NULL
    * @return NULL
    */
	void FixedUpdate () 
	{
		switch (UserPrefs.cardboardType) {
		case "Gen1":
			MoveGen1 ();
			break;

		case "Gen2":
			MoveGen2 ();
			break;
		}

	}


	private void MoveGen1(){
		m_GazeCursor.lockSize = false;

		//check if walkable surface was hit
		if(m_GazeCursor.hitTag == "Walkable" && m_GazeCursor.hitDistance <= m_InteractionRangeMax && m_GazeCursor.hitDistance >= m_InteractionRangeMin){

			//update states
			if(!m_HitWalkable){
				m_HitWalkable = true;
				m_HoverOnWalkable = true;
			} else{
				m_HoverOnWalkable = false;
			}
			m_HoverOffWalkable = false;

			if (!m_CursorSpawned && !m_TurnOff) {
				m_Cursor = (GameObject)Instantiate (m_WalkhereCursor, m_GazeCursor.focusPoint, Quaternion.identity);
				m_CursorSpawned = true;
			}

			if (m_CursorSpawned) {
				m_Cursor.transform.position = m_GazeCursor.focusPoint;
			}

			//check if m_Player clicks mouse (for devellopment only) or taps screen
			if (Input.GetMouseButtonDown(0) || Input.GetButtonDown("Fire1") || Input.GetKeyDown("r") || Cardboard.SDK.CardboardTriggered){
				m_IsWalking = false;
				m_TargetPosition = new Vector3(m_GazeCursor.focusPoint.x, m_Player.transform.position.y, m_GazeCursor.focusPoint.z);
				m_WalkHereTargetPosition = m_GazeCursor.focusPoint;
				//if you were already walking, respawn target to new position
				if(m_WalkHereTargetSpawned){
					Destroy(m_Target);
					m_Target =  (GameObject) Instantiate(m_WalkHereTarget, m_GazeCursor.focusPoint, Quaternion.identity);
				}
			}
		} else{

			if (m_CursorSpawned) {
				Destroy (m_Cursor);
				m_CursorSpawned = false;
			}

			//update states
			if(m_HitWalkable){
				m_HitWalkable = false;
				m_HoverOffWalkable = true;
			} else{
				m_HoverOffWalkable = false;
			}
			m_HoverOnWalkable = false;
		}

		//move m_Player if the target doesnt match the m_Player position
		if(r1(m_Player.transform.position.x) != r1(m_TargetPosition.x) && r1(m_Player.transform.position.z) != r1(m_TargetPosition.z) && m_TargetPosition != Vector3.zero){
			m_IsWalking = true;
			transform.position = Vector3.MoveTowards (m_Player.transform.position, m_TargetPosition, Time.deltaTime * m_Speed);
		} else {
			m_IsWalking = false;
			if(m_WalkHereTargetSpawned){
				Destroy(m_Target);
				m_WalkHereTargetSpawned = false;
			}
		}

		//update cursor
		if (m_HoverOffWalkable && !m_GazeCursor.rayHitInteractive || m_HoverOffWalkable && m_GazeCursor.hitDistance > m_InteractionRangeMax || m_HoverOffWalkable && m_GazeCursor.hitDistance < m_InteractionRangeMin || m_TurnOff)
		{
			foreach (Text text in m_GazeCursor.cursorText) {
				text.text = "";
			}
			m_GazeCursor.cursorSprite.sprite = m_GazeCursor.defaultCursor;
			m_GazeCursor.lockSize = true;
		}

		if (m_HoverOnWalkable) {
			m_GazeCursor.cursorSprite.sprite = m_GazeCursor.emptyCursor;
		}

		//spawn walk target
		if(m_IsWalking && !m_WalkHereTargetSpawned){
			m_WalkHereTargetSpawned = true;
			m_Target =  (GameObject) Instantiate(m_WalkHereTarget, m_GazeCursor.focusPoint, Quaternion.identity);
		}

		if (!m_IsWalking && m_TurnOff) {
			enabled = false;
		}
	}

	private void MoveGen2(){
		if (InputState.hold) {
			m_Player.transform.Translate ( m_PlayerForewardAnchor.forward * Time.deltaTime * m_Speed);
		}
	}

   /**
   * rounds float to one digit after comma
   *
   * @param float x
   * @return float
   */
	float r1(float x)
	{
		return Mathf.Round(x * 10f) / 10f;
	}	
}
