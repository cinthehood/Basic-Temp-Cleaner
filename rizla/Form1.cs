using System;
using System.IO;
using System.Windows.Forms;
using static System.Windows.Forms.DataFormats;

namespace rizla
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            // Formun border stilini none yap�yoruz, yani ba�l�k �ubu�u olmayacak
            this.MouseDown += new MouseEventHandler(Form1_MouseDown);
            this.MouseMove += new MouseEventHandler(Form1_MouseMove);
            this.MouseUp += new MouseEventHandler(Form1_MouseUp);
        }

        // S�r�kleme i�lemini kontrol etmek i�in de�i�kenler
        int TogMove = 0;  // Ba�lang��ta s�r�kleme yap�lmas�n
        int MValX;
        int MValY;

        // Form y�klenirken herhangi bir i�lem yap�lmas� gerekmez
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        // Fare tu�u b�rak�ld���nda s�r�kleme i�lemini durdur
        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            TogMove = 0;  // S�r�klemeyi durdur
        }

        // Fare t�kland���nda s�r�kleme i�lemini ba�lat
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            TogMove = 1;  // S�r�kleme i�lemini ba�lat
            MValX = e.X;  // Fare t�klama pozisyonunu kaydet
            MValY = e.Y;  // Fare t�klama pozisyonunu kaydet
        }

        // Fare hareket etti�inde formu ta��
        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (TogMove == 1)  // E�er s�r�kleme i�lemi aktifse
            {
                this.SetDesktopLocation(MousePosition.X - MValX, MousePosition.Y - MValY);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Sonu�lar� saklamak i�in saya�lar
            int totalFilesDeleted = 0;
            int totalFilesFailed = 0;
            int totalFilesInFolder = 0;

            // 1. CheckBox1'i kontrol et ve dosyalar� sil
            if (checkBox1.Checked)
            {
                string folderPath1 = @"C:\Windows\Temp"; // 1. konum
                var result = SilDosyalar(folderPath1);
                totalFilesDeleted += result.deletedFiles;
                totalFilesFailed += result.failedFiles;
                totalFilesInFolder += result.totalFiles;
            }

            // 2. CheckBox2'yi kontrol et ve dosyalar� sil
            if (checkBox2.Checked)
            {
                string folderPath2 = @"C:\Users\frcin\AppData\Local\Temp"; // 2. konum
                var result = SilDosyalar(folderPath2);
                totalFilesDeleted += result.deletedFiles;
                totalFilesFailed += result.failedFiles;
                totalFilesInFolder += result.totalFiles;
            }

            // 3. CheckBox3'� kontrol et ve dosyalar� sil
            if (checkBox3.Checked)
            {
                string folderPath3 = @"C:\Windows\Prefetch"; // 3. konum
                var result = SilDosyalar(folderPath3);
                totalFilesDeleted += result.deletedFiles;
                totalFilesFailed += result.failedFiles;
                totalFilesInFolder += result.totalFiles;
            }

            // Sonu�lar� kullan�c�ya g�ster
            MessageBox.Show($"Toplamda {totalFilesInFolder} dosya bulundu.\n" +
                            $"{totalFilesDeleted} dosya ba�ar�yla silindi.\n" +
                            $"{totalFilesFailed} dosya silinemedi.",
                            "Silme ��lemi Sonucu", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private (int deletedFiles, int failedFiles, int totalFiles) SilDosyalar(string folderPath)
        {
            int deletedFiles = 0; // Silinen dosyalar�n say�s�
            int failedFiles = 0;  // Silinemeyen dosyalar�n say�s�
            int totalFiles = 0;   // Klas�rdeki toplam dosya say�s�

            try
            {
                // Klas�r var m� kontrol et
                if (Directory.Exists(folderPath))
                {
                    // Klas�rdeki t�m dosyalar� al
                    string[] files = Directory.GetFiles(folderPath);
                    totalFiles = files.Length; // Klas�rdeki dosya say�s�n� sakla

                    if (files.Length == 0)
                    {
                        // Klas�r bo�sa, kullan�c�ya bilgi ver
                        MessageBox.Show($"Klas�rde hi� dosya bulunamad�: {folderPath}");
                        return (deletedFiles, failedFiles, totalFiles);
                    }

                    foreach (string file in files)
                    {
                        try
                        {
                            // Dosya kullan�labilir mi kontrol et
                            if (IsFileLocked(file))
                            {
                                failedFiles++; // Dosya kullan�l�yorsa, silinemedi olarak say
                                continue; // Dosya kullan�mda oldu�u i�in atla
                            }

                            // Dosyay� sil
                            File.Delete(file);
                            deletedFiles++; // Silinen dosyalar� say
                        }
                        catch (Exception)
                        {
                            failedFiles++; // Dosya silinemedi, hata olu�tu
                            continue; // Hata olu�tu�u i�in ge�
                        }
                    }
                }
                else
                {
                    MessageBox.Show($"Klas�r bulunamad�: {folderPath}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata: {ex.Message}");
            }

            // Silinen, silinemeyen ve toplam dosya say�s�n� geri d�nd�r
            return (deletedFiles, failedFiles, totalFiles);
        }

        // Dosyan�n kullan�mda olup olmad���n� kontrol et
        private bool IsFileLocked(string filePath)
        {
            try
            {
                using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                {
                    return false; // Dosya kullan�lm�yorsa
                }
            }
            catch (IOException)
            {
                return true; // Dosya kullan�mda
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();

            // Form2'yi a�
            form2.Show();
        }
    }
}