using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Role
{
    LANG_REN_1 = 0,
    LANG_REN_2 = 1,
    ZHUA_YA = 2,
    YU_YAN_JIA = 3,
    HUA_SHEN_YOU_LING = 4,
    QIANG_DAO = 5,
    DAO_DAN_GUI = 6,
    SHI_MIAN_ZHE = 7,
    JIU_GUI = 8,
    KA_BEI = 9
}

public enum GameState
{
    INIT = 0,
    CHOOSE = 1,
    WATCH = 2,
    LANG_REN = 3,
    ZHUA_YA = 4,
    YU_YAN_JIA = 5,
    HUA_SHEN_YOU_LING = 6,
    QIANG_DAO = 7,
    DAO_DAN_GUI = 8,
    SHI_MIAN_ZHE = 9,
    JIU_GUI = 10,
    FREE = 11,
    SHOW_RESULT = 12
}




public class State { 
    protected static State m_instance=null;
    public bool isOK = false;
    public virtual void OnStateEnter() { }  //进入此状态执行一次
    public virtual void OnUpdate() { }      //每一帧执行一次
    public virtual void OnStateExit() { }   //跳出状态时调用一次
    public virtual bool IsValid() { return true; }
    public virtual void OnReadyClick() { }
    public virtual void Role_Select(int cardID, Role role) { }
    public virtual void Role_DeSelect(int cardID){}
    public virtual void OnCardClick(int cardID) { }//点击卡牌
    public virtual void OnMySelect(int cardID) { }//我选中的卡牌
    public virtual void OnOtherSelect(int cardID) { }//其他玩家选中的卡牌
    public void SetNexState(State _state) { nex_State = _state; }
    public State GetNexState() { return nex_State; }
    private State nex_State=null;
    public GameState Estate;
    public double m_time;
    //protected int sec_frame = 144;
    public StateManager manager = null;
    public virtual void OnPlayerClick(int id) { }//选中头像
}

public class Init:State{
    public Init(StateManager _manager)
    {
        Estate = GameState.INIT;
        manager = _manager;
    }
    public override void OnStateEnter()
    {
        manager.seatMap = new Dictionary<int, int>();
        manager.readyMap = new Dictionary<int, bool>();
        manager.playerCardMap = new Dictionary<int, int>();
        manager.playerMap = new Dictionary<int, Player>();
        manager.seatRoleMap = new Dictionary<int, Role>();
        manager.playerRoleMap = new Dictionary<int, Role>();
        manager.voteMap = new Dictionary<int, int>();
        Debug.Log("初始阶段");
        m_time = -1;
        manager.CardManager.GetComponent<CardManager>().MoveAllToInit();
        manager.playerList = PhotonNetwork.PlayerList;
        manager.playerMap.Clear();
        foreach(var r in manager.playerList)
        {
            manager.playerMap.Add(r.ActorNumber, r);
        }
        manager.currentRoom = PhotonNetwork.CurrentRoom;
        manager.isMaster = PhotonNetwork.IsMasterClient;
        manager.actorID = PhotonNetwork.LocalPlayer.ActorNumber;
        manager.isReady = false;
        manager.playerLimit = manager.currentRoom.MaxPlayers;
        Debug.Log("limit:"+manager.playerLimit);
        manager.UIManager.GetComponent<UIManager>().SetIsReady(manager.isReady,manager.isMaster);
        manager.UIManager.GetComponent<UIManager>().SetPlayerLimit(manager.playerLimit);
        manager.UIManager.GetComponent<UIManager>().SetPlayerMap(manager.playerMap);
        manager.UIManager.GetComponent<UIManager>().SetReadyMap(manager.readyMap);
        manager.UIManager.GetComponent<UIManager>().SetSeatMap(manager.seatMap);
        manager.UIManager.GetComponent<UIManager>().SetState(this.Estate);
        manager.UIManager.GetComponent<UIManager>().SetTimer(m_time);

        if (manager.isMaster)
        {
            int seatPos = 0;
            for (int i = 0; i <= manager.playerLimit; i++)
            {
                if (!manager.seatMap.ContainsKey(i))
                {
                    seatPos = i;
                    break;
                }
                if (!manager.playerMap.ContainsKey(manager.seatMap[i]))
                {
                    seatPos = i;
                    break;
                }
            }
            manager.mySeatID = seatPos;
            //Debug.Log("master pos: " + seatPos);
            if (!manager.seatMap.ContainsKey(seatPos))
            {
                manager.seatMap.Add(seatPos, manager.actorID);
            }
            else
            {
                manager.seatMap[seatPos] = manager.actorID;
            }

        }
        else
        {
            manager.SendRPC(StateManager.GET_SEAT_REQ, manager.actorID);
        }
        manager.UIRefresh();
        manager.SendRPC(StateManager.GET_ALL_SEAT_REQ, null);
    }
    public override void OnReadyClick()
    {
        Debug.Log("ready click");
        if (manager.isMaster)
        {
            int cnt = 0;
            foreach (var r in manager.readyMap)
            {
                if (r.Value == false) continue;
                if (!manager.playerMap.ContainsKey(r.Key)) continue;
                cnt++;
            }
            //Debug.Log("ready Num:" + cnt);
            //Debug.Log("Limit Num:" + manager.playerLimit);
            if (cnt >= manager.playerLimit - 1)
            {
                isOK = true;
            }
        }
        else
        {
            if (manager.isReady)
            {
                manager.isReady = false;
            }
            else
            {
                manager.isReady = true;
            }
            if (manager.readyMap.ContainsKey(manager.actorID))
            {
                manager.readyMap[manager.actorID] = manager.isReady;
            }
            else
            {
                manager.readyMap.Add(manager.actorID, manager.isReady);
            }
            object[] odata = { manager.actorID, manager.isReady };
            manager.SendRPC(StateManager.READY_ACK, odata);
        }
        manager.UIRefresh();
    }

    public override void OnUpdate()
    {
        //m_time--;
    }
    public override void OnStateExit()
    {
        if (manager.isMaster)
        {
            manager.CreateRole();
            manager.playerCardMap = new Dictionary<int, int>();
            manager.playerRoleMap = new Dictionary<int, Role>();
            manager.SendRPC(StateManager.CHANGE_STATE_NTF,this.Estate);
        }
        isOK = false;
    }
    public override bool IsValid()
    {
        //if (m_time == 0) return true;
        if (isOK) return true;
        return false;
    }
}

public class Choose : State
{
    public Choose(StateManager _manager)
    {
        Estate = GameState.CHOOSE;
        manager = _manager;
    }
    public override void OnStateEnter()
    {
        Debug.Log("选牌阶段");
        m_time = 90;
        manager.DataRefresh();
        manager.UIRefresh();
        manager.CardManager.GetComponent<CardManager>().MoveAllToChoose();
        //manager.CreateRole();
    }
    public override void OnUpdate()
    {
        m_time-=Time.deltaTime;
    }
    public override void OnStateExit()
    {
        isOK = false;
        if (manager.isMaster)
        {
            manager.SendRPC(StateManager.CHANGE_STATE_NTF,this.Estate);
        }
    }
    public override bool IsValid()
    {
        if (manager.isMaster)
        {
            int cnt = manager.playerRoleMap.Count;
            if (cnt == manager.playerLimit)
            {
                return true;
            }
            return false;
        }
        if (isOK) return true;
        return false;
    }
    public override void OnCardClick(int cardID)
    {
        if (manager.isMaster)
        {
            if (manager.playerCardMap.ContainsKey(manager.actorID))
            {
                return;
            }
            bool otherSelect=false;
            foreach(var r in manager.playerCardMap)
            {
                if (r.Value == cardID)
                {
                    otherSelect = true;
                    break;
                }
            }
            if (otherSelect) return;

            manager.playerCardMap.Add(manager.actorID, cardID);
            Role _role = manager.cardRoleMap[cardID];
            manager.startRole = _role;
            manager.playerRoleMap.Add(manager.actorID, _role);

            object[] odata = { manager.actorID, cardID, _role };
            this.OnMySelect(cardID);
            manager.SendRPC(StateManager.CHOOSE_CARD_NTF, odata);
        }
        else
        {
            object[] odata = { manager.actorID, cardID };
            manager.SendRPC(StateManager.CHOOSE_CARD_REQ, odata);
        }
    }
    public override void OnMySelect(int cardID)
    {
        manager.CardManager.GetComponent<CardManager>().OnMySelect(cardID);
    }

    public override void OnOtherSelect(int cardID)
    {
        manager.CardManager.GetComponent<CardManager>().OnOtherSelect(cardID);
    }
}

