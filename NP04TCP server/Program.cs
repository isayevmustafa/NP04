using NP04TCP_server;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;

var ip = IPAddress.Loopback;
var port = 27001;

var listener = new TcpListener(ip, port);
listener.Start();

while (true)
{
    var client = listener.AcceptTcpClient();
    var stream = client.GetStream();
    var br = new BinaryReader(stream);
    var bw = new BinaryWriter(stream);
    while (true)
    {
        var input = br.ReadString();
        var command = JsonSerializer.Deserialize<Command>(input);
        if (command == null) continue;
        Console.WriteLine(command.Text);
        Console.WriteLine(command.Param);

        switch (command.Text)
        {
            case Command.ProcessList:
                var process = Process.GetProcesses();
                var processNames = JsonSerializer.
                    Serialize(process.Select(p => p.ProcessName));
                bw.Write(processNames);
                break;
            case Command.RUN:

                if (command.Param != null)
                {
                    var processToRun = command.Param;
                    bw.Write(processToRun);
                }

                break;
            case Command.Kill:
                if (command.Param != null)
                {
                    var processToRun = command.Param;
                    bw.Write(processToRun);
                }
                break;
            default: break;
        }

    }
}
