using UnityEngine;
using UnityEngine.UI;

public class Player : Photon.MonoBehaviour
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
		}
	}
}