public class Watch : State
{
    //public int m_showTime = -1;
    public Watch(StateManager _manager)
    {
        Estate = GameState.WATCH;
        manager = _manager;
    }
    public override void OnStateEnter()
    {
        Debug.Log("看牌阶段");
        if (manager.isMaster)
        {
            manager.seatRoleMap = new Dictionary<int, Role>();
        }
        m_time = 15.0;
        manager.DataRefresh();
        manager.UIRefresh();
        int myPos = manager.mySeatID;
        int myCard = 0;

        foreach (var r in manager.playerCardMap)
        {
            manager.CardManager.GetComponent<CardManager>().OnBanAllPick();
        }

        foreach (var r in manager.playerCardMap)
        {
            if (r.Key == manager.actorID)
            {
                myCard = r.Value;
                break;
            }
        }
        manager.CardManager.GetComponent<CardManager>().MoveToMid(myCard, manager.startRole);
        manager.CardManager.GetComponent<CardManager>().SetSeatID(myCard, 0);
        if (manager.isMaster)
        {
            manager.seatRoleMap.Add(0, manager.startRole);
        }
        

        foreach (var r in manager.playerCardMap)
        {
            if (r.Key != manager.actorID)
            {
                int seat = 0;
                foreach (var k in manager.seatMap)
                {
                    if (k.Value == r.Key)
                    {
                        seat = k.Key;
                        break;
                    }
                }

                int pos = (seat - myPos + manager.playerLimit) % (manager.playerLimit);
                manager.CardManager.GetComponent<CardManager>().MoveToSeat(r.Value, pos);
                manager.CardManager.GetComponent<CardManager>().SetSeatID(r.Value, pos);
                if (manager.isMaster)
                {
                    Role tmp = manager.cardRoleMap[r.Value];
                    manager.seatRoleMap.Add(pos, tmp);
                }

            }
        }

        int cnt = 6;
        for (int i = 0; i < 9; i++)
        {
            bool has = false;
            foreach (var r in manager.playerCardMap)
            {
                if (r.Value == i)
                {
                    has = true;
                    break;
                }
            }
            if (has) continue;
            manager.CardManager.GetComponent<CardManager>().MoveToSeat(i, cnt);
            manager.CardManager.GetComponent<CardManager>().SetSeatID(i, cnt);
            if (manager.isMaster)
            {
                Role tmp = manager.cardRoleMap[i];
                manager.seatRoleMap.Add(cnt, tmp);
            }
            //Debug.Log(cnt + ":cardID:" + i);
            cnt++;
            
        }

    }
    public override void OnUpdate()
    {
        m_time -= Time.deltaTime;
    }
    public override void OnStateExit()
    {
        isOK = false;

        int myCard = 0;
        foreach (var r in manager.playerCardMap)
        {
            if (r.Key == manager.actorID)
            {
                myCard = r.Value;
                break;
            }
        }
        manager.CardManager.GetComponent<CardManager>().MoveToSeat(myCard, 0);
        manager.CardManager.GetComponent<CardManager>().HideRole(myCard);


        if (manager.isMaster)
        {
            manager.SendRPC(StateManager.CHANGE_STATE_NTF, this.Estate);
        }
    }
    public override bool IsValid()
    {
        if (manager.isMaster)
        {
            if (m_time <= 0.0)
            {
                return true;
            }
            return false;
        }
        if (isOK) return true;
        return false;
    }
}

public class LangRen : State
{
    public int wolf_num = 0;
    public bool has_select = false;
    public LangRen(StateManager _manager)
    {
        Estate = GameState.LANG_REN;
        manager = _manager;
    }
    public override void OnStateEnter()
    {
        Debug.Log("狼人阶段");
        m_time = 20.0;
        manager.DataRefresh();
        manager.UIRefresh();
        if (manager.startRole!=Role.LANG_REN_1 && manager.startRole != Role.LANG_REN_2)
        {
            return;
        }
        wolf_num = 0;
        has_select = false;
        foreach (var r in manager.seatMap)
        {
            int id = r.Value;
            if (manager.playerRoleMap[id]== Role.LANG_REN_1 || manager.playerRoleMap[id] == Role.LANG_REN_2)
            {
                manager.CardManager.GetComponent<CardManager>().TurnWolf((r.Key-manager.mySeatID+manager.playerLimit)%(manager.playerLimit));
                wolf_num++;
            }
        }
        Debug.Log("wolf num:" + wolf_num);
        if (wolf_num == 1)
        {
            manager.CardManager.GetComponent<CardManager>().OnUnBanPickBySeatID(6);
            manager.CardManager.GetComponent<CardManager>().OnUnBanPickBySeatID(7);
            manager.CardManager.GetComponent<CardManager>().OnUnBanPickBySeatID(8);
        }
    }
    public override void OnUpdate()
    {
        m_time -= Time.deltaTime;
    }
    public override void OnStateExit()
    {
        manager.CardManager.GetComponent<CardManager>().TurnAllToHuman(manager.playerLimit);
        manager.CardManager.GetComponent<CardManager>().OnBanAllPick();
        Role_DeSelect(-1);
        isOK = false;
        if (manager.isMaster)
        {
            manager.SendRPC(StateManager.CHANGE_STATE_NTF, this.Estate);
        }
    }
    public override bool IsValid()
    {
        if (manager.isMaster)
        {
            if (m_time <= 0.0) return true;
            return false;
        }
        else
        {
            if (isOK) return true;
            return false;
        }
        
    }

    public override void Role_DeSelect(int cardID)
    {
        if (cardID == -1)
        {
            manager.CardManager.GetComponent<CardManager>().HideAllRole();
        }
        manager.CardManager.GetComponent<CardManager>().HideRole(cardID);
    }

    public override void Role_Select(int cardID,Role role)
    {
        manager.CardManager.GetComponent<CardManager>().ShowRole(cardID, role);
        has_select = true;
    }

    public override void OnCardClick(int cardID)
    {
        if (has_select) return;
        if (wolf_num != 1) return;
        int stID = manager.CardManager.GetComponent<CardManager>().GetSeatID(cardID);
        if (stID < 6) return;
        if (manager.isMaster)
        {
            Role tmp = manager.seatRoleMap[stID];
            this.Role_Select(cardID,tmp);
        }
        else
        {
            object[] odata = { manager.actorID, stID };
            manager.SendRPC(StateManager.LANG_REN_SELECT_REQ, odata);
        }
        
    }
}

public class ZhuaYa : State
{
    public ZhuaYa(StateManager _manager)
    {
        Estate = GameState.ZHUA_YA;
        manager = _manager;
    }
    public override void OnStateEnter()
    {
        Debug.Log("爪牙阶段");
        m_time = 15.0;
        manager.UIRefresh();
        if (manager.startRole != Role.ZHUA_YA) return;
        foreach (var r in manager.seatMap)
        {
            int id = r.Value;
            if (manager.playerRoleMap[id] == Role.LANG_REN_1 || manager.playerRoleMap[id] == Role.LANG_REN_2)
            {
                manager.CardManager.GetComponent<CardManager>().TurnWolf((r.Key - manager.mySeatID + manager.playerLimit) % (manager.playerLimit));
            }
        }

    }
    public override void OnUpdate()
    {
        m_time -= Time.deltaTime;
    }
    public override void OnStateExit()
    {
        manager.CardManager.GetComponent<CardManager>().TurnAllToHuman(manager.playerLimit);
        isOK = false;
        if (manager.isMaster)
        {
            manager.SendRPC(StateManager.CHANGE_STATE_NTF, this.Estate);
        }
    }
    public override bool IsValid()
    {
        if (manager.isMaster)
        {
            if (m_time <= 0.0) return true;
            return false;
        }
        else
        {
            if (isOK) return true;
            return false;
        }
    }
}

public class YuYanJia : State
{
    public int select_num_1 = 0;//五选一
    public int select_num_2 = 0;//三选二
    public YuYanJia(StateManager _manager)
    {
        Estate = GameState.YU_YAN_JIA;
        manager = _manager;
    }
    public override void OnStateEnter()
    {
        Debug.Log("预言家阶段");
        m_time = 15.0;
        select_num_1 = 0;
        select_num_2 = 0;
        manager.UIRefresh();
        if (manager.startRole != Role.YU_YAN_JIA)
        {
            manager.CardManager.GetComponent<CardManager>().OnBanAllPick();
        }
        else
        {
            manager.CardManager.GetComponent<CardManager>().OnUnBanAllPick();
        }
    }
    public override void OnUpdate()
    {
        m_time -= Time.deltaTime;
    }
    public override void OnStateExit()
    {
        manager.CardManager.GetComponent<CardManager>().OnBanAllPick();
        Role_DeSelect(-1);
        isOK = false;
        if (manager.isMaster)
        {
            manager.SendRPC(StateManager.CHANGE_STATE_NTF, this.Estate);
        }
    }
    public override bool IsValid()
    {
        if (manager.isMaster)
        {
            if (m_time <= 0.0) return true;
            return false;
        }
        else
        {
            if (isOK) return true;
            return false;
        }
    }
    public override void Role_DeSelect(int cardID)
    {
        if (cardID == -1)
        {
            manager.CardManager.GetComponent<CardManager>().HideAllRole();
        }
        manager.CardManager.GetComponent<CardManager>().HideRole(cardID);
    }

