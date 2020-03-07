using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class Photon_connect : MonoBehaviour
{
	[SerializeField]
	private Button startButton;
	[SerializeField]
	private CanvasGroup touchBlockCanvas;
	[SerializeField]
	private CanvasGroup joystickCanvasGroup;
	[SerializeField]
	private ButtonController matchingController;
	[SerializeField]
	private string m_resourcePath = "";
	[SerializeField]
	private float m_randomCircle = 4.0f;
	private string ROOM_NAME = "defaultRoom";
	public bool AutoConnect = true;
	public byte Version = 1;
	private bool ConnectInUpdate = true;
	private PhotonView m_photonView = null;
	private Vector3[] startPos = { new Vector3(16, 0, 12), new Vector3(-16, 0, 12), new Vector3(-16, 0, -12), new Vector3(16, 0, -12) };
	private ReactiveProperty<int> roomPlayerCount;
	private bool isManualOnConnect = false;
	private bool isReady;
	public bool IsMakeRoomFailed { get; private set; }


	private void Start()
	{
		Init();
	}

	private void Init()
	{
		isReady = false;
		isManualOnConnect = false;
		IsMakeRoomFailed = false;
		touchBlockCanvas.blocksRaycasts = true;
		startButton.interactable = false;
		startButton.gameObject.SetActive(true);
		joystickCanvasGroup.interactable = false;
		joystickCanvasGroup.blocksRaycasts = false;
	}

	// cube生成
	public void SpawnObject()
	{
		matchingController.LoadCanvas(true, false, true);

		GameObject player = PhotonNetwork.Instantiate("Player", new Vector3(0, 0, 0), Quaternion.identity, 0);
		if (!player.GetComponent<Rigidbody>())
		{
			player.gameObject.AddComponent<Rigidbody>();
		}

		m_photonView = player.GetComponent<PhotonView>();
		int ownerID = m_photonView.ownerId;
		Vector3 playerPos = player.transform.position;
		playerPos = startPos[(ownerID - 1) % 4];
		player.transform.position = playerPos;
	}

	// 生成するcubeの座標をランダム生成
	private Vector3 GetRandomPosition()
	{
		var rand = UnityEngine.Random.insideUnitCircle * m_randomCircle;
		return rand;
	}

	public virtual void Update()
	{
		// Debug.Log("RoomName: "+ROOM_NAME+ ", RandomJoin: "+isRandomJoin+ ", IsGetName: "+isGetRoomName);

		if (ConnectInUpdate && AutoConnect && !PhotonNetwork.connected)
		{
			Debug.Log("Update() was called by Unity. Scene is loaded. Let's connect to the Photon Master Server. Calling: PhotonNetwork.ConnectUsingSettings();");
			ConnectInUpdate = false;
			PhotonNetwork.ConnectUsingSettings(Version + "." + SceneManagerHelper.ActiveSceneBuildIndex);
		}
		if (isManualOnConnect)
		{
			OnConnectedToMaster();
			isManualOnConnect = false;
		}
		if (isReady)
		{
			if (roomPlayerCount.Value != PhotonNetwork.playerList.Length)
			{
				roomPlayerCount.Value = PhotonNetwork.playerList.Length;
			}
		}
		//if (Input.GetMouseButton(0))
		//{
		//    SpawnObject();
		//}
	}

	public virtual void OnConnectedToMaster()
	{
		Debug.Log("OnConnectedToMaster() was called by PUN. Now this client is connected and could join a room. Calling: PhotonNetwork.JoinRandomRoom();");
		//PhotonNetwork.JoinRandomRoom();
		//PhotonNetwork.CreateRoom(null, new RoomOptions() { MaxPlayers = 4 }, null);
		// PhotonNetwork.JoinOrCreateRoom(ROOM_NAME, new RoomOptions(), TypedLobby.Default);
		Debug.Log("WhenConnect...RoomName: "+matchingController.roomName+ ", RandomJoin: "+matchingController.isRandomJoin+ ", IsGetName: "+matchingController.isGetRoomName + ", isJoin: " + matchingController.isJoin + ", isCreate: "+matchingController.isCreate);

		if (matchingController.isGetRoomName || matchingController.isRandomJoin)
		{
			if (matchingController.isRandomJoin)
			{
				PhotonNetwork.JoinRandomRoom();
				Debug.Log("RandomRoomName"+ROOM_NAME);
			}
			else if (matchingController.isJoin)
			{
				// join失敗したらofflineになるかも?
				PhotonNetwork.JoinOrCreateRoom(ROOM_NAME, new RoomOptions() { MaxPlayers=4 }, TypedLobby.Default);
				Debug.Log("JoinRoomCOmplete");
			}
			else
			{
				PhotonNetwork.CreateRoom(ROOM_NAME, new RoomOptions() { MaxPlayers=4 }, TypedLobby.Default);
				Debug.Log("CreateRoomComplete");
			}
		}
	}

	public void OnJoinedRoom()
	{
		Debug.Log("OnJoinedRoom() called by PUN. Now this client is in a room. From here on, your game would be running. For reference, all callbacks are listed in enum: PhotonNetworkingMessage");
		SpawnObject();
		SetRoomPlayerCount();
	}

	public void OnPhotonCreateRoomFailed()
	{
		// ルーム作成失敗時
		Debug.Log("OnCreateRoom() called by PUN. But Failed so RoomName is exist.");
		Init();
		IsMakeRoomFailed = true;
		matchingController.ReMatching();
		isManualOnConnect = true;
	}

	private void SetRoomPlayerCount()
	{
		roomPlayerCount = new ReactiveProperty<int>();
        roomPlayerCount.Value = PhotonNetwork.playerList.Length;
        IDisposable subscription = roomPlayerCount.Subscribe(x => {
			Debug.Log("PlayerValue:"+roomPlayerCount.Value);
            if (roomPlayerCount.Value > 1)
            {
				ActiveStartButton(true);
            }
			else
			{
				ActiveStartButton(false);
			}
        });
        Debug.Log("UniRxStart: " + roomPlayerCount.Value);
		isReady = true;
	}

	private void ActiveStartButton(bool isActive)
	{
		if(isActive && PhotonNetwork.isMasterClient)
		{
			startButton.interactable = true;
			touchBlockCanvas.blocksRaycasts = false;
		}
		else
		{
			startButton.interactable = false;
			touchBlockCanvas.interactable = false;
		}
	}

	public void PushStartButton()
	{
		if (!PhotonNetwork.isMasterClient) return;

		// startAnimationを流した後
		touchBlockCanvas.interactable = false;
		touchBlockCanvas.blocksRaycasts = false;
		joystickCanvasGroup.interactable = true;
		joystickCanvasGroup.blocksRaycasts = true;
		startButton.gameObject.SetActive(false);
	}
}