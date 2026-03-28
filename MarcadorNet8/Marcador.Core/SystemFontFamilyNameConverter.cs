using System.Collections;
using System.ComponentModel;
using System.Drawing.Text;
using System.Runtime.Versioning;

namespace Marcador.Core;

/// <summary>
/// En el PropertyGrid muestra un desplegable con las fuentes instaladas en el sistema.
/// <see cref="StringConverter.GetStandardValuesExclusive"/> en false permite seguir escribiendo un nombre a mano si hace falta.
/// </summary>
[SupportedOSPlatform("windows")]
public sealed class SystemFontFamilyNameConverter : StringConverter
{
    private static string[]? _names;
    private static readonly object LockObj = new();

    public override bool GetStandardValuesSupported(ITypeDescriptorContext? context) => true;

    public override bool GetStandardValuesExclusive(ITypeDescriptorContext? context) => false;

    public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext? context)
    {
        lock (LockObj)
        {
            if (_names == null)
            {
                using var col = new InstalledFontCollection();
                _names = col.Families
                    .Select(f => f.Name)
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .OrderBy(n => n, StringComparer.OrdinalIgnoreCase)
                    .ToArray();
            }
        }

        return new StandardValuesCollection(_names);
    }
}