    public override void Role_Select(int cardID, Role role)
    {
        int stID = manager.CardManager.GetComponent<CardManager>().GetSeatID(cardID);
        if (stID >= 6)
        {
            select_num_2++;
        }
        else
        {
            select_num_1++;
        }
        if (select_num_2 == 2 || select_num_1 == 1)
        {
            manager.CardManager.GetComponent<CardManager>().OnBanAllPick();
        }
        manager.CardManager.GetComponent<CardManager>().ShowRole(cardID, role);
        

    }
    public override void OnCardClick(int cardID)
    {
        int stID = manager.CardManager.GetComponent<CardManager>().GetSeatID(cardID);
        if (manager.startRole != Role.YU_YAN_JIA) return;
        if (6 <= stID && stID <= 8)
        {
            if (select_num_1 > 0 || select_num_2 >= 2) return;
        }
        else
        {
            if (select_num_1 > 0 || select_num_2 > 0) return;
        }

        if (manager.isMaster)
        {   if (stID < 6)
            {
                stID = (stID - manager.mySeatID + manager.playerLimit) % manager.playerLimit;
            }
            Role tmp = manager.seatRoleMap[stID];
            this.Role_Select(cardID, tmp);
        }
        else
        {
            if (stID >= 6)
            {
                stID = stID;
            }
            else
            {
                stID = (stID + manager.mySeatID) % manager.playerLimit;
            }
            object[] odata = { manager.actorID, stID };
            manager.SendRPC(StateManager.YU_YAN_JIA_SELECT_REQ, odata);
        }

    }
}

public class HuaShenYouLing : State
{
    public int select_num = 0;
    public int select_seat_1 = 0;
    public int select_seat_2 = 0;
    public int select_card_1 = 0;
    public int select_card_2 = 0;
    public HuaShenYouLing(StateManager _manager)
    {
        Estate = GameState.HUA_SHEN_YOU_LING;
        manager = _manager;
    }
    public override void OnStateEnter()
    {
        Debug.Log("化身幽灵");
        m_time = 15.0;
        select_num = 0;
        select_seat_1 = 0;
        select_seat_2 = 0;
        select_card_1 = 0;
        select_card_2 = 0;
        if (manager.startRole != Role.HUA_SHEN_YOU_LING)
        {
            manager.CardManager.GetComponent<CardManager>().OnBanAllPick();
        }
        else
        {
            manager.CardManager.GetComponent<CardManager>().OnUnBanPickBySeatID(6);
            manager.CardManager.GetComponent<CardManager>().OnUnBanPickBySeatID(7);
            manager.CardManager.GetComponent<CardManager>().OnUnBanPickBySeatID(8);
        }
        manager.UIRefresh();
    }
    public override void OnUpdate()
    {
        m_time -= Time.deltaTime;
    }
    public override void OnStateExit()
    {
        manager.CardManager.GetComponent<CardManager>().OnBanAllPick();
        Role_DeSelect(-1);
        isOK = false;
        if (manager.isMaster)
        {
            foreach(var r in manager.seatRoleMap)
            {
                Debug.Log(r.Key + ":"+r.Value);
            }
            manager.SendRPC(StateManager.CHANGE_STATE_NTF, this.Estate);
        }
    }
    public override bool IsValid()
    {
        if (manager.isMaster)
        {
            if (m_time <= 0.0) return true;
            return false;
        }
        else
        {
            if (isOK) return true;
            return false;
        }
    }
    public override void Role_DeSelect(int cardID)
    {
        if (cardID == -1)
        {
            manager.CardManager.GetComponent<CardManager>().HideAllRole();
        }
        manager.CardManager.GetComponent<CardManager>().HideRole(cardID);
    }

    public override void Role_Select(int cardID, Role role)
    {
        select_num++;
        if (select_num == 1)
        {
            manager.CardManager.GetComponent<CardManager>().ShowRole(cardID, role);
            manager.CardManager.GetComponent<CardManager>().OnUnBanPickBySeatID(0);
            manager.CardManager.GetComponent<CardManager>().OnUnBanPickBySeatID(1);
            manager.CardManager.GetComponent<CardManager>().OnUnBanPickBySeatID(2);
            manager.CardManager.GetComponent<CardManager>().OnUnBanPickBySeatID(3);
            manager.CardManager.GetComponent<CardManager>().OnUnBanPickBySeatID(4);
            manager.CardManager.GetComponent<CardManager>().OnUnBanPickBySeatID(5);
            manager.CardManager.GetComponent<CardManager>().OnBanPickBySeatID(6);
            manager.CardManager.GetComponent<CardManager>().OnBanPickBySeatID(7);
            manager.CardManager.GetComponent<CardManager>().OnBanPickBySeatID(8);
            select_seat_1 = manager.CardManager.GetComponent<CardManager>().GetSeatID(cardID);
            select_card_1 = cardID;
        }
        else if (select_num == 2)
        {
            manager.CardManager.GetComponent<CardManager>().OnBanPickBySeatID(0);
            manager.CardManager.GetComponent<CardManager>().OnBanPickBySeatID(1);
            manager.CardManager.GetComponent<CardManager>().OnBanPickBySeatID(2);
            manager.CardManager.GetComponent<CardManager>().OnBanPickBySeatID(3);
            manager.CardManager.GetComponent<CardManager>().OnBanPickBySeatID(4);
            manager.CardManager.GetComponent<CardManager>().OnBanPickBySeatID(5);
            select_seat_2 = manager.CardManager.GetComponent<CardManager>().GetSeatID(cardID);
            select_card_2 = cardID;
            manager.CardManager.GetComponent<CardManager>().MoveToSeat(select_card_1, select_seat_2);
            manager.CardManager.GetComponent<CardManager>().MoveToSeat(select_card_2, select_seat_1);
            manager.CardManager.GetComponent<CardManager>().SetSeatID(select_card_1, select_seat_2);
            manager.CardManager.GetComponent<CardManager>().SetSeatID(select_card_2, select_seat_1);

            if (!manager.isMaster)
            {
                object[] odata = { select_seat_1,
                (select_seat_2+manager.mySeatID)% manager.playerLimit};
                manager.SendRPC(StateManager.HUA_SHEN_YOU_LING_SELECT_2_REQ, odata);
            }
            else
            {
                Role rl1 = manager.seatRoleMap[select_seat_1];
                Role rl2 = manager.seatRoleMap[select_seat_2];
                manager.seatRoleMap[select_seat_1] = rl2;
                manager.seatRoleMap[select_seat_2] = rl1;
            }
            
        }

    }
    public override void OnCardClick(int cardID)
    {
        int stID = manager.CardManager.GetComponent<CardManager>().GetSeatID(cardID);
        if (manager.startRole != Role.HUA_SHEN_YOU_LING) return;
        if (select_num >= 2) return;
        if (select_num == 0 && stID < 6) return;
        if (select_num == 1 && stID >= 6) return;
        
        if (manager.isMaster)
        {
            if (stID < 6)
            {
                stID = (stID - manager.mySeatID + manager.playerLimit) % manager.playerLimit;
            }
            Role tmp = manager.seatRoleMap[stID];
            this.Role_Select(cardID, tmp);
        }
        else
        {
            if (stID < 6)
            {
                stID = (stID + manager.mySeatID) % manager.playerLimit;
            }
            Debug.Log("化身幽灵准备发数据");
            object[] odata = { manager.actorID, stID };
            manager.SendRPC(StateManager.HUA_SHEN_YOU_LING_SELECT_1_REQ, odata);
        }
    }

        

    
}

public class QiangDao : State
{
    public bool has_select = false;
    public QiangDao(StateManager _manager)
    {
        Estate = GameState.QIANG_DAO;
        manager = _manager;
    }
    public override void OnStateEnter()
    {
        Debug.Log("强盗阶段");
        m_time = 15.0;
        if (manager.startRole != Role.QIANG_DAO)
        {
            manager.CardManager.GetComponent<CardManager>().OnBanAllPick();
        }
        else
        {
            manager.CardManager.GetComponent<CardManager>().OnUnBanPickBySeatID(0);
            manager.CardManager.GetComponent<CardManager>().OnUnBanPickBySeatID(1);
            manager.CardManager.GetComponent<CardManager>().OnUnBanPickBySeatID(2);
            manager.CardManager.GetComponent<CardManager>().OnUnBanPickBySeatID(3);
            manager.CardManager.GetComponent<CardManager>().OnUnBanPickBySeatID(4);
            manager.CardManager.GetComponent<CardManager>().OnUnBanPickBySeatID(5);
        }
        manager.UIRefresh();
    }
    public override void OnUpdate()
    {
        m_time -= Time.deltaTime;
    }
    public override void OnStateExit()
    {
        manager.CardManager.GetComponent<CardManager>().OnBanAllPick();
        Role_DeSelect(-1);
        isOK = false;
        if (manager.isMaster)
        {
            foreach (var r in manager.seatRoleMap)
            {
                Debug.Log(r.Key + ":" + r.Value);
            }
            manager.SendRPC(StateManager.CHANGE_STATE_NTF, this.Estate);
        }
    }
    public override bool IsValid()
    {
        if (manager.isMaster)
        {
            if (m_time <= 0.0) return true;
            return false;
        }
        else
        {
            if (isOK) return true;
            return false;
        }
    }

