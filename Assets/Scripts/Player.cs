using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
	[SerializeField]
	private Button onFireButton;
	[SerializeField]
	private Shotbullet shotBullet;
	public float moveSpeed = 10f;
	public Joystick joystick;
	private PhotonView photonView;

	void Start()
	{
		photonView = PhotonView.Get(this);
		joystick = GameObject.Find("Joystick").GetComponent<Joystick>();
		onFireButton = GameObject.Find("OnFireButton").GetComponent<Button>();
		onFireButton.onClick.AddListener(() => shotBullet.ButtonShot());
	}


	void Update()
	{
		if (photonView.isMine)
		{
			Vector3 moveVector = (Vector3.right * joystick.Horizontal + Vector3.forward * joystick.Vertical);

			if (moveVector != Vector3.zero)
			{
				transform.rotation = Quaternion.LookRotation(moveVector);
				transform.Translate(moveVector * moveSpeed * Time.deltaTime, Space.World);
			}
		}
	}

	private void OnCollisionEnter(Collision other)
	{
		if (photonView.isMine)
		{
			if (other.gameObject.CompareTag("Bullet"))
			{
				Destroy(this.gameObject);
				Destroy(other.gameObject);
			}
		}

	}
}