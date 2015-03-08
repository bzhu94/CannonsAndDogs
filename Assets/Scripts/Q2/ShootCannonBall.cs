using UnityEngine;
using System.Collections;

public class ShootCannonBall : MonoBehaviour {

	public CannonBall cannonBallPrefab;
	public Transform cannonEnd;

	public static float initVelocity;
	// Use this for initialization
	void Start () {
		initVelocity = 5;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("space"))
		{
			Shoot();
			//Debug.Log ("Space pressed");
		}
		

	}
	
	//3 is lowest speed
	//5 is the medium speed. 
	//7 is highest speed.
	void Shoot()
	{
		initVelocity = Random.Range(3.0f, 7.0f);
		int Angle = Random.Range (15, 60);
		transform.eulerAngles = new Vector3(0, 0, -1 * Angle); //move cannon to new angle
		CannonBall cannonBall = Instantiate (cannonBallPrefab, cannonEnd.position, cannonEnd.rotation) as CannonBall; //create a new cannonball

		cannonBall.Velocity = new Vector3( -1 * initVelocity * Mathf.Cos((Angle * Mathf.PI)/180), initVelocity * Mathf.Sin((Angle * Mathf.PI)/180) , 0); //set cannonball initial velocity
		
	}

}
