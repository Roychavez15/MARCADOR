using System.Drawing;
using System.Drawing.Text;
using System.Globalization;
using System.Collections.Generic;
using Marcador.Core;
using Microsoft.AspNetCore.SignalR.Client;

namespace Marcador.Display;

public partial class Form1 : Form
{
    private MarcadorDb _db = null!;
    private HubConnection? _hubConnection;
    private string? _layoutJsonCache;
    private DateTime? _cronometroStartedAtUtc;
    private long _cronometroTotalSecondsStopped;
    private bool _cronometroIsRunning;
    private System.Windows.Forms.Timer? _timerCronometro;
    private static readonly HttpClient HttpImages = new() { Timeout = TimeSpan.FromSeconds(12) };
    private string? _fondoCacheKey;
    private Image? _fondoCacheImage;
    private string? _splashCelebracionKey;
    private Image? _splashCelebracionImage;
    private string? _celebracionManualCacheKey;
    private int _celebracionGeomJw = -1, _celebracionGeomJh = -1;
    private int _celebracionGeomMw = -1, _celebracionGeomMh = -1;
    private int _celebracionLayoutModo; // 1 = detalle jugador, 2 = manual

    public Form1()
    {
        InitializeComponent();
    }

    /// <summary>Referencia de diseño de la pantalla de celebración (coincide con ClientWidth/Height por defecto del marcador).</summary>
    private const int CelebracionRefW = 256;
    private const int CelebracionRefH = 236;

    private static void AsignarFuente(Label lbl, Font nueva)
    {
        var anterior = lbl.Font;
        lbl.Font = nueva;
        anterior?.Dispose();
    }

    /// <summary>Coloca nombre, dorsal, fotos y estadísticas del gol escalados al tamaño actual del cliente (mismo que el Display).</summary>
    private void AplicarGeometriaCelebracionDetalleJugador(int cw, int ch, bool forzar = false)
    {
        cw = Math.Max(64, cw);
        ch = Math.Max(64, ch);
        if (!forzar && _celebracionLayoutModo == 1 && cw == _celebracionGeomJw && ch == _celebracionGeomJh)
            return;
        _celebracionLayoutModo = 1;
        _celebracionGeomJw = cw;
        _celebracionGeomJh = ch;
        var sx = cw / (float)CelebracionRefW;
        var sy = ch / (float)CelebracionRefH;
        var fs = Math.Min(sx, sy);
        int S(float v) => Math.Max(1, (int)Math.Round(v));

        void Place(Control c, int x, int y, int w, int h)
        {
            c.Location = new Point(S(x * sx), S(y * sy));
            c.Size = new Size(S(w * sx), S(h * sy));
        }

        AsignarFuente(lblGolNombre, new Font("Arial", 11.25f * fs, FontStyle.Bold, GraphicsUnit.Point));
        lblGolNombre.ForeColor = SystemColors.Control;
        lblGolNombre.AutoSize = false;
        lblGolNombre.AutoEllipsis = true;
        lblGolNombre.TextAlign = ContentAlignment.TopCenter;
        Place(lblGolNombre, 4, 25, 248, 32);

        AsignarFuente(lblGolNumero, new Font("Arial", 20.25f * fs, FontStyle.Bold, GraphicsUnit.Point));
        lblGolNumero.ForeColor = SystemColors.Control;
        lblGolNumero.AutoSize = false;
        lblGolNumero.TextAlign = ContentAlignment.MiddleCenter;
        Place(lblGolNumero, 7, 133, 89, 40);

        picGolEscudo.SizeMode = PictureBoxSizeMode.Zoom;
        Place(picGolEscudo, 5, 60, 89, 70);

        picGolFoto.SizeMode = PictureBoxSizeMode.Zoom;
        Place(picGolFoto, 102, 60, 145, 111);

        AsignarFuente(lblGolEtiquetaPartido, new Font("Arial Narrow", 9.75f * fs, FontStyle.Bold, GraphicsUnit.Point));
        AsignarFuente(lblGolEtiquetaCamp, new Font("Arial Narrow", 9.75f * fs, FontStyle.Bold, GraphicsUnit.Point));
        AsignarFuente(lblGolPartidoValor, new Font("Arial Narrow", 9.75f * fs, FontStyle.Bold, GraphicsUnit.Point));
        AsignarFuente(lblGolCampValor, new Font("Arial Narrow", 9.75f * fs, FontStyle.Bold, GraphicsUnit.Point));

        lblGolEtiquetaPartido.ForeColor = SystemColors.Control;
        lblGolEtiquetaCamp.ForeColor = SystemColors.Control;
        lblGolPartidoValor.ForeColor = SystemColors.Control;
        lblGolCampValor.ForeColor = SystemColors.Control;
        lblGolEtiquetaPartido.AutoSize = false;
        lblGolEtiquetaCamp.AutoSize = false;
        lblGolPartidoValor.AutoSize = false;
        lblGolCampValor.AutoSize = false;
        lblGolEtiquetaPartido.TextAlign = ContentAlignment.MiddleLeft;
        lblGolEtiquetaCamp.TextAlign = ContentAlignment.MiddleLeft;
        lblGolPartidoValor.TextAlign = ContentAlignment.MiddleRight;
        lblGolCampValor.TextAlign = ContentAlignment.MiddleRight;
        Place(lblGolEtiquetaPartido, 4, 180, 160, 22);
        Place(lblGolEtiquetaCamp, 4, 202, 160, 22);
        Place(lblGolPartidoValor, 172, 180, 76, 22);
        Place(lblGolCampValor, 172, 202, 76, 22);
    }

