/**
 * 
 * Скріпт вішається на тригер платформи. Він відключає зіткнення для батька тригера.
 * Дозволяє ігроку застрибувати на планформу через низ.
 * 
 **/ 

using UnityEngine;
using System.Collections;

public class PlatformBottom : MonoBehaviour {
	
	void Start () {
	
	}
	
	void Update () {
		
	}
	
	void OnTriggerEnter(Collider jumper) {
    	//make the parent platform ignore the jumper
    	Transform platform = transform.parent;
		Physics.IgnoreCollision(jumper, platform.collider);
	}

	void OnTriggerExit(Collider jumper) {
    	//reset jumper's layer to something that the platform collides with
    	//just in case we wanted to jump throgh this one
    	//jumper.gameObject.layer = 0;
		
    	//re-enable collision between jumper and parent platform, so we can stand on top again
    	Transform platform = transform.parent;
    	Physics.IgnoreCollision(jumper, platform.collider, false);
	}
	
}
