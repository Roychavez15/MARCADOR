namespace Marcador.Admin;

partial class Form1
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
            components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        timerRelojAdmin = new System.Windows.Forms.Timer(components);
        tabPrincipal = new TabControl();
        tabPageOperacion = new TabPage();
        panelScrollOperacion = new Panel();
        grpModo = new GroupBox();
        radManual = new RadioButton();
        radAuto = new RadioButton();
        btnSincronizar = new Button();
        grpEquipos = new GroupBox();
        btnAplicarEquipos = new Button();
        lblVisitante = new Label();
        lblLocal = new Label();
        cmbVisitante = new ComboBox();
        cmbLocal = new ComboBox();
        grpManual = new GroupBox();
        picPreviewLogoVisitante = new PictureBox();
        picPreviewLogoLocal = new PictureBox();
        txtNombreVisitante = new TextBox();
        txtNombreLocal = new TextBox();
        btnLogoVisitante = new Button();
        btnLogoLocal = new Button();
        btnAplicarManual = new Button();
        grpCronometro = new GroupBox();
        btnCronReset = new Button();
        btnCronIniciar = new Button();
        lblCronometroAdmin = new Label();
        btnEnviarPeriodoCron = new Button();
        cmbPeriodoOperacion = new ComboBox();
        lblPeriodoOperacion = new Label();
        grpGoles = new GroupBox();
        btnQuitarGolVisitante = new Button();
        lstGolesVisitante = new ListBox();
        lblListaGolesVisitante = new Label();
        btnQuitarGolLocal = new Button();
        lstGolesLocal = new ListBox();
        lblListaGolesLocal = new Label();
        btnAnadirGolVisitanteCombo = new Button();
        cmbJugadorVisitanteGol = new ComboBox();
        lblComboJugadoresVisitante = new Label();
        btnAnadirGolLocalCombo = new Button();
        cmbJugadorLocalGol = new ComboBox();
        lblComboJugadoresLocal = new Label();
        lblAyudaListas = new Label();
        lblMarcador = new Label();
        lblTextoGolManualLocal = new Label();
        txtTextoGolManualLocal = new TextBox();
        lblTextoGolManualVisitante = new Label();
        txtTextoGolManualVisitante = new TextBox();
        btnGolLocal = new Button();
        btnGolVisitante = new Button();
        btnMenosLocal = new Button();
        btnMenosVisitante = new Button();
        tabPageTextos = new TabPage();
        grpCabecera = new GroupBox();
        btnGuardarCabecera = new Button();
        cmbSubtituloPeriodo = new ComboBox();
        lblSubtituloPeriodo = new Label();
        txtEtapaMarquee = new TextBox();
        lblEtapaMarquee = new Label();
        txtTituloLiga = new TextBox();
        lblTituloLiga = new Label();
        tabPageDiseno = new TabPage();
        panelDisenoHost = new Panel();
        splitContainerDiseno = new SplitContainer();
        displayDesignerControl = new DisplayDesignerControl();
        propertyGridLayout = new PropertyGrid();
        panelDisenoPresets = new Panel();
        grpDisenoPerfiles = new GroupBox();
        btnLayoutEliminarPerfil = new Button();
        btnLayoutGuardarComo = new Button();
        txtLayoutPerfilNombre = new TextBox();
        lblDisenoNuevoPerfil = new Label();
        cmbLayoutPerfiles = new ComboBox();
        lblLayoutPerfil = new Label();
        grpDisenoImagenes = new GroupBox();
        btnQuitarImagenGolIntermedia = new Button();
        btnElegirImagenGolIntermedia = new Button();
        lblDisenoSplashGol = new Label();
        btnQuitarFondo = new Button();
        btnElegirFondo = new Button();
        lblDisenoFondo = new Label();
        grpDisenoArchivo = new GroupBox();
        btnLayoutImportarArchivo = new Button();
        btnLayoutExportarArchivo = new Button();
        lblDisenoArchivoAyuda = new Label();
        panelDisenoBotones = new Panel();
        btnLayoutPredeterminado = new Button();
        btnGuardarLayoutMarcador = new Button();
        tabPrincipal.SuspendLayout();
        tabPageOperacion.SuspendLayout();
        panelScrollOperacion.SuspendLayout();
        grpModo.SuspendLayout();
        grpEquipos.SuspendLayout();
        grpManual.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)picPreviewLogoVisitante).BeginInit();
        ((System.ComponentModel.ISupportInitialize)picPreviewLogoLocal).BeginInit();
        grpCronometro.SuspendLayout();
        grpGoles.SuspendLayout();
        tabPageTextos.SuspendLayout();
        grpCabecera.SuspendLayout();
        tabPageDiseno.SuspendLayout();
        panelDisenoHost.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)splitContainerDiseno).BeginInit();
        splitContainerDiseno.Panel1.SuspendLayout();
        splitContainerDiseno.Panel2.SuspendLayout();
        splitContainerDiseno.SuspendLayout();
        panelDisenoPresets.SuspendLayout();
        grpDisenoPerfiles.SuspendLayout();
        grpDisenoImagenes.SuspendLayout();
        grpDisenoArchivo.SuspendLayout();
        panelDisenoBotones.SuspendLayout();
        SuspendLayout();
        // 
        // timerRelojAdmin
        // 
        timerRelojAdmin.Interval = 1000;
        timerRelojAdmin.Tick += TimerRelojAdmin_Tick;
        // 
        // tabPrincipal
        // 
        tabPrincipal.Controls.Add(tabPageOperacion);
        tabPrincipal.Controls.Add(tabPageTextos);
        tabPrincipal.Controls.Add(tabPageDiseno);
        tabPrincipal.Dock = DockStyle.Fill;
        tabPrincipal.Location = new Point(0, 0);
        tabPrincipal.Margin = new Padding(3, 4, 3, 4);
        tabPrincipal.Name = "tabPrincipal";
        tabPrincipal.SelectedIndex = 0;
        tabPrincipal.Size = new Size(1000, 1227);
        tabPrincipal.TabIndex = 0;
        // 
        // tabPageOperacion
        // 
        tabPageOperacion.Controls.Add(panelScrollOperacion);
        tabPageOperacion.Location = new Point(4, 29);
        tabPageOperacion.Margin = new Padding(3, 4, 3, 4);
        tabPageOperacion.Name = "tabPageOperacion";
        tabPageOperacion.Size = new Size(992, 1194);
        tabPageOperacion.TabIndex = 0;
        tabPageOperacion.Text = "Operación";
        tabPageOperacion.UseVisualStyleBackColor = true;
        // 
        // panelScrollOperacion
        // 
        panelScrollOperacion.AutoScroll = true;
        panelScrollOperacion.AutoScrollMinSize = new Size(0, 940);
        panelScrollOperacion.Controls.Add(grpModo);
        panelScrollOperacion.Controls.Add(btnSincronizar);
        panelScrollOperacion.Controls.Add(grpEquipos);
        panelScrollOperacion.Controls.Add(grpManual);
        panelScrollOperacion.Controls.Add(grpCronometro);
        panelScrollOperacion.Controls.Add(grpGoles);
        panelScrollOperacion.Dock = DockStyle.Fill;
        panelScrollOperacion.Location = new Point(0, 0);
        panelScrollOperacion.Margin = new Padding(3, 4, 3, 4);
        panelScrollOperacion.Name = "panelScrollOperacion";
        panelScrollOperacion.Size = new Size(992, 1194);
        panelScrollOperacion.TabIndex = 0;
        // 
        // grpModo
        // 
        grpModo.Controls.Add(radManual);
        grpModo.Controls.Add(radAuto);
        grpModo.Location = new Point(14, 16);
        grpModo.Margin = new Padding(3, 4, 3, 4);
        grpModo.Name = "grpModo";
        grpModo.Padding = new Padding(3, 4, 3, 4);
        grpModo.Size = new Size(229, 93);
        grpModo.TabIndex = 0;
        grpModo.TabStop = false;
        grpModo.Text = "Modo";
        // 
        // radManual
        // 
        radManual.Location = new Point(17, 59);
        radManual.Margin = new Padding(3, 4, 3, 4);
        radManual.Name = "radManual";
        radManual.Size = new Size(171, 27);
        radManual.TabIndex = 0;
        radManual.Text = "Manual";
        // 
        // radAuto
        // 
        radAuto.Checked = true;
        radAuto.Location = new Point(17, 29);
        radAuto.Margin = new Padding(3, 4, 3, 4);
        radAuto.Name = "radAuto";
        radAuto.Size = new Size(194, 27);
        radAuto.TabIndex = 1;
        radAuto.TabStop = true;
        radAuto.Text = "Automático (API)";
        // 
        // btnSincronizar
        // 
        btnSincronizar.Location = new Point(249, 33);
        btnSincronizar.Margin = new Padding(3, 4, 3, 4);
        btnSincronizar.Name = "btnSincronizar";
        btnSincronizar.Size = new Size(160, 47);
        btnSincronizar.TabIndex = 1;
        btnSincronizar.Text = "Sincronizar API";
        btnSincronizar.Click += BtnSincronizar_Click;
        // 
        // grpEquipos
        // 
        grpEquipos.Controls.Add(btnAplicarEquipos);
        grpEquipos.Controls.Add(lblVisitante);
        grpEquipos.Controls.Add(lblLocal);
        grpEquipos.Controls.Add(cmbVisitante);
        grpEquipos.Controls.Add(cmbLocal);
        grpEquipos.Location = new Point(14, 117);
        grpEquipos.Margin = new Padding(3, 4, 3, 4);
        grpEquipos.Name = "grpEquipos";
        grpEquipos.Padding = new Padding(3, 4, 3, 4);
        grpEquipos.Size = new Size(658, 157);
        grpEquipos.TabIndex = 2;
        grpEquipos.TabStop = false;
        grpEquipos.Text = "Equipos del partido";
        // 
        // btnAplicarEquipos
        // 
        btnAplicarEquipos.Location = new Point(17, 115);
        btnAplicarEquipos.Margin = new Padding(3, 4, 3, 4);
        btnAplicarEquipos.Name = "btnAplicarEquipos";
        btnAplicarEquipos.Size = new Size(617, 37);
        btnAplicarEquipos.TabIndex = 0;
        btnAplicarEquipos.Text = "Aplicar equipos al partido";
        btnAplicarEquipos.Click += BtnAplicarEquipos_Click;
        // 
        // lblVisitante
        // 
        lblVisitante.AutoSize = true;
        lblVisitante.Location = new Point(17, 77);
        lblVisitante.Name = "lblVisitante";
        lblVisitante.Size = new Size(69, 20);
        lblVisitante.TabIndex = 1;
        lblVisitante.Text = "Visitante:";
        // 
        // lblLocal
        // 
        lblLocal.AutoSize = true;
        lblLocal.Location = new Point(17, 37);
        lblLocal.Name = "lblLocal";
        lblLocal.Size = new Size(47, 20);
        lblLocal.TabIndex = 2;
        lblLocal.Text = "Local:";
        // 
        // cmbVisitante
        // 
        cmbVisitante.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbVisitante.Location = new Point(91, 73);
        cmbVisitante.Margin = new Padding(3, 4, 3, 4);
        cmbVisitante.Name = "cmbVisitante";
        cmbVisitante.Size = new Size(542, 28);
        cmbVisitante.TabIndex = 3;
        // 
        // cmbLocal
        // 
        cmbLocal.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbLocal.Location = new Point(91, 33);
        cmbLocal.Margin = new Padding(3, 4, 3, 4);
        cmbLocal.Name = "cmbLocal";
        cmbLocal.Size = new Size(542, 28);
        cmbLocal.TabIndex = 4;
        // 
        // grpManual
        // 
        grpManual.Controls.Add(picPreviewLogoVisitante);
        grpManual.Controls.Add(picPreviewLogoLocal);
        grpManual.Controls.Add(txtNombreVisitante);
        grpManual.Controls.Add(txtNombreLocal);
        grpManual.Controls.Add(btnLogoVisitante);
        grpManual.Controls.Add(btnLogoLocal);
        grpManual.Controls.Add(btnAplicarManual);
        grpManual.Location = new Point(14, 117);
        grpManual.Margin = new Padding(3, 4, 3, 4);
        grpManual.Name = "grpManual";
        grpManual.Padding = new Padding(3, 4, 3, 4);
        grpManual.Size = new Size(658, 165);
        grpManual.TabIndex = 3;
        grpManual.TabStop = false;
        grpManual.Text = "Modo manual";
        grpManual.Visible = false;
        // 
        // picPreviewLogoVisitante
        // 
        picPreviewLogoVisitante.BorderStyle = BorderStyle.FixedSingle;
        picPreviewLogoVisitante.Location = new Point(531, 69);
        picPreviewLogoVisitante.Margin = new Padding(3, 4, 3, 4);
        picPreviewLogoVisitante.Name = "picPreviewLogoVisitante";
        picPreviewLogoVisitante.Size = new Size(52, 52);
        picPreviewLogoVisitante.SizeMode = PictureBoxSizeMode.Zoom;
        picPreviewLogoVisitante.TabIndex = 5;
        picPreviewLogoVisitante.TabStop = false;
        // 
        // picPreviewLogoLocal
        // 
        picPreviewLogoLocal.BorderStyle = BorderStyle.FixedSingle;
        picPreviewLogoLocal.Location = new Point(531, 24);
        picPreviewLogoLocal.Margin = new Padding(3, 4, 3, 4);
        picPreviewLogoLocal.Name = "picPreviewLogoLocal";
        picPreviewLogoLocal.Size = new Size(52, 52);
        picPreviewLogoLocal.SizeMode = PictureBoxSizeMode.Zoom;
        picPreviewLogoLocal.TabIndex = 6;
        picPreviewLogoLocal.TabStop = false;
        // 
        // txtNombreVisitante
        // 
        txtNombreVisitante.Location = new Point(91, 79);
        txtNombreVisitante.Margin = new Padding(3, 4, 3, 4);
        txtNombreVisitante.Name = "txtNombreVisitante";
        txtNombreVisitante.PlaceholderText = "Nombre equipo visitante";
        txtNombreVisitante.Size = new Size(334, 27);
        txtNombreVisitante.TabIndex = 0;
        // 
        // txtNombreLocal
        // 
        txtNombreLocal.Location = new Point(91, 33);
        txtNombreLocal.Margin = new Padding(3, 4, 3, 4);
        txtNombreLocal.Name = "txtNombreLocal";
        txtNombreLocal.PlaceholderText = "Nombre equipo local";
        txtNombreLocal.Size = new Size(334, 27);
        txtNombreLocal.TabIndex = 1;
        // 
        // btnLogoVisitante
        // 
        btnLogoVisitante.Location = new Point(435, 77);
        btnLogoVisitante.Margin = new Padding(3, 4, 3, 4);
        btnLogoVisitante.Name = "btnLogoVisitante";
        btnLogoVisitante.Size = new Size(86, 33);
        btnLogoVisitante.TabIndex = 2;
        btnLogoVisitante.Text = "Logo...";
        // 
        // btnLogoLocal
        // 
        btnLogoLocal.Location = new Point(435, 31);
        btnLogoLocal.Margin = new Padding(3, 4, 3, 4);
        btnLogoLocal.Name = "btnLogoLocal";
        btnLogoLocal.Size = new Size(86, 33);
        btnLogoLocal.TabIndex = 3;
        btnLogoLocal.Text = "Logo...";
        // 
        // btnAplicarManual
        // 
        btnAplicarManual.Location = new Point(17, 125);
        btnAplicarManual.Margin = new Padding(3, 4, 3, 4);
        btnAplicarManual.Name = "btnAplicarManual";
        btnAplicarManual.Size = new Size(617, 37);
        btnAplicarManual.TabIndex = 4;
        btnAplicarManual.Text = "Aplicar nombres y logos";
        // 
        // grpCronometro
        // 
        grpCronometro.Controls.Add(btnCronReset);
        grpCronometro.Controls.Add(btnCronIniciar);
        grpCronometro.Controls.Add(lblCronometroAdmin);
        grpCronometro.Controls.Add(btnEnviarPeriodoCron);
        grpCronometro.Controls.Add(cmbPeriodoOperacion);
        grpCronometro.Controls.Add(lblPeriodoOperacion);
        grpCronometro.Location = new Point(14, 293);
        grpCronometro.Margin = new Padding(3, 4, 3, 4);
        grpCronometro.Name = "grpCronometro";
        grpCronometro.Padding = new Padding(3, 4, 3, 4);
        grpCronometro.Size = new Size(658, 123);
        grpCronometro.TabIndex = 4;
        grpCronometro.TabStop = false;
        grpCronometro.Text = "Cronómetro y periodo (BD + marcador)";
        // 
        // btnCronReset
        // 
        btnCronReset.Location = new Point(295, 69);
        btnCronReset.Margin = new Padding(3, 4, 3, 4);
        btnCronReset.Name = "btnCronReset";
        btnCronReset.Size = new Size(114, 40);
        btnCronReset.TabIndex = 0;
        btnCronReset.Text = "DETENER";
        btnCronReset.Click += BtnCronReset_Click;
        // 
        // btnCronIniciar
        // 
        btnCronIniciar.Location = new Point(171, 69);
        btnCronIniciar.Margin = new Padding(3, 4, 3, 4);
        btnCronIniciar.Name = "btnCronIniciar";
        btnCronIniciar.Size = new Size(114, 40);
        btnCronIniciar.TabIndex = 1;
        btnCronIniciar.Text = "Iniciar";
        btnCronIniciar.Click += BtnCronIniciar_Click;
        // 
        // lblCronometroAdmin
        // 
        lblCronometroAdmin.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
        lblCronometroAdmin.Location = new Point(14, 72);
        lblCronometroAdmin.Name = "lblCronometroAdmin";
        lblCronometroAdmin.Size = new Size(137, 43);
        lblCronometroAdmin.TabIndex = 2;
        lblCronometroAdmin.Text = "00:00";
        lblCronometroAdmin.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // btnEnviarPeriodoCron
        // 
        btnEnviarPeriodoCron.Location = new Point(352, 28);
        btnEnviarPeriodoCron.Margin = new Padding(3, 4, 3, 4);
        btnEnviarPeriodoCron.Name = "btnEnviarPeriodoCron";
        btnEnviarPeriodoCron.Size = new Size(194, 37);
        btnEnviarPeriodoCron.TabIndex = 3;
        btnEnviarPeriodoCron.Text = "Enviar periodo";
        btnEnviarPeriodoCron.Click += BtnEnviarPeriodoCron_Click;
        // 
        // cmbPeriodoOperacion
        // 
        cmbPeriodoOperacion.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbPeriodoOperacion.Location = new Point(89, 28);
        cmbPeriodoOperacion.Margin = new Padding(3, 4, 3, 4);
        cmbPeriodoOperacion.Name = "cmbPeriodoOperacion";
        cmbPeriodoOperacion.Size = new Size(251, 28);
        cmbPeriodoOperacion.TabIndex = 4;
        // 
        // lblPeriodoOperacion
        // 
        lblPeriodoOperacion.AutoSize = true;
        lblPeriodoOperacion.Location = new Point(14, 35);
        lblPeriodoOperacion.Name = "lblPeriodoOperacion";
        lblPeriodoOperacion.Size = new Size(63, 20);
        lblPeriodoOperacion.TabIndex = 5;
        lblPeriodoOperacion.Text = "Periodo:";
        // 
        // grpGoles
        // 
        grpGoles.Controls.Add(btnQuitarGolVisitante);
        grpGoles.Controls.Add(lstGolesVisitante);
        grpGoles.Controls.Add(lblListaGolesVisitante);
        grpGoles.Controls.Add(btnQuitarGolLocal);
        grpGoles.Controls.Add(lstGolesLocal);
        grpGoles.Controls.Add(lblListaGolesLocal);
        grpGoles.Controls.Add(btnAnadirGolVisitanteCombo);
        grpGoles.Controls.Add(cmbJugadorVisitanteGol);
        grpGoles.Controls.Add(lblComboJugadoresVisitante);
        grpGoles.Controls.Add(btnAnadirGolLocalCombo);
        grpGoles.Controls.Add(cmbJugadorLocalGol);
        grpGoles.Controls.Add(lblComboJugadoresLocal);
        grpGoles.Controls.Add(lblAyudaListas);
        grpGoles.Controls.Add(lblMarcador);
        grpGoles.Controls.Add(lblTextoGolManualLocal);
        grpGoles.Controls.Add(txtTextoGolManualLocal);
        grpGoles.Controls.Add(lblTextoGolManualVisitante);
        grpGoles.Controls.Add(txtTextoGolManualVisitante);
        grpGoles.Controls.Add(btnGolLocal);
        grpGoles.Controls.Add(btnGolVisitante);
        grpGoles.Controls.Add(btnMenosLocal);
        grpGoles.Controls.Add(btnMenosVisitante);
        grpGoles.Location = new Point(14, 427);
        grpGoles.Margin = new Padding(3, 4, 3, 4);
        grpGoles.Name = "grpGoles";
        grpGoles.Padding = new Padding(3, 4, 3, 4);
        grpGoles.Size = new Size(658, 491);
        grpGoles.TabIndex = 5;
        grpGoles.TabStop = false;
        grpGoles.Text = "Marcador";
        // 
        // btnQuitarGolVisitante
        // 
        btnQuitarGolVisitante.Location = new Point(335, 445);
        btnQuitarGolVisitante.Margin = new Padding(3, 4, 3, 4);
        btnQuitarGolVisitante.Name = "btnQuitarGolVisitante";
        btnQuitarGolVisitante.Size = new Size(306, 37);
        btnQuitarGolVisitante.TabIndex = 0;
        btnQuitarGolVisitante.Text = "Quitar gol seleccionado";
        btnQuitarGolVisitante.Click += BtnQuitarGolVisitante_Click;
        // 
        // lstGolesVisitante
        // 
        lstGolesVisitante.IntegralHeight = false;
        lstGolesVisitante.Location = new Point(335, 240);
        lstGolesVisitante.Margin = new Padding(3, 4, 3, 4);
        lstGolesVisitante.Name = "lstGolesVisitante";
        lstGolesVisitante.Size = new Size(306, 199);
        lstGolesVisitante.TabIndex = 1;
        // 
        // lblListaGolesVisitante
        // 
        lblListaGolesVisitante.AutoSize = true;
        lblListaGolesVisitante.Location = new Point(335, 213);
        lblListaGolesVisitante.Name = "lblListaGolesVisitante";
        lblListaGolesVisitante.Size = new Size(49, 20);
        lblListaGolesVisitante.TabIndex = 2;
        lblListaGolesVisitante.Text = "Goles:";
        // 
        // btnQuitarGolLocal
        // 
        btnQuitarGolLocal.Location = new Point(17, 445);
        btnQuitarGolLocal.Margin = new Padding(3, 4, 3, 4);
        btnQuitarGolLocal.Name = "btnQuitarGolLocal";
        btnQuitarGolLocal.Size = new Size(306, 37);
        btnQuitarGolLocal.TabIndex = 3;
        btnQuitarGolLocal.Text = "Quitar gol seleccionado";
        btnQuitarGolLocal.Click += BtnQuitarGolLocal_Click;
        // 
        // lstGolesLocal
        // 
        lstGolesLocal.IntegralHeight = false;
        lstGolesLocal.Location = new Point(17, 240);
        lstGolesLocal.Margin = new Padding(3, 4, 3, 4);
        lstGolesLocal.Name = "lstGolesLocal";
        lstGolesLocal.Size = new Size(306, 199);
        lstGolesLocal.TabIndex = 4;
        // 
        // lblListaGolesLocal
        // 
        lblListaGolesLocal.AutoSize = true;
        lblListaGolesLocal.Location = new Point(17, 213);
        lblListaGolesLocal.Name = "lblListaGolesLocal";
        lblListaGolesLocal.Size = new Size(49, 20);
        lblListaGolesLocal.TabIndex = 5;
        lblListaGolesLocal.Text = "Goles:";
        // 
        // btnAnadirGolVisitanteCombo
        // 
        btnAnadirGolVisitanteCombo.Location = new Point(561, 97);
        btnAnadirGolVisitanteCombo.Margin = new Padding(3, 4, 3, 4);
        btnAnadirGolVisitanteCombo.Name = "btnAnadirGolVisitanteCombo";
        btnAnadirGolVisitanteCombo.Size = new Size(80, 33);
        btnAnadirGolVisitanteCombo.TabIndex = 6;
        btnAnadirGolVisitanteCombo.Text = "Añadir gol";
        btnAnadirGolVisitanteCombo.Click += BtnAnadirGolVisitanteCombo_Click;
        // 
        // cmbJugadorVisitanteGol
        // 
        cmbJugadorVisitanteGol.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbJugadorVisitanteGol.Location = new Point(335, 101);
        cmbJugadorVisitanteGol.Margin = new Padding(3, 4, 3, 4);
        cmbJugadorVisitanteGol.Name = "cmbJugadorVisitanteGol";
        cmbJugadorVisitanteGol.Size = new Size(218, 28);
        cmbJugadorVisitanteGol.TabIndex = 7;
        // 
        // lblComboJugadoresVisitante
        // 
        lblComboJugadoresVisitante.AutoSize = true;
        lblComboJugadoresVisitante.Location = new Point(335, 75);
        lblComboJugadoresVisitante.Name = "lblComboJugadoresVisitante";
        lblComboJugadoresVisitante.Size = new Size(170, 20);
        lblComboJugadoresVisitante.TabIndex = 8;
        lblComboJugadoresVisitante.Text = "VISITANTE — jugadores:";
        // 
        // btnAnadirGolLocalCombo
        // 
        btnAnadirGolLocalCombo.Location = new Point(247, 97);
        btnAnadirGolLocalCombo.Margin = new Padding(3, 4, 3, 4);
        btnAnadirGolLocalCombo.Name = "btnAnadirGolLocalCombo";
        btnAnadirGolLocalCombo.Size = new Size(80, 33);
        btnAnadirGolLocalCombo.TabIndex = 9;
        btnAnadirGolLocalCombo.Text = "Añadir gol";
        btnAnadirGolLocalCombo.Click += BtnAnadirGolLocalCombo_Click;
        // 
        // cmbJugadorLocalGol
        // 
        cmbJugadorLocalGol.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbJugadorLocalGol.Location = new Point(17, 101);
        cmbJugadorLocalGol.Margin = new Padding(3, 4, 3, 4);
        cmbJugadorLocalGol.Name = "cmbJugadorLocalGol";
        cmbJugadorLocalGol.Size = new Size(224, 28);
        cmbJugadorLocalGol.TabIndex = 10;
        // 
        // lblComboJugadoresLocal
        // 
        lblComboJugadoresLocal.AutoSize = true;
        lblComboJugadoresLocal.Location = new Point(17, 75);
        lblComboJugadoresLocal.Name = "lblComboJugadoresLocal";
        lblComboJugadoresLocal.Size = new Size(144, 20);
        lblComboJugadoresLocal.TabIndex = 11;
        lblComboJugadoresLocal.Text = "LOCAL — jugadores:";
        // 
        // lblAyudaListas
        // 
        lblAyudaListas.ForeColor = SystemColors.GrayText;
        lblAyudaListas.Location = new Point(274, 29);
        lblAyudaListas.Name = "lblAyudaListas";
        lblAyudaListas.Size = new Size(366, 43);
        lblAyudaListas.TabIndex = 12;
        lblAyudaListas.Text = "Automático: local izquierda, visitante derecha. Manual: +1 / −.";
        // 
        // lblMarcador
        // 
        lblMarcador.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
        lblMarcador.Location = new Point(17, 29);
        lblMarcador.Name = "lblMarcador";
        lblMarcador.Size = new Size(251, 47);
        lblMarcador.TabIndex = 13;
        lblMarcador.Text = "0 - 0";
        // 
        // lblTextoGolManualLocal
        // 
        lblTextoGolManualLocal.AutoSize = true;
        lblTextoGolManualLocal.Location = new Point(17, 118);
        lblTextoGolManualLocal.Name = "lblTextoGolManualLocal";
        lblTextoGolManualLocal.Size = new Size(113, 20);
        lblTextoGolManualLocal.TabIndex = 18;
        lblTextoGolManualLocal.Text = "Goleador Local:";
        lblTextoGolManualLocal.Visible = false;
        // 
        // txtTextoGolManualLocal
        // 
        txtTextoGolManualLocal.Location = new Point(17, 141);
        txtTextoGolManualLocal.Margin = new Padding(3, 4, 3, 4);
        txtTextoGolManualLocal.Name = "txtTextoGolManualLocal";
        txtTextoGolManualLocal.PlaceholderText = "Ej. nombre del goleador";
        txtTextoGolManualLocal.Size = new Size(154, 27);
        txtTextoGolManualLocal.TabIndex = 19;
        txtTextoGolManualLocal.Visible = false;
        // 
        // lblTextoGolManualVisitante
        // 
        lblTextoGolManualVisitante.AutoSize = true;
        lblTextoGolManualVisitante.Location = new Point(183, 118);
        lblTextoGolManualVisitante.Name = "lblTextoGolManualVisitante";
        lblTextoGolManualVisitante.Size = new Size(135, 20);
        lblTextoGolManualVisitante.TabIndex = 20;
        lblTextoGolManualVisitante.Text = "Goleador Visitante:";
        lblTextoGolManualVisitante.Visible = false;
        // 
        // txtTextoGolManualVisitante
        // 
        txtTextoGolManualVisitante.Location = new Point(183, 141);
        txtTextoGolManualVisitante.Margin = new Padding(3, 4, 3, 4);
        txtTextoGolManualVisitante.Name = "txtTextoGolManualVisitante";
        txtTextoGolManualVisitante.PlaceholderText = "Ej. nombre del goleador";
        txtTextoGolManualVisitante.Size = new Size(173, 27);
        txtTextoGolManualVisitante.TabIndex = 21;
        txtTextoGolManualVisitante.Visible = false;
        // 
        // btnGolLocal
        // 
        btnGolLocal.Location = new Point(17, 77);
        btnGolLocal.Margin = new Padding(3, 4, 3, 4);
        btnGolLocal.Name = "btnGolLocal";
        btnGolLocal.Size = new Size(101, 37);
        btnGolLocal.TabIndex = 14;
        btnGolLocal.Text = "+1 Local";
        btnGolLocal.Click += BtnGolLocal_Click;
        // 
        // btnGolVisitante
        // 
        btnGolVisitante.Location = new Point(183, 77);
        btnGolVisitante.Margin = new Padding(3, 4, 3, 4);
        btnGolVisitante.Name = "btnGolVisitante";
        btnGolVisitante.Size = new Size(120, 37);
        btnGolVisitante.TabIndex = 15;
        btnGolVisitante.Text = "+1 Visitante";
        btnGolVisitante.Click += BtnGolVisitante_Click;
        // 
        // btnMenosLocal
        // 
        btnMenosLocal.Location = new Point(125, 77);
        btnMenosLocal.Margin = new Padding(3, 4, 3, 4);
        btnMenosLocal.Name = "btnMenosLocal";
        btnMenosLocal.Size = new Size(46, 37);
        btnMenosLocal.TabIndex = 16;
        btnMenosLocal.Text = "-";
        btnMenosLocal.Click += BtnMenosLocal_Click;
        // 
        // btnMenosVisitante
        // 
        btnMenosVisitante.Location = new Point(310, 77);
        btnMenosVisitante.Margin = new Padding(3, 4, 3, 4);
        btnMenosVisitante.Name = "btnMenosVisitante";
        btnMenosVisitante.Size = new Size(46, 37);
        btnMenosVisitante.TabIndex = 17;
        btnMenosVisitante.Text = "-";
        btnMenosVisitante.Click += BtnMenosVisitante_Click;
        // 
        // tabPageTextos
        // 
        tabPageTextos.Controls.Add(grpCabecera);
        tabPageTextos.Location = new Point(4, 29);
        tabPageTextos.Margin = new Padding(3, 4, 3, 4);
        tabPageTextos.Name = "tabPageTextos";
        tabPageTextos.Padding = new Padding(9, 11, 9, 11);
        tabPageTextos.Size = new Size(992, 1194);
        tabPageTextos.TabIndex = 1;
        tabPageTextos.Text = "Textos pantalla";
        tabPageTextos.UseVisualStyleBackColor = true;
        // 
        // grpCabecera
        // 
        grpCabecera.Controls.Add(btnGuardarCabecera);
        grpCabecera.Controls.Add(cmbSubtituloPeriodo);
        grpCabecera.Controls.Add(lblSubtituloPeriodo);
        grpCabecera.Controls.Add(txtEtapaMarquee);
        grpCabecera.Controls.Add(lblEtapaMarquee);
        grpCabecera.Controls.Add(txtTituloLiga);
        grpCabecera.Controls.Add(lblTituloLiga);
        grpCabecera.Dock = DockStyle.Fill;
        grpCabecera.Location = new Point(9, 11);
        grpCabecera.Margin = new Padding(3, 4, 3, 4);
        grpCabecera.Name = "grpCabecera";
        grpCabecera.Padding = new Padding(3, 4, 3, 4);
        grpCabecera.Size = new Size(974, 1172);
        grpCabecera.TabIndex = 0;
        grpCabecera.TabStop = false;
        grpCabecera.Text = "Textos pantalla (título, etapa, periodo)";
        // 
        // btnGuardarCabecera
        // 
        btnGuardarCabecera.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnGuardarCabecera.Location = new Point(868, 109);
        btnGuardarCabecera.Margin = new Padding(3, 4, 3, 4);
        btnGuardarCabecera.Name = "btnGuardarCabecera";
        btnGuardarCabecera.Size = new Size(91, 37);
        btnGuardarCabecera.TabIndex = 0;
        btnGuardarCabecera.Text = "Guardar";
        btnGuardarCabecera.Click += BtnGuardarCabecera_Click;
        // 
        // cmbSubtituloPeriodo
        // 
        cmbSubtituloPeriodo.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbSubtituloPeriodo.Location = new Point(114, 111);
        cmbSubtituloPeriodo.Margin = new Padding(3, 4, 3, 4);
        cmbSubtituloPeriodo.Name = "cmbSubtituloPeriodo";
        cmbSubtituloPeriodo.Size = new Size(228, 28);
        cmbSubtituloPeriodo.TabIndex = 1;
        // 
        // lblSubtituloPeriodo
        // 
        lblSubtituloPeriodo.AutoSize = true;
        lblSubtituloPeriodo.Location = new Point(14, 115);
        lblSubtituloPeriodo.Name = "lblSubtituloPeriodo";
        lblSubtituloPeriodo.Size = new Size(115, 20);
        lblSubtituloPeriodo.TabIndex = 2;
        lblSubtituloPeriodo.Text = "Periodo (PT/ST):";
        // 
        // txtEtapaMarquee
        // 
        txtEtapaMarquee.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        txtEtapaMarquee.Location = new Point(137, 73);
        txtEtapaMarquee.Margin = new Padding(3, 4, 3, 4);
        txtEtapaMarquee.Name = "txtEtapaMarquee";
        txtEtapaMarquee.PlaceholderText = "Ej. SEGUNDA ETAPA";
        txtEtapaMarquee.Size = new Size(1225, 27);
        txtEtapaMarquee.TabIndex = 3;
        // 
        // lblEtapaMarquee
        // 
        lblEtapaMarquee.AutoSize = true;
        lblEtapaMarquee.Location = new Point(14, 77);
        lblEtapaMarquee.Name = "lblEtapaMarquee";
        lblEtapaMarquee.Size = new Size(123, 20);
        lblEtapaMarquee.TabIndex = 4;
        lblEtapaMarquee.Text = "Etapa (marquee):";
        // 
        // txtTituloLiga
        // 
        txtTituloLiga.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        txtTituloLiga.Location = new Point(114, 33);
        txtTituloLiga.Margin = new Padding(3, 4, 3, 4);
        txtTituloLiga.Name = "txtTituloLiga";
        txtTituloLiga.PlaceholderText = "Ej. LIGA GUAPULO";
        txtTituloLiga.Size = new Size(1248, 27);
        txtTituloLiga.TabIndex = 5;
        // 
        // lblTituloLiga
        // 
        lblTituloLiga.AutoSize = true;
        lblTituloLiga.Location = new Point(14, 37);
        lblTituloLiga.Name = "lblTituloLiga";
        lblTituloLiga.Size = new Size(79, 20);
        lblTituloLiga.TabIndex = 6;
        lblTituloLiga.Text = "Título liga:";
        // 
        // tabPageDiseno
        // 
        tabPageDiseno.Controls.Add(panelDisenoHost);
        tabPageDiseno.Location = new Point(4, 29);
        tabPageDiseno.Margin = new Padding(3, 4, 3, 4);
        tabPageDiseno.Name = "tabPageDiseno";
        tabPageDiseno.Padding = new Padding(9, 11, 9, 11);
        tabPageDiseno.Size = new Size(992, 1194);
        tabPageDiseno.TabIndex = 2;
        tabPageDiseno.Text = "Diseño marcador";
        tabPageDiseno.UseVisualStyleBackColor = true;
        // 
        // panelDisenoHost
        // 
        panelDisenoHost.Controls.Add(splitContainerDiseno);
        panelDisenoHost.Controls.Add(panelDisenoPresets);
        panelDisenoHost.Controls.Add(panelDisenoBotones);
        panelDisenoHost.Dock = DockStyle.Fill;
        panelDisenoHost.Location = new Point(9, 11);
        panelDisenoHost.Margin = new Padding(3, 4, 3, 4);
        panelDisenoHost.Name = "panelDisenoHost";
        panelDisenoHost.Size = new Size(974, 1172);
        panelDisenoHost.TabIndex = 0;
        // 
        // splitContainerDiseno
        // 
        splitContainerDiseno.Dock = DockStyle.Fill;
        splitContainerDiseno.Location = new Point(0, 356);
        splitContainerDiseno.Name = "splitContainerDiseno";
        // 
        // splitContainerDiseno.Panel1
        // 
        splitContainerDiseno.Panel1.Controls.Add(displayDesignerControl);
        splitContainerDiseno.Panel1MinSize = 120;
        // 
        // splitContainerDiseno.Panel2
        // 
        splitContainerDiseno.Panel2.Controls.Add(propertyGridLayout);
        splitContainerDiseno.Panel2MinSize = 100;
        splitContainerDiseno.Size = new Size(974, 757);
        splitContainerDiseno.SplitterDistance = 500;
        splitContainerDiseno.TabIndex = 3;
        // 
        // displayDesignerControl
        // 
        displayDesignerControl.BackColor = Color.FromArgb(45, 45, 48);
        displayDesignerControl.Dock = DockStyle.Fill;
        displayDesignerControl.Location = new Point(0, 0);
        displayDesignerControl.Margin = new Padding(0);
        displayDesignerControl.Name = "displayDesignerControl";
        displayDesignerControl.Size = new Size(500, 757);
        displayDesignerControl.TabIndex = 0;
        // 
        // propertyGridLayout
        // 
        propertyGridLayout.Dock = DockStyle.Fill;
        propertyGridLayout.Location = new Point(0, 0);
        propertyGridLayout.Margin = new Padding(3, 4, 3, 4);
        propertyGridLayout.Name = "propertyGridLayout";
        propertyGridLayout.PropertySort = PropertySort.Categorized;
        propertyGridLayout.Size = new Size(470, 757);
        propertyGridLayout.TabIndex = 0;
        propertyGridLayout.ToolbarVisible = false;
        // 
        // panelDisenoPresets
        // 
        panelDisenoPresets.Controls.Add(grpDisenoPerfiles);
        panelDisenoPresets.Controls.Add(grpDisenoImagenes);
        panelDisenoPresets.Controls.Add(grpDisenoArchivo);
        panelDisenoPresets.Dock = DockStyle.Top;
        panelDisenoPresets.Location = new Point(0, 0);
        panelDisenoPresets.Margin = new Padding(3, 4, 3, 4);
        panelDisenoPresets.Name = "panelDisenoPresets";
        panelDisenoPresets.Padding = new Padding(6, 4, 6, 4);
        panelDisenoPresets.Size = new Size(974, 356);
        panelDisenoPresets.TabIndex = 1;
        // 
        // grpDisenoPerfiles
        // 
        grpDisenoPerfiles.Controls.Add(btnLayoutEliminarPerfil);
        grpDisenoPerfiles.Controls.Add(btnLayoutGuardarComo);
        grpDisenoPerfiles.Controls.Add(txtLayoutPerfilNombre);
        grpDisenoPerfiles.Controls.Add(lblDisenoNuevoPerfil);
        grpDisenoPerfiles.Controls.Add(cmbLayoutPerfiles);
        grpDisenoPerfiles.Controls.Add(lblLayoutPerfil);
        grpDisenoPerfiles.Dock = DockStyle.Top;
        grpDisenoPerfiles.Location = new Point(6, 240);
        grpDisenoPerfiles.Margin = new Padding(3, 4, 3, 8);
        grpDisenoPerfiles.Name = "grpDisenoPerfiles";
        grpDisenoPerfiles.Padding = new Padding(3, 4, 3, 4);
        grpDisenoPerfiles.Size = new Size(962, 108);
        grpDisenoPerfiles.TabIndex = 0;
        grpDisenoPerfiles.TabStop = false;
        grpDisenoPerfiles.Text = "Perfiles de diseño (en la base de datos)";
        // 
        // btnLayoutEliminarPerfil
        // 
        btnLayoutEliminarPerfil.Location = new Point(620, 64);
        btnLayoutEliminarPerfil.Margin = new Padding(3, 4, 3, 4);
        btnLayoutEliminarPerfil.Name = "btnLayoutEliminarPerfil";
        btnLayoutEliminarPerfil.Size = new Size(180, 35);
        btnLayoutEliminarPerfil.TabIndex = 5;
        btnLayoutEliminarPerfil.Text = "Eliminar perfil seleccionado";
        btnLayoutEliminarPerfil.Click += BtnLayoutEliminarPerfil_Click;
        // 
        // btnLayoutGuardarComo
        // 
        btnLayoutGuardarComo.Location = new Point(410, 64);
        btnLayoutGuardarComo.Margin = new Padding(3, 4, 3, 4);
        btnLayoutGuardarComo.Name = "btnLayoutGuardarComo";
        btnLayoutGuardarComo.Size = new Size(200, 35);
        btnLayoutGuardarComo.TabIndex = 4;
        btnLayoutGuardarComo.Text = "Guardar diseño como perfil…";
        btnLayoutGuardarComo.Click += BtnLayoutGuardarComo_Click;
        // 
        // txtLayoutPerfilNombre
        // 
        txtLayoutPerfilNombre.Location = new Point(180, 68);
        txtLayoutPerfilNombre.Margin = new Padding(3, 4, 3, 4);
        txtLayoutPerfilNombre.Name = "txtLayoutPerfilNombre";
        txtLayoutPerfilNombre.PlaceholderText = "Ej. AMISTOSO";
        txtLayoutPerfilNombre.Size = new Size(220, 27);
        txtLayoutPerfilNombre.TabIndex = 3;
        // 
        // lblDisenoNuevoPerfil
        // 
        lblDisenoNuevoPerfil.AutoSize = true;
        lblDisenoNuevoPerfil.Location = new Point(14, 72);
        lblDisenoNuevoPerfil.Name = "lblDisenoNuevoPerfil";
        lblDisenoNuevoPerfil.Size = new Size(150, 20);
        lblDisenoNuevoPerfil.TabIndex = 2;
        lblDisenoNuevoPerfil.Text = "Nombre nuevo perfil:";
        // 
        // cmbLayoutPerfiles
        // 
        cmbLayoutPerfiles.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbLayoutPerfiles.Location = new Point(130, 29);
        cmbLayoutPerfiles.Margin = new Padding(3, 4, 3, 4);
        cmbLayoutPerfiles.Name = "cmbLayoutPerfiles";
        cmbLayoutPerfiles.Size = new Size(320, 28);
        cmbLayoutPerfiles.TabIndex = 1;
        cmbLayoutPerfiles.SelectedIndexChanged += CmbLayoutPerfiles_SelectedIndexChanged;
        // 
        // lblLayoutPerfil
        // 
        lblLayoutPerfil.AutoSize = true;
        lblLayoutPerfil.Location = new Point(14, 33);
        lblLayoutPerfil.Name = "lblLayoutPerfil";
        lblLayoutPerfil.Size = new Size(89, 20);
        lblLayoutPerfil.TabIndex = 0;
        lblLayoutPerfil.Text = "Perfil activo:";
        // 
        // grpDisenoImagenes
        // 
        grpDisenoImagenes.Controls.Add(btnQuitarImagenGolIntermedia);
        grpDisenoImagenes.Controls.Add(btnElegirImagenGolIntermedia);
        grpDisenoImagenes.Controls.Add(lblDisenoSplashGol);
        grpDisenoImagenes.Controls.Add(btnQuitarFondo);
        grpDisenoImagenes.Controls.Add(btnElegirFondo);
        grpDisenoImagenes.Controls.Add(lblDisenoFondo);
        grpDisenoImagenes.Dock = DockStyle.Top;
        grpDisenoImagenes.Location = new Point(6, 122);
        grpDisenoImagenes.Margin = new Padding(3, 4, 3, 8);
        grpDisenoImagenes.Name = "grpDisenoImagenes";
        grpDisenoImagenes.Padding = new Padding(3, 4, 3, 4);
        grpDisenoImagenes.Size = new Size(962, 118);
        grpDisenoImagenes.TabIndex = 1;
        grpDisenoImagenes.TabStop = false;
        grpDisenoImagenes.Text = "Imágenes del marcador (fondo y celebración de gol)";
        // 
        // btnQuitarImagenGolIntermedia
        // 
        btnQuitarImagenGolIntermedia.Location = new Point(588, 72);
        btnQuitarImagenGolIntermedia.Margin = new Padding(3, 4, 3, 4);
        btnQuitarImagenGolIntermedia.Name = "btnQuitarImagenGolIntermedia";
        btnQuitarImagenGolIntermedia.Size = new Size(200, 35);
        btnQuitarImagenGolIntermedia.TabIndex = 5;
        btnQuitarImagenGolIntermedia.Text = "Quitar imagen «¡GOL!»";
        btnQuitarImagenGolIntermedia.Click += BtnQuitarImagenGolIntermedia_Click;
        // 
        // btnElegirImagenGolIntermedia
        // 
        btnElegirImagenGolIntermedia.Location = new Point(380, 72);
        btnElegirImagenGolIntermedia.Margin = new Padding(3, 4, 3, 4);
        btnElegirImagenGolIntermedia.Name = "btnElegirImagenGolIntermedia";
        btnElegirImagenGolIntermedia.Size = new Size(200, 35);
        btnElegirImagenGolIntermedia.TabIndex = 4;
        btnElegirImagenGolIntermedia.Text = "Elegir imagen «¡GOL!»…";
        btnElegirImagenGolIntermedia.Click += BtnElegirImagenGolIntermedia_Click;
        // 
        // lblDisenoSplashGol
        // 
        lblDisenoSplashGol.AutoSize = true;
        lblDisenoSplashGol.Location = new Point(14, 78);
        lblDisenoSplashGol.Name = "lblDisenoSplashGol";
        lblDisenoSplashGol.Size = new Size(364, 20);
        lblDisenoSplashGol.TabIndex = 3;
        lblDisenoSplashGol.Text = "Imagen a pantalla completa antes del detalle del gol:";
        // 
        // btnQuitarFondo
        // 
        btnQuitarFondo.Location = new Point(588, 30);
        btnQuitarFondo.Margin = new Padding(3, 4, 3, 4);
        btnQuitarFondo.Name = "btnQuitarFondo";
        btnQuitarFondo.Size = new Size(200, 35);
        btnQuitarFondo.TabIndex = 2;
        btnQuitarFondo.Text = "Quitar imagen de fondo";
        btnQuitarFondo.Click += BtnQuitarFondo_Click;
        // 
        // btnElegirFondo
        // 
        btnElegirFondo.Location = new Point(380, 30);
        btnElegirFondo.Margin = new Padding(3, 4, 3, 4);
        btnElegirFondo.Name = "btnElegirFondo";
        btnElegirFondo.Size = new Size(200, 35);
        btnElegirFondo.TabIndex = 1;
        btnElegirFondo.Text = "Elegir imagen de fondo…";
        btnElegirFondo.Click += BtnElegirFondo_Click;
        // 
        // lblDisenoFondo
        // 
        lblDisenoFondo.AutoSize = true;
        lblDisenoFondo.Location = new Point(14, 36);
        lblDisenoFondo.Name = "lblDisenoFondo";
        lblDisenoFondo.Size = new Size(315, 20);
        lblDisenoFondo.TabIndex = 0;
        lblDisenoFondo.Text = "Imagen de fondo (detrás de textos y escudos):";
        // 
        // grpDisenoArchivo
        // 
        grpDisenoArchivo.Controls.Add(btnLayoutImportarArchivo);
        grpDisenoArchivo.Controls.Add(btnLayoutExportarArchivo);
        grpDisenoArchivo.Controls.Add(lblDisenoArchivoAyuda);
        grpDisenoArchivo.Dock = DockStyle.Top;
        grpDisenoArchivo.Location = new Point(6, 4);
        grpDisenoArchivo.Margin = new Padding(3, 4, 3, 4);
        grpDisenoArchivo.Name = "grpDisenoArchivo";
        grpDisenoArchivo.Padding = new Padding(3, 4, 3, 4);
        grpDisenoArchivo.Size = new Size(962, 118);
        grpDisenoArchivo.TabIndex = 2;
        grpDisenoArchivo.TabStop = false;
        grpDisenoArchivo.Text = "Copia del diseño en archivo (JSON)";
        // 
        // btnLayoutImportarArchivo
        // 
        btnLayoutImportarArchivo.Location = new Point(264, 74);
        btnLayoutImportarArchivo.Margin = new Padding(3, 4, 3, 4);
        btnLayoutImportarArchivo.Name = "btnLayoutImportarArchivo";
        btnLayoutImportarArchivo.Size = new Size(240, 35);
        btnLayoutImportarArchivo.TabIndex = 2;
        btnLayoutImportarArchivo.Text = "Importar diseño desde archivo…";
        btnLayoutImportarArchivo.Click += BtnLayoutImportarArchivo_Click;
        // 
        // btnLayoutExportarArchivo
        // 
        btnLayoutExportarArchivo.Location = new Point(14, 74);
        btnLayoutExportarArchivo.Margin = new Padding(3, 4, 3, 4);
        btnLayoutExportarArchivo.Name = "btnLayoutExportarArchivo";
        btnLayoutExportarArchivo.Size = new Size(240, 35);
        btnLayoutExportarArchivo.TabIndex = 1;
        btnLayoutExportarArchivo.Text = "Exportar diseño a archivo…";
        btnLayoutExportarArchivo.Click += BtnLayoutExportarArchivo_Click;
        // 
        // lblDisenoArchivoAyuda
        // 
        lblDisenoArchivoAyuda.Location = new Point(14, 28);
        lblDisenoArchivoAyuda.Name = "lblDisenoArchivoAyuda";
        lblDisenoArchivoAyuda.Size = new Size(930, 44);
        lblDisenoArchivoAyuda.TabIndex = 0;
        lblDisenoArchivoAyuda.Text = "Exporte el diseño actual a un archivo para respaldo o para copiarlo a otro PC. Tras importar, use «Guardar diseño» abajo para guardarlo en la base de datos y enviarlo al marcador.";
        // 
        // panelDisenoBotones
        // 
        panelDisenoBotones.Controls.Add(btnLayoutPredeterminado);
        panelDisenoBotones.Controls.Add(btnGuardarLayoutMarcador);
        panelDisenoBotones.Dock = DockStyle.Bottom;
        panelDisenoBotones.Location = new Point(0, 1113);
        panelDisenoBotones.Margin = new Padding(3, 4, 3, 4);
        panelDisenoBotones.Name = "panelDisenoBotones";
        panelDisenoBotones.Size = new Size(974, 59);
        panelDisenoBotones.TabIndex = 2;
        // 
        // btnLayoutPredeterminado
        // 
        btnLayoutPredeterminado.Location = new Point(197, 11);
        btnLayoutPredeterminado.Margin = new Padding(3, 4, 3, 4);
        btnLayoutPredeterminado.Name = "btnLayoutPredeterminado";
        btnLayoutPredeterminado.Size = new Size(229, 37);
        btnLayoutPredeterminado.TabIndex = 0;
        btnLayoutPredeterminado.Text = "Restaurar predeterminados";
        btnLayoutPredeterminado.Click += BtnLayoutPredeterminado_Click;
        // 
        // btnGuardarLayoutMarcador
        // 
        btnGuardarLayoutMarcador.Location = new Point(0, 11);
        btnGuardarLayoutMarcador.Margin = new Padding(3, 4, 3, 4);
        btnGuardarLayoutMarcador.Name = "btnGuardarLayoutMarcador";
        btnGuardarLayoutMarcador.Size = new Size(183, 37);
        btnGuardarLayoutMarcador.TabIndex = 1;
        btnGuardarLayoutMarcador.Text = "Guardar diseño";
        btnGuardarLayoutMarcador.Click += BtnGuardarLayoutMarcador_Click;
        // 
        // Form1
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1000, 1227);
        Controls.Add(tabPrincipal);
        Margin = new Padding(3, 4, 3, 4);
        MinimumSize = new Size(880, 918);
        Name = "Form1";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Marcador LBG - Administrador";
        tabPrincipal.ResumeLayout(false);
        tabPageOperacion.ResumeLayout(false);
        panelScrollOperacion.ResumeLayout(false);
        grpModo.ResumeLayout(false);
        grpEquipos.ResumeLayout(false);
        grpEquipos.PerformLayout();
        grpManual.ResumeLayout(false);
        grpManual.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)picPreviewLogoVisitante).EndInit();
        ((System.ComponentModel.ISupportInitialize)picPreviewLogoLocal).EndInit();
        grpCronometro.ResumeLayout(false);
        grpCronometro.PerformLayout();
        grpGoles.ResumeLayout(false);
        grpGoles.PerformLayout();
        tabPageTextos.ResumeLayout(false);
        grpCabecera.ResumeLayout(false);
        grpCabecera.PerformLayout();
        tabPageDiseno.ResumeLayout(false);
        panelDisenoHost.ResumeLayout(false);
        splitContainerDiseno.Panel1.ResumeLayout(false);
        splitContainerDiseno.Panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)splitContainerDiseno).EndInit();
        splitContainerDiseno.ResumeLayout(false);
        panelDisenoPresets.ResumeLayout(false);
        grpDisenoPerfiles.ResumeLayout(false);
        grpDisenoPerfiles.PerformLayout();
        grpDisenoImagenes.ResumeLayout(false);
        grpDisenoImagenes.PerformLayout();
        grpDisenoArchivo.ResumeLayout(false);
        panelDisenoBotones.ResumeLayout(false);
        ResumeLayout(false);
    }

    private System.Windows.Forms.Timer timerRelojAdmin;
    private TabControl tabPrincipal;
    private TabPage tabPageOperacion;
    private TabPage tabPageTextos;
    private TabPage tabPageDiseno;
    private Panel panelScrollOperacion;
    private Panel panelDisenoHost;
    private SplitContainer splitContainerDiseno;
    private DisplayDesignerControl displayDesignerControl;
    private Panel panelDisenoPresets;
    private GroupBox grpDisenoPerfiles;
    private GroupBox grpDisenoImagenes;
    private GroupBox grpDisenoArchivo;
    private Label lblLayoutPerfil;
    private Label lblDisenoNuevoPerfil;
    private ComboBox cmbLayoutPerfiles;
    private TextBox txtLayoutPerfilNombre;
    private Button btnLayoutGuardarComo;
    private Button btnLayoutEliminarPerfil;
    private Label lblDisenoFondo;
    private Button btnElegirFondo;
    private Button btnQuitarFondo;
    private Label lblDisenoSplashGol;
    private Button btnElegirImagenGolIntermedia;
    private Button btnQuitarImagenGolIntermedia;
    private Label lblDisenoArchivoAyuda;
    private Button btnLayoutExportarArchivo;
    private Button btnLayoutImportarArchivo;
    private Panel panelDisenoBotones;
    private GroupBox grpCabecera;
    private Label lblTituloLiga;
    private TextBox txtTituloLiga;
    private Label lblEtapaMarquee;
    private TextBox txtEtapaMarquee;
    private Label lblSubtituloPeriodo;
    private ComboBox cmbSubtituloPeriodo;
    private Button btnGuardarCabecera;
    private GroupBox grpCronometro;
    private Label lblPeriodoOperacion;
    private ComboBox cmbPeriodoOperacion;
    private Button btnEnviarPeriodoCron;
    private Label lblCronometroAdmin;
    private Button btnCronIniciar;
    private Button btnCronReset;
    private GroupBox grpModo;
    private RadioButton radManual;
    private RadioButton radAuto;
    private Button btnSincronizar;
    private GroupBox grpEquipos;
    private Label lblVisitante;
    private Label lblLocal;
    private ComboBox cmbVisitante;
    private ComboBox cmbLocal;
    private Button btnAplicarEquipos;
    private GroupBox grpGoles;
    private Label lblMarcador;
    private Button btnGolLocal;
    private Button btnGolVisitante;
    private Button btnMenosLocal;
    private Button btnMenosVisitante;
    private Label lblAyudaListas;
    private Label lblComboJugadoresLocal;
    private ComboBox cmbJugadorLocalGol;
    private Button btnAnadirGolLocalCombo;
    private Label lblComboJugadoresVisitante;
    private ComboBox cmbJugadorVisitanteGol;
    private Button btnAnadirGolVisitanteCombo;
    private Label lblListaGolesLocal;
    private ListBox lstGolesLocal;
    private Button btnQuitarGolLocal;
    private Label lblListaGolesVisitante;
    private ListBox lstGolesVisitante;
    private Button btnQuitarGolVisitante;
    private PropertyGrid propertyGridLayout;
    private Button btnGuardarLayoutMarcador;
    private Button btnLayoutPredeterminado;
    private GroupBox grpManual;
    private PictureBox picPreviewLogoLocal;
    private PictureBox picPreviewLogoVisitante;
    private TextBox txtNombreVisitante;
    private TextBox txtNombreLocal;
    private Button btnLogoVisitante;
    private Button btnLogoLocal;
    private Button btnAplicarManual;
    private Label lblTextoGolManualLocal;
    private TextBox txtTextoGolManualLocal;
    private Label lblTextoGolManualVisitante;
    private TextBox txtTextoGolManualVisitante;
}
