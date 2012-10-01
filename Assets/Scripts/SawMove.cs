using UnityEngine;
using System.Collections;

public class SawMove : MonoBehaviour {
	public GameObject saw;
	private int speed;
	private int xMove;
	// Use this for initialization
	void Start () {
		speed = 5;
		xMove = 1;
	}
	
	// Update is called once per frame
	void Update () {
		saw.transform.position += new Vector3(xMove, 0, 0)*speed*Time.deltaTime;
	}
	
	void OnTriggerExit(Collider other) {
		if (other.gameObject.tag.Equals("Saw")) {
			xMove *= -1;
		}
	}
	
	/*
	void OnTriggerEnter(Collider other) {
		Debug.Log("tag:" + other.gameObject.tag);
	}
	
	void OnTriggerStay(Collider other) {
		Debug.Log("tag:" + other.gameObject.tag);
	}
	*/
}
