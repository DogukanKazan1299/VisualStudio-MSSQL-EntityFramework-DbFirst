using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace EntityOrnek
{
    public partial class Form1 : Form
    {
        DbSinavOgrenciEntities db = new DbSinavOgrenciEntities();
        public Form1()
        {
            InitializeComponent();
        }


        private void BtnDersListesi_Click(object sender, EventArgs e)
        {
            SqlConnection baglanti = new SqlConnection(@"Data Source=DESKTOP-6AC6160;Initial Catalog=DbSinavOgrenci;Integrated Security=True");
            SqlCommand komut = new SqlCommand("select * from TblDersler", baglanti);
            SqlDataAdapter da = new SqlDataAdapter(komut);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        private void BtnOgrenciListele_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = db.TblOgrenci.ToList();
        }

        private void BtnNotListesi_Click(object sender, EventArgs e)
        {
            var query = from item in db.TblNotlar
                        select new
                        {
                            item.Notid,
                            item.TblOgrenci.ad,
                            item.TblDersler.Dersad,
                            item.Sinav1,
                            item.Sinav2,
                            item.Sinav3,
                            item.Ortalama,
                            item.Durum
                        };
            dataGridView1.DataSource = query.ToList();
        }

        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            TblOgrenci t = new TblOgrenci();
            t.ad = TxtAd.Text;
            t.soyad = TxtSoyad.Text;
            db.TblOgrenci.Add(t);
            db.SaveChanges();
            MessageBox.Show("Yeni Öğrenci Eklendi..");
        }

        private void BtnSil_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(TxtOgrenciid.Text);
            var x = db.TblOgrenci.Find(id);
            db.TblOgrenci.Remove(x);
            db.SaveChanges();
            MessageBox.Show("Öğrenci Silme Başarılı");
        }

        private void BtnGuncelle_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(TxtOgrenciid.Text);
            var x = db.TblOgrenci.Find(id);
            x.ad = TxtAd.Text;
            x.soyad = TxtSoyad.Text;
            x.fotograf = TxtFotograf.Text;
            db.SaveChanges();
            MessageBox.Show("Öğrenci Güncelleme İşlemi Basarili");
        }

        private void BtnProsedur_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = db.NotListesi();
        }

        private void BtnBul_Click(object sender, EventArgs e)
        {
            //dataGridView1.DataSource = db.TblOgrenci.Where(x => x.ad == TxtAd.Text).ToList();
            dataGridView1.DataSource = db.TblOgrenci.Where(x => x.ad == TxtAd.Text | x.soyad == TxtSoyad.Text).ToList();
        }

        private void TxtAd_TextChanged(object sender, EventArgs e)
        {
            string aranan = TxtAd.Text;
            var degerler = from item in db.TblOgrenci where item.ad.Contains(aranan) select item;
            dataGridView1.DataSource = degerler.ToList();
        }

        private void BtnLinqEntity_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                List<TblOgrenci> liste1 = db.TblOgrenci.OrderBy(p => p.ad).ToList();
                dataGridView1.DataSource = liste1;

            }
            if (radioButton2.Checked == true)
            {
                List<TblOgrenci> liste2 = db.TblOgrenci.OrderByDescending(p => p.ad).ToList();
                dataGridView1.DataSource = liste2;
            }
            if (radioButton3.Checked == true)
            {
                List<TblOgrenci> liste3 = db.TblOgrenci.OrderBy(p => p.ad).Take(3).ToList();
                dataGridView1.DataSource = liste3;
            }
            if (radioButton4.Checked == true)
            {
                List<TblOgrenci> liste4 = db.TblOgrenci.Where(p => p.id == 9).ToList();
                dataGridView1.DataSource = liste4;

            }
            if (radioButton5.Checked == true)
            {
                List<TblOgrenci> liste5 = db.TblOgrenci.Where(p => p.ad.StartsWith("a")).ToList();
                dataGridView1.DataSource = liste5;
            }
            if (radioButton6.Checked == true)
            {
                List<TblOgrenci> liste6 = db.TblOgrenci.Where(p => p.ad.EndsWith("a")).ToList();
                dataGridView1.DataSource = liste6;
            }
            if (radioButton7.Checked == true)
            {
                bool deger = db.TblKulupler.Any();
                MessageBox.Show(deger.ToString(), "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            if (radioButton8.Checked == true)
            {
                int toplam = db.TblOgrenci.Count();
                MessageBox.Show(toplam.ToString(), "Toplam Öğrenci Sayısı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (radioButton9.Checked == true)
            {
                var toplam = db.TblNotlar.Sum(p => p.Sinav1);
                MessageBox.Show("Sınav 1 Toplam Puan = " + toplam.ToString());
            }
            if (radioButton10.Checked == true)
            {
                var ortalama = db.TblNotlar.Average(p => p.Sinav1);
                MessageBox.Show("Sınav 1 Ortalama Puan = " + ortalama.ToString());
            }
            if (radioButton11.Checked == true)
            {
                var enyuksek = db.TblNotlar.Max(p => p.Sinav1);
                MessageBox.Show("Sınav 1 Max Puan = " + enyuksek);
            }
            if (radioButton12.Checked == true)
            {
                var endusuk = db.TblNotlar.Min(p => p.Sinav1);
                MessageBox.Show("Sınav 1 Min Puan = " + endusuk);
            }
        }

        private void BtnJoin_Click(object sender, EventArgs e)
        {
            var sorgu = from d1 in db.TblNotlar
                        join d2 in db.TblOgrenci
                        on d1.Ogrid equals d2.id
                        select new
                        {
                            Öğrenci = d2.ad,
                            Soyad = d2.soyad,
                            Sınav1 = d1.Sinav1,
                            Sınav2 = d1.Sinav2,
                            Sınav3 = d1.Sinav3,
                            Ortalama = d1.Ortalama

                        };
            dataGridView1.DataSource = sorgu.ToList();
        }
    }
}
