using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardEvent : MonoBehaviour
{
    public GameObject cardManager;
    public int id;
    public GameObject colorGameObject;
    public GameObject frontGameObject;
    public GameObject backGameObject;
    private int frame;
    private bool isSelect = false;
    public bool isSelectByMe = false;
    public int playerPos = -1;
    public int showMeFrame = 0;
    public bool isSizing = false;
    public int nowPosID = -1;
    public Role role;

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

    public int seatID;

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
    

    private Vector3 targetPos;

    private Vector3 hugeSize = new Vector3(3f, 3f, 3f);
    private Vector3 bigSize = new Vector3(1.2f, 1.2f, 1.2f);
    private Vector3 smallSize = new Vector3(1f, 1f, 1f);
    private Vector3 targetSize = new Vector3(1f, 1f, 1f);
    private bool isMoving = false;
    

    public void MoveCard(Vector3 _target, Vector3 sz)
    {
        targetPos = _target;
        targetSize = sz;
        isMoving = true;
        isSizing = true;
    }

    // Start is called before the first frame update
    void Start()
    {
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
            case Role.HUA_SHEN_YOU_LING:
                backGameObject.GetComponent<Image>().sprite = hua_shen_you_ling;
                break;
            case Role.SHI_MIAN_ZHE:
                backGameObject.GetComponent<Image>().sprite = shi_mian_zhe;
                break;
            case Role.QIANG_DAO:
                backGameObject.GetComponent<Image>().sprite = qiang_dao;
                break;
            case Role.DAO_DAN_GUI:
                backGameObject.GetComponent<Image>().sprite = dao_dan_gui;
                break;
            case Role.ZHUA_YA:
                backGameObject.GetComponent<Image>().sprite = zhua_ya;
                break;
            case Role.LANG_REN_1:
                backGameObject.GetComponent<Image>().sprite = lang_ren_1;
                break;
            case Role.LANG_REN_2:
                backGameObject.GetComponent<Image>().sprite = lang_ren_2;
                break;
            case Role.JIU_GUI:
                backGameObject.GetComponent<Image>().sprite = jiu_gui;
                break;
            case Role.YU_YAN_JIA:
                backGameObject.GetComponent<Image>().sprite = yu_yan_jia;
                break;

            default:
                backGameObject.GetComponent<Image>().sprite = ka_bei;
                break;
        }
    }

    public void SetRole(Role _role)
    {
        role = _role;
        Debug.Log("My Role:" + role);
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

    public void OnOthersPick()
    {
        isSelect = true;
        TurnRed();
    }

    public void OnMyPick()
    {
        isSelect = true;
        isSelectByMe = true;
        this.transform.SetAsLastSibling();
        TurnGreen();
        //ShowFront();
    }

    public void OnDisPick()
    {
        isSelect = false;
        isSelectByMe = false;
        TurnWhite();
    }

    public void BanPick()
    {
        isSelect = true;
        TurnWhite();
        TurnSmall();
    }

    public void UnBanPick()
    {
        isSelect = false;
        TurnWhite();
        TurnSmall();
    }

    public void SetSeatID(int x)
    {
        seatID = x;
    }

    public int GetSeatID()
    {
        return seatID;
    }

    public void Click()
    {
        cardManager.GetComponent<CardManager>().OnClick(id);
    }
}
