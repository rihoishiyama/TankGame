using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotbullet : MonoBehaviour
{
	public GameObject shellPrefab;
	public float shotSpeed;
	public AudioClip shotSound;
	public static int bulletcount;

	public void ShotButton()
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

	public void Shot()
	{
		// プレファブから砲弾(Shell)オブジェクトを作成し、それをshellという名前の箱に入れる。
		GameObject shell = (GameObject)Instantiate(shellPrefab, transform.position, Quaternion.identity);

		// Rigidbodyの情報を取得し、それをshellRigidbodyという名前の箱に入れる。
		Rigidbody shellRigidbody = shell.GetComponent<Rigidbody>();

		// shellRigidbodyにz軸方向の力を加える。
		shellRigidbody.AddForce(transform.forward * shotSpeed);
	}





}
