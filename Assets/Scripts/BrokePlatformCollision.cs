using UnityEngine;
using System.Collections;

public class BrokePlatformCollision : MonoBehaviour {
	
	private int state = 0;
	
	void Start () {
		state = 0;
	}
	
	void Update () {
	}
	
	
	public void PlayAnimation() {
		state++;
		OTAnimatingSprite animation = transform.parent.gameObject.GetComponent<OTAnimatingSprite>();
		switch (state) {
		case 1:
			animation.Play("broke_1");
			break;
		case 2:
			animation.Play("broke_2");
			break;
		case 3:
			animation.Play("broken");
			animation.onAnimationFinish = new OTObject.ObjectDelegate(SelfDestroy);
			break;
		default:
			break;
		}
	}
	
	
	void SelfDestroy(OTObject param) {
		Destroy(transform.parent.gameObject);
	}
	
	void OnTriggerEnter(Collider jumper) {
		if (jumper.gameObject.tag.Equals("Player")) {
			PlayAnimation();
		}
	}
	
	
}
