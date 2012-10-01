/**
 *
 * Скріпт для переміщення камери за ігроком по вісі OY
 * 
 **/

using UnityEngine;
using System.Collections;

public class CameraMove : MonoBehaviour {
	
	public Transform playerTransform;
	
	void Start () {	}
	
	void Update () {
		Vector3 temp = transform.position;
		temp.y = playerTransform.position.y + 3;
		transform.position = temp;
	}
	
}
