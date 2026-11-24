using Microsoft.Data.Sqlite;

public class Form1 : Form
{
    string cs = "Data Source=ogrenci.db";
    TextBox txtAd = new(), txtSoyad = new(), txtNo = new(), txtVize = new(), txtFinal = new();
    DataGridView grid = new();
    int seciliId = -1;

    public Form1()
    {
        Text = "Öğrenci Not Takip Sistemi";
        Width = 820; Height = 480;
        StartPosition = FormStartPosition.CenterScreen;

        string[] etiket = { "Ad", "Soyad", "Numara", "Vize", "Final" };
        TextBox[] kutu = { txtAd, txtSoyad, txtNo, txtVize, txtFinal };
        for (int i = 0; i < 5; i++)
        {
            Controls.Add(new Label { Text = etiket[i], Left = 20, Top = 22 + i * 35, Width = 70 });
            kutu[i].SetBounds(100, 19 + i * 35, 150, 25);
            Controls.Add(kutu[i]);
        }

        Buton("Ekle", 210, Ekle);
        Buton("Güncelle", 245, Guncelle);
        Buton("Sil", 280, Sil);
        Buton("Temizle", 315, Temizle);

        grid.SetBounds(280, 20, 500, 400);
        grid.ReadOnly = true;
        grid.AllowUserToAddRows = false;
        grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        grid.CellClick += GridTikla;
        Controls.Add(grid);

        Calistir("CREATE TABLE IF NOT EXISTS Ogrenciler(Id INTEGER PRIMARY KEY AUTOINCREMENT, Ad TEXT, Soyad TEXT, Numara TEXT, Vize REAL, Final REAL)");
        Listele();
    }

    void Buton(string yazi, int top, EventHandler olay)
    {
        var b = new Button { Text = yazi, Left = 100, Top = top, Width = 150, Height = 30 };
        b.Click += olay;
        Controls.Add(b);
    }

    void Calistir(string sql, params (string, object)[] p)
    {
        using var con = new SqliteConnection(cs);
        con.Open();
        using var cmd = new SqliteCommand(sql, con);
        foreach (var (ad, deger) in p) cmd.Parameters.AddWithValue(ad, deger);
        cmd.ExecuteNonQuery();
    }

    void Listele()
    {
        using var con = new SqliteConnection(cs);
        con.Open();
        string sql = @"SELECT Id, Ad, Soyad, Numara, Vize, Final,
                       ROUND(Vize*0.4+Final*0.6,2) AS Ortalama,
                       CASE WHEN Vize*0.4+Final*0.6>=50 THEN 'Geçti' ELSE 'Kaldı' END AS Durum
                       FROM Ogrenciler";
        using var cmd = new SqliteCommand(sql, con);
        var dt = new System.Data.DataTable();
        dt.Load(cmd.ExecuteReader());
        grid.DataSource = dt;
    }

    bool Kontrol(out double vize, out double fin)
    {
        vize = fin = 0;
        if (txtAd.Text == "" || txtSoyad.Text == "" || txtNo.Text == "")
        { MessageBox.Show("Ad, soyad ve numara boş olamaz."); return false; }
        var harfler = new System.Text.RegularExpressions.Regex(@"^[\p{L} ]+$");
        if (!harfler.IsMatch(txtAd.Text.Trim()))
        { MessageBox.Show("Ad sadece harflerden oluşmalı."); return false; }
        if (!harfler.IsMatch(txtSoyad.Text.Trim()))
        { MessageBox.Show("Soyad sadece harflerden oluşmalı."); return false; }
        if (!double.TryParse(txtVize.Text, out vize) || !double.TryParse(txtFinal.Text, out fin)
            || vize < 0 || vize > 100 || fin < 0 || fin > 100)
        { MessageBox.Show("Notlar 0 ile 100 arasında sayı olmalı."); return false; }
        return true;
    }

    void Ekle(object? s, EventArgs e)
    {
        if (!Kontrol(out var v, out var f)) return;
        Calistir("INSERT INTO Ogrenciler(Ad,Soyad,Numara,Vize,Final) VALUES(@a,@s,@n,@v,@f)",
            ("@a", txtAd.Text), ("@s", txtSoyad.Text), ("@n", txtNo.Text), ("@v", v), ("@f", f));
        Listele(); Temizle(null, EventArgs.Empty);
    }

    void Guncelle(object? s, EventArgs e)
    {
        if (seciliId < 0) { MessageBox.Show("Önce tablodan bir öğrenci seç."); return; }
        if (!Kontrol(out var v, out var f)) return;
        Calistir("UPDATE Ogrenciler SET Ad=@a,Soyad=@s,Numara=@n,Vize=@v,Final=@f WHERE Id=@id",
            ("@a", txtAd.Text), ("@s", txtSoyad.Text), ("@n", txtNo.Text), ("@v", v), ("@f", f), ("@id", seciliId));
        Listele(); Temizle(null, EventArgs.Empty);
    }

    void Sil(object? s, EventArgs e)
    {
        if (seciliId < 0) { MessageBox.Show("Önce tablodan bir öğrenci seç."); return; }
        if (MessageBox.Show("Silinsin mi?", "Onay", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
        Calistir("DELETE FROM Ogrenciler WHERE Id=@id", ("@id", seciliId));
        Listele(); Temizle(null, EventArgs.Empty);
    }

    void Temizle(object? s, EventArgs e)
    {
        foreach (var k in new[] { txtAd, txtSoyad, txtNo, txtVize, txtFinal }) k.Clear();
        seciliId = -1;
    }

    void GridTikla(object? s, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;
        var r = grid.Rows[e.RowIndex];
        seciliId = Convert.ToInt32(r.Cells["Id"].Value);
        txtAd.Text = r.Cells["Ad"].Value?.ToString();
        txtSoyad.Text = r.Cells["Soyad"].Value?.ToString();
        txtNo.Text = r.Cells["Numara"].Value?.ToString();
        txtVize.Text = r.Cells["Vize"].Value?.ToString();
        txtFinal.Text = r.Cells["Final"].Value?.ToString();
    }
}
