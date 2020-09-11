using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaFenQiCardManager : MonoBehaviour
{
    public GameObject card0;
    public GameObject card1;
    public GameObject card2;
    public GameObject card3;
    public GameObject card4;
    public GameObject card5;
    public GameObject card6;
    public GameObject card7;
    public GameObject card8;
    public GameObject card9;
    public GameObject card10;
    public GameObject card11;
    public GameObject card12;
    public GameObject card13;
    public GameObject card14;
    public GameObject card15;
    public GameObject card16;
    public GameObject card17;
    public GameObject card18;
    public GameObject card19;
    public GameObject card20;
    public GameObject card21;
    public GameObject card22;
    public GameObject card23;
    public GameObject card24;
    public GameObject card25;

    public GameObject GetCard(int id)
    {
        switch (id)
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
            case 9:
                return card9;
            case 10:
                return card10;
            case 11:
                return card11;
            case 12:
                return card12;
            case 13:
                return card13;
            case 14:
                return card14;
            case 15:
                return card15;
            case 16:
                return card16;
            case 17:
                return card17;
            case 18:
                return card18;
            case 19:
                return card19;
            case 20:
                return card20;
            case 21:
                return card21;
            case 22:
                return card22;
            case 23:
                return card23;
            case 24:
                return card24;
            case 25:
                return card25;

        }
        return card0;
    }

    private Vector3 bigSize = new Vector3(0.6f, 0.6f, 0.6f);
    private Vector3 smallSize = new Vector3(0.5f, 0.5f, 0.5f);
    private Vector3 targetSize = new Vector3(0.5f, 0.5f, 0.5f);

    private Vector3 start0 = new Vector3(-5.9f, 0.7f, 90f);
    private Vector3 start1 = new Vector3(-4.9f, 0.7f, 90f);
    private Vector3 start2 = new Vector3(-4.0f, 0.7f, 90f);
    private Vector3 start3 = new Vector3(-3.1f, 0.7f, 90f);
    private Vector3 start4 = new Vector3(-2.3f, 0.7f, 90f);
    private Vector3 start5 = new Vector3(-1.4f, 0.7f, 90f);
    private Vector3 start6 = new Vector3(-0.5f, 0.7f, 90f);
    private Vector3 start7 = new Vector3(0.4f, 0.7f, 90f);
    private Vector3 start8 = new Vector3(1.3f, 0.7f, 90f);
    private Vector3 start9 = new Vector3(2.2f, 0.7f, 90f);
    private Vector3 start10 = new Vector3(3.1f, 0.7f, 90f);
    private Vector3 start11 = new Vector3(4.0f, 0.7f, 90f);
    private Vector3 start12 = new Vector3(4.9f, 0.7f, 90f);

    private Vector3 start13 = new Vector3(-5.9f, -0.7f, 90f);
    private Vector3 start14 = new Vector3(-4.9f, -0.7f, 90f);
    private Vector3 start15 = new Vector3(-4.0f, -0.7f, 90f);
    private Vector3 start16 = new Vector3(-3.1f, -0.7f, 90f);
    private Vector3 start17 = new Vector3(-2.3f, -0.7f, 90f);
    private Vector3 start18 = new Vector3(-1.4f, -0.7f, 90f);
    private Vector3 start19 = new Vector3(-0.5f, -0.7f, 90f);
    private Vector3 start20 = new Vector3(0.4f, -0.7f, 90f);
    private Vector3 start21 = new Vector3(1.3f, -0.7f, 90f);
    private Vector3 start22 = new Vector3(2.2f, -0.7f, 90f);
    private Vector3 start23 = new Vector3(3.1f, -0.7f, 90f);
    private Vector3 start24 = new Vector3(4.0f, -0.7f, 90f);
    private Vector3 start25 = new Vector3(4.9f, -0.7f, 90f);
    public Vector3 GetStart(int _id)
    {
        switch (_id)
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
            case 9:
                return start9;
            case 10:
                return start10;
            case 11:
                return start11;
            case 12:
                return start12;
            case 13:
                return start13;
            case 14:
                return start14;
            case 15:
                return start15;
            case 16:
                return start16;
            case 17:
                return start17;
            case 18:
                return start18;
            case 19:
                return start19;
            case 20:
                return start20;
            case 21:
                return start21;
            case 22:
                return start22;
            case 23:
                return start23;
            case 24:
                return start24;
            case 25:
                return start25;

        }
        return start0;
    }
    
    private Vector3 myPos0 = new Vector3(-4.0f, -4.5f, 90f);
    private Vector3 myPos1 = new Vector3(-3.1f, -4.5f, 90f);
    private Vector3 myPos2 = new Vector3(-2.2f, -4.5f, 90f);
    private Vector3 myPos3 = new Vector3(-1.3f, -4.5f, 90f);
    private Vector3 myPos4 = new Vector3(-0.4f, -4.5f, 90f);
    private Vector3 myPos5 = new Vector3(0.5f, -4.5f, 90f);
    private Vector3 myPos6 = new Vector3(1.4f, -4.5f, 90f);
    private Vector3 myPos7 = new Vector3(2.3f, -4.5f, 90f);
    private Vector3 myPos8 = new Vector3(3.2f, -4.5f, 90f);
    private Vector3 myPos9 = new Vector3(4.1f, -4.5f, 90f);
    private Vector3 myPos10 = new Vector3(5.0f, -4.5f, 90f);
    private Vector3 myPos11 = new Vector3(5.9f, -4.5f, 90f);
    private Vector3 myPos12 = new Vector3(6.8f, -4.5f, 90f);

    public Vector3 GetMyPos(int _id)
    {
        switch (_id)
        {
            case 0:
                return myPos0;
            case 1:
                return myPos1;
            case 2:
                return myPos2;
            case 3:
                return myPos3;
            case 4:
                return myPos4;
            case 5:
                return myPos5;
            case 6:
                return myPos6;
            case 7:
                return myPos7;
            case 8:
                return myPos8;
            case 9:
                return myPos9;
            case 10:
                return myPos10;
            case 11:
                return myPos11;
            case 12:
                return myPos12;
        }
        return myPos0;
    }

    private Vector3 hisPos0 = new Vector3(4.9f, 4.5f, 90f);
    private Vector3 hisPos1 = new Vector3(4.0f, 4.5f, 90f);
    private Vector3 hisPos2 = new Vector3(3.1f, 4.5f, 90f);
    private Vector3 hisPos3 = new Vector3(2.2f, 4.5f, 90f);
    private Vector3 hisPos4 = new Vector3(1.3f, 4.5f, 90f);
    private Vector3 hisPos5 = new Vector3(0.4f, 4.5f, 90f);
    private Vector3 hisPos6 = new Vector3(-0.5f, 4.5f, 90f);
    private Vector3 hisPos7 = new Vector3(-1.4f, 4.5f, 90f);
    private Vector3 hisPos8 = new Vector3(-2.3f, 4.5f, 90f);
    private Vector3 hisPos9 = new Vector3(-3.1f, 4.5f, 90f);
    private Vector3 hisPos10 = new Vector3(-4.0f, 4.5f, 90f);
    private Vector3 hisPos11 = new Vector3(-4.9f, 4.5f, 90f);
    private Vector3 hisPos12 = new Vector3(-5.9f, 4.5f, 90f);

    public Vector3 GetHisPos(int _id)
    {
        switch (_id)
        {
            case 0:
                return hisPos0;
            case 1:
                return hisPos1;
            case 2:
                return hisPos2;
            case 3:
                return hisPos3;
            case 4:
                return hisPos4;
            case 5:
                return hisPos5;
            case 6:
                return hisPos6;
            case 7:
                return hisPos7;
            case 8:
                return hisPos8;
            case 9:
                return hisPos9;
            case 10:
                return hisPos10;
            case 11:
                return hisPos11;
            case 12:
                return hisPos12;
        }
        return hisPos0;
    }
   
    public void MoveAllToStart()
    {
        for (int i = 0; i <= 12; i++)
        {
            GetCard(i).GetComponent<DaFenQiCardEvent>().SetCol(0);
            GetCard(i).GetComponent<DaFenQiCardEvent>().SetNum(-2);
            //GetCard(i).GetComponent<DaFenQiCardEvent>().MoveCard(GetStart(i), smallSize);
            GetCard(i).GetComponent<DaFenQiCardEvent>().MoveCard(GetMyPos(i), smallSize);
        }
        for (int i = 13; i <= 25; i++)
        {
            GetCard(i).GetComponent<DaFenQiCardEvent>().SetCol(1);
            GetCard(i).GetComponent<DaFenQiCardEvent>().SetNum(-2);
            //GetCard(i).GetComponent<DaFenQiCardEvent>().MoveCard(GetStart(i), smallSize);
            GetCard(i).GetComponent<DaFenQiCardEvent>().MoveCard(GetHisPos(i-13), smallSize);
        }


    }

    // Start is called before the first frame update
    void Start()
    {
        MoveAllToStart();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
