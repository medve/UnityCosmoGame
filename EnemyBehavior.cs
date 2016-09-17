using UnityEngine;
using System.Collections;

public class EnemyBehavior : MonoBehaviour {


	private GameObject GodGameObject;
	private Player player;
	private GodBehavior god;
	private float moveThisWay;
	private float maxStepSize = 0.1f;
	private float stepSize;
	[SerializeField]private float maxWayChange = 60.0f;
	private float wayChange;
	private float fireFrequency;
	private float dirrection;
	private float bonusFreqMinimizer = 0.0f;
	public enum bonus {AUTO, TRIPLECUM, OVERCUM};
	private const int NONE_BONUS = 100;
	[SerializeField]private float[] bonusesFreqSet;



	void Start()
	{
		god = GodGameObject.GetComponent<GodBehavior> ();
		player = GetComponent<Player> ();
		moveThisWay = 0.0f;
		dirrection = Mathf.Sign(Random.Range (-1, 1));
		stepSize = Random.Range (0.0f, maxStepSize);
		wayChange = Random.Range (0.0f, maxWayChange);

	}

	public void SetRandomBonusToPlayer()
	{
		bonus playerBonus = (bonus)(bonusesFreqSet.Length - 1);
		float r = Random.value + bonusFreqMinimizer;
		for (int i = 0; i < bonusesFreqSet.Length; i++) {
			//ищем подходящий бонус
			if(bonusesFreqSet[i] > r){
				playerBonus = (i == 0 ? (bonus)NONE_BONUS : (bonus)(i - 1));
				break;
			}
		}
		BonusOn(playerBonus);
	}

	public void BonusOn(bonus b)
	{
		switch (b) {
		case bonus.OVERCUM:
			player.SetFireMode (Player.FireMode.OVERCUM, 999999);
			break;
		case bonus.TRIPLECUM:
			player.SetFireMode (Player.FireMode.TRIPLE, 999999);
			break;
		case bonus.AUTO:
			player.SetFireMode (Player.FireMode.AUTO, 999999);
			break;
		default:
			break;
		} 
	}

	void FixedUpdate()
	{
		if (transform.position.y > 14.0 || transform.position.y < -10.0f 
			|| transform.position.x > 17.0f || transform.position.x < -17.0f) {
			player.Die ();
			god.EnemyDead ();
		}

		if (moveThisWay > wayChange) {
			moveThisWay = 0.0f;
			dirrection *= -1.0f;
		}

		moveThisWay += Mathf.Abs(stepSize);
		player.MakeStepWithY(stepSize*dirrection);
		if (Random.Range(0.0f,1.0f) > fireFrequency) {
			player.Fire ();
		}
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		Debug.Log ("collision");
		if (col.gameObject.tag == "Player" || col.gameObject.tag == "bullet") {
			Die ();
		}
	}

	void Die()
	{
		player.Die ();
		god.EnemyKilled ();
	}

	public void SetGodGameObject(GameObject g)
	{
		GodGameObject = g;
	}

	public void SetMaxStepSize(float g)
	{
		maxStepSize = g;
	}

	public void SetFireFrequency(float g)
	{
		fireFrequency = g;
	}

	public float GetMaxStepSize()
	{
		return maxStepSize;
	}

	public float GetFireFrequency()
	{
		return fireFrequency;
	}

	public int getBonusesNum ()
	{
		return bonusesFreqSet.Length;
	}

}
