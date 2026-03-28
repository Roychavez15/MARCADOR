using System.Drawing;
using System.IO;
using Marcador.Core;

namespace Marcador.Admin;

/// <summary>Datos actuales del Display para previsualizar en el diseñador (nombres, logos, tiempo, periodo, goles).</summary>
public sealed record DisplayDatosPreview(
    string TituloLiga,
    string Etapa,
    string Periodo,
    string NombreLocal,
    string NombreVisitante,
    string? LogoLocalPath,
    string? LogoVisitantePath,
    long GolesLocal,
    long GolesVisitante,
    string CronometroText
);

/// <summary>Canvas visual para arrastrar y redimensionar los controles del Display. Tamaño 1:1. Lee el mismo <see cref="MarcadorLayoutSnapshot"/> que el PropertyGrid.</summary>
public class DisplayDesignerControl : UserControl
{
    private const int CanvasScale = 1;
    private const int GripMinPx = 12;
    private readonly Label _lblInfo = new()
    {
        Dock = DockStyle.Top,
        Height = 44,
        ForeColor = Color.Gainsboro,
        BackColor = Color.FromArgb(45, 45, 48),
        TextAlign = ContentAlignment.TopLeft,
        Padding = new Padding(8, 6, 8, 0),
        Font = new Font("Segoe UI", 8.25f)
    };

    private readonly Label _lblDatos = new()
    {
        Dock = DockStyle.Bottom,
        Height = 28,
        ForeColor = Color.FromArgb(180, 220, 180),
        BackColor = Color.FromArgb(35, 38, 42),
        TextAlign = ContentAlignment.MiddleLeft,
        Padding = new Padding(8, 0, 8, 0),
        Font = new Font("Consolas", 9f),
        Text = "Pase el ratón sobre un componente para ver X, Y, W, H"
    };

    private Panel _canvas = null!;
    private MarcadorLayoutSnapshot _layout = null!;
    private Control? _dragging;
    private Point _dragStart;
    private Point _controlStartLoc;
    private Size _controlStartSize;
    private bool _resizing;

    public event EventHandler? LayoutChanged;

    /// <summary>True mientras el usuario arrastra o redimensiona un componente.</summary>
    public bool IsDragging => _dragging != null;

    public DisplayDesignerControl()
    {
        BackColor = Color.FromArgb(45, 45, 48);
        Controls.Add(_lblInfo);
        Controls.Add(_lblDatos);
        _canvas = new Panel
        {
            BackColor = Color.Black,
            BorderStyle = BorderStyle.FixedSingle,
            AutoScroll = false
        };
        _canvas.MouseMove += (_, _) => MostrarDatosCanvas();
        _canvas.MouseLeave += (_, _) => MostrarDatosVacio();
        Controls.Add(_canvas);
        Resize += (_, _) => CentrarCanvas();
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        CentrarCanvas();
    }

    private void ActualizarTextoInfo()
    {
        if (_layout == null) return;
        _lblInfo.Text =
            $"Área cliente del Display: {_layout.ClientWidth} × {_layout.ClientHeight} px (valores guardados en el perfil). " +
            "Arrastrar: mover. Redimensionar: esquina inferior derecha (marca ┐), Shift + arrastre, o clic derecho.";
    }

    private static string NombreComponente(string key) => key switch
    {
        "TituloLiga" => "Título liga",
        "Etapa" => "Etapa",
        "Periodo" => "Periodo",
        "Cronometro" => "Cronómetro",
        "LogoLocal" => "Logo local",
        "LogoVisitante" => "Logo visitante",
        "NombreLocal" => "Nombre local",
        "NombreVisitante" => "Nombre visitante",
        "GolLocal" => "Goles local",
        "GolVisitante" => "Goles visitante",
        _ => key
    };

    private void MostrarDatosControl(Control c)
    {
        if (c.Tag is not string key) return;
        var x = EscalaAPixelsLogicos(c.Left);
        var y = EscalaAPixelsLogicos(c.Top);
        var w = EscalaAPixelsLogicos(c.Width);
        var h = EscalaAPixelsLogicos(c.Height);
        _lblDatos.Text = $"  {NombreComponente(key)}  │  X: {x,4}   Y: {y,4}   W: {w,4}   H: {h,4}  (px)";
    }

