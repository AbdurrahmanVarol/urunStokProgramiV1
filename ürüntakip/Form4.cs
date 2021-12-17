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
    public partial class Form4 : Form
    {
        public Form4()
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
        public string dolap_id(string str)
        {
            if (baglanti.State == ConnectionState.Open)
                baglanti.Close();

            string id = "";
            SqlCommand komut = new SqlCommand();
            komut.Connection = baglanti;
            komut.CommandType = CommandType.Text;
            SqlDataReader dr;

            baglanti.Open();
            komut.CommandText = "SELECT *FROM dolap WHERE dolap_ad ='" + str + "'";
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
            }
            baglanti.Close();
            return sayi;
        }
        private void Form4_Load(object sender, EventArgs e)
        {
            SqlCommand komut = new SqlCommand();
            komut.Connection = baglanti;
            komut.CommandType = CommandType.Text;
            SqlDataReader dr;

            label1.Text = "ÜRÜNÜN ADI: ";
            label2.Text = "DOLAP NUMARASI: ";
            label3.Text = "RAF NUMARASI: ";
            label4.Text = "DEĞİŞECEK ÜRÜN SAYISI: ";
            label5.Text = "ÜRÜNÜN TÜRÜ";
            button1.Text = "GÜNCELLE";
            button2.Text = "ÇIKIŞ";
            groupBox1.Text = "ÜRÜNÜN DURUMU: ";
            radioButton1.Text = "SAĞLAM";
            radioButton2.Text = "HASARLI";
            radioButton3.Text = "BOZUK";
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox3.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            comboBox3.Items.Clear();


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

            textBox1.Text = Form1.ad;
            comboBox1.Text = Form1.ürün_dlp;
            comboBox2.Text = Form1.ürün_raf;
            comboBox3.Text = Form1.ürün_tür;



        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlCommand komut = new SqlCommand();
            komut.Connection = baglanti;
            komut.CommandType = CommandType.Text;
            SqlDataReader dr;
            string kayit = "";

            string ürün_ad = Form1.ad;
            string ürün_tür = Form1.tr_id;
            string ürün_dlp = Form1.dlp_id;
            string ürün_raf = Form1.rf_id;
            string ürün_durum = Form1.ürün_durum;
            string str = "";
            int adet = Convert.ToInt32(Form1.adet);
            // textboxa girilen değer boşsa ya da ürün adetinden fazlaysa tüm ürünleri güncelle
            if (textBox2.Text == "" || (adet - Convert.ToInt32(textBox2.Text) <=0))
            {
                //teztboxa girilen değer ürün adından farklıysa değişkene yeni bilgiyi ata
                if ((textBox1.Text != Form1.ad) && textBox1.Text != "")
                {
                   ürün_ad = textBox1.Text;

                    str += "Ürünün Adı " + textBox1.Text + "\n";
                }
                //comboboxlara girilen dolap ve raf bilgileri değişmişse değişkene yeni bilgiyi ata
                if (((comboBox1.Text != Form1.ürün_dlp) && comboBox1.Text != "") && (comboBox2.Text != Form1.ürün_raf) && comboBox2.Text != "")
                {

                    string dlp = dolap_id(comboBox1.Text);
                    string rf = raf_id(dlp);
                    baglanti.Open();
                    komut.CommandText = "select * from raf where id=" + rf;
                    dr = komut.ExecuteReader();
                    string rf_ad = dr["raf_ad"].ToString();
                    baglanti.Close();
                    ürün_raf = rf;
                    str += "Dolap " + comboBox1.Text + "\n" + "Raf " + rf_ad + "\n";
                }

                //tür bilgisi değişmişse değişkene yeni bilgiyi ata
                if ((comboBox3.Text != Form1.ürün_tür) && comboBox3.Text != "")
                {

                    string tur = tur_id();
                    ürün_tür = tur;
                    str += "Ürün Türü " + comboBox3.Text + "\n";
                }
                // durum bilgisi değişmişse değişkene yeni bilgiyi ata
                if (radioButton1.Checked == true && radioButton1.Text != Form1.ürün_durum)
                {
                    ürün_durum = radioButton1.Text;
                    str += "Durum " + radioButton1.Text + "\n";
                }
                if (radioButton2.Checked == true && radioButton2.Text != Form1.ürün_durum)
                {
                    ürün_durum = radioButton2.Text;
                    str += "Durum " + radioButton2.Text + "\n";
                }
                if (radioButton3.Checked == true && radioButton3.Text != Form1.ürün_durum)
                {
                    ürün_durum = radioButton3.Text;
                    str += "Durum " + radioButton1.Text + "\n";
                }
                // herhangi bir bilgi değişmişse  ürün bilgisini güncelle
                if (ürün_ad != Form1.ad || ürün_durum != Form1.ürün_durum || ürün_raf != Form1.rf_id || ürün_tür != Form1.tr_id)
                {

                    DialogResult dialog = new DialogResult();
                    dialog = MessageBox.Show(str + "Olarak Güncellenecek. \n Emin misiniz ?.", "GÜNCELLE", MessageBoxButtons.YesNo);
                    if (dialog == DialogResult.Yes)
                    {
                        
                        baglanti.Open();
                        komut.CommandText = "update urun set urun_ad='"+ ürün_ad +"', urun_durum='"+ ürün_durum +"' , tur_id="+ ürün_tür +", raf_id="+ ürün_raf + "where id =" + Form1.ürün_id;
                        komut.ExecuteNonQuery();
                        baglanti.Close();

                        if (str != "")
                        {
                            MessageBox.Show(str + "Olarak Güncellendi.");
                            this.Close();
                        }
                    }
                    else
                    {
                        this.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Bütün Bilgiler Aynı İse Güncelleme Yapamazsınız.");
                    this.Close();
                }



            }
            else //textboxa girilen değer ürün adetinden fazlaysa değilse değişen bilgilerde yeni bir ürün ekle ve eski ürünün adetini azaltt
            {
                int id = id_bul();
                if ((textBox1.Text != Form1.ad) && textBox1.Text != "")
                {
                    baglanti.Open();
                    komut.CommandText = "update urun set urun_adet=" + (adet - Convert.ToInt32(textBox2.Text)).ToString() + " where id=" + Form1.ürün_id;
                    komut.ExecuteNonQuery();
                    baglanti.Close();

                    ürün_ad = textBox1.Text;

                    str += "Ürünün Adı " + textBox1.Text + "\n";
                }                
                if ((comboBox1.Text != Form1.ürün_dlp) && comboBox1.Text != "" || (comboBox2.Text != Form1.ürün_raf) && comboBox2.Text != "")
                {
                    string dlp = dolap_id(comboBox1.Text);
                    string rf = raf_id(dlp);
                    baglanti.Open();
                    komut.CommandText = "select * from raf where id="+ rf;
                    dr = komut.ExecuteReader();
                    dr.Read();
                    string rf_ad = dr["raf_ad"].ToString();
                    baglanti.Close();
                    ürün_raf = rf;
                    str += "Dolap " + comboBox1.Text + "\n";
                    str += "Raf " + rf_ad + "\n";
                }


                if ((comboBox3.Text != Form1.ürün_tür) && comboBox3.Text != "")
                {

                    string tur = tur_id();
                    ürün_tür = tur;
                    str += "Ürün Türü " + comboBox3.Text + "\n";
                }
                if (radioButton1.Checked == true && radioButton1.Text != Form1.ürün_durum)
                {
                    ürün_durum = radioButton1.Text;
                    str += "Durum " + radioButton1.Text + "\n";
                }
                if (radioButton2.Checked == true && radioButton2.Text != Form1.ürün_durum)
                {
                    ürün_durum = radioButton2.Text;
                    str += "Durum " + radioButton2.Text + "\n";
                }
                if (radioButton3.Checked == true && radioButton3.Text != Form1.ürün_durum)
                {
                    ürün_durum = radioButton3.Text;
                    str += "Durum " + radioButton1.Text + "\n";
                }
                if (ürün_ad != Form1.ad || ürün_durum != Form1.ürün_durum || ürün_raf != Form1.rf_id || ürün_tür != Form1.tr_id)
                {

                    int var_mii = var_mi(ürün_raf);

                    if(var_mii == 0)
                    {
                        DialogResult dialog = new DialogResult();
                        dialog = MessageBox.Show(str + "Olarak Güncellenecek. \n Emin misiniz ?.", "ÇIKIŞ", MessageBoxButtons.YesNo);
                        if (dialog == DialogResult.Yes)
                        {
                            baglanti.Open();
                            komut.CommandText = "insert into urun (id,urun_ad,urun_durum,urun_adet,tur_id,raf_id)" +
                                               "values (" + id + ",'" + ürün_ad + "','" + ürün_durum + "'," + Convert.ToInt32(textBox2.Text).ToString() + "," + ürün_tür + "," + ürün_raf + ");";
                            komut.ExecuteNonQuery();
                            baglanti.Close();

                            baglanti.Open();
                            komut.CommandText = "update urun set urun_adet=" + (adet - Convert.ToInt32(textBox2.Text)).ToString() + "where id =" + Form1.ürün_id;
                            komut.ExecuteNonQuery();
                            baglanti.Close();

                            if (str != "")
                            {
                                MessageBox.Show(str + "Olarak Güncellendi.");
                                this.Close();
                            }
                        }
                        
                    
                    }
                    else
                    {
                        baglanti.Open();
                        komut.CommandText = "select * from urun where id=" + var_mii;
                        dr = komut.ExecuteReader();
                        dr.Read();
                        int adt = Convert.ToInt32(dr["urun_adet"]);
                        baglanti.Close();

                        adt += Convert.ToInt32(textBox2.Text);

                        baglanti.Open();
                        komut.CommandText = "update urun set urun_adet=" + adt + "where id =" + var_mii;
                        komut.ExecuteNonQuery();
                        baglanti.Close();

                        baglanti.Open();
                        komut.CommandText = "update urun set urun_adet=" + (adet - Convert.ToInt32(textBox2.Text)).ToString() + "where id =" + Form1.ürün_id;
                        komut.ExecuteNonQuery();
                        baglanti.Close();
                        this.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Bütün Bilgiler Aynı İse Güncelleme Yapamazsınız.");
                    this.Close();
                }
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

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }
    }
}
