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

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info){


		if (stream.isWriting)
		{
			Rigidbody rigidbody = this.gameObject.GetComponent<Rigidbody>();
			stream.SendNext(rigidbody.position);
       		stream.SendNext(rigidbody.rotation);
			stream.SendNext(rigidbody.velocity);
		}
		else
		{
			networkPosition = (Vector3)stream.ReceiveNext();
       		networkRotation = (Quaternion)stream.ReceiveNext();
			rigidbody.velocity = (Vector3)stream.ReceiveNext();


       		float lag = Mathf.Abs((float)(PhotonNetwork.time - info.timestamp));
       		networkPosition += (this.velocity * lag);
		}		
	}

	public void FixedUpdate()
	{
		if (photonView.isMine){
			rigidbody.position = Vector3.MoveTowards(this.position, networkPosition, Time.fixedDeltaTime);
			rigidbody.rotation = Quaternion.RotateTowards(rigidbody.rotation, networkRotation, Time.fixedDeltaTime * 100.0f);
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
}