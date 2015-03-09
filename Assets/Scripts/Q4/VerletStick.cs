using UnityEngine;
using System.Collections;

public class VerletStick : MonoBehaviour {

	public VerletPointWorst pointa;
	public VerletPointWorst pointb;
	public double distConstraint;

	private LineRenderer lr;
	// Use this for initialization

	void Start () {
		lr = GetComponent<LineRenderer>();
		lr.SetWidth (.05f, .05f);
		lr.SetPosition(0, pointa.transform.position);
		lr.SetPosition(1, pointb.transform.position);
		
		
		double dx = pointa.getX() - pointb.getX();
		double dy = pointa.getY() - pointb.getY();
		
		distConstraint = (double)Mathf.Sqrt((float)(dx*dx + dy*dy));
		Debug.Log ("distConstraint is: "+distConstraint);
	}
	
	// Update is called once per frame
	void Update () {
		contract();
		lr.SetPosition(0, pointa.transform.position);
		lr.SetPosition(1, pointb.transform.position);
	}

	public void contract()
	{
		double dx = pointb.getX() - pointa.getX();
		double dy = pointb.getY() - pointa.getY();

		double h = (double)Mathf.Sqrt((float)(dx*dx + dy*dy)); //dist of a and b, w/o contraction
		double diff = (distConstraint - h)/h;
		double offx = dx * .5 * diff;
		double offy = dy * .5 * diff;
		
		pointa.pos.x -= offx;
		pointa.pos.y -= offy;

		pointb.pos.x += offx;
		pointb.pos.y += offy;

		pointa.transform.Translate(new Vector3((float)-offx, (float)-offy, 0) * Time.deltaTime);
		pointb.transform.Translate(new Vector3((float)offx, (float)offy, 0) * Time.deltaTime);
		
		Debug.Log ("distconst is: "+distConstraint+" and new h is: "+(double)Mathf.Sqrt((float)(dx*dx + dy*dy))); //dist of a and b, w/o contraction
		
		
	}
}
