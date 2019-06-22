using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : Photon.MonoBehaviour
{
	[SerializeField]
	private PhotonView photonview;
	public AudioClip reboundSound;
	public int reboundcount;


	void OnCollisionEnter(Collision other)
	{

		if (other.gameObject.CompareTag("Wall"))
		{

			reboundcount += 1;
			var hit = other.gameObject;

			if (reboundcount > 1)
			{
				if (photonView.isMine)
				{
					Shotbullet.bulletcount -= 1;
				}

				PhotonNetwork.Destroy(this.gameObject);
			}
			else
			{
				AudioSource.PlayClipAtPoint(reboundSound, transform.position);
			}
		}

		if (other.gameObject.CompareTag("Bullet"))
		{

			if (photonview.isMine)
			{
				Shotbullet.bulletcount -= 1;
			}
			PhotonNetwork.Destroy(this.gameObject);

		}

		if (other.gameObject.CompareTag("Player"))
		{
<<<<<<< HEAD
			if (photonView.isMine)
=======

			if (photonview.isMine)
>>>>>>> 23f5a3a86896aedaf46b9e3900b17897d6f5634b
			{
				Shotbullet.bulletcount -= 1;
			}
			PhotonNetwork.Destroy(other.gameObject);
			PhotonNetwork.Destroy(this.gameObject);

		}
    }
	
}
