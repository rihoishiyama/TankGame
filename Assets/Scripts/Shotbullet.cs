using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Shotbullet : MonoBehaviour
{
	[SerializeField]
	private PhotonView photonView;
	public Transform shotPlace;
	public GameObject shellPrefab;
	public float shotSpeed;
	public AudioClip shotSound;
	public static int bulletcount;

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
				Debug.Log("ルーム閉じる。");
				PhotonNetwork.room.name = "newRoomName";
				PhotonNetwork.room.open = false;
				PhotonNetwork.room.visible = false;

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
		Rigidbody shellRigidbody = shell.GetComponent<Rigidbody>();

		// shellRigidbodyにz軸方向の力を加える。
		shellRigidbody.AddForce(transform.forward * shotSpeed);
	}
}
