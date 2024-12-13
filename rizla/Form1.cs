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
            // Formun border stilini none yapýyoruz, yani baþlýk çubuðu olmayacak
            this.MouseDown += new MouseEventHandler(Form1_MouseDown);
            this.MouseMove += new MouseEventHandler(Form1_MouseMove);
            this.MouseUp += new MouseEventHandler(Form1_MouseUp);
        }

        // Sürükleme iþlemini kontrol etmek için deðiþkenler
        int TogMove = 0;  // Baþlangýçta sürükleme yapýlmasýn
        int MValX;
        int MValY;

        // Form yüklenirken herhangi bir iþlem yapýlmasý gerekmez
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        // Fare tuþu býrakýldýðýnda sürükleme iþlemini durdur
        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            TogMove = 0;  // Sürüklemeyi durdur
        }

        // Fare týklandýðýnda sürükleme iþlemini baþlat
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            TogMove = 1;  // Sürükleme iþlemini baþlat
            MValX = e.X;  // Fare týklama pozisyonunu kaydet
            MValY = e.Y;  // Fare týklama pozisyonunu kaydet
        }

        // Fare hareket ettiðinde formu taþý
        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (TogMove == 1)  // Eðer sürükleme iþlemi aktifse
            {
                this.SetDesktopLocation(MousePosition.X - MValX, MousePosition.Y - MValY);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Sonuçlarý saklamak için sayaçlar
            int totalFilesDeleted = 0;
            int totalFilesFailed = 0;
            int totalFilesInFolder = 0;

            // 1. CheckBox1'i kontrol et ve dosyalarý sil
            if (checkBox1.Checked)
            {
                string folderPath1 = @"C:\Windows\Temp"; // 1. konum
                var result = SilDosyalar(folderPath1);
                totalFilesDeleted += result.deletedFiles;
                totalFilesFailed += result.failedFiles;
                totalFilesInFolder += result.totalFiles;
            }

            // 2. CheckBox2'yi kontrol et ve dosyalarý sil
            if (checkBox2.Checked)
            {
                string folderPath2 = @"C:\Users\frcin\AppData\Local\Temp"; // 2. konum
                var result = SilDosyalar(folderPath2);
                totalFilesDeleted += result.deletedFiles;
                totalFilesFailed += result.failedFiles;
                totalFilesInFolder += result.totalFiles;
            }

            // 3. CheckBox3'ü kontrol et ve dosyalarý sil
            if (checkBox3.Checked)
            {
                string folderPath3 = @"C:\Windows\Prefetch"; // 3. konum
                var result = SilDosyalar(folderPath3);
                totalFilesDeleted += result.deletedFiles;
                totalFilesFailed += result.failedFiles;
                totalFilesInFolder += result.totalFiles;
            }

            // Sonuçlarý kullanýcýya göster
            MessageBox.Show($"Toplamda {totalFilesInFolder} dosya bulundu.\n" +
                            $"{totalFilesDeleted} dosya baþarýyla silindi.\n" +
                            $"{totalFilesFailed} dosya silinemedi.",
                            "Silme Ýþlemi Sonucu", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private (int deletedFiles, int failedFiles, int totalFiles) SilDosyalar(string folderPath)
        {
            int deletedFiles = 0; // Silinen dosyalarýn sayýsý
            int failedFiles = 0;  // Silinemeyen dosyalarýn sayýsý
            int totalFiles = 0;   // Klasördeki toplam dosya sayýsý

            try
            {
                // Klasör var mý kontrol et
                if (Directory.Exists(folderPath))
                {
                    // Klasördeki tüm dosyalarý al
                    string[] files = Directory.GetFiles(folderPath);
                    totalFiles = files.Length; // Klasördeki dosya sayýsýný sakla

                    if (files.Length == 0)
                    {
                        // Klasör boþsa, kullanýcýya bilgi ver
                        MessageBox.Show($"Klasörde hiç dosya bulunamadý: {folderPath}");
                        return (deletedFiles, failedFiles, totalFiles);
                    }

                    foreach (string file in files)
                    {
                        try
                        {
                            // Dosya kullanýlabilir mi kontrol et
                            if (IsFileLocked(file))
                            {
                                failedFiles++; // Dosya kullanýlýyorsa, silinemedi olarak say
                                continue; // Dosya kullanýmda olduðu için atla
                            }

                            // Dosyayý sil
                            File.Delete(file);
                            deletedFiles++; // Silinen dosyalarý say
                        }
                        catch (Exception)
                        {
                            failedFiles++; // Dosya silinemedi, hata oluþtu
                            continue; // Hata oluþtuðu için geç
                        }
                    }
                }
                else
                {
                    MessageBox.Show($"Klasör bulunamadý: {folderPath}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata: {ex.Message}");
            }

            // Silinen, silinemeyen ve toplam dosya sayýsýný geri döndür
            return (deletedFiles, failedFiles, totalFiles);
        }

        // Dosyanýn kullanýmda olup olmadýðýný kontrol et
        private bool IsFileLocked(string filePath)
        {
            try
            {
                using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                {
                    return false; // Dosya kullanýlmýyorsa
                }
            }
            catch (IOException)
            {
                return true; // Dosya kullanýmda
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();

            // Form2'yi aç
            form2.Show();
        }
    }
}