    public override void OnCardClick(int cardID)
    {
        int stID = manager.CardManager.GetComponent<CardManager>().GetSeatID(cardID);
        if (manager.startRole != Role.QIANG_DAO) return;
        if (has_select) return;
        if (stID >= 6) return;
        if (stID == 0) return;
        if (manager.isMaster)
        {
            Role tmp = manager.seatRoleMap[stID];
            Role_Select(cardID, tmp);
        }
        else
        {
            object[] odata = { manager.actorID, (stID+manager.mySeatID)%manager.playerLimit , manager.mySeatID};
            manager.SendRPC(StateManager.QIANG_DAO_SELECT_REQ, odata);
        }
    }

    public override void Role_DeSelect(int cardID)
    {
        if (cardID == -1)
        {
            manager.CardManager.GetComponent<CardManager>().HideAllRole();
        }
        manager.CardManager.GetComponent<CardManager>().HideRole(cardID);
    }

    public override void Role_Select(int cardID, Role role)
    {
        has_select = true;
        int stID = manager.CardManager.GetComponent<CardManager>().GetSeatID(cardID);
        manager.CardManager.GetComponent<CardManager>().ShowRole(cardID, role);
        if (manager.isMaster)
        {
            Role rl1 = role;
            Role rl2 = manager.seatRoleMap[0];
            int seatID1 = manager.CardManager.GetComponent<CardManager>().GetSeatID(cardID);
            int seatID2 = 0;
            manager.seatRoleMap[seatID1] = rl2;
            manager.seatRoleMap[seatID2] = rl1;
        }
        int card1 = cardID;
        int seat1 = manager.CardManager.GetComponent<CardManager>().GetSeatID(cardID);
        int card2 = manager.CardManager.GetComponent<CardManager>().GetCardID(0);
        int seat2 = 0;
        manager.CardManager.GetComponent<CardManager>().MoveToSeat(card1, seat2);
        manager.CardManager.GetComponent<CardManager>().MoveToSeat(card2, seat1);
        manager.CardManager.GetComponent<CardManager>().SetSeatID(card1, seat2);
        manager.CardManager.GetComponent<CardManager>().SetSeatID(card2, seat1);
        manager.CardManager.GetComponent<CardManager>().OnBanPickBySeatID(0);
        manager.CardManager.GetComponent<CardManager>().OnBanPickBySeatID(1);
        manager.CardManager.GetComponent<CardManager>().OnBanPickBySeatID(2);
        manager.CardManager.GetComponent<CardManager>().OnBanPickBySeatID(3);
        manager.CardManager.GetComponent<CardManager>().OnBanPickBySeatID(4);
        manager.CardManager.GetComponent<CardManager>().OnBanPickBySeatID(5);
    }
}

public class DaoDanGui : State
{
    public int select_num=0;
    public int seat_id_1 = 0;
    public int seat_id_2 = 0;
    public DaoDanGui(StateManager _manager)
    {
        Estate = GameState.DAO_DAN_GUI;
        manager = _manager;
    }
    public override void OnStateEnter()
    {
        Debug.Log("捣蛋鬼阶段");
        m_time = 15.0;
        seat_id_1 = 0;
        seat_id_2 = 0;
        select_num = 0;
        manager.UIRefresh();
        if (manager.startRole != Role.DAO_DAN_GUI)
        {
            manager.CardManager.GetComponent<CardManager>().OnBanAllPick();
        }
        else
        {
            //manager.CardManager.GetComponent<CardManager>().OnUnBanPickBySeatID(0);
            manager.CardManager.GetComponent<CardManager>().OnUnBanPickBySeatID(1);
            manager.CardManager.GetComponent<CardManager>().OnUnBanPickBySeatID(2);
            manager.CardManager.GetComponent<CardManager>().OnUnBanPickBySeatID(3);
            manager.CardManager.GetComponent<CardManager>().OnUnBanPickBySeatID(4);
            manager.CardManager.GetComponent<CardManager>().OnUnBanPickBySeatID(5);
        }
        manager.UIRefresh();
    }
    public override void OnUpdate()
    {
        m_time -= Time.deltaTime;
    }
    public override void OnStateExit()
    {
        manager.CardManager.GetComponent<CardManager>().OnBanAllPick();
        Role_DeSelect(-1);
        isOK = false;
        if (manager.isMaster)
        {
            foreach (var r in manager.seatRoleMap)
            {
                Debug.Log(r.Key + ":" + r.Value);
            }
            manager.SendRPC(StateManager.CHANGE_STATE_NTF, this.Estate);
        }
    }
    public override bool IsValid()
    {
        if (manager.isMaster)
        {
            if (m_time <= 0.0) return true;
            return false;
        }
        else
        {
            if (isOK) return true;
            return false;
        }
    }
    public override void OnCardClick(int cardID)
    {
        int stID = manager.CardManager.GetComponent<CardManager>().GetSeatID(cardID);
        if (manager.startRole != Role.DAO_DAN_GUI) return;
        if (select_num>=2) return;
        if (stID == 0) return;
        if (stID >= 6) return;
        if (manager.isMaster)
        {
            Role tmp = manager.seatRoleMap[stID];
            Role_Select(cardID, tmp);
        }
        else
        {
            object[] odata = { manager.actorID, (stID + manager.mySeatID) % manager.playerLimit };
            manager.SendRPC(StateManager.DAO_DAN_GUI_SELECT_REQ, odata);
        }
    }
    public override void Role_DeSelect(int cardID)
    {
        if (cardID == -1)
        {
            manager.CardManager.GetComponent<CardManager>().HideAllRole();
        }
        manager.CardManager.GetComponent<CardManager>().HideRole(cardID);
    }

    public override void Role_Select(int cardID, Role role)
    {
        select_num++;
        if (select_num == 1)
        {
            manager.CardManager.GetComponent<CardManager>().OnMySelect(cardID);
            int stID = manager.CardManager.GetComponent<CardManager>().GetSeatID(cardID);
            seat_id_1 = stID;//stID这里是相对值
        }
        else if (select_num == 2)
        {
            manager.CardManager.GetComponent<CardManager>().OnMySelect(cardID);
            int stID = manager.CardManager.GetComponent<CardManager>().GetSeatID(cardID);
            seat_id_2 = stID;//stID这里是相对值

            int card1 = manager.CardManager.GetComponent<CardManager>().GetCardID(seat_id_1);
            int card2 = manager.CardManager.GetComponent<CardManager>().GetCardID(seat_id_2);

            manager.CardManager.GetComponent<CardManager>().MoveToSeat(card1, seat_id_2);
            manager.CardManager.GetComponent<CardManager>().MoveToSeat(card2, seat_id_1);
            manager.CardManager.GetComponent<CardManager>().SetSeatID(card1, seat_id_2);
            manager.CardManager.GetComponent<CardManager>().SetSeatID(card2, seat_id_1);
            
            manager.CardManager.GetComponent<CardManager>().OnBanPickBySeatID(0);
            manager.CardManager.GetComponent<CardManager>().OnBanPickBySeatID(1);
            manager.CardManager.GetComponent<CardManager>().OnBanPickBySeatID(2);
            manager.CardManager.GetComponent<CardManager>().OnBanPickBySeatID(3);
            manager.CardManager.GetComponent<CardManager>().OnBanPickBySeatID(4);
            manager.CardManager.GetComponent<CardManager>().OnBanPickBySeatID(5);

            if (manager.isMaster)
            {
                Role rl1 = manager.seatRoleMap[seat_id_1];
                Role rl2 = manager.seatRoleMap[seat_id_2];
                manager.seatRoleMap[seat_id_1] = rl2;
                manager.seatRoleMap[seat_id_2] = rl1;
            }
            else
            {
                object[] odata = { (seat_id_1+manager.mySeatID)%manager.playerLimit,
                    (seat_id_2+manager.mySeatID)%manager.playerLimit };
                manager.SendRPC(StateManager.DAO_DAN_GUI_DOUBLE_SELECT_REQ, odata);
            }

        }
        
    }
}

