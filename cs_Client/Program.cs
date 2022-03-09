using System.Net.Sockets;
using System.Text.Json;
using cs_Server;

namespace cs_Client;

internal static class Program
{
    private static string _jsonString;
    private static BinaryWriter _writer;
    private static BinaryReader _reader;
    private static Command _command;

    static void Main()
    {
        //Thread.Sleep(5000);

        var client = new TcpClient();
        client.Connect("127.0.0.1", 45678);

        var stream = client.GetStream();

        _writer= new BinaryWriter(stream);
        _reader= new BinaryReader(stream);

        Cli();

    }

    private static void Cli()
    {
        while (true)
        {
            Console.Write(">> ");
            switch (Console.ReadLine()!.ToUpper())
            {
                case Command.GET:
                    GetCommand();
                    break;
                case Command.PUT:
                    PutCommand();
                    break;
                case Command.POST:
                    PostCommand();
                    break;
                case Command.DELETE:
                    DeleteCommand();
                    break;
                case "CLEAR":
                    Console.Clear();
                    continue;
                case "":
                    continue;
                default:
                    Console.WriteLine("Invalid command!!!");
                    continue;
            }
            _jsonString = JsonSerializer.Serialize(_command);
            _writer.Write(_jsonString);
            Console.WriteLine(_reader.ReadString());
        }
    }

    private static void GetCommand()
    {
        _command = new Command
        {
            HttpCommand = Command.GET,
            Value = null
        };
    }

    private static void PutCommand()
    {
        while (true)
        {
            try
            {
                var car = new Car();
                Console.Write("ID: >>");
                car.Id = int.Parse(Console.ReadLine() ?? throw new InvalidOperationException());
                Console.Write("Model: >>");
                car.Model = Console.ReadLine() ?? throw new InvalidOperationException();
                Console.Write("Vendor: >>");
                car.Vendor = Console.ReadLine() ?? throw new InvalidOperationException();
                Console.Write("Year: >>");
                car.Year = int.Parse(Console.ReadLine() ?? throw new InvalidOperationException());

                _command = new Command
                {
                    HttpCommand = Command.PUT,
                    Value = car
                };
                break;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

    }
    private static void PostCommand()
    {
        while (true)
        {
            try
            {
                var car = new Car();
                Console.Write("Model: >>");
                car.Model = Console.ReadLine() ?? throw new InvalidOperationException();
                Console.Write("Vendor: >>");
                car.Vendor = Console.ReadLine() ?? throw new InvalidOperationException();
                Console.Write("Year: >>");
                car.Year = int.Parse(Console.ReadLine() ?? throw new InvalidOperationException());

                _command = new Command
                {
                    HttpCommand = Command.POST,
                    Value = car
                };
                break;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
    private static void DeleteCommand()
    {
        while (true)
        {
            try
            {
                var car = new Car();
                Console.Write("ID: >>");
                car.Id = int.Parse(Console.ReadLine() ?? throw new InvalidOperationException());

                _command = new Command
                {
                    HttpCommand = Command.DELETE,
                    Value = car
                };
                break;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}