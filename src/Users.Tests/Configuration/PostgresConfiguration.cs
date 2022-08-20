namespace Users.Tests.Configuration;

using Users.Core.Domain.Configurations;

internal class PostgresConfiguration : IPersistenceConfiguration
{
    public const string CONTAINER_NAME = "demo_pgsql";
    public const string VOLUME_NAME = "demo_pgsql";
    public const string IMAGE_TAG = "14.5-alpine";

    public string Host { get; }
    public string Password { get; }
    public int Port { get; private set; }
    public string User { get; }
    public string DbName { get; }

    private PostgresConfiguration(string host, string password, int port, string user, string dbName)
    {
        Host = host;
        Password = password;
        Port = port;
        User = user;
        DbName = dbName;
    }

    public static PostgresConfiguration Create()
        => new(
            "localhost",
            "password",
            5432,
            "demo",
            "demo"
        );

    public string GetConnectionString()
    => @$"
            Host={Host};
            Port={Port};
            Username={User};
            Password={Password};
            Database={DbName};
            Trust Server Certificate=true;
            Server Compatibility Mode=Redshift
        ";

    public void UpdatePort(int port)
    {
        Port = port;
    }
}
