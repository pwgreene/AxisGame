using UnityEngine;
using System.Collections;

public class Spoke : MonoBehaviour {

	public TurretController player;
	public RotatingCoreBehaviour core;

	Color color = Color.gray;

	const int VERTEX_COUNT = 2;
	const float WIDTH = .2f;

	LineRenderer line;

	// Use this for initialization
	void Start () {
		line = this.gameObject.GetComponent<LineRenderer> ();
		if (line == null) {
			print ("Something bad happened with the Spoke");
		}
		else {
			line.SetColors (color, color);
			line.SetVertexCount (VERTEX_COUNT);
			line.SetWidth (WIDTH, WIDTH);

			Vector3 direction = (player.gameObject.transform.position - core.gameObject.transform.position).normalized;
			print ("Spoke " + direction);

			line.SetPosition (0, direction*core.GetComponent<CircleCollider2D>().radius);
			line.SetPosition (1, direction*player.max_distance);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (player != null) {
			Vector3 direction = (player.gameObject.transform.position - core.gameObject.transform.position).normalized;
			//print ("Spoke " + direction);

			line.SetPosition (0, direction*core.GetComponent<CircleCollider2D>().radius);
			line.SetPosition (1, direction*player.max_distance);
		}
		else {
			print ("I think we should disappear");
			Destroy (this.gameObject);
		}
	}
}
