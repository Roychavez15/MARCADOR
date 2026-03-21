namespace Marcador.Display;

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
        pnlMarcador = new Panel();
        lblTituloLiga = new Label();
        lblEtapa = new Label();
        lblPeriodo = new Label();
        lblNombreLocal = new Label();
        lblNombreVisitante = new Label();
        lblCronometro = new Label();
        picLogoLocal = new PictureBox();
        picLogoVisitante = new PictureBox();
        lblGolesLocal = new Label();
        lblGolesVisitante = new Label();
        pnlCelebracion = new Panel();
        picSplashGol = new PictureBox();
        lblGolNombre = new Label();
        lblGolNumero = new Label();
        lblGolPartidoValor = new Label();
        lblGolCampValor = new Label();
        picGolEscudo = new PictureBox();
        picGolFoto = new PictureBox();
        lblGolEtiquetaPartido = new Label();
        lblGolEtiquetaCamp = new Label();
        timerRefresh = new System.Windows.Forms.Timer(components);

        ((System.ComponentModel.ISupportInitialize)picLogoLocal).BeginInit();
        ((System.ComponentModel.ISupportInitialize)picLogoVisitante).BeginInit();
        pnlCelebracion.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)picGolEscudo).BeginInit();
        ((System.ComponentModel.ISupportInitialize)picGolFoto).BeginInit();
        ((System.ComponentModel.ISupportInitialize)picSplashGol).BeginInit();
        pnlMarcador.SuspendLayout();
        SuspendLayout();

        const int W = 256;
        const int H = 236;

        pnlMarcador.BackColor = Color.Black;
        pnlMarcador.Size = new Size(W, H);
        pnlMarcador.Location = new Point(0, 0);

        lblTituloLiga.AutoSize = false;
        lblTituloLiga.Font = new Font("Microsoft Sans Serif", 11F, FontStyle.Bold);
        lblTituloLiga.ForeColor = Color.White;
        lblTituloLiga.Location = new Point(0, 1);
        lblTituloLiga.Size = new Size(W, 29);
        lblTituloLiga.Text = "LIGA GUAPULO";
        lblTituloLiga.TextAlign = ContentAlignment.MiddleCenter;

        lblEtapa.Font = new Font("Comic Sans MS", 11F, FontStyle.Bold);
        lblEtapa.ForeColor = Color.White;
        lblEtapa.Location = new Point(1, 32);
        lblEtapa.Size = new Size(251, 23);
        lblEtapa.Text = "SEGUNDA ETAPA";
        lblEtapa.TextAlign = ContentAlignment.MiddleCenter;

        lblPeriodo.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold);
        lblPeriodo.ForeColor = SystemColors.ButtonFace;
        lblPeriodo.Location = new Point(7, 65);
        lblPeriodo.Size = new Size(245, 21);
        lblPeriodo.Text = "PT";
        lblPeriodo.TextAlign = ContentAlignment.MiddleCenter;

        lblNombreLocal.AutoSize = false;
        lblNombreLocal.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold);
        lblNombreLocal.ForeColor = Color.White;
        lblNombreLocal.Location = new Point(1, 38);
        lblNombreLocal.Size = new Size(121, 16);
        lblNombreLocal.Text = "Local";
        lblNombreLocal.TextAlign = ContentAlignment.TopCenter;

        lblNombreVisitante.AutoSize = false;
        lblNombreVisitante.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold);
        lblNombreVisitante.ForeColor = Color.White;
        lblNombreVisitante.Location = new Point(131, 38);
        lblNombreVisitante.Size = new Size(121, 16);
        lblNombreVisitante.Text = "Visitante";
        lblNombreVisitante.TextAlign = ContentAlignment.TopCenter;

        picLogoLocal.Location = new Point(1, 90);
        picLogoLocal.Size = new Size(121, 32);
        picLogoLocal.SizeMode = PictureBoxSizeMode.Zoom;
        picLogoLocal.BackColor = Color.Black;

        picLogoVisitante.Location = new Point(131, 90);
        picLogoVisitante.Size = new Size(121, 32);
        picLogoVisitante.SizeMode = PictureBoxSizeMode.Zoom;
        picLogoVisitante.BackColor = Color.Black;

        lblGolesLocal.Font = new Font("Microsoft Sans Serif", 22F, FontStyle.Bold);
        lblGolesLocal.ForeColor = Color.White;
        lblGolesLocal.Location = new Point(16, 129);
        lblGolesLocal.Size = new Size(93, 46);
        lblGolesLocal.Text = "0";
        lblGolesLocal.TextAlign = ContentAlignment.MiddleCenter;

        lblGolesVisitante.Font = new Font("Microsoft Sans Serif", 22F, FontStyle.Bold);
        lblGolesVisitante.ForeColor = Color.White;
        lblGolesVisitante.Location = new Point(147, 129);
        lblGolesVisitante.Size = new Size(93, 46);
        lblGolesVisitante.Text = "0";
        lblGolesVisitante.TextAlign = ContentAlignment.MiddleCenter;

        lblCronometro.AutoSize = true;
        lblCronometro.Font = new Font("Microsoft Sans Serif", 16F, FontStyle.Bold);
        lblCronometro.ForeColor = SystemColors.ButtonHighlight;
        lblCronometro.Location = new Point(79, 175);
        lblCronometro.Text = "00:00";

        pnlMarcador.Controls.Add(lblTituloLiga);
        pnlMarcador.Controls.Add(lblEtapa);
        pnlMarcador.Controls.Add(lblPeriodo);
        pnlMarcador.Controls.Add(lblNombreLocal);
        pnlMarcador.Controls.Add(lblNombreVisitante);
        pnlMarcador.Controls.Add(picLogoLocal);
        pnlMarcador.Controls.Add(picLogoVisitante);
        pnlMarcador.Controls.Add(lblGolesLocal);
        pnlMarcador.Controls.Add(lblGolesVisitante);
        pnlMarcador.Controls.Add(lblCronometro);

        pnlCelebracion.BackColor = Color.Black;
        pnlCelebracion.Location = new Point(0, 0);
        pnlCelebracion.Size = new Size(W, H);
        pnlCelebracion.Visible = false;
        // 
        // picSplashGol
        // 
        picSplashGol.BackColor = Color.Black;
        picSplashGol.Dock = DockStyle.Fill;
        picSplashGol.Location = new Point(0, 0);
        picSplashGol.Name = "picSplashGol";
        picSplashGol.Size = new Size(W, H);
        picSplashGol.SizeMode = PictureBoxSizeMode.Zoom;
        picSplashGol.TabStop = false;
        picSplashGol.Visible = false;
        pnlCelebracion.Controls.Add(picSplashGol);
        pnlCelebracion.Controls.Add(picGolFoto);
        pnlCelebracion.Controls.Add(picGolEscudo);
        pnlCelebracion.Controls.Add(lblGolEtiquetaCamp);
        pnlCelebracion.Controls.Add(lblGolEtiquetaPartido);
        pnlCelebracion.Controls.Add(lblGolCampValor);
        pnlCelebracion.Controls.Add(lblGolPartidoValor);
        pnlCelebracion.Controls.Add(lblGolNumero);
        pnlCelebracion.Controls.Add(lblGolNombre);

        lblGolNombre.Font = new Font("Arial", 11.25F, FontStyle.Bold);
        lblGolNombre.ForeColor = SystemColors.Control;
        lblGolNombre.Location = new Point(4, 25);
        lblGolNombre.Size = new Size(248, 32);
        lblGolNombre.Text = "Jugador";
        lblGolNombre.TextAlign = ContentAlignment.TopCenter;

        lblGolNumero.Font = new Font("Arial", 20.25F, FontStyle.Bold);
        lblGolNumero.ForeColor = SystemColors.Control;
        lblGolNumero.Location = new Point(7, 133);
        lblGolNumero.Size = new Size(89, 40);
        lblGolNumero.Text = "99";
        lblGolNumero.TextAlign = ContentAlignment.MiddleCenter;

        picGolEscudo.Location = new Point(5, 60);
        picGolEscudo.Size = new Size(89, 70);
        picGolEscudo.SizeMode = PictureBoxSizeMode.Zoom;

        picGolFoto.Location = new Point(102, 60);
        picGolFoto.Size = new Size(145, 111);
        picGolFoto.SizeMode = PictureBoxSizeMode.Zoom;

        lblGolEtiquetaPartido.Font = new Font("Arial Narrow", 9.75F, FontStyle.Bold);
        lblGolEtiquetaPartido.ForeColor = SystemColors.Control;
        lblGolEtiquetaPartido.Location = new Point(4, 180);
        lblGolEtiquetaPartido.Text = "GOLES PARTIDO:";

        lblGolEtiquetaCamp.Font = new Font("Arial Narrow", 9.75F, FontStyle.Bold);
        lblGolEtiquetaCamp.ForeColor = SystemColors.Control;
        lblGolEtiquetaCamp.Location = new Point(4, 202);
        lblGolEtiquetaCamp.Text = "GOLES CAMPEONATO:";

        lblGolPartidoValor.Font = new Font("Arial Narrow", 9.75F, FontStyle.Bold);
        lblGolPartidoValor.ForeColor = SystemColors.Control;
        lblGolPartidoValor.Location = new Point(172, 180);
        lblGolPartidoValor.Text = "00";

        lblGolCampValor.Font = new Font("Arial Narrow", 9.75F, FontStyle.Bold);
        lblGolCampValor.ForeColor = SystemColors.Control;
        lblGolCampValor.Location = new Point(172, 202);
        lblGolCampValor.Text = "00";

        timerRefresh.Interval = 250;
        timerRefresh.Tick += TimerRefresh_Tick;

        AutoScaleMode = AutoScaleMode.None;
        BackColor = Color.Black;
        ClientSize = new Size(W, H);
        MinimumSize = new Size(W, H);
        MaximumSize = new Size(W, H);
        Controls.Add(pnlMarcador);
        Controls.Add(pnlCelebracion);
        FormBorderStyle = FormBorderStyle.None;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "Form1";
        Location = new Point(0, 0);
        StartPosition = FormStartPosition.Manual;
        Text = "Marcador";
        DoubleClick += Form1_DoubleClick;

        pnlCelebracion.ResumeLayout(false);
        pnlCelebracion.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)picLogoLocal).EndInit();
        ((System.ComponentModel.ISupportInitialize)picLogoVisitante).EndInit();
        ((System.ComponentModel.ISupportInitialize)picGolEscudo).EndInit();
        ((System.ComponentModel.ISupportInitialize)picGolFoto).EndInit();
        ((System.ComponentModel.ISupportInitialize)picSplashGol).EndInit();
        pnlMarcador.ResumeLayout(false);
        pnlMarcador.PerformLayout();
        ResumeLayout(false);
    }

    internal Panel pnlMarcador;
    internal Label lblTituloLiga;
    internal Label lblEtapa;
    internal Label lblPeriodo;
    internal Label lblNombreLocal;
    internal Label lblNombreVisitante;
    internal Label lblCronometro;
    internal PictureBox picLogoLocal;
    internal PictureBox picLogoVisitante;
    internal Label lblGolesLocal;
    internal Label lblGolesVisitante;
    internal Panel pnlCelebracion;
    internal PictureBox picSplashGol;
    internal Label lblGolNombre;
    internal Label lblGolNumero;
    internal Label lblGolPartidoValor;
    internal Label lblGolCampValor;
    internal PictureBox picGolEscudo;
    internal PictureBox picGolFoto;
    internal Label lblGolEtiquetaPartido;
    internal Label lblGolEtiquetaCamp;
    internal System.Windows.Forms.Timer timerRefresh;
}
