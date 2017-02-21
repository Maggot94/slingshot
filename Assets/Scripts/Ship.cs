using UnityEngine;

[RequireComponent (typeof (Rigidbody2D))]
public class Ship : MonoBehaviour 
{
	private Rigidbody2D rigidbody;
	private SpringJoint2D spring;
	private Transform catapult;
	private Ray rayToMouse;
	private Ray leftCatapultToProjectile;
	private float maxStretchSqr;
	private float circleRadius;
	private bool clickedOn = false;
	private Vector2 preVelocity;
	private PolygonCollider2D circle;
	public float maxStretch = 3f;
	public LineRenderer catapultLineFront;

	private void Awake () 
	{
		spring = GetComponent<SpringJoint2D> ();
		rigidbody = GetComponent<Rigidbody2D> ();
		circle = GetComponent<PolygonCollider2D > ();
		catapult = spring.connectedBody.transform;

	}

	private void Start () 
	{

		LineRendererSetup ();
		rayToMouse = new Ray (catapult.position, Vector3.zero);
		leftCatapultToProjectile = new Ray (catapultLineFront.transform.position, Vector3.zero);
		maxStretchSqr = maxStretch * maxStretch;
		circleRadius = circle.pathCount;
	}

	
	void Update () {

		if (clickedOn)
			Dragging ();
		if (spring != null) {
			if (!rigidbody.isKinematic && preVelocity.sqrMagnitude > rigidbody.velocity.sqrMagnitude) {
				//Destroy (spring);
				spring.enabled = false; 
				catapultLineFront.enabled = false;
				rigidbody.velocity = preVelocity;

			}
			if (!clickedOn)
				preVelocity = rigidbody.velocity;
			LineRendererUpdate ();
		} else {
			catapultLineFront.enabled = false;


		}

	}

	void LineRendererSetup (){
		catapultLineFront.SetPosition (0, catapultLineFront.transform.position);
		catapultLineFront.sortingOrder = 3;
	
	}

	void OnMouseDown(){
		spring.enabled = false;
		clickedOn = true;
	}

	void OnMouseUp(){
		spring.enabled = true;
		rigidbody.isKinematic = false;
		clickedOn = false;

	}

	void Dragging(){
		Vector3 mouseWorldPoint = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		Vector2 catapultToMouse = mouseWorldPoint - catapult.position;
		if (catapultToMouse.sqrMagnitude > maxStretchSqr) {
			rayToMouse.direction = catapultToMouse;
			mouseWorldPoint = rayToMouse.GetPoint (maxStretch);
		}

		mouseWorldPoint.z = 0f;
		transform.position = mouseWorldPoint;
	}
	void LineRendererUpdate(){
		Vector2 catapultToProjectile = transform.position - catapultLineFront.transform.position;
		leftCatapultToProjectile.direction = catapultToProjectile;
		Vector3 holdPoint=leftCatapultToProjectile.GetPoint(catapultToProjectile.magnitude+circleRadius);
		catapultLineFront.SetPosition (1, holdPoint);

	}


	void OnTriggerEnter2D(Collider2D other){

		if (other.gameObject.tag == "Planet") {

			Debug.Log ("hubo colision"); 
			LineRendererUpdate();
			preVelocity = new Vector2 (0, 0); 
			rigidbody.isKinematic = true; 
			spring.enabled = true; 


		}
	}

	private void LookTowards (Vector2 direction)
	{
		
		transform.localRotation = Quaternion.LookRotation (Vector3.forward, direction);
	}
}
