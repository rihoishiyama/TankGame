using UnityEngine;
using UnityEngine.UI;

public class Player : Photon.MonoBehaviour
{

	[SerializeField]
	private Button onFireButton;
	[SerializeField]
	private Shotbullet shotBullet;
	[SerializeField]
	private PhotonView photonView;
	[SerializeField]
	private PhotonTransformView photonTransformView;
	private Vector3 playerSpeed;
	private Transform networkTransform;
	public float moveSpeed = 10f;
	public Joystick joystick;


	void Start()
	{
		joystick = GameObject.Find("Joystick").GetComponent<Joystick>();
		onFireButton = GameObject.Find("OnFireButton").GetComponent<Button>();
		onFireButton.onClick.AddListener(() => shotBullet.ButtonShot());
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			stream.SendNext(transform.position);
			stream.SendNext(transform.rotation);

			//データの送信
		}
		else
		{
			//データの受信
			networkTransform.position = (Vector3)stream.ReceiveNext();
			networkTransform.rotation = (Quaternion)stream.ReceiveNext();

			float lag = Mathf.Abs((float)(PhotonNetwork.time - info.timestamp));
			networkTransform.position += (playerSpeed * lag);
		}
	}

	public void FixedUpdate()
	{
		if (!photonView.isMine)
		{
			transform.position = Vector3.MoveTowards(transform.position, networkTransform.position, Time.fixedDeltaTime);
			transform.rotation = Quaternion.RotateTowards(transform.rotation, networkTransform.rotation, Time.fixedDeltaTime * 100.0f);
		}
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
				playerSpeed = moveVector * moveSpeed * Time.deltaTime;
			}
		}
	}

	private void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.CompareTag("Bullet"))
		{
			PhotonNetwork.Destroy(this.gameObject);
		}
	}
}