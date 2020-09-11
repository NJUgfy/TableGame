using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitButton : MonoBehaviour
{
    //Transform transform;
    // Start is called before the first frame update
    Vector3 bigSize = new Vector3(1.2f, 1.2f, 1.2f);
    Vector3 smallSize = new Vector3(1.0f, 1.0f, 1.0f);
    public GameObject StateManager;

    void Start()
    {
        
    }

    public void OnEnter()
    {
        transform.localScale = bigSize;
    }

    public void OnExit()
    {
        transform.localScale = smallSize;
    }

    public void OnClick()
    {
        StateManager.GetComponent<StateManager>().ExitRoom();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
