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
using System.Collections;


namespace ürüntakip
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection("Data Source=DESKTOP-OTMDOAG\\SQLEXPRESS;Initial Catalog=urunstok;Integrated Security=True;");

        public static string ürün_id = "";
        public static string dlp_id = "";
        public static string rf_id = "";
        public static string tr_id = "";
        public static string ad = "";
        public static string ürün_durum = "";
        public static string adet = "";
        public static string ürün_tür = "";
        public static string ürün_dlp = "";
        public static string ürün_raf = "";
        public static string tablo = "";
        public static int var;

        public void temizle()
        {
            // baglanti açıksa kapatılması
            if (baglanti.State == ConnectionState.Open)
                baglanti.Close();
            // sql ilemlerinde kullanılacak deikenler
            SqlCommand komut = new SqlCommand();
            SqlDataReader dr;

            komut.Connection = baglanti;
            komut.CommandType = CommandType.Text;
            // form üzerine bilgi girilmişse bu deerlerin sıfırlanması
            textBox1.Text = "";
            //combobox ın içerisindeki verilerin silinmesi ( Böyle yapılmazsa form tekrar yüklendiğii zaman veya başla bir iişlem esnasında sql den alınan bilgiler var olan bilgilerin üzerine ekleniyor ve her dolap raf vb birden fazla comboboox un içerisine yerleiyor)
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            comboBox3.Items.Clear();
            textBox2.Text = "";
            //radio butonların tıklanma özelliğini kaldırma 
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            radioButton3.Checked = false;
            //daha önceden içerisindeki öğeleri silinen comboboxların içerisine tekrardan sql den alınan verilerin girilmesi
            komut.CommandText = "SELECT *FROM dolap";
            baglanti.Open();
            dr = komut.ExecuteReader();
            while (dr.Read())
            {
                //combobx un içerisine sql deki dolap tablusunun  dolap_ad kızmındaki verilerin combobox a eklenmesi
                comboBox1.Items.Add(dr["dolap_ad"].ToString());
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
            listView1.Items.Clear();

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
            // parametre larak gelen combobx1 deki verinin id sinin bulunması
            baglanti.Open();
            komut.CommandText = "SELECT *FROM dolap WHERE dolap_ad ='" + str + "'";
            dr = komut.ExecuteReader();
            dr.Read();
            id = dr["id"].ToString();
            baglanti.Close();
            return id;
        }
        public string raf_id(string dolap, string ad)
        {
            // komut.ExecuteNonQuery() komut.CommandText e girilen sql sorgusunu çalıştırır. dr = komut.ExecuteReader() ise girilen sql soorgunun çıktılarını okur. dr.read() eklinde çalıtırıp örnekdeğişken=dr["örnekdeğer"] eklinde çalışır.
            //her sql sorgudan önce mutlaka baglanti.Open() ile bağlantıyı açmak ve sorgu veya okuma işlemleri bittikten sonra baglanti.close() ile bağlantıyı kapanmak gerekiyor.
            if (baglanti.State == ConnectionState.Open)
                baglanti.Close();
            string id = "";

            SqlCommand komut = new SqlCommand();
            komut.Connection = baglanti;
            komut.CommandType = CommandType.Text;
            SqlDataReader dr;
            //dolap_id fooonksiyonunda bulunan dolap id yi ve combobox2 ye girilen değeri parametre olarak alıp o değerlerdeki raf id sinin bulunması
            baglanti.Open();

            komut.CommandText = "SELECT *FROM raf WHERE raf_ad ='" + ad + "' and dolap_id =" + dolap;
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
            //Tıklanan radibutonun text değerini durum değişkenine eşitlenmesi
            string durum = "";
            if (radioButton1.Checked == true)
                durum = radioButton1.Text;
            if (radioButton2.Checked == true)
                durum = radioButton2.Text;
            if (radioButton3.Checked == true)
                durum = radioButton3.Text;
            //Durum değişkeninin değeriinin döndürülmesi
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
            {   //cmbobox3 te seçilen değere güre seçilen türün id sinin bulunması
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
            //ürünün id bilgisini tutmak için kullanılacak int değişlken
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
            {   //Fonksiyoona parametre olarak gelen raf id değerini ve durum ve tür id leri bulunduktan soonra böyle bir ürünün oolup olmadığının koontrolü 
                if (textBox1.Text == dr["urun_ad"].ToString() && raf == dr["raf_id"].ToString() && tur == dr["tur_id"].ToString() && durum == dr["urun_durum"].ToString())
                    sayi = Convert.ToInt32(dr["id"]);
            }
            baglanti.Close();
            // eğer ürün yoksa 0 değerini döndürür, ürün varsa if blğu içerisinde girip satı değişkeni bulunan ürünün id değerini alır 
            return sayi;
        }
        public int ürün_adet(string raf)
        {
            if (baglanti.State == ConnectionState.Open)
                baglanti.Close();

            int adet;
            SqlCommand komut = new SqlCommand();
            SqlDataReader dr;
            komut.Connection = baglanti;
            komut.CommandType = CommandType.Text;

            baglanti.Open();
            //text box a girilen ürün iismi ve parametre olarak gönderilen raf id bilgilerini kullanak bulunan ürünün adet bilgisinin döndürülmesi
            //aynı raf ve dolapta aynı isimde ürünler olabilir ve yanlı adet bilgisini döndürebilir bunun için var_mi fonksiyonundaki gibi tür ve durum bilgilerini de kullanabilirsin :) 
            komut.CommandText = "SELECT * FROM urun WHERE urun_ad ='" + textBox1.Text + "' and raf_id =" + raf;
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
            //yeni kayıt sırasında her ürüne farklı bir id bulunması gerekir. eğer bir ürün silinmişse oluşan boşluğa yeni ürünü girmeliyiz
            //bu fonkisyon örneğin id leri 1 2 ve 3 olan 3 üründen 2 numaralı ürün silindiğinde yeni eklenecek ürünün id sini 4 yerine 2 larak belirler
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
        private void Form1_Load(object sender, EventArgs e)
        {
            SqlCommand komut = new SqlCommand();
            komut.Connection = baglanti;
            komut.CommandType = CommandType.Text;
            SqlDataReader dr;
            //Form load esnasında butoon label gibi nesnelerin textlerinin girilmesi
            this.Text = "OES DENİZCİLİK ÜRÜN STOK PROGRAMI";
            label1.Text = "ÜRÜNÜN ADI: ";
            label2.Text = "DOLAP NUMARASI: ";
            label3.Text = "RAF NUMARASI: ";
            label4.Text = "ÜRÜN SAYISI: ";
            label5.Text = "ÜRÜN TÜRÜ: ";
            groupBox1.Text = "ÜRÜNÜN DURUMU: ";
            groupBox2.Text = "ÜRÜN: ";
            groupBox3.Text = "TABLO";
            radioButton1.Text = "SAĞLAM";
            radioButton2.Text = "HASARLI";
            radioButton3.Text = "BOZUK";
            radioButton6.Text = "EKLE";
            radioButton7.Text = "ÇIKAR";
            button1.Text = "BUL";
            button2.Text = "EKLE";
            button3.Text = "SİL";
            button4.Text = "LİSTELE";
            button5.Text = "GÜNCELLE";
            button6.Text = "EKLE/ÇIKAR";
            //Comboboxa kullanıcının bir veri girişi yapamaması için DropDownList özelliğini kullanmalısın
            //dropdownlist özelliğini yorum satırına alıp nasıl etkilediğini görebilirsin
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox3.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox4.DropDownStyle = ComboBoxStyle.DropDownList;
            listView1.Columns.Add("ÜRÜN ADI", 250);
            listView1.Columns.Add("ÜRÜN TÜR", 150);
            listView1.Columns.Add("DOLAP", 100);
            listView1.Columns.Add("RAF", 75);
            listView1.Columns.Add("ADET", 50);
            listView1.Columns.Add("ÜRÜNÜN DURUMU", 100);

            //combobox4 için  verilerin girilmesi
            string[] veri = { "Dolap", "Raf", "Tür" };
            for (int i = 0; i < veri.Length; i++)
            {
                comboBox4.Items.Add(veri[i]);
            }

            //form load esnasında comboboxlara dolap ve tür biilgilerinin eklenmesi
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
            //Burda dolap sayısını yukarıdaki döngürle hesaplayıp aşşağıda for döngüsünde kullandım ve ger döngideki i değerini stringe dönütürüp cmboobox 1 deki değere eşitse o anki i  değerini raf tablosundaki dolap id si olarak kullandım 
            // ve daha sonra bu komutu çalıştırıp okunan raf değerlerinin combobox 2 ye ekledim.
            //biraz aşşağıya daha mantıklı bir yöntemi ekledim bu fonksiyonu yazarken aklıma gelmemişti deneyip hata vermiyorsa kullanabiilirsin
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

            //baglanti.Open();
            //komut.CommandText = "SELECT *FROM dolap";
            //dr = komut.ExecuteReader();
            //while (dr.Read())
            //{
            //    if (comboBox1.Text == dr["dolap_ad"].ToString())
            //        komut.CommandText = "select * from raf where dolap_id=" + dr["id"].ToString();
            //}
            //baglanti.Close();

            //baglanti.Open();
            //dr = komut.ExecuteReader();
            //while (dr.Read())
            //{
            //    comboBox2.Items.Add(dr["raf_ad"]);
            //}
            //baglanti.Close();



        }
        private void button1_Click(object sender, EventArgs e)
        {

            listView1.Items.Clear();
            SqlCommand komut = new SqlCommand();
            komut.Connection = baglanti;
            komut.CommandType = CommandType.Text;
            SqlDataReader dr;
            // aranan isimde birden fazla ürün olabileceği için bu ürünlerin id lerini tutmak için bir arraylist tanımladım
            ArrayList ürün = new ArrayList();

            if (textBox1.Text != "")
            {
                //textbox1 boş değişlde bu koomutlar çalışır
                // sql de like komutunu harfi ya da kelime yazdığınızda eşleşen tüm sonuçları listeler
                int sayi = 0;
                baglanti.Open();
                komut.CommandText = "SELECT * FROM urun where urun_ad like '%" + textBox1.Text + "%'";

                dr = komut.ExecuteReader();
                while (dr.Read())
                {       //bulunan sonuçlarının id lerinin oluturulan arraylistin içetisine eklenmesi      
                    sayi = 1;
                    ürün.Add(dr["id"]);

                }
                baglanti.Close();
                if (sayi == 1)
                {
                    for (int i = 0; i < ürün.Count; i++)
                    {
                        baglanti.Open();
                        komut.CommandText = "select\r\n" +
                            "urun.urun_ad as adi,\r\n" +
                            "uruntur.tur_ad as tür,\r\n" +
                            "dolap.dolap_ad as dolap,\r\n" +
                            "raf.raf_ad as raf,\r\n" +
                            "urun.urun_adet as adet,\r\n" +
                            "urun.urun_durum as durum\r\n" +
                            "from urun \r\n" +
                            "inner join uruntur on uruntur.id= urun.tur_id\r\n" +
                            "inner join raf on urun.raf_id=raf.id\r\n" +
                            "inner join dolap on raf.dolap_id = dolap.id\r\n" +
                            "where urun.id=" + ürün[i] + ";";
                        //innerjoin ile 3 tablonun birbirine bağlanıp tek seferde listelenmesi
                        komut.Connection = baglanti;
                        komut.CommandType = CommandType.Text;
                        dr = komut.ExecuteReader();
                        dr.Read();
                        // listelenen bilgileri alıp listview e eklenmesi 
                        string[] str = { dr["adi"].ToString(), dr["tür"].ToString(), dr["dolap"].ToString(), dr["raf"].ToString(), dr["adet"].ToString(), dr["durum"].ToString() };
                        ListViewItem veri = new ListViewItem(str);
                        listView1.Items.Add(veri);
                        baglanti.Close();
                    }

                }
                else
                {//kooulların sağlanamadığı durumlarda kullanıcıya uyarı mesağlarının verilmesi
                    MessageBox.Show(textBox1.Text + " ADINDA BİR ÜRÜN BULUNAMADI");
                }
            }
            else
            {
                MessageBox.Show("BULMA İŞLEMİ ESNASINDA ÜRÜN ADINI GiRMELİSİNİZ");
            }
            // baglanti açıksa kapatılması
            if (baglanti.State == ConnectionState.Open)
                baglanti.Close();
            // sql ilemlerinde kullanılacak deikenler
            komut.Connection = baglanti;
            komut.CommandType = CommandType.Text;
            // form üzerine bilgi girilmişse bu deerlerin sıfırlanması
            textBox1.Text = "";
            //combobox ın içerisindeki verilerin silinmesi ( Böyle yapılmazsa form tekrar yüklendiğii zaman veya başla bir iişlem esnasında sql den alınan bilgiler var olan bilgilerin üzerine ekleniyor ve her dolap raf vb birden fazla comboboox un içerisine yerleiyor)
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            comboBox3.Items.Clear();
            textBox2.Text = "";
            //radio butonların tıklanma özelliğini kaldırma 
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            radioButton3.Checked = false;
            //daha önceden içerisindeki öğeleri silinen comboboxların içerisine tekrardan sql den alınan verilerin girilmesi
            komut.CommandText = "SELECT *FROM dolap";
            baglanti.Open();
            dr = komut.ExecuteReader();
            while (dr.Read())
            {
                //combobx un içerisine sql deki dolap tablusunun  dolap_ad kızmındaki verilerin combobox a eklenmesi
                comboBox1.Items.Add(dr["dolap_ad"].ToString());
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
        }
        private void button2_Click(object sender, EventArgs e)
        {
            SqlCommand komut = new SqlCommand();
            SqlDataReader dr;
            komut.Connection = baglanti;
            komut.CommandType = CommandType.Text;
            //Form2 nin oluşturulması
            Form2 f2 = new Form2();

            //listview i eklerken özellikler kısmından yanlızca 1 seçin yapabilecek şekilde ayarladım
            //seçilenlerinin sayısı 1 ise yani bir ürün listeden seçilmişse bu kod bloğu çalışacak
            if (listView1.SelectedItems.Count == 1)
            {
                //listView1.SelectedItems[0] kullanıcı tarafından seçilen ürünü ifade eder sub itemler ise bu listedeki sütunları ifade eder.
                //subitemler 0.indexten başlar listView1.SelectedItems[0].SubItems[0].Text kullanıcı adını ifade eder vb ...
                ad = listView1.SelectedItems[0].SubItems[0].Text;
                ürün_tür = listView1.SelectedItems[0].SubItems[1].Text;
                ürün_dlp = listView1.SelectedItems[0].SubItems[2].Text;
                ürün_raf = listView1.SelectedItems[0].SubItems[3].Text;
                adet = listView1.SelectedItems[0].SubItems[4].Text;
                //aşağıdaki kontollere gerek yok aslında seninde böyle hatalar yapmaman iiçin böyle bırakıyorum
                if (listView1.SelectedItems[0].SubItems[5].Text == "SAĞLAM")
                    ürün_durum = "SAĞLAM";

                if (listView1.SelectedItems[0].SubItems[5].Text == "HASARLI")
                    ürün_durum = "HASARLI";


                if (listView1.SelectedItems[0].SubItems[5].Text == "BOZUK")
                    ürün_durum = "BOZUK";
                //ürün_durum = listView1.SelectedItems[0].SubItems[5].Text; şeklinde yapman yeterli olacaktır

                //listView den alınan dolap ve raf adlarına göre raf ve dolap id lerinin bulunması
                string dlp = dolap_id(ürün_dlp);
                string raf = raf_id(dlp, ürün_raf);

                int sayi = 0;
                // liistviewden alınan ürün türü bilgisine göre id sinin bulunması 
                // tur_id fonksiyonunu biraz değiştirip  alttaki kod fazlalığı yerine tur=tur_id(ürün_tür); şekliinde kullanabilirdim ama üşendim çünkü bir çk yerde o fonsiynu o şekilde kullandım
                string tur = "";
                baglanti.Open();
                komut.CommandText = "SELECT *FROM uruntur WHERE tur_ad ='" + ürün_tür + "'";
                dr = komut.ExecuteReader();
                dr.Read();
                tur = dr["id"].ToString();
                baglanti.Close();
                tr_id = tur;

                //listviewden alınan bilgilere göre ürünün id sinin bulunması
                baglanti.Open();
                komut.CommandText = "select * from urun where urun_ad = '" + ad + "' and urun_durum = '" + ürün_durum + "' and urun_adet = " + adet + " and tur_id = " + tur + " and raf_id = " + raf + ";";
                dr = komut.ExecuteReader();
                dr.Read();
                ürün_id = dr["id"].ToString();
                baglanti.Close();
                //ürün_id, ürün_tür gibi değikenleri formlar bilgi akışı için kullandım en üst kısımda public static olacak şekilde tanımladım.
                //oluşturulan form2 nin ekranda gösterilmesi;
                //f2.show() şeklinde kullanırsan kullanıcı 2 forma da müdahale edebilir. f2.ShowDialog() olarak kullanırsan kullanıcı sadece üste açılan forma müdahale edebiliyor üstteki form kapanmadan alttaki forma müdahale edemiyor.
                f2.ShowDialog();
                //listView1.SelectedItems[0].Selected = false; liseten seçilen ürünün işlemi yapıldıktan sonra seçilme özelliğini kaldırıyor
                listView1.SelectedItems[0].Selected = false;
            }
            else
            {   //eğer kullanmıcı bir ürün seçmediyse yani yeni bir kayıt ekleyecekse bütün veri girilecek alanların boş olamaması gerekir
                if (textBox1.Text != "" && textBox2.Text != "" && comboBox1.Text != "" && comboBox2.Text != "" && comboBox3.Text != "" && (radioButton1.Checked == true || radioButton2.Checked == true || radioButton3.Checked == true))
                {
                    string dlp = dolap_id(comboBox1.Text);
                    string raf = raf_id(dlp, comboBox2.Text);
                    string durum = durum_bul();
                    int id = var_mi(raf);
                    //id 0 değilse yani böyle bir ürün varsa ürünün sadece sayısı artmalı, aynı özelliklerde yeni bir ürün eklememeli.
                    if (id != 0)
                    {
                        MessageBox.Show("(" + textBox1.Text + ") Adında Bir Ürün Bulundu. \n Ürünün Sayısını Arttırabilirsiniz.");

                        ad = textBox1.Text;
                        ürün_id = id.ToString();
                        dlp_id = dolap_id(comboBox1.Text);
                        rf_id = raf_id(dlp_id, comboBox2.Text);
                        adet = textBox2.Text;
                        f2.ShowDialog();
                    }
                    else
                    {
                        // id 0 sa yani böyle bir ürün yoksa id bul fonkisyonu ile uygun bir id nin bulunması
                        ürün_id = id_bul().ToString();
                        dlp_id = dolap_id(comboBox1.Text);
                        rf_id = raf_id(dlp_id, comboBox2.Text);
                        //ürünün durumu ve tür bilgilerinin alınması                            
                        string drm = "";
                        if (radioButton1.Checked == true)
                            drm = radioButton1.Text;
                        if (radioButton2.Checked == true)
                            drm = radioButton2.Text;
                        if (radioButton3.Checked == true)
                            drm = radioButton3.Text;
                        //Tür id nin bulunması
                        string tur = "";
                        baglanti.Open();
                        komut.CommandText = "SELECT *FROM uruntur WHERE tur_ad ='" + comboBox3.Text + "'";
                        dr = komut.ExecuteReader();
                        dr.Read();
                        tur = dr["id"].ToString();
                        baglanti.Close();
                        //ürün veri tabanına eklenmeden önce kullanıcıya bir uyarıyla ürünün ekleneceğinin bildirilmesi.
                        // kullanıcının cevabu evet ise ürünü veri tabanına eklenmesi
                        DialogResult dialog = new DialogResult();
                        dialog = MessageBox.Show("Ürün Eklenecek. \n Emin misiniz ?.", "Ürün Ekle", MessageBoxButtons.YesNo);
                        if (dialog == DialogResult.Yes)
                        {
                            string kayit = "";
                            if (baglanti.State == ConnectionState.Open)
                                baglanti.Close();

                            baglanti.Open();
                            kayit = "insert into urun (id,urun_ad,urun_durum,urun_adet,tur_id,raf_id)" +
                                           "values (" + ürün_id.ToString() + ",'" + textBox1.Text + "','" + drm + "'," + textBox2.Text + "," + tur + "," + rf_id + ");";
                            komut.CommandText = kayit;
                            komut.ExecuteNonQuery();
                            baglanti.Close();

                            MessageBox.Show(" ( " + textBox1.Text + " ) Adlı Ürün Eklendi");
                            Form1.ürün_id = "0";
                            temizle();
                        }
                        else
                            MessageBox.Show("Ürün Eklenmedi.");
                    }


                }
                else
                    MessageBox.Show("Tüm Bilgiler Girilmeli");
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            SqlCommand komut = new SqlCommand();
            SqlDataReader dr;
            komut.Connection = baglanti;
            komut.CommandType = CommandType.Text;
            Form3 f3 = new Form3();


            //Buton 2  de olduğu gibi listview den alınan bilgilerin yeni form ile arasında veri akışı için glbal değişkenlere atanması
            if (listView1.SelectedItems.Count == 1)
            {
                ad = listView1.SelectedItems[0].SubItems[0].Text;
                ürün_tür = listView1.SelectedItems[0].SubItems[1].Text;
                ürün_dlp = listView1.SelectedItems[0].SubItems[2].Text;
                ürün_raf = listView1.SelectedItems[0].SubItems[3].Text;
                adet = listView1.SelectedItems[0].SubItems[4].Text;
                if (listView1.SelectedItems[0].SubItems[5].Text == "SAĞLAM")
                    ürün_durum = "SAĞLAM";

                if (listView1.SelectedItems[0].SubItems[5].Text == "HASARLI")
                    ürün_durum = "HASARLI";


                if (listView1.SelectedItems[0].SubItems[5].Text == "BOZUK")
                    ürün_durum = "BOZUK";

                string dlp = dolap_id(ürün_dlp);
                string raf = raf_id(dlp, ürün_raf);

                string tur = "";
                baglanti.Open();
                komut.CommandText = "SELECT *FROM uruntur WHERE tur_ad ='" + ürün_tür + "'";
                dr = komut.ExecuteReader();
                dr.Read();
                tur = dr["id"].ToString();
                baglanti.Close();


                baglanti.Open();
                komut.CommandText = "select * from urun where urun_ad = '" + ad + "' and urun_durum = '" + ürün_durum + "' and urun_adet = " + adet + " and tur_id = " + tur + " and raf_id = " + raf + ";";
                dr = komut.ExecuteReader();
                dr.Read();
                ürün_id = dr["id"].ToString();
                baglanti.Close();
                f3.ShowDialog();
                listView1.SelectedItems[0].Selected = false;

            }
            else
            {   // eğer listeden ürün seçilmediyse yani kullanıcı bilgi girişi yaparak işlem yapmak iştiyrsa girilerin verilerin glbal değişkenlere atanması
                if (textBox1.Text != "" && textBox2.Text != "" && comboBox1.Text != "" && comboBox2.Text != "" && comboBox3.Text != "" && radioButton1.Checked == true || radioButton2.Checked == true || radioButton3.Checked == true)
                {
                    string dlp = dolap_id(comboBox1.Text);
                    string raf = raf_id(dlp, comboBox2.Text);
                    int id = var_mi(raf);
                    if (id != 0)
                    {
                        MessageBox.Show("(" + textBox1.Text + ") Adında Bir Ürün Bulundu. \n Silme İşlemleri Yapabilirsiniz");
                        ürün_id = id.ToString();
                        dlp_id = dolap_id(comboBox1.Text);
                        rf_id = raf;
                        adet = textBox2.Text;
                        ad = textBox1.Text;
                        ürün_dlp = comboBox1.Text;
                        ürün_raf = comboBox2.Text;
                        adet = textBox2.Text;
                        ürün_durum = durum_bul();
                        ürün_tür = comboBox3.Text;
                        f3.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("(" + textBox1.Text + ") Adında Bir Ürün Bulunamadı. \n  Silme İşlemleri Yapamazsınız.");
                    }

                }
                else
                    MessageBox.Show("Tüm Bilgiler Girilmeli");
            }




        }
        private void button4_Click(object sender, EventArgs e)
        {
            temizle();
            baglanti.Open();
            SqlCommand komut = new SqlCommand();
            SqlDataReader dr;
            komut.CommandText = "select\r\n" +
                "urun.urun_ad as adi,\r\n" +
                "uruntur.tur_ad as tür,\r\n" +
                "dolap.dolap_ad as dolap,\r\n" +
                "raf.raf_ad as raf,\r\n" +
                "urun.urun_adet as adet,\r\n" +
                "urun.urun_durum as durum\r\n" +
                "from urun \r\n" +
                "inner join uruntur on uruntur.id= urun.tur_id\r\n" +
                "inner join raf on urun.raf_id=raf.id\r\n" +
                "inner join dolap on raf.dolap_id = dolap.id;";
            // 3 tabloyu inner joinle bağladıktan sonra datareader ile bütün ürün bilgilerinin döngü içerisinde listview e eklenmesi
            komut.Connection = baglanti;
            komut.CommandType = CommandType.Text;
            dr = komut.ExecuteReader();
            while (dr.Read())
            {
                string[] str = { dr["adi"].ToString(), dr["tür"].ToString(), dr["dolap"].ToString(), dr["raf"].ToString(), dr["adet"].ToString(), dr["durum"].ToString() };
                ListViewItem veri = new ListViewItem(str);
                listView1.Items.Add(veri);
            }
            baglanti.Close();

        }
        private void button5_Click(object sender, EventArgs e)
        {
            SqlCommand komut = new SqlCommand();
            SqlDataReader dr;
            komut.Connection = baglanti;
            komut.CommandType = CommandType.Text;
            Form4 f4 = new Form4();

            if (listView1.SelectedItems.Count == 1)
            {


                ad = listView1.SelectedItems[0].SubItems[0].Text;
                ürün_tür = listView1.SelectedItems[0].SubItems[1].Text;
                ürün_dlp = listView1.SelectedItems[0].SubItems[2].Text;
                ürün_raf = listView1.SelectedItems[0].SubItems[3].Text;
                adet = listView1.SelectedItems[0].SubItems[4].Text;
                ürün_durum = listView1.SelectedItems[0].SubItems[5].Text;

                string dlp = dolap_id(ürün_dlp);
                string raf = raf_id(dlp, ürün_raf);
                string tur = "";
                baglanti.Open();
                komut.CommandText = "SELECT *FROM uruntur WHERE tur_ad ='" + ürün_tür + "'";
                dr = komut.ExecuteReader();
                dr.Read();
                tur = dr["id"].ToString();
                baglanti.Close();
                dlp_id = dlp;
                rf_id = raf;
                tr_id = tur;
                baglanti.Open();
                komut.CommandText = "select * from urun where urun_ad = '" + ad + "' and urun_durum = '" + ürün_durum + "' and urun_adet = " + adet + " and tur_id = " + tur + " and raf_id = " + raf + ";";
                dr = komut.ExecuteReader();
                dr.Read();
                ürün_id = dr["id"].ToString();
                baglanti.Close();
                f4.ShowDialog();
                listView1.SelectedItems[0].Selected = false;

            }
            else
            {
                if (textBox1.Text != "" && textBox2.Text != "" && comboBox1.Text != "" && comboBox2.Text != "" && comboBox3.Text != "" && radioButton1.Checked == true || radioButton2.Checked == true || radioButton3.Checked == true)
                {
                    string dlp = dolap_id(comboBox1.Text);
                    string raf = raf_id(dlp, comboBox2.Text);
                    int id = var_mi(raf);
                    if (id != 0)
                    {
                        MessageBox.Show("(" + textBox1.Text + ") Adında Bir Ürün Bulundu. \n Güncelleme İşlemi Yapabilirsiniz");
                        ürün_id = id.ToString();
                        dlp_id = dolap_id(comboBox1.Text);
                        rf_id = raf;
                        adet = textBox2.Text;
                        ad = textBox1.Text;
                        ürün_dlp = comboBox1.Text;
                        ürün_raf = comboBox2.Text;
                        adet = textBox2.Text;
                        ürün_durum = durum_bul();
                        ürün_tür = comboBox3.Text;
                        tablo = comboBox4.Text;
                        baglanti.Open();
                        komut.CommandText = "SELECT *FROM uruntur WHERE tur_ad ='" + ürün_tür + "'";
                        dr = komut.ExecuteReader();
                        dr.Read();
                        tr_id = dr["id"].ToString();
                        baglanti.Close();
                        f4.ShowDialog();

                    }
                    else
                    {
                        MessageBox.Show("(" + textBox1.Text + ") Adında Bir Ürün Bulunamadı. \n  Güncelleme İşlemi Yapamazsınız.");
                    }

                }
                else
                    MessageBox.Show("Tüm Bilgiler Girilmelii");
            }
            temizle();


        }
        private void button6_Click(object sender, EventArgs e)
        {
            Form5 f5 = new Form5();
            tablo = "";
            if (comboBox4.Text == "Dolap" && radioButton6.Checked == true)
            {
                tablo = "Dolap-Ekle";
            }
            if (comboBox4.Text == "Dolap" && radioButton7.Checked == true)
            {
                tablo = "Dolap-Çıkar";
            }
            if (comboBox4.Text == "Raf" && radioButton6.Checked == true)
            {
                tablo = "Raf-Ekle";
            }
            if (comboBox4.Text == "Raf" && radioButton7.Checked == true)
            {
                tablo = "Raf-Çıkar";
            }
            if (comboBox4.Text == "Durum" && radioButton6.Checked == true)
            {
                tablo = "Durum-Ekle";
            }
            if (comboBox4.Text == "Durum" && radioButton7.Checked == true)
            {
                tablo = "Durum-Çıkar";
            }
            if (comboBox4.Text == "Tür" && radioButton6.Checked == true)
            {
                tablo = "Tür-Ekle";
            }
            if (comboBox4.Text == "Tür" && radioButton7.Checked == true)
            {
                tablo = "Tür-Çıkar";
            }
            string s = tablo;
            f5.Show();
            temizle();
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {

        }
        //textbox 2 ye sadece sayı girileceği için keypress eventini textbox2 ye ekledim ve içerisindeki komutu kullandım 
        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }
    }
}
