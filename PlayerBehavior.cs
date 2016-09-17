using UnityEngine;
using System.Collections;

public class PlayerBehavior : MonoBehaviour {

	private Player player;
	public GameObject GodGameObject;
	private GodBehavior god;
	[SerializeField]private int numLives;
	public enum bonus {AUTO, TRIPLECUM, OVERCUM, LIVE, LOWER };
	[SerializeField]float LIVE_FREQ=0.9999f;


	void Start()
	{
		god = GodGameObject.GetComponent<GodBehavior> ();
		player = GetComponent<Player> ();
		god.SetLivesNum (numLives);
	}

	void Update()
	{
		player.MakeStep(Input.GetAxis("Mouse X"));
		if (Input.GetMouseButtonDown(0) && !god.isPaused) {
			player.Fire ();
		}
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		if (col.gameObject.tag == "enemy" || col.gameObject.tag == "enemybullet") {
			Die ();
		}
	}

	public void AddLive()
	{
		numLives ++;
		god.SetLivesNum (numLives);
	}

	public void BonusOn(int durration)
	{
		if(Random.value > LIVE_FREQ)
		{
			AddLive();
		} else {
			player.SetNextFireModeOrIncreaseFireModeOffset(durration);
		}
	}
		

	void Die()
	{
		if (numLives <= 0) {
			player.Die ();
			god.GameOver ();
		} else {
			numLives--;
			player.DefaultFireMode();
			god.SetLivesNum (numLives);

		}
	}
		
}
