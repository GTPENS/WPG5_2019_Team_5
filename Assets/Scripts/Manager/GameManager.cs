using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject sliderObject;
    public GameObject bidText;

    NetworkManager network;

    void Start()
    {
        Data data = new Data("join");
        string jsonString = JsonUtility.ToJson(data);
        Debug.Log($"data: {data.command} json: {jsonString}");

        network = GetComponent<NetworkManager>();

        if (network.setupSocket())
        {
            Debug.Log("Connecting to Server");
            network.writeSocket(jsonString);

            Debug.Log($"Return from Server: {network.readSocket()}");
        }
        else
        {
            Debug.Log("Server is not Running");
        }
    
    }
}
