using UnityEngine;
using System.Collections;

public class ShootDog : MonoBehaviour {

	public Dog DogPrefab;
	public Transform cannonEnd;
	
	public static float initVelocity;
	// Use this for initialization
	void Start () {
		initVelocity = 5;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("tab"))
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
		initVelocity = Random.Range(3.0f, 6.0f);
		int Angle = Random.Range (-15, -60);
		transform.eulerAngles = new Vector3(0, 0, -1 * Angle); //move cannon to new angle
		Dog dog = Instantiate (DogPrefab, cannonEnd.position, cannonEnd.rotation) as Dog; //create a new cannonball
		dog.InstantiateModel();
		dog.transform.localScale = new Vector3(.5f, .5f, .5f);
		//foreach ()
		dog.setVelocityAll(new Vector3(initVelocity * Mathf.Cos((Angle * Mathf.PI)/180), -1 * initVelocity * Mathf.Sin((Angle * Mathf.PI)/180) , 0)); //set cannonball initial velocity
		
	}
}	