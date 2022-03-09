
using System.Reflection.Emit;

namespace cs_Server;

internal class Car
{
    private static int id = 0;
    public int Id { get; set; }
    public string Model { get; set; }

    public string Vendor { get; set; }

    public int Year { get; set; }

    public Car(string model, string vendor, int year)
    {
        Id=id++;
        Model=model;
        Vendor=vendor;
        Year = year;
    }
}

