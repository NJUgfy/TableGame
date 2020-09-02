using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomLanucher : MonoBehaviourPunCallbacks
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

    //player scoller
    public GameObject turioal;
    public GameObject time1;
    public GameObject time2;

    public GameObject playerNamePrefab;
    public Transform playerNameGridLayout;
    public GameObject playerNumText;

    public GameObject readyOrStartButton;

    public GameObject playerSeat0;
    public GameObject playerSeat1;
    public GameObject playerSeat2;
    public GameObject playerSeat3;
    public GameObject playerSeat4;
    public GameObject playerSeat5;

    public enum GameState
    {
        START = 0,
        DESK = 1,
        PLAYER = 2,
        LANG_REN = 3,
        ZHUA_YA =4
    }

    public enum PosType
    {
        START = 0,
        DESK = 1,
        PLAYER = 2
    }

    public enum Role
    {
        HUA_SHEN_YOU_LING = 0,
        SHI_MIAN_ZHE = 1,
        QIANG_DAO = 2,
        DAO_DAN_GUI = 3,
        ZHUA_YA = 4,
        LANG_REN_1 = 5,
        LANG_REN_2 = 6,
        JIU_GUI = 7,
        YU_YAN_JIA = 8
    }

    public Role startRole;

    public GameObject card0;
    public GameObject card1;
    public GameObject card2;
    public GameObject card3;
    public GameObject card4;
    public GameObject card5;
    public GameObject card6;
    public GameObject card7;
    public GameObject card8;

    public GameObject cardPtr;

    public Sprite num0;
    public Sprite num1;
    public Sprite num2;
    public Sprite num3;
    public Sprite num4;
    public Sprite num5;
    public Sprite num6;
    public Sprite num7;
    public Sprite num8;
    public Sprite num9;

    public Sprite not_ready_tips;
    public Sprite ready_tips;
    public Sprite check_card_tips;
    public Sprite lang_ren_tips;




    private void SetCardState(GameState state)
    {
        if (state == GameState.PLAYER)
        {
            SolveNoPicked();
        }
        card0.GetComponent<CardEvent>().SetState(state: state);
        card1.GetComponent<CardEvent>().SetState(state: state);
        card2.GetComponent<CardEvent>().SetState(state: state);
        card3.GetComponent<CardEvent>().SetState(state: state);
        card4.GetComponent<CardEvent>().SetState(state: state);
        card5.GetComponent<CardEvent>().SetState(state: state);
        card6.GetComponent<CardEvent>().SetState(state: state);
        card7.GetComponent<CardEvent>().SetState(state: state);
        card8.GetComponent<CardEvent>().SetState(state: state);
    }
    private void SolveNoPicked()
    {
        int cnt = 6;
        if (card0.GetComponent<CardEvent>().playerPos == -1)
        {
            card0.GetComponent<CardEvent>().OnOthersPick(cnt);
            Role rl = cardRoleMap[0];
            if (!playerRoleMap.ContainsKey(cnt))
            {
                playerRoleMap.Add(cnt, rl);
            }
            else
            {
                playerRoleMap[cnt] = rl;
            }
            cnt++;
        }
        if (card1.GetComponent<CardEvent>().playerPos == -1)
        {
            card1.GetComponent<CardEvent>().OnOthersPick(cnt);
            Role rl = cardRoleMap[1];
            if (!playerRoleMap.ContainsKey(cnt))
            {
                playerRoleMap.Add(cnt, rl);
            }
            else
            {
                playerRoleMap[cnt] = rl;
            }
            cnt++;
        }
        if (card2.GetComponent<CardEvent>().playerPos == -1)
        {
            card2.GetComponent<CardEvent>().OnOthersPick(cnt);
            Role rl = cardRoleMap[2];
            if (!playerRoleMap.ContainsKey(cnt))
            {
                playerRoleMap.Add(cnt, rl);
            }
            else
            {
                playerRoleMap[cnt] = rl;
            }
            cnt++;
        }
        if (card3.GetComponent<CardEvent>().playerPos == -1)
        {
            card3.GetComponent<CardEvent>().OnOthersPick(cnt);
            Role rl = cardRoleMap[3];
            if (!playerRoleMap.ContainsKey(cnt))
            {
                playerRoleMap.Add(cnt, rl);
            }
            else
            {
                playerRoleMap[cnt] = rl;
            }
            cnt++;
        }
        if (card4.GetComponent<CardEvent>().playerPos == -1)
        {
            card4.GetComponent<CardEvent>().OnOthersPick(cnt);
            Role rl = cardRoleMap[4];
            if (!playerRoleMap.ContainsKey(cnt))
            {
                playerRoleMap.Add(cnt, rl);
            }
            else
            {
                playerRoleMap[cnt] = rl;
            }
            cnt++;
        }
        if (card5.GetComponent<CardEvent>().playerPos == -1)
        {
            card5.GetComponent<CardEvent>().OnOthersPick(cnt);
            Role rl = cardRoleMap[5];
            if (!playerRoleMap.ContainsKey(cnt))
            {
                playerRoleMap.Add(cnt, rl);
            }
            else
            {
                playerRoleMap[cnt] = rl;
            }
            cnt++;
        }
        if (card6.GetComponent<CardEvent>().playerPos == -1)
        {
            card6.GetComponent<CardEvent>().OnOthersPick(cnt);
            Role rl = cardRoleMap[6];
            if (!playerRoleMap.ContainsKey(cnt))
            {
                playerRoleMap.Add(cnt, rl);
            }
            else
            {
                playerRoleMap[cnt] = rl;
            }
            cnt++;
        }
        if (card7.GetComponent<CardEvent>().playerPos == -1)
        {
            card7.GetComponent<CardEvent>().OnOthersPick(cnt);
            Role rl = cardRoleMap[7];
            if (!playerRoleMap.ContainsKey(cnt))
            {
                playerRoleMap.Add(cnt, rl);
            }
            else
            {
                playerRoleMap[cnt] = rl;
            }
            cnt++;
        }
        if (card8.GetComponent<CardEvent>().playerPos == -1)
        {
            card8.GetComponent<CardEvent>().OnOthersPick(cnt);
            Role rl = cardRoleMap[8];
            if (!playerRoleMap.ContainsKey(cnt))
            {
                playerRoleMap.Add(cnt, rl);
            }
            else
            {
                playerRoleMap[cnt] = rl;
            }
            cnt++;
        }

    }
    private void PickOthersCard(int seatID, int cardPos)
    {//id: 玩家座位 pos:卡
        Role rl = cardRoleMap[cardPos];
        seatID = (seatID - mySeat + playerLimit) % playerLimit;
        if (isMaster)
        {
            if (!playerRoleMap.ContainsKey(seatID))
            {
                playerRoleMap.Add(seatID, rl);
            }
            else
            {
                playerRoleMap[seatID] = rl;
            }
        }
        Debug.Log(seatID + "pick a card");
        switch (cardPos)
        {
            case 0:
                card0.GetComponent<CardEvent>().OnOthersPick(seatID);
                break;
            case 1:
                card1.GetComponent<CardEvent>().OnOthersPick(seatID);
                break;
            case 2:
                card2.GetComponent<CardEvent>().OnOthersPick(seatID);
                break;
            case 3:
                card3.GetComponent<CardEvent>().OnOthersPick(seatID);
                break;
            case 4:
                card4.GetComponent<CardEvent>().OnOthersPick(seatID);
                break;
            case 5:
                card5.GetComponent<CardEvent>().OnOthersPick(seatID);
                break;
            case 6:
                card6.GetComponent<CardEvent>().OnOthersPick(seatID);
                break;
            case 7:
                card7.GetComponent<CardEvent>().OnOthersPick(seatID);
                break;
            case 8:
                card8.GetComponent<CardEvent>().OnOthersPick(seatID);
                break;
        }

    }
    private void PickMyCard(int id, Role rl)
    {
        startRole = rl;
        if (isMaster)
        {
            cardRoleMap[id] = rl;
            if (!playerRoleMap.ContainsKey(0))
            {
                playerRoleMap.Add(0, rl);
            }
            else
            {
                playerRoleMap[0] = rl;
            }
            
        }
        switch (id)
        {
            case 0:
                card0.GetComponent<CardEvent>().OnMyPick(0);
                card0.GetComponent<CardEvent>().SetRole(startRole);
                break;
            case 1:
                card1.GetComponent<CardEvent>().OnMyPick(0);
                card1.GetComponent<CardEvent>().SetRole(startRole);
                break;
            case 2:
                card2.GetComponent<CardEvent>().OnMyPick(0);
                card2.GetComponent<CardEvent>().SetRole(startRole);
                break;
            case 3:
                card3.GetComponent<CardEvent>().OnMyPick(0);
                card3.GetComponent<CardEvent>().SetRole(startRole);
                break;
            case 4:
                card4.GetComponent<CardEvent>().OnMyPick(0);
                card4.GetComponent<CardEvent>().SetRole(startRole);
                break;
            case 5:
                card5.GetComponent<CardEvent>().OnMyPick(0);
                card5.GetComponent<CardEvent>().SetRole(startRole);
                break;
            case 6:
                card6.GetComponent<CardEvent>().OnMyPick(0);
                card6.GetComponent<CardEvent>().SetRole(startRole);
                break;
            case 7:
                card7.GetComponent<CardEvent>().OnMyPick(0);
                card7.GetComponent<CardEvent>().SetRole(startRole);
                break;
            case 8:
                card8.GetComponent<CardEvent>().OnMyPick(0);
                card8.GetComponent<CardEvent>().SetRole(startRole);
                break;
        }
        
    }
    private void CreateRole()
    {
        UnityEngine.Random.InitState((Time.frameCount)%1000000);
        cardRoleMap = new Dictionary<int, Role>();
        foreach (Role r in Enum.GetValues(typeof(Role)))
        {
            int pos = UnityEngine.Random.Range(0, 9);
            while (cardRoleMap.ContainsKey(pos))
            {
                pos= UnityEngine.Random.Range(0, 9);
            }
            cardRoleMap.Add(pos, r);
        }
    }
    private void SetGameState(GameState state)
    {
        gameState = state;
        if (state == GameState.START)
        {
            turioal.GetComponent<Image>().sprite = not_ready_tips;
        }
        if (state == GameState.DESK)
        {
            turioal.GetComponent<Image>().sprite = ready_tips;
            CreateRole();
        }
        if (state == GameState.PLAYER)
        {
            turioal.GetComponent<Image>().sprite = ready_tips;
        }
        if (state == GameState.LANG_REN)
        {
            turioal.GetComponent<Image>().sprite = lang_ren_tips;
            round_frame = 60 * 15;
        }
        SetCardState(gameState);
    }

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
            case 4:
                return playerSeat4;
            case 5:
                return playerSeat5;
            default:
                return playerSeat0;
        }
    }

    private void SetHuman(int pos)
    {
        playerSeat0.GetComponent<PlayerEvent>().TurnToHuman();
        playerSeat1.GetComponent<PlayerEvent>().TurnToHuman();
        playerSeat2.GetComponent<PlayerEvent>().TurnToHuman();
        playerSeat3.GetComponent<PlayerEvent>().TurnToHuman();
        playerSeat4.GetComponent<PlayerEvent>().TurnToHuman();
        playerSeat5.GetComponent<PlayerEvent>().TurnToHuman();
    }
    private void SetWolf(int pos)
    {
        switch (pos)
        {
            case 0:
                playerSeat0.GetComponent<PlayerEvent>().TurnToWolf();
                break;
            case 1:
                playerSeat1.GetComponent<PlayerEvent>().TurnToWolf();
                break;
            case 2:
                playerSeat2.GetComponent<PlayerEvent>().TurnToWolf();
                break;
            case 3:
                playerSeat3.GetComponent<PlayerEvent>().TurnToWolf();
                break;
            case 4:
                playerSeat4.GetComponent<PlayerEvent>().TurnToWolf();
                break;
            case 5:
                playerSeat5.GetComponent<PlayerEvent>().TurnToWolf();
                break;
        }
    }

    private void lang_ren_follower_round(int pos0,int pos1)
    {
        if (isMaster) return;
        SetWolf(0);
        if (pos1 == -1)
        {
            lang_ren_num = 1;
            return;
        }
        lang_ren_num = 2;
        pos1 = pos1 - pos0 + mySeat;
        pos1 = (pos1 + playerLimit) % playerLimit;
        SetWolf(pos1);
    }
    private void lang_ren_master_round()
    {
        if (!isMaster) return;
        int pos1 = -1;
        int pos2 = -1;
        foreach (var r in playerRoleMap)
        {
            Debug.Log("pos:" + r.Key+"role:"+r.Value);
            int pos = r.Key;
            if (pos >= 6) continue;
            if (r.Value != Role.LANG_REN_1 && r.Value != Role.LANG_REN_2) continue;
            Debug.Log("pos:" + pos);
            if (pos1 == -1) pos1 = pos;
            else pos2 = pos;
        }
        if (startRole == Role.LANG_REN_1 || startRole == Role.LANG_REN_2)
        {
            if (pos1 != -1)
                SetWolf(pos1);
            if (pos2 != -1)
                SetWolf(pos2);
        }
        object[] obj = { mySeat, pos1, pos2 };
        Debug.Log("send LANG_REN_REQ");
        Debug.Log("pos1:" + pos1);
        Debug.Log("pos2:" + pos2);
        PhotonNetwork.RaiseEvent(LANG_REN_REQ, obj, RaiseEventOptions.Default, SendOptions.SendReliable);
        SetGameState(GameState.LANG_REN);
    }

    private void CheckAllPicked()
    {
        int cnt = 0;
        foreach(var r in playerMap)
        {
            int acID = r.Value.ActorNumber;
            bool pd2 = false;
            foreach (var l in cardPickMap)
            {
                if (l.Value == acID)
                {
                    pd2 = true;
                    cnt++;
                    break;
                }
            }
            if (pd2 == false)
            {
                return;
            }
        }
        placeNum = playerLimit - 1;
        Debug.Log(cnt);
        Debug.Log(placeNum);
        if (playerLimit != cnt) return;
        PhotonNetwork.RaiseEvent(PLACE_CARD_REQ, null, RaiseEventOptions.Default, SendOptions.SendReliable);
        SetGameState(GameState.PLAYER);
    }

    public int GetCardPos(int cardID)
    {
        switch (cardID)
        {
            case 0:
                return card0.GetComponent<CardEvent>().GetPosID();
            case 1:
                return card1.GetComponent<CardEvent>().GetPosID();
            case 2:
                return card2.GetComponent<CardEvent>().GetPosID();
            case 3:
                return card3.GetComponent<CardEvent>().GetPosID();
            case 4:
                return card4.GetComponent<CardEvent>().GetPosID();
            case 5:
                return card5.GetComponent<CardEvent>().GetPosID();
            case 6:
                return card6.GetComponent<CardEvent>().GetPosID();
            case 7:
                return card7.GetComponent<CardEvent>().GetPosID();
            case 8:
                return card8.GetComponent<CardEvent>().GetPosID();
        }
        return -1;
    }

    public void OnClickCard(int cardID)
    {
        if (gameState == GameState.START) return;
        if (gameState == GameState.DESK)
        {
            if (hasCard) return;
            if (isMaster)
            {
                if (!cardPickMap.ContainsKey(cardID))
                {
                    cardPickMap.Add(cardID, actorID);
                } else if (!playerMap.ContainsKey(cardPickMap[cardID]))
                {
                    cardPickMap[cardID] = actorID;
                }
                else
                {
                    return;
                }
                hasCard = true;
                PickMyCard(cardID, cardRoleMap[cardID]);
                object[] oData = { cardID, actorID, mySeat};
                PhotonNetwork.RaiseEvent(PICK_CARD_NTF, oData, RaiseEventOptions.Default, SendOptions.SendReliable);
                CheckAllPicked();
            }
            else
            {
                object[] oData = { cardID, actorID };
                PhotonNetwork.RaiseEvent(PICK_CARD_REQ, oData, RaiseEventOptions.Default, SendOptions.SendReliable);
            }
        }
        if (gameState == GameState.LANG_REN)
        {
            if (startRole == Role.LANG_REN_1 || startRole == Role.LANG_REN_2)
            {
                if (lang_ren_num != 1) return;
                if (lang_ren_picked == 1) return;
                lang_ren_picked = 1;
                if (isMaster)
                {
                    Role picked_role = cardRoleMap[cardID];
                    PickMyCard(cardID, picked_role);
                }
                else
                {
                    int pos = GetCardPos(cardID);
                    //PhotonNetwork.RaiseEvent(LANG_REN_PICK_REQ, oData, RaiseEventOptions.Default, SendOptions.SendReliable);
                }
            }
        }
    }

    private void OnStateDeskMaster()
    {
        cardPickMap = new Dictionary<int, int>();
        PhotonNetwork.RaiseEvent(STATE_DESK_NTF, null, RaiseEventOptions.Default, SendOptions.SendReliable);
        SetGameState(GameState.DESK);

    }
    private void OnStateDeskFollower()
    {
        SetGameState(GameState.DESK);
    }

    //-------------------------------------
    private int frame = 0;

    private bool inited=false;
    public Photon.Realtime.Player[] playerList;
    public Dictionary<int,Player> playerMap;
    public Dictionary<int, int> readyMap;
    public Dictionary<int, int> seatMap;
    public Dictionary<int, int> cardPickMap;
    public Dictionary<int, Role> cardRoleMap;
    public Dictionary<int, Role> seatRoleMap;
    public Dictionary<int, Role> playerRoleMap;
    public int mySeat=-1;
    public Room currentRoom;
    public int playerCnt;
    public int playerLimit;
    public bool isMaster;
    public bool isReady;
    public int actorID;
    public GameState gameState;
    private bool hasCard;
    public int placeNum = 0;
    public int lang_ren_num = 0;
    public int zhua_ya_num = 0;
    public int round_frame = 0;
    public int lang_ren_picked = 0;


    private void Awake()
    {
        UnityEngine.Random.InitState((int)System.DateTime.Now.Ticks);
        playerMap = new Dictionary<int, Player>();
        readyMap = new Dictionary<int, int>();
        seatMap = new Dictionary<int, int>();
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
    Sprite GetNum(int x)
    {
        switch (x)
        {
            case 0:
                return num0;
            case 1:
                return num1;
            case 2:
                return num2;
            case 3:
                return num3;
            case 4:
                return num4;
            case 5:
                return num5;
            case 6:
                return num6;
            case 7:
                return num7;
            case 8:
                return num8;
            case 9:
                return num9;
        }
        return null;
    }
    void ShowTime()
    {
        int t = round_frame / 60;
        int x = t / 10;
        int y = t % 10;
        if (x == 0 && y == 0)
        {
            time1.SetActive(false);
            time2.SetActive(false);
            return;
        }
        time1.SetActive(true);
        time2.SetActive(true);
        time1.GetComponent<Image>().sprite = GetNum(x);
        time2.GetComponent<Image>().sprite = GetNum(y);
    }

    void zhua_ya_master_round()
    {

    }

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
        ShowTime();
        if (round_frame > 0)
        {
            round_frame--;
            if (round_frame == 0)
            {
                if (gameState == GameState.PLAYER)
                {
                    lang_ren_master_round();
                }
                else
                if (gameState == GameState.LANG_REN && lang_ren_num!=1)
                {
                    zhua_ya_master_round();
                }
            }
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
                OnStateDeskMaster();
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
                for (int i = 0; i <= 5; i++)
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
                OnStateDeskFollower();
                break;
            case PICK_CARD_REQ:
                if (!isMaster) break;
                object[] pick_card_req_list = (object[])obj.CustomData;
                int pos = (int)pick_card_req_list[0];
                int id = (int)pick_card_req_list[1];
                Role rl = cardRoleMap[pos];
                object[] pick_card_ack_data = { pos, id, rl, true };
                Debug.Log(pick_card_ack_data);
                if (cardPickMap.ContainsKey(pos))
                {
                    pick_card_ack_data[3] = false;
                    PhotonNetwork.RaiseEvent(PICK_CARD_ACK, pick_card_ack_data, RaiseEventOptions.Default, SendOptions.SendReliable);
                    break;
                }
                cardPickMap.Add(pos, id);
                PhotonNetwork.RaiseEvent(PICK_CARD_ACK, pick_card_ack_data, RaiseEventOptions.Default, SendOptions.SendReliable);
                break;
            case PICK_CARD_ACK:
                if (isMaster) break;
                Debug.Log("receive pick_card_ack");
                object[] pick_card_ack_list = (object[])obj.CustomData;
                int pick_card_ack_pos = (int)pick_card_ack_list[0];
                int pick_card_ack_id = (int)pick_card_ack_list[1];
                Role rol = (Role)pick_card_ack_list[2];
                bool av= (bool)pick_card_ack_list[3];
                Debug.Log("pick_card_ack_list"+pick_card_ack_pos+rol);
                if (pick_card_ack_id != actorID) break;
                if (!av) break;
                hasCard = true;
                PickMyCard(pick_card_ack_pos, rol);
                object[] pick_card_ntf = { pick_card_ack_pos, pick_card_ack_id, mySeat };
                PhotonNetwork.RaiseEvent(PICK_CARD_NTF, pick_card_ntf, RaiseEventOptions.Default, SendOptions.SendReliable);
                break;
            case PICK_CARD_NTF:
                object[] pick_card_ntf_list = (object[])obj.CustomData;
                int pick_card_ntf_id = (int)pick_card_ntf_list[1];
                int pick_card_ntf_pos = (int)pick_card_ntf_list[0];
                int seatID = (int)pick_card_ntf_list[2];
                if (pick_card_ntf_id == actorID) break;
                PickOthersCard(seatID, pick_card_ntf_pos);
                if (isMaster)
                {
                    CheckAllPicked();
                }
                break;
            case PLACE_CARD_REQ:
                if (isMaster) break;
                SetGameState(GameState.PLAYER);
                PhotonNetwork.RaiseEvent(PLACE_CARD_ACK, null, RaiseEventOptions.Default, SendOptions.SendReliable);
                break;
            case PLACE_CARD_ACK:
                if (!isMaster) break;
                placeNum--;
                if (placeNum == 0)
                {
                    round_frame = 60 * 4;
                }
                break;
            case LANG_REN_REQ:
                if (isMaster) break;
                //if (startRole != Role.LANG_REN_1 || startRole != Role.LANG_REN_2) break;
                object[] lang_ren_req_list = (object[])obj.CustomData;
                int pos0 = (int)lang_ren_req_list[0];
                int pos1 = (int)lang_ren_req_list[1];
                int pos2 = (int)lang_ren_req_list[2];
                if (pos1 != -1 && ((mySeat-pos0+playerLimit)%playerLimit) == pos1)
                {
                    lang_ren_follower_round(pos0,pos2);
                }
                if (pos2 != -1 && ((mySeat - pos0 + playerLimit) % playerLimit) == pos2)
                {
                    lang_ren_follower_round(pos0,pos1);
                }
                SetGameState(GameState.LANG_REN);
                break;
            
        }
        RefreshRoomView();
        //throw new NotImplementedException();
    }














    //-----------------------------------
    private void InitRoomData()
    {
        hasCard = false;
        playerRoleMap = new Dictionary<int, Role>();
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
        SetGameState(GameState.START);

        if (isMaster)
        {
            mySeat = UnityEngine.Random.Range(0, 6);
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
        for (int i = 0; i <= 5; i++)
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
        for (int i = 0; i <= 5; i++)
        {
            GameObject target = GetPlayerSeat(i);
            target.SetActive(false);
        }
        foreach (var r in seatMap)
        {
            Debug.Log("seat:" + r.Key.ToString() + "id:" + r.Value.ToString()+"name:"+playerMap[r.Value].NickName);
            if (!playerMap.ContainsKey(r.Value)) continue;
            GameObject target = GetPlayerSeat(((r.Key-mySeat+6)%6));
            string name = playerMap[r.Value].NickName;
            string seat = "("+(r.Key+1).ToString()+"号位)";
            target.GetComponentInChildren<Text>().text = name+seat;
            target.SetActive(true);
        }
    }
}
