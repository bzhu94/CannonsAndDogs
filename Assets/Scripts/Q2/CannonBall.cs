using UnityEngine;
using System.Collections;

public class CannonBall : MonoBehaviour {

	public Vector3 Acceleration;
	public Vector3 Velocity;
	public Vector3 OldVelocity; 
	public float Radius;

	public LineRenderer lr;
	// Use this for initialization
	void Start () {
		Acceleration = PhysicsEngine.Gravity;
		Radius = .09f;
	}
	
	// Update is called once per frame
	void Update () {
		RefreshPhysics();
		CheckInBounds();

	}

	void RefreshPhysics() //changes velocity, acceleration, position in accordance to verlet integration
	{
		//WindForce = new Vector3(Random.Range(-.5f, .5f), 0, 0);
		//Debug.Log (WindForce);
		Vector3 OldVelocity = new Vector3(Velocity.x, Velocity.y, Velocity.z);
		
		Acceleration += PhysicsEngine.WindForce * Time.deltaTime;
		
		Velocity += Acceleration * Time.deltaTime;
		
		Velocity = new Vector3(PhysicsEngine.WindResistance * Velocity.x, PhysicsEngine.WindResistance * Velocity.y, 0); //account in  wind resistance
		
		Debug.Log ("Outside" + (OldVelocity + Velocity) * .5f * Time.deltaTime);
		
		CheckCollision();


		transform.position += (OldVelocity + Velocity) * .5f * Time.deltaTime;
	}

	void CheckInBounds()
	{
		if(transform.position.x < .5 || transform.position.x > 5.7 || transform.position.y < 2.8) 
		Destroy(gameObject); //if goes out of left bounds of canyon
	}

	void CheckCollision()
	{

		//find the forward ray position of the predicted translated object's point in the velocity direction
		GameObject forwardCenter =  new GameObject();
		forwardCenter.transform.position = new Vector3((OldVelocity.x + Velocity.x) * .5f * Time.deltaTime + transform.position.x, (OldVelocity.y + Velocity.y) * .5f * Time.deltaTime + transform.position.y, 0);
		Debug.Log ("Center: "+ forwardCenter.transform.position);
		
		GameObject projection = new GameObject();

		projection.transform.position = new Vector3((OldVelocity.x + Velocity.x) * .5f, (OldVelocity.y + Velocity.y) * .5f, 0); //first find the direction 	
		Debug.Log ("Before: "+projection.transform.position);
		projection.transform.position.Normalize(); //now Normalize this direction Vector
		Debug.Log ("Normalized: " + projection.transform.position);
		Debug.Log ("Magnitude: "+ projection.transform.position.magnitude);
		projection.transform.position = new Vector3(projection.transform.position.x * Radius, projection.transform.position.y * Radius, 0); //now the direction vector is of length radius

		Debug.Log ("Radius Direction Vector: " + projection.transform.position);
		projection.transform.position = new Vector3(projection.transform.position.x + forwardCenter.transform.position.x, projection.transform.position.y + forwardCenter.transform.position.y, 0); //now translate to actual coordinates
		

		//FOR DEBUGGING: DRAW LINE!

		//Debug.Log ("Center: "+ forwardCenter.transform.position + ",  Projection: "+ projection.transform.position);
		/*
		lr = GetComponent<LineRenderer>();
		lr.SetWidth (.05f, .05f);
		lr.SetPosition(0, forwardCenter.transform.position);
		lr.SetPosition(1, projection.transform.position);
		*/

		RaycastHit hit;
		Ray r = new Ray(forwardCenter.transform.position, projection.transform.position);
		if(Physics.Raycast(r, out hit, Radius))
		{
			Debug.Log ("HIT!");
		}

		Destroy(forwardCenter);
		Destroy (projection);

		//MAKE SURE TO DELETE THESE GAMEOBJECTS TO SAVE SPACE!!!!
	}


}
