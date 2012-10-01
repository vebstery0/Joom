using UnityEngine;
using System.Collections;

public class KillZoneCollision : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter(Collider jumper) {
		if (jumper.gameObject.tag.Equals("Player")) {
			Debug.Log("Game Over !!!!!!!!!!!!!!!!!!!!!!");
		}
	}
}
