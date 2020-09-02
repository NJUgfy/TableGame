using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEvent : MonoBehaviour
{
    public GameObject Human;
    public GameObject Wolf;
    public Sprite HumanSprite;
    public Sprite WolfSprite;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