    /// <summary>Celebración manual (escudo + nombre) con la misma escala que el área cliente del Display.</summary>
    private void AplicarGeometriaCelebracionDetalleManual(int cw, int ch, bool forzar = false)
    {
        cw = Math.Max(64, cw);
        ch = Math.Max(64, ch);
        if (!forzar && _celebracionLayoutModo == 2 && cw == _celebracionGeomMw && ch == _celebracionGeomMh)
            return;
        _celebracionLayoutModo = 2;
        _celebracionGeomMw = cw;
        _celebracionGeomMh = ch;
        var sx = cw / (float)CelebracionRefW;
        var sy = ch / (float)CelebracionRefH;
        var fs = Math.Min(sx, sy);
        int S(float v) => Math.Max(1, (int)Math.Round(v));

        var logoW = S(89 * sx);
        var logoH = S(70 * sy);
        picGolEscudo.SizeMode = PictureBoxSizeMode.Zoom;
        picGolEscudo.Size = new Size(logoW, logoH);
        picGolEscudo.Location = new Point((cw - logoW) / 2, S(60 * sy));

        AsignarFuente(lblGolNombre, new Font("Segoe UI", 16f * fs, FontStyle.Bold, GraphicsUnit.Point));
        lblGolNombre.ForeColor = Color.White;
        lblGolNombre.AutoSize = false;
        lblGolNombre.AutoEllipsis = true;
        lblGolNombre.TextAlign = ContentAlignment.TopCenter;
        var pad = S(8 * sx);
        var gap = S(12 * Math.Min(sx, sy));
        var topNom = picGolEscudo.Bottom + gap;
        lblGolNombre.Location = new Point(pad, topNom);
        lblGolNombre.Size = new Size(Math.Max(40, cw - 2 * pad), Math.Max(S(48 * sy), ch - topNom - pad));
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        Directory.CreateDirectory(Path.GetDirectoryName(Config.DbPath)!);
        _db = new MarcadorDb(Config.DbPath);
        ConectarSignalR();
        _timerCronometro = new System.Windows.Forms.Timer { Interval = 100 };
        _timerCronometro.Tick += TimerCronometro_Tick;
        _timerCronometro.Start();
        timerRefresh.Start();
        RefrescarTodo();
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        _timerCronometro?.Stop();
        _timerCronometro?.Dispose();
        timerRefresh.Stop();
        _fondoCacheImage?.Dispose();
        _fondoCacheImage = null;
        picSplashGol.Image = null;
        _splashCelebracionImage?.Dispose();
        _splashCelebracionImage = null;
        _ = _hubConnection?.StopAsync();
        base.OnFormClosing(e);
    }

