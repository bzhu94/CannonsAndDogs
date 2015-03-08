using UnityEngine;
using System.Collections;

public class PhysicsEngine : MonoBehaviour {

	public static Vector3 WindForce;
	public static float WindResistance;
	public static Vector3 Gravity;
	public static float ForceValue;
	// Use this for initialization
	void Start () {
		Gravity = new Vector3 (0, -5, 0);
		WindResistance = .99f;
		//WindResistance = 1f; //no wr
		ForceValue = 4;
		//ForceValue = 0; //no WindForce
	}
	
	// Update is called once per frame
	void Update () {
		InvokeRepeating("ChangeWindForce", .5f, .5f); //Change every half a second
	}

	void ChangeWindForce()
	{
		WindForce = new Vector3(Random.Range(-ForceValue, ForceValue), 0, 0); //Change force
		//Debug.Log ("Wind force is now: " + WindForce);
	}
}
