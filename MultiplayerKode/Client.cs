using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Numerics;

namespace MultiplayerKode
{
    public class Client
    {
        private String address;
        private String nome;
        private int port;
        private bool online = true;
        private Player player = null;
        private IPEndPoint endPoint;
        private int id;
        private int timeOut;

        public Client(string Address, int port, string nome, int id = 0)
        {
            this.Address = Address;
            this.Port = port;
            this.Nome = nome;
            online = true;
            this.Id = id;
            Player = new Player(Id);
            this.endPoint = new IPEndPoint(IPAddress.Parse(Address), port);
            Console.WriteLine("New user created in memory");
        }
        public Client()
        {
            Console.WriteLine("—WARNING! OFFLINE CLIENT FOR TESTS—");
            Player = new Player(-1);
        }

        public int Port { get => port; set => port = value; }
        public bool Online { get => online; set => online = value; }
        public string Address { get => address; set => address = value; }
        public string Nome { get => nome; set => nome = value; }
        public int Id { get => id; set => id = value; }
        public int TimeOut { get => timeOut; set => timeOut = value; }
        internal Player Player { get => player; set => player = value; }
        public IPEndPoint EndPoint { get => endPoint;}

        /// <summary>
        /// Gets the 3D position of a player.
        /// </summary>
        /// <returns>A string with the 3D axes token at ";"</returns>
        /// 
        [Obsolete]
        public string GetPositionString()
        {
            return player.Position.X + ";" + player.Position.Y + ";" + player.Position.Z;
        }

        public byte[] GetPosition()
        {
            byte[] x = new byte[4], y = new byte[4], z = new byte[4];
            x = BitConverter.GetBytes(player.Position.X);
            y = BitConverter.GetBytes(player.Position.Y);
            z = BitConverter.GetBytes(player.Position.Z);
            byte[] complete = new byte[12];
            for (int i = 0; i < 12; i++)
            {
                if (i < 4)
                {
                    complete[i] = x[i];
                }
                if (i >= 4 && i < 8)
                {
                    complete[i] = y[i - 4];
                }
                if (i >= 8 && i < 12)
                {
                    complete[i] = z[i - 8];
                }
            }
            return complete;
        }

        /// <summary>
        /// Change position of a player.
        /// </summary>
        /// <param name="X">X axel</param>
        /// <param name="Y">Y axel</param>
        /// <param name="Z">Z axel</param>
        public void SetPosition(float X, float Y, float Z)
        {
            Vector3 temp = Player.Position;
            temp.X = X;
            temp.Y = Y;
            temp.Z = Z;
            Player.Position = temp;
        }

        /// <summary>
        /// Set the position of a player by a raw position string
        /// </summary>
        /// <param name="PositionInString">Raw position String</param>
        /// <param name="token">token that separates positions (not the same of the package)</param>
        public void SetPositionByString(string PositionInString, char token = ';')
        {
            string[] pos = PositionInString.Split(token);
            float X = float.Parse(pos[0]);
            float Y = float.Parse(pos[1]);
            float Z = float.Parse(pos[2]);
            SetPosition(X, Y, Z);
        }

        public void SetPositionByByteArray(byte[] Bytes)
        {
            byte[] x = new byte[4] {Bytes[0], Bytes[1], Bytes[2], Bytes[3] };
            byte[] y = new byte[4] { Bytes[4], Bytes[5], Bytes[6], Bytes[7] };
            byte[] z = new byte[4] { Bytes[8], Bytes[9], Bytes[10], Bytes[11] };
            float X = BitConverter.ToSingle(x);
            float Y = BitConverter.ToSingle(y);
            float Z = BitConverter.ToSingle(z);
            Console.WriteLine("x: "+X);
            Console.WriteLine("y: "+Y);
            Console.WriteLine("z: "+Z);
            SetPosition(X, Y, Z);
        }

        /// <summary>
        /// Get the custom variables that a player can have
        /// </summary>
        /// <param name="token">token to make the division between the parameters</param>
        /// <returns>a string with all the custom variables</returns>
        public string GetCustomVariables(char token = ';')
        {
            string complete = "";

            for(int i = 0; i < player.CustomVariables.Length; i++)
            {
                complete += player.CustomVariables[i];
                complete += token;
            }
            return complete;
        }

        public void SetCustomVariables(int index, object value)
        {
            Player.ChangeInfo(index, value);
        }

    }
}