public class ShiMianZhe : State
{
    public ShiMianZhe(StateManager _manager)
    {
        Estate = GameState.SHI_MIAN_ZHE;
        manager = _manager;
    }
    public override void OnStateEnter()
    {
        Debug.Log("失眠者阶段");
        m_time = 15.0;
        manager.UIRefresh();
        if (manager.startRole != Role.SHI_MIAN_ZHE) return;
        object[] obj = { manager.actorID, manager.mySeatID };
        if (!manager.isMaster)
        {
            manager.SendRPC(StateManager.SHI_MIAN_ZHE_REQ, obj);
        }
        else
        {
            Role rl = manager.seatRoleMap[0];
            int cardID = manager.CardManager.GetComponent<CardManager>().GetCardID(0);
            manager.CardManager.GetComponent<CardManager>().ShowRole(cardID, rl);
        }
    }
    public override void OnUpdate()
    {
        m_time -= Time.deltaTime;
    }
    public override void OnStateExit()
    {
        isOK = false;
        Role_DeSelect(-1);
        if (manager.isMaster)
        {
            manager.SendRPC(StateManager.CHANGE_STATE_NTF, this.Estate);
        }
    }
    public override bool IsValid()
    {
        if (manager.isMaster)
        {
            if (m_time <= 0.0) return true;
            return false;
        }
        else
        {
            if (isOK) return true;
            return false;
        }
    }
    public override void Role_DeSelect(int cardID)
    {
        if (cardID == -1)
        {
            manager.CardManager.GetComponent<CardManager>().HideAllRole();
        }
        manager.CardManager.GetComponent<CardManager>().HideRole(cardID);
    }
}

public class JiuGui : State
{
    public bool has_select = false;
    public JiuGui(StateManager _manager)
    {
        Estate = GameState.JIU_GUI;
        manager = _manager;
    }
    public override void OnStateEnter()
    {
        Debug.Log("酒鬼阶段");
        has_select = false;
        m_time = 15.0;
        manager.UIRefresh();
        if (manager.startRole != Role.JIU_GUI) return;
        manager.CardManager.GetComponent<CardManager>().OnUnBanPickBySeatID(6);
        manager.CardManager.GetComponent<CardManager>().OnUnBanPickBySeatID(7);
        manager.CardManager.GetComponent<CardManager>().OnUnBanPickBySeatID(8);
        
    }
    public override void OnUpdate()
    {
        m_time -= Time.deltaTime;
    }
    public override void OnStateExit()
    {
        isOK = false;
        Role_DeSelect(-1);
        if (manager.isMaster)
        {
            foreach (var r in manager.seatRoleMap)
            {
                Debug.Log(r.Key + ":" + r.Value);
            }
            manager.SendRPC(StateManager.CHANGE_STATE_NTF, this.Estate);
        }
    }
    public override bool IsValid()
    {
        if (manager.isMaster)
        {
            if (m_time <= 0.0) return true;
            return false;
        }
        else
        {
            if (isOK) return true;
            return false;
        }
    }
    public override void OnCardClick(int cardID)
    {
        int stID = manager.CardManager.GetComponent<CardManager>().GetSeatID(cardID);
        if (manager.startRole != Role.JIU_GUI) return;
        if (has_select) return;
        if (stID < 6) return;
        if (manager.isMaster)
        {
            Role rl = manager.seatRoleMap[stID];
            Role_Select(cardID, rl);
        }
        else
        {
            object[] obj = { manager.actorID,manager.mySeatID, stID };
            manager.SendRPC(StateManager.JIU_GUI_REQ, obj);
        }
        
    }
    public override void Role_Select(int cardID, Role role)
    {
        has_select = true;
        int stID1 = 0;
        int stID2 = manager.CardManager.GetComponent<CardManager>().GetSeatID(cardID);
        if (manager.isMaster)
        {
            Role rl1 = manager.seatRoleMap[stID1];
            Role rl2 = manager.seatRoleMap[stID2];
            manager.seatRoleMap[stID1] = rl2;
            manager.seatRoleMap[stID2] = rl1;
        }
        int card1 = manager.CardManager.GetComponent<CardManager>().GetCardID(stID1);
        int card2 = manager.CardManager.GetComponent<CardManager>().GetCardID(stID2);
        manager.CardManager.GetComponent<CardManager>().MoveToSeat(card1, stID2);
        manager.CardManager.GetComponent<CardManager>().MoveToSeat(card2, stID1);
        manager.CardManager.GetComponent<CardManager>().SetSeatID(card1, stID2);
        manager.CardManager.GetComponent<CardManager>().SetSeatID(card2, stID1);
        
    }
    public override void Role_DeSelect(int cardID)
    {
        if (cardID == -1)
        {
            manager.CardManager.GetComponent<CardManager>().HideAllRole();
        }
        manager.CardManager.GetComponent<CardManager>().HideRole(cardID);
    }
}

public class Free : State
{
    public int selectID = -1;
    public Free(StateManager _manager)
    {
        Estate = GameState.FREE;
        manager = _manager;
    }
    public override void OnStateEnter()
    {
        Debug.Log("自由发言阶段");
        selectID = -1;
        m_time = -1.0;
        if (manager.isMaster)
        {
            manager.UIManager.GetComponent<UIManager>().SetResult(true);
        }
        manager.UIRefresh();
    }
    public override void OnUpdate()
    {
        m_time -= Time.deltaTime;
    }
    public override void OnStateExit()
    {
        isOK = false;
        //Role_DeSelect(-1);
        if (manager.isMaster)
        {
            manager.UIManager.GetComponent<UIManager>().SetResult(false);
        }
        manager.UIManager.GetComponent<UIManager>().PlayerAllTurnWhite();
        if (manager.isMaster)
        {
            int mx = 0, player = -1;
            for (int i = 0; i < manager.playerLimit; i++)
            {
                int cnt = 0;
                int acID = manager.seatMap[i];
                if (acID <= 0) continue;
                foreach (var r in manager.voteMap)
                {
                    if (r.Value == acID) cnt++;
                }
                if (cnt > mx)
                {
                    mx = cnt;
                    player = i;
                }
            }
            player = (player + manager.mySeatID) % manager.playerLimit;
            manager.SetVote(player, mx);

            object[] obj = { player, mx };
            manager.SendRPC(StateManager.VOTE_ACK, obj);
        }


        if (manager.isMaster)
        {
            foreach (var r in manager.seatRoleMap)
            {
                Debug.Log(r.Key + ":" + r.Value);
            }
            manager.SendRPC(StateManager.CHANGE_STATE_NTF, this.Estate);
        }
    }
    public override void OnReadyClick()
    {
        if (!manager.isMaster) return;
        isOK = true;
    }
    public override void OnPlayerClick(int id)
    {
        if (selectID == id)
        {
            manager.UIManager.GetComponent<UIManager>().PlayerTurnWhite(id);
            selectID = -1;
        }else
        if (selectID != -1)
        {
            manager.UIManager.GetComponent<UIManager>().PlayerTurnWhite(selectID);
            selectID = id;
            manager.UIManager.GetComponent<UIManager>().PlayerTurnGreen(selectID);
        }
        else
        {
            selectID = id;
            manager.UIManager.GetComponent<UIManager>().PlayerTurnGreen(selectID);
        }


        if (!manager.isMaster)
        {
            object[] obj = { manager.actorID, manager.mySeatID, selectID };
            manager.SendRPC(StateManager.VOTE_REQ, obj);
        }
        else
        {
            
            int ans = -1;
            if (selectID != -1)
            {   
                ans= manager.seatMap[selectID];
            }
            if (manager.voteMap.ContainsKey(manager.actorID))
            {
                manager.voteMap[manager.actorID] = ans;
            }
            else
            {
                manager.voteMap.Add(manager.actorID, ans);
            }
        }
        
    }
    public override bool IsValid()
    {
        if (manager.isMaster)
        {
            if (isOK) return true;
            return false;
        }
        else
        {
            if (isOK) return true;
            return false;
        }
    }
    public override void Role_DeSelect(int cardID)
    {
        if (cardID == -1)
        {
            manager.CardManager.GetComponent<CardManager>().HideAllRole();
        }
        manager.CardManager.GetComponent<CardManager>().HideRole(cardID);
    }
}

public class ShowResult : State
{
    public int vote1 = -1;
    public int vote2 = -1;
    public ShowResult(StateManager _manager)
    {
        Estate = GameState.SHOW_RESULT;
        manager = _manager;
    }
    public override void OnStateEnter()
    {
        Debug.Log("展示结果阶段");
        if (manager.isMaster)
        {
            Debug.Log("vote:" + vote1 + "," + vote2);
        }
        m_time = 20.0;
        manager.UIManager.GetComponent<UIManager>().SetVoting(true);
        manager.UIManager.GetComponent<UIManager>().SetVoteNum(manager.voteNum1,manager.voteNum2);
        manager.UIRefresh();
        if (manager.isMaster)
        {
            for (int i = 0; i < 9; i++)
            {
                Role rl = manager.seatRoleMap[i];
                int cardID = manager.CardManager.GetComponent<CardManager>().GetCardID(i);
                manager.CardManager.GetComponent<CardManager>().ShowRole(cardID, rl);
            }
            object[] obj = {
                            manager.seatRoleMap[0],
                            manager.seatRoleMap[1],
                            manager.seatRoleMap[2],
                            manager.seatRoleMap[3],
                            manager.seatRoleMap[4],
                            manager.seatRoleMap[5],
                            manager.seatRoleMap[6],
                            manager.seatRoleMap[7],
                            manager.seatRoleMap[8],
                            manager.mySeatID};
            manager.SendRPC(StateManager.SHOW_RESULT_NTF, obj);
        }
    }
    public override void OnUpdate()
    {
        m_time -= Time.deltaTime;
    }
    public override void OnStateExit()
    {
        isOK = false;
        Role_DeSelect(-1);
        manager.UIManager.GetComponent<UIManager>().SetVoting(false);
        manager.UIRefresh();
        if (manager.isMaster)
        {
            foreach (var r in manager.voteMap)
            {
                Debug.Log("voteMap:"+r.Key + ":" + r.Value);
            }
            manager.SendRPC(StateManager.CHANGE_STATE_NTF, this.Estate);
        }
    }
    public override bool IsValid()
    {
        if (manager.isMaster)
        {
            if (m_time <= 0.0) return true;
            return false;
        }
        else
        {
            if (isOK) return true;
            return false;
        }
    }
    public override void Role_DeSelect(int cardID)
    {
        if (cardID == -1)
        {
            manager.CardManager.GetComponent<CardManager>().HideAllRole();
        }
        manager.CardManager.GetComponent<CardManager>().HideRole(cardID);
    }
}

