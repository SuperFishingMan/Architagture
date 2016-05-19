using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour {

    //general
    GameObject player;
    GazeCursor gazeCursor;//raycast input script on player
    public float interactionRange;
    	
    //INTERACTABLE
    GameObject hitObjectInteractive;
    //states
    bool hitInteractive = false;
    bool hoverOnInteractive = false;//true for first frame after cursor leaving an interactive surface

    //GUI
    public Sprite GUIHoverCursor;
    public Sprite GUIClickCursor;
    //states
    bool hitGUI = false;
    bool hitGUIInteractive = false;

    /**
     * Runs at start
     * 
     * @param NULL
     * @return NULL
     */
    void Start () {
        player = GameObject.FindGameObjectWithTag ("Player"); //locates the player
        gazeCursor = player.GetComponent <GazeCursor>();
    }

    /**
    * Main Update function, checks if surface was hit withing range, calls necessary methods accordingly
    *
    * @param NULL
    * @return NULL
    */
	void Update () {

        //INTRERACCTIVE surface was hit
		if(gazeCursor.hitTag == "Interactive" && gazeCursor.hitDistance <= interactionRange){

            hitObjectInteractive = gazeCursor.hitObject;

			//check if player clicks mouse (for devellopment only) or taps screen
			if (Input.GetMouseButtonDown(0) || Input.GetButtonDown("Fire1") || Input.GetKeyDown("r") || Cardboard.SDK.CardboardTriggered){
				//call action method, if it exists
				gazeCursor.hitObject.SendMessage ("Action", SendMessageOptions.DontRequireReceiver);
			} else{
				//call hover method, if it exists
				gazeCursor.hitObject.SendMessage ("Hover", SendMessageOptions.DontRequireReceiver);
			}

            //update states
            hitInteractive = true;
		} else{
            if (hitInteractive)
            {
                hitObjectInteractive.SendMessage("HoverOff", SendMessageOptions.DontRequireReceiver);
            }

            //update states
            hitInteractive = false;
		}

        //GUI surface was hit
        if (gazeCursor.hitTag == "GUI" && gazeCursor.hitDistance <= interactionRange)
        {
            if (!hitGUI)
            {
                foreach (Text text in gazeCursor.cursorText)
                {
                    text.text = "";
                }
                gazeCursor.cursorSprite.sprite = GUIHoverCursor;
            }

            hitGUI = true;
        }
        else
        {
            if (hitGUI && !gazeCursor.rayHitInteractive)
            {
                gazeCursor.cursorSprite.sprite = gazeCursor.defaultCursor;
            }

            hitGUI = false;
        }

        //GUIInteractive surface was hit
        if (gazeCursor.hitTag == "GUIInteractive" && gazeCursor.hitDistance <= interactionRange)
        {
            if (!hitGUIInteractive)
            {
                foreach (Text text in gazeCursor.cursorText)
                {
                    text.text = "";
                }
                gazeCursor.cursorSprite.sprite = GUIClickCursor;
            }

            hitGUI = true;
        }
        else
        {
            if (hitGUIInteractive && !gazeCursor.rayHitInteractive)
            {
                gazeCursor.cursorSprite.sprite = gazeCursor.defaultCursor;
            }

            hitGUI = false;
        }
	}
}