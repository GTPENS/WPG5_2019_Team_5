using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Net.Sockets;
using System;
using System.Text;
using System.Threading;

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
            return "notready";

        Thread.Sleep(100);
        
        if (theStream.DataAvailable)
        {
            char[] buffer = new char[mySocket.ReceiveBufferSize];
            theReader.Read(buffer, 0, mySocket.ReceiveBufferSize);
        
            return new string(buffer);
        }
        
        return "notavailable";
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
