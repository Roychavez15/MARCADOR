using System.ComponentModel;
using System.Drawing;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Marcador.Core;

/// <summary>Posición y tipografía del marcador (256×236 por defecto). Se serializa en SQLite.</summary>
public sealed class MarcadorLayoutSnapshot
{
    [Category("Ventana")]
    [Description("Ancho del área cliente del formulario marcador")]
    public int ClientWidth { get; set; } = 256;

    [Category("Ventana")]
    [Description("Alto del área cliente")]
    public int ClientHeight { get; set; } = 236;

    [Category("Ventana")]
    [Description("Posición en pantalla del Display (marcador): borde izquierdo. Solo editable desde Admin / diseño guardado.")]
    public int DisplayLeft { get; set; }

    [Category("Ventana")]
    [Description("Posición en pantalla del Display: borde superior.")]
    public int DisplayTop { get; set; }

    [Category("Cabecera")]
    [Description("Si es true, título y etapa se apilan desde Y=0 solo cuando tienen texto (ignora Y fijos de cada uno)")]
    public bool Cabecera_ApilarSiVacio { get; set; }

    [Category("Título liga")]
    public int TituloLiga_X { get; set; }
    [Category("Título liga")] public int TituloLiga_Y { get; set; } = 1;
    [Category("Título liga")] public int TituloLiga_W { get; set; } = 256;
    [Category("Título liga")] public int TituloLiga_H { get; set; } = 29;
    [Category("Título liga")] public float TituloLiga_FontPt { get; set; } = 11f;
    [Category("Título liga")] public bool TituloLiga_Bold { get; set; } = true;
    [Category("Título liga")] public string TituloLiga_FontFamily { get; set; } = "Microsoft Sans Serif";
    [Category("Título liga")]
    [Description("Color de texto")]
    public Color TituloLiga_Color { get; set; } = Color.White;

    [Category("Etapa (marquee)")]
    public int Etapa_X { get; set; }
    [Category("Etapa (marquee)")] public int Etapa_Y { get; set; } = 32;
    [Category("Etapa (marquee)")] public int Etapa_W { get; set; } = 256;
    [Category("Etapa (marquee)")] public int Etapa_H { get; set; } = 23;
    [Category("Etapa (marquee)")] public float Etapa_FontPt { get; set; } = 11f;
    [Category("Etapa (marquee)")] public bool Etapa_Bold { get; set; } = true;
    [Category("Etapa (marquee)")] public string Etapa_FontFamily { get; set; } = "Comic Sans MS";
    [Category("Etapa (marquee)")] public Color Etapa_Color { get; set; } = Color.White;

    [Category("Periodo (junto al tiempo, izquierda)")]
    public int Periodo_X { get; set; } = 4;
    [Category("Periodo (junto al tiempo, izquierda)")] public int Periodo_Y { get; set; } = 196;
    [Category("Periodo (junto al tiempo, izquierda)")]
    [Description("Ancho del área del periodo; conviene que no llegue al centro del cliente para no tapar el tiempo centrado")]
    public int Periodo_W { get; set; } = 72;
    [Category("Periodo (junto al tiempo, izquierda)")] public int Periodo_H { get; set; } = 24;
    [Category("Periodo (junto al tiempo, izquierda)")] public float Periodo_FontPt { get; set; } = 9.75f;
    [Category("Periodo (junto al tiempo, izquierda)")] public bool Periodo_Bold { get; set; } = true;
    [Category("Periodo (junto al tiempo, izquierda)")] public string Periodo_FontFamily { get; set; } = "Microsoft Sans Serif";
    [Category("Periodo (junto al tiempo, izquierda)")] public Color Periodo_Color { get; set; } = Color.FromArgb(240, 240, 240);

    [Category("Cronómetro (tiempo)")]
    [Description("Si es true, el tiempo se centra en el ancho del cliente; si no, usa Cronometro_X")]
    public bool Cronometro_CentrarHorizontal { get; set; } = true;
    [Category("Cronómetro (tiempo)")] public int Cronometro_X { get; set; } = 79;
    [Category("Cronómetro (tiempo)")] public int Cronometro_Y { get; set; } = 196;
    [Category("Cronómetro (tiempo)")] public float Cronometro_FontPt { get; set; } = 16f;
    [Category("Cronómetro (tiempo)")] public bool Cronometro_Bold { get; set; } = true;
    [Category("Cronómetro (tiempo)")] public string Cronometro_FontFamily { get; set; } = "Microsoft Sans Serif";
    [Category("Cronómetro (tiempo)")] public Color Cronometro_Color { get; set; } = Color.FromArgb(224, 224, 224);

