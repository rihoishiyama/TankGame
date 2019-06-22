using UnityEngine;
using UnityEngine.UI;

public class Player : Photon.MonoBehaviour, IPunObservable
{
	PhotonView m_photonView;

	[SerializeField]
	private Button onFireButton;
	[SerializeField]
	private Shotbullet shotBullet;
	[SerializeField]
	private PhotonView photonview;

	private const float InterpolationDuration = 0.2f;

	private Vector3 startPosition;
	private Vector3 endPosition;
	private Quaternion startRotation;
	private Quaternion endRotation;
	private float elapsedTime = 0f;
	public float moveSpeed = 10f;
	public Joystick joystick;

	void Awake()
	{
		startPosition = transform.position;
		endPosition = transform.position;
		startRotation = transform.rotation;
		endRotation = transform.rotation;
	}


	void Start()
	{
		joystick = GameObject.Find("Joystick").GetComponent<Joystick>();
		onFireButton = GameObject.Find("OnFireButton").GetComponent<Button>();
		onFireButton.onClick.AddListener(() => shotBullet.ButtonShot());
<<<<<<< HEAD

		m_photonView = GetComponent<PhotonView> ();
		
	}


    
	// public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info){


	// 	if (stream.isWriting)
	// 	{
	// 		stream.SendNext(transform.position);
    //    		stream.SendNext(transform.rotation);

	// 		 //データの送信
	// 	}
	// 	else
	// 	{
	// 		 //データの受信
    //         transform.position = (Vector3)stream.ReceiveNext();
    //         transform.rotation = (Quaternion)stream.ReceiveNext();
	// 	}		
	// }


=======
	}

	void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			stream.SendNext(transform.position);
			stream.SendNext(transform.rotation);
>>>>>>> 23f5a3a86896aedaf46b9e3900b17897d6f5634b

			//データの送信
		}
		else
		{
			//データの受信
			startPosition = transform.position;
			startRotation = transform.rotation;

			endPosition = (Vector3)stream.ReceiveNext();
			endRotation = (Quaternion)stream.ReceiveNext();
			elapsedTime = 0f;
		}
	}

	void Update()
	{
		if (photonview.isMine)
		{
			Vector3 moveVector = (Vector3.right * joystick.Horizontal + Vector3.forward * joystick.Vertical);

			if (moveVector != Vector3.zero)
			{
				transform.rotation = Quaternion.LookRotation(moveVector);
				transform.Translate(moveVector * moveSpeed * Time.deltaTime, Space.World);
			}
		}
		else
		{
			elapsedTime += Time.deltaTime;
            transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / InterpolationDuration);
			transform.rotation = Quaternion.Lerp(startRotation, endRotation, elapsedTime / InterpolationDuration);
		}
	}

	private void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.CompareTag("Bullet"))
		{
			this.gameObject.GetComponent<PhotonView> ().TransferOwnership (PhotonNetwork.player.ID);
			PhotonNetwork.Destroy(this.gameObject);
		}
	}
}