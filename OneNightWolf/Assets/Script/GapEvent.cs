using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GapEvent : MonoBehaviour
{
    public int belong;
    public int id;
    public GameObject CardManager;
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
        Debug.Log("belong:" + belong + ",pos:" + id);
    }
}
