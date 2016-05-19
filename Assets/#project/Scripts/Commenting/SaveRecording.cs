using UnityEngine;
using System.Collections;

public class SaveRecording : MonoBehaviour {

	public void startRecording(){
		GameObject CR = GameObject.FindGameObjectWithTag("CommentRecorder"); //locates the player
		SingleMicrophoneCapture SMC = CR.GetComponent<SingleMicrophoneCapture>();

		SMC.record();
	}

	public void die(){
		GameObject button = GameObject.FindGameObjectWithTag("Button"); //locates the player
		ButtonInteraction BI = button.GetComponent<ButtonInteraction>();
		BI.StopRecording ();
		Destroy (this.gameObject.transform.parent.gameObject);
	}
}
