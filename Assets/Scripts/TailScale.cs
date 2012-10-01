using UnityEngine;
using System.Collections;

public class TailScale : MonoBehaviour {
	
	private float yMouse;	// mouse position
	private float xMouse;	// mouse position
	public float xLen;		// Катет трикутника
	public float yLen;		// Катет трикутника
	public float Gipo = 0;	// Гіпотенуза
	public float angle_aR;	// кут в радіанах
	public float angle_aG;	// кут в градусах
	private float rotate_angle;
	public Camera camera;
		
	public static int TAIL_DEFAULT = 0;
	public static int TAIL_SCALING = 1;
	public static int TAIL_PUSH = 2;
	
	private static int DIRECTION_RIGHT = 1;
	private static int DIRECTION_LEFT = -1;
	private static int DIRECTION_UP = 1;
	private static int DIRECTION_DOWN = -1;
	
	private string POINT_HIDDEN = "TrajectoryPointHidden";
	private string POINT_VISIBLE = "TrajectoryPointVisible";
	
	public int state = 0;
	public int xDirection = 0;
	public int yDirection = 0;
	
	public GameObject trajectoryPoint;	// TODO: connect point's gameObject to this field (from inspector)
	private GameObject[] point;			// array of trajectoryPoint's copies
	
	void Start () {
		// hide tail
		renderer.enabled = false;
		
		// copy trajectoryPoint to point array
		point = new GameObject[10];
		for (int i = 0; i < 10; i++) {
			point[i] = Instantiate(trajectoryPoint) as GameObject;
			point[i].transform.parent = transform.parent;
			point[i].transform.localPosition = new Vector3(0, 0, 0);
			point[i].transform.tag = POINT_HIDDEN;
		}
	}

