﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
	public AudioClip reboundSound;
	public int reboundcount;

	void OnCollisionEnter(Collision other)
	{
		reboundcount += 1;

		if (reboundcount > 1)
		{
			Shotbullet.bulletcount -= 1;

			Destroy(this.gameObject);
		}
		else
		{
			AudioSource.PlayClipAtPoint(reboundSound, transform.position);
		}
	}
}
