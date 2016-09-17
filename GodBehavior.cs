using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GodBehavior : MonoBehaviour {

	[SerializeField]private GameObject enemyPrefab = null;
	private int timeout = 0;
	[SerializeField] private int enemyTimeOut = 120;
	[SerializeField]private int maxEnemies = 10;
	private int _enemies_num = 0;
	private int _enemies_killed = 0;
	[SerializeField]private Text enemies_killed_text = null;
	[SerializeField]private Text lives_num_text = null;
	[SerializeField]private Graphic GameOverPanelPrefab;
	[SerializeField]private Canvas UICan;
	[SerializeField]private GameObject under_wall_obj;
	[SerializeField]private GameObject player;
	[SerializeField]private GameObject bonusObj;
	[SerializeField]private float bonusFrequency;
	//private GameObject under_wall_instance;
	private Graphic gameOverPan = null;
	private bool isGameOvered = false;
	private Vector3[] bornPositions;
	private int pos_num;
	[SerializeField]private float enemiesFireFreq;
	[SerializeField]private float enemiesMaxStepSize;
	public bool isPaused{ get; private set;}


	void Start()
	{
		isPaused = false;
		_enemies_killed = 0;
		//if (under_wall_instance != null)
		Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
		pos_num = 4;
		bornPositions = new Vector3[pos_num];
		bornPositions [0] = new Vector3 (-10.7f,2.66f,0.0f);
		bornPositions [1] = new Vector3 (-2.8f,2.66f,0.0f);
		bornPositions [2] = new Vector3 (4.9f,2.66f,0.0f);
		bornPositions [3] = new Vector3 (12.8f,2.66f,0.0f);
	}



	// Update is called once per frame
	void Update () {
		if (isGameOvered && Input.GetKeyDown(KeyCode.Space)) {
			isGameOvered = false;
			RestartGame ();

		}

		if (!isGameOvered && Input.GetKeyDown(KeyCode.Escape)) {
			PauseGame ();
		}

		if (timeout == 0) {
			CreateEnemy ();
		}
		timeout = (timeout + 1) % enemyTimeOut;

		if (Random.value > bonusFrequency) {
			CreateBonus ();
		}
	}

	void CreateBonus()
	{
		if (!isPaused) {
			GameObject enemyObject = (GameObject)Instantiate<GameObject> (bonusObj);
			enemyObject.transform.position = bornPositions[Random.Range(0,pos_num)];
		}
	}

	void CreateEnemy()
	{
		if (_enemies_num < maxEnemies) {
			GameObject enemyObject = (GameObject)Instantiate<GameObject> (enemyPrefab);
			enemyObject.transform.position = bornPositions[Random.Range(0,pos_num)];
			EnemyBehavior enemyBehavior  = enemyObject.GetComponent<EnemyBehavior> ();
			enemyBehavior.SetGodGameObject(gameObject);

			enemyBehavior.SetMaxStepSize(enemiesMaxStepSize);
		    enemyBehavior.SetFireFrequency(enemiesFireFreq);

			enemyBehavior.SetRandomBonusToPlayer();

			_enemies_num++;
		}
	}


	public void EnemyDead()
	{
		_enemies_num--;

	}

	public void EnemyKilled()
	{
		EnemyDead ();
		_enemies_killed++;
		enemies_killed_text.text = _enemies_killed.ToString ();
		if ( _enemies_killed % 10 == 0 ) {
			IncreaseDifficulty(0);
		}
	}

	public void GameOver()
	{
		//show message
		//pause game
		//blur scene
		Debug.Log("GameOver");
		gameOverPan = Instantiate (GameOverPanelPrefab);
		gameOverPan.transform.SetParent (UICan.transform, false);
		gameOverPan.transform.SetAsFirstSibling ();
		Instantiate<GameObject> (under_wall_obj);
		isGameOvered = true;
		maxEnemies = 100;
	}

	public void SetLivesNum(int lives)
	{
		lives_num_text.text = lives.ToString ();
	}

	private void IncreaseDifficulty(float multiplier)
	{
		maxEnemies += 1;
		enemiesFireFreq -= 0.001f;
		enemiesMaxStepSize += 0.01f;
		bonusFrequency -= 0.001f;
	}

	public void RestartGame()
	{
//		if (gameOverPan != null) {
//			Destroy (gameOverPan.gameObject);
//			gameOverPan = null;
//		}
		SceneManager.LoadScene (gameObject.scene.buildIndex);
	}

	public void Mute (bool muteVal)
	{
		AudioListener.pause = muteVal;
	}

	public void PauseGame ()
	{
		if (isPaused) {
			isPaused = false;
			Time.timeScale = 1;
			Mute (false);
		} else {
			isPaused = true;
			Time.timeScale = 0;
			Mute (true);
		}
	}
}

