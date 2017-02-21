using UnityEngine;

[RequireComponent (typeof (Rigidbody2D))]
public class Planet : MonoBehaviour 
{
	[SerializeField]
	private float maxTorque;
	private new Rigidbody2D rigidbody;



	private void Awake ()
	{

		rigidbody = GetComponent<Rigidbody2D> ();
	
	}

	private void Start () 
	{
		AddRandomTorque ();	

	}

	void Update () {



	}
	private void AddRandomTorque ()
	{
		float torque = Random.Range (-maxTorque, maxTorque);
		rigidbody.AddTorque (torque);
	}


}
