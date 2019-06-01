using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Shotbullet : MonoBehaviour
{
	[SerializeField]
	private PhotonView photonView;
	[SerializeField]
	private PhotonTransformView photonTransformView;
	private Transform shellTransform;
	private Rigidbody shellRigid;
	public Transform shotPlace;
	public GameObject shellPrefab;
	public float shotSpeed;
	public AudioClip shotSound;
	public static int bulletcount;

	public void FixedUpdate()
	{
		if (!photonView.isMine)
		{
			shellRigid.position = Vector3.MoveTowards(shellRigid.position, shellTransform.position, Time.fixedDeltaTime);
			shellRigid.rotation = Quaternion.RotateTowards(shellRigid.rotation, shellTransform.rotation, Time.fixedDeltaTime * 100.0f);
		}
	}

	public void ButtonShot()
	{
		if (photonView.isMine)
		{
			// もしも「Fire1」というボタンが押されたら（条件）
			if (bulletcount < 5)
			{
				Shot();

				// ②効果音を再生する。
				AudioSource.PlayClipAtPoint(shotSound, transform.position);
				bulletcount += 1;
			}
			else
			{
				AudioSource.PlayClipAtPoint(shotSound, transform.position);
			}
		}
	}

	public void Shot()
	{
		// プレファブから砲弾(Shell)オブジェクトを作成し、それをshellという名前の箱に入れる。
		GameObject shell = PhotonNetwork.Instantiate("bullet", shotPlace.position, Quaternion.identity, 0);

		// Rigidbodyの情報を取得し、それをshellRigidbodyという名前の箱に入れる。
		shellRigid = shell.GetComponent<Rigidbody>();
		shellTransform = shell.transform;

		// shellRigidbodyにz軸方向の力を加える。
		shellRigid.AddForce(transform.forward * shotSpeed);
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (photonView.isMine)
		{
			if (stream.isWriting)
			{
				stream.SendNext(shellRigid.position);
				stream.SendNext(shellRigid.rotation);
				stream.SendNext(shellRigid.velocity);
			}
			else
			{
				shellTransform.position = (Vector3)stream.ReceiveNext();
				shellTransform.rotation = (Quaternion)stream.ReceiveNext();
				shellRigid.velocity = (Vector3)stream.ReceiveNext();
				float lag = Mathf.Abs((float)(PhotonNetwork.time - info.timestamp));
				shellTransform.position += (shellRigid.velocity * lag);
			}
		}
	}
}
