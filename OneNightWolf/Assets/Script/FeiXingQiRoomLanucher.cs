using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FeiXingQiRoomLanucher : MonoBehaviourPunCallbacks
{
    //-----------Proto---------------------
    private const byte IS_READY_NTF = 0;// follower 2 master
    private const byte IS_NOT_READY_NTF = 1;
    private const byte GET_READY_REQ = 2;// follower 2 master
    private const byte GET_SEAT_REQ = 3;
    private const byte GET_SEAT_ACK = 4;
    private const byte GET_SEAT_NTF = 5;
    private const byte GET_ALL_SEAT_REQ = 6;
    private const byte STATE_DESK_NTF = 7;
    private const byte PICK_CARD_REQ = 8;
    private const byte PICK_CARD_ACK = 9;
    private const byte PICK_CARD_NTF = 10;
    private const byte PLACE_CARD_REQ = 11;
    private const byte PLACE_CARD_ACK = 12;
    private const byte LANG_REN_REQ = 13;
    private const byte LANG_REN_ACK = 14;
    //-------------------------------------

    //-----------GameObject----------------
    

    public GameObject playerNamePrefab;
    public Transform playerNameGridLayout;
    public GameObject playerNumText;

    public GameObject readyOrStartButton;

    public GameObject playerSeat0;
    public GameObject playerSeat1;
    public GameObject playerSeat2;
    public GameObject playerSeat3;

    
    private GameObject GetPlayerSeat(int x)
    {
        switch (x)
        {
            case 0:
                return playerSeat0;
            case 1:
                return playerSeat1;
            case 2:
                return playerSeat2;
            case 3:
                return playerSeat3;
            default:
                return playerSeat0;
        }
    }
    
    //-------------------------------------
    private int frame = 0;

    private bool inited=false;
    public Photon.Realtime.Player[] playerList;
    public Dictionary<int,Player> playerMap;
    public Dictionary<int, int> readyMap;
    public Dictionary<int, int> seatMap;
    public int mySeat=-1;
    public Room currentRoom;
    public int playerCnt;
    public int playerLimit;
    public bool isMaster;
    public bool isReady;
    public int actorID;


    private void Awake()
    {
        UnityEngine.Random.InitState((int)System.DateTime.Now.Ticks);
        playerMap = new Dictionary<int, Player>();
        readyMap = new Dictionary<int, int>();
        //seatMap = new Dictionary<int, int>();
    }
    private void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += NetworkingClient_EventReceived;
    }

    private void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= NetworkingClient_EventReceived;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        frame++;
        if (!inited)
        {
            if (initRoom())
            {
                inited = true;
            }
        }

        if (frame == 20000)
        {
            frame = 0;
            RefreshRoomData();
            RefreshRoomView();
        }
        
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        object[] oData = { mySeat, actorID };
        PhotonNetwork.RaiseEvent(GET_SEAT_NTF, oData, RaiseEventOptions.Default, SendOptions.SendReliable);
        
        RefreshRoomData();
        RefreshRoomView();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        RefreshRoomData();
        RefreshRoomView();
    }

    bool initRoom()
    {
        if (!PhotonNetwork.InRoom)
        {
            return false;
        }
        
        InitRoomData();
        RefreshRoomData();
        RefreshRoomView();
        return true;
    }

    public void OnClickReadyOrStartButton()
    {
        if (isMaster)
        {
            int cnt = 0;
            foreach (var r in playerMap)
            {
                if (readyMap.ContainsKey(r.Key))
                {
                    cnt++;
                }
            }
            if (cnt >= playerLimit - 1)
            {
                
            }
        }

        else
        {
            if (!isReady)
            {
                isReady = true;
                object oData = actorID;
                PhotonNetwork.RaiseEvent(IS_READY_NTF, oData, RaiseEventOptions.Default, SendOptions.SendReliable);

            }
            else
            {
                isReady = false;
                object oData = actorID;
                PhotonNetwork.RaiseEvent(IS_NOT_READY_NTF, oData, RaiseEventOptions.Default, SendOptions.SendReliable);
            }

        }
        RefreshRoomView();
    }

    //---------------Receive-------------
    void NetworkingClient_EventReceived(EventData obj)
    {
        switch (obj.Code)
        {
            case IS_READY_NTF:
                Debug.Log("Receive IS_READY_NTF");
                int ac1 = (int)obj.CustomData;
                if (!playerMap.ContainsKey(ac1)) break;
                if (!readyMap.ContainsKey(ac1))
                {
                    readyMap.Add(ac1, 1);
                }
                break;
            case IS_NOT_READY_NTF:
                Debug.Log("Receive IS_NOT_READY_NTF");
                int ac2 = (int)obj.CustomData;
                if (!playerMap.ContainsKey(ac2)) break;
                if (readyMap.ContainsKey(ac2))
                {
                    readyMap.Remove(ac2);
                }
                break;
            case GET_READY_REQ:
                Debug.Log("Receive GET_READY_REQ");
                object oData1 = (object)actorID;
                if (isReady)
                    PhotonNetwork.RaiseEvent(IS_READY_NTF, oData1, RaiseEventOptions.Default, SendOptions.SendReliable);
                else
                    PhotonNetwork.RaiseEvent(IS_NOT_READY_NTF, oData1, RaiseEventOptions.Default, SendOptions.SendReliable);
                break;
            case GET_SEAT_REQ:
                if (!isMaster) break;
                int seatPos = 0;
                for (int i = 0; i <= playerLimit-1; i++)
                {
                    if (!seatMap.ContainsKey(i))
                    {
                        seatPos = i;
                        break;
                    }
                    if (!playerMap.ContainsKey(seatMap[i]))
                    {
                        seatPos = i;
                        break;
                    }
                }
                int ac3 = (int)obj.CustomData;
                object[] oData2 = { seatPos, ac3 };
                Debug.Log("Receive GET_SEAT_REQ:"+ ac3.ToString());
                Debug.Log("Send GET_SEAT_ACK, pos:" + seatPos.ToString()
                    + ",id:" + ac3.ToString());
                PhotonNetwork.RaiseEvent(GET_SEAT_ACK, oData2, RaiseEventOptions.Default, SendOptions.SendReliable);
                break;
            case GET_SEAT_ACK:
                if (isMaster) break;
                object[] get_seat_ack_list = (object[])obj.CustomData;
                int get_seat_ack_seat_pos = (int)get_seat_ack_list[0];
                int get_seat_ack_actorID = (int)get_seat_ack_list[1];
                Debug.Log("Receive GET_SEAT_ACK, pos:"+get_seat_ack_seat_pos.ToString()
                    +",id:"+get_seat_ack_actorID.ToString());
                if (actorID != get_seat_ack_actorID) break;
                mySeat = get_seat_ack_seat_pos;
                if (!seatMap.ContainsKey(mySeat))
                {
                    seatMap.Add(mySeat, actorID);
                }
                else
                {
                    seatMap[mySeat] = actorID;
                }
                object[] oData3 = { mySeat, actorID };
                Debug.Log("Send GET_SEAT_NTF, pos:" + mySeat.ToString()
                    + ",id:" + actorID.ToString());
                PhotonNetwork.RaiseEvent(GET_SEAT_NTF, oData3, RaiseEventOptions.Default, SendOptions.SendReliable);
                break;
            case GET_SEAT_NTF:
                object[] get_seat_ntf_list = (object[])obj.CustomData;
                int get_seat_ntf_seat_pos = (int)get_seat_ntf_list[0];
                int get_seat_ntf_actorID = (int)get_seat_ntf_list[1];
                Debug.Log("Receive GET_SEAT_NTF, pos:" + get_seat_ntf_seat_pos.ToString()
                    + ",id:" + get_seat_ntf_actorID.ToString());
                if (!seatMap.ContainsKey(get_seat_ntf_seat_pos))
                {
                    seatMap.Add(get_seat_ntf_seat_pos, get_seat_ntf_actorID);
                }
                else
                {
                    seatMap[get_seat_ntf_seat_pos] = get_seat_ntf_actorID;
                }
                break;
            case GET_ALL_SEAT_REQ:
                if (mySeat < 0) break;
                object[] oData4 = { mySeat, actorID };
                Debug.Log("Send GET_SEAT_NTF, pos:" + mySeat.ToString()
                    + ",id:" + actorID.ToString());
                PhotonNetwork.RaiseEvent(GET_SEAT_NTF, oData4, RaiseEventOptions.Default, SendOptions.SendReliable);
                break;
            case STATE_DESK_NTF:
                if (isMaster) break;
                //todo
                break;
           
            
        }
        RefreshRoomView();
        //throw new NotImplementedException();
    }














    //-----------------------------------
    private void InitRoomData()
    {
        playerList = PhotonNetwork.PlayerList;
        currentRoom = PhotonNetwork.CurrentRoom;
        playerLimit = currentRoom.MaxPlayers;
        playerCnt = currentRoom.PlayerCount;
        playerMap = currentRoom.Players;
        isMaster = PhotonNetwork.IsMasterClient;
        actorID = PhotonNetwork.LocalPlayer.ActorNumber;
        isReady = false;
        object oData1 = null;
        PhotonNetwork.RaiseEvent(GET_READY_REQ, oData1, RaiseEventOptions.Default, SendOptions.SendReliable);
        seatMap = new Dictionary<int, int>();

        if (isMaster)
        {
            mySeat = UnityEngine.Random.Range(0, playerLimit);
            if (!seatMap.ContainsKey(mySeat))
            {
                seatMap.Add(mySeat, actorID);
            }
            else
            {
                seatMap[mySeat] = actorID;
            }
            
            object[] oData2 = { mySeat, actorID };
            PhotonNetwork.RaiseEvent(GET_SEAT_NTF, oData2, RaiseEventOptions.Default, SendOptions.SendReliable);
        }
        else
        {
            object oData3 = actorID;
            PhotonNetwork.RaiseEvent(GET_SEAT_REQ, oData3, RaiseEventOptions.Default, SendOptions.SendReliable);
            PhotonNetwork.RaiseEvent(GET_ALL_SEAT_REQ, null, RaiseEventOptions.Default, SendOptions.SendReliable);
        }

    }
    private void RefreshRoomData()
    {
        playerList = PhotonNetwork.PlayerList;
        currentRoom = PhotonNetwork.CurrentRoom;
        playerLimit = currentRoom.MaxPlayers;
        playerCnt = currentRoom.PlayerCount;
        playerMap = currentRoom.Players;
        isMaster = PhotonNetwork.IsMasterClient;
        actorID = PhotonNetwork.LocalPlayer.ActorNumber;
        for (int i = 0; i <= playerLimit; i++)
        {
            if (!seatMap.ContainsKey(i)) continue;
            if (!playerMap.ContainsKey(seatMap[i]))
            {
                seatMap.Remove(i);
            }
        }
        
    }
    

    private void RefreshRoomView()
    {
        RefreshPlayScollerView();
        RefreshReadyOrStartButton();
        RefreshSeatView();
    }
    private void RefreshPlayScollerView()
    {
        GameObject[] pr = GameObject.FindGameObjectsWithTag("PlayerName");
        foreach (var r in pr)
        {
            Destroy(r);
        }
        
        foreach (var r in playerMap)
        {
            GameObject newPlayer = Instantiate(playerNamePrefab, playerNameGridLayout.position, Quaternion.identity, playerNameGridLayout.transform);
            string name = r.Value.NickName;
            if (r.Value.IsMasterClient)
            {
                name += "(房主)";
            }
            else
            {
                if (actorID == r.Value.ActorNumber)
                {
                    if (isReady)
                    {
                        name += "(已准备)";
                    }
                    else
                    {
                        name += "(未准备)";
                    }
                }
                else
                {
                    if (readyMap.ContainsKey(r.Key))
                    {
                        name += "(已准备)";
                    }
                    else
                    {
                        name += "(未准备)";
                    }
                }
            }
            newPlayer.GetComponentInChildren<Text>().text = name;
            Debug.Log("playername:" + r.Value.NickName);
        }
        playerNumText.GetComponent<Text>().text = "房间人数: " + playerMap.Count.ToString() + "/" + playerLimit.ToString();
    }

    public void RefreshReadyOrStartButton()
    {
        string msg = "";
        if (isMaster)
        {
            msg = "开始";
        }
        else
        {
            if (!isReady)
            {
                msg = "准备";
            }
            else
            {
                msg = "已准备";
            }
        }
        readyOrStartButton.GetComponentInChildren<Text>().text = msg;
    }

    private void RefreshSeatView()
    {
        for (int i = 0; i <= playerLimit-1; i++)
        {
            GameObject target = GetPlayerSeat(i);
            target.SetActive(false);
        }
        foreach (var r in seatMap)
        {
            Debug.Log("seat:" + r.Key.ToString() + "id:" + r.Value.ToString()+"name:"+playerMap[r.Value].NickName);
            if (!playerMap.ContainsKey(r.Value)) continue;
            GameObject target = GetPlayerSeat(r.Key);
            string name = playerMap[r.Value].NickName;
            string seat = "("+(r.Key+1).ToString()+"号位)";
            target.GetComponentInChildren<Text>().text = name+seat;
            target.SetActive(true);
        }
    }
}
