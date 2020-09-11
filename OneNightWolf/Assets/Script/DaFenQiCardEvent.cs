using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DaFenQiCardEvent : MonoBehaviour
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

    public int col;
    public int num;

    public Sprite black_0;
    public Sprite black_1;
    public Sprite black_2;
    public Sprite black_3;
    public Sprite black_4;
    public Sprite black_5;
    public Sprite black_6;
    public Sprite black_7;
    public Sprite black_8;
    public Sprite black_9;
    public Sprite black_10;
    public Sprite black_11;
    public Sprite black_gang;
    public Sprite black_back;

    public Sprite white_0;
    public Sprite white_1;
    public Sprite white_2;
    public Sprite white_3;
    public Sprite white_4;
    public Sprite white_5;
    public Sprite white_6;
    public Sprite white_7;
    public Sprite white_8;
    public Sprite white_9;
    public Sprite white_10;
    public Sprite white_11;
    public Sprite white_gang;
    public Sprite white_back;

    public int seatID;
    public int seatOrder;

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
    private Vector3 bigSize = new Vector3(0.6f, 0.6f, 0.6f);
    private Vector3 smallSize = new Vector3(0.5f, 0.5f, 0.5f);
    private Vector3 targetSize = new Vector3(0.5f, 0.5f, 0.5f);
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
        OnChangeFront();
    }

    // Update is called once per frame
    void Update()
    {
        frame++;
        if (isSizing && targetSize != null)
        {
            Vector3 del = targetSize - transform.localScale;
            float dis = Vector3.Distance(transform.localScale, targetSize);
            Vector3 mid = transform.localScale + (del / 20.0f);
            transform.localScale = mid;
            if (dis <= 0.05f)
            {
                isSizing = false;
                transform.localScale = targetSize;
            }
        }
        if (isMoving && targetPos != null)
        {

            Vector3 del = targetPos - transform.position;
            float dis = Vector3.Distance(transform.position, targetPos);
            Vector3 mid = transform.position + (del / 30.0f);
            transform.position = mid;
            if (dis <= 0.05f)
            {
                isMoving = false;
                transform.position = targetPos;
            }
        }
        if (frame % 64 == 0)
        {
            if (color == EColor.NONE)
            {
                colorGameObject.GetComponent<Image>().color = new Color(0, 0, 0, 0);
            }
            else
            if (color == EColor.GREEN)
            {
                colorGameObject.GetComponent<Image>().color = new Color(6f / 255f, 250f / 255f, 72f / 255f, 1.0f);
            }
            else
            if (color == EColor.RED)
            {
                colorGameObject.GetComponent<Image>().color = new Color(250f / 255f, 17f / 255f, 6f / 255f, 1.0f);
            }
        }

        if (frame == 64 * 64) frame = 0;
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
        if (col == 0)
        {
            switch (num)
            {
                case 0:
                    backGameObject.GetComponent<Image>().sprite = black_0;
                    break;
                case 1:
                    backGameObject.GetComponent<Image>().sprite = black_1;
                    break;
                case 2:
                    backGameObject.GetComponent<Image>().sprite = black_2;
                    break;
                case 3:
                    backGameObject.GetComponent<Image>().sprite = black_3;
                    break;
                case 4:
                    backGameObject.GetComponent<Image>().sprite = black_4;
                    break;
                case 5:
                    backGameObject.GetComponent<Image>().sprite = black_5;
                    break;
                case 6:
                    backGameObject.GetComponent<Image>().sprite = black_6;
                    break;
                case 7:
                    backGameObject.GetComponent<Image>().sprite = black_7;
                    break;
                case 8:
                    backGameObject.GetComponent<Image>().sprite = black_8;
                    break;
                case 9:
                    backGameObject.GetComponent<Image>().sprite = black_9;
                    break;
                case 10:
                    backGameObject.GetComponent<Image>().sprite = black_10;
                    break;
                case 11:
                    backGameObject.GetComponent<Image>().sprite = black_11;
                    break;
                case -1:
                    backGameObject.GetComponent<Image>().sprite = black_gang;
                    break;
                case -2:
                    backGameObject.GetComponent<Image>().sprite = black_back;
                    break;
            }
        }else if (col == 1)
        {
            switch (num)
            {
                case 0:
                    backGameObject.GetComponent<Image>().sprite = white_0;
                    break;
                case 1:
                    backGameObject.GetComponent<Image>().sprite = white_1;
                    break;
                case 2:
                    backGameObject.GetComponent<Image>().sprite = white_2;
                    break;
                case 3:
                    backGameObject.GetComponent<Image>().sprite = white_3;
                    break;
                case 4:
                    backGameObject.GetComponent<Image>().sprite = white_4;
                    break;
                case 5:
                    backGameObject.GetComponent<Image>().sprite = white_5;
                    break;
                case 6:
                    backGameObject.GetComponent<Image>().sprite = white_6;
                    break;
                case 7:
                    backGameObject.GetComponent<Image>().sprite = white_7;
                    break;
                case 8:
                    backGameObject.GetComponent<Image>().sprite = white_8;
                    break;
                case 9:
                    backGameObject.GetComponent<Image>().sprite = white_9;
                    break;
                case 10:
                    backGameObject.GetComponent<Image>().sprite = white_10;
                    break;
                case 11:
                    backGameObject.GetComponent<Image>().sprite = white_11;
                    break;
                case -1:
                    backGameObject.GetComponent<Image>().sprite = white_gang;
                    break;
                case -2:
                    backGameObject.GetComponent<Image>().sprite = white_back;
                    break;
            }
        }
    }
    
    public void SetCol(int _col)
    {
        col = _col;
    }

    public void SetNum(int _num)
    {
        num = _num;
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
        
    }
}
