using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

namespace RadikoNetcode
{
    class Player
    {
        private Vector3 position;
        private int id;
        private Vector3 spawn;
        private object[] customVariables;
        private int hP = 100;
        private List<CustomVars> inventory;

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
        public List<CustomVars> Inventory { get => inventory; set => inventory = value; }
        public int HP { get => hP; set => hP = value; }

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

        public void addItem(int itemID, int quantity = 1, object value = null, string name = "")
        {
            bool existe = false;
            foreach (CustomVars item in inventory)
            {
                if(item.ID == itemID)
                {
                    existe = false;
                    item.Add(quantity);
                }
            }
            if(existe == false)
            {
                CustomVars temp = new CustomVars(itemID, quantity, value, name);
                inventory.Add(temp);
            }
        }
    }

    public class CustomVars
    {
        private int iD;
        private string name;
        private object value;
        private int quantity = 1;

        public int ID { get => iD; set => iD = value; }
        public string Name { get => name; set => name = value; }
        public object Value { get => value; set => this.value = value; }
        public int Quantity { get => quantity; set => quantity = value; }

        /// <summary>
        /// Item for inventory.
        /// </summary>
        /// <param name="ID">Identity of the item</param>
        /// <param name="name">Name of the item</param>
        /// <param name="value">Value represented by the item, it can be anything</param>
        /// <param name="quantity">Quantity to be added to an object</param>
        public CustomVars(int ID, int quantity = 1, object value = null, string name = "")
        {
            iD = ID;
            this.value = value;
            this.name = name.Length == 0 ? ("Item N°" + ID) : name;
            this.quantity = quantity;
        }

        public void Add(int quantity = 1)
        {
            this.quantity += quantity;
        }

        public bool Sub(int quantity = 1)
        {
            if(quantity >= this.quantity)
            {
                return false;
            }
            else
            {
                this.quantity -= quantity;
                return true;
            }
        }
    }
}
