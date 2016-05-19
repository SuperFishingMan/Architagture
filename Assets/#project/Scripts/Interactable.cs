using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Interactable : MonoBehaviour {

	public string promptText;
    public Sprite hoverSprite;

    //general
    GameObject player;
    GazeCursor gazeCursor;//raycast input script on player
	
	public void Hover()
	{
        player = GameObject.FindGameObjectWithTag("Player"); //locates the player
        gazeCursor = player.GetComponent<GazeCursor>();

        foreach (Text text in gazeCursor.cursorText)
        {
            text.text = promptText;
        }
        gazeCursor.cursorSprite.sprite = hoverSprite;
	}

	public void HoverOff()
	{
        player = GameObject.FindGameObjectWithTag("Player"); //locates the player
        gazeCursor = player.GetComponent<GazeCursor>();

        foreach (Text text in gazeCursor.cursorText)
        {
            text.text = "";
        }
        gazeCursor.cursorSprite.sprite = gazeCursor.defaultCursor;
	}
}
