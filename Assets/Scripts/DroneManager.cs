using UnityEngine;
using System.Net.Sockets;
using System;
using System.Net;
using System.Text;

public class DroneManager : MonoBehaviour
{
    UdpClient udpClient;
    bool isCommandMode;

    public void Start()
    {
        isCommandMode = false;
        InitiateUDPClient();
    }

    void InitiateUDPClient()
    {
        try
        {
            udpClient = new UdpClient(8889);
            udpClient.Connect("192.168.10.1", 8889);
            //Invoke("CloseConnection", 5f);
        }
        catch (Exception e)
        {
            Debug.LogError("Error connecting to drone :: " + e.Message);
        }
    }

    private void CloseConnection()
    {
        if (udpClient.Available > 0)
        {
            Debug.LogError("UDP Client available closing connection");
            return;
        }
        else
        {
            Debug.LogError("No UDP Client available");
            udpClient.Close();
        }
    }

    public void ConnectToDrone()
    {
        Debug.Log("DM : Intiating connection to Drona...");
        // Sends a message to the host to which you have connected.
        //Byte[] sendBytes = Encoding.ASCII.GetBytes("command");
        Byte[] command = new byte[] { 0x63, 0x6f, 0x6d, 0x6d, 0x61, 0x6e, 0x64 };
        udpClient.Send(command, command.Length);

        //// Sends a message to a different host using optional hostname and port parameters.
        //UdpClient udpClientB = new UdpClient();
        //udpClientB.Send(sendBytes, sendBytes.Length, "AlternateHostMachineName", 11000);

        //IPEndPoint object will allow us to read datagrams sent from any source.
        IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);

        // Blocks until a message returns on this socket from a remote host.
        Byte[] receiveBytes = udpClient.Receive(ref RemoteIpEndPoint);
        string returnData = Encoding.ASCII.GetString(receiveBytes);

        // Uses the IPEndPoint object to determine which of these two hosts responded.
        Debug.Log("This is the message you received " +
                                     returnData.ToString());
        Debug.Log("This message was sent from " +
                                   RemoteIpEndPoint.Address.ToString() +
                                   " on their port number " +
                                   RemoteIpEndPoint.Port.ToString());
        isCommandMode = true;
    }


    public void TakeOff()
    {
        if (isCommandMode)
        {
            Debug.Log("DM : Commanding Drona to take off ...");
            // takeoff 
            Byte[] takeoff = new byte[] { 0x74, 0x61, 0x6b, 0x65, 0x6f, 0x66, 0x66 };
            udpClient.Send(takeoff, takeoff.Length);

            //// Sends a message to a different host using optional hostname and port parameters.
            //UdpClient udpClientB = new UdpClient();
            //udpClientB.Send(sendBytes, sendBytes.Length, "AlternateHostMachineName", 11000);

            //IPEndPoint object will allow us to read datagrams sent from any source.
            IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);

            // Blocks until a message returns on this socket from a remote host.
            Byte[] receiveBytes = udpClient.Receive(ref RemoteIpEndPoint);
            string returnData = Encoding.ASCII.GetString(receiveBytes);

            // Uses the IPEndPoint object to determine which of these two hosts responded.
            Debug.Log("This is the message you received " +
                                         returnData.ToString());
            Debug.Log("This message was sent from " +
                                       RemoteIpEndPoint.Address.ToString() +
                                       " on their port number " +
                                     RemoteIpEndPoint.Port.ToString());

            if (returnData == "error")
            {
                Invoke("CheckBattery", 0.5f);
            }
        }
    }

    public void Land()
    {
        if (isCommandMode)
        {
            Debug.Log("DM : Initiating land sequence...");
            Byte[] land = new byte[] { 0x6c, 0x61, 0x6e, 0x64 };
            udpClient.Send(land, land.Length);
            //// Sends a message to a different host using optional hostname and port parameters.
            //UdpClient udpClientB = new UdpClient();
            //udpClientB.Send(sendBytes, sendBytes.Length, "AlternateHostMachineName", 11000);

            //IPEndPoint object will allow us to read datagrams sent from any source.
            IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);

            // Blocks until a message returns on this socket from a remote host.
            Byte[] receiveBytes = udpClient.Receive(ref RemoteIpEndPoint);
            string returnData = Encoding.ASCII.GetString(receiveBytes);

            // Uses the IPEndPoint object to determine which of these two hosts responded.
            Debug.Log("This is the message you received " +
                                         returnData.ToString());
            Debug.Log("This message was sent from " +
                                       RemoteIpEndPoint.Address.ToString() +
                                       " on their port number " +
                                       RemoteIpEndPoint.Port.ToString());

            udpClient.Close();
        }
    }

    void CheckBattery()
    {
        Debug.Log("DM : Initiating land sequence...");
        Byte[] batteryInfo = new byte[] { 0x62, 0x61, 0x74, 0x74, 0x65, 0x72, 0x79, 0x3f };
        udpClient.Send(batteryInfo, batteryInfo.Length);
        //// Sends a message to a different host using optional hostname and port parameters.
        //UdpClient udpClientB = new UdpClient();
        //udpClientB.Send(sendBytes, sendBytes.Length, "AlternateHostMachineName", 11000);

        //IPEndPoint object will allow us to read datagrams sent from any source.
        IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);

        // Blocks until a message returns on this socket from a remote host.
        Byte[] receiveBytes = udpClient.Receive(ref RemoteIpEndPoint);
        string returnData = Encoding.ASCII.GetString(receiveBytes);

        // Uses the IPEndPoint object to determine which of these two hosts responded.
        Debug.Log("This is the message you received " +
                                     returnData.ToString());
        Debug.Log("This message was sent from " +
                                   RemoteIpEndPoint.Address.ToString() +
                                   " on their port number " +
                                   RemoteIpEndPoint.Port.ToString());
        Debug.Log("DM : Drona battery level is " + returnData);
        if(int.Parse(returnData) < 10)
        {
            Debug.Log("DM : Closing connection as Drona battery is" + returnData);
            udpClient.Close();
        }
    }
}
