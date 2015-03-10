using UnityEngine;
using System.Collections;

public class ArrowWind : MonoBehaviour {

	public Transform LeftPos;
	public Transform RightPos;
	public GameObject cone;

	//by default, cone will point left

	// Use this for initialization
	void Start () {
		InvokeRepeating("changeWind", 0, .5f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void changeWind()
	{
		Vector3 pastPos = transform.position;
		transform.position = new Vector3(0, 0, 0);
		
		float WindForceX = PhysicsEngine.WindForce.x;
		transform.localScale = new Vector3(transform.localScale.x, WindForceX * .5f, transform.localScale.z);
		transform.position = pastPos;	

		cone.transform.position = LeftPos.position;
	}
}
