using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEvent : MonoBehaviour
{
    public GameObject Human;
    public GameObject Wolf;
    public GameObject Color;
    public GameObject UIManager;
    public Sprite HumanSprite;
    public Sprite WolfSprite;
    public int id;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        UIManager.GetComponent<UIManager>().PlayerOnClick(id);
    }
    
    public void TurnGreen()
    {
        Color.GetComponent<Image>().color = new Color(6f / 255f, 250f / 255f, 72f / 255f, 1.0f);
    }

    public void TurnWhite()
    {
        Color.GetComponent<Image>().color = new Color(0, 0, 0, 0);
    }

    public void TurnToWolf()
    {
        Wolf.GetComponent<Image>().sprite = WolfSprite;
    }

    public void TurnToHuman()
    {
        Wolf.GetComponent<Image>().sprite = HumanSprite;
    }
}
