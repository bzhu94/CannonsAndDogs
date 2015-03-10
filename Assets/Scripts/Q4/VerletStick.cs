using UnityEngine;
using System.Collections;

public class VerletStick : MonoBehaviour {

	public VerletPoint pointa;
	public VerletPoint pointb;
	public float distConstraint;

	public LineRenderer lr;

	public bool visible;
	// Use this for initialization

	void Start () {

		lr = GetComponent<LineRenderer>();	
		lr.SetWidth(.005f, .005f);

	}
	
	// Update is called once per frame
	void Update () {	
		if(visible == true && pointa != null && pointb != null)
		setPosLR();		
	}

	public void StartUp()
	{
		visible = true;
		Vector3 dist = pointa.transform.position - pointb.transform.position;
		
		distConstraint = dist.magnitude * .5f;
		//Debug.Log ("distConstraint is: "+distConstraint);
	}

	public void removeLineRenderer()
	{
		visible = false;
		lr.SetVertexCount(0);
	}


	public void contract()
	{
		if(pointa != null && pointb != null)
		{
			
			Vector3 dist = pointa.transform.position - pointb.transform.position;
			
			float h = dist.magnitude; //dist of a and b, w/o contraction
			float diff = h - distConstraint;
			
			dist.Normalize();
			
			//Debug.Log (dist.magnitude);
			Vector3 off = dist * diff * .5f;
			
			//Debug.Log (pointa.transform.position + ", " +pointb.transform.position);
			//Debug.Log ("dist is : " + "(" + dist.x + ", " + dist.y + ", " + dist.z + ")" + "and off is: "+"(" + off.x + ", " + off.y + ", " + off.z + ")"+ " and diff is: " +diff + " and h is: "+h);		
			//Debug.Log ("before: "+pointa.transform.position + ",  "+ pointb.transform.position);
			
			pointa.transform.position += -1 * off;
			pointb.transform.position += off;
			
			Vector3 distafter = pointa.transform.position - pointb.transform.position;
			//Debug.Log ("after: "+pointa.transform.position + ",  "+ pointb.transform.position);
			
			//Debug.Log ("constraint: "+distConstraint+", and current dist is: "+dist.magnitude + " and after dist is: " + distafter.magnitude);
		}
		
	}

	public void setPosLR()
	{
		lr.SetPosition(0, pointa.transform.position);
		lr.SetPosition(1, pointb.transform.position);	
	}

}
