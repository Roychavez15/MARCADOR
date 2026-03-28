using System.Drawing;
using System.Runtime.Versioning;

namespace Marcador.Core;

/// <summary>
/// Carga imágenes locales sin usar <see cref="Image.FromFile"/>, que deja el archivo bloqueado
/// mientras el <see cref="Image"/> exista e impide sobrescribirlo (p. ej. al copiar un nuevo logo).
/// </summary>
[SupportedOSPlatform("windows")]
public static class ImagenArchivoSinBloqueo
{
    public static Image? CrearDesdeArchivo(string? path)
    {
        if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
            return null;
        try
        {
            var bytes = File.ReadAllBytes(path);
            using var ms = new MemoryStream(bytes);
            using var tmp = Image.FromStream(ms, false, true);
            return new Bitmap(tmp);
        }
        catch
        {
            return null;
        }
    }
}
