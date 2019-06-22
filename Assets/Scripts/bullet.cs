using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : Photon.MonoBehaviour
{
	[SerializeField]
	private PhotonView photonView;
	[SerializeField]
	private PhotonTransformView photonTransformView;
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

			if (photonView.isMine)
			{
				Shotbullet.bulletcount -= 1;
			}
			PhotonNetwork.Destroy(this.gameObject);

		}

		if (other.gameObject.CompareTag("Player"))
		{
			if (photonView.isMine)
			{
				Shotbullet.bulletcount -= 1;
			}
			other.gameObject.GetComponent<Player> ().TakeDamage (this.gameObject);
			PhotonNetwork.Destroy(this.gameObject);

		}
    }
	
}
