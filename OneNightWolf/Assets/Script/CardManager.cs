using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public GameObject stateManager;

    public GameObject head0;
    public GameObject head1;
    public GameObject head2;
    public GameObject head3;
    public GameObject head4;
    public GameObject head5;

    public GameObject GetHead(int x)
    {
        switch (x)
        {
            case 0:
                return head0;
            case 1:
                return head1;
            case 2:
                return head2;
            case 3:
                return head3;
            case 4:
                return head4;
            case 5:
                return head5;
        }
        return head0;
    }

    public GameObject GetCard(int x)
    {
        switch (x)
        {
            case 0:
                return card0;
            case 1:
                return card1;
            case 2:
                return card2;
            case 3:
                return card3;
            case 4:
                return card4;
            case 5:
                return card5;
            case 6:
                return card6;
            case 7:
                return card7;
            case 8:
                return card8;

        }
        return card0;
    }

    public GameObject card0;
    public GameObject card1;
    public GameObject card2;
    public GameObject card3;
    public GameObject card4;
    public GameObject card5;
    public GameObject card6;
    public GameObject card7;
    public GameObject card8;

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

    private Vector3 hugeSize = new Vector3(3f, 3f, 3f);
    private Vector3 bigSize = new Vector3(1.2f, 1.2f, 1.2f);
    private Vector3 smallSize = new Vector3(1f, 1f, 1f);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick(int cardID)
    {
        stateManager.GetComponent<StateManager>().OnCardClick(cardID);
    }

    public void OnDePick(int cardID)
    {
        GetCard(cardID).GetComponent<CardEvent>().OnDisPick();
    }

    public void OnUnBanPick(int cardID)
    {
        GetCard(cardID).GetComponent<CardEvent>().UnBanPick();
    }

    public void OnUnBanPickBySeatID(int seatID)
    {
        for (int i = 0; i < 9; i++)
        {
            if (GetCard(i).GetComponent<CardEvent>().GetSeatID() == seatID)
            {
                GetCard(i).GetComponent<CardEvent>().UnBanPick();
            }
        }
    }
    public void OnBanPickBySeatID(int seatID)
    {
        for (int i = 0; i < 9; i++)
        {
            if (GetCard(i).GetComponent<CardEvent>().GetSeatID() == seatID)
            {
                GetCard(i).GetComponent<CardEvent>().BanPick();
            }
        }
    }


    public void OnUnBanAllPick()
    {
        for (int i = 0; i < 9; i++)
            OnUnBanPick(i);
    }

    public void OnBanPick(int cardID)
    {
        GetCard(cardID).GetComponent<CardEvent>().BanPick();
    }

    public void OnBanAllPick()
    {
        for (int i = 0; i < 9; i++)
            OnBanPick(i);
    }

    public void OnMySelect(int cardID)
    {
        GetCard(cardID).GetComponent<CardEvent>().OnMyPick();
    }

    public void OnOtherSelect(int cardID)
    {
        GetCard(cardID).GetComponent<CardEvent>().OnOthersPick();
    }

    public void MoveAllToInit()
    {
        card0.GetComponent<CardEvent>().MoveCard(start0, smallSize);
        card1.GetComponent<CardEvent>().MoveCard(start1, smallSize);
        card2.GetComponent<CardEvent>().MoveCard(start2, smallSize);
        card3.GetComponent<CardEvent>().MoveCard(start3, smallSize);
        card4.GetComponent<CardEvent>().MoveCard(start4, smallSize);
        card5.GetComponent<CardEvent>().MoveCard(start5, smallSize);
        card6.GetComponent<CardEvent>().MoveCard(start6, smallSize);
        card7.GetComponent<CardEvent>().MoveCard(start7, smallSize);
        card8.GetComponent<CardEvent>().MoveCard(start8, smallSize);
    }

    public void MoveAllToChoose()
    {
        card0.GetComponent<CardEvent>().MoveCard(desk0, smallSize);
        card1.GetComponent<CardEvent>().MoveCard(desk1, smallSize);
        card2.GetComponent<CardEvent>().MoveCard(desk2, smallSize);
        card3.GetComponent<CardEvent>().MoveCard(desk3, smallSize);
        card4.GetComponent<CardEvent>().MoveCard(desk4, smallSize);
        card5.GetComponent<CardEvent>().MoveCard(desk5, smallSize);
        card6.GetComponent<CardEvent>().MoveCard(desk6, smallSize);
        card7.GetComponent<CardEvent>().MoveCard(desk7, smallSize);
        card8.GetComponent<CardEvent>().MoveCard(desk8, smallSize);
    }
    
    public void MoveToMid(int cardID, Role _role)
    {
        GetCard(cardID).GetComponent<CardEvent>().SetRole(_role);
        GetCard(cardID).GetComponent<CardEvent>().MoveCard(midPos,hugeSize);
    }

    public void ShowRole(int cardID, Role _role)
    {
        GetCard(cardID).GetComponent<CardEvent>().SetRole(_role);
    }

    public void HideRole(int cardID)
    {
        GetCard(cardID).GetComponent<CardEvent>().SetRole(Role.KA_BEI);
    }

    public void HideAllRole()
    {
        for (int i = 0; i < 9; i++)
        {
            HideRole(i);
        }
    }

    public void MoveToSeat(int cardID, int seatID)
    {
        Vector3 tg = GetPlayerPos(seatID);
        GetCard(cardID).GetComponent<CardEvent>().MoveCard(tg, smallSize);
    }

    public void SetSeatID(int cardID, int seatID)
    {
        GetCard(cardID).GetComponent<CardEvent>().SetSeatID(seatID);
    }

    public void TurnWolf(int seatID)
    {
        GetHead(seatID).GetComponent<PlayerEvent>().TurnToWolf();
    }

    public void TurnHuman(int seatID)
    {
        GetHead(seatID).GetComponent<PlayerEvent>().TurnToHuman();
    }

    public void TurnAllToHuman(int len)
    {
        for (int i = 0; i < len; i++)
            TurnHuman(i);
    }

    public int GetSeatID(int cardID)
    {
       return GetCard(cardID).GetComponent<CardEvent>().GetSeatID();
    }

    public int GetCardID(int seatID)
    {
        for (int i=0;i<9;i++)
            if (GetCard(i).GetComponent<CardEvent>().GetSeatID() == seatID)
            {
                return i;
            }
        return 0;
    }
}
