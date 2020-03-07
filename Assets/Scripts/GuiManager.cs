using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuiManager : MonoBehaviour {
    public GameObject gameOverPanel;
    [SerializeField]
    private Button exitButton;

    // Use this for initialization
    void Start () 
    {
        gameOverPanel.SetActive(false);
        exitButton.onClick.AddListener(() =>
        {
            PhotonNetwork.LeaveRoom();
            //マッチング画面に戻る
        });
    }

}
