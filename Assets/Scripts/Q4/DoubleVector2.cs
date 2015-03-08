using UnityEngine;
using System.Collections;

public class DoubleVector2 : MonoBehaviour {

	public double x;
	public double y;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void setPos(double x, double y)
	{
		this.x = x;
		this.y = y;
	}
}
