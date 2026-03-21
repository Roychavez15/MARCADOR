namespace Marcador.Admin;

/// <summary>Diálogo de confirmación con cabecera destacada (mejor que MessageBox plano).</summary>
internal sealed class ConfirmacionAccionForm : Form
{
    public static bool Mostrar(Form? owner, string tituloBarra, string titulo, string mensaje, string textoAceptar)
    {
        using var f = new ConfirmacionAccionForm(tituloBarra, titulo, mensaje, textoAceptar);
        if (owner != null)
        {
            f.Owner = owner;
            f.StartPosition = FormStartPosition.CenterParent;
        }
        else
            f.StartPosition = FormStartPosition.CenterScreen;
        return f.ShowDialog(owner) == DialogResult.Yes;
    }

    private ConfirmacionAccionForm(string tituloBarra, string titulo, string mensaje, string textoAceptar)
    {
        Text = tituloBarra;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MinimizeBox = false;
        MaximizeBox = false;
        ShowInTaskbar = false;
        BackColor = Color.White;
        Font = new Font("Segoe UI", 9.75F);
        ClientSize = new Size(440, 230);
        var azul = Color.FromArgb(0, 103, 184);

        var header = new Panel
        {
            Location = Point.Empty,
            Size = new Size(440, 48),
            BackColor = azul
        };
        header.Controls.Add(new Label
        {
            Text = titulo,
            ForeColor = Color.White,
            Font = new Font("Segoe UI", 12F, FontStyle.Bold),
            AutoSize = true,
            Location = new Point(16, 13)
        });

        var lblBody = new Label
        {
            Text = mensaje,
            Location = new Point(20, 56),
            MaximumSize = new Size(400, 0),
            AutoSize = true,
            ForeColor = Color.FromArgb(64, 64, 64)
        };

        var btnCancelar = new Button
        {
            Text = "Cancelar",
            DialogResult = DialogResult.No,
            Location = new Point(220, 182),
            Size = new Size(100, 32),
            Anchor = AnchorStyles.Bottom | AnchorStyles.Right,
            UseVisualStyleBackColor = true,
            Cursor = Cursors.Hand
        };
        var btnAceptar = new Button
        {
            Text = textoAceptar,
            DialogResult = DialogResult.Yes,
            Location = new Point(328, 182),
            Size = new Size(100, 32),
            Anchor = AnchorStyles.Bottom | AnchorStyles.Right,
            BackColor = azul,
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            UseVisualStyleBackColor = false,
            Cursor = Cursors.Hand
        };
        btnAceptar.FlatAppearance.BorderSize = 0;

        Controls.Add(btnAceptar);
        Controls.Add(btnCancelar);
        Controls.Add(lblBody);
        Controls.Add(header);

        AcceptButton = btnAceptar;
        CancelButton = btnCancelar;
    }
}
