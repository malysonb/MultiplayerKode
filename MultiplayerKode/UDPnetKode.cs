using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

/*
    Server based multiplayer mode, 
    Can be used on Massive Multiplayer games 

    Alert: It's not cheatproof yet.
*/

namespace RadikoNetcode
{
    class UDPnetKode
    {
        private UdpClient server;
        private IPEndPoint clientes, broadcast;
        public bool alive = true;
        private int port = 8484;
        private int IDcont = 1;
        private List<Client> users = new List<Client>();
        private System.Timers.Timer clock, sync_time;
        private Thread broad;

        /// <summary>
        /// Udp server module for Radiko Multiplayer Netcode.
        /// </summary>
        /// <param name="port">Port for listening</param>
        public UDPnetKode(int port)
        {
            this.port = port;
            server = new UdpClient(port);
            clientes = new IPEndPoint(IPAddress.Any, port);
            Console.WriteLine("Started Successfully!\n" +
                "Version Alpha 0.2.0\n" +
                "Listening at port "+port);
            clock = new System.Timers.Timer(2000);
            clock.Elapsed += Clock_Elapsed;
            clock.Enabled = true;
            clock.AutoReset = true;
            sync_time = new System.Timers.Timer(200);
            sync_time.Elapsed += Sync_time_Elapsed;
            sync_time.Enabled = true;
            sync_time.AutoReset = true;
            //broad = new Thread(Sync);
            //broad.Start();
            Thread messages = new Thread(RecvMessage);
            messages.Start();
        }

        private void Sync_time_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //throw new NotImplementedException();
            Sync();
        }

