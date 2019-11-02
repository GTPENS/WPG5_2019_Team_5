using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject sliderObject;
    public GameObject bidText;
    public GameObject bidButton;

    NetworkManager network;
    bool isServerOn;
    int playerId;

    void Start()
    {
        bidButton.GetComponent<Button>().onClick.AddListener(onClickBid);

        onJoinGame();
    }

    void onJoinGame()
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

            string result = network.readSocket();
            Debug.Log($"Receive {result} from Server");

            Data dataResult = JsonUtility.FromJson<Data>(result);
            playerId = dataResult.playerId;
        }
        else
        {
            Debug.Log("Server is not Running");
        }
    }

    void onClickBid()
    {
        Data data = new Data("bid");
        data.playerId = playerId;
        data.bidValue = 2000;

        string jsonString = JsonUtility.ToJson(data);

        network.writeSocket(jsonString);
        string result = network.readSocket();

        Data dataResult = JsonUtility.FromJson<Data>(result);
        Debug.Log($"data: {dataResult.command}");
    }
}
