using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VerletPoint: MonoBehaviour {

	public DoubleVector2 pos;
	public DoubleVector2 oldpos;
	public List<VerletStick> sticks;

	// Use this for initialization
	void Awake () {
		setPos(transform.position.x, transform.position.y);
		//Debug.Log("x,y: "+transform.position.x + ", " +transform.position.y);
		sticks = new List<VerletStick>();
	}

	
	// Update is called once per frame
	void Update () {
		setPos(transform.position.x, transform.position.y);
		//refresh ();
	}


	public void setPos(double x, double y )
	{	
		pos.setPos(x, y);
	}

	public void setOldPos(double x, double y)
	{
		oldpos.setPos(x, y);
	}

	public double getX()
	{
		return pos.x;
	}

	public double getY()
	{
		return pos.y;	
	}

	public double getOldX()
	{
		return oldpos.x;
	}

	public double getOldY()
	{
		return oldpos.y;
	}

	public void solveConstraints()
	{
		foreach (VerletStick v in sticks)
		{
			v.contract();
		}


	}
	
	public void refresh()
	{
		double tempx = pos.x;
		double tempy = pos.y;
		
		pos.x += pos.x - oldpos.x;
		pos.y += pos.y - oldpos.y;
		oldpos.x  = tempx;
		oldpos.y = tempy;
		
		Vector3 tempPos = transform.position;
		tempPos.x = (float)pos.x;
		tempPos.y = (float)pos.y;
		transform.position = tempPos;
	}


}
