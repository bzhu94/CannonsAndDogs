using UnityEngine;
using System.Collections;

public class CannonBall : MonoBehaviour {

	//public AudioClip coin;
	public Vector3 Acceleration;
	public Vector3 Velocity;
	public Vector3 OldVelocity; 
	public static float Radius;

	public LineRenderer lr;
	// Use this for initialization
	void Start () {
		Acceleration = PhysicsEngine.Gravity;
		Radius = .09f;
		//gameObject.audio.enabled = true;
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
		
		//Debug.Log ("Outside" + (OldVelocity + Velocity) * .5f * Time.deltaTime);
		
		CheckCollision();

		//Debug.Log ("Before: "+ transform.position + " vel: " + (OldVelocity + Velocity) * .5f);
		transform.position += (OldVelocity + Velocity) * .5f * Time.deltaTime;
		//Debug.Log ("After: " + transform.position);

	}

	void CheckInBounds() //if out, destroy
	{
		if(transform.position.x < 0 || transform.position.x > 6 || transform.position.y < 2.7) 
		Destroy(gameObject); //if goes out of left bounds of canyon
	}

	void CheckCollision()
	{

		//find the forward ray position of the predicted translated object's point in the velocity direction
		GameObject forwardCenter =  new GameObject();
		forwardCenter.transform.position = new Vector3((OldVelocity.x + Velocity.x) * .5f * Time.deltaTime + transform.position.x, (OldVelocity.y + Velocity.y) * .5f * Time.deltaTime + transform.position.y, 0);
		//Debug.Log ("Center: "+ forwardCenter.transform.position);
		
		GameObject projection = new GameObject();

		projection.transform.position = new Vector3((OldVelocity.x + Velocity.x) * .5f, (OldVelocity.y + Velocity.y) * .5f, 0); //first find the direction 			
		
		RaycastHit hit;
		Ray r = new Ray(forwardCenter.transform.position, projection.transform.position);
		//Debug.DrawRay(forwardCenter.transform.position, projection.transform.position * Radius);
		if(Physics.Raycast(r, out hit, Radius))
		{
			//Debug.Log ("HIT: "+hit.collider.gameObject.name);
			
			Vector3 norm = hit.normal;

			Vector3 reflected = Velocity - (Vector3.Dot (2f * Velocity, norm) / Mathf.Pow(norm.magnitude, 2)) * norm;

			reflected = reflected * PhysicsEngine.bounce;


			if((hit.transform.parent.name == "CanyonLeft" && reflected.x < 0) || (hit.transform.parent.name == "CanyonRight" && reflected.x > 0)) //so if hits a funny angle, make it not be funny LOL
			{
				reflected.x *= -1;
				reflected.y = Mathf.Abs(reflected.y);
			}

			Velocity = reflected; //now set these equal to new bounced off
			OldVelocity = reflected; //now set these equal to new bounced off
		}

		Destroy(forwardCenter);
		Destroy (projection);

		//MAKE SURE TO DELETE THESE GAMEOBJECTS TO SAVE SPACE!!!!
	}


}
