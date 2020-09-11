using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class NetworkLanucher : MonoBehaviourPunCallbacks
{
    public GameObject nameUI;
    public GameObject loginUI;
    public GameObject roomList;
    public InputField roomName;
    public InputField playerName;
    public GameObject createLoading;
    public GameObject joinLoading;
    public GameObject masterLoading;
    public Dropdown dropdown;
    private bool joinLobbyReq;
    private string gameString;
    private int gameType;
    private byte roomSize;
    // Start is called before the first frame update
    void Start()
    {
        gameType = 0;
        roomSize = 6;
    }

    public void OnTypeValueChange(int n)
    {
        gameType = dropdown.value;
        Debug.Log("type:" + gameType);
        switch (gameType)
        {
            case 0://狼人杀
                roomSize = 6;
                break;
            case 1://阿瓦隆
                roomSize = 6;
                break;
            case 2://飞行棋
                roomSize = 4;
                break;
            case 3://达芬奇密码
                roomSize = 2;
                break;
            default:
                roomSize = 6;
                break;
        }
        Debug.Log("roomSize:" + roomSize);
    }

    void Update()
    {
        if (!PhotonNetwork.IsConnected)
        {
            Debug.Log("try connect to master");
            masterLoading.SetActive(true);
            joinLoading.SetActive(false);
            createLoading.SetActive(false);
            nameUI.SetActive(false);
            loginUI.SetActive(false);
            roomList.SetActive(false);
            joinLobbyReq = false;
            PhotonNetwork.ConnectUsingSettings();
            
        }
    
    }



    public override void OnConnectedToMaster()
    {
        //base.OnConnectedToMaster();
        Debug.Log("is connected");
        nameUI.SetActive(true);
        loginUI.SetActive(false);
        roomList.SetActive(false);
        createLoading.SetActive(false);
        joinLoading.SetActive(false);
        masterLoading.SetActive(false);
    }


    public void PlayButton()
    {
        if (!PhotonNetwork.IsConnected) return;
        if (playerName.text.Length < 2) return;
        PhotonNetwork.NickName = playerName.text;
        PhotonNetwork.JoinLobby();
        Debug.Log(PhotonNetwork.NickName);
       
        roomList.SetActive(true);
        nameUI.SetActive(false);
        loginUI.SetActive(true); 
    }

    public void JoinOrCreateButton()
    {
        if (roomName.text.Length < 2) return;
        switch (gameType)
        {
            case 0:
                gameString = "(一夜狼)";
                break;
            case 1:
                gameString = "(阿瓦隆)";
                break;
            case 2:
                gameString = "(飞行棋)";
                break;
            case 3:
                gameString = "(达芬奇密码)";
                break;
            default:
                gameString = "(一夜狼)";
                break;
        }
        RoomOptions options = new RoomOptions { MaxPlayers = roomSize , PublishUserId = true};
        CanvasGroup[] loginUIcanvas = loginUI.GetComponentsInChildren<CanvasGroup>();
        roomList.GetComponent<CanvasGroup>().alpha = 0.0f;
        for (int i = 0; i < loginUIcanvas.Length; i++)
        {
            loginUIcanvas[i].alpha = 0.0f;
        }
        createLoading.SetActive(true);
        PhotonNetwork.JoinOrCreateRoom(roomName.text + gameString, options, default);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        CanvasGroup[] loginUIcanvas = loginUI.GetComponentsInChildren<CanvasGroup>();
        roomList.GetComponent<CanvasGroup>().alpha = 1.0f;
        for (int i = 0; i < loginUIcanvas.Length; i++)
        {
            loginUIcanvas[i].alpha = 1.0f;
        }
        createLoading.SetActive(false);
    }

    public override void OnJoinedRoom()
    {
        Room cur_room = PhotonNetwork.CurrentRoom;
        string name = cur_room.Name;
        Debug.Log(name);
        if (name.EndsWith("(一夜狼)"))
            PhotonNetwork.LoadLevel(1);
        else
        if (name.EndsWith("(阿瓦隆)"))
            PhotonNetwork.LoadLevel(1);
        else
        if (name.EndsWith("(飞行棋)"))
            PhotonNetwork.LoadLevel(2);
        else
        if (name.EndsWith("(达芬奇密码)"))
            PhotonNetwork.LoadLevel(3);
        //base.OnJoinedRoom();

    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        CanvasGroup[] loginUIcanvas = loginUI.GetComponentsInChildren<CanvasGroup>();
        roomList.GetComponent<CanvasGroup>().alpha = 1.0f;
        for (int i = 0; i < loginUIcanvas.Length; i++)
        {
            loginUIcanvas[i].alpha = 1.0f;
        }
        joinLoading.SetActive(false);
    }
    

    // Update is called once per frame
    
}
