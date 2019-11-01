using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject sliderObject;
    public GameObject bidText;

    NetworkManager network;
    bool isServerOn;

    void Start()
    {
        Data data = new Data("join");
        string jsonString = JsonUtility.ToJson(data);

        network = GetComponent<NetworkManager>();
        isServerOn = network.setupSocket();

        if (isServerOn)
        {
            Debug.Log("Connecting to Server");
            Debug.Log($"Send {jsonString} to Server");
            
            network.writeSocket(jsonString);
            Debug.Log($"Receive {network.readSocket()} from Server");
        }
        else
        {
            Debug.Log("Server is not Running");
        }
    }

    // void Update()
    // {
    //     if (isServerOn && Input.GetKeyDown(KeyCode.Space))
    //     {
    //         Data data = new Data("join");
    //         string jsonString = JsonUtility.ToJson(data);

    //         network.writeSocket(jsonString);
    //         Debug.Log($"Return from Server: {network.readSocket()}");
    //     }
    // }
}
