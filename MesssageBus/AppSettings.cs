using System.IO;
using Microsoft.Extensions.Configuration;

public class AppSettings: IAppSettings
{
    private static IConfiguration Configuration { get; set; }
    public AppSettings()
	{
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
        Configuration = builder.Build();
    }
    public string GetHostName()
    {
       return Configuration["hostname"];
    }
    public string GetUsername()
    {
        return Configuration["username"];
    }
    public string GetPassword()
    {
        return Configuration["password"];
    }
    public string GetSentMessageKey()
    {
       return Configuration["sentmessagekey"];
    }
    public string GetReceivedMessageKey()
    {
        return Configuration["responsemessagekey"];
    }
}
