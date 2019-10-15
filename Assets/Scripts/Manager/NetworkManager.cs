using System.Collections;
using System.Collections.Generic;
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

    public string readSocket() 
    {
        if (!socketReady)
            return "";
        
        if (theStream.DataAvailable)
            return theReader.ReadLine();
        
        return "";
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
        if(!theStream.CanRead) {
            setupSocket();
        }
    }
}
