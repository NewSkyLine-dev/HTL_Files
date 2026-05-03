using LinqToDB;
using Network;
using Server;
using System.Net;
using System.Net.Sockets;

MyDb db = new("Data Source=at.db");

TcpListener listener = new(IPAddress.Any, 12345);
listener.Start();

while (true)
{
    TcpClient client = listener.AcceptTcpClient();

    Task.Run(() =>
    {
        Transfer<MSG> transfer = new(client);
        transfer.OnMessageReceived += ReceivedFunction;
    });

    try
    {
        while (client.Connected)
            Thread.Sleep(1000);
    } catch (Exception ex)
    {
        client.Close();
    }
}

void ReceivedFunction(object? sender, MSG e)
{
    var cites = db.Cities.LoadWith(c => c.CityInfos).Where(c => c.City1.Contains(e.SearchText));

    ((Transfer<MSG>)sender).Send(new() { Cities = cites.ToList() });
}