    [Category("Celebración de gol (pantalla completa)")]
    [Description("Imagen de «¡GOL!» a pantalla completa antes del detalle (ruta local o URL). Vacío = ir directo al jugador o al equipo.")]
    public string Celebracion_ImagenIntermediaPath { get; set; } = "";

    [Category("Celebración de gol (pantalla completa)")]
    [Description("Segundos mostrando la imagen intermedia (si hay ruta válida). Modo manual: mismo tiempo antes de mostrar el equipo.")]
    public int Celebracion_SplashSegundos { get; set; } = 5;

    [Category("Celebración de gol (pantalla completa)")]
    [Description("Segundos del detalle: jugador (automático) o escudo+nombre de equipo (manual).")]
    public int Celebracion_DetalleSegundos { get; set; } = 8;

    [Category("Fondo")]
    [Description("Color de fondo cuando no se usa imagen")]
    public Color Fondo_Color { get; set; } = Color.Black;
    [Category("Fondo")]
    [Description("Habilita imagen de fondo")]
    public bool Fondo_UsarImagen { get; set; }
    [Category("Fondo")]
    [Description("Ruta local o URL de la imagen de fondo")]
    public string Fondo_ImagenPath { get; set; } = "";
    [Category("Fondo")]
    [Description("PictureBoxSizeMode: 0 Normal, 1 StretchImage, 2 AutoSize, 3 CenterImage, 4 Zoom")]
    public int Fondo_ImagenSizeMode { get; set; } = 4;

    [Category("Logo local")]
    [Description("Mostrar u ocultar el cuadro del logo (el nombre puede quedar arriba del marcador numérico si el logo está oculto)")]
    public bool LogoLocal_Visible { get; set; } = true;
    [Category("Logo local")]
    [Description("PictureBoxSizeMode: 0 Normal, 1 StretchImage, 2 AutoSize, 3 CenterImage, 4 Zoom")]
    public int LogoLocal_SizeMode { get; set; } = 4;
    [Category("Logo local")] public int LogoLocal_X { get; set; } = 1;
    [Category("Logo local")] public int LogoLocal_Y { get; set; } = 57;
    [Category("Logo local")] public int LogoLocal_W { get; set; } = 121;
    [Category("Logo local")] public int LogoLocal_H { get; set; } = 32;

    [Category("Logo visitante")]
    public bool LogoVisitante_Visible { get; set; } = true;
    [Category("Logo visitante")]
    [Description("PictureBoxSizeMode: 0 Normal, 1 StretchImage, 2 AutoSize, 3 CenterImage, 4 Zoom")]
    public int LogoVisitante_SizeMode { get; set; } = 4;
    [Category("Logo visitante")] public int LogoVisitante_X { get; set; } = 131;
    [Category("Logo visitante")] public int LogoVisitante_Y { get; set; } = 57;
    [Category("Logo visitante")] public int LogoVisitante_W { get; set; } = 121;
    [Category("Logo visitante")] public int LogoVisitante_H { get; set; } = 32;

    [Category("Nombre equipo local")]
    [Description("Posición cuando el logo local está visible (encima del logo)")]
    public int NombreLocal_X { get; set; } = 1;
    [Category("Nombre equipo local")] public int NombreLocal_Y_ConLogo { get; set; } = 38;
    [Category("Nombre equipo local")]
    [Description("Posición Y cuando el logo local está oculto (encima del número de goles)")]
    public int NombreLocal_Y_SinLogo { get; set; } = 72;
    [Category("Nombre equipo local")] public int NombreLocal_W { get; set; } = 121;
    [Category("Nombre equipo local")] public int NombreLocal_H { get; set; } = 16;
    [Category("Nombre equipo local")] public float NombreLocal_FontPt { get; set; } = 8.25f;
    [Category("Nombre equipo local")] public bool NombreLocal_Bold { get; set; } = true;
    [Category("Nombre equipo local")] public string NombreLocal_FontFamily { get; set; } = "Microsoft Sans Serif";
    [Category("Nombre equipo local")] public Color NombreLocal_Color { get; set; } = Color.White;

