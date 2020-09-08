using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviourPunCallbacks
{
    public GameObject vote_content;
    public GameObject vote_num1;
    public GameObject vote_num2;

    public GameObject playerNamePrefab;
    public Transform playerNameGridLayout;
    public GameObject playerNumText;
    public GameState state;
    public GameObject StateManager;

    public GameObject readyOrStartButton;

    public GameObject playerSeat0;
    public GameObject playerSeat1;
    public GameObject playerSeat2;
    public GameObject playerSeat3;
    public GameObject playerSeat4;
    public GameObject playerSeat5;

    public GameObject tips;
    public GameObject time1;
    public GameObject time2;

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
    public Sprite empty;

    public Sprite tips_init;
    public Sprite tips_choose;
    public Sprite tips_watch;
    public Sprite tips_lang_ren;
    public Sprite tips_zhua_ya;
    public Sprite tips_yu_yan_jia;
    public Sprite tips_hua_shen_you_ling;
    public Sprite tips_qiang_dao;
    public Sprite tips_dao_dan_gui;
    public Sprite tips_shi_mian_zhe;
    public Sprite tips_jiu_gui;
    public Sprite tips_free;
    public Sprite tips_show_result;

    public Sprite vote_words;


    private double timeFrame = -1;
    private int playerLimit = 0;
    private int mySeatID = 0;
    private bool isMaster;
    private bool isReady;
    private bool isVoting=false;
    private int voteNum1 = -1;
    private int voteNum2 = -1;

    private bool isResult = false;
    public Dictionary<int, Player> playerMap = new Dictionary<int, Player>(); // actorID player
    public Dictionary<int, int> seatMap = new Dictionary<int, int>();      // seatID  actorID
    public Dictionary<int, bool> readyMap = new Dictionary<int, bool>();     // actorID isReady



    private Sprite GetNumSprite(int x)
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
        return num0;
    }
    private void ShowTime(double sc)
    {
        int sec = (int)sc;
        if (sec == -1)
        {
            time1.GetComponent<Image>().sprite = empty;
            time2.GetComponent<Image>().sprite = empty;
            return;
        }
        int x = sec / 10;
        int y = sec % 10;
        time1.GetComponent<Image>().sprite = GetNumSprite(x);
        time2.GetComponent<Image>().sprite = GetNumSprite(y);
    }

    private void CheckPlayer()
    {
        GameObject[] pr = GameObject.FindGameObjectsWithTag("PlayerName");
        foreach (var r in pr)
        {
            Destroy(r);
        }
        if (playerMap == null)
        {
            playerMap = new Dictionary<int, Player>();
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
                if (readyMap.ContainsKey(r.Key)&&readyMap[r.Key]==true)
                {
                    name += "(已准备)";
                }
                else
                {
                    name += "(未准备)";
                }
            }
            newPlayer.GetComponentInChildren<Text>().text = name;
            //Debug.Log("playername:" + r.Value.NickName);
        }
        playerNumText.GetComponent<Text>().text = "房间人数: " + playerMap.Count.ToString() + "/" + playerLimit.ToString();
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
        }
        return playerSeat0;
    }

    private void CheckSeat()
    {
        for (int i = 0; i <= playerLimit; i++)
        {
            GameObject target = GetPlayerSeat(i);
            target.SetActive(false);
        }
        foreach (var r in seatMap)
        {
            //Debug.Log("seat:" + r.Key.ToString() + "id:" + r.Value.ToString() + "name:" + playerMap[r.Value].NickName);
            if (!playerMap.ContainsKey(r.Value)) continue;
            GameObject target = GetPlayerSeat(((r.Key - mySeatID + playerLimit) % playerLimit));
            string name = playerMap[r.Value].NickName;
            string seat = "(" + (r.Key + 1).ToString() + "号位)";
            target.GetComponentInChildren<Text>().text = name + seat;
            target.SetActive(true);
        }
    }

    private void CheckReady()
    {

    }

    public void PlayerOnClick(int x)
    {
        StateManager.GetComponent<StateManager>().PlayerOnClick(x);
    }

    public void PlayerTurnWhite(int x)
    {
        GetPlayerSeat(x).GetComponent<PlayerEvent>().TurnWhite();
    }

    public void PlayerTurnGreen(int x)
    {
        GetPlayerSeat(x).GetComponent<PlayerEvent>().TurnGreen();
    }

    public void PlayerAllTurnWhite()
    {
        for (int i = 0; i < 6; i++)
            PlayerTurnWhite(i);
    }

    private void CheckVote()
    {
        if (!isVoting)
        {
            vote_content.GetComponent<Image>().sprite = empty;
            vote_num1.GetComponent<Image>().sprite = empty;
            vote_num2.GetComponent<Image>().sprite = empty;
            return;
        }
        else
        {
            vote_content.GetComponent<Image>().sprite = vote_words;
            vote_num1.GetComponent<Image>().sprite = GetNumSprite(voteNum1+1);
            vote_num2.GetComponent<Image>().sprite = GetNumSprite(voteNum2);
        }
    }

    private void CheckTips()
    {
        switch(state){
            case GameState.INIT:
                tips.GetComponent<Image>().sprite = tips_init;
                break;
            case GameState.CHOOSE:
                tips.GetComponent<Image>().sprite = tips_choose;
                break;
            case GameState.WATCH:
                tips.GetComponent<Image>().sprite = tips_watch;
                break;
            case GameState.LANG_REN:
                tips.GetComponent<Image>().sprite = tips_lang_ren;
                break;
            case GameState.ZHUA_YA:
                tips.GetComponent<Image>().sprite = tips_zhua_ya;
                break;
            case GameState.YU_YAN_JIA:
                tips.GetComponent<Image>().sprite = tips_yu_yan_jia;
                break;
            case GameState.HUA_SHEN_YOU_LING:
                tips.GetComponent<Image>().sprite = tips_hua_shen_you_ling;
                break;
            case GameState.QIANG_DAO:
                tips.GetComponent<Image>().sprite = tips_qiang_dao;
                break;
            case GameState.DAO_DAN_GUI:
                tips.GetComponent<Image>().sprite = tips_dao_dan_gui;
                break;
            case GameState.SHI_MIAN_ZHE:
                tips.GetComponent<Image>().sprite = tips_shi_mian_zhe;
                break;
            case GameState.JIU_GUI:
                tips.GetComponent<Image>().sprite = tips_jiu_gui;
                break;
            case GameState.FREE:
                tips.GetComponent<Image>().sprite = tips_free;
                break;
            case GameState.SHOW_RESULT:
                tips.GetComponent<Image>().sprite = tips_show_result;
                break;
            default:
                tips.GetComponent<Image>().sprite = null;
                break;
        }

    }

    public void SetTimer(double frame)
    {
        timeFrame = frame;
    }

    public void SetPlayerLimit(int x)
    {
        playerLimit = x;
    }

    public void SetPlayerMap(Dictionary<int, Player> _playerMap)
    {
        playerMap = _playerMap;
        CheckPlayer();
    }

    public void SetSeatMap(Dictionary<int, int> _seatMap)
    {
        seatMap = _seatMap;
        CheckSeat();
    }

    public void SetReadyMap(Dictionary<int, bool> _readyMap)
    {
        readyMap = _readyMap;
        CheckReady();
    }

    public void SetVoting(bool pd)
    {
        isVoting = pd;
    }
    public void SetVoteNum(int x,int y)
    {
        voteNum1 = x;
        voteNum2 = y;
    }

    public void SetResult(bool pd)
    {
        isResult = pd;
        if (isMaster)
        {
            SetIsReady(true, isMaster);
        }
    }

    public void SetIsReady(bool _isReady,bool _isMaster)
    {
        string msg = "";
        if (_isMaster)
        {
            isMaster = true;
            msg = "开始";
            if (isResult)
            {
                msg = "结算投票";
            }
        }
        else
        {
            isMaster = false;
            if (!_isReady)
            {
                isReady = false;
                msg = "准备";
            }
            else
            {
                isReady = true;
                msg = "已准备";
            }
        }
        readyOrStartButton.GetComponentInChildren<Text>().text = msg; 
    }

    public void SetState(GameState _state)
    {
        state = _state;
        CheckTips();
    }

    public void SetMySeat(int _seat)
    {
        mySeatID = _seat;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void CheckTimer()
    {
        if (timeFrame <= -1)
        {
            ShowTime(-1);
        }
        else
        {
            ShowTime(timeFrame);
            timeFrame -= Time.deltaTime;
        }
    }

    public void OnReadyOrStartButtonClick()
    {
        StateManager.GetComponent<StateManager>().OnReadyClick();
    }

    public void RefreshUI()
    {
        CheckPlayer();
        CheckReady();
        CheckSeat();
        CheckTimer();
        CheckTips();
        CheckVote();
    }

    // Update is called once per frame
    void Update()
    {
        CheckTimer();
    }
}
