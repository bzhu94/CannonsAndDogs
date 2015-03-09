using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Dog : MonoBehaviour {

	public Hashtable verletPoints;
	public List<VerletStick> verletSticks;

	public VerletPoint verletPointPrefab;
	public VerletStick verletStickPrefab;

	public float dogSize;

	//int constraintintAcc = 3;
	// Use this for initialization
	void Start () {
		verletPoints = new Hashtable();
		verletSticks = new List<VerletStick>();
	}
	
	// Update is called once per frame
	void Update () {
		//this.
		
	}

	void create(float x, float y, string name)
	{
		VerletPoint newPoint = Instantiate(verletPointPrefab, new Vector3(dogSize * x + transform.position.x, dogSize * y + transform.position.y, 0), this.transform.rotation) as VerletPoint;
		newPoint.name = name;
		newPoint.transform.parent = transform;
		verletPoints.Add (name, newPoint);
	}
	
	public void InstantiateModel()
	{
		dogSize = .5f;
		
		verletPoints = new Hashtable();
		//creates a point subobject
		create(0, 3, "Eye");
		create (.5f, 1.5f, "BodyBottomLeft");
		create (2, 2.5f, "BodyTopRight");
		create (2, 1.5f, "BodyBottomRight");
		create (.5f, 2.5f, "BodyTopLeft");	
	
		create (.3f, 2.8f, "HeadBottomRight");
		create (0, 2.8f, "HeadBottomLeft");		
		create (.3f, 3, "HeadTopRight");
		create (0, 3, "HeadTopLeft");
		create (.1f, 2.95f, "HeadEye");

		create (.75f, 1.5f, "FrontBodyConnect");
		create (.75f, 1, "FrontKnee");
		create (.75f, .5f, "FrontHeel");
		create (.5f, .5f, "FrontToe");

		create (1.75f, 1.5f, "BackBodyConnect");
		create (1.75f, 1, "BackKnee");
		create (1.75f, .5f, "BackHeel");
		create (1.5f, .5f, "BackToe");

		create (2.5f, 2.7f, "Tail");
		
		//create ()
	}

	void InstantiateLists(GameObject obj) { 
		//Debug.Log(obj.name); 
		foreach (Transform child in obj.transform) 
		{
			
			Debug.Log (child);
			//if(child.GetType)
			//InstantiateLists(child);
		}
		
	}
}
