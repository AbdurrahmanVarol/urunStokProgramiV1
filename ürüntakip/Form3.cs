using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Collections;

namespace ürüntakip
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection("Data Source=DESKTOP-OTMDOAG\\SQLEXPRESS;Initial Catalog=urunstok;Integrated Security=True;");

        void temizle()
        {
            SqlCommand komut = new SqlCommand();
            SqlDataReader dr;
            komut.Connection = baglanti;
            komut.CommandType = CommandType.Text;

            textBox1.Text = "";

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
        public string raf_id(string dolap, string str)
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
            komut.CommandText = "SELECT *FROM raf WHERE raf_ad ='" + str + "' and dolap_id =" + dp;
            dr = komut.ExecuteReader();
            dr.Read();
            id = dr["id"].ToString();
            baglanti.Close();
            return id;
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
        public int var_mi(string raf)
        {
            int sayi = 0;
            SqlCommand komut = new SqlCommand();
            SqlDataReader dr;
            komut.Connection = baglanti;
            komut.CommandType = CommandType.Text;
            baglanti.Open();
            komut.CommandText = "SELECT * FROM urun";
            dr = komut.ExecuteReader();
            while (dr.Read())
            {
                if (textBox1.Text == dr["urun_ad"].ToString() && raf == dr["raf_id"].ToString())
                    sayi = 1;
                else
                    sayi = 0;
            }
            baglanti.Close();
            return sayi;
        }
        private void Form3_Load(object sender, EventArgs e)
        {
            SqlCommand komut = new SqlCommand();
            komut.Connection = baglanti;
            komut.CommandType = CommandType.Text;
            SqlDataReader dr;

            label1.Text = "ÜRÜN SAYISI: ";
            button1.Text = "SİL";
            button2.Text = "ÇIKIŞ";
            groupBox1.Text = "SİLME SEÇENEKLERİ: ";
            radioButton1.Text = "Adet Azalt";
            radioButton2.Text = "Ürünü Sil";
            radioButton1.Checked = true;

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

            //veri tabanındaki doğru ürünü bulabilmek için isim dolap ve raf bilgilerinin girilip girilmediğinin kontrolü

            if (radioButton1.Checked == true)
            {

                int sayac;
                string kayit = "";
                int adet = Convert.ToInt32(Form1.adet);
                baglanti.Open();
                // textbox2 boşsa ve ürünün sayısından 1 eksildiğinde sıfıra eşit ya da küçükse ya da ürünün sayısı textbox2 te girilen değere eşitse ürünün tamamen veri tabanından silinmesi
                if ((adet - 1) <= 0 || (adet - Convert.ToInt32(textBox1.Text)) <= 0 )
                {
                    komut.CommandText = "delete from urun where id=" + Form1.ürün_id;
                    komut.ExecuteNonQuery();
                    baglanti.Close();
                    MessageBox.Show("ÜRÜN SİLNDİ");
                    this.Close();
                }
                //ürünün sayısı textbox2 te girilen değere eşit değilse girilen değer kadar ürünün adetinin azaltılması
                else
                {
                    if (textBox1.Text == "")
                    {
                        adet--;
                    }
                    else
                    {                           
                        adet -= Convert.ToInt32(textBox1.Text);
                    }

                    DialogResult dialog = new DialogResult();
                    dialog = MessageBox.Show("Böyle Bir Ürün Var. Ürünün Sayısı "+ textBox1.Text +" Azaltılacak. \n Emin misiniz ?.", "SİLME", MessageBoxButtons.YesNo);
                    if (dialog == DialogResult.Yes)
                    {
                        //ürün adet bilgisinin güncellenmesi
                        komut.CommandText = "Update urun set urun_adet=" + adet.ToString() + "where id=" + Form1.ürün_id;
                        komut.ExecuteNonQuery();
                        baglanti.Close();
                        MessageBox.Show("ÜRÜN SAYISI " + textBox1.Text + " KADAR AZALTILDI");
                        temizle();
                        this.Close();
                    }
                    else
                        this.Close();
                }

            }
            if (radioButton2.Checked == true)
            {

                DialogResult dialog = new DialogResult();
                dialog = MessageBox.Show(Form1.ad +" Adlı Ürün Silinecek. \n Emin misiniz ?.", "SİLME", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    if (baglanti.State == ConnectionState.Closed)
                        baglanti.Open();

                    komut.CommandText = "delete from urun where id=" + Form1.ürün_id;
                    komut.ExecuteNonQuery();
                    baglanti.Close();
                    MessageBox.Show("ÜRÜN SİLİNDİ");
                    this.Close();
                }
                else
                    this.Close();
            }



            temizle();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Enabled = true;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked == true)
            {
                textBox1.Enabled = false;
            }

        }


        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    }
}
