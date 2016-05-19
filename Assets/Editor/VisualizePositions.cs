using UnityEngine;
using UnityEditor;
using System.Collections;

public class VisualizePositions : MonoBehaviour {
	public GameObject gazeLinePrefab;
	public GameObject positionLinePrefab;

	public bool visualizeSingleWalkthrough;
	public TextAsset data;
	//public GameObject positionLineObject;
	//public GameObject gazeLineObject;
	public bool drawPositionsLines = true;
	public bool drawGazeLines = true;
	public bool drawGazePoints = false;
	public GameObject pointPrefab;

	private LineRenderer positionLine;
	private LineRenderer gazeLine;

	// Use this for initialization
	void Start () 
	{
		if (visualizeSingleWalkthrough) {
			DrawData (data);
		} else {
			string[] dataGUID = AssetDatabase.FindAssets("data_ t:TextAsset");
			TextAsset[] dataFiles = new TextAsset[dataGUID.Length];
			Debug.Log(dataGUID.Length);
			for (int i =0; i < dataGUID.Length; i++) 
			{
				if( !(AssetDatabase.GUIDToAssetPath(dataGUID[i]).Contains("Data"))){
					return;
				}
				TextAsset dataFile = (TextAsset) AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(dataGUID[i]), typeof(TextAsset));
				//dataFiles[i] = dataFile;
				//string[] lines = dataFile.text.Split("\n"[0]);
				//Debug.Log(lines[0]);
				//Debug.Log(i+": "+dataGUID[i]);
				//Debug.Log(i+": "+AssetDatabase.GUIDToAssetPath(dataGUID[i]));
				DrawData(dataFile);
			}
		}
	}

	void DrawData(TextAsset data)
	{
		//instantiate and get the linerenderers
		GameObject gazeLineObject = (GameObject) Instantiate(gazeLinePrefab, new Vector3(0f,0f,0f), Quaternion.identity);
		GameObject positionLineObject = (GameObject) Instantiate(positionLinePrefab, new Vector3(0f,0f,0f), Quaternion.identity);
		LineRenderer gazeLineRenderer = gazeLineObject.GetComponent<LineRenderer> ();
		LineRenderer positionLineRenderer = positionLineObject.GetComponent<LineRenderer> ();


		Debug.Log ("drew a dataline");
		//parse data
		string[] lines = data.text.Split("\n"[0]);

		//set n° points in lines
		positionLineRenderer.SetVertexCount(lines.Length - 1);
		gazeLineRenderer.SetVertexCount((lines.Length - 1)*3);

		//draw each point
		for (int i =0; i < lines.Length; i++) 
		{
			Debug.Log (i+": "+lines[i]);
			if(lines[i] == "" || lines[i] == null)
			{
				return;
			}

			string[] positionDataStrings = lines[i].Split(',');
			float[] positionData = new float[positionDataStrings.Length];
			//convert data from string to float
			for(int j=0; j < positionDataStrings.Length; j++)
			{
				float f;
				if(float.TryParse(positionDataStrings[j], out f)){
					positionData[j] = f;
				} else{
					positionData[j] = 0f;
				}
			}

			Vector3 posP = new Vector3(positionData[0], positionData[1], positionData[2]);//player position
			Vector3 posG = new Vector3(positionData[3], positionData[4], positionData[5]);//player gaze
			//Vector3 posP = new Vector3(float.TryParse(positionData[0]), float.TryParse(positionData[1]), float.TryParse(positionData[2]));//player position
			//Vector3 posG = new Vector3(float.Parse(positionData[3]), float.Parse(positionData[4]), float.Parse(positionData[5]));//player gaze
			
			if(drawPositionsLines)
			{
				positionLineRenderer.SetPosition(i, posP);
			}
			
			if(drawGazeLines)
			{
				gazeLineRenderer.SetPosition(3*i, posP);
				gazeLineRenderer.SetPosition(3*i+1, posG);
				gazeLineRenderer.SetPosition(3*i+2, posP);
			}
			
			if(drawGazePoints)
			{
				Instantiate(pointPrefab, new Vector3(positionData[3],positionData[4], positionData[5]), Quaternion.identity);
				//Instantiate(pointPrefab, new Vector3(float.Parse(positionData[3]),float.Parse(positionData[4]), float.Parse(positionData[5])), Quaternion.identity);
			}

		}
	}
}
