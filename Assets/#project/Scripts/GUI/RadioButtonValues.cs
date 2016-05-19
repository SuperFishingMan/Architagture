using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class RadioButtonValues : MonoBehaviour {

	public RadioButtonValuePair[] RadioButtons;

	public string GetActiveValue(){
		string returnValue = "";
		foreach (RadioButtonValuePair radioButton in RadioButtons) {
			if(radioButton.toggle.isOn) {
				returnValue = radioButton.value;
			}
		}
		return returnValue;
	}

	public void SetActiveValue(string value){
		foreach (RadioButtonValuePair radioButton in RadioButtons) {
			Debug.Log(value+"-"+radioButton.value);
			if (value.Contains(radioButton.value)) {
				radioButton.toggle.isOn = true;
			} else {
				radioButton.toggle.isOn = false;
			}
		}
	}
}

[System.Serializable]
public class RadioButtonValuePair{
	public Toggle toggle;
	public string value;
}

/*
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RadioButton : MonoBehaviour {

	private Toggle _ThisButton;
	public Toggle[] _RadioButtons;

	void Start(){
		_ThisButton = GetComponent<Toggle> ();
	}

	public void ToggleValue(){
		foreach(Toggle Button in _RadioButtons){
			Button.isOn =  !_ThisButton.isOn;
		}
	}
}
*/