    private void MostrarDatosCanvas()
    {
        if (_layout == null) return;
        _lblDatos.Text = $"  Área cliente  │  W: {_layout.ClientWidth,4}   H: {_layout.ClientHeight,4}  (px)  —  Pase el ratón sobre un componente para ver sus datos";
    }

    private void MostrarDatosVacio()
    {
        _lblDatos.Text = "  Pase el ratón sobre un componente para ver X, Y, W, H";
    }

    private void CentrarCanvas()
    {
        if (_layout == null) return;
        var w = Math.Max(64, _layout.ClientWidth) * CanvasScale;
        var h = Math.Max(64, _layout.ClientHeight) * CanvasScale;
        _canvas.Size = new Size(w, h);
        var topOff = _lblInfo.Bottom + 4;
        var bottomOff = _lblDatos.Height + 4;
        _canvas.Left = Math.Max(8, (Width - _canvas.Width) / 2);
        var availH = Height - topOff - bottomOff;
        _canvas.Top = topOff + Math.Max(0, (availH - _canvas.Height) / 2);
    }

    public void AplicarLayout(MarcadorLayoutSnapshot lay, DisplayDatosPreview? datos = null)
    {
        _layout = lay;
        ActualizarTextoInfo();
        _canvas.Controls.Clear();
        var w = Math.Max(64, lay.ClientWidth) * CanvasScale;
        var h = Math.Max(64, lay.ClientHeight) * CanvasScale;
        _canvas.Size = new Size(w, h);
        CentrarCanvas();

        void AddLabel(string key, int x, int y, int ww, int hh, string text, Color fore, string fontFamily, float fontPt, bool bold, ContentAlignment align, bool usarFuenteFija = false)
        {
            var pnl = new DesignerLabelPanel(text, fore, fontFamily, fontPt, bold, align, ww, hh, usarFuenteFija)
            {
                Size = new Size(ww * CanvasScale, hh * CanvasScale),
                Location = new Point(x * CanvasScale, y * CanvasScale),
                Tag = key
            };
            WireDesignControl(pnl);
            _canvas.Controls.Add(pnl);
        }

        void AddLogo(string key, int x, int y, int ww, int hh, string hint, Image? img)
        {
            var pic = new DesignerLogoPanel(hint, img)
            {
                Size = new Size(ww * CanvasScale, hh * CanvasScale),
                Location = new Point(x * CanvasScale, y * CanvasScale),
                Tag = key
            };
            WireDesignControl(pic);
            _canvas.Controls.Add(pic);
        }

        var titulo = datos?.TituloLiga.Trim() ?? "TÍTULO LIGA";
        var etapa = datos?.Etapa.Trim() ?? "Etapa";
        var periodo = datos?.Periodo.Trim() ?? "1º T";
        var crono = datos?.CronometroText ?? "00:00";
        var nomLoc = datos?.NombreLocal.Trim() ?? "Local";
        var nomVis = datos?.NombreVisitante.Trim() ?? "Visitante";
        var golLoc = datos?.GolesLocal.ToString() ?? "0";
        var golVis = datos?.GolesVisitante.ToString() ?? "0";
        var imgLocal = CargarImagenLogo(datos?.LogoLocalPath);
        var imgVisitante = CargarImagenLogo(datos?.LogoVisitantePath);

        AddLabel("TituloLiga", lay.TituloLiga_X, lay.TituloLiga_Y, lay.TituloLiga_W, lay.TituloLiga_H, titulo.Length > 0 ? titulo : "TÍTULO LIGA", lay.TituloLiga_Color, lay.TituloLiga_FontFamily, lay.TituloLiga_FontPt, lay.TituloLiga_Bold, ContentAlignment.MiddleCenter);
        AddLabel("Etapa", lay.Etapa_X, lay.Etapa_Y, lay.Etapa_W, lay.Etapa_H, etapa.Length > 0 ? etapa : "Etapa", lay.Etapa_Color, lay.Etapa_FontFamily, lay.Etapa_FontPt, lay.Etapa_Bold, ContentAlignment.MiddleCenter);
        AddLabel("Periodo", lay.Periodo_X, lay.Periodo_Y, lay.Periodo_W, lay.Periodo_H, periodo.Length > 0 ? periodo : "1º T", lay.Periodo_Color, lay.Periodo_FontFamily, lay.Periodo_FontPt, lay.Periodo_Bold, ContentAlignment.MiddleLeft);
        AddLabel("Cronometro", lay.Cronometro_X, lay.Cronometro_Y, lay.Cronometro_W, lay.Cronometro_H, crono, lay.Cronometro_Color, lay.Cronometro_FontFamily, lay.Cronometro_FontPt, lay.Cronometro_Bold, ContentAlignment.MiddleCenter);

        if (lay.LogoLocal_Visible)
            AddLogo("LogoLocal", lay.LogoLocal_X, lay.LogoLocal_Y, lay.LogoLocal_W, lay.LogoLocal_H, nomLoc.Length > 0 ? nomLoc : "Logo local", imgLocal);
        var yLoc = lay.LogoLocal_Visible ? lay.NombreLocal_Y_ConLogo : lay.NombreLocal_Y_SinLogo;
        AddLabel("NombreLocal", lay.NombreLocal_X, yLoc, lay.NombreLocal_W, lay.NombreLocal_H, nomLoc.Length > 0 ? nomLoc : "Local", lay.NombreLocal_Color, lay.NombreLocal_FontFamily, lay.NombreLocal_FontPt, lay.NombreLocal_Bold, ContentAlignment.TopCenter, usarFuenteFija: true);
        if (lay.LogoVisitante_Visible)
            AddLogo("LogoVisitante", lay.LogoVisitante_X, lay.LogoVisitante_Y, lay.LogoVisitante_W, lay.LogoVisitante_H, nomVis.Length > 0 ? nomVis : "Logo visitante", imgVisitante);
        var yVis = lay.LogoVisitante_Visible ? lay.NombreVisitante_Y_ConLogo : lay.NombreVisitante_Y_SinLogo;
        AddLabel("NombreVisitante", lay.NombreVisitante_X, yVis, lay.NombreVisitante_W, lay.NombreVisitante_H, nomVis.Length > 0 ? nomVis : "Visitante", lay.NombreVisitante_Color, lay.NombreVisitante_FontFamily, lay.NombreVisitante_FontPt, lay.NombreVisitante_Bold, ContentAlignment.TopCenter, usarFuenteFija: true);

        AddLabel("GolLocal", lay.GolLocal_X, lay.GolLocal_Y, lay.GolLocal_W, lay.GolLocal_H, golLoc, lay.GolLocal_Color, lay.GolLocal_FontFamily, lay.GolLocal_FontPt, lay.GolLocal_Bold, ContentAlignment.MiddleCenter);
        AddLabel("GolVisitante", lay.GolVisitante_X, lay.GolVisitante_Y, lay.GolVisitante_W, lay.GolVisitante_H, golVis, lay.GolVisitante_Color, lay.GolVisitante_FontFamily, lay.GolVisitante_FontPt, lay.GolVisitante_Bold, ContentAlignment.MiddleCenter);
    }

