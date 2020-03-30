using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

namespace RadikoNetcode
{
    class Player
    {
        private Vector3 position;
        private Vector3 rotation;
        private int id;
        private Vector3 spawn;
        private object[] customVariables;
        private int hP = 100;

        /// <summary>
        /// Create a new player with all its stuffs
        /// </summary>
        /// <param name="id"></param>
        public Player(int id)
        {
            this.id = id;
        }

        /// <summary>
        /// Sets or Gets player's position
        /// </summary>
        public Vector3 Position { get => position; set => position = value; }

        /// <summary>
        /// Get player's ID number
        /// </summary>
        public int Id { get => id;}
        public Vector3 Spawn { get => spawn; set => spawn = value; }
        public object[] CustomVariables { get => customVariables; set => customVariables = value; }
        public int HP { get => hP; set => hP = value; }
        public Vector3 Rotation { get => rotation; set => rotation = value; }

        public void ChangeInfo(int index, object value)
        {
            if(customVariables == null)
            {
                return;
            }
            if(index < customVariables.Length)
            {
                CustomVariables[index] = value;
                Console.WriteLine("mudando valor de: "+index);
            }
            else
            {
                Console.WriteLine("Erro ao colocar em: " + index+"\n" +
                    "no usuário: "+Id);
            }
        }
    }
}
