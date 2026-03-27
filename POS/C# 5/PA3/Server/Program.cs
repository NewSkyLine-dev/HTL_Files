using Server;
using Server.Models;
using Server.Network;
using System.Net;
using System.Net.Sockets;

Database db = new("Data Source=worldcities.sqlite");

TcpListener listener = new(IPAddress.Any, 12345);
listener.Start();

while (true)
{
    TcpClient client = listener.AcceptTcpClient();
    Task.Run(() =>
    {
        Transfer<MSG> transfer = new(client);
        transfer.OnMessageReceived += ReceivedFunction;

        try
        {
            while (client.Connected)
            {
                Thread.Sleep(100);
            }
        }
        finally
        {
            client.Close();
        }
    }); 
}

async void ReceivedFunction(object? sender, MSG e)
{
    var cities = db.Worldcities.Where(c => c.City.Contains(e.Name)).ToList();

    MSG antwort = new() { Name = "", Cities = cities };
    ((Transfer<MSG>)sender!).Send(antwort);
}