using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;



namespace ürüntakip
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        SqlConnection baglanti = new SqlConnection("Data Source=DESKTOP-OTMDOAG\\SQLEXPRESS;Initial Catalog=urunstok;Integrated Security=True;");

        public void temizle()
        {
            SqlCommand komut = new SqlCommand();
            SqlDataReader dr;
            komut.Connection = baglanti;
            komut.CommandType = CommandType.Text;

            textBox1.Text = "";
            textBox2.Text = "";
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            comboBox3.Items.Clear();
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            radioButton3.Checked = false;

            comboBox1.Items.Clear();
            komut.CommandText = "SELECT *FROM dolap";
            baglanti.Open();
            dr = komut.ExecuteReader();
            while (dr.Read())
            {
                comboBox1.Items.Add(dr["dolap_ad"]);
            }
            baglanti.Close();
            comboBox2.Items.Clear();
            komut.CommandText = "SELECT *FROM uruntur";
            baglanti.Open();
            dr = komut.ExecuteReader();
            while (dr.Read())
            {
                comboBox3.Items.Add(dr["tur_ad"]);
            }
            baglanti.Close();
        }
        public string dolap_id()
        {
            if (baglanti.State == ConnectionState.Open)
                baglanti.Close();
            string id = "";
            SqlCommand komut = new SqlCommand();
            komut.Connection = baglanti;
            komut.CommandType = CommandType.Text;
            SqlDataReader dr;

            baglanti.Open();
            komut.CommandText = "SELECT *FROM dolap WHERE dolap_ad ='" + comboBox1.Text + "'";
            dr = komut.ExecuteReader();
            dr.Read();
            id = dr["id"].ToString();
            baglanti.Close();
            return id;
        }
        public string raf_id(string dolap)
        {
            if (baglanti.State == ConnectionState.Open)
                baglanti.Close();
            string id = "";
            string dp = dolap;
            SqlCommand komut = new SqlCommand();
            komut.Connection = baglanti;
            komut.CommandType = CommandType.Text;
            SqlDataReader dr;
            baglanti.Open();
            komut.CommandText = "SELECT *FROM raf WHERE raf_ad ='" + comboBox2.Text + "' and dolap_id =" + dp;
            dr = komut.ExecuteReader();
            dr.Read();
            id = dr["id"].ToString();
            baglanti.Close();
            return id;
        }
        public string durum_bul()
        {
            if (baglanti.State == ConnectionState.Open)
                baglanti.Close();

            string durum = "";
            if (radioButton1.Checked == true)
                durum = radioButton1.Text;
            if (radioButton2.Checked == true)
                durum = radioButton2.Text;
            if (radioButton3.Checked == true)
                durum = radioButton3.Text;
            return durum;
        }
        public string tur_id()
        {
            if (baglanti.State == ConnectionState.Open)
                baglanti.Close();

            SqlCommand komut = new SqlCommand();
            SqlDataReader dr;
            komut.Connection = baglanti;
            komut.CommandType = CommandType.Text;
            if (comboBox3.Text != "")
            {
                string tur = "";
                baglanti.Open();
                komut.CommandText = "SELECT *FROM uruntur WHERE tur_ad ='" + comboBox3.Text + "'";
                dr = komut.ExecuteReader();
                dr.Read();
                tur = dr["id"].ToString();
                baglanti.Close();
                return tur;
            }
            else
                return "";
        }
        public int var_mi(string raf)
        {
            if (baglanti.State == ConnectionState.Open)
                baglanti.Close();

            int sayi = 0;
            SqlCommand komut = new SqlCommand();
            SqlDataReader dr;
            komut.Connection = baglanti;
            komut.CommandType = CommandType.Text;
            string durum = durum_bul();
            string tur = tur_id();
            baglanti.Open();
            komut.CommandText = "SELECT * FROM urun";
            dr = komut.ExecuteReader();
            while (dr.Read())
            {
                if (textBox1.Text == dr["urun_ad"].ToString() && raf == dr["raf_id"].ToString() && tur == dr["tur_id"].ToString() && durum == dr["urun_durum"].ToString())
                    sayi = Convert.ToInt32(dr["id"]);
                else
                    sayi = 0;
            }
            baglanti.Close();
            return sayi;
        }
        public int ürün_adet(string raf)
        {
            if (baglanti.State == ConnectionState.Open)
                baglanti.Close();
            int adet;
            string rf = raf;
            SqlCommand komut = new SqlCommand();
            SqlDataReader dr;
            komut.Connection = baglanti;
            komut.CommandType = CommandType.Text;
            baglanti.Open();
            komut.CommandText = "SELECT * FROM urun WHERE urun_ad ='" + textBox1.Text + "' and raf_id =" + rf;
            dr = komut.ExecuteReader();
            dr.Read();
            adet = Convert.ToInt32(dr["urun_adet"]);
            baglanti.Close();
            return adet;
        }
        public int id_bul()
        {
            SqlCommand komut = new SqlCommand();
            SqlDataReader dr;
            komut.Connection = baglanti;
            komut.CommandType = CommandType.Text;
            int sayac = 1;
            baglanti.Open();
            komut.CommandText = "select * from urun";
            dr = komut.ExecuteReader();
            while (dr.Read())
            {
                if (Convert.ToInt32(dr["id"]) == sayac)
                {
                    sayac++;
                }
                else
                {
                    break;

                }
            }
            baglanti.Close();
            return sayac;
        }
        private void Form2_Load(object sender, EventArgs e)
        {
            SqlCommand komut = new SqlCommand();
            komut.Connection = baglanti;
            komut.CommandType = CommandType.Text;
            SqlDataReader dr;

            this.Text = "ÜRÜN EKLEME";
            label1.Text = "ÜRÜNÜN ADI: ";
            label2.Text = "DOLAP NUMARASI: ";
            label3.Text = "RAF NUMARASI: ";
            label4.Text = "ÜRÜN SAYISI: ";
            label5.Text = "ÜRÜNÜN TÜRÜ";
            button1.Text = "EKLE";
            button2.Text = "ÇIKIŞ";
            groupBox1.Text = "ÜRÜNÜN DURUMU: ";
            radioButton1.Text = "SAĞLAM";
            radioButton2.Text = "HASARLI";
            radioButton3.Text = "BOZUK";
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox3.DropDownStyle = ComboBoxStyle.DropDownList;

            komut.CommandText = "SELECT *FROM dolap";
            baglanti.Open();
            dr = komut.ExecuteReader();
            while (dr.Read())
            {
                comboBox1.Items.Add(dr["dolap_ad"]);
            }
            baglanti.Close();

            komut.CommandText = "SELECT *FROM uruntur";
            baglanti.Open();
            dr = komut.ExecuteReader();
            while (dr.Read())
            {
                comboBox3.Items.Add(dr["tur_ad"]);
            }
            baglanti.Close();

            if (Convert.ToInt32(Form1.ürün_id) != 0)
            {
                textBox1.Enabled = false;
                comboBox1.Enabled = false;
                comboBox2.Enabled = false;
                comboBox3.Enabled = false;
                groupBox1.Enabled = false;
                textBox2.Enabled = true;
            }
            else
            {
                textBox1.Text = Form1.ad;
                textBox2.Text = Form1.adet;
                comboBox1.Text = Form1.ürün_dlp;
                comboBox2.Text = Form1.ürün_raf;
                comboBox3.Text = Form1.ürün_tür;
                if (Form1.ürün_durum == "SAĞLAM")
                {
                    radioButton1.Checked = true;
                }
                if (Form1.ürün_durum == "HASARLI")
                {
                    radioButton2.Checked = true;
                }
                if (Form1.ürün_durum == "BOZUK")
                {
                    radioButton3.Checked = true;
                }
                textBox1.Enabled = true;
                comboBox1.Enabled = true;
                comboBox2.Enabled = true;
                comboBox3.Enabled = true;
                groupBox1.Enabled = true;
                textBox2.Enabled = true;
            }

        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //combobox1 de seçilen bilgiye göre combobox2 ye raf bilgilerinin girilmesi
            int sayac = 0;
            SqlCommand komut = new SqlCommand();
            SqlDataReader dr;
            komut.Connection = baglanti;
            komut.CommandType = CommandType.Text;
            baglanti.Open();
            komut.CommandText = "SELECT *FROM dolap";
            dr = komut.ExecuteReader();
            while (dr.Read())
            {
                sayac++;
            }
            baglanti.Close();

            baglanti.Open();
            for (int i = 1; i <= sayac; i++)
            {
                if (comboBox1.Text == "Dolap " + i.ToString())
                {
                    comboBox2.Items.Clear();
                    komut.CommandText = "SELECT *FROM raf WHERE dolap_id=" + i.ToString();
                }
            }
            dr = komut.ExecuteReader();
            while (dr.Read())
            {
                comboBox2.Items.Add(dr["raf_ad"]);
            }
            baglanti.Close();
        }
        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }
        private void button1_Click(object sender, EventArgs e)
        {

            SqlCommand komut = new SqlCommand();
            SqlDataReader dr;
            komut.Connection = baglanti;
            komut.CommandType = CommandType.Text;
            string kayit = "";
            // ürün varsa yapılacak işlemler
            if (Convert.ToInt32(Form1.ürün_id) != 0)
            {

                // ürünün adet bilgisinin alınması
                baglanti.Open();
                komut.CommandText = "select * from urun where id =" + Form1.ürün_id;
                dr = komut.ExecuteReader();
                dr.Read();
                int adet = Convert.ToInt32(dr["urun_adet"]);
                baglanti.Close();

                //textbox2 ye değer girilmediyse ürün sayısı 1 arttırılıyor
                if (textBox2.Text == "")
                {
                    adet++;
                    DialogResult dialog = new DialogResult();
                    dialog = MessageBox.Show("Böyle Bir Ürün Var. Ürünün Sayısı 1 Arttırılacak. \n Emin misiniz ?.", "ÇIKIŞ", MessageBoxButtons.YesNo);
                    if (dialog == DialogResult.Yes)
                    {
                        baglanti.Open();
                        adet++;
                        kayit = "Update urun set urun_adet=" + adet.ToString() + " where id=" + Form1.ürün_id;
                        komut.CommandText = kayit;
                        komut.ExecuteNonQuery();
                        baglanti.Close();
                        MessageBox.Show(" ( " + Form1.ad + " ) Adlı Ürünün satısı 1 defa arttırıldı.");
                    }
                    else
                        this.Close();
                }
                else
                {
                    adet += Convert.ToInt32(textBox2.Text);

                    DialogResult dialog = new DialogResult();
                    dialog = MessageBox.Show("Böyle Bir Ürün Var. Ürünün Sayısı " + Convert.ToInt32(textBox2.Text) + " Kadar Arttırılacak. \n Emin misiniz ?.", "ÇIKIŞ", MessageBoxButtons.YesNo);
                    if (dialog == DialogResult.Yes)
                    {
                        //textbox2 ye değer girildiyse girilen değer kadar ürün sayısı  arttırılıyor
                        baglanti.Open();
                        kayit = "Update urun set urun_adet=" + adet.ToString() + "  where id=" + Form1.ürün_id;
                        komut.CommandText = kayit;
                        komut.ExecuteNonQuery();
                        baglanti.Close();
                        MessageBox.Show(" ( " + Form1.ad + " ) Adlı Ürünün satısı " + textBox2.Text + " kadar arttırıldı.");
                    }
                    else
                        this.Close();

                }
                Form1.ürün_id = "0";
                temizle();
                this.Close();


            }

            if (baglanti.State == ConnectionState.Open)
                baglanti.Close();
            temizle();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
