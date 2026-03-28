using System.Drawing.Text;
using System.Net.Http;
using System.Threading;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Marcador.Core;

namespace Marcador.Admin;

public partial class Form1 : Form
{
    private static readonly HttpClient HttpPreviewImagen = new() { Timeout = TimeSpan.FromSeconds(10) };

    private MarcadorDb _db = null!;
    private ApiClient _api = null!;
    private WebApplication? _signalRApp;
    private IHubContext<MarcadorHub>? _hubContext;
    private string? _logoLocalPath;
    private string? _logoVisitantePath;
    private Image? _previewLogoLocal;
    private Image? _previewLogoVisitante;
    private bool _sincronizacionInicialHecha;
    private readonly List<JugadorRow> _rosterLocal = new();
    private readonly List<JugadorRow> _rosterVisit = new();
    private MarcadorLayoutSnapshot _layoutEdicion = MarcadorLayoutSnapshot.CreateDefault();
    private bool _suppressLayoutPerfilCombo;
    private bool _suppressModoRadioPersist;

    public Form1()
    {
        InitializeComponent();
        RellenarItemsPeriodo(cmbPeriodoOperacion);
        RellenarItemsPeriodo(cmbSubtituloPeriodo);
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        Directory.CreateDirectory(Path.GetDirectoryName(Config.DbPath)!);
        _db = new MarcadorDb(Config.DbPath);
        _api = new ApiClient(Config.ApiBaseUrl);
        IniciarSignalR();
        radAuto.CheckedChanged += (_, _) => OnModoRadioChanged();
        radManual.CheckedChanged += (_, _) => OnModoRadioChanged();
        btnLogoLocal.Click += BtnLogoLocal_Click;
        btnLogoVisitante.Click += BtnLogoVisitante_Click;
        btnAplicarManual.Click += BtnAplicarManual_Click;
        timerRelojAdmin.Start();
        RefrescarComboLayoutPerfiles();
        _layoutEdicion = _db.GetMarcadorLayoutSnapshot();
        propertyGridLayout.SelectedObject = _layoutEdicion;
        RefrescarDisenadorConDatosDisplay();
        displayDesignerControl.LayoutChanged += (_, _) => propertyGridLayout.Refresh();
        propertyGridLayout.PropertyValueChanged += (_, _) => RefrescarDisenadorConDatosDisplay();
        tabPrincipal.SelectedIndexChanged += (_, _) => { if (tabPrincipal.SelectedTab == tabPageDiseno) RefrescarDisenadorConDatosDisplay(); };
        BeginInvoke(new Action(RefrescarDisenadorConDatosDisplay));
        AplicarModoDesdeBaseDatos();
        RefrescarUI();
        AplicarIconosBotones();
        BeginInvoke(new Action(() => _ = SincronizarAlArranqueAsync()));
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        timerRelojAdmin.Stop();
        LimpiarVistaPreviaLogosManual();
        _ = _signalRApp?.StopAsync();
        base.OnFormClosing(e);
    }

    private async Task SincronizarAlArranqueAsync()
    {
        if (_sincronizacionInicialHecha) return;
        try
        {
            await SincronizarDesdeApiCoreAsync();
            _sincronizacionInicialHecha = true;
            if (IsDisposed) return;
            BeginInvoke(() =>
            {
                RefrescarUI();
                Text = "Marcador LBG - Administrador (sincronizado al inicio)";
            });
        }
        catch
        {
            if (IsDisposed) return;
            BeginInvoke(() =>
            {
                Text = "Marcador LBG - Administrador (sincronización inicial falló — use el botón)";
            });
        }
    }

