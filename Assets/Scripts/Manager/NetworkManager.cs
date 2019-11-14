using System.Collections;
using UnityEngine;
using System.IO;
using System.Net.Sockets;
using System;

public class NetworkManager : MonoBehaviour
{
    bool socketReady = false;

    TcpClient mySocket;
    NetworkStream theStream;
    StreamWriter theWriter;
    StreamReader theReader;
    Action<string> filterData;

    string HOST = "localhost";
    int PORT = 8080;

    public bool setupSocket() 
    {
        try {
            mySocket = new TcpClient(HOST, PORT);
            theStream = mySocket.GetStream();
            theWriter = new StreamWriter(theStream);
            theReader = new StreamReader(theStream);
            socketReady = true;

            return true;
        }
        catch (Exception e) {
            Debug.Log($"Socket Error: {e.ToString()}");
            return false;
        }
    }

    public void writeSocket(string theLine) 
    {
        if (!socketReady)
            return;

        string tmpString = theLine + "\r\n";
        theWriter.Write(tmpString);
        theWriter.Flush();
    }

    IEnumerator listenData()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            
            if (theStream.DataAvailable)
            {
                char[] buffer = new char[mySocket.ReceiveBufferSize];
                theReader.Read(buffer, 0, mySocket.ReceiveBufferSize);

                string result = new string(buffer);
                string[] manyResult = result.Trim().Replace("\n", "").Replace("}{", "}|{").Split('|');

                foreach (var item in manyResult)
                {
                    Debug.Log($"Receive {item} from Server");
                    filterData(item);
                }
            }
        }
    }

    public void readSocket<T>(Action<string> filterData) 
    {
        if (!socketReady)
            Debug.Log("Socket not Ready");

        this.filterData = filterData;
        StartCoroutine(listenData());
    }

    public void closeSocket() 
    {
        if (!socketReady)
            return;
        
        theWriter.Close();
        theReader.Close();
        mySocket.Close();
        socketReady = false;
    }
 
    public void maintainConnection() 
    {
        if (!theStream.CanRead) {
            setupSocket();
        }
    }
}
