using UnityEngine;
using System.Collections;

public class BonusBehaviour : MonoBehaviour {

	[SerializeField] private float speed;
	[SerializeField]private int bonusDuration;
	
	// Update is called once per frame
	void Update () {
		transform.Translate (0.0f, speed * Time.deltaTime, 0.0f);
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		if (col.gameObject.tag == "Player") {
			col.gameObject.GetComponent<PlayerBehavior>().BonusOn (bonusDuration);
			Destroy (gameObject);
		}
	}
}
