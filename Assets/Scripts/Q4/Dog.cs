using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Dog : MonoBehaviour {

	public List<VerletPoint> verletPoints;
	public List<VerletStick> verletSticks;

	public VerletPoint verletPointPrefab;
	public VerletStick verletStickPrefab;

	public float dogSize;

	//int constraintintAcc = 3;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

		RefreshPhysicsAll();	
		CheckInBoundsAll();

		for(int i =0; i < 10; i++) //solve constraints multiple times so all constraints are met nicely
		{
			SatisfyStickConstraintsAll();
		}

	}

	public void setVelocityAll(Vector3 vel)
	{
		foreach (VerletPoint v in verletPoints)
		{
			v.Velocity = vel;
			
			//Debug.Log (v.name + " velocity is: " + v.Velocity);
		}	
	}


	VerletPoint create(float x, float y, string name)
	{
		VerletPoint newPoint = Instantiate(verletPointPrefab, new Vector3(dogSize * x + transform.position.x, dogSize * y + transform.position.y, 0), this.transform.rotation) as VerletPoint;
		newPoint.name = name;
		newPoint.transform.parent = transform;
		verletPoints.Add(newPoint);
		//verletPoints.Add (name, newPoint);
		return newPoint;
	}
	//RefreshPhysics();
	//CheckInBounds();

	VerletStick createLine(VerletPoint a, VerletPoint b) //creates a default visible line
	{	
		float lineWidth = .5f;

		//a.transform.position = a.transform.position * .5f;//.5 is to scale down
		//b.transform.position = b.transform.position * .5f;
		
		Vector3 inputPosA = a.transform.position;
		Vector3 inputPosB = b.transform.position;

			
		
		Vector3 posC = ( ( inputPosB - inputPosA ) * 0.5f ) + inputPosA; 
			float lengthC = ( inputPosB - inputPosA ).magnitude; 
			float sineC  = ( inputPosB.y - inputPosA.y ) / lengthC; 
			float angleC = Mathf.Asin( sineC ) * Mathf.Rad2Deg; 
			if (inputPosB.x < inputPosA.x) {angleC = 0 - angleC;} 
			
			//Debug.Log( "inputPosA" + inputPosA + " : inputPosB" + inputPosB + " : posC" + posC + " : lengthC " + lengthC + " : sineC " + sineC + " : angleC " + angleC );
			
			VerletStick myLine = Instantiate( verletStickPrefab, posC, Quaternion.identity ) as VerletStick; 

			verletSticks.Add(myLine); //adds to the list

			
			myLine.pointa = a;
			myLine.pointb = b;
		
			//myLine.transform.localScale = new Vector3(lengthC, lineWidth, lineWidth); 
			//myLine.transform.rotation = Quaternion.Euler(0, 0, angleC);
		
			myLine.transform.parent = transform;

			myLine.StartUp();
			//Debug.Log ("YAY: "+a.transform.position + ",  "+ b.transform.position);
			return myLine;
	}


	public void RefreshPhysicsAll()
	{
		//Debug.Log (verletPoints.Count);

		List<VerletPoint> pointsToRemove = new List<VerletPoint>();

		foreach (VerletPoint v in verletPoints)
		{
			if( v != null)
			{
				v.RefreshPhysics();

				//Debug.Log (v.name + " velocity is: " + v.Velocity);
			}
			else pointsToRemove.Add(v);
		}

		foreach (VerletPoint rem in pointsToRemove)
		{
			verletPoints.Remove(rem);
		}
	} 

	public void CheckInBoundsAll()
	{
		
		List<VerletPoint> pointsToRemove = new List<VerletPoint>();
		List<VerletStick> sticksToRemove = new List<VerletStick>();

		foreach (VerletPoint v in verletPoints)
		{
			
			if(!v.CheckInBounds()) //now destroy dog and all of its components
			{
				Destroy (gameObject); //destroy dog..
				
				foreach (VerletPoint ver in verletPoints)
				{
					Destroy (ver.gameObject);
					pointsToRemove.Add(ver);
				}
	

				foreach (VerletStick vstick in verletSticks)
				{
					Destroy (vstick.gameObject);
					sticksToRemove.Add (vstick);
				}

				//Debug.Log ("BOOM");
				break;
			}

		}	

		foreach (VerletPoint rem in pointsToRemove)
		{
			verletPoints.Remove(rem);
		}

		foreach (VerletStick remstick in sticksToRemove)
		{
			verletSticks.Remove(remstick);
		}
	}

	public void SatisfyStickConstraintsAll()
	{
		//Debug.Log (verletPoints.Count);
		
		List<VerletStick> sticksToRemove = new List<VerletStick>();
		
		foreach (VerletStick v in verletSticks)
		{
			if( v != null)
			{
				v.contract();		
				//Debug.Log (v.name + " velocity is: " + v.Velocity);
			}
			else sticksToRemove.Add(v);
		}
		
		foreach (VerletStick rem in sticksToRemove)
		{
			verletSticks.Remove(rem);
		}
	}

	public void InstantiateModel()
	{
		dogSize = .5f;
		
		verletPoints = new List<VerletPoint>();
		//creates a point subobject

		//0-4
		create(.1f, 2.95f, "BADEye").transform.renderer.enabled = false; //this is bad........ ATTACH TO THINGS LIKE REAL EYE turn this crap off...

		create (.5f, 1.5f, "BodyBottomLeft");
		create (2, 2.5f, "BodyTopRight");
		create (2, 1.5f, "BodyBottomRight");
		create (.5f, 2.5f, "BodyTopLeft");	
	
		//5-9
		create (.3f, 2.8f, "HeadBottomRight");
		create (0, 2.8f, "HeadBottomLeft");		
		create (.3f, 3, "HeadTopRight");
		create (0, 3, "HeadTopLeft");

		create (.1f, 2.95f, "HeadEye");

		//10-13
		create (.75f, 1.5f, "FrontBodyConnect");
		create (.75f, 1, "FrontKnee");
		create (.75f, .5f, "FrontHeel");
		create (.5f, .5f, "FrontToe");
		
		//14-17
		create (1.75f, 1.5f, "BackBodyConnect");
		create (1.75f, 1, "BackKnee");
		create (1.75f, .5f, "BackHeel");
		create (1.5f, .5f, "BackToe");
		
		//18
		create (2.5f, 2.7f, "Tail");
		
		//create lines now!


		//head: 
		createLine(verletPoints[7], verletPoints[8]);
		createLine(verletPoints[5], verletPoints[7]);
		createLine(verletPoints[5], verletPoints[6]);
		createLine(verletPoints[9], verletPoints[8]).removeLineRenderer(); //good eye
		createLine(verletPoints[6], verletPoints[8]);

		//body: 
		createLine(verletPoints[2], verletPoints[4]); //spine
		createLine(verletPoints[1], verletPoints[4]); //left side
		createLine(verletPoints[2], verletPoints[3]); //right side
		createLine(verletPoints[1], verletPoints[10]); //bodyfront to bodyconnect
		createLine(verletPoints[10], verletPoints[14]); //bodyconnect to bodyconnect
		createLine(verletPoints[14], verletPoints[3]); //bodyconnect to bodyback

		createLine(verletPoints[1], verletPoints[2]); //left bottom to right bottom for le protections
		createLine(verletPoints[4], verletPoints[3]); //even more le protections lewllll
		

		//neck:
		createLine(verletPoints[5], verletPoints[4]);

		//tail:
		createLine(verletPoints[18], verletPoints[2]);

		//front legs beyotch!
		createLine(verletPoints[10], verletPoints[11]);
		createLine(verletPoints[11], verletPoints[12]);
		createLine(verletPoints[12], verletPoints[13]);

		//back legs beyotch!
		createLine(verletPoints[14], verletPoints[15]);
		createLine(verletPoints[15], verletPoints[16]);
		createLine(verletPoints[16], verletPoints[17]);

		//createLine (verletPoints[0], verletPoints[8]); //eye!

		//now make sure dog doesn't collapse LOOOOL

		//BODY FRAME: 
		VerletStick frameUno = createLine(verletPoints[4], verletPoints[10]);
		frameUno.removeLineRenderer();
		//frameUno.GetComponent<LineRenderer>.isVisible = false;

		VerletStick frameDos = createLine(verletPoints[2], verletPoints[14]);
		frameDos.removeLineRenderer();
		//frameDos.visible = false;

		//left to right cross
		VerletStick frameTres = createLine(verletPoints[4], verletPoints[14]);
		frameTres.removeLineRenderer();
		VerletStick frameQuatro = createLine(verletPoints[2], verletPoints[10] );
		frameQuatro.removeLineRenderer();
		//HEAD FRAME:

		VerletStick headFrameUno = createLine(verletPoints[8], verletPoints[5]);		
		headFrameUno.removeLineRenderer();
		VerletStick headFrameDos = createLine (verletPoints[6], verletPoints[7]); //TOPRIGHT TO BOTTOMLEFT
		headFrameDos.removeLineRenderer();

		//Eye Frame:
		
		VerletStick eyeFrame = createLine(verletPoints[9], verletPoints[6]);
		eyeFrame.removeLineRenderer();
		
		VerletStick e1 = createLine(verletPoints[9], verletPoints[7]);
		e1.removeLineRenderer();
		VerletStick e2 = createLine(verletPoints[9], verletPoints[5]);
		e2.removeLineRenderer();
	}
	
}