    private async void ConectarSignalR()
    {
        try
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl(Config.SignalRUrl)
                .WithAutomaticReconnect()
                .Build();

            _hubConnection.On("EstadoActualizado", () =>
            {
                if (IsDisposed) return;
                BeginInvoke(RefrescarTodo);
            });

            await _hubConnection.StartAsync();
        }
        catch { }
    }

    private void TimerRefresh_Tick(object? sender, EventArgs e) => RefrescarTodo();

    /// <summary>Timer dedicado al cronómetro (100ms). Calcula tiempo localmente sin tocar DB; solo usa caché actualizado por RefrescarMarcador.</summary>
    private void TimerCronometro_Tick(object? sender, EventArgs e)
    {
        if (IsDisposed || lblCronometro.IsDisposed) return;
        long segundos;
        if (_cronometroIsRunning && _cronometroStartedAtUtc.HasValue)
        {
            var elapsed = (DateTime.UtcNow - _cronometroStartedAtUtc.Value).TotalSeconds;
            segundos = (long)Math.Max(0, Math.Floor(elapsed));
        }
        else
            segundos = _cronometroTotalSecondsStopped;

        var ts = TimeSpan.FromSeconds(segundos);
        var txt = $"{(int)ts.TotalMinutes:00}:{ts.Seconds:00}";
        if (lblCronometro.Text != txt)
            lblCronometro.Text = txt;
    }

    private void RefrescarTodo()
    {
        try
        {
            RefrescarMarcador();
            RefrescarCelebracion();
        }
        catch { }
    }

    private void RefrescarMarcador()
    {
        var lay = _db.GetMarcadorLayoutSnapshot();
        var json = MarcadorLayoutSnapshot.ToJson(lay);
        if (json != _layoutJsonCache)
        {
            _layoutJsonCache = json;
            AplicarLayoutDesdeSnapshot(lay);
        }
        else
            AplicarPosicionDisplayDesdeSnapshot(lay);

        var partido = _db.GetPartidoActual();
        var estado = _db.GetEstado();
        var cron = _db.GetCronometro();
        _cronometroIsRunning = cron.IsRunning == 1;
        if (_cronometroIsRunning && !string.IsNullOrWhiteSpace(cron.StartedAtUtc) &&
            DateTime.TryParse(cron.StartedAtUtc, null, System.Globalization.DateTimeStyles.RoundtripKind, out var startUtc))
        {
            _cronometroStartedAtUtc = startUtc;
            _cronometroTotalSecondsStopped = 0;
        }
        else
        {
            _cronometroStartedAtUtc = null;
            _cronometroTotalSecondsStopped = cron.TotalSeconds;
        }
        TimerCronometro_Tick(null, EventArgs.Empty);

        var titulo = (estado.TituloLiga ?? "").Trim();
        var etapa = (estado.TextoMarquee ?? "").Trim();
        var periodo = (estado.SubtituloPeriodo ?? "").Trim();
        var mostrarTitulo = titulo.Length > 0;
        var mostrarEtapa = etapa.Length > 0;
        var mostrarPeriodo = periodo.Length > 0;

        lblTituloLiga.Visible = mostrarTitulo;
        lblEtapa.Visible = mostrarEtapa;
        lblPeriodo.Visible = mostrarPeriodo;
        if (mostrarTitulo)
            lblTituloLiga.Text = titulo;
        if (mostrarEtapa)
            lblEtapa.Text = etapa;
        if (mostrarPeriodo)
            lblPeriodo.Text = periodo;

        if (lay.Cabecera_ApilarSiVacio)
        {
            var y = 0;
            if (mostrarTitulo)
            {
                lblTituloLiga.Location = new Point(lay.TituloLiga_X, y);
                lblTituloLiga.Size = new Size(Math.Max(1, lay.TituloLiga_W), Math.Max(1, lay.TituloLiga_H));
                y += Math.Max(1, lay.TituloLiga_H);
            }
            if (mostrarEtapa)
            {
                lblEtapa.Location = new Point(lay.Etapa_X, y);
                lblEtapa.Size = new Size(Math.Max(1, lay.Etapa_W), Math.Max(1, lay.Etapa_H));
            }
        }

        var nomLoc = (partido?.NombreLocal ?? "").Trim();
        var nomVis = (partido?.NombreVisitante ?? "").Trim();
        lblNombreLocal.Visible = nomLoc.Length > 0;
        lblNombreVisitante.Visible = nomVis.Length > 0;
        if (nomLoc.Length > 0)
            lblNombreLocal.Text = nomLoc;
        if (nomVis.Length > 0)
            lblNombreVisitante.Text = nomVis;

        var yLoc = lay.LogoLocal_Visible ? lay.NombreLocal_Y_ConLogo : lay.NombreLocal_Y_SinLogo;
        var yVis = lay.LogoVisitante_Visible ? lay.NombreVisitante_Y_ConLogo : lay.NombreVisitante_Y_SinLogo;
        lblNombreLocal.Location = new Point(lay.NombreLocal_X, yLoc);
        lblNombreLocal.Size = new Size(Math.Max(1, lay.NombreLocal_W), Math.Max(1, lay.NombreLocal_H));
        lblNombreVisitante.Location = new Point(lay.NombreVisitante_X, yVis);
        lblNombreVisitante.Size = new Size(Math.Max(1, lay.NombreVisitante_W), Math.Max(1, lay.NombreVisitante_H));

        picLogoLocal.Visible = lay.LogoLocal_Visible;
        picLogoVisitante.Visible = lay.LogoVisitante_Visible;

        lblCronometro.AutoSize = false;
        lblCronometro.Size = new Size(Math.Max(1, lay.Cronometro_W), Math.Max(1, lay.Cronometro_H));
        if (lay.Cronometro_CentrarHorizontal)
            lblCronometro.Location = new Point((lay.ClientWidth - lay.Cronometro_W) / 2, lay.Cronometro_Y);
        else
            lblCronometro.Location = new Point(lay.Cronometro_X, lay.Cronometro_Y);

        CargarLogo(picLogoLocal, partido?.LogoLocal);
        CargarLogo(picLogoVisitante, partido?.LogoVisitante);

        // Al final: el tamaño del marcador numérico debe seguir al texto (2+ dígitos). MeasureString coincide con el dibujo GDI+ del Label.
        AjustarEtiquetaGoles(lblGolesLocal, estado.GolesLocal.ToString(), lay.GolLocal_X, lay.GolLocal_W, lay.GolLocal_Y, lay.GolLocal_H);
        AjustarEtiquetaGoles(lblGolesVisitante, estado.GolesVisitante.ToString(), lay.GolVisitante_X, lay.GolVisitante_W, lay.GolVisitante_Y, lay.GolVisitante_H);

        lblCronometro.BringToFront();
    }

    /// <summary>Ancho/alto al menos los del diseño, pero expande al texto real (evita recorte con fuentes grandes o negrita).</summary>
    private static void AjustarEtiquetaGoles(Label lbl, string texto, int colX, int colW, int y, int h)
    {
        lbl.AutoEllipsis = false;
        lbl.Text = texto;
        lbl.AutoSize = false;

        var padX = 22;
        int textW;
        float lineH;
        try
        {
            using var g = lbl.CreateGraphics();
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            var sz = g.MeasureString(texto, lbl.Font);
            textW = (int)Math.Ceiling(sz.Width) + padX;
            lineH = lbl.Font.GetHeight(g);
        }
        catch
        {
            var sz = TextRenderer.MeasureText(texto, lbl.Font, Size.Empty,
                TextFormatFlags.SingleLine | TextFormatFlags.NoPadding);
            textW = sz.Width + padX;
            lineH = sz.Height;
        }

        var w = Math.Max(Math.Max(1, colW), textW);
        var hh = Math.Max(Math.Max(1, h), (int)Math.Ceiling(lineH) + 6);
        lbl.Size = new Size(w, hh);
        var cx = colX + Math.Max(1, colW) / 2;
        lbl.Location = new Point(Math.Max(0, cx - w / 2), y);
    }

    private void AplicarPosicionDisplayDesdeSnapshot(MarcadorLayoutSnapshot lay)
    {
        var p = new Point(lay.DisplayLeft, lay.DisplayTop);
        if (Location != p)
            Location = p;
    }

    private void AplicarLayoutDesdeSnapshot(MarcadorLayoutSnapshot lay)
    {
        var w = Math.Max(64, lay.ClientWidth);
        var h = Math.Max(64, lay.ClientHeight);
        ClientSize = new Size(w, h);
        MinimumSize = new Size(w, h);
        MaximumSize = new Size(w, h);
        AplicarPosicionDisplayDesdeSnapshot(lay);
        pnlMarcador.Size = new Size(w, h);
        pnlCelebracion.Size = new Size(w, h);
        picSplashGol.Dock = DockStyle.Fill;
        picSplashGol.SizeMode = PictureBoxSizeMode.Zoom;
        picSplashGol.BackColor = Color.Black;
        pnlMarcador.BackColor = lay.Fondo_Color;

        // Para que el fondo (color/imagen) se vea detrás del texto.
        lblTituloLiga.BackColor = Color.Transparent;
        lblEtapa.BackColor = Color.Transparent;
        lblPeriodo.BackColor = Color.Transparent;
        lblNombreLocal.BackColor = Color.Transparent;
        lblNombreVisitante.BackColor = Color.Transparent;
        lblGolesLocal.BackColor = Color.Transparent;
        lblGolesVisitante.BackColor = Color.Transparent;
        lblCronometro.BackColor = Color.Transparent;
        picLogoLocal.BackColor = Color.Transparent;
        picLogoVisitante.BackColor = Color.Transparent;

        lblTituloLiga.AutoSize = false;
        if (!lay.Cabecera_ApilarSiVacio)
        {
            lblTituloLiga.Location = new Point(lay.TituloLiga_X, lay.TituloLiga_Y);
            lblTituloLiga.Size = new Size(Math.Max(1, lay.TituloLiga_W), Math.Max(1, lay.TituloLiga_H));
        }
        lblTituloLiga.Font = CrearFuente(lay.TituloLiga_FontFamily, lay.TituloLiga_FontPt, lay.TituloLiga_Bold);
        lblTituloLiga.ForeColor = lay.TituloLiga_Color;
        lblTituloLiga.TextAlign = ContentAlignment.MiddleCenter;

        lblEtapa.AutoSize = false;
        if (!lay.Cabecera_ApilarSiVacio)
        {
            lblEtapa.Location = new Point(lay.Etapa_X, lay.Etapa_Y);
            lblEtapa.Size = new Size(Math.Max(1, lay.Etapa_W), Math.Max(1, lay.Etapa_H));
        }
        lblEtapa.Font = CrearFuente(lay.Etapa_FontFamily, lay.Etapa_FontPt, lay.Etapa_Bold);
        lblEtapa.ForeColor = lay.Etapa_Color;
        lblEtapa.TextAlign = ContentAlignment.MiddleCenter;

        lblPeriodo.AutoSize = false;
        lblPeriodo.Location = new Point(lay.Periodo_X, lay.Periodo_Y);
        lblPeriodo.Size = new Size(Math.Max(1, lay.Periodo_W), Math.Max(1, lay.Periodo_H));
        lblPeriodo.Font = CrearFuente(lay.Periodo_FontFamily, lay.Periodo_FontPt, lay.Periodo_Bold);
        lblPeriodo.ForeColor = lay.Periodo_Color;
        lblPeriodo.TextAlign = ContentAlignment.MiddleLeft;

        lblCronometro.Font = CrearFuente(lay.Cronometro_FontFamily, lay.Cronometro_FontPt, lay.Cronometro_Bold);
        lblCronometro.ForeColor = lay.Cronometro_Color;

        picLogoLocal.SizeMode = ModoImagen(lay.LogoLocal_SizeMode);
        picLogoLocal.Location = new Point(lay.LogoLocal_X, lay.LogoLocal_Y);
        picLogoLocal.Size = new Size(Math.Max(1, lay.LogoLocal_W), Math.Max(1, lay.LogoLocal_H));
        picLogoVisitante.SizeMode = ModoImagen(lay.LogoVisitante_SizeMode);
        picLogoVisitante.Location = new Point(lay.LogoVisitante_X, lay.LogoVisitante_Y);
        picLogoVisitante.Size = new Size(Math.Max(1, lay.LogoVisitante_W), Math.Max(1, lay.LogoVisitante_H));

        lblNombreLocal.AutoSize = false;
        lblNombreLocal.Font = CrearFuente(lay.NombreLocal_FontFamily, lay.NombreLocal_FontPt, lay.NombreLocal_Bold);
        lblNombreLocal.ForeColor = lay.NombreLocal_Color;
        lblNombreLocal.TextAlign = ContentAlignment.TopCenter;
        lblNombreVisitante.AutoSize = false;
        lblNombreVisitante.Font = CrearFuente(lay.NombreVisitante_FontFamily, lay.NombreVisitante_FontPt, lay.NombreVisitante_Bold);
        lblNombreVisitante.ForeColor = lay.NombreVisitante_Color;
        lblNombreVisitante.TextAlign = ContentAlignment.TopCenter;

        lblGolesLocal.AutoSize = false;
        lblGolesLocal.Location = new Point(lay.GolLocal_X, lay.GolLocal_Y);
        lblGolesLocal.Size = new Size(Math.Max(1, lay.GolLocal_W), Math.Max(1, lay.GolLocal_H));
        lblGolesLocal.Font = CrearFuente(lay.GolLocal_FontFamily, lay.GolLocal_FontPt, lay.GolLocal_Bold);
        lblGolesLocal.ForeColor = lay.GolLocal_Color;
        lblGolesLocal.TextAlign = ContentAlignment.MiddleCenter;

        lblGolesVisitante.AutoSize = false;
        lblGolesVisitante.Location = new Point(lay.GolVisitante_X, lay.GolVisitante_Y);
        lblGolesVisitante.Size = new Size(Math.Max(1, lay.GolVisitante_W), Math.Max(1, lay.GolVisitante_H));
        lblGolesVisitante.Font = CrearFuente(lay.GolVisitante_FontFamily, lay.GolVisitante_FontPt, lay.GolVisitante_Bold);
        lblGolesVisitante.ForeColor = lay.GolVisitante_Color;
        lblGolesVisitante.TextAlign = ContentAlignment.MiddleCenter;

        AplicarFondo(lay);

        AplicarGeometriaCelebracionDetalleJugador(w, h, forzar: true);
        _celebracionManualCacheKey = null;
    }

    private static PictureBoxSizeMode ModoImagen(int v)
    {
        v = Math.Clamp(v, 0, 4);
        return (PictureBoxSizeMode)v;
    }

    private void AplicarFondo(MarcadorLayoutSnapshot lay)
    {
        pnlMarcador.BackColor = lay.Fondo_Color;
        var usar = lay.Fondo_UsarImagen && !string.IsNullOrWhiteSpace(lay.Fondo_ImagenPath);
        if (!usar)
        {
            if (pnlMarcador.BackgroundImage != null)
                pnlMarcador.BackgroundImage = null;
            return;
        }

        var key = (lay.Fondo_ImagenPath ?? "").Trim();
        if (!string.Equals(_fondoCacheKey, key, StringComparison.OrdinalIgnoreCase))
        {
            _fondoCacheImage?.Dispose();
            _fondoCacheImage = CargarImagen(key);
            _fondoCacheKey = key;
        }
        pnlMarcador.BackgroundImage = _fondoCacheImage;
        pnlMarcador.BackgroundImageLayout = lay.Fondo_ImagenSizeMode switch
        {
            0 => ImageLayout.None,
            1 => ImageLayout.Stretch,
            2 => ImageLayout.None,
            3 => ImageLayout.Center,
            4 => ImageLayout.Zoom,
            _ => ImageLayout.Zoom
        };
    }

    private static Image? CargarImagen(string pathOrUrl)
    {
        try
        {
            if (pathOrUrl.StartsWith("http", StringComparison.OrdinalIgnoreCase))
            {
                var bytes = HttpImages.GetByteArrayAsync(pathOrUrl).GetAwaiter().GetResult();
                using var ms = new MemoryStream(bytes);
                return Image.FromStream(ms);
            }
            if (File.Exists(pathOrUrl))
                return Image.FromFile(pathOrUrl);
        }
        catch { }
        return null;
    }

    private static Font CrearFuente(string? familia, float puntos, bool negrita)
    {
        var f = string.IsNullOrWhiteSpace(familia) ? "Microsoft Sans Serif" : familia.Trim();
        try
        {
            return new Font(f, puntos, negrita ? FontStyle.Bold : FontStyle.Regular, GraphicsUnit.Point);
        }
        catch
        {
            return new Font("Microsoft Sans Serif", puntos, negrita ? FontStyle.Bold : FontStyle.Regular, GraphicsUnit.Point);
        }
    }

    private void RefrescarCelebracion()
    {
        var c = _db.GetCelebracion();
        if (c.Activa != 1)
        {
            _celebracionManualCacheKey = null;
            pnlCelebracion.Visible = false;
            picSplashGol.Visible = false;
            return;
        }

        DateTime? hasta = null;
        try
        {
            if (!string.IsNullOrEmpty(c.VisibleHastaUtc))
                hasta = DateTime.Parse(c.VisibleHastaUtc, null, DateTimeStyles.RoundtripKind);
        }
        catch { }

        if (hasta.HasValue && DateTime.UtcNow > hasta.Value)
        {
            _db.DesactivarCelebracionGol();
            pnlCelebracion.Visible = false;
            picSplashGol.Visible = false;
            return;
        }

        var lay = _db.GetMarcadorLayoutSnapshot();
        var splashPath = (lay.Celebracion_ImagenIntermediaPath ?? "").Trim();
        var puedeSplash = !string.IsNullOrEmpty(splashPath)
            && (splashPath.StartsWith("http", StringComparison.OrdinalIgnoreCase) || File.Exists(splashPath));
        var splashDur = puedeSplash ? Math.Max(0, lay.Celebracion_SplashSegundos) : 0;

        DateTime inicioUtc;
        try
        {
            if (!string.IsNullOrEmpty(c.InicioUtc))
                inicioUtc = DateTime.Parse(c.InicioUtc, null, DateTimeStyles.RoundtripKind).ToUniversalTime();
            else
                inicioUtc = DateTime.UtcNow;
        }
        catch
        {
            inicioUtc = DateTime.UtcNow;
        }

        var elapsed = (DateTime.UtcNow - inicioUtc).TotalSeconds;
        var enSplash = splashDur > 0 && elapsed < splashDur;

        if (enSplash)
        {
            _celebracionManualCacheKey = null;
            pnlCelebracion.Visible = true;
            pnlCelebracion.BringToFront();
            picSplashGol.Visible = true;
            picSplashGol.BringToFront();
            AsegurarImagenSplashCelebracion(splashPath);
            OcultarControlesDetalleCelebracion();
            return;
        }

        var manual = c.EsManual == 1;
        if (manual)
        {
            var key = $"manual|{c.Nombres ?? ""}|{c.EscudoUrl ?? ""}";
            if (key != _celebracionManualCacheKey)
            {
                _celebracionManualCacheKey = key;
                AplicarVistaCelebracionEquipoManual(c);
            }
        }
        else
        {
            _celebracionManualCacheKey = null;
            AplicarVistaCelebracionJugador(c);
        }

        picSplashGol.Visible = false;
        pnlCelebracion.Visible = true;
        pnlCelebracion.BringToFront();
    }

    private void AsegurarImagenSplashCelebracion(string path)
    {
        var key = path.Trim();
        if (string.Equals(_splashCelebracionKey, key, StringComparison.OrdinalIgnoreCase) && _splashCelebracionImage != null)
        {
            if (!ReferenceEquals(picSplashGol.Image, _splashCelebracionImage))
            {
                picSplashGol.Image = null;
                picSplashGol.Image = _splashCelebracionImage;
            }
            return;
        }

        picSplashGol.Image = null;
        _splashCelebracionImage?.Dispose();
        _splashCelebracionImage = CargarImagen(key);
        _splashCelebracionKey = key;
        picSplashGol.Image = _splashCelebracionImage;
    }

    private void OcultarControlesDetalleCelebracion()
    {
        foreach (var ctl in ControlesDetalleCelebracion())
            ctl.Visible = false;
    }

    private IEnumerable<Control> ControlesDetalleCelebracion()
    {
        yield return picGolFoto;
        yield return picGolEscudo;
        yield return lblGolNombre;
        yield return lblGolNumero;
        yield return lblGolEtiquetaPartido;
        yield return lblGolEtiquetaCamp;
        yield return lblGolPartidoValor;
        yield return lblGolCampValor;
    }

    private void AplicarVistaCelebracionJugador(CelebracionRow c)
    {
        pnlCelebracion.SuspendLayout();
        foreach (var ctl in ControlesDetalleCelebracion())
            ctl.Visible = true;
        RestaurarLayoutCelebracionJugadorPorDefecto();

        lblGolNombre.Text = FormatearNombreParaCelebracionJugador(c.Nombres);
        lblGolNumero.Text = string.IsNullOrEmpty(c.Numero) ? "-" : c.Numero;
        lblGolPartidoValor.Text = c.GolesPartido.ToString();
        lblGolCampValor.Text = c.GolesCampeonato.ToString();

        CargarLogo(picGolEscudo, c.EscudoUrl);
        CargarImagenControl(picGolFoto, c.FotoUrl);
        pnlCelebracion.ResumeLayout(false);
    }

    private void AplicarVistaCelebracionEquipoManual(CelebracionRow c)
    {
        pnlCelebracion.SuspendLayout();
        foreach (var ctl in ControlesDetalleCelebracion())
            ctl.Visible = false;
        picGolEscudo.Visible = true;
        lblGolNombre.Visible = true;

        var w = Math.Max(64, pnlCelebracion.ClientSize.Width);
        var h = Math.Max(64, pnlCelebracion.ClientSize.Height);
        AplicarGeometriaCelebracionDetalleManual(w, h, forzar: true);

        lblGolNombre.Text = c.Nombres ?? "";

        CargarLogo(picGolEscudo, c.EscudoUrl);
        picGolFoto.Image?.Dispose();
        picGolFoto.Image = null;
        pnlCelebracion.ResumeLayout(false);
    }

    /// <summary>
    /// Acorta el nombre para la celebración: 2 palabras → ambas; 3 o 4 → 1.ª y 3.ª; 5+ → texto completo.
    /// </summary>
    private static string FormatearNombreParaCelebracionJugador(string? nombresCompletos)
    {
        var raw = (nombresCompletos ?? "").Trim();
        if (raw.Length == 0)
            return "";
        try
        {
            var parts = raw.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            return parts.Length switch
            {
                1 => raw,
                2 => $"{parts[0]} {parts[1]}",
                3 => $"{parts[0]} {parts[2]}",
                4 => $"{parts[0]} {parts[2]}",
                _ => raw
            };
        }
        catch
        {
            return raw;
        }
    }

    /// <summary>Posiciones y fuentes de la celebración con jugador (automático), escaladas al cliente actual.</summary>
    private void RestaurarLayoutCelebracionJugadorPorDefecto()
    {
        var w = Math.Max(64, pnlCelebracion.ClientSize.Width);
        var h = Math.Max(64, pnlCelebracion.ClientSize.Height);
        AplicarGeometriaCelebracionDetalleJugador(w, h);
    }

    private void CargarLogo(PictureBox pic, string? origen)
    {
        if (string.IsNullOrEmpty(origen))
        {
            pic.Image?.Dispose();
            pic.Image = null;
            return;
        }

        try
        {
            Image? raw = null;
            if (origen.StartsWith("http", StringComparison.OrdinalIgnoreCase))
            {
                var bytes = HttpImages.GetByteArrayAsync(origen).GetAwaiter().GetResult();
                using var ms = new MemoryStream(bytes);
                raw = Image.FromStream(ms);
            }
            else if (File.Exists(origen))
            {
                raw = Image.FromFile(origen);
            }

            if (raw == null)
            {
                pic.Image?.Dispose();
                pic.Image = null;
                return;
            }

            using (raw)
            {
                var logo = QuitarFondoDesdeBordes(raw);
                pic.Image?.Dispose();
                pic.Image = logo;
            }
        }
        catch
        {
            pic.Image?.Dispose();
            pic.Image = null;
        }
    }

    /// <summary>
    /// Quita el color dominante del borde (blanco, negro, etc.) sin tocar huecos internos no conectados.
    /// </summary>
    private static Bitmap QuitarFondoDesdeBordes(Image source)
    {
        var bmp = new Bitmap(source.Width, source.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
        using (var g = Graphics.FromImage(bmp))
            g.DrawImage(source, 0, 0, source.Width, source.Height);

        var w = bmp.Width;
        var h = bmp.Height;
        var visit = new bool[w, h];
        var q = new Queue<(int X, int Y)>();
        var colorFondo = DetectarColorDominanteBorde(bmp);
        const int tolerancia = 26;

        void TryPush(int x, int y)
        {
            if (x < 0 || y < 0 || x >= w || y >= h || visit[x, y]) return;
            var c = bmp.GetPixel(x, y);
            if (!EsColorSimilar(c, colorFondo, tolerancia)) return;
            visit[x, y] = true;
            q.Enqueue((x, y));
        }

        for (var x = 0; x < w; x++) { TryPush(x, 0); TryPush(x, h - 1); }
        for (var y = 0; y < h; y++) { TryPush(0, y); TryPush(w - 1, y); }

        while (q.Count > 0)
        {
            var (x, y) = q.Dequeue();
            bmp.SetPixel(x, y, Color.FromArgb(0, 0, 0, 0));
            TryPush(x + 1, y);
            TryPush(x - 1, y);
            TryPush(x, y + 1);
            TryPush(x, y - 1);
        }

        return bmp;
    }

    private static Color DetectarColorDominanteBorde(Bitmap bmp)
    {
        var buckets = new Dictionary<int, int>();

        void Add(Color c)
        {
            var r = c.R / 16;
            var g = c.G / 16;
            var b = c.B / 16;
            var key = (r << 8) | (g << 4) | b;
            if (buckets.TryGetValue(key, out var n))
                buckets[key] = n + 1;
            else
                buckets[key] = 1;
        }

        var w = bmp.Width;
        var h = bmp.Height;
        for (var x = 0; x < w; x++) { Add(bmp.GetPixel(x, 0)); Add(bmp.GetPixel(x, h - 1)); }
        for (var y = 0; y < h; y++) { Add(bmp.GetPixel(0, y)); Add(bmp.GetPixel(w - 1, y)); }

        var best = 0;
        var max = -1;
        foreach (var kv in buckets)
        {
            if (kv.Value > max)
            {
                max = kv.Value;
                best = kv.Key;
            }
        }

        var rr = ((best >> 8) & 0xF) * 16 + 8;
        var gg = ((best >> 4) & 0xF) * 16 + 8;
        var bb = (best & 0xF) * 16 + 8;
        return Color.FromArgb(255, rr, gg, bb);
    }

    private static bool EsColorSimilar(Color c, Color objetivo, int tol)
    {
        if (c.A < 16) return true;
        return Math.Abs(c.R - objetivo.R) <= tol
            && Math.Abs(c.G - objetivo.G) <= tol
            && Math.Abs(c.B - objetivo.B) <= tol;
    }

    private void CargarImagenControl(PictureBox pic, string? origen)
    {
        if (string.IsNullOrEmpty(origen))
        {
            pic.Image?.Dispose();
            pic.Image = null;
            return;
        }

        try
        {
            if (origen.StartsWith("http", StringComparison.OrdinalIgnoreCase))
            {
                var bytes = HttpImages.GetByteArrayAsync(origen).GetAwaiter().GetResult();
                using var ms = new MemoryStream(bytes);
                var img = Image.FromStream(ms);
                pic.Image?.Dispose();
                pic.Image = img;
            }
            else if (File.Exists(origen))
            {
                pic.Image?.Dispose();
                pic.Image = Image.FromFile(origen);
            }
        }
        catch
        {
            pic.Image?.Dispose();
            pic.Image = null;
        }
    }

    private void Form1_DoubleClick(object? sender, EventArgs e) => Application.Exit();
}
