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
        public string GetPosition()
        {
            return player.Position.X + ";" + player.Position.Y + ";" + player.Position.Z;
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