        /// <summary>
        /// Clock to send ping signal to all players
        /// </summary>
        private void Clock_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //send("",true);
            Broadcast(null, true);
        }

        /// <summary>
        /// Where you receive your messages
        /// </summary>
        /// 
        [Obsolete("Not working anymore, still here just to help you to understand the code\nWill be removed in the future, a new one will substitute this\n" +
            "Use RecvMessage Instead")]
        public void Messages()
        {
            while (alive)
            {
                try
                {
                    byte[] pacote = server.Receive(ref clientes);
                    string[] mensagens = PkgMngr.Translate(pacote).Split('|');
                    if (mensagens[0] == "Hello")
                    {
                        //sendDirect(package.GenerateMessage('|', "HANDSHAKE", IDcont), clientes.Address.ToString(), clientes.Port);
                        users.Add(inserir(clientes.Address.ToString(), clientes.Port, mensagens[1]));
                        //send(package.GenerateMessage('|', "INFO", "Join", SearchByAddres(clientes.Address.ToString(), clientes.Port).Id, SearchByAddres(clientes.Address.ToString(), clientes.Port).Nome,2));
                    }
                    else if (mensagens[0] == "bye")
                    {
                        remove(clientes.Address.ToString(), clientes.Port, "Disconnected safely");
                    }
                    else if (mensagens[0] == "pong")
                    {
                        SearchByAddres(clientes.Address.ToString(), clientes.Port).TimeOut = 0;
                    }
                    else if( mensagens[0] == "SYNC")
                    {
                        if (ProcurarPorID(Int32.Parse(mensagens[2])) != null)
                        {
                            Client cliente = ProcurarPorID(Int32.Parse(mensagens[2]));
                            Player jogador = cliente.Player;
                            cliente.SetPositionByString(mensagens[1]);
                        }
                    }
                    else if(mensagens[0] == "CHAT")
                    {
                        if (SearchByAddres(clientes.Address.ToString(), clientes.Port) != null)
                        {
                            Client clien = SearchByAddres(clientes.Address.ToString(), clientes.Port);
                            Console.WriteLine(clien.Nome + ": " + PkgMngr.Translate(pacote));
                        }
                    }
                    else
                    {
                        if(SearchByAddres(clientes.Address.ToString(),clientes.Port) != null)
                        {
                            Client clien = SearchByAddres(clientes.Address.ToString(), clientes.Port);
                            Console.WriteLine(clien.Nome+": " + PkgMngr.Translate(pacote)+ "(without use chat command)");
                        }
                        else
                        {
                            Console.WriteLine("Invalid Session!");
                            sendDirect("PleaseConnect", clientes.Address.ToString(), clientes.Port);
                        }
                    }  
                }
                catch (SocketException e)
                {
                    Console.WriteLine("Trying communication. error: " + e.Message);
                }
            }
        }

        /// <summary>
        /// Server receives messages from the clients
        /// </summary>
        public void RecvMessage()
        {
            while (alive)
            {
                try
                {
                    byte[] Pkg = server.Receive(ref clientes);
                    switch (Pkg[0])
                    {
                        case IPkgInterf.IHeader.HELLO:
                            if (SearchByAddres(clientes.Address.ToString(), clientes.Port) == null)
                            {
                                byte[] idtosend = new byte[4];
                                idtosend = BitConverter.GetBytes(IDcont);
                                sendDirect(IPkgInterf.IHeader.HANDSHAKE, idtosend, clientes.Address, clientes.Port);
                                inserir(clientes.Address.ToString(), clientes.Port, PkgMngr.Translate(PkgMngr.TrimByteArray(1, Pkg.Length, Pkg)));
                            }
                            break;
                        case IPkgInterf.IHeader.GOODBYE:
                            if (SearchByAddres(clientes.Address.ToString(), clientes.Port) != null)
                            {
                                Client temp = SearchByAddres(clientes.Address.ToString(), clientes.Port);
                                remove(temp.Id, "Disconnected Safely");
                            }
                            else
                                sendDirect(IPkgInterf.IHeader.ERROR,clientes.Address,clientes.Port);
                            break;
                        case IPkgInterf.IHeader.PING:
                            if(SearchByAddres(clientes.Address.ToString(), clientes.Port) != null)
                            {
                                SearchByAddres(clientes.Address.ToString(), clientes.Port).TimeOut = 0;
                            }
                            else
                                sendDirect(IPkgInterf.IHeader.ERROR, clientes.Address, clientes.Port);
                            break;
                        case IPkgInterf.IHeader.SYNC:
                            if(SearchByAddres(clientes.Address.ToString(),clientes.Port) != null)
                            {
                                /*1-XXXX-YYYY-ZZZZ*/
                                Client temp = SearchByAddres(clientes.Address.ToString(), clientes.Port);
                                temp.SetPositionByByteArray(PkgMngr.TrimByteArray(1,13,Pkg));
                            }
                            else
                                sendDirect(IPkgInterf.IHeader.ERROR, clientes.Address, clientes.Port);
                            break;
                        case IPkgInterf.IHeader.INPUT:
                            /*It will send to all players the input to of an player
                             * but syncing his position and rotation before all thing*/
                            if (SearchByAddres(clientes.Address.ToString(), clientes.Port) != null)
                            {
                                /*3-XXXX-YYYY-ZZZZ-xxxx-yyyy-zzzz-I-A-IDID*/
                                Client temp = SearchByAddres(clientes.Address.ToString(), clientes.Port);
                                temp.SetPositionByByteArray(PkgMngr.TrimByteArray(1, 13, Pkg));
                                temp.SetRotationByByteArray(PkgMngr.TrimByteArray(13, 25, Pkg));
                                Broadcast(Pkg,false);
                            }
                            else
                                sendDirect(IPkgInterf.IHeader.ERROR, clientes.Address, clientes.Port);
                            break;
                        case IPkgInterf.IHeader.CHAT:
                            //TODO: CHAT PROTOCOL
                            break;
                        default:
                            if (SearchByAddres(clientes.Address.ToString(), clientes.Port) != null)
                            {
                                Client clien = SearchByAddres(clientes.Address.ToString(), clientes.Port);
                                Console.WriteLine(clien.Nome + ": " + PkgMngr.Translate(Pkg) + " (Using an unkown header)");
                            }
                            else
                            {
                                Console.WriteLine("Invalid Session!");
                                sendDirect(IPkgInterf.IHeader.ERROR, clientes.Address, clientes.Port);
                            }
                            break;
                    }
                }
                catch (SocketException e)
                {
                    Console.WriteLine("Trying Communication. error: " + e.Message);
                }
            }
        }

        /// <summary>
        /// Sync player's position
        /// </summary>
        public void Sync()
        {
            //HACK: Temporary.
            for (int j = 0; j < users.Count; j++)
            {
                try
                {
                    broadcast = users[j].EndPoint;
                    for (int i = 0; i < users.Count; i++)
                    {
                        if (i != j)
                        {
                            byte[] msg = PkgMngr.GenerateMessage(IPkgInterf.IHeader.SYNC, users[i].GetPosition(), users[i].GetID());
                            Console.WriteLine("Sending: " + msg.Length + " Bits.");
                            server.Send(msg, msg.Length, broadcast);
                        }
                    }
                    Thread.Sleep(100);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Something happened: " + e.Message);
                    continue;
                }
            }
            if (users.Count == 0)
            {
                Thread.Sleep(50);
            }
        }

        /// <summary>
        /// Send a ping signal to keep connection alive.
        /// </summary>
        /// <param name="advert">Send a custom message</param>
        /// 
        [Obsolete("Will be removed in the future, use Broadcast instead")]
        public void send(string advert = "",bool sync = false)
        {
            if(advert == "EXIT" || advert == "exit" || advert == "stop")
            {
                alive = false;
                server.Close();
                clock.Enabled = false;
                clock.AutoReset = false;
            }
            for(int i = 0; i < users.Count; i++)
            {
                if (users[i].TimeOut >= 10)
                {
                    remove(users[i].Address, users[i].Port, "Timed Out");
                }
                else
                {
                    try
                    {
                        broadcast = users[i].EndPoint;
                        byte[] msg;
                        if (sync)
                        {
                            msg = PkgMngr.GetBytes("ping|" + users[i].Id);
                            server.Send(msg, msg.Length, broadcast);
                            users[i].TimeOut++;
                        }
                        if (advert.Length > 0)
                        {
                            msg = PkgMngr.GetBytes(advert);
                            server.Send(msg, msg.Length, broadcast);
                            Console.WriteLine("Sent: " + advert);
                        }
                    }
                    catch (SocketException e)
                    {
                        remove(users[i].Address, users[i].Port, "Problem: "+e);
                    }
                }
            }
        }

        /// <summary>
        /// Broadcast messages to all connected clients.
        /// </summary>
        /// <param name="Custom">Custom bytearray to send to the clients.</param>
        /// <param name="sync">if it is true, will sync players position.</param>
        public void Broadcast(byte[] Custom, bool sync)
        {
            //TODO: FRONTEND TO THE SERVER.
            for (int i = 0; i < users.Count; i++)
            {
                if (users[i].TimeOut >= 10)
                {
                    remove(users[i].Id, "Timed Out");
                }
                else
                {
                    try
                    {
                        broadcast = users[i].EndPoint;
                        byte[] msg;
                        if (sync)
                        {
                            msg = PkgMngr.GenerateMessage(IPkgInterf.IHeader.PING,users[i].GetID());
                            server.Send(msg, msg.Length, broadcast);
                            users[i].TimeOut++;
                        }
                        if (Custom != null)
                        {
                            server.Send(Custom, Custom.Length, broadcast);
                            Console.WriteLine("Sent: " + PkgMngr.Translate(Custom));
                        }
                    }
                    catch (SocketException e)
                    {
                        remove(users[i].Address, users[i].Port, "Problem: "+e);
                    }
                }
            }
        }

        /// <summary>
        /// Search for a user using his address
        /// </summary>
        /// <param name="Address"></param>
        /// <param name="port"></param>
        /// <returns>Client object</returns>
        public Client SearchByAddres(string Address, int port)
        {
            for (int i = 0; i < users.Count; i++)
            {
                if (users[i].Address == Address && users[i].Port == port)
                {
                    return users[i];
                }
            }
            return null;
        }

        /// <summary>
        /// Search a user by an ID
        /// </summary>
        /// <param name="_id"></param>
        /// <returns>Client object</returns>
        public Client ProcurarPorID(int _id)
        {
            for(int i = 0; i < users.Count; i++)
            {
                if (users[i].Id == _id)
                {
                    return users[i];
                }
            }
            return null;
        }

        /// <summary>
        /// Insert a new client to the list
        /// </summary>
        /// <param name="Address">Client's IP Address.</param>
        /// <param name="_port">Client's port</param>
        /// <param name="nome">Client's name</param>
        /// <param name="qt_custom">Extra sync vars</param>
        /// <returns>Client object</returns>
        public Client inserir(string Address, int _port, string nome)
        {
            Client obj = new Client(Address, _port, nome, IDcont);
            Console.WriteLine("Welcome! " + nome + " with the ID: " + IDcont);
            users.Add(obj);
            IDcont++;
            return obj;
        }

        /// <summary>
        /// Send a message directly to an Address
        /// </summary>
        /// <param name="message">String to send to a client</param>
        /// <param name="Address">client's Address</param>
        /// <param name="_port"></param>
        ///
        [Obsolete("Will be removed in the future. use sendDirect(byte signal) Instead")]
        public void sendDirect(string message, string Address, int _port)
        {
            IPEndPoint EP = new IPEndPoint(IPAddress.Parse(Address), _port);
            byte[] msg = PkgMngr.GetBytes(message);
            server.Send(msg, msg.Length, EP);
            Console.WriteLine("Sending HandShake");
        }

        /// <summary>
        /// Send a message directly to a address
        /// </summary>
        /// <param name="signal">Signal token to an server command.</param>
        /// <param name="message">Byte array to be sent to the server.</param>
        /// <param name="Address">IP Address of the client.</param>
        /// <param name="_port">Port of the server.</param>
        public void sendDirect(byte signal, byte[] message, IPAddress Address, int _port)
        {
            IPEndPoint EP = new IPEndPoint(Address, _port);
            byte[] msg = PkgMngr.GenerateMessage(signal, message);
            server.Send(msg, msg.Length, EP);
            Console.WriteLine("Sending Handshake");
        }
        /// <summary>
        /// Send a message directly to an ID
        /// </summary>
        /// <param name="signal">Signal token to an server command.</param>
        /// <param name="message">Byte array to be sent to the server.</param>
        /// <param name="ID">ID of the user</param>
        public void sendDirect(byte signal, byte[] message, int ID = -1)
        {

            IPEndPoint EP = ProcurarPorID(ID).EndPoint;
            byte[] msg = PkgMngr.GenerateMessage(signal, message);
            server.Send(msg, msg.Length, EP);
        }

        /// <summary>
        /// Send just the signal to an Address. <br/>(idk if it can be useful)
        /// </summary>
        /// <param name="signal">Signal token to an server command.</param>
        /// <param name="Address">IP Address of the client.</param>
        /// <param name="_port">Port of the server.</param>
        public void sendDirect(byte signal, IPAddress Address, int _port)
        {

            IPEndPoint EP = new IPEndPoint(Address, _port);
            byte[] msg = new byte[1];
            msg[0] = signal;
            server.Send(msg, msg.Length, EP);
        }

        /// <summary>
        /// removes a client from the list
        /// </summary>
        /// <param name="ip">Client's IP Address</param>
        /// <param name="port">Client's port</param>
        /// <param name="Why">Reason why it is going to be removed</param>
        public void remove(string ip = "", int port = 8484, string Why = "")
        {
            Console.WriteLine("Removing a client by: " + Why);
            for (int i = 0; i < users.Count; i++)
            {
                if (users[i].Address == ip && users[i].Port == port)
                {
                    int id = users[i].Id;
                    string nome = users[i].Nome;
                    users.RemoveAt(i);
                    Console.WriteLine("Disconnected: " + nome);
                    Broadcast(PkgMngr.GenerateMessage(IPkgInterf.IHeader.GOODBYE, BitConverter.GetBytes(id)),false);
                    break;
                }
            }
        }

        /// <summary>
        /// removes a client from the list
        /// </summary>
        /// <param name="_ID">Client ID</param>
        /// <param name="Why">Reason why it is going to be removed</param>
        public void remove(int _ID = -1, string Why = "")
        {
            Console.WriteLine("Removing a client by: " + Why);
            for (int i = 0; i < users.Count; i++)
            {
                if (users[i].Id == _ID)
                {
                    users.RemoveAt(i);
                }
            }
        }

    }
}