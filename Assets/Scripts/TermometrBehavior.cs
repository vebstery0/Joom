using UnityEngine;
using System.Collections;

public class TermometrBehavior : MonoBehaviour {
public CharacterController playerControl;
	public float speed;
	// Use this for initialization
	void Start () {
		transform.localScale= new Vector3(0.41F, 0 , 1);
		speed = 0.05f;
	}
	
	// Update is called once per frame
	void Update () {
		if ((playerControl.isGrounded) && (transform.localScale.y <= 0.9679772)){
		transform.localScale += new Vector3(0, 1*Time.deltaTime*speed , 0);
					} else 
		{
		transform.localScale -= new Vector3(0, 1*Time.deltaTime*speed , 0);
		}
	}
}
