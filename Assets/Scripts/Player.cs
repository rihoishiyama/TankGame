using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
	[SerializeField]
	private Button onFireButton;
	[SerializeField]
	private Shotbullet shotBullet;
	[SerializeField]
	private PhotonView photonView;
	[SerializeField]
	private PhotonTransformView photonTransformView;
	[SerializeField]
	private Rigidbody playerRigid;
	[SerializeField]
	private Transform playerTransform;
	public float moveSpeed = 10f;
	public Joystick joystick;


	void Start()
	{
		joystick = GameObject.Find("Joystick").GetComponent<Joystick>();
		onFireButton = GameObject.Find("OnFireButton").GetComponent<Button>();
		onFireButton.onClick.AddListener(() => shotBullet.ButtonShot());
	}

	public void FixedUpdate()
	{
		if (!photonView.isMine)
		{
			playerRigid.position = Vector3.MoveTowards(playerRigid.position, playerTransform.position, Time.fixedDeltaTime);
			playerRigid.rotation = Quaternion.RotateTowards(playerRigid.rotation, playerTransform.rotation, Time.fixedDeltaTime * 100.0f);
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
			}
		}
	}

	private void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.CompareTag("Bullet"))
		{
			Destroy(this.gameObject);
			Destroy(other.gameObject);
		}
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (photonView.isMine)
		{
			if (stream.isWriting)
			{
				stream.SendNext(playerRigid.position);
				stream.SendNext(playerRigid.rotation);
				stream.SendNext(playerRigid.velocity);
			}
			else
			{
				playerTransform.position = (Vector3)stream.ReceiveNext();
				playerTransform.rotation = (Quaternion)stream.ReceiveNext();
				playerRigid.velocity = (Vector3)stream.ReceiveNext();
				float lag = Mathf.Abs((float)(PhotonNetwork.time - info.timestamp));
				playerTransform.position += (playerRigid.velocity * lag);
			}
		}
	}
}