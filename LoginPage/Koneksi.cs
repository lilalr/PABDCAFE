using System;
using System.Net;
using System.Net.Sockets;

namespace PABDCAFE
{
    internal class Koneksi
    {
        public string connectionString()
        {
            string connectStr = "";
            try
            {
                string localIP = GetLocalIPAddress();
                connectStr = $"Server={localIP}; Initial Catalog=ReservasiCafe;";
                return connectStr;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error mendapatkan connection string: " + ex.Message);
                return string.Empty;
            }
        }

        private static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("Tidak ada alamat IP yang sesuai ditemukan.");
        }
    }
}