public class StateManager : MonoBehaviourPunCallbacks
{
    public const byte GET_SEAT_REQ = 0;
    public const byte GET_SEAT_ACK = 1;
    public const byte GET_ALL_SEAT_REQ = 2;
    public const byte GET_SEAT_NTF = 3;
    public const byte READY_REQ = 4;
    public const byte READY_ACK = 5;
    public const byte CHANGE_STATE_NTF = 6;
    public const byte CHOOSE_CARD_REQ = 7;
    public const byte CHOOSE_CARD_NTF = 8;
    public const byte LANG_REN_SELECT_REQ = 9;
    public const byte LANG_REN_SELECT_ACK = 10;
    public const byte YU_YAN_JIA_SELECT_REQ = 11;
    public const byte YU_YAN_JIA_SELECT_ACK = 12;
    public const byte HUA_SHEN_YOU_LING_SELECT_1_REQ = 13;
    public const byte HUA_SHEN_YOU_LING_SELECT_1_ACK = 14;
    public const byte HUA_SHEN_YOU_LING_SELECT_2_REQ = 15;
    public const byte QIANG_DAO_SELECT_REQ = 16;
    public const byte QIANG_DAO_SELECT_ACK = 17;
    public const byte DAO_DAN_GUI_SELECT_REQ = 18;
    public const byte DAO_DAN_GUI_SELECT_ACK = 19;
    public const byte DAO_DAN_GUI_DOUBLE_SELECT_REQ = 20;
    public const byte SHI_MIAN_ZHE_REQ = 21;
    public const byte SHI_MIAN_ZHE_ACK = 22;
    public const byte JIU_GUI_REQ = 23;
    public const byte JIU_GUI_ACK = 24;
    public const byte SHOW_RESULT_NTF = 25;
    public const byte VOTE_REQ = 26;
    public const byte VOTE_ACK = 27;


    public GameObject UIManager;
    public GameObject CardManager;
    public State nowState=null;
    private int sec_frame = 100;
    private State initState;
    private State chooseState;
    private State watchState;
    private State langRenState;
    private State zhuaYaState;
    private State yuYanJiaState;
    private State huaShenYouLingState;
    private State qiangDaoState;
    private State daoDanGuiState;
    private State shiMianZheState;
    private State jiuGuiState;
    private State freeState;
    private State showResultState;

    public Photon.Realtime.Player[] playerList;
    public Room currentRoom;
    public bool isMaster;
    public bool isReady;
    public int actorID;
    public Dictionary<int, Player> playerMap;// actorID  player
    public Dictionary<int, bool> readyMap;
    public Dictionary<int, int> seatMap;
    public Dictionary<int, Role> cardRoleMap;
    public Dictionary<int, Role> playerRoleMap;
    public Dictionary<int, int> playerCardMap;
    public Dictionary<int, Role> seatRoleMap;
    public Dictionary<int, int> voteMap;
    public int playerLimit = 0;
    public int mySeatID = -1;
    public Role startRole;
    public int voteNum1 = -1;
    public int voteNum2 = -1;

    // Start is called before the first frame update
    void Start()
    {
        playerMap = new Dictionary<int, Player>(); // actorID  player
        readyMap = new Dictionary<int, bool>();
        seatMap = new Dictionary<int, int>();
        cardRoleMap = new Dictionary<int, Role>();
        playerCardMap = new Dictionary<int, int>();//初始选择的卡牌，展示红绿用
        playerRoleMap = new Dictionary<int, Role>();//玩家与初始角色的映射
        seatRoleMap = new Dictionary<int, Role>();//坑位与角色的映射(只有主客户端用)
        voteMap = new Dictionary<int, int>();
        initState = new Init(this);
        chooseState = new Choose(this);
        watchState = new Watch(this);
        langRenState = new LangRen(this);
        zhuaYaState = new ZhuaYa(this);
        yuYanJiaState = new YuYanJia(this);
        huaShenYouLingState = new HuaShenYouLing(this);
        qiangDaoState = new QiangDao(this);
        daoDanGuiState = new DaoDanGui(this);
        shiMianZheState = new ShiMianZhe(this);
        jiuGuiState = new JiuGui(this);
        freeState = new Free(this);
        showResultState = new ShowResult(this);
        

        initState.SetNexState(chooseState);
        chooseState.SetNexState(watchState);
        watchState.SetNexState(langRenState);
        langRenState.SetNexState(zhuaYaState);
        zhuaYaState.SetNexState(yuYanJiaState);
        yuYanJiaState.SetNexState(huaShenYouLingState);
        huaShenYouLingState.SetNexState(qiangDaoState);
        qiangDaoState.SetNexState(daoDanGuiState);
        daoDanGuiState.SetNexState(shiMianZheState);
        shiMianZheState.SetNexState(jiuGuiState);
        jiuGuiState.SetNexState(freeState);
        freeState.SetNexState(showResultState);
        showResultState.SetNexState(initState);

        nowState = initState;
        nowState.OnStateEnter();
    }

    // Update is called once per frame
    void Update()
    {
        if (nowState == null) return;
        nowState.OnUpdate();
        if (nowState.IsValid())
        {
            nowState.OnStateExit();
            nowState = nowState.GetNexState();
            nowState.OnStateEnter();
        }
    }

