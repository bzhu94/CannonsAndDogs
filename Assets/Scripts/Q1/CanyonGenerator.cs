using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CanyonGenerator : MonoBehaviour {

	public GameObject platformPrefab;
	public int terrainWidth; // How many vertices wide the terrain will be

	public float platformWidth;
	
	public float terrainWorldWidth; //How long terrain is in world length
	//public float resolution; // How granular the terrain will be the higher
	// the number, the closer together points will be
	
	public float roughness; // How rough the terrain will be
	
	public float startHeight; // How high the leftmost vertex will be
	
	public float endHeight; // How high the rightmost vertex will be
	
	public float xPos;
	public float yPos;

	public float[] heightmap;

	public Transform Cannon;

	public List<GameObject> lines;
	// Use this for initialization
	void Awake () {
		
		xPos = transform.position.x;
		yPos = transform.position.y;
		
		GenerateCanyonSegment();
		//PositionCannon();

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	float[] GenerateHeightMap(float startHeight, float endHeight, int count) {
		//Debug.Log("Generating heightmap"); 
		// Create a heightmap array and set the start and endpoints
		float[] heightmap = new float[count + 1];
		heightmap[0] = startHeight;
		heightmap[heightmap.Length - 1] = endHeight;
		
		// Call the recursive function to generate the heightmap
		GenerateMidPoint(0, heightmap.Length - 1, roughness, heightmap);
		//Debug.Log("Heightmap complete");
		return heightmap;
	}
	
	void GenerateMidPoint(int start, int end, float roughness, float[] heightmap) {
		// Find the midpoint of the array for this step
		int midPoint = (int)Mathf.Floor((start + end) / 2);
		
		if (midPoint != start) {
			// Find the mid height for this step
			var midHeight = (heightmap[start] + heightmap[end]) / 2;
			
			// Generate a new displacement between the roughness factor
			heightmap[midPoint] = midHeight + Random.Range(-roughness, roughness);
			
			// Repeat the process for the left side and right side of
			// the new mid point
			GenerateMidPoint(start, midPoint, roughness / 2, heightmap);
			GenerateMidPoint(midPoint, end, roughness / 2, heightmap);
		}
	}

	public void DrawLine (GameObject myCube, Vector3 inputPosA, Vector3 inputPosB, float lineWidth, List<GameObject> lines) {

		/*
		// calculate vector from point A to point B
		Vector3 lineVector = to - from;
		float dist = lineVector.magnitude;
		
		// instantiate line
		GameObject lineInstance = Instantiate(linePrefab) as GameObject;
		
		// set position of line instance to center of line
		lineInstance.transform.position = from + (lineVector / 2f);
		lineInstance.transform.localScale = new Vector3(dist, .1f, .1f);
		// set rotation of line instance so that it's Y axis is from point A to point B
		//lineInstance.transform.localScale

		*/
	
			Vector3 posC = ( ( inputPosB - inputPosA ) * 0.5f ) + inputPosA; 
			float lengthC = ( inputPosB - inputPosA ).magnitude; 
			float sineC  = ( inputPosB.y - inputPosA.y ) / lengthC; 
			float angleC = Mathf.Asin( sineC ) * Mathf.Rad2Deg; 
			if (inputPosB.x < inputPosA.x) {angleC = 0 - angleC;} 
			
			//Debug.Log( "inputPosA" + inputPosA + " : inputPosB" + inputPosB + " : posC" + posC + " : lengthC " + lengthC + " : sineC " + sineC + " : angleC " + angleC );
			
			GameObject myLine = Instantiate( myCube, posC, Quaternion.identity ) as GameObject; 

			lines.Add(myLine); //adds to the list

			myLine.transform.localScale = new Vector3(lengthC*2f, lineWidth*1.3f, lineWidth); 
			myLine.transform.rotation = Quaternion.Euler(0, 0, angleC);

			myLine.transform.parent = transform;
	}

	public void GenerateCanyonSegment() //the "main method"
	{
		// Generate the heightmap
		heightmap = GenerateHeightMap(startHeight, endHeight, terrainWidth);
		
		//in this loop, create pairs of transforms, then create a line that joins these transforms.
		for(int i =0; i < heightmap.Length -1; i++)
		{
			GameObject left = new GameObject();
			GameObject right = new GameObject();

			left.transform.position = new Vector3(i * (terrainWorldWidth/terrainWidth) + xPos,heightmap[i] + yPos,0);
			right.transform.position = new Vector3((i+1) * (terrainWorldWidth/terrainWidth) + xPos,heightmap[i+1] + yPos,0);

			//creates a line out of these 2 points
			DrawLine(platformPrefab, left.transform.position, right.transform.position, platformWidth, lines);

			Destroy(left);
			Destroy (right);
			
		}

	}

	public void PositionCannon()
	{
		if(Cannon != null)
		{
			 //Debug.Log("Before: "+Cannon.position);	
			 lines[lines.Count/3].name = "Cannon Position";
			 Cannon.position = lines[lines.Count/3].transform.position;
			 //Debug.Log("After: "+Cannon.position);
			
		}
	}


}
