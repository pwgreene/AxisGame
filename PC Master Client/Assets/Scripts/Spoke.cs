using UnityEngine;
using System.Collections;

public class Spoke : MonoBehaviour {

	public TurretController player;
	public RotatingCoreBehaviour core;


	const int VERTEX_COUNT = 2;
	const float WIDTH = .2f;

	LineRenderer line;

	// Use this for initialization
	void Start () {
		Vector3 direction = (player.gameObject.transform.position - core.gameObject.transform.position).normalized;
//		line = this.gameObject.GetComponent<LineRenderer> ();
		if (line == null) {
			print ("Something bad happened with the Spoke");
		}
		else {
//			line.SetColors (color, color);
//			line.SetVertexCount (VERTEX_COUNT);
//			line.SetWidth (WIDTH, WIDTH);

			//Vector3 direction = (player.gameObject.transform.position - core.gameObject.transform.position).normalized;
			print ("Spoke " + direction);

//			line.SetPosition (0, direction*core.GetComponent<CircleCollider2D>().radius);
//			line.SetPosition (1, direction*player.max_distance);

		}
	}
	
	// Update is called once per frame
	void Update () {
		if (player != null || core != null) {
			Vector3 direction = (player.gameObject.transform.position - core.gameObject.transform.position).normalized;
			//print ("Spoke " + direction);

//			line.SetPosition (0, direction*core.GetComponent<CircleCollider2D>().radius);
//			line.SetPosition (1, direction*player.max_distance);
			float angle = Vector3.Angle(Vector3.up, direction);
			Vector3 cross = Vector3.Cross (Vector3.up, direction);
			if (cross.z < 0) { //see if angle is >180 deg
				angle = 360 - angle;
			}
			transform.rotation = Quaternion.Euler (0, 0, angle);
		}
		else {
			print ("I think we should disappear");
			Destroy (this.gameObject);
		}
	}
}
