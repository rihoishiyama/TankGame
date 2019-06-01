using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Photon_connect : MonoBehaviour
{
	[SerializeField]
	private string m_resourcePath = "";
	[SerializeField]
	private float m_randomCircle = 4.0f;
	private const string ROOM_NAME = "RoomA";
	public bool AutoConnect = true;
	public byte Version = 1;
	private bool ConnectInUpdate = true;
    private PhotonView m_photonView = null;
    private Vector3[] startPos = {new Vector3(16, 0, 12), new Vector3(-16, 0, 12), new Vector3(-16, 0, -12), new Vector3(16, 0, -12) };
    // cube生成
    public void SpawnObject()
	{
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
		var rand = Random.insideUnitCircle * m_randomCircle;
		return rand;
	}
	public virtual void Update()
	{
		if (ConnectInUpdate && AutoConnect && !PhotonNetwork.connected)
		{
			Debug.Log("Update() was called by Unity. Scene is loaded. Let's connect to the Photon Master Server. Calling: PhotonNetwork.ConnectUsingSettings();");
			ConnectInUpdate = false;
			PhotonNetwork.ConnectUsingSettings(Version + "." + SceneManagerHelper.ActiveSceneBuildIndex);
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
		PhotonNetwork.JoinOrCreateRoom(ROOM_NAME, new RoomOptions(), TypedLobby.Default);
	}
	public void OnJoinedRoom()
	{
		Debug.Log("OnJoinedRoom() called by PUN. Now this client is in a room. From here on, your game would be running. For reference, all callbacks are listed in enum: PhotonNetworkingMessage");
		SpawnObject();
	}
}