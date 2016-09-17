using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	private Collider2D _collider;
	private Rigidbody2D _rigidbody;
	[SerializeField]private float _speed_x = 0.0f;
	[SerializeField]private GameObject bulletPrefab;
	[SerializeField]private Vector2 bulletForce;
	[SerializeField]private float move_y;
	private int fireModeOffset = -1;
	public enum FireMode
	{
		SINGLE,
		AUTO,
		TRIPLE,
		OVERCUM
	};
	private int fireModesNum  = 4;
	private FireMode fireMode = FireMode.SINGLE;
	private AudioSource _audioSource;
	[SerializeField]private AudioClip[] _bornClipsCollection;
	private AudioClip _bornClip;
	[SerializeField]private AudioClip _fireClip;
	[SerializeField]private Color fireColor;
	private Renderer _rend;
	private Color _defaultColor;

	void Awake()
	{
		_rend = GetComponent<Renderer> ();
		_defaultColor = _rend.material.color;
		_collider = GetComponent<Collider2D> ();
		_rigidbody = GetComponent<Rigidbody2D> ();
		_audioSource = GetComponent<AudioSource> ();
		if (_bornClipsCollection.Length > 0) {
			_bornClip = _bornClipsCollection [Random.Range (0, _bornClipsCollection.Length - 1)];
		}
	}

	// Use this for initialization
	void Start () {
		
		if (_bornClip != null) {
			_audioSource.PlayOneShot (_bornClip);
		}
		
	}


	Vector2 Get2DPosition ()
	{
		return (new Vector2 (transform.position.x,transform.position.y));	
	}

//TODO:объединить повторяющиеся части этих двух методов
	public void MakeStep(float coef)
	{
		float move_pos = coef * _speed_x;
		Vector2 moveVector = Get2DPosition ();
		moveVector.x = Mathf.Clamp (moveVector.x
			           + move_pos * Time.deltaTime, 
			           -16.75f, 16.75f);
		Move (moveVector);
	}

	public void MakeStepWithY(float coef)
	{
		float move_pos = coef * _speed_x;
		Vector2 m = new Vector2 (move_pos, move_y);
		Move (Get2DPosition () + m * Time.deltaTime);
	}

	void Move(Vector2 move_coord)
	{
		_rigidbody.MovePosition (move_coord);
	}

	private void Shot(float rotation, float force_y = 0)
	{
		GameObject bulletObject = (GameObject)Instantiate<GameObject> (bulletPrefab);
		bulletObject.transform.position = Get2DPosition () + new Vector2(0.0f, Mathf.Sign(bulletForce.y)*4.0f);
		Vector2 thisBulletForce = new Vector2 (rotation,bulletForce.y + force_y);
		bulletObject.GetComponent<Rigidbody2D> ().AddForce (thisBulletForce);
	}

	public void FireColorOn()
	{
		_rend.material.color = fireColor;
	}

	public void FireColorOff()
	{
		_rend.material.color = _defaultColor;
	}

	public void Fire()
	{
		switch (fireMode) {
		case FireMode.SINGLE:
			Shot (0.0f);
			break;
		case FireMode.TRIPLE:
			Shot (0.0f);
			Shot (20000.0f);
			Shot (-20000.0f);
			break;
		case FireMode.AUTO:
			Shot (0.0f);
			Shot (0.0f,1000.0f);
			Shot (0.0f,10000.0f);
			break;
		case FireMode.OVERCUM:
			for (int i = 0; i < 30; i++) {
				Shot ((float)i*1000,(float)i*(-10));
			}
			break;
		default:
			break;
		}
		_audioSource.PlayOneShot (_fireClip);

		if(fireModeOffset <= 0) {
			DefaultFireMode();
		} else {
			fireModeOffset --;
		}
	}

	public void SetFireMode(FireMode fm, int duration = -1)
	{
		fireModeOffset = duration;
		fireMode = fm;
	}

	public void SetNextFireModeOrIncreaseFireModeOffset(int duration)
	{
		if( fireMode == FireMode.SINGLE) {
			fireModeOffset = duration;
			fireMode ++;
		} else if ((int)fireMode < fireModesNum -1){
			fireMode ++;
		} else {
			fireModeOffset += duration;
		}
	}

	public void DefaultFireMode()
	{
		if (fireMode != FireMode.SINGLE){
			SetFireMode (FireMode.SINGLE);
			fireModeOffset = -1;
		}
		
	}

	public void Die()
	{
		//GetComponent<Animator> ().SetBool ("Die", true);
		Destroy (gameObject);
	}

}