    private void IniciarSignalR()
    {
        try
        {
            var builder = WebApplication.CreateBuilder(new WebApplicationOptions { Args = new[] { $"--urls=http://localhost:{Config.SignalRPort}" } });
            builder.Services.AddSignalR();
            var app = builder.Build();
            app.MapHub<MarcadorHub>("/marcador");
            _hubContext = app.Services.GetRequiredService<IHubContext<MarcadorHub>>();
            _ = app.RunAsync();
            _signalRApp = app;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"No se pudo iniciar SignalR: {ex.Message}", "Aviso");
        }
    }

    /// <summary>Debe coincidir con la altura del <see cref="grpGoles"/> en el diseñador (listas + botones visibles).</summary>
    private const int AlturaGrpGolesAutomatico = 491;
    private const int AlturaGrpGolesManual = 178;

    private static bool EsModoManualEnBase(string? modo) =>
        string.Equals((modo ?? "").Trim(), "Manual", StringComparison.OrdinalIgnoreCase);

    private void AplicarModoDesdeBaseDatos()
    {
        var manual = EsModoManualEnBase(_db.GetPartidoActual()?.Modo);
        _suppressModoRadioPersist = true;
        try
        {
            radManual.Checked = manual;
            radAuto.Checked = !manual;
        }
        finally
        {
            _suppressModoRadioPersist = false;
        }
        ActualizarModo();
    }

    private void OnModoRadioChanged()
    {
        if (!_suppressModoRadioPersist)
            _db.SetSoloModoPartido(radManual.Checked ? "Manual" : "Auto");
        ActualizarModo();
    }

    private void ActualizarModo()
    {
        var manual = radManual.Checked;
        grpEquipos.Visible = !manual;
        grpManual.Visible = manual;

        var auto = !manual;
        lblComboJugadoresLocal.Visible = auto;
        cmbJugadorLocalGol.Visible = auto;
        btnAnadirGolLocalCombo.Visible = auto;
        lblComboJugadoresVisitante.Visible = auto;
        cmbJugadorVisitanteGol.Visible = auto;
        btnAnadirGolVisitanteCombo.Visible = auto;

        btnGolLocal.Visible = manual;
        btnGolVisitante.Visible = manual;
        btnMenosLocal.Visible = manual;
        btnMenosVisitante.Visible = manual;
        lblTextoGolManualLocal.Visible = manual;
        txtTextoGolManualLocal.Visible = manual;
        lblTextoGolManualVisitante.Visible = manual;
        txtTextoGolManualVisitante.Visible = manual;

        lblListaGolesLocal.Visible = auto;
        lblListaGolesVisitante.Visible = auto;
        lstGolesLocal.Visible = auto;
        lstGolesVisitante.Visible = auto;
        btnQuitarGolLocal.Visible = auto;
        btnQuitarGolVisitante.Visible = auto;

        grpGoles.Height = manual ? AlturaGrpGolesManual : AlturaGrpGolesAutomatico;

        lblAyudaListas.Text = manual
            ? "Modo manual: use +1 y − para el marcador. Las listas de goles no se usan en este modo."
            : "Modo automático: local a la izquierda, visitante a la derecha; Añadir gol y listas por equipo.";

        if (manual)
        {
            var p = _db.GetPartidoActual();
            txtNombreLocal.Text = p?.NombreLocal ?? "";
            txtNombreVisitante.Text = p?.NombreVisitante ?? "";
            _logoLocalPath = p?.LogoLocal;
            _logoVisitantePath = p?.LogoVisitante;
            ActualizarVistaPreviaLogosManual();
        }
        else
        {
            LimpiarVistaPreviaLogosManual();
        }

        RefrescarUI();
    }

    private static void RellenarComboJugadoresEquipo(ComboBox cmb, IReadOnlyList<JugadorRow> roster)
    {
        cmb.Items.Clear();
        foreach (var j in roster.OrderBy(x => x.Numero, StringComparer.OrdinalIgnoreCase))
            cmb.Items.Add(new JugadorListaItem(j.Identificacion, j.Nombres, j.Numero));
        cmb.SelectedIndex = cmb.Items.Count > 0 ? 0 : -1;
    }

    private async void BtnAnadirGolLocalCombo_Click(object? sender, EventArgs e)
    {
        if (radManual.Checked) return;
        if (cmbJugadorLocalGol.SelectedItem is not JugadorListaItem jug)
        {
            var p = _db.GetPartidoActual();
            var nombre = string.IsNullOrWhiteSpace(p?.NombreLocal) ? "local" : p.NombreLocal.Trim();
            MessageBox.Show($"Seleccione un jugador de {nombre}.", "Gol");
            return;
        }
        await RegistrarGolAsync(true, jug.Identificacion);
    }

    private async void BtnAnadirGolVisitanteCombo_Click(object? sender, EventArgs e)
    {
        if (radManual.Checked) return;
        if (cmbJugadorVisitanteGol.SelectedItem is not JugadorListaItem jug)
        {
            var p = _db.GetPartidoActual();
            var nombre = string.IsNullOrWhiteSpace(p?.NombreVisitante) ? "visitante" : p.NombreVisitante.Trim();
            MessageBox.Show($"Seleccione un jugador de {nombre}.", "Gol");
            return;
        }
        await RegistrarGolAsync(false, jug.Identificacion);
    }

    private async void BtnQuitarGolLocal_Click(object? sender, EventArgs e)
    {
        if (lstGolesLocal.SelectedItem is not GolPartidoListItem item)
        {
            MessageBox.Show("Seleccione un gol en la lista de este equipo.", "Quitar");
            return;
        }
        _db.EliminarGolPorId(item.Id);
        await NotifyDisplayAsync();
        RefrescarUI();
    }

    private async void BtnQuitarGolVisitante_Click(object? sender, EventArgs e)
    {
        if (lstGolesVisitante.SelectedItem is not GolPartidoListItem item)
        {
            MessageBox.Show("Seleccione un gol en la lista de este equipo.", "Quitar");
            return;
        }
        _db.EliminarGolPorId(item.Id);
        await NotifyDisplayAsync();
        RefrescarUI();
    }

    private void RefrescarUI()
    {
        var estado = _db.GetEstado();
        lblMarcador.Text = $"{estado.GolesLocal} - {estado.GolesVisitante}";
        txtTituloLiga.Text = estado.TituloLiga ?? "";
        txtEtapaMarquee.Text = estado.TextoMarquee ?? "";
        SeleccionarPeriodoEnCombo(cmbPeriodoOperacion, estado.SubtituloPeriodo);
        SeleccionarPeriodoEnCombo(cmbSubtituloPeriodo, estado.SubtituloPeriodo);
        ActualizarEtiquetaCronometro();

        _rosterLocal.Clear();
        _rosterVisit.Clear();
        var partido = _db.GetPartidoActual();
        if (partido != null)
        {
            if (partido.IdEquipoLocal > 0)
                _rosterLocal.AddRange(_db.GetJugadoresPorEquipo(partido.IdEquipoLocal));
            if (partido.IdEquipoVisitante > 0)
                _rosterVisit.AddRange(_db.GetJugadoresPorEquipo(partido.IdEquipoVisitante));
        }

        if (radAuto.Checked)
        {
            RellenarComboJugadoresEquipo(cmbJugadorLocalGol, _rosterLocal);
            RellenarComboJugadoresEquipo(cmbJugadorVisitanteGol, _rosterVisit);
        }

        var nomLoc = string.IsNullOrWhiteSpace(partido?.NombreLocal) ? "Local" : partido!.NombreLocal.Trim();
        var nomVis = string.IsNullOrWhiteSpace(partido?.NombreVisitante) ? "Visitante" : partido!.NombreVisitante.Trim();
        if (tabPrincipal.SelectedTab == tabPageDiseno && !displayDesignerControl.IsDragging)
            RefrescarDisenadorConDatosDisplay();
        lblComboJugadoresLocal.Text = $"{nomLoc} — jugadores:";
        lblComboJugadoresVisitante.Text = $"{nomVis} — jugadores:";
        lblListaGolesLocal.Text = $"Goles — {nomLoc}";
        lblListaGolesVisitante.Text = $"Goles — {nomVis}";

        lstGolesLocal.Items.Clear();
        lstGolesVisitante.Items.Clear();
        foreach (var g in _db.GetGolesPartido())
        {
            string det;
            if (string.IsNullOrWhiteSpace(g.IdentificacionJugador))
                det = "(sin jugador)";
            else
            {
                var num = string.IsNullOrWhiteSpace(g.Numero) ? "" : $"#{g.Numero} ";
                var nom = string.IsNullOrWhiteSpace(g.Nombres) ? g.IdentificacionJugador : g.Nombres;
                det = num + nom;
            }
            var item = new GolPartidoListItem(g.Id, det);
            if (g.EquipoLocal == 1)
                lstGolesLocal.Items.Add(item);
            else
                lstGolesVisitante.Items.Add(item);
        }
    }

    private async Task RegistrarGolAsync(bool esLocal, string? identificacionJugador, string? textoCelebracionManual = null)
    {
        _db.AgregarGol(esLocal ? 1 : 0, identificacionJugador, null);
        _db.RecalcularMarcadorDesdeGoles();
        if (string.IsNullOrWhiteSpace(identificacionJugador))
            ActivarCelebracionEquipoManual(esLocal, textoCelebracionManual);
        else
            ActivarCelebracionSiJugador(identificacionJugador, esLocal);
        await NotifyDisplayAsync();
        RefrescarUI();
    }

    private void TimerRelojAdmin_Tick(object? sender, EventArgs e) => ActualizarEtiquetaCronometro();

    private void ActualizarEtiquetaCronometro()
    {
        var elapsed = _db.GetCronometroElapsedSeconds();
        var ts = TimeSpan.FromSeconds(elapsed);
        lblCronometroAdmin.Text = $"{(int)ts.TotalMinutes:00}:{ts.Seconds:00}";
    }

    private void BtnGuardarCabecera_Click(object? sender, EventArgs e)
    {
        var cod = CodigoPeriodoSeleccionado(cmbSubtituloPeriodo);
        _db.SetTextosCabecera(txtTituloLiga.Text.Trim(), txtEtapaMarquee.Text.Trim(), cod);
        SeleccionarPeriodoEnCombo(cmbPeriodoOperacion, cod);
        _ = NotifyDisplayAsync();
    }

    private async void BtnEnviarPeriodoCron_Click(object? sender, EventArgs e)
    {
        var est = _db.GetEstado();
        var cod = CodigoPeriodoSeleccionado(cmbPeriodoOperacion);
        _db.SetTextosCabecera(est.TituloLiga ?? "", est.TextoMarquee ?? "", cod);
        SeleccionarPeriodoEnCombo(cmbSubtituloPeriodo, cod);
        await NotifyDisplayAsync();
    }

    private async void BtnCronIniciar_Click(object? sender, EventArgs e)
    {
        var est = _db.GetEstado();
        var cod = CodigoPeriodoSeleccionado(cmbPeriodoOperacion);
        _db.SetTextosCabecera(est.TituloLiga ?? "", est.TextoMarquee ?? "", cod);
        SeleccionarPeriodoEnCombo(cmbSubtituloPeriodo, cod);
        var actual = _db.GetCronometroElapsedSeconds();
        _db.CronometroStart(actual);
        await NotifyDisplayAsync();
        ActualizarEtiquetaCronometro();
    }

    private async void BtnCronReset_Click(object? sender, EventArgs e)
    {
        const string msg = "El cronómetro volverá a 00:00 y quedará detenido.\nEl marcador de goles no cambia.\n\n¿Desea continuar?";
        if (!ConfirmacionAccionForm.Mostrar(this, "Marcador LBG", "Detener cronómetro", msg, "Sí, detener"))
            return;
        _db.CronometroReset();
        await NotifyDisplayAsync();
        ActualizarEtiquetaCronometro();
    }

    private async Task SincronizarDesdeApiCoreAsync()
    {
        var equipos = await _api.GetEquiposAsync();
        _db.SyncEquipos(equipos.Select(e => (e.Id, e.Nombre, e.NombreCorto, e.Logo)));

        var jugadores = await _api.GetJugadoresCampeonatoAsync();
        _db.SyncJugadores(DateTime.Now.Year.ToString(), jugadores.Select(j => (j.Identificacion, j.Nombres, j.Numero, j.Equipo, j.Foto)));

        if (IsDisposed) return;
        void Rellenar()
        {
            var partido = _db.GetPartidoActual();
            cmbLocal.Items.Clear();
            cmbVisitante.Items.Clear();
            foreach (var eq in _db.GetEquipos())
            {
                cmbLocal.Items.Add(new EquipoComboItem(eq.Id, eq.NombreCorto));
                cmbVisitante.Items.Add(new EquipoComboItem(eq.Id, eq.NombreCorto));
            }
            SeleccionarEquiposGuardadosEnCombos(partido?.IdEquipoLocal ?? 0, partido?.IdEquipoVisitante ?? 0);
        }
        if (InvokeRequired)
            Invoke(Rellenar);
        else
            Rellenar();
    }

    private void SeleccionarEquiposGuardadosEnCombos(long idLocal, long idVisitante)
    {
        var idxLocal = -1;
        var idxVisit = -1;
        for (var i = 0; i < cmbLocal.Items.Count; i++)
        {
            if (cmbLocal.Items[i] is EquipoComboItem a && a.Id == idLocal)
                idxLocal = i;
            if (cmbVisitante.Items[i] is EquipoComboItem b && b.Id == idVisitante)
                idxVisit = i;
        }
        cmbLocal.SelectedIndex = idxLocal >= 0 ? idxLocal : (cmbLocal.Items.Count > 0 ? 0 : -1);
        cmbVisitante.SelectedIndex = idxVisit >= 0 ? idxVisit : (cmbVisitante.Items.Count > 0 ? 0 : -1);
    }

    private async void BtnSincronizar_Click(object? sender, EventArgs e)
    {
        btnSincronizar.Enabled = false;
        try
        {
            await SincronizarDesdeApiCoreAsync();
            _sincronizacionInicialHecha = true;
            RefrescarUI();
            Text = "Marcador LBG - Administrador";
            MessageBox.Show("Sincronización completada (equipos y jugadores).", "API");
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al sincronizar: {ex.Message}", "Error");
        }
        finally
        {
            btnSincronizar.Enabled = true;
        }
    }

    private async void BtnAplicarEquipos_Click(object? sender, EventArgs e)
    {
        var loc = (cmbLocal.SelectedItem as EquipoComboItem)?.Id ?? 0;
        var vis = (cmbVisitante.SelectedItem as EquipoComboItem)?.Id ?? 0;
        var equipos = _db.GetEquipos();
        var eqLoc = equipos.FirstOrDefault(e => e.Id == loc);
        var eqVis = equipos.FirstOrDefault(e => e.Id == vis);
        var nomL = eqLoc?.NombreCorto ?? "(local)";
        var nomV = eqVis?.NombreCorto ?? "(visitante)";
        var msg =
            $"Se aplicarán al partido:\n• Local: {nomL}\n• Visitante: {nomV}\n\n" +
            "Se reiniciará el partido:\n• Marcador a 0 - 0\n• Cronómetro a 00:00 (detenido)\n• Lista de goles vacía\n\n¿Desea continuar?";
        if (!ConfirmacionAccionForm.Mostrar(this, "Marcador LBG", "Aplicar equipos al partido", msg, "Sí, aplicar"))
            return;

        _db.SetPartidoEquipos(loc, vis, eqLoc?.NombreCorto, eqVis?.NombreCorto, eqLoc?.Logo, eqVis?.Logo);
        _db.ReiniciarPartidoEnJuego();
        await NotifyDisplayAsync();
        RefrescarUI();
    }

    private async void BtnGolLocal_Click(object? sender, EventArgs e)
    {
        await RegistrarGolAsync(esLocal: true, identificacionJugador: null, textoCelebracionManual: txtTextoGolManualLocal.Text);
    }

    private async void BtnGolVisitante_Click(object? sender, EventArgs e)
    {
        await RegistrarGolAsync(esLocal: false, identificacionJugador: null, textoCelebracionManual: txtTextoGolManualVisitante.Text);
    }

    private void ActivarCelebracionSiJugador(string? identificacion, bool esLocal)
    {
        if (string.IsNullOrWhiteSpace(identificacion))
            return;
        var j = _db.GetJugador(identificacion);
        if (j == null)
            return;
        var partido = _db.GetPartidoActual();
        string? escudoUrl = ResolverEscudoGol(partido, j, esLocal);
        long golesPartido = _db.CountGolesJugadorEnPartido(identificacion);
        long golesCampeonato = golesPartido;
        var (splash, detalle) = LeerDuracionesCelebracionLayout();
        _db.ActivarCelebracionGol(j.Identificacion, j.Nombres, j.Numero, j.Foto, escudoUrl, golesPartido, golesCampeonato, esManual: false, splash, detalle);
    }

    /// <param name="textoGoleadorOpcional">Texto extra bajo el escudo (ej. nombre del goleador). Se guarda en Identificacion solo en modo manual.</param>
    private void ActivarCelebracionEquipoManual(bool esLocal, string? textoGoleadorOpcional)
    {
        var partido = _db.GetPartidoActual();
        var nom = esLocal ? partido?.NombreLocal?.Trim() : partido?.NombreVisitante?.Trim();
        if (string.IsNullOrEmpty(nom))
            nom = esLocal ? "Local" : "Visitante";
        var logo = esLocal ? partido?.LogoLocal : partido?.LogoVisitante;
        var est = _db.GetEstado();
        var golesEq = esLocal ? est.GolesLocal : est.GolesVisitante;
        var (splash, detalle) = LeerDuracionesCelebracionLayout();
        var extra = (textoGoleadorOpcional ?? "").Trim();
        _db.ActivarCelebracionGol(extra, nom, "", "", logo ?? "", golesEq, 0, esManual: true, splash, detalle);
    }

    private static bool RutaImagenCelebracionIntermediaValida(string? path)
    {
        var p = (path ?? "").Trim();
        if (string.IsNullOrEmpty(p)) return false;
        if (p.StartsWith("http", StringComparison.OrdinalIgnoreCase)) return true;
        return File.Exists(p);
    }

    private (int splash, int detalle) LeerDuracionesCelebracionLayout()
    {
        var lay = _db.GetMarcadorLayoutSnapshot();
        var splash = RutaImagenCelebracionIntermediaValida(lay.Celebracion_ImagenIntermediaPath)
            ? Math.Max(0, lay.Celebracion_SplashSegundos)
            : 0;
        var detalle = Math.Max(1, lay.Celebracion_DetalleSegundos);
        return (splash, detalle);
    }

    private static string? ResolverEscudoGol(PartidoRow? partido, JugadorRow j, bool esLocal)
    {
        if (partido == null)
            return null;
        if (partido.IdEquipoLocal > 0 || partido.IdEquipoVisitante > 0)
        {
            if (j.Equipo == partido.IdEquipoLocal)
                return string.IsNullOrEmpty(partido.LogoLocal) ? null : partido.LogoLocal;
            if (j.Equipo == partido.IdEquipoVisitante)
                return string.IsNullOrEmpty(partido.LogoVisitante) ? null : partido.LogoVisitante;
        }
        return esLocal ? (string.IsNullOrEmpty(partido.LogoLocal) ? null : partido.LogoLocal) : (string.IsNullOrEmpty(partido.LogoVisitante) ? null : partido.LogoVisitante);
    }

    private async void BtnMenosLocal_Click(object? sender, EventArgs e)
    {
        var id = _db.ObtenerUltimoGolIdEquipo(true);
        if (id.HasValue)
            _db.EliminarGolPorId(id.Value);
        else
        {
            var e0 = _db.GetEstado();
            if (e0.GolesLocal > 0)
                _db.SetGoles(e0.GolesLocal - 1, e0.GolesVisitante);
        }
        await NotifyDisplayAsync();
        RefrescarUI();
    }

    private async void BtnMenosVisitante_Click(object? sender, EventArgs e)
    {
        var id = _db.ObtenerUltimoGolIdEquipo(false);
        if (id.HasValue)
            _db.EliminarGolPorId(id.Value);
        else
        {
            var e0 = _db.GetEstado();
            if (e0.GolesVisitante > 0)
                _db.SetGoles(e0.GolesLocal, e0.GolesVisitante - 1);
        }
        await NotifyDisplayAsync();
        RefrescarUI();
    }

    private void LimpiarVistaPreviaLogosManual()
    {
        picPreviewLogoLocal.Image = null;
        picPreviewLogoVisitante.Image = null;
        _previewLogoLocal?.Dispose();
        _previewLogoVisitante?.Dispose();
        _previewLogoLocal = null;
        _previewLogoVisitante = null;
    }

    private void ActualizarVistaPreviaLogosManual()
    {
        if (!radManual.Checked)
            return;
        AsignarImagenPreview(ref _previewLogoLocal, picPreviewLogoLocal, _logoLocalPath);
        AsignarImagenPreview(ref _previewLogoVisitante, picPreviewLogoVisitante, _logoVisitantePath);
    }

    private static void AsignarImagenPreview(ref Image? cache, PictureBox pic, string? pathOrUrl)
    {
        pic.Image = null;
        cache?.Dispose();
        cache = null;
        var s = (pathOrUrl ?? "").Trim();
        if (s.Length == 0)
            return;
        try
        {
            Image? img = null;
            if (s.StartsWith("http", StringComparison.OrdinalIgnoreCase))
            {
                var bytes = HttpPreviewImagen.GetByteArrayAsync(s).GetAwaiter().GetResult();
                using var ms = new MemoryStream(bytes);
                img = Image.FromStream(ms);
            }
            else if (File.Exists(s))
            {
                img = ImagenArchivoSinBloqueo.CrearDesdeArchivo(s);
            }

            if (img == null)
                return;
            cache = img;
            pic.Image = img;
        }
        catch
        {
            cache?.Dispose();
            cache = null;
        }
    }

    private static void CopiarArchivoLogoConReintentos(string origen, string destino)
    {
        const int maxIntentos = 5;
        for (var i = 1; i <= maxIntentos; i++)
        {
            try
            {
                File.Copy(origen, destino, true);
                return;
            }
            catch (IOException) when (i < maxIntentos)
            {
                Thread.Sleep(100);
            }
        }
    }

    private void BtnLogoLocal_Click(object? sender, EventArgs e)
    {
        using var ofd = new OpenFileDialog { Filter = "Imágenes|*.png;*.jpg;*.jpeg" };
        if (ofd.ShowDialog() == DialogResult.OK)
        {
            var dest = Path.Combine(Path.GetDirectoryName(Config.DbPath)!, "logo_local" + Path.GetExtension(ofd.FileName));
            CopiarArchivoLogoConReintentos(ofd.FileName, dest);
            _logoLocalPath = dest;
            ActualizarVistaPreviaLogosManual();
            if (radManual.Checked)
                AplicarSoloNombresLogosManual();
            else
            {
                var partido = _db.GetPartidoActual();
                _db.SetPartidoEquipos(partido?.IdEquipoLocal ?? 0, partido?.IdEquipoVisitante ?? 0, txtNombreLocal.Text, txtNombreVisitante.Text, dest, partido?.LogoVisitante);
                _ = NotifyDisplayAsync();
            }
        }
    }

    private void BtnLogoVisitante_Click(object? sender, EventArgs e)
    {
        using var ofd = new OpenFileDialog { Filter = "Imágenes|*.png;*.jpg;*.jpeg" };
        if (ofd.ShowDialog() == DialogResult.OK)
        {
            var dest = Path.Combine(Path.GetDirectoryName(Config.DbPath)!, "logo_visitante" + Path.GetExtension(ofd.FileName));
            CopiarArchivoLogoConReintentos(ofd.FileName, dest);
            _logoVisitantePath = dest;
            ActualizarVistaPreviaLogosManual();
            if (radManual.Checked)
                AplicarSoloNombresLogosManual();
            else
            {
                var partido = _db.GetPartidoActual();
                _db.SetPartidoEquipos(partido?.IdEquipoLocal ?? 0, partido?.IdEquipoVisitante ?? 0, txtNombreLocal.Text, txtNombreVisitante.Text, partido?.LogoLocal, dest);
                _ = NotifyDisplayAsync();
            }
        }
    }

    private async void BtnAplicarManual_Click(object? sender, EventArgs e)
    {
        var loc = string.IsNullOrWhiteSpace(txtNombreLocal.Text) ? "(sin nombre local)" : txtNombreLocal.Text.Trim();
        var vis = string.IsNullOrWhiteSpace(txtNombreVisitante.Text) ? "(sin nombre visitante)" : txtNombreVisitante.Text.Trim();
        var msg =
            $"Se aplicarán en modo manual:\n• Local: {loc}\n• Visitante: {vis}\n\n" +
            "Se reiniciará el partido:\n• Marcador a 0 - 0\n• Cronómetro a 00:00 (detenido)\n• Tabla de goles vacía (coherente con el marcador)\n\n¿Desea continuar?";
        if (!ConfirmacionAccionForm.Mostrar(this, "Marcador LBG", "Aplicar nombres y logos", msg, "Sí, aplicar"))
            return;

        _db.SetModoManual(txtNombreLocal.Text, txtNombreVisitante.Text, _logoLocalPath, _logoVisitantePath);
        _db.ReiniciarPartidoEnJuego();
        await NotifyDisplayAsync();
        RefrescarUI();
        ActualizarVistaPreviaLogosManual();
    }

    /// <summary>Actualiza nombres/logos en BD sin reiniciar marcador ni tiempo (p. ej. tras elegir logo).</summary>
    private void AplicarSoloNombresLogosManual()
    {
        _db.SetModoManual(txtNombreLocal.Text, txtNombreVisitante.Text, _logoLocalPath, _logoVisitantePath);
        _ = NotifyDisplayAsync();
        RefrescarUI();
    }

    private async void BtnGuardarLayoutMarcador_Click(object? sender, EventArgs e)
    {
        try
        {
            propertyGridLayout.Refresh();
            if (_layoutEdicion.ClientWidth < 80 || _layoutEdicion.ClientHeight < 80)
            {
                MessageBox.Show("El tamaño de ventana es demasiado pequeño (mín. 80×80).", "Diseño");
                return;
            }
            _db.SetMarcadorLayout(_layoutEdicion);
            await NotifyDisplayAsync();
            var perfil = _db.GetActiveMarcadorLayoutPresetName();
            MessageBox.Show($"Diseño guardado en el perfil «{perfil}». El marcador lo aplica al instante.", "Diseño");
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Diseño");
        }
    }

    private void BtnLayoutPredeterminado_Click(object? sender, EventArgs e)
    {
        _layoutEdicion = MarcadorLayoutSnapshot.CreateDefault();
        propertyGridLayout.SelectedObject = null;
        propertyGridLayout.SelectedObject = _layoutEdicion;
        RefrescarDisenadorConDatosDisplay();
    }

    private void RefrescarDisenadorConDatosDisplay()
    {
        try
        {
            var estado = _db.GetEstado();
            var partido = _db.GetPartidoActual();
            var datos = new DisplayDatosPreview(
                estado.TituloLiga ?? "",
                estado.TextoMarquee ?? "",
                estado.SubtituloPeriodo ?? "",
                partido?.NombreLocal ?? "",
                partido?.NombreVisitante ?? "",
                partido?.LogoLocal ?? _logoLocalPath,
                partido?.LogoVisitante ?? _logoVisitantePath,
                estado.GolesLocal,
                estado.GolesVisitante,
                "00:00"
            );
            displayDesignerControl.AplicarLayout(_layoutEdicion, datos);
        }
        catch
        {
            displayDesignerControl.AplicarLayout(_layoutEdicion, null);
        }
    }

    private void RefrescarComboLayoutPerfiles()
    {
        _suppressLayoutPerfilCombo = true;
        try
        {
            var act = _db.GetActiveMarcadorLayoutPresetName();
            cmbLayoutPerfiles.Items.Clear();
            foreach (var n in _db.GetMarcadorLayoutPresetNames())
                cmbLayoutPerfiles.Items.Add(n);
            var idx = -1;
            for (var i = 0; i < cmbLayoutPerfiles.Items.Count; i++)
            {
                if (string.Equals(cmbLayoutPerfiles.Items[i]?.ToString(), act, StringComparison.OrdinalIgnoreCase))
                {
                    idx = i;
                    break;
                }
            }
            if (idx >= 0)
                cmbLayoutPerfiles.SelectedIndex = idx;
            else if (cmbLayoutPerfiles.Items.Count > 0)
                cmbLayoutPerfiles.SelectedIndex = 0;
        }
        finally
        {
            _suppressLayoutPerfilCombo = false;
        }
    }

    private async void CmbLayoutPerfiles_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (_suppressLayoutPerfilCombo || cmbLayoutPerfiles.SelectedItem is not string nombre)
            return;
        try
        {
            _db.SetActiveMarcadorLayoutPreset(nombre);
            _layoutEdicion = _db.GetMarcadorLayoutSnapshot();
            propertyGridLayout.SelectedObject = null;
            propertyGridLayout.SelectedObject = _layoutEdicion;
            RefrescarDisenadorConDatosDisplay();
            await NotifyDisplayAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Perfil de diseño");
        }
    }

    private async void BtnLayoutGuardarComo_Click(object? sender, EventArgs e)
    {
        try
        {
            propertyGridLayout.Refresh();
            var nombre = (txtLayoutPerfilNombre.Text ?? "").Trim();
            if (nombre.Length == 0)
            {
                MessageBox.Show("Escriba un nombre para el nuevo perfil (ej. AMISTOSO).", "Guardar como");
                return;
            }
            if (_layoutEdicion.ClientWidth < 80 || _layoutEdicion.ClientHeight < 80)
            {
                MessageBox.Show("El tamaño de ventana es demasiado pequeño (mín. 80×80).", "Diseño");
                return;
            }
            _db.SaveMarcadorLayoutPreset(nombre, _layoutEdicion);
            _db.SetActiveMarcadorLayoutPreset(nombre);
            RefrescarComboLayoutPerfiles();
            txtLayoutPerfilNombre.Clear();
            await NotifyDisplayAsync();
            MessageBox.Show($"Perfil «{nombre}» guardado y activado.", "Diseño");
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Guardar como");
        }
    }

    private async void BtnLayoutEliminarPerfil_Click(object? sender, EventArgs e)
    {
        if (cmbLayoutPerfiles.SelectedItem is not string nombre)
        {
            MessageBox.Show("Seleccione un perfil en la lista.", "Eliminar");
            return;
        }
        if (MessageBox.Show($"¿Eliminar el perfil «{nombre}»?", "Eliminar perfil", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
            return;
        try
        {
            _db.DeleteMarcadorLayoutPreset(nombre);
            RefrescarComboLayoutPerfiles();
            _layoutEdicion = _db.GetMarcadorLayoutSnapshot();
            propertyGridLayout.SelectedObject = null;
            propertyGridLayout.SelectedObject = _layoutEdicion;
            RefrescarDisenadorConDatosDisplay();
            await NotifyDisplayAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Eliminar perfil");
        }
    }

    private void BtnElegirFondo_Click(object? sender, EventArgs e)
    {
        using var ofd = new OpenFileDialog { Filter = "Imágenes|*.png;*.jpg;*.jpeg;*.bmp;*.webp" };
        if (ofd.ShowDialog() != DialogResult.OK)
            return;
        _layoutEdicion.Fondo_ImagenPath = ofd.FileName;
        _layoutEdicion.Fondo_UsarImagen = true;
        propertyGridLayout.Refresh();
        RefrescarDisenadorConDatosDisplay();
    }

    private void BtnQuitarFondo_Click(object? sender, EventArgs e)
    {
        _layoutEdicion.Fondo_UsarImagen = false;
        _layoutEdicion.Fondo_ImagenPath = "";
        propertyGridLayout.Refresh();
        RefrescarDisenadorConDatosDisplay();
    }

    private void BtnElegirImagenGolIntermedia_Click(object? sender, EventArgs e)
    {
        using var ofd = new OpenFileDialog { Filter = "Imágenes|*.png;*.jpg;*.jpeg;*.bmp;*.webp" };
        if (ofd.ShowDialog() != DialogResult.OK)
            return;
        _layoutEdicion.Celebracion_ImagenIntermediaPath = ofd.FileName;
        propertyGridLayout.Refresh();
        RefrescarDisenadorConDatosDisplay();
    }

    private void BtnQuitarImagenGolIntermedia_Click(object? sender, EventArgs e)
    {
        _layoutEdicion.Celebracion_ImagenIntermediaPath = "";
        propertyGridLayout.Refresh();
        RefrescarDisenadorConDatosDisplay();
    }

    private void BtnLayoutExportarArchivo_Click(object? sender, EventArgs e)
    {
        try
        {
            propertyGridLayout.Refresh();
            using var sfd = new SaveFileDialog
            {
                Filter = "Diseño marcador JSON (*.json)|*.json|Todos los archivos|*.*",
                DefaultExt = "json",
                FileName = $"marcador-diseno-{DateTime.Now:yyyyMMdd-HHmm}.json",
                Title = "Exportar diseño"
            };
            if (sfd.ShowDialog() != DialogResult.OK)
                return;
            var json = MarcadorLayoutSnapshot.ToJson(_layoutEdicion);
            File.WriteAllText(sfd.FileName, json);
            MessageBox.Show($"Se guardó una copia del diseño en:\n{sfd.FileName}", "Exportar diseño");
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Exportar diseño");
        }
    }

    private void BtnLayoutImportarArchivo_Click(object? sender, EventArgs e)
    {
        try
        {
            using var ofd = new OpenFileDialog
            {
                Filter = "JSON (*.json)|*.json|Todos los archivos|*.*",
                Title = "Importar diseño"
            };
            if (ofd.ShowDialog() != DialogResult.OK)
                return;
            var json = File.ReadAllText(ofd.FileName);
            var snap = MarcadorLayoutSnapshot.FromJson(json);
            if (snap == null)
            {
                MessageBox.Show("El archivo no contiene un diseño válido (JSON incorrecto).", "Importar diseño");
                return;
            }
            if (snap.ClientWidth < 80 || snap.ClientHeight < 80)
            {
                MessageBox.Show("El diseño importado tiene un tamaño de ventana inválido (mín. 80×80).", "Importar diseño");
                return;
            }
            _layoutEdicion = snap;
            propertyGridLayout.SelectedObject = null;
            propertyGridLayout.SelectedObject = _layoutEdicion;
            propertyGridLayout.Refresh();
            RefrescarDisenadorConDatosDisplay();
            MessageBox.Show(
                "Diseño cargado en el editor (vista previa y cuadro de propiedades).\n\n" +
                "Pulse «Guardar diseño» para guardarlo en el perfil activo de la base de datos y enviarlo al marcador en pantalla.",
                "Importar diseño");
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Importar diseño");
        }
    }

    private async Task NotifyDisplayAsync()
    {
        try
        {
            if (_hubContext != null)
                await _hubContext.Clients.All.SendAsync("EstadoActualizado");
        }
        catch { }
    }

    private sealed record PeriodoCodigoItem(string Codigo, string Etiqueta)
    {
        public override string ToString() => Etiqueta;
    }

    private static void RellenarItemsPeriodo(ComboBox cmb)
    {
        cmb.Items.Clear();
        cmb.Items.Add(new PeriodoCodigoItem("PT", "PRIMER TIEMPO"));
        cmb.Items.Add(new PeriodoCodigoItem("ST", "SEGUNDO TIEMPO"));
    }

    private static string NormalizarCodigoPeriodoDesdeBd(string? guardado)
    {
        var s = (guardado ?? "").Trim();
        if (string.Equals(s, "ST", StringComparison.OrdinalIgnoreCase)) return "ST";
        if (string.Equals(s, "PT", StringComparison.OrdinalIgnoreCase)) return "PT";
        if (s.Contains("SEGUNDO", StringComparison.OrdinalIgnoreCase) || s.Contains("2º", StringComparison.OrdinalIgnoreCase))
            return "ST";
        return "PT";
    }

    private static void SeleccionarPeriodoEnCombo(ComboBox cmb, string? subtituloBd)
    {
        var cod = NormalizarCodigoPeriodoDesdeBd(subtituloBd);
        for (var i = 0; i < cmb.Items.Count; i++)
        {
            if (cmb.Items[i] is PeriodoCodigoItem p && p.Codigo == cod)
            {
                cmb.SelectedIndex = i;
                return;
            }
        }
        if (cmb.Items.Count > 0)
            cmb.SelectedIndex = 0;
    }

    private static string CodigoPeriodoSeleccionado(ComboBox cmb) =>
        cmb.SelectedItem is PeriodoCodigoItem p ? p.Codigo : "PT";

    private static bool FuenteMdl2Disponible()
    {
        try
        {
            using var _ = new Font("Segoe MDL2 Assets", 9f, FontStyle.Regular, GraphicsUnit.Point);
            return true;
        }
        catch
        {
            return false;
        }
    }

    private static Image? IconoGlyphMdl2(string glyph, int px)
    {
        var bmp = new Bitmap(px, px);
        try
        {
            using var g = Graphics.FromImage(bmp);
            g.Clear(Color.Transparent);
            g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            using var font = new Font("Segoe MDL2 Assets", px * 0.58f, FontStyle.Regular, GraphicsUnit.Pixel);
            using var brush = new SolidBrush(SystemColors.ControlText);
            var sz = g.MeasureString(glyph, font);
            g.DrawString(glyph, font, brush, (px - sz.Width) / 2f, (px - sz.Height) / 2f);
        }
        catch
        {
            bmp.Dispose();
            return null;
        }
        return bmp;
    }

    private void AsignarIconoBoton(Button b, string glyph, int px = 18)
    {
        if (!FuenteMdl2Disponible())
            return;
        var img = IconoGlyphMdl2(glyph, px);
        if (img == null)
            return;
        b.Image?.Dispose();
        b.Image = img;
        b.TextImageRelation = TextImageRelation.ImageBeforeText;
        b.ImageAlign = ContentAlignment.MiddleLeft;
    }

    private void AplicarIconosBotones()
    {
        if (!FuenteMdl2Disponible())
            return;
        AsignarIconoBoton(btnSincronizar, "\uE72C");
        AsignarIconoBoton(btnAplicarEquipos, "\uE930");
        AsignarIconoBoton(btnAplicarManual, "\uE930");
        AsignarIconoBoton(btnEnviarPeriodoCron, "\uE74E");
        AsignarIconoBoton(btnCronIniciar, "\uE768");
        AsignarIconoBoton(btnCronReset, "\uE71A");
        AsignarIconoBoton(btnAnadirGolLocalCombo, "\uE948");
        AsignarIconoBoton(btnAnadirGolVisitanteCombo, "\uE948");
        AsignarIconoBoton(btnQuitarGolLocal, "\uE738");
        AsignarIconoBoton(btnQuitarGolVisitante, "\uE738");
        AsignarIconoBoton(btnGolLocal, "\uE948", 16);
        AsignarIconoBoton(btnGolVisitante, "\uE948", 16);
        AsignarIconoBoton(btnMenosLocal, "\uE738", 16);
        AsignarIconoBoton(btnMenosVisitante, "\uE738", 16);
        AsignarIconoBoton(btnGuardarCabecera, "\uE74E");
        AsignarIconoBoton(btnGuardarLayoutMarcador, "\uE74E");
        AsignarIconoBoton(btnLayoutPredeterminado, "\uE777");
        AsignarIconoBoton(btnLayoutGuardarComo, "\uE74E");
        AsignarIconoBoton(btnLayoutEliminarPerfil, "\uE738");
        AsignarIconoBoton(btnElegirFondo, "\uE722");
        AsignarIconoBoton(btnQuitarFondo, "\uE711");
        AsignarIconoBoton(btnElegirImagenGolIntermedia, "\uE722");
        AsignarIconoBoton(btnQuitarImagenGolIntermedia, "\uE711");
        AsignarIconoBoton(btnLayoutExportarArchivo, "\uEDE1");
        AsignarIconoBoton(btnLayoutImportarArchivo, "\uE838");
    }

    private sealed record EquipoComboItem(long Id, string Display)
    {
        public override string ToString() => Display;
    }

    private sealed record JugadorListaItem(string Identificacion, string Nombres, string Numero)
    {
        public override string ToString() => $"{Numero} {Nombres}";
    }

    private sealed class GolPartidoListItem(long id, string linea)
    {
        public long Id { get; } = id;
        public override string ToString() => linea;
    }
}
