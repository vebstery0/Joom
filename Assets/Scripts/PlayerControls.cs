using UnityEngine;
using System.Collections;

public class PlayerControls : MonoBehaviour {
	
	public float speed = 10.0F;
	public float jumpSpeed = 8.0F;
	public float gravity = 25.0F;
	
	public TailScale tail;
	public Vector3 moveDirection = Vector3.zero;
	
	public bool isJumpPlatform = false;
	public Vector3 collisionSpeed;
	
	void Start () {
	}
	
	void Update() {
		CharacterController controller = GetComponent<CharacterController>();
		
		if (isJumpPlatform) {
		}
		else if (controller.isGrounded) {
			OTAnimatingSprite animation = GetComponent<OTAnimatingSprite>();
			animation.looping = false;
			animation.Play("default");
			
	        moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, 0);
	        moveDirection = transform.TransformDirection(moveDirection);
	        moveDirection *= speed;
			// Якщо користувач відпустив хвіст
			if (tail.state == TailScale.TAIL_PUSH) {
				float t = 1f;
				float V0 = tail.Gipo / jumpSpeed;
				float g = gravity;
				float Angle = Mathf.Abs(tail.angle_aG * 3.14f / 180f);
				moveDirection.x = tail.xDirection * V0 * Mathf.Cos(Angle);
   				moveDirection.y = tail.yDirection * V0 * Mathf.Sin(Angle);
				
				// old
				//moveDirection.y = tail.yLen * tail.yDirection / jumpSpeed;
				//moveDirection.x = tail.xLen * tail.xDirection / jumpSpeed;
				
				tail.state = TailScale.TAIL_DEFAULT;
				OTAnimatingSprite flyAnimation = GetComponent<OTAnimatingSprite>();
				flyAnimation.looping = true;
				flyAnimation.Play("fly");
			}
	    }
		
		
		if ((controller.collisionFlags == CollisionFlags.Below || controller.collisionFlags == CollisionFlags.Above) && isJumpPlatform) {
			moveDirection.y = collisionSpeed.y * (-0.8f);
			isJumpPlatform = false;
		} else if (controller.collisionFlags == CollisionFlags.Sides && isJumpPlatform) {
			moveDirection.x = collisionSpeed.x * (-0.8f);
			isJumpPlatform = false;
		} 
		
		else
		// Умова зіткнення з об'єктом знизу
		if (controller.collisionFlags == CollisionFlags.Above && !isJumpPlatform) {
			moveDirection.x = 0;
			//moveDirection.y = -1; // якщо розкоментувати то ігрок відіб'ється (зараз на деякий час прилипає)
		} else
		// Умова зіткнення об бік
		if (controller.collisionFlags == CollisionFlags.Sides && !isJumpPlatform) {
			moveDirection.y = 0;
			moveDirection.x = 0;
		}
		
		
	    moveDirection.y -= gravity * Time.deltaTime;
	    controller.Move(moveDirection * Time.deltaTime);
		
		// якщо ігрок вилітає за межі [-10, 10] по осі ОХ, то він появляється з іншої сторони
		if (transform.position.x >= 10.0f) {
			Vector3 newPosition = transform.position;
			newPosition.x = -10.0f;
			transform.position = newPosition;
		} else if (transform.position.x <= -10.0f) {
			Vector3 newPosition = transform.position;
			newPosition.x = 10.0f;
			transform.position = newPosition;
		}
	}
	
	
	void OnControllerColliderHit(ControllerColliderHit cch) {
		if (cch.gameObject.tag.Equals("JumpPlatform") && collisionSpeed.y != 0 && isJumpPlatform) {
			OTAnimatingSprite jumpPlatform = cch.gameObject.GetComponent<OTAnimatingSprite>();
			jumpPlatform.Play("hit");
		} else if (cch.gameObject.tag.Equals("BrokePlatform")) {
			//BrokePlatformCollision brokePlatform = cch.gameObject.GetComponent<BrokePlatformCollision>();
			//brokePlatform.PlayAnimation();
		}
	}
		
}