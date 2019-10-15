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
        network = GetComponent<NetworkManager>();

        if (network.setupSocket())
        {
            Debug.Log("Connecting to Server");
            network.writeSocket("test");

            Debug.Log($"Return from Server: {network.readSocket()}");
        }
        else
        {
            Debug.Log("Server is not Running");
        }
    }
}
