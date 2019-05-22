using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
	public AudioClip reboundSound;
	public int reboundcount;
	private PhotonView photonView;
	private PhotonTransformView photonTransformView;


	void Start()
	{
		photonTransformView = GetComponent<PhotonTransformView>();
		photonView = PhotonView.Get(this);
	}


	void OnCollisionEnter(Collision other)
	{

		if (other.gameObject.CompareTag("Wall"))
		{

			reboundcount += 1;

			if (reboundcount > 1)
			{
				if (photonView.isMine)
				{
					Shotbullet.bulletcount -= 1;
				}

				Destroy(this.gameObject);
			}
			else
			{
				AudioSource.PlayClipAtPoint(reboundSound, transform.position);
			}
		}


		if (other.gameObject.CompareTag("Bullet"))
		{

			if (photonView.isMine)
			{
				Shotbullet.bulletcount -= 1;
			}
			Destroy(this.gameObject);

		}
	}

}
