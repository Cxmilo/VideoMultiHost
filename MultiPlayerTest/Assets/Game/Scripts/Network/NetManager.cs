using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System;
using System.Text;
using System.Collections.Generic;
using UnityEngine.Networking;


public class NetManager : NetworkManager
{

    public int currentConnections;
    public int maxConnection;
    public int server_port = 5000;
    public string server_ip;
    List<string> conectionsIp = new List<string>();
    //multicast
    int startup_port = 5100;
    //no funciona con equipos wlan

    IPAddress group_address = IPAddress.Parse("224.0.0.224");
    UdpClient udp_client;
    IPEndPoint remote_end;

    private static NetManager _instance;

    public static NetManager Instance { get { return _instance; } }


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
        
    }

    // Use this for initialization
    void Start()
    {
        /*
        //load elsewhere
        if (Application.platform != RuntimePlatform.Android)
        {
            StartGameServer();
        }
        else
        {
            StartGameClient();
        }*/
    }

    public void StartGameServer()
    {
        NetManager.Instance.networkPort = server_port;
        NetManager.Instance.networkAddress = Network.player.ipAddress;
        NetManager.Instance.StartServer();
       /* var init_status = Network.InitializeServer(10, server_port, false);*/
        
        StartCoroutine(StartBroadcast());

        
    }


    public void StartGameClient()
    {


        //multicast recive setup
        remote_end = new IPEndPoint(IPAddress.Any, startup_port);
        udp_client = new UdpClient(remote_end);
        udp_client.JoinMulticastGroup(group_address);
        
        //async callback for multicast
        udp_client.BeginReceive(new AsyncCallback(ServerLookup), null);
        StartCoroutine(MakeConnection());


    }

    IEnumerator MakeConnection()
    {
        //continues after we get server Addres

        
        while (server_ip == "")
            yield return null;

        while (!IsClientConnected())
        {


            if (server_ip != "")
            {
                // DebugConsole.Log( UnityEngine.Random.Range(0,0.1f) + "-" + "Conecting:" + server_ip + ":" + server_port);
                // the unity 3d way to connect to a server
                
                

                NetManager.Instance.networkPort = server_port;
                NetManager.Instance.networkAddress = server_ip;
                NetManager.Instance.StartClient();
                
                /*NetworkConnectionError error = Network.Connect(server_ip, server_port);
                DebugConsole.Log(UnityEngine.Random.Range(1, 10) + error.ToString());*/

            }

            yield return new WaitForSeconds(10);
        }

    }


    /*broadcast functions*/

    void ServerLookup(IAsyncResult ar)
    {
        //recivers package and identifies IP
        var receiveBytes = udp_client.EndReceive(ar, ref remote_end);

        server_ip = remote_end.Address.ToString();
        


    }

    IEnumerator StartBroadcast()
    {
        //multicast send setup

        udp_client = new UdpClient();
        udp_client.JoinMulticastGroup(group_address);


        //sends multicast
        while (/*NetworkServer.connections.Count < maxConnection*/true)
        {


            String[] localIP = Network.player.ipAddress.ToString().Split('.');
            string ipBase = localIP[0] + "." + localIP[1] + "." + localIP[2] + ".";

            for (int i = 0; i < 255; i++)
            {
                string ipPing = ipBase + i;
                if (currentConnections <= maxConnection)
                {

                    if (!conectionsIp.Contains(ipPing))
                    {
                        remote_end = new IPEndPoint(IPAddress.Parse(ipPing), startup_port);
                        var buffer = Encoding.ASCII.GetBytes("GameServer");
                        udp_client.Send(buffer, buffer.Length, remote_end); 
                    }
                    //Debug.Log("Send message" + remote_end.Address.ToString());

                }
                yield return new WaitForEndOfFrame();
            }

            yield return new WaitForSeconds(1);
        }

    }

    void Update()
    {
        
        for (int i = 0; i < NetworkServer.connections.Count; i++)
        {
            if (NetworkServer.connections[i] != null)
            {
                
                if (!conectionsIp.Contains(NetworkServer.connections[i].address))
                {
                    conectionsIp.Add(NetworkServer.connections[i].address);
                }
            }
        }

        currentConnections = conectionsIp.Count;
    }

    public override void OnClientDisconnect(UnityEngine.Networking.NetworkConnection conn)
    {
        
        base.OnClientDisconnect(conn);
        
        server_ip = "";
        StartGameClient();
    }

    public override void OnClientConnect(UnityEngine.Networking.NetworkConnection conn)
    {
        base.OnClientConnect(conn);
        
    }

    public override void OnClientError(UnityEngine.Networking.NetworkConnection conn, int errorCode)
    {
        base.OnClientError(conn,errorCode);
        
    }

    
}
