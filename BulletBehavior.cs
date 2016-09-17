using UnityEngine;
using System.Collections;

public class BulletBehavior : MonoBehaviour {

	void Awake()
	{
		Destroy (gameObject, 1.0f);
	}

	void FixedUpdate () {
		if (transform.position.y > 10.0f || transform.position.y < -10.0 
			|| transform.position.x > 17.0 || transform.position.x < -17.0) {
			Destroy (gameObject);
		}
	}
}
