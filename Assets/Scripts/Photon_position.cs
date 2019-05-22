using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Photon_position : MonoBehaviour
{
	[SerializeField]
	private float m_speed = 6.0f;
	private PhotonView m_photonView = null;
	private Renderer m_render = null;
	private readonly Color[] MATERIAL_COLORS = new Color[]
	{
		Color.white, Color.red, Color.green, Color.blue, Color.green,
	};

	private Text text;
	GameObject game;

	void Awake()
	{
		m_photonView = GetComponent<PhotonView>();
		m_render = GetComponent<Renderer>();
	}

	private PhotonView photonView;
	private PhotonTransformView photonTransformView;
	private CharacterController characterController;

	void Start()
	{
		photonTransformView = GetComponent<PhotonTransformView>();
		photonView = PhotonView.Get(this);
		game = GameObject.Find("Text");
		//text = game.GetComponent<Text>();
		int ownerID = m_photonView.ownerId;
		m_render.material.color = MATERIAL_COLORS[ownerID];
	}

	void Update()
	{
		// 持ち主でないのなら制御させない
		if (photonView.isMine)
		{
			//現在の移動速度
			Vector3 velocity = gameObject.GetComponent<Rigidbody>().velocity;
			//移動速度を指定
			photonTransformView.SetSynchronizedValues(velocity, 0);
		}

		//text.text = m_photonView.isMine.ToString();
		Vector3 pos = transform.position;
		pos.x += Input.GetAxis("Horizontal") * m_speed * Time.deltaTime;
		pos.y += Input.GetAxis("Vertical") * m_speed * Time.deltaTime;
		transform.position = pos;
	} // class DemoObject
}