	void Update () 
	{
		yMouse = Input.mousePosition.y;//координати по у
		xMouse = Input.mousePosition.x;//координати по х
		Vector3 screenPos = camera.WorldToScreenPoint(transform.parent.position);
		Renderer parentRenderer = transform.parent.gameObject.GetComponent<Renderer>();
		float pixelRatio = camera.pixelHeight / (camera.orthographicSize * 2);
		Vector2 parentSize = new Vector2(parentRenderer.bounds.size.x * pixelRatio, parentRenderer.bounds.size.y * pixelRatio);
		
		if (Input.GetMouseButton(0)){
			// if touch outside - don't begin scaling
			if (!(xMouse >= screenPos.x - parentSize.x / 2 && xMouse <= screenPos.x + parentSize.x / 2
				&& yMouse >= screenPos.y - parentSize.y / 2 && yMouse <= screenPos.y + parentSize.y / 2)) {
				if (state != TAIL_SCALING)
					return;
			}
			
			state = TAIL_SCALING;
			// show tail
			renderer.enabled = true;
			OTAnimatingSprite animation = transform.parent.gameObject.GetComponent<OTAnimatingSprite>();
			animation.Play("eyesClose");
			
			if (yMouse < screenPos.y && xMouse > screenPos.x) {
				xDirection = DIRECTION_LEFT;
				yDirection = DIRECTION_UP;
				yLen = screenPos.y - yMouse;	
				xLen = xMouse - screenPos.x;
				Gipo = Mathf.Sqrt(Mathf.Pow(yLen, 2) + Mathf.Pow(xLen, 2));
				angle_aR = Mathf.Acos((Mathf.Pow(yLen, 2)+Mathf.Pow(Gipo, 2)-Mathf.Pow(xLen, 2))/(2*yLen*Gipo));	
				rotate_angle = (float)((angle_aR * 180)/3.14);
				angle_aG = (float)(90 - (angle_aR * 180)/3.14);
			}
			if (yMouse > screenPos.y && xMouse > screenPos.x) {
				xDirection = DIRECTION_LEFT;
				yDirection = DIRECTION_DOWN;
				yLen = yMouse - screenPos.y;	
				xLen = xMouse - screenPos.x;
				Gipo = Mathf.Sqrt(Mathf.Pow(yLen, 2) + Mathf.Pow(xLen, 2));
				angle_aR = Mathf.Acos((Mathf.Pow(xLen, 2)+Mathf.Pow(Gipo, 2)-Mathf.Pow(yLen, 2))/(2*xLen*Gipo));
				rotate_angle = (float)((angle_aR * 180)/3.14)+90;
				angle_aG = (float)((angle_aR * 180)/3.14);
			}
			if (yMouse > screenPos.y && xMouse < screenPos.x) {
				xDirection = DIRECTION_RIGHT;
				yDirection = DIRECTION_DOWN;
				yLen = yMouse - screenPos.y;	
				xLen = screenPos.x - xMouse;
				Gipo = Mathf.Sqrt(Mathf.Pow(yLen, 2) + Mathf.Pow(xLen, 2));
				angle_aR = Mathf.Acos((Mathf.Pow(yLen, 2)+Mathf.Pow(Gipo, 2)-Mathf.Pow(xLen, 2))/(2*yLen*Gipo));
				rotate_angle = (float)((angle_aR * 180)/3.14)+180;
				angle_aG = (float)(90 - (angle_aR * 180)/3.14);
			}
			if (yMouse < screenPos.y && xMouse < screenPos.x) {
				xDirection = DIRECTION_RIGHT;
				yDirection = DIRECTION_UP;
				yLen = screenPos.y - yMouse;	
				xLen = screenPos.x - xMouse;
				Gipo = Mathf.Sqrt(Mathf.Pow(yLen, 2) + Mathf.Pow(xLen, 2));
				angle_aR = Mathf.Acos((Mathf.Pow(xLen, 2)+Mathf.Pow(Gipo, 2)-Mathf.Pow(yLen, 2))/(2*xLen*Gipo));
				rotate_angle = (float)((angle_aR * 180)/3.14)+270;
				angle_aG = (float)((angle_aR * 180)/3.14);
			}
			transform.localScale = new Vector3(0.91F, Gipo / 30, 1);
			Quaternion target = Quaternion.Euler (0, 0, rotate_angle );
			transform.rotation = Quaternion.Slerp(transform.rotation , target, 2);
			
			// trajectory
			DrawTrajectory();
		} else {
			if (state == 1) {
				// tell to controller to push our object
				state = TAIL_PUSH;
				// hide tail
				renderer.enabled = false;
				// hide all trajectory points
				for (int i = 0; i < point.Length; i++) {
					point[i].transform.localPosition = new Vector3(0, 0, 0);
				}
			}
			transform.localScale = new Vector3(1F,0.5F,1F); 
			Quaternion target = Quaternion.Euler (0, 0, 0);
			transform.rotation = Quaternion.Slerp(transform.rotation , target, 2);
		}
	}
	
	
	/*
	 * Method to work with trajectory.
	 * Calculate and set positions of points.
	 * 
	 */
	private void DrawTrajectory() {
		PlayerControls player = transform.parent.gameObject.GetComponent<PlayerControls>();
		float t = 0.05f; // time
		float V0 = Gipo / player.jumpSpeed; // start speed
		float g = player.gravity; // gravity from player settings
		float Angle = Mathf.Abs(angle_aG * 3.14f / 180f); // angle of push
		
		Vector3 tempPos = new Vector3(0, 0, 0);
		int maxIterations = (int)(Gipo / 7) > 10 ? 10 : (int)(Gipo / 7);	// calculate number of points to show
		
		for (int i = 0; i < maxIterations; i++) {
			if (point[i].CompareTag(POINT_HIDDEN)) {
				OTAnimatingSprite animation = point[i].GetComponent<OTAnimatingSprite>();
				animation.Play("show");
				point[i].tag = POINT_VISIBLE;
			}
			
			// calculate position of point in time
			tempPos.x = xDirection * V0 * Mathf.Cos(Angle) * t;
   			tempPos.y = yDirection * V0 * Mathf.Sin(Angle) * t - g*t*t / 2; 
			point[i].transform.localPosition = tempPos;
			t += 0.1f;
		}
		
		// if we have unused points, we should set them position to default
		for (int i = maxIterations; i < point.Length; i++) {
			OTAnimatingSprite animation = point[i].GetComponent<OTAnimatingSprite>();
			animation.Play("normal");
			point[i].tag = POINT_HIDDEN;
			point[i].transform.localPosition = new Vector3(0, 0, 0);
		}
	}
	
	
}
