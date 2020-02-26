using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace MultiplayerKode
{
    class UDPnetKode
    {
        UdpClient server;
        IPEndPoint clientes;
        IPEndPoint broadcast;
        bool pode = true;
        public bool alive = true;
        int port;
        int IDcont = 1;
        List<Client> users = new List<Client>();
        string mensagem = "";
        System.Timers.Timer clock;
        PackageManager package = new PackageManager();
        Thread broad;


        public UDPnetKode(int port = 8484)
        {
            this.port = port;
            server = new UdpClient(port);
            clientes = new IPEndPoint(IPAddress.Any, port);
            Console.WriteLine("Started Successfully!\n" +
                "Version Alpha 0.0.1.0");
            clock = new System.Timers.Timer(1000);
            clock.Elapsed += Clock_Elapsed;
            clock.Enabled = true;
            clock.AutoReset = true;
            broad = new Thread(Geral);
            broad.Start();
            Thread messages = new Thread(Mensagens);
            messages.Start();
        }

        private void Clock_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            enviar("",true);
        }

        /// <summary>
        /// Servidor recebe mensagens dos clientes
        /// </summary>
        public void Mensagens()
        {
            while (alive)
            {
                try
                {
                    byte[] pacote = server.Receive(ref clientes);
                    string[] mensagens = package.Translate(pacote).Split('|');
                    if (mensagens[0] == "Hello")
                    {
                        sendDirect(package.GenerateMessage('|', "HANDSHAKE", IDcont, 2), clientes.Address.ToString(), clientes.Port);
                        users.Add(inserir(clientes.Address.ToString(), clientes.Port, mensagens[1],2));
                        enviar(package.GenerateMessage('|', "INFO", "Join", Procurar(clientes.Address.ToString(), clientes.Port).Id, Procurar(clientes.Address.ToString(), clientes.Port).Nome,2));
                    }
                    else if (mensagens[0] == "bye")
                    {
                        remover(false, null,clientes.Address.ToString(), clientes.Port, "Disconnected safely");
                    }
                    else if (mensagens[0] == "pong")
                    {
                        Procurar(clientes.Address.ToString(), clientes.Port).TimeOut = 0;
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
                        if (Procurar(clientes.Address.ToString(), clientes.Port) != null)
                        {
                            Client clien = Procurar(clientes.Address.ToString(), clientes.Port);
                            Console.WriteLine(clien.Nome + ": " + package.Translate(pacote));
                        }
                    }
                    else
                    {
                        if(Procurar(clientes.Address.ToString(),clientes.Port) != null)
                        {
                            Client clien = Procurar(clientes.Address.ToString(), clientes.Port);
                            Console.WriteLine(clien.Nome+": " + package.Translate(pacote)+ "(without use chat command)");
                        }
                        else
                        {
                            Console.WriteLine("Invalid Session!");
                        }
                    }  
                }
                catch (SocketException e)
                {
                    Console.WriteLine("Trying communication.");
                }
            }
        }

        /// <summary>
        /// Enviará informações de sincronização dos jogadores
        /// </summary>
        public void Geral()
        {
            while (pode)
            {
                for(int j = 0; j < users.Count; j++)
                {
                    try
                    {
                        broadcast = users[j].EndPoint;
                        for (int i = 0; i < users.Count; i++)
                        {
                            if (i != j)
                            {
                                byte[] msg;
                                msg = package.GetBytesFromMessage('|', "SYNC", users[i].GetPosition(), users[i].Id);
                                server.Send(msg, msg.Length, broadcast);
                            }
                        }
                        Thread.Sleep(100);
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
        }

        /// <summary>
        /// Envia mensagem de ping para saber se os jogadores ainda estão conectados
        /// </summary>
        /// <param name="aviso">Envia um aviso sobre essa mensagem</param>
        public void enviar(string aviso = "",bool sync = false)
        {
            if(aviso == "EXIT" || aviso == "exit" || aviso == "stop")
            {
                alive = false;
                pode = false;
                server.Close();
                clock.Enabled = false;
                clock.AutoReset = false;
            }
            for(int i = 0; i < users.Count; i++)
            {
                if (users[i].TimeOut >= 10)
                {
                    remover(false, null, users[i].Address, users[i].Port, "Timed Out");
                }
                else
                {
                    try
                    {
                        broadcast = users[i].EndPoint;
                        byte[] msg;
                        if (sync)
                        {
                            msg = package.GetBytes("ping|" + users[i].Id);
                            server.Send(msg, msg.Length, broadcast);
                            users[i].TimeOut++;
                        }
                        if (aviso.Length > 0)
                        {
                            msg = package.GetBytes(aviso);
                            server.Send(msg, msg.Length, broadcast);
                            Console.WriteLine("Sent: " + aviso);
                        }
                    }
                    catch
                    {
                        remover(false, null, users[i].Address, users[i].Port, "Problem");
                    }
                }
            }
        }

        /// <summary>
        /// Procura um usuário pelo IP
        /// </summary>
        /// <param name="Address"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public Client Procurar(string Address, int port)
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
        /// Procura um usuário pelo ID
        /// </summary>
        /// <param name="_id"></param>
        /// <returns></returns>
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
        /// Inserir um novo usuário na lista.
        /// </summary>
        /// <param name="Address">Endereço do jogador.</param>
        /// <param name="_port">Porta do jogador</param>
        /// <param name="nome">Nome do jogador</param>
        /// <param name="qt_custom">quantidade de variaveis extras</param>
        /// <returns>Retorna o usuário</returns>
        public Client inserir(string Address, int _port, string nome, int qt_custom = 2)
        {
            Client obj = new Client(Address, _port, nome, IDcont,qt_custom);
            Console.WriteLine("Welcome! " + nome + " with the ID: " + IDcont);
            IDcont++;
            return obj;
        }

        public void sendDirect(string message, string Address, int _port)
        {
            IPEndPoint EP = new IPEndPoint(IPAddress.Parse(Address), _port);
            byte[] msg = package.GetBytes(message);
            server.Send(msg, msg.Length, EP);
            Console.WriteLine("Sending HandShake");
        }

        /// <summary>
        /// Remove um usuário
        /// </summary>
        /// <param name="ip">Endereço do cliente</param>
        /// <param name="port"></param>
        /// <param name="Why"></param>
        public void remover(bool porID, Client e = null, string ip = "", int port = 8484, string Why = "")
        {
            Console.WriteLine("Removendo usuario da memoria por: " + Why);
            for (int i = 0; i < users.Count; i++)
            {
                if (porID)
                {
                    if(users[i] == e)
                    {
                        users.RemoveAt(i);
                    }
                }
                else if (users[i].Address == ip && users[i].Port == port)
                {
                    int id = users[i].Id;
                    string nome = users[i].Nome;
                    users.RemoveAt(i);
                    Console.WriteLine("Disconnected: " + nome);
                    enviar(package.GenerateMessage('|',"INFO","Left",id,nome));
                    break;
                }
            }
        }
    }
}