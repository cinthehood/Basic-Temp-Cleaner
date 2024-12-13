using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rizla
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            this.MouseDown += new MouseEventHandler(Form2_MouseDown);
            this.MouseMove += new MouseEventHandler(Form2_MouseMove);
            this.MouseUp += new MouseEventHandler(Form2_MouseUp);
        }

        int TogMove = 0;  // Başlangıçta sürükleme yapılmasın
        int M2ValX;
        int M2ValY;

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void Form2_MouseUp(object sender, MouseEventArgs e)
        {
            TogMove = 0;  // Sürüklemeyi durdur
        }

        // Fare tıklandığında sürükleme işlemini başlat
        private void Form2_MouseDown(object sender, MouseEventArgs e)
        {
            TogMove = 1;  // Sürükleme işlemini başlat
            M2ValX = e.X;  // Fare tıklama pozisyonunu kaydet
            M2ValY = e.Y;  // Fare tıklama pozisyonunu kaydet
        }

        // Fare hareket ettiğinde formu taşı
        private void Form2_MouseMove(object sender, MouseEventArgs e)
        {
            if (TogMove == 1)  // Eğer sürükleme işlemi aktifse
            {
                this.SetDesktopLocation(MousePosition.X - M2ValX, MousePosition.Y - M2ValY);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
