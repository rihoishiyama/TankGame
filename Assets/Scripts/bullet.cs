using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour {


	public int reboundcount;


	void OnCollisionEnter(Collision other){
		reboundcount += 1;

		if(reboundcount > 2){

			Shotbullet.bulletcount -= 1;

			Destroy(this.gameObject);
		}





	}
       



		

}

