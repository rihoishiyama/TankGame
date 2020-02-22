using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
	[SerializeField]
	private Photon_connect connect;
	[SerializeField]
	private GameObject MainTankGameObj;
	[SerializeField]
	private GameObject joystickCanvasObj;
	[SerializeField]
	private GameObject matchingCanvas;
	[SerializeField]
	private GameObject createButtonObj;
	[SerializeField]
	private GameObject joinButtonObj;
	[SerializeField]
	private GameObject roomNameInputFieldObj;
	[SerializeField]
	private InputField inputField;
	[SerializeField]
	private GameObject warningTextObj;
	[SerializeField]
	private GameObject cautionTextObj;
	[SerializeField]
	private GameObject randomRoomObj;
	[SerializeField]
	private GameObject undoButtonObj;

	public string roomName;
	public bool isCreate;
	public bool isJoin;
	public bool isRandomJoin;
	public bool isGetRoomName;

	private void Start()
	{
		Init();
	}

	private void Init()
	{
		LoadCanvas(false, true);
		isCreate = false;
		isJoin = false;
		isGetRoomName = false;
		isRandomJoin = false;
		roomName = "";
		inputField.text = "";
		ActiveJoinOrCreateButtons(true);
		roomNameInputFieldObj.SetActive(false);
		warningTextObj.SetActive(false);
		cautionTextObj.SetActive(false);
		randomRoomObj.SetActive(false);
		undoButtonObj.SetActive(false);
	}

	private void ActiveJoinOrCreateButtons(bool active)
	{
		createButtonObj.SetActive(active);
		joinButtonObj.SetActive(active);
	}

	public void GetRoomName()
	{
		roomName = inputField.text;

		if (roomName.Length > 10 || roomName.Length == 0)
		{
			warningTextObj.SetActive(true);
			GetRoomName();
		}
		isGetRoomName = true;

		if (isGetRoomName)
		{
			LoadCanvas(true, true);
		}
	}

	public void PushCreateButton()
	{
		isCreate = true;
		isJoin = false;
		ActiveJoinOrCreateButtons(false);
		roomNameInputFieldObj.SetActive(true);
		cautionTextObj.SetActive(true);
		undoButtonObj.SetActive(true);
	}

	public void PushJoinButton()
	{
		isJoin = true;
		isCreate = false;
		ActiveJoinOrCreateButtons(false);
		roomNameInputFieldObj.SetActive(true);
		cautionTextObj.SetActive(true);
		randomRoomObj.SetActive(true);
		undoButtonObj.SetActive(true);
		Debug.Log("PushJoinButton");
	}

	public void PushRandomJoinButton()
	{
		isJoin = true;
		isRandomJoin = true;
		isCreate = false;

		LoadCanvas(true, false);
	}

	public void PushUndoButton()
	{
		undoButtonObj.SetActive(false);
		randomRoomObj.SetActive(false);
		warningTextObj.SetActive(false);
		cautionTextObj.SetActive(false);
		roomNameInputFieldObj.SetActive(false);
		isCreate = false;
		isJoin = false;
		isRandomJoin = false;
		isGetRoomName = false;
		ActiveJoinOrCreateButtons(true);
	}

	public void LoadCanvas(bool mainCanvasOn, bool matchingCanvasOn, bool joystickCanvasOn = false)
	{
		MainTankGameObj.SetActive(mainCanvasOn);
		matchingCanvas.SetActive(matchingCanvasOn);
		joystickCanvasObj.SetActive(joystickCanvasOn);
	}

	public void ReMatching()
	{
		if (connect.IsMakeRoomFailed)
		{
			Init();
		}
	}

}