    public void NetworkingClient_EventReceived(EventData obj)//收消息
    {
        switch (obj.Code)
        {
            case GET_SEAT_REQ://0
                if (!isMaster) break;
                DataRefresh();
                int actorID0 = (int)obj.CustomData;
                int seatID0 = -1;
                int seatPos = 0;
                for (int i = 0; i <= playerLimit; i++)
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
                seatID0 = seatPos;
                seatMap.Add(seatID0, actorID0);
                object[] oDataSend0 = {actorID0, seatID0};
                UIRefresh();
                SendRPC(GET_SEAT_ACK, oDataSend0);
                break;
            case GET_SEAT_ACK://1
                if (isMaster) break;
                object[] oDataRev1 = (object[])obj.CustomData;
                int actorID1 = (int)oDataRev1[0];
                int seatID1 = (int)oDataRev1[1];
                if (actorID1 != actorID) break;
                if (!seatMap.ContainsKey(seatID1))
                {
                    seatMap.Add(seatID1, actorID1);
                }
                else
                {
                    seatMap[seatID1] = actorID1;
                }
                
                mySeatID = seatID1;
                object[] oDataSend1 = { actorID, mySeatID, isReady };
                DataRefresh();
                UIRefresh();
                SendRPC(GET_SEAT_NTF, oDataSend1);
                break;
            case GET_ALL_SEAT_REQ://2
                object[] oDataSend2 = {actorID, mySeatID ,isReady};
                DataRefresh();
                UIRefresh();
                SendRPC(GET_SEAT_NTF, oDataSend2);
                break;
            case GET_SEAT_NTF://3
                object[] oDataRecv3 = (object[])obj.CustomData;
                int actorID3 = (int)oDataRecv3[0];
                int seatID3 = (int)oDataRecv3[1];
                bool isR = (bool)oDataRecv3[2];
                if (!seatMap.ContainsKey(seatID3))
                {
                    seatMap.Add(seatID3, actorID3);
                }
                else
                {
                    seatMap[seatID3] = actorID3;
                }
                if (!readyMap.ContainsKey(actorID3))
                {
                    readyMap.Add(actorID3, isR);
                }
                else
                {
                    readyMap[actorID3] = isR;
                }
                DataRefresh();
                UIRefresh();
                break;
            case READY_REQ://4
                object[] oDataSend4 = { actorID, isReady };
                SendRPC(READY_ACK, oDataSend4);
                break;
            case READY_ACK://5
                object[] oDataSend5 = (object[])obj.CustomData;
                int actorID5 = (int)oDataSend5[0];
                bool isReady5 = (bool)oDataSend5[1];
                if (readyMap.ContainsKey(actorID5))
                {
                    readyMap[actorID5] = isReady5;
                }
                else
                {
                    readyMap.Add(actorID5, isReady5);
                }
                DataRefresh();
                UIRefresh();
                break;
            case CHANGE_STATE_NTF://6
                GameState st = (GameState)obj.CustomData;
                if (this.nowState.Estate.Equals(st))
                {
                    this.nowState.isOK = true;
                }
                break;
            case CHOOSE_CARD_REQ://7
                if (!isMaster) break;
                object[] oDataRecv7 = (object[])obj.CustomData;
                int actorID7 = (int)oDataRecv7[0];
                int cardID7 = (int)oDataRecv7[1];
                bool pd7 = false;
                foreach (var r in playerCardMap)
                {
                    if (r.Value == cardID7)
                    {
                        pd7 = true;
                        break;
                    }
                }
                if (pd7) break;
                playerCardMap.Add(actorID7, cardID7);
                Role role7 = cardRoleMap[cardID7];
                playerRoleMap.Add(actorID7, role7);
                nowState.OnOtherSelect(cardID7);
                object[] oDataSend7 = {actorID7, cardID7, role7};
                SendRPC(CHOOSE_CARD_NTF, oDataSend7);
                break;
            case CHOOSE_CARD_NTF://8;
                if (isMaster) break;
                object[] oDataRecv8 = (object[])obj.CustomData;
                int actorID8 = (int)oDataRecv8[0];
                int cardID8 = (int)oDataRecv8[1];
                Role role8 = (Role)oDataRecv8[2];
                if (actorID8 == actorID)
                {
                    nowState.OnMySelect(cardID8);
                    startRole = role8;
                }
                else
                {
                    nowState.OnOtherSelect(cardID8);
                }
                playerCardMap.Add(actorID8, cardID8);
                playerRoleMap.Add(actorID8, role8);
                break;
            case LANG_REN_SELECT_REQ://9
                if (!isMaster) break;
                object[] oDataRecv9 = (object[])obj.CustomData;
                int actorID9 = (int)oDataRecv9[0];
                int seatID9 = (int)oDataRecv9[1];
                Role role9 = seatRoleMap[seatID9];
                object[] oDataSend9 = { actorID9, seatID9, role9 };
                SendRPC(LANG_REN_SELECT_ACK, oDataSend9);
                break;
            case LANG_REN_SELECT_ACK://10
                if (isMaster) break;
                object[] oDataRecv10 = (object[])obj.CustomData;
                int actorID10 = (int)oDataRecv10[0];
                int seatID10 = (int)oDataRecv10[1];
                Role role10 = (Role)oDataRecv10[2];
                if (actorID != actorID10) break;
                int cardID10 = CardManager.GetComponent<CardManager>().GetCardID(seatID10);
                nowState.Role_Select(cardID10, role10);
                break;
            case YU_YAN_JIA_SELECT_REQ://11
                if (!isMaster) break;
                object[] oDataRecv11 = (object[])obj.CustomData;
                int actorID11 = (int)oDataRecv11[0];
                int seatID11 = (int)oDataRecv11[1];
                Role role11;
                if (seatID11 >= 6)
                {
                    role11 = seatRoleMap[seatID11];
                }
                else
                {
                    role11 = seatRoleMap[(seatID11-mySeatID+playerLimit)% playerLimit];
                }
                 
                object[] oDataSend11 = { actorID11, seatID11, role11 };
                SendRPC(YU_YAN_JIA_SELECT_ACK, oDataSend11);
                break;
            case YU_YAN_JIA_SELECT_ACK://12
                if (isMaster) break;
                object[] oDataRecv12 = (object[])obj.CustomData;
                int actorID12 = (int)oDataRecv12[0];
                int seatID12 = (int)oDataRecv12[1];
                Role role12 = (Role)oDataRecv12[2];
                if (actorID != actorID12) break;
                if (seatID12 < 6)
                {
                    seatID12 = (seatID12 - mySeatID + playerLimit) % playerLimit;
                }
                int cardID12 = CardManager.GetComponent<CardManager>().GetCardID(seatID12);
                nowState.Role_Select(cardID12, role12);
                break;
            case HUA_SHEN_YOU_LING_SELECT_1_REQ://13
                if (!isMaster) break;
                Debug.Log("收到化身幽灵的请求");
                object[] oDataRecv13 = (object[])obj.CustomData;
                int actorID13 = (int)oDataRecv13[0];
                int seatID13 = (int)oDataRecv13[1];
                Role role13;
                if (seatID13 >= 6)
                {
                    role13 = seatRoleMap[seatID13];
                }
                else
                {
                    role13 = seatRoleMap[(seatID13 - mySeatID + playerLimit) % playerLimit];
                }

                object[] oDataSend13 = { actorID13, seatID13, role13 };
                SendRPC(HUA_SHEN_YOU_LING_SELECT_1_ACK, oDataSend13);
                break;
            case HUA_SHEN_YOU_LING_SELECT_1_ACK://14
                if (isMaster) break;
                Debug.Log("化身幽灵收到主机响应");
                object[] oDataRecv14 = (object[])obj.CustomData;
                int actorID14 = (int)oDataRecv14[0];
                int seatID14 = (int)oDataRecv14[1];
                Role role14 = (Role)oDataRecv14[2];
                if (actorID != actorID14) break;
                if (seatID14 < 6)
                {
                    seatID14 = (seatID14 - mySeatID + playerLimit) % playerLimit;
                }
                int cardID14 = CardManager.GetComponent<CardManager>().GetCardID(seatID14);
                nowState.Role_Select(cardID14, role14);
                break;
            case HUA_SHEN_YOU_LING_SELECT_2_REQ://15
                if (!isMaster) break;
                object[] oDataRecv15 = (object[])obj.CustomData;
                int seatID15_1 = (int)oDataRecv15[0];
                int seatID15_2 = (int)oDataRecv15[1];
                seatID15_2 = (seatID15_2 - mySeatID + playerLimit) % playerLimit;
                Debug.Log("swap:" + seatID15_1 + "+" + seatID15_2);
                Role role15_1 = seatRoleMap[seatID15_1];
                Role role15_2 = seatRoleMap[seatID15_2];
                seatRoleMap[seatID15_1] = role15_2;
                seatRoleMap[seatID15_2] = role15_1;
                break;
            case QIANG_DAO_SELECT_REQ://16
                if (!isMaster) break;
                object[] oDataRecv16 = (object[])obj.CustomData;
                int actorID16 = (int)oDataRecv16[0];
                int seatID16_1 = (int)oDataRecv16[1];
                int seatID16_2 = (int)oDataRecv16[2];
                int seatID16_tmp_1 = (seatID16_1 - mySeatID + playerLimit) % playerLimit;
                int seatID16_tmp_2 = (seatID16_2 - mySeatID + playerLimit) % playerLimit;
                Role role16_1 = seatRoleMap[seatID16_tmp_1];
                Role role16_2 = seatRoleMap[seatID16_tmp_2];
                seatRoleMap[seatID16_tmp_1] = role16_2;
                seatRoleMap[seatID16_tmp_2] = role16_1;
                object[] oDataSend16 = { actorID16, seatID16_1, role16_1, seatID16_2, role16_2 };
                SendRPC(QIANG_DAO_SELECT_ACK, oDataSend16);
                break;
            case QIANG_DAO_SELECT_ACK://17
                if (isMaster) break;
                if (startRole != Role.QIANG_DAO) break;
                object[] oDataRecv17 = (object[])obj.CustomData;
                int actorID17 = (int)oDataRecv17[0];
                int seatID17_1 = (int)oDataRecv17[1];
                Role role17_1 = (Role)oDataRecv17[2];
                int seatID17_2 = (int)oDataRecv17[3];
                Role role17_2 = (Role)oDataRecv17[4];
                seatID17_1 = (seatID17_1 - mySeatID + playerLimit) % playerLimit;
                seatID17_2 = (seatID17_2 - mySeatID + playerLimit) % playerLimit;
                int cardID17 = CardManager.GetComponent<CardManager>().GetCardID(seatID17_1);
                nowState.Role_Select(cardID17, role17_1);
                break;
            case DAO_DAN_GUI_SELECT_REQ://18
                if (!isMaster) break;
                object[] oDataRecv18 = (object[])obj.CustomData;
                int actorID18 = (int)oDataRecv18[0];
                int seatID18 = (int)oDataRecv18[1];
                Role role18 = seatRoleMap[(seatID18-mySeatID+playerLimit)%playerLimit];
                object[] oDataSend18 = { actorID18, seatID18, role18 };
                SendRPC(DAO_DAN_GUI_SELECT_ACK, oDataSend18);
                break;
            case DAO_DAN_GUI_SELECT_ACK://19
                if (isMaster) break;
                if (startRole != Role.DAO_DAN_GUI) break;
                object[] oDataRecv19 = (object[])obj.CustomData;
                int actorID19 = (int)oDataRecv19[0];
                int seatID19 = (int)oDataRecv19[1];
                Role role19 = (Role)oDataRecv19[2];
                seatID19 = (seatID19 - mySeatID + playerLimit) % playerLimit;
                int cardID19 = CardManager.GetComponent<CardManager>().GetCardID(seatID19);
                nowState.Role_Select(cardID19, role19);
                break;
            case DAO_DAN_GUI_DOUBLE_SELECT_REQ://20
                if (!isMaster) break;
                object[] oDataRecv20 = (object[])obj.CustomData;
                int seatID20_1 = (int)oDataRecv20[0];
                int seatID20_2 = (int)oDataRecv20[1];
                seatID20_1 = (seatID20_1 - mySeatID + playerLimit) % playerLimit;
                seatID20_2 = (seatID20_2 - mySeatID + playerLimit) % playerLimit;
                Role rl1 = seatRoleMap[seatID20_1];
                Role rl2 = seatRoleMap[seatID20_2];
                seatRoleMap[seatID20_1] = rl2;
                seatRoleMap[seatID20_2] = rl1;
                break;
            case SHI_MIAN_ZHE_REQ://21
                if (!isMaster) break;
                object[] oDataRecv21 = (object[])obj.CustomData;
                int actorID21 = (int)oDataRecv21[0];
                int seatID21 = (int)oDataRecv21[1];
                Role role21 = seatRoleMap[(seatID21 - mySeatID + playerLimit) % playerLimit];
                object[] oDataSend21 = { actorID21, seatID21, role21 };
                SendRPC(SHI_MIAN_ZHE_ACK, oDataSend21);
                break;
            case SHI_MIAN_ZHE_ACK://22
                if (isMaster) break;
                if (startRole != Role.SHI_MIAN_ZHE) break;
                object[] oDataRecv22 = (object[])obj.CustomData;
                int actorID22 = (int)oDataRecv22[0];
                int seatID22 = (int)oDataRecv22[1];
                Role role22 = (Role)oDataRecv22[2];
                if (actorID != actorID22) break;
                seatID22 = (seatID22 - mySeatID + playerLimit) % playerLimit;
                int cardID22 = CardManager.GetComponent<CardManager>().GetCardID(seatID22);
                CardManager.GetComponent<CardManager>().ShowRole(cardID22, role22);
                break;
            case JIU_GUI_REQ://23
                if (!isMaster) break;
                object[] oDataRecv23 = (object[])obj.CustomData;
                Debug.Log("收到酒鬼的消息");
                int actorID23 = (int)oDataRecv23[0];
                int seatID23_1 = (int)oDataRecv23[1];//自己的
                int seatID23_2 = (int)oDataRecv23[2];//上面的
                int seatID23_1_tmp = (seatID23_1 - mySeatID + playerLimit) % playerLimit;
                int seatID23_2_tmp = seatID23_2;
                Debug.Log("交换" + seatID23_1_tmp + ":" + seatID23_2_tmp);
                Role rl23_1 = seatRoleMap[seatID23_1_tmp];
                Role rl23_2 = seatRoleMap[seatID23_2_tmp];
                seatRoleMap[seatID23_1_tmp] = rl23_2;
                seatRoleMap[seatID23_2_tmp] = rl23_1;
                object[] oDataSend23 = { actorID23, seatID23_1, seatID23_2};
                SendRPC(JIU_GUI_ACK, oDataSend23);
                break;
            case JIU_GUI_ACK://24
                if (isMaster) break;
                if (startRole != Role.JIU_GUI) break;
                object[] oDataRecv24 = (object[])obj.CustomData;
                int actorID24 = (int)oDataRecv24[0];
                int seatID24_1 = (int)oDataRecv24[1];
                int seatID24_2 = (int)oDataRecv24[2];
                seatID24_1 = (seatID24_1 - mySeatID + playerLimit) % playerLimit;
                seatID24_2 = seatID24_2;
                if (actorID != actorID24) break;
                int cardID = CardManager.GetComponent<CardManager>().GetCardID(seatID24_2);
                nowState.Role_Select(cardID, Role.KA_BEI);
                break;
            case SHOW_RESULT_NTF://25
                if (isMaster) break;
                object[] oDataRecv25= (object[])obj.CustomData; 
                Debug.Log("收到主机展示结果");
                int masterSeat=(int)oDataRecv25[9];
                for (int i = 0; i < 9; i++)
                {
                    int stID25 = (i - mySeatID + masterSeat + playerLimit)%playerLimit;
                    if (i >= 6)
                    {
                        stID25 = i;
                    }
                    Role rl25 = (Role)oDataRecv25[i];
                    int cardID25 = CardManager.GetComponent<CardManager>().GetCardID(stID25);
                    Debug.Log("cardID:" + cardID25 + "role:" + rl25);
                    CardManager.GetComponent<CardManager>().ShowRole(cardID25, rl25);
                }
                break;
            case VOTE_REQ://26
                if (!isMaster) break;
                object[] oDataRecv26 = (object[])obj.CustomData;
                int actorID26 = (int)oDataRecv26[0];
                int his_seatID = (int)oDataRecv26[1];
                int vote_seatID = (int)oDataRecv26[2];
                if (vote_seatID != -1)
                {
                    vote_seatID = (vote_seatID + his_seatID - mySeatID + playerLimit) % playerLimit;
                }
                if (vote_seatID == -1)
                {
                    if (voteMap.ContainsKey(actorID26))
                    {
                        voteMap[actorID26] = -1;
                    }
                    else
                    {
                        voteMap.Add(actorID26, -1);
                    }
                }
                else
                {
                    if (voteMap.ContainsKey(actorID26))
                    {
                        voteMap[actorID26] = seatMap[vote_seatID];
                    }
                    else
                    {
                        voteMap.Add(actorID26, seatMap[vote_seatID]);
                    }
                }
                break;
            case VOTE_ACK://27
                if (isMaster) break;
                object[] oDataRecv27 = (object[])obj.CustomData;
                int seatID27 = (int)oDataRecv27[0];
                int num27 = (int)oDataRecv27[1];
                SetVote(seatID27, num27);
                break;
        } 
        
    }

