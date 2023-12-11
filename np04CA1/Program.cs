using np04CA1;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using System.Threading.Channels;

var ip = IPAddress.Loopback;
var port = 27001;

var client = new TcpClient();
client.Connect(ip, port);

var stream = client.GetStream();
var br = new BinaryReader(stream);
var bw = new BinaryWriter(stream);

Command command = null;
string responce = null;
string str = null;
while (true)
{
    Console.WriteLine("Write command for HELP:");
    str = Console.ReadLine();
    if (str == "HELP" || str == "help" || str == "Help")
    {
        Console.WriteLine();
        Console.WriteLine("Command lsit: ");
        Console.WriteLine(Command.ProcessList);
        Console.WriteLine($"{Command.Kill} <Process name>");
        Console.WriteLine($"{Command.RUN} <Process name>");
        Console.WriteLine("HELP");
        Console.ReadLine();
        Console.Clear();
    }

    var input = str.Split(' ');

    switch (input[0].ToUpper())
    {
        case Command.ProcessList:
            command = new Command() { Text = input[0].ToUpper() };
            bw.Write(JsonSerializer.Serialize(command));
            responce = br.ReadString();
            var processList = JsonSerializer.Deserialize<string[]>(responce);
            foreach (var processName in processList)
            {
                Console.WriteLine($"    {processName}");
            }
            break;

        case Command.RUN:
            command = new Command() { Text = input[0].ToUpper(), Param = input[1] };
            bw.Write(JsonSerializer.Serialize(command));
            responce = br.ReadString();
            Process.Start(responce);
            break;

        case Command.Kill:
            command = new Command() { Text = input[0].ToUpper(), Param = input[1] };
            bw.Write(JsonSerializer.Serialize(command));
            responce = br.ReadString();
            var process = Process.GetProcesses().FirstOrDefault(p => p.ProcessName == responce);

            if (process != null)
                Process.GetProcessById(process.Id).Kill();

            else Console.WriteLine("Process did not found");
            break;

        default:
            break;
    }
    Console.WriteLine("Press any key to continue");
    Console.ReadKey();
    Console.Clear();
}

