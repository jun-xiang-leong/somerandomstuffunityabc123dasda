using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEditor;
using UnityEngine;


public class UdpReceiver : MonoBehaviour
{
    private UdpClient udpClient;
    private int port = 12345; // The port you're listening on

    struct PayloadPackage
    {
        public uint UID;
        public double lon;
        public double lat;
        public string symbolCode;
        public string description;
        public string info;
    }


    void Start()
    {
        // Start receiving data on a separate thread to avoid blocking the main thread
        udpClient = new UdpClient(port);
        udpClient.BeginReceive(new AsyncCallback(ReceiveCallback), null);
        Debug.Log("Listening on UDP port " + port);
    }

    // Callback method that gets triggered when data is received
    private void ReceiveCallback(IAsyncResult result)
    {
        try
        {
            // Receive the data from the UDP socket
            IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, port);
            byte[] receivedData = udpClient.EndReceive(result, ref remoteEndPoint);

            // Convert byte array to string
            string jsonData = Encoding.UTF8.GetString(receivedData);
            Debug.Log("Received JSON: " + jsonData);

            // Parse the JSON data into a C# object (using Newtonsoft.Json library)
            //MyData myData = JsonConvert.DeserializeObject<MyData>(jsonData);
            PayloadPackage pack = JsonUtility.FromJson<PayloadPackage>(jsonData);
            
            Debug.Log("Parsed data: " + pack.UID +pack.lat +pack.lon +pack.info) ;

            // Continue receiving
            udpClient.BeginReceive(new AsyncCallback(ReceiveCallback), null);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error receiving UDP data: " + ex.Message);
        }
    }

    void OnApplicationQuit()
    {
        udpClient.Close();
    }
}
