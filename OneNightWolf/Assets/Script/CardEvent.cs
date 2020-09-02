using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardEvent : MonoBehaviour
{
    public int id;
    public GameObject colorGameObject;
    public GameObject frontGameObject;
    public GameObject backGameObject;
    private int frame;
    private bool isSelect = false;
    public bool isSelectByMe = false;
    public GameObject roomManager;
    public RoomLanucher.Role role;
    public RoomLanucher.Role beginRole;
    public int playerPos = -1;
    public int showMeFrame = 0;
    public bool isSizing = false;
    public int nowPosID = -1;

    public Sprite hua_shen_you_ling;
    public Sprite shi_mian_zhe;
    public Sprite qiang_dao;
    public Sprite dao_dan_gui;
    public Sprite zhua_ya;
    public Sprite lang_ren_1;
    public Sprite lang_ren_2;
    public Sprite jiu_gui;
    public Sprite yu_yan_jia;
    public Sprite ka_bei;

    private void DoNothing()
    {
        this.CancelInvoke();
    }
    public enum EColor
    {
        NONE = 0,
        GREEN = 1,
        RED = 2
    }
    public EColor color;


    public enum PosType{
        START = 0,
        DESK = 1,
        PLAYER = 2
    }
    public PosType posType;

    public enum GameState
    {
        START = 0,
        DESK = 1,
        PLAYER = 2,
        LANG_REN = 3,
        ZHUA_YA = 4
    }

    public GameState gameState;

    public void SetState(GameState state){
        gameState = state;
        switch (gameState) {
            case GameState.START:
                MoveCard(PosType.START, id, smallSize);
                break;
            case GameState.DESK:
                MoveCard(PosType.DESK, id, smallSize);
                break;
            case GameState.PLAYER:
                TurnWhite();
                if (playerPos == -1) break;
                ShowFront();
                if (!isSelectByMe)
                {
                    MoveCard(PosType.PLAYER, playerPos, smallSize);
                    break;
                }
                MoveCard(PosType.DESK, 5, hugeSize);
                //MoveCard(PosType.PLAYER, playerPos);
                showMeFrame = 150;
                break;
            case GameState.LANG_REN:
                break;
            case GameState.ZHUA_YA:
                break;
        }
    }

    public GameState GetGameState()
    {
        return gameState;
    }

    public int GetPosID()
    {
        return nowPosID;
    }

    public Vector3 GetStartPos(int x)
    {
        switch (x)
        {
            case 0:
                return start0;
            case 1:
                return start1;
            case 2:
                return start2;
            case 3:
                return start3;
            case 4:
                return start4;
            case 5:
                return start5;
            case 6:
                return start6;
            case 7:
                return start7;
            case 8:
                return start8;
        }
        return start0;
    }
    private Vector3 start0 = new Vector3(4.2f, -2.9f, 90f);
    private Vector3 start1 = new Vector3(4.7f, -2.9f, 90f);
    private Vector3 start2 = new Vector3(5.2f, -2.9f, 90f);

    internal void SetState(RoomLanucher.GameState state)
    {
        Debug.Log("state changed");
        switch (state)
        {
            case RoomLanucher.GameState.START:
                SetState(GameState.START);
                break;
            case RoomLanucher.GameState.DESK:
                SetState(GameState.DESK);
                break;
            case RoomLanucher.GameState.PLAYER:
                SetState(GameState.PLAYER);
                break;
            case RoomLanucher.GameState.LANG_REN:
                SetState(GameState.LANG_REN);
                break;
            case RoomLanucher.GameState.ZHUA_YA:
                SetState(GameState.ZHUA_YA);
                break;
        }
    }

    private Vector3 start3 = new Vector3(5.7f, -2.9f, 90f);
    private Vector3 start4 = new Vector3(6.2f, -2.9f, 90f);
    private Vector3 start5 = new Vector3(6.7f, -2.9f, 90f);
    private Vector3 start6 = new Vector3(7.2f, -2.9f, 90f);
    private Vector3 start7 = new Vector3(7.7f, -2.9f, 90f);
    private Vector3 start8 = new Vector3(8.2f, -2.9f, 90f);

    public Vector3 GetDeskPos(int x)
    {
        switch (x)
        {
            case 0:
                return desk0;
            case 1:
                return desk1;
            case 2:
                return desk2;
            case 3:
                return desk3;
            case 4:
                return desk4;
            case 5:
                return desk5;
            case 6:
                return desk6;
            case 7:
                return desk7;
            case 8:
                return desk8;
        }
        return desk0;
    }
    private Vector3 desk0 = new Vector3(-3.2f, 0.7f, 90f);
    private Vector3 desk1 = new Vector3(-1.4f, 0.7f, 90f);
    private Vector3 desk2 = new Vector3(0.4f, 0.7f, 90f);
    private Vector3 desk3 = new Vector3(-3.2f, -1.4f, 90f);
    private Vector3 desk4 = new Vector3(-1.4f, -1.4f, 90f);
    private Vector3 desk5 = new Vector3(0.4f, -1.4f, 90f);
    private Vector3 desk6 = new Vector3(-3.2f, -3.5f, 90f);
    private Vector3 desk7 = new Vector3(-1.4f, -3.5f, 90f);
    private Vector3 desk8 = new Vector3(0.4f, -3.5f, 90f);

    public Vector3 GetPlayerPos(int x)
    {
        switch (x)
        {
            case 0:
                return player0;
            case 1:
                return player1;
            case 2:
                return player2;
            case 3:
                return player3;
            case 4:
                return player4;
            case 5:
                return player5;
            case 6:
                return player6;
            case 7:
                return player7;
            case 8:
                return player8;

        }
        return player0;
    }
    private Vector3 player0 = new Vector3(-6.1f, -4.0f, 90f);
    private Vector3 player1 = new Vector3(-6.3f, 0.5f, 90f);
    private Vector3 player2 = new Vector3(-4.1f, 1.6f, 90f);
    private Vector3 player3 = new Vector3(-1.3f, 1.6f, 90f);
    private Vector3 player4 = new Vector3(1.2f, 1.6f, 90f);
    private Vector3 player5 = new Vector3(3.2f, -0.2f, 90f);
    private Vector3 player6 = new Vector3(-3.3f, -1.5f, 90f);
    private Vector3 player7 = new Vector3(-1.4f, -1.5f, 90f);
    private Vector3 player8 = new Vector3(0.5f, -1.5f, 90f);

    private Vector3 midPos = new Vector3(1.2f, 1.6f, 90f);

    private Vector3 targetPos;

    private Vector3 hugeSize = new Vector3(3f, 3f, 3f);
    private Vector3 bigSize = new Vector3(1.2f, 1.2f, 1.2f);
    private Vector3 smallSize = new Vector3(1f, 1f, 1f);
    private Vector3 targetSize = new Vector3(1f, 1f, 1f);
    private bool isMoving = false;

    public void MoveCard(PosType tp,int posID, Vector3 sz)
    {
        targetSize = sz;
        nowPosID = posID;
        switch (tp)
        {
            case PosType.START:
               targetPos = GetStartPos(posID);
               break;
            case PosType.DESK:
                targetPos = GetDeskPos(posID);
                break;
            case PosType.PLAYER:
                targetPos = GetPlayerPos(posID);
                break;
        }
        isMoving = true;
        isSizing = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        MoveCard(PosType.START, id, smallSize);
    }

    // Update is called once per frame
    void Update()
    {
        frame++;
        if (isSizing&&targetSize != null)
        {
            Vector3 del = targetSize - transform.localScale;
            float dis = Vector3.Distance(transform.localScale,targetSize);
            Vector3 mid = transform.localScale + (del / 20.0f);
            transform.localScale = mid;
            if (dis<=0.05f)
            {
                isSizing = false;
                transform.localScale = targetSize;
            }
        }
        if (isMoving&&targetPos != null)
        {
            
            Vector3 del = targetPos - transform.position;
            float dis = Vector3.Distance(transform.position, targetPos);
            Vector3 mid = transform.position + (del/30.0f);
            transform.position = mid;
            if (dis <= 0.05f) isMoving = false;
        }
        if (frame%64==0)
        {
            if (color == EColor.NONE)
            {
                colorGameObject.GetComponent<Image>().color = new Color(0, 0, 0, 0);
            }else
            if (color == EColor.GREEN)
            {
                colorGameObject.GetComponent<Image>().color = new Color(6f/255f, 250f/255f, 72f/255f, 1.0f);
            }else
            if (color == EColor.RED)
            {
                colorGameObject.GetComponent<Image>().color = new Color(250f / 255f, 17f / 255f, 6f / 255f, 1.0f);
            }
        }
        
        if (frame == 64*64) frame = 0;
        if (showMeFrame > 0)
        {
            showMeFrame--;
            if (showMeFrame == 0)
            {
                //HideFront();
                MoveCard(PosType.PLAYER, playerPos, smallSize);
            }
        }
    }

    public void Enter()
    {
        if (isSelect) return;
        transform.localScale = bigSize;
    }
    public void Exit()
    {
        if (isSelect) return;
        transform.localScale = smallSize;
    }

    public void Select()
    {
        
    }
   

    public void DeSelect()
    {
        
    }

    private void TurnGreen()
    {
        color = EColor.GREEN;
    }

    private void TurnRed()
    {
        color = EColor.RED;
    }

    private void TurnWhite()
    {
        color = EColor.NONE;
    }

    private void TurnBig()
    {
        transform.localScale = bigSize;
    }

    private void TurnSmall()
    {
        transform.localScale = smallSize;
    }

    
    
    public void OnChangeFront()
    {
        switch (role)
        {
            case RoomLanucher.Role.HUA_SHEN_YOU_LING:
                frontGameObject.GetComponent<Image>().sprite = hua_shen_you_ling;
                break;
            case RoomLanucher.Role.SHI_MIAN_ZHE:
                frontGameObject.GetComponent<Image>().sprite = shi_mian_zhe;
                break;
            case RoomLanucher.Role.QIANG_DAO:
                frontGameObject.GetComponent<Image>().sprite = qiang_dao;
                break;
            case RoomLanucher.Role.DAO_DAN_GUI:
                frontGameObject.GetComponent<Image>().sprite = dao_dan_gui;
                break;
            case RoomLanucher.Role.ZHUA_YA:
                frontGameObject.GetComponent<Image>().sprite = zhua_ya;
                break;
            case RoomLanucher.Role.LANG_REN_1:
                frontGameObject.GetComponent<Image>().sprite = lang_ren_1;
                break;
            case RoomLanucher.Role.LANG_REN_2:
                frontGameObject.GetComponent<Image>().sprite = lang_ren_2;
                break;
            case RoomLanucher.Role.JIU_GUI:
                frontGameObject.GetComponent<Image>().sprite = jiu_gui;
                break;
            case RoomLanucher.Role.YU_YAN_JIA:
                frontGameObject.GetComponent<Image>().sprite = yu_yan_jia;
                break;

            default:
                frontGameObject.GetComponent<Image>().sprite = ka_bei;
                break;
        }
    }

    public void SetRole(RoomLanucher.Role startRole)
    {
        role = startRole;
        OnChangeFront();
    }

    public void ShowFront()
    {
        backGameObject.SetActive(false);
    }

    public void HideFront()
    {
        backGameObject.SetActive(true);
    }

    public void OnOthersPick(int seatID)
    {
        playerPos = seatID;
        isSelect = true;
        TurnRed();
    }

    public void OnMyPick(int seatID)
    {
        playerPos = seatID;
        isSelect = true;
        isSelectByMe = true;
        this.tag = "Head";
        TurnGreen();
        //ShowFront();
    }

    public void Click()
    {
        roomManager.GetComponent<RoomLanucher>().OnClickCard(id);
    }
}
