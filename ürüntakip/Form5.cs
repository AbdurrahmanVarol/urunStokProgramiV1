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
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection("Data Source=DESKTOP-OTMDOAG\\SQLEXPRESS;Initial Catalog=urunstok;Integrated Security=True;");

        public int id_bul(string str)
        {
            SqlCommand komut = new SqlCommand();
            SqlDataReader dr;
            komut.Connection = baglanti;
            komut.CommandType = CommandType.Text;
            int sayac = 1;
            baglanti.Open();
            komut.CommandText = "select * from "+str;
            dr = komut.ExecuteReader();
            while (dr.Read())
            {
                if (Convert.ToInt32(dr["id"]) == sayac)
                {
                    sayac++;
                }
                else
                {
                    sayac = Convert.ToInt32(dr["id"]);
                    sayac++;

                }
            }
            baglanti.Close();
            return sayac;
        }
        private void Form5_Load(object sender, EventArgs e)
        {
            SqlCommand komut = new SqlCommand();
            komut.Connection = baglanti;
            komut.CommandType = CommandType.Text;
            SqlDataReader dr;

            label1.Text = "EKLENECEK DOLAP NUMARASI: ";
            label2.Text = "EKLENECEK DOLABIN RAF SAYISI";
            label3.Text = "EKLENECEK TÜR ADI: ";
            label4.Text = "";
            label5.Text = "";
            label6.Text = "";
            if (Form1.tablo == "Dolap-Ekle")
            {
                groupBox2.Enabled = false;
                groupBox1.Enabled = true;
                textBox3.Enabled = false;
                label1.Text = "EKLENECEK DOLAP NUMARASI: ";
                label2.Text = "EKLENECEK DOLABIN RAF SAYISI";
                label3.Text = "";
                label4.Text = "";
                label5.Text = "";
                label6.Text = "";
            }
            else if (Form1.tablo == "Dolap-Çıkar")
            {
                groupBox2.Enabled = true;
                groupBox1.Enabled = false;
                comboBox2.Enabled = false;
                textBox4.Enabled = false;
                label1.Text = "";
                label2.Text = "";
                label3.Text = "";
                label4.Text = "ÇIKARILACAK DOLAP: ";
                label5.Text = "";
                label6.Text = "";
            }
            else if (Form1.tablo == "Raf-Ekle")
            {
                groupBox2.Enabled = true;
                groupBox1.Enabled = false;
                comboBox1.Enabled = true;
                textBox4.Enabled = true;
                comboBox2.Enabled = false;
                label1.Text = "";
                label2.Text = "";
                label3.Text = "";
                label4.Text = "DOLAP: ";
                label5.Text = "EKLENECEK RAF SAYISI: ";
                label6.Text = "";
            }
            else if (Form1.tablo == "Raf-Çıkar")
            {
                groupBox2.Enabled = true;
                groupBox1.Enabled = false;
                comboBox1.Enabled = true;
                textBox4.Enabled = true;
                comboBox2.Enabled = false;
                label1.Text = "";
                label2.Text = "";
                label3.Text = "";
                label4.Text = "DOLAP: ";
                label5.Text = "ÇIKARILACAK RAF SAYISI: ";
                label6.Text = "";
            }
            else if (Form1.tablo == "Tür-Ekle")
            {
                groupBox2.Enabled = false;
                groupBox1.Enabled = true;
                textBox1.Enabled = false;
                textBox2.Enabled = false;
                label1.Text = "";
                label2.Text = "";
                label3.Text = "EKLENECEK TÜR:";
                label4.Text = "";
                label5.Text = "";
                label6.Text = "";
            }
            else if (Form1.tablo == "Tür-Çıkar")
            {
                groupBox2.Enabled = true;
                groupBox1.Enabled = false;
                comboBox1.Enabled = false;
                textBox4.Enabled = false;
                label1.Text = "";
                label2.Text = "";
                label3.Text = "";
                label4.Text = "";
                label5.Text = "";
                label6.Text = "ÇIKARILACAK TÜR: ";
            }

            groupBox1.Text = "İŞLEM ";
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;

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
                comboBox2.Items.Add(dr["tur_ad"]);
            }
            baglanti.Close();




        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            

        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlCommand komut = new SqlCommand();
            komut.Connection = baglanti;
            komut.CommandType = CommandType.Text;
            SqlDataReader dr;

            if (Form1.tablo == "Dolap-Ekle")
            {
                if (textBox1.Text != "" && textBox2.Text != "")
                {
                    string id = textBox1.Text;
                    baglanti.Open();
                    komut.CommandText = "select * from dolap";
                    dr = komut.ExecuteReader();
                    while (dr.Read())
                    {
                        if (id == dr["id"].ToString())
                            id = "";
                    }
                    baglanti.Close();
                    if(id != "")
                    {
                        baglanti.Open();
                        komut.CommandText = "insert into dolap(id,dolap_ad) values(" + textBox1.Text + ",'Dolap " + textBox1.Text + "')";
                        komut.ExecuteNonQuery();
                        baglanti.Close();

                        for (int i = 1; i <= Convert.ToInt32(textBox2.Text); i++)
                        {
                            baglanti.Open();
                            komut.CommandText = "insert into raf(id,dolap_id,raf_ad) values(" + textBox1.Text + i.ToString() + "," + textBox1.Text + ",'Raf " + i.ToString() + "')";
                            komut.ExecuteNonQuery();
                            baglanti.Close();
                        }
                        MessageBox.Show("Dolap " + textBox1.Text + " Eklendi.");
                        this.Close();
                    }                   
                }
            }
            if (Form1.tablo == "Dolap-Çıkar")
            {
                if (comboBox1.Text != "")
                {

                    int sayac = 0;
                    baglanti.Open();
                    komut.CommandText = "select * from dolap";
                    dr = komut.ExecuteReader();
                    while (dr.Read())
                    {
                        if (comboBox1.Text == dr["dolap_ad"].ToString())
                        {
                            sayac = Convert.ToInt32(dr["id"]);
                        }
                    }
                    baglanti.Close();
                    sayac++;

                    baglanti.Open();
                    komut.CommandText = "delete from dolap where id=" + sayac.ToString();
                    komut.ExecuteNonQuery();
                    baglanti.Close();

                    baglanti.Open();
                    komut.CommandText = "delete from raf where dolap_id=" + sayac.ToString();
                    komut.ExecuteNonQuery();
                    baglanti.Close();
                    MessageBox.Show(comboBox1.Text + " Silindi.");
                    this.Close();
                }
            }
            if (Form1.tablo == "Raf-Ekle")
            {
                if (comboBox1.Text != "" && textBox4.Text != "" && textBox4.Text != "0")
                {
                    int count = 0;
                    string id = "";
                    baglanti.Open();
                    komut.CommandText = "select * from dolap";
                    dr = komut.ExecuteReader();
                    while (dr.Read())
                    {
                        if (comboBox1.Text == dr["dolap_ad"].ToString())
                        {
                            id = dr["id"].ToString();
                        }
                    }
                    baglanti.Close();

                    baglanti.Open();
                    komut.CommandText = "select * from raf";
                    dr = komut.ExecuteReader();
                    while (dr.Read())
                    {
                        if (id == dr["dolap_id"].ToString())
                        {
                            count++;
                        }
                    }
                    baglanti.Close();

                    for (int i = count + 1; i < count + Convert.ToInt32(textBox4.Text) + 1; i++)
                    {
                        baglanti.Open();
                        komut.CommandText = "insert into raf(id,dolap_id,raf_ad) values(" + id + i.ToString() + "," + id + ",'Raf " + i.ToString() + "')";
                        komut.ExecuteNonQuery();
                        baglanti.Close();
                    }
                    MessageBox.Show(comboBox1.Text + " 'a " + textBox4.Text + " Tane Raf Eklendi.");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Dolap Nunarasını Ve Eklenecek Raf Sayısını Girmelisiniz\n Raf Sayısı 0 Olamaz.");
                    this.Close();
                }


            }
            if (Form1.tablo == "Raf-Çıkar")
            {
                if (comboBox1.Text != "" && textBox4.Text != "" && textBox4.Text != "0")
                {
                    int count = 0;
                    string id = "";
                    baglanti.Open();
                    komut.CommandText = "select * from dolap";
                    dr = komut.ExecuteReader();
                    while (dr.Read())
                    {
                        if (comboBox1.Text == dr["dolap_ad"].ToString())
                        {
                            id = dr["id"].ToString();
                        }
                    }
                    baglanti.Close();

                    baglanti.Open();
                    komut.CommandText = "select * from raf";
                    dr = komut.ExecuteReader();
                    while (dr.Read())
                    {
                        if (id == dr["dolap_id"].ToString())
                        {
                            count++;
                        }
                    }
                    baglanti.Close();

                    for (int i = 0; i < Convert.ToInt32(textBox4.Text); i++)
                    {
                        baglanti.Open();
                        komut.CommandText = "delete from raf where id=" + id + (count - i).ToString();
                        komut.ExecuteNonQuery();
                        baglanti.Close();
                    }
                    MessageBox.Show(textBox4.Text + " Tane Raf Silindi.");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Dolap Nunarasını Ve Siliinecek Raf Sayısını Girmelisiniz\n Raf Sayısı 0 Olamaz.");
                    this.Close();
                }

            }
            if (Form1.tablo == "Tür-Ekle")
            {
                if (textBox3.Text != "")
                {
                    int id = id_bul("uruntur");
                    bool varmı = false;
                    baglanti.Open();
                    komut.CommandText = "select * from uruntur";
                    dr = komut.ExecuteReader();
                    while (dr.Read())
                    {
                        if (dr["tur_ad"].ToString() == textBox3.Text)
                        {
                            varmı = true;
                        }
                    }
                    baglanti.Close();
                    if (varmı == false)
                    {
                        baglanti.Open();
                        komut.CommandText = "insert into uruntur(id,tur_ad) values(" + id + ",'" + textBox3.Text + "')";
                        komut.ExecuteNonQuery();
                        baglanti.Close();
                        MessageBox.Show(textBox3.Text +"Adlı Tür Eklendi.");
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Böyle Bir Tür Var");
                        this.Close();
                    }

                }
                if (Form1.tablo == "Tür-Çıkar")
                {
                    MessageBox.Show("s");
                    string id = "";
                    
                    baglanti.Open();
                    komut.CommandText = "select * from uruntur";
                    dr = komut.ExecuteReader();
                    while (dr.Read())
                    {
                        if (dr["tur_ad"].ToString() == comboBox2.Text)
                        {
                            id = dr["id"].ToString();
                        }
                    }
                    baglanti.Close();

                    if (id != "")
                    {
                        DialogResult dialog = new DialogResult();
                        dialog = MessageBox.Show(comboBox2.Text + " Adlı Tür Silinecek. \n Emin misiniz ?.", "ÇIKIŞ", MessageBoxButtons.YesNo);
                        if (dialog == DialogResult.Yes)
                        {

                            baglanti.Open();
                            komut.CommandText = "delete from uruntur where i="+ id;
                            komut.ExecuteNonQuery();
                            baglanti.Close();
                            MessageBox.Show(comboBox2.Text+" Adlı Tür Silindi.");
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Böyle Bir Tür Yok. Silinemez");
                            this.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("s");
                        this.Close();
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
