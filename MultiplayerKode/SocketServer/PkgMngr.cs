using System;
using System.Text;

namespace RadikoNetcode
{
    /// <summary>
    /// PACKAGE MANAGER MODULE FOR RADIKO MULTIPLAYER NETCODE SERVER<br/>
    /// VERSION 3.2
    /// </summary>
    /// 
    static class PkgMngr
    {
        /// <summary>
        /// Will generate an ready message to send
        /// </summary>
        /// <param name="signal">Its a token to recognize the command</param>
        /// <param name="args">Array of array of bytes</param>
        /// <returns>An array of bytes.</returns>
        public static byte[] GenerateMessage(byte signal, params byte[][] args)
        {
            int length = 0;
            for (int i = 0; i < args.Length; i++)
            {
                length += args[i].Length;
            }
            byte[] Package = new byte[length + 1];
            Package[0] = signal;
            int index = 1;
            for (int i = 0; i < args.Length; i++)
            {
                for (int j = 0; j < args[i].Length; j++)
                {
                    Package[index] = args[i][j];
                    index++;
                }
            }
            return Package;
        }

        public static byte[] Union(params byte[][] args)
        {
            int length = 0;
            for (int i = 0; i < args.Length; i++)
            {
                length += args[i].Length;
            }
            byte[] Package = new byte[length];
            int index = 0;
            for (int i = 0; i < args.Length; i++)
            {
                for (int j = 0; j < args[i].Length; j++)
                {
                    Package[index] = args[i][j];
                    index++;
                }
            }
            return Package;
        }

        /// <summary>
        /// Translates a byte array to an ASCII string.
        /// </summary>
        /// <param name="bytearray">Byte array</param>
        /// <returns>A string</returns>
        public static string Translate(byte[] bytearray)
        {
            return Encoding.ASCII.GetString(bytearray);
        }

        /// <summary>
        /// Converts a string to a byte array
        /// </summary>
        /// <param name="message">string to be translated to a byte array</param>
        /// <returns>byte array</returns>
        public static byte[] GetBytes(string _string)
        {
            return Encoding.ASCII.GetBytes(_string);
        }

        public static byte GetBytes(char _char)
        {
            string a = "";
            a += _char;
            byte result = byte.Parse(a);
            return result;
        }

        /// <summary>
        /// Translates an array of bytes to an array of strings.
        /// </summary>
        /// <param name="array">Byte array</param>
        /// <param name="token">Token to separate the message</param>
        /// <returns>An array of strings</returns>
        public static string[] Translate(byte[] array, char token = '|')
        {
            return Encoding.ASCII.GetString(array).Split(token);
        }

        /// <summary>
        /// Trim an array of bytes inside an array of bytes
        /// </summary>
        /// <param name="start">index of the first byte</param>
        /// <param name="end">index of the last byte (will not be added to the returned array)</param>
        /// <param name="byteArray">byte array to be trimmed</param>
        /// <returns>A trimmed byte array</returns>
        public static byte[] TrimByteArray(int start, int end, byte[] byteArray)
        {
            byte[] result = new byte[end - start];
            int index = 0;
            for(int i = start; i < end; i++)
            {
                result[index] = byteArray[i];
                index++;
            }
            return result;
        }
    }
}