    private static Image? CargarImagenLogo(string? path)
    {
        if (string.IsNullOrWhiteSpace(path)) return null;
        try
        {
            if (path.StartsWith("http", StringComparison.OrdinalIgnoreCase))
            {
                using var client = new System.Net.Http.HttpClient();
                var bytes = client.GetByteArrayAsync(path).GetAwaiter().GetResult();
                using var ms = new MemoryStream(bytes);
                using var img = Image.FromStream(ms);
                return new Bitmap(img);
            }
            if (!File.Exists(path)) return null;
            return ImagenArchivoSinBloqueo.CrearDesdeArchivo(path);
        }
        catch { return null; }
    }

    private static Font CrearFuente(string family, float sizePt, bool bold)
    {
        try
        {
            return new Font(family, sizePt, bold ? FontStyle.Bold : FontStyle.Regular, GraphicsUnit.Point);
        }
        catch
        {
            return new Font("Microsoft Sans Serif", sizePt, bold ? FontStyle.Bold : FontStyle.Regular, GraphicsUnit.Point);
        }
    }

    private void WireDesignControl(Control c)
    {
        c.MouseDown += Control_MouseDown;
        c.MouseMove += Control_MouseMove;
        c.MouseUp += Control_MouseUp;
        c.MouseLeave += Control_MouseLeave;
    }

    private static int GripSize(Control c)
    {
        var g = Math.Min(GripMinPx, Math.Min(c.Width, c.Height) / 2);
        return Math.Clamp(g, 10, 28);
    }

    private static bool PuntoEnGripRedimension(Control c, Point clientPt)
    {
        var g = GripSize(c);
        return clientPt.X >= c.Width - g && clientPt.Y >= c.Height - g;
    }

