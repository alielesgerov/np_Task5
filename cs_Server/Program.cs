using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace cs_Server;

static class Program
{
    private static NetworkStream? _stream;
    static List<Car> _cars = new List<Car>();
    private static Command _command;
    private static BinaryWriter _writer;
    private static BinaryReader _reader;

    static void Main()
    {
        FillCars();
        var listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 45678);
        listener.Start(10);

        var client = listener.AcceptTcpClient();
        _stream = client.GetStream();
        _writer = new BinaryWriter(_stream);
        _reader = new BinaryReader(_stream);
        FromClient();
    }

    private static void FillCars()
    {
        _cars.Add(new Car("Cherokee","Jeep",2001));
        _cars.Add(new Car("Mirage", "Mitsubishi", 1988));
        _cars.Add(new Car("W126", "Mercedes-Benz", 1981));
        _cars.Add(new Car("TSX", "Acura", 2005));
        _cars.Add(new Car("Ram Van 3500", "Dodge", 1998));
    }

    private static void FromClient()
    {

        while (true)
        {
            _command = JsonSerializer.Deserialize<Command>(_reader.ReadString())!;
            switch (_command.HttpCommand)
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
            }
        }
    }

    private static void GetCommand()
    {
        var option = new JsonSerializerOptions { WriteIndented = true };
        var data = JsonSerializer.Serialize<List<Car>>(_cars,option);
        _writer.Write(data);
    }

    private static void PutCommand()
    {
        foreach (var car in _cars.Where(car => car.Id == _command.Value.Id))
        {
            car.Model = _command.Value.Model;
            car.Vendor =_command.Value.Vendor;
            car.Year = _command.Value.Year;
            _writer.Write("True");
            break;
        }
    }

    private static void PostCommand()
    {

        _cars.Add(new Car(_command.Value.Model,_command.Value.Vendor,_command.Value.Year));

        _writer.Write("True");

    }

    private static void DeleteCommand()
    {
        foreach (var car in _cars.Where(car => car.Id == _command.Value.Id))
        {
            _cars.Remove(car);
            _writer.Write("True");
            break;
        }
    }
}