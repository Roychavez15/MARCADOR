using System.Windows.Forms;

namespace Marcador.Display;

/// <summary>Label que fuerza el texto en una sola línea, sin saltos. Si no cabe, muestra "...".</summary>
public sealed class SingleLineLabel : Label
{
    private const int SS_ENDELLIPSIS = 0x00000040;

    protected override CreateParams CreateParams
    {
        get
        {
            var cp = base.CreateParams;
            cp.Style |= SS_ENDELLIPSIS;
            return cp;
        }
    }
}
