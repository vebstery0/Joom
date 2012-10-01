using UnityEngine;
using System.Collections;

public class JumpPlatformCollision : MonoBehaviour {
	
	
	void Start () {
	}
	
	void Update () {
	}
		
	void OnTriggerEnter(Collider jumper) {
		if (jumper.gameObject.tag.Equals("Player")) {
			PlayerControls player = jumper.gameObject.GetComponent<PlayerControls>();
			player.isJumpPlatform = true;
			
			player.collisionSpeed = player.moveDirection;
			Debug.Log("is jump platform set to true when y = " + player.moveDirection.y);
			
			//OTAnimatingSprite platform = transform.parent.gameObject.GetComponent<OTAnimatingSprite>();
			//platform.Play();
		}
	}
	
	void OnTriggerExit(Collider jumper) {
		if (jumper.gameObject.tag.Equals("Player")) {
			PlayerControls player = jumper.gameObject.GetComponent<PlayerControls>();
			player.isJumpPlatform = false;
			//player.collisionSpeed = null;
		}
	}
	
	
}