    public void SetVote(int x,int y)
    {
        voteNum1 = x;
        voteNum2 = y;
        UIManager.GetComponent<UIManager>().SetVoteNum(x, y);
    }

    public void OnReadyClick()
    {
        this.nowState.OnReadyClick();
    }

    public void PlayerOnClick(int id)
    {
        this.nowState.OnPlayerClick(id);
    }

    public void DataRefresh()
    {
        playerList = PhotonNetwork.PlayerList;
        playerMap.Clear();
        foreach (var r in playerList)
        {
            playerMap.Add(r.ActorNumber, r);
        }
    }

    public void UIRefresh()
    {
        UIManager.GetComponent<UIManager>().SetMySeat(mySeatID);
        UIManager.GetComponent<UIManager>().SetIsReady(isReady, isMaster);
        UIManager.GetComponent<UIManager>().SetPlayerLimit(playerLimit);
        UIManager.GetComponent<UIManager>().SetPlayerMap(playerMap);
        UIManager.GetComponent<UIManager>().SetReadyMap(readyMap);
        UIManager.GetComponent<UIManager>().SetSeatMap(seatMap);
        UIManager.GetComponent<UIManager>().SetState(nowState.Estate);
        UIManager.GetComponent<UIManager>().SetTimer(nowState.m_time);

        UIManager.GetComponent<UIManager>().RefreshUI();
    }

    public void SendRPC(byte eventCode,object eventContent)//发消息
    {
        PhotonNetwork.RaiseEvent(eventCode, eventContent, RaiseEventOptions.Default, SendOptions.SendReliable);
    }

    public void OnCardClick(int cardID)
    {
        nowState.OnCardClick(cardID);
    }

    public void CreateRole()
    {
        int seed = (int)System.DateTime.Now.Ticks;
        UnityEngine.Random.InitState(seed);
        int cnt = 0;
        cardRoleMap = new Dictionary<int, Role>();
        while (cnt < 9)
        {
            int x = UnityEngine.Random.Range(0, 9);
            Role tmp = (Role)x;
            bool has = false; 
            foreach (var r in cardRoleMap)
            {
                if (r.Value == tmp)
                {
                    has = true;
                    break;
                }
            }
            if (!has)
            {
                cardRoleMap.Add(cnt, tmp);
                cnt++;
            }
        }
        //for (int i = 0; i < 9; i++)
        //    Debug.Log("card" + i + ":" + cardRoleMap[i]);
    }

    private void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += NetworkingClient_EventReceived;
    }

    private void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= NetworkingClient_EventReceived;
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        DataRefresh();
        UIRefresh();
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        DataRefresh();
        UIRefresh();
    }

    public void ExitRoom()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LeaveLobby();
        PhotonNetwork.LoadLevel(0);
    }
}
