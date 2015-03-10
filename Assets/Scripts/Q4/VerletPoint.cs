using UnityEngine;
using System.Collections;

public class VerletPoint : MonoBehaviour {

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
		//Velocity = new Vector3(0, 0, 0);
	}
	
	// Update is called once per frame
	void Update () {
		//RefreshPhysics();
		//CheckInBounds();
		
		
	}
	
	public void RefreshPhysics() //changes velocity, acceleration, position in accordance to verlet integration
	{
		//WindForce = new Vector3(Random.Range(-.5f, .5f), 0, 0);
		//Debug.Log (WindForce);
		Vector3 OldVelocity = new Vector3(Velocity.x, Velocity.y, Velocity.z);
		
		Acceleration += PhysicsEngine.WindForce * Time.deltaTime;
		
		Velocity += Acceleration * Time.deltaTime;
		
		Velocity = new Vector3(PhysicsEngine.WindResistance * Velocity.x, PhysicsEngine.WindResistance * Velocity.y, 0); //account in  wind resistance
		
		//Debug.Log ("Outside" + (OldVelocity + Velocity) * .5f * Time.deltaTime);
		
		CheckCollision();
		
		CheckCannonBallCollision();
		
		transform.position += (OldVelocity + Velocity) * .5f * Time.deltaTime;
		
	}
	
	public bool CheckInBounds() //if out, destroy
	{
		if(transform.position.x < 0 || transform.position.x > 6 || transform.position.y < 2.7) 
		{
			//Debug.Log ("Destroyed: " + transform.position);
			Destroy(gameObject); //if goes out of left bounds of canyon
			return false;
		}
		else return true;
	}
	
	public void CheckCollision()
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

			if(hit.transform.parent.name == "Bottom") 
			{
				Destroy(gameObject);
			}

			
			Velocity = reflected; //now set these equal to new bounced off
			OldVelocity = reflected; //now set these equal to new bounced off
		}
		
		Destroy(forwardCenter);
		Destroy (projection);
		
		//MAKE SURE TO DELETE THESE GAMEOBJECTS TO SAVE SPACE!!!!
	}

	public bool CheckCannonBallCollision()
	{
		foreach(GameObject cannonBall in GameObject.FindGameObjectsWithTag("Cannon"))
		{
			Vector3 ballPos = cannonBall.transform.position;

			Vector3 forwardCenter = new Vector3((OldVelocity.x + Velocity.x) * .5f * Time.deltaTime + transform.position.x, (OldVelocity.y + Velocity.y) * .5f * Time.deltaTime + transform.position.y, 0);
			//Debug.Log ("Center: "+ forwardCenter.transform.position);
			
			Vector3 velocityDir =  new Vector3((OldVelocity.x + Velocity.x) * .5f * Time.deltaTime, (OldVelocity.y + Velocity.y) * .5f * Time.deltaTime, 0);
			velocityDir.Normalize();
			
			Vector3 projectionPoint = new Vector3(forwardCenter.x + velocityDir.x * Radius, forwardCenter.y + velocityDir.y * Radius, 0); //first find the direction 	

			float x = projectionPoint.x;
			float y = projectionPoint.y;
			
			if(Mathf.Pow(x-ballPos.x, 2) + Mathf.Pow(y-ballPos.y, 2) < CannonBall.Radius)
			{
				//Debug.Log ("OW!");
				Velocity  *= -1;
				return true;
			}
			
		}

		return false;
	}

	
}
