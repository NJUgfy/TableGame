using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class RoomListManager : MonoBehaviourPunCallbacks
{
    public GameObject roomNamePrefab;
    public Transform gridLayout;
    private Dictionary<string, RoomInfo> roomMap;
    public void Awake()
    {
        roomMap = new Dictionary<string, RoomInfo>();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        GameObject[] pr = GameObject.FindGameObjectsWithTag("Room");
        foreach(var r in pr)
        {
            Destroy(r);
        }
        foreach (var r in roomList)
        {
            if (!r.IsOpen || !r.IsVisible || r.RemovedFromList)
            {
                if (roomMap.ContainsKey(r.Name))
                {
                    roomMap.Remove(r.Name);
                }
                continue;
            }

            if (roomMap.ContainsKey(r.Name))
            {
                roomMap[r.Name] = r;
            }
            else
            {
                roomMap.Add(r.Name, r);
            }
        }
        Debug.Log(roomList.Count);
        foreach(var r in roomMap)
        {
            GameObject newRoom = Instantiate(roomNamePrefab, gridLayout.position, Quaternion.identity,gridLayout.transform);
            newRoom.GetComponentInChildren<Text>().text = "[" + r.Key + "]" + "(" + r.Value.PlayerCount.ToString()+ "/"+ r.Value.MaxPlayers.ToString() + ")"; 
            Debug.Log(newRoom.GetComponentInChildren<Text>().text);
            newRoom.GetComponent<Button>().onClick.AddListener(
                    delegate
                    {
                        JoinRoom(r.Key);
                    }
                );
        }
    }

    void JoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