    private void ActualizarCursorSobreControl(Control c, Point clientPt)
    {
        if (_dragging != null) return;
        c.Cursor = PuntoEnGripRedimension(c, clientPt) ? Cursors.SizeNWSE : Cursors.SizeAll;
    }

    public void SincronizarALayout()
    {
        foreach (Control c in _canvas.Controls)
        {
            if (c.Tag is not string key) continue;
            var x = EscalaAPixelsLogicos(c.Left);
            var y = EscalaAPixelsLogicos(c.Top);
            var w = EscalaAPixelsLogicos(c.Width);
            var h = EscalaAPixelsLogicos(c.Height);

            switch (key)
            {
                case "TituloLiga": _layout.TituloLiga_X = x; _layout.TituloLiga_Y = y; _layout.TituloLiga_W = w; _layout.TituloLiga_H = h; break;
                case "Etapa": _layout.Etapa_X = x; _layout.Etapa_Y = y; _layout.Etapa_W = w; _layout.Etapa_H = h; break;
                case "Periodo":
                    _layout.Periodo_X = x; _layout.Periodo_Y = y; _layout.Periodo_W = w; _layout.Periodo_H = h;
                    if (c is DesignerLabelPanel plpPeriodo) _layout.Periodo_FontPt = plpPeriodo.CalcularFontPtParaTamano(w, h);
                    break;
                case "Cronometro":
                    _layout.Cronometro_CentrarHorizontal = false; _layout.Cronometro_X = x; _layout.Cronometro_Y = y; _layout.Cronometro_W = w; _layout.Cronometro_H = h;
                    if (c is DesignerLabelPanel plpCrono) _layout.Cronometro_FontPt = plpCrono.CalcularFontPtParaTamano(w, h, "99:59");
                    break;
                case "LogoLocal": _layout.LogoLocal_X = x; _layout.LogoLocal_Y = y; _layout.LogoLocal_W = w; _layout.LogoLocal_H = h; break;
                case "LogoVisitante": _layout.LogoVisitante_X = x; _layout.LogoVisitante_Y = y; _layout.LogoVisitante_W = w; _layout.LogoVisitante_H = h; break;
                case "NombreLocal":
                    _layout.NombreLocal_X = x;
                    if (_layout.LogoLocal_Visible) _layout.NombreLocal_Y_ConLogo = y;
                    else _layout.NombreLocal_Y_SinLogo = y;
                    _layout.NombreLocal_W = w; _layout.NombreLocal_H = h;
                    break;
                case "NombreVisitante":
                    _layout.NombreVisitante_X = x;
                    if (_layout.LogoVisitante_Visible) _layout.NombreVisitante_Y_ConLogo = y;
                    else _layout.NombreVisitante_Y_SinLogo = y;
                    _layout.NombreVisitante_W = w; _layout.NombreVisitante_H = h;
                    break;
                case "GolLocal":
                    _layout.GolLocal_X = x; _layout.GolLocal_Y = y; _layout.GolLocal_W = w; _layout.GolLocal_H = h;
                    if (c is DesignerLabelPanel plpGolL) _layout.GolLocal_FontPt = plpGolL.CalcularFontPtParaTamano(w, h, "99");
                    break;
                case "GolVisitante":
                    _layout.GolVisitante_X = x; _layout.GolVisitante_Y = y; _layout.GolVisitante_W = w; _layout.GolVisitante_H = h;
                    if (c is DesignerLabelPanel plpGolV) _layout.GolVisitante_FontPt = plpGolV.CalcularFontPtParaTamano(w, h, "99");
                    break;
            }
        }
        LayoutChanged?.Invoke(this, EventArgs.Empty);
    }

    private static int EscalaAPixelsLogicos(int scaledPixels) =>
        Math.Max(1, (int)Math.Round(scaledPixels / (double)CanvasScale));

    private void Control_MouseDown(object? sender, MouseEventArgs e)
    {
        if (sender is not Control c || c.Tag is not string || e.Button == MouseButtons.Middle)
            return;
        _dragging = c;
        _dragStart = _canvas.PointToClient(Cursor.Position);
        _controlStartLoc = c.Location;
        _controlStartSize = c.Size;
        var enGrip = PuntoEnGripRedimension(c, e.Location);
        _resizing = enGrip || e.Button == MouseButtons.Right
                    || ((ModifierKeys & Keys.Shift) != 0 && e.Button == MouseButtons.Left);
        if (_resizing && e.Button == MouseButtons.Right)
            _dragStart = _canvas.PointToClient(Cursor.Position);
        Capture = true;
    }

