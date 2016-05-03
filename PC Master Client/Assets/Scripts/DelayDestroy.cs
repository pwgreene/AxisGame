using UnityEngine;
using System.Collections;

public class DelayDestroy : MonoBehaviour {
	public float delayedTimeDestroy;
	private float currentTime;
	// Use this for initialization
	void Start () {
		currentTime = Time.time;
		Debug.Log (currentTime);
	}
	
	// Update is called once per frame
	void Update () {
		if ((currentTime + delayedTimeDestroy) <= Time.time) {
			Destroy (this.gameObject);
		}
	}
}
