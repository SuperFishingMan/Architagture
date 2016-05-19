using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CursorController : MonoBehaviour {

	[Header("General")]
	public bool _ToNormal;
	public bool _UniformSize;
	public float _NeutralDistance = 2f;
	public float _AtInfiniteDistance = 10f;
	public SpriteRenderer _CursorSprite;
	public float _DefaultDelay = 0.1f; //delay until cursor resets to default; seemed more relyable than 'hover off' events

	[Header("Cursor Presets")]
	public CursorPreset[] _CursorPresets;
	private Dictionary<string, CursorPreset> _CursorPresetsDic;

	private Transform _Camera;
	private Vector3 _TargetPos; 
	private Vector3 _TargetNorm;
	private Vector3 _OriginalScale;
	private float _LastChangeTime = 0f;
	private string _CurrentCursorState = "default";
	private bool _IsActive = true;


	void Start () {
		_OriginalScale = transform.localScale;
		_Camera = GameObject.FindGameObjectWithTag ("MainCamera").transform;

		_CursorPresetsDic = new Dictionary<string,CursorPreset>();
		foreach (CursorPreset CP in _CursorPresets) {
			_CursorPresetsDic.Add(CP.name, CP);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
		//place cursor
		if (_IsActive) {
			//position
			if (RayCast.HitObject) {
				transform.position = RayCast.Hitpoint;
				//Debug.Log ("rayhit: "+RayCast.Hitpoint+", position: "+transform.position);
			} else {
				transform.position =  _Camera.position;
				transform.position += _Camera.forward * _AtInfiniteDistance;
			}

			//resize
			if (_UniformSize) {
				float dist = Vector3.Distance (_Camera.position, transform.position);
				float factor = dist / _NeutralDistance;
				transform.localScale = _OriginalScale * factor;
			}
		}

		//orientation
		switch (_CurrentCursorState) {

		case "XZ":
			transform.forward = Vector3.up;
			break;
		
		case "Y":
			transform.LookAt (_Camera);
			transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, transform.eulerAngles.z);
			break;

		case "norm":
			transform.forward = RayCast.Hitnormal;
			break;

		case "cam":
			transform.LookAt (_Camera);
			break;

		default:
			if (_ToNormal) {
				transform.forward = RayCast.Hitnormal;
			} else {
				transform.LookAt(_Camera);
			}
			break;
		}

		//revert to default after timedelay
		if ((Time.time - _LastChangeTime) > _DefaultDelay)
			SetCursor ("default");
	}

	public void SetCursor(string name){
		CursorPreset CP = null;
		if (_CursorPresetsDic.TryGetValue (name, out CP)) {

			_CurrentCursorState = CP.orientation;
			_CursorSprite.sprite = CP.sprite;
			_CursorSprite.color = CP.color;

			_LastChangeTime = Time.time;
		}
	}

	public void Lock(Transform parent){
		if (_IsActive) {
			transform.gameObject.transform.parent = parent;
			_IsActive = false;
		}
	}

	public void UnLock(){
		if (!_IsActive) {
			transform.parent = null;
			_IsActive = true;
			transform.localScale = _OriginalScale;
		}	
	}
}

[System.Serializable]
public class CursorPreset{
	public string name;
	public Sprite sprite;
	public Color color;
	public string orientation;
}