    private void Control_MouseMove(object? sender, MouseEventArgs e)
    {
        if (sender is Control c)
        {
            if (_dragging == null)
            {
                ActualizarCursorSobreControl(c, e.Location);
                MostrarDatosControl(c);
            }
        }
        ProcesarArrastre();
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);
        ProcesarArrastre();
    }

    private void Control_MouseLeave(object? sender, EventArgs e)
    {
        if (sender is Control c && _dragging == null)
            c.Cursor = Cursors.Default;
    }

    private void ProcesarArrastre()
    {
        if (_dragging == null) return;
        var pos = _canvas.PointToClient(Cursor.Position);
        if (_resizing)
        {
            var dx = pos.X - _dragStart.X;
            var dy = pos.Y - _dragStart.Y;
            var nw = Math.Max(16, _controlStartSize.Width + dx);
            var nh = Math.Max(12, _controlStartSize.Height + dy);
            _dragging.Size = new Size(nw, nh);
            _dragging.Invalidate();
        }
        else
        {
            _dragging.Location = new Point(
                Math.Max(0, Math.Min(_canvas.Width - _dragging.Width, _controlStartLoc.X + pos.X - _dragStart.X)),
                Math.Max(0, Math.Min(_canvas.Height - _dragging.Height, _controlStartLoc.Y + pos.Y - _dragStart.Y)));
        }
        MostrarDatosControl(_dragging);
    }

    private void Control_MouseUp(object? sender, MouseEventArgs e) => FinalizarArrastre();

    protected override void OnMouseUp(MouseEventArgs e)
    {
        base.OnMouseUp(e);
        FinalizarArrastre();
    }

    private void FinalizarArrastre()
    {
        if (_dragging != null)
        {
            Capture = false;
            if (_dragging.Tag is string)
                ActualizarCursorSobreControl(_dragging, _dragging.PointToClient(Cursor.Position));
            _dragging = null;
            SincronizarALayout();
        }
    }

    /// <summary>Panel para labels con borde visible. La fuente se escala para llenar el componente al redimensionar.</summary>
    private sealed class DesignerLabelPanel : Panel
    {
        private readonly string _text;
        private readonly Color _foreColor;
        private readonly string _fontFamily;
        private readonly float _baseFontPt;
        private readonly bool _bold;
        private readonly ContentAlignment _align;
        private readonly int _origW, _origH;
        private readonly bool _usarFuenteFija;

        public DesignerLabelPanel(string text, Color foreColor, string fontFamily, float baseFontPt, bool bold, ContentAlignment align, int origW, int origH, bool usarFuenteFija = false)
        {
            _text = text;
            _foreColor = foreColor;
            _fontFamily = fontFamily;
            _baseFontPt = baseFontPt;
            _bold = bold;
            _align = align;
            _origW = Math.Max(1, origW);
            _origH = Math.Max(1, origH);
            _usarFuenteFija = usarFuenteFija;
            DoubleBuffered = true;
            BackColor = Color.FromArgb(50, 50, 55);
            BorderStyle = BorderStyle.FixedSingle;
        }

        /// <summary>Calcula el tamaño de fuente que llena el rectángulo. textOverride para usar texto representativo al sincronizar (ej. "99" en goles).</summary>
        public float CalcularFontPtParaTamano(int w, int h, string? textOverride = null)
        {
            var t = textOverride ?? _text;
            if (string.IsNullOrEmpty(t) || w <= 0 || h <= 0) return Math.Max(6f, _baseFontPt);
            var r = new Rectangle(0, 0, w, h);
            var fmt = TextFormatFlags.SingleLine | TextFormatFlags.NoPadding;
            for (var pt = 72f; pt >= 6f; pt -= 1f)
            {
                Font? f = null;
                try
                {
                    f = new Font(_fontFamily, pt, _bold ? FontStyle.Bold : FontStyle.Regular, GraphicsUnit.Point);
                }
                catch
                {
                    f = new Font("Microsoft Sans Serif", pt, _bold ? FontStyle.Bold : FontStyle.Regular, GraphicsUnit.Point);
                }
                try
                {
                    var sz = TextRenderer.MeasureText(t, f, Size.Empty, fmt);
                    if (sz.Width <= r.Width && sz.Height <= r.Height) return pt;
                }
                finally { f?.Dispose(); }
            }
            return 6f;
        }

        private float CalcularFuenteQueLlena(Rectangle r)
        {
            if (string.IsNullOrEmpty(_text) || r.Width <= 0 || r.Height <= 0) return Math.Max(6f, _baseFontPt);
            var fmt = TextFormatFlags.SingleLine | TextFormatFlags.NoPadding;
            for (var pt = 72f; pt >= 6f; pt -= 1f)
            {
                Font? f = null;
                try
                {
                    f = new Font(_fontFamily, pt, _bold ? FontStyle.Bold : FontStyle.Regular, GraphicsUnit.Point);
                }
                catch
                {
                    f = new Font("Microsoft Sans Serif", pt, _bold ? FontStyle.Bold : FontStyle.Regular, GraphicsUnit.Point);
                }
                try
                {
                    var sz = TextRenderer.MeasureText(_text, f, Size.Empty, fmt);
                    if (sz.Width <= r.Width && sz.Height <= r.Height) return pt;
                }
                finally { f?.Dispose(); }
            }
            return 6f;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            const int pad = 2;
            var r = new Rectangle(pad, pad, Math.Max(1, Width - pad * 2), Math.Max(1, Height - pad * 2));
            var pt = _usarFuenteFija ? _baseFontPt : CalcularFuenteQueLlena(r);
            Font? font = null;
            try
            {
                font = new Font(_fontFamily, pt, _bold ? FontStyle.Bold : FontStyle.Regular, GraphicsUnit.Point);
            }
            catch
            {
                font = new Font("Microsoft Sans Serif", pt, _bold ? FontStyle.Bold : FontStyle.Regular, GraphicsUnit.Point);
            }
            try
            {
                var fmt = (_align switch
                {
                    ContentAlignment.TopCenter => TextFormatFlags.HorizontalCenter | TextFormatFlags.Top | TextFormatFlags.SingleLine | TextFormatFlags.NoPadding,
                    ContentAlignment.MiddleCenter => TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine | TextFormatFlags.NoPadding,
                    ContentAlignment.MiddleLeft => TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.SingleLine | TextFormatFlags.NoPadding,
                    _ => TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine | TextFormatFlags.NoPadding
                }) | TextFormatFlags.EndEllipsis;
                TextRenderer.DrawText(e.Graphics, _text, font, r, _foreColor, fmt);
            }
            finally
            {
                font?.Dispose();
            }
            var g = GripSize(this);
            using var pen = new Pen(Color.FromArgb(180, 180, 120), 1.5f);
            e.Graphics.DrawRectangle(pen, Width - g, Height - g, g - 1, g - 1);
        }
    }

    /// <summary>Panel del logo. Si hay imagen la escala para llenar (Zoom). Si no, texto escalado.</summary>
    private sealed class DesignerLogoPanel : Panel
    {
        private readonly string _titulo;
        private readonly Image? _image;

        public DesignerLogoPanel(string titulo, Image? image)
        {
            _titulo = titulo;
            _image = image;
            DoubleBuffered = true;
            BackColor = Color.FromArgb(55, 55, 58);
            BorderStyle = BorderStyle.FixedSingle;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var r = ClientRectangle;
            if (_image != null)
            {
                var iw = _image.Width;
                var ih = _image.Height;
                if (iw > 0 && ih > 0)
                {
                    var scale = Math.Min((float)r.Width / iw, (float)r.Height / ih);
                    var sw = (int)(iw * scale);
                    var sh = (int)(ih * scale);
                    var x = (r.Width - sw) / 2;
                    var y = (r.Height - sh) / 2;
                    e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    e.Graphics.DrawImage(_image, x, y, sw, sh);
                }
            }
            else
            {
                var pt = Math.Clamp(Math.Min(Width, Height) / 4f, 6f, 24f);
                using var font = new Font("Segoe UI", pt);
                TextRenderer.DrawText(e.Graphics, _titulo, font, r, Color.Silver,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine);
            }
            var g = GripSize(this);
            using var pen = new Pen(Color.FromArgb(220, 220, 100), 2f);
            e.Graphics.DrawRectangle(pen, Width - g, Height - g, g - 1, g - 1);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) _image?.Dispose();
            base.Dispose(disposing);
        }
    }
}
