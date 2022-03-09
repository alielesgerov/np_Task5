
namespace cs_Server;

internal class Command
{
    public const string GET = "GET";
    public const string POST = "POST";
    public const string PUT = "PUT";
    public const string DELETE = "DELETE";

    public string HttpCommand { get; set; }
    public Car Value { get; set; }
}

