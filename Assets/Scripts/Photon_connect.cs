using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
public class Photon_connect : MonoBehaviour
{
	[SerializeField]
	private string m_resourcePath = "";
	[SerializeField]
	private float m_randomCircle = 4.0f;
	private string ROOM_NAME = "";
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


	}
	

	void OnJoinedLobby()
    {
        Debug.Log("OnJoinedLobby() was called by PUN.");

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;
        roomOptions.IsOpen = true;
        roomOptions.IsVisible = true;
        PhotonNetwork.JoinRandomRoom();
    }

    void OnPhotonRandomJoinFailed()
    {

        foreach (string str in Enumerable.Range(0, 1).Select((n) => Path.GetRandomFileName()))
        {
            ROOM_NAME = str;
            Debug.Log(ROOM_NAME);
        }
		RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;
        roomOptions.IsOpen = true;
        roomOptions.IsVisible = true;

        PhotonNetwork.CreateRoom(ROOM_NAME, roomOptions, TypedLobby.Default);
        Debug.Log("ルーム入室に失敗したのでルームを作成します");
    }








	public void OnJoinedRoom()
	{
		Debug.Log("OnJoinedRoom() called by PUN. Now this client is in a room. From here on, your game would be running. For reference, all callbacks are listed in enum: PhotonNetworkingMessage");
		SpawnObject();
	}
	void OnReceivedRoomListUpdate()
    {
        //ルーム一覧を取る
        RoomInfo[] rooms = PhotonNetwork.GetRoomList();
        if (rooms.Length == 0)
        {
            Debug.Log("ルームが一つもありません");
        }
        else
        {
            //ルームが1件以上ある時ループでRoomInfo情報をログ出力
            for (int i = 0; i < rooms.Length; i++)
            {
                Debug.Log("RoomName:" + rooms[i].name);
            }
        }
    }

}