    [Category("Nombre equipo visitante")]
    public int NombreVisitante_X { get; set; } = 131;
    [Category("Nombre equipo visitante")] public int NombreVisitante_Y_ConLogo { get; set; } = 38;
    [Category("Nombre equipo visitante")] public int NombreVisitante_Y_SinLogo { get; set; } = 72;
    [Category("Nombre equipo visitante")] public int NombreVisitante_W { get; set; } = 121;
    [Category("Nombre equipo visitante")] public int NombreVisitante_H { get; set; } = 16;
    [Category("Nombre equipo visitante")] public float NombreVisitante_FontPt { get; set; } = 8.25f;
    [Category("Nombre equipo visitante")] public bool NombreVisitante_Bold { get; set; } = true;
    [Category("Nombre equipo visitante")] public string NombreVisitante_FontFamily { get; set; } = "Microsoft Sans Serif";
    [Category("Nombre equipo visitante")] public Color NombreVisitante_Color { get; set; } = Color.White;

    [Category("Goles local")]
    public int GolLocal_X { get; set; } = 16;
    [Category("Goles local")] public int GolLocal_Y { get; set; } = 95;
    [Category("Goles local")] public int GolLocal_W { get; set; } = 93;
    [Category("Goles local")] public int GolLocal_H { get; set; } = 46;
    [Category("Goles local")] public float GolLocal_FontPt { get; set; } = 22f;
    [Category("Goles local")] public bool GolLocal_Bold { get; set; } = true;
    [Category("Goles local")] public string GolLocal_FontFamily { get; set; } = "Microsoft Sans Serif";
    [Category("Goles local")] public Color GolLocal_Color { get; set; } = Color.White;

    [Category("Goles visitante")]
    public int GolVisitante_X { get; set; } = 147;
    [Category("Goles visitante")] public int GolVisitante_Y { get; set; } = 95;
    [Category("Goles visitante")] public int GolVisitante_W { get; set; } = 93;
    [Category("Goles visitante")] public int GolVisitante_H { get; set; } = 46;
    [Category("Goles visitante")] public float GolVisitante_FontPt { get; set; } = 22f;
    [Category("Goles visitante")] public bool GolVisitante_Bold { get; set; } = true;
    [Category("Goles visitante")] public string GolVisitante_FontFamily { get; set; } = "Microsoft Sans Serif";
    [Category("Goles visitante")] public Color GolVisitante_Color { get; set; } = Color.White;

    public static MarcadorLayoutSnapshot CreateDefault() => new();

    public static string ToJson(MarcadorLayoutSnapshot s) =>
        JsonSerializer.Serialize(s, JsonOpts);

    public static MarcadorLayoutSnapshot? FromJson(string? json)
    {
        if (string.IsNullOrWhiteSpace(json)) return null;
        try
        {
            return JsonSerializer.Deserialize<MarcadorLayoutSnapshot>(json, JsonOpts);
        }
        catch
        {
            return null;
        }
    }

    private static readonly JsonSerializerOptions JsonOpts = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    static MarcadorLayoutSnapshot()
    {
        JsonOpts.Converters.Add(new ColorHexJsonConverter());
    }
}

internal sealed class ColorHexJsonConverter : JsonConverter<Color>
{
    public override Color Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            var s = reader.GetString();
            if (TryParseHex(s, out var c))
                return c;
            return Color.Black;
        }
        if (reader.TokenType == JsonTokenType.StartObject)
        {
            using var doc = JsonDocument.ParseValue(ref reader);
            var root = doc.RootElement;
            var a = root.TryGetProperty("A", out var pa) ? pa.GetByte() : (byte)255;
            var r = root.TryGetProperty("R", out var pr) ? pr.GetByte() : (byte)0;
            var g = root.TryGetProperty("G", out var pg) ? pg.GetByte() : (byte)0;
            var b = root.TryGetProperty("B", out var pb) ? pb.GetByte() : (byte)0;
            return Color.FromArgb(a, r, g, b);
        }
        return Color.Black;
    }

    public override void Write(Utf8JsonWriter writer, Color value, JsonSerializerOptions options)
    {
        writer.WriteStringValue($"#{value.R:X2}{value.G:X2}{value.B:X2}");
    }

    private static bool TryParseHex(string? hex, out Color color)
    {
        color = Color.Empty;
        if (string.IsNullOrWhiteSpace(hex)) return false;
        var s = hex.Trim();
        if (s.Length == 7 && s[0] == '#')
        {
            try { color = ColorTranslator.FromHtml(s); return true; } catch { return false; }
        }
        if (s.Length == 9 && s[0] == '#')
        {
            try
            {
                var a = Convert.ToInt32(s.Substring(1, 2), 16);
                var r = Convert.ToInt32(s.Substring(3, 2), 16);
                var g = Convert.ToInt32(s.Substring(5, 2), 16);
                var b = Convert.ToInt32(s.Substring(7, 2), 16);
                color = Color.FromArgb(a, r, g, b);
                return true;
            }
            catch { return false; }
        }
        return false;
    }
}
