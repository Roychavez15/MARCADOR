namespace Marcador.Core;

public static class Config
{
    public static string DbPath => Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "MarcadorLBG", "marcador.db");

    public static string ApiBaseUrl { get; set; } = "http://181.39.104.93:5028";

    public static int SignalRPort { get; set; } = 58273;

    public static string SignalRUrl => $"http://localhost:{SignalRPort}/marcador";
}
