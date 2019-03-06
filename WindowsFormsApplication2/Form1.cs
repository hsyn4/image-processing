using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
        string dosyaYolu = string.Empty;
        Bitmap bmp = null;
        public Form1()
        {
            InitializeComponent();
        }

        private void resimyükle_Click(object sender, EventArgs e)
        {
            openFileDialog1.Title = "Dosya seçiniz";
            openFileDialog1.Filter = " Text files (*.jpg) | *.jpg|Tüm dosyalar (*.*)|*.*";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                /* pictureBox1.Image = new Bitmap(openFileDialog1.OpenFile());
                 textBox1.Text = openFileDialog1.FileName; */

                dosyaYolu = openFileDialog1.FileName;

                //bir bitmap nesnesi oluşturulur ve seçilen resim bu nesneye yüklenir. 

                bmp = new Bitmap(dosyaYolu);

                pictureBox1.Image = bmp;

                //picturebox nesnesinin sizemode özelliği strech olarak ayarlanır.Bunun //sebebi ise seçilen resmin picturebox nesnesinde tam olarak gözükmesini sağlamaktır.

                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;

            }

        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }




        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnGrayScale_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null)
            {
                MessageBox.Show("Resim Seçilmedi!!");
            }
            else
            {
                progressBar1.Visible = true;

                Bitmap bitmap1 = new Bitmap(pictureBox1.Image);
                Bitmap grayScale = GrayScale(bitmap1);
                pictureBox2.Image = grayScale;
                progressBar1.Visible = false;

            }

        }











        private void btnNegative_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null)
            {
                MessageBox.Show("Önce bir resim Seçin");
            }
            else
            {
                progressBar1.Visible = true;

                Bitmap bitmap1 = new Bitmap(pictureBox1.Image);
                Bitmap negative = NegativeImage(bitmap1);
                progressBar1.Maximum = bitmap1.Width * bitmap1.Height;

                pictureBox2.Image = negative;
            }


        }





        private void btnSobel_Click(object sender, EventArgs e)
        {

            if (pictureBox1.Image == null)
            {
                MessageBox.Show("Resim Seçilmedi!!");
            }
            else
            {
                progressBar1.Visible = true;
                Bitmap bitmap1 = new Bitmap(pictureBox1.Image);
               // Bitmap gray = GrayScale(bitmap1);
                Bitmap sobel = SobelEdgeDetection(bitmap1);
               // Buffer buffer = new Bitmap(gray.Width, gray.Height);




                pictureBox2.Image = sobel;
                progressBar1.Visible = false;
            }

        }

        private Bitmap SobelEdgeDetection(Bitmap bitmap1)
        {
            Bitmap gray = GrayScale(bitmap1);
           Bitmap buffer = new Bitmap(gray.Width, gray.Height);
            Color renk;
            int valX, valY,gradient;
           // int width = bitmap1.Width;
            //int height = bitmap1.Height;
         
           
            int[,] sx = new int[3,3]; //{ { -1, 0, 1 }, { -2, 0, 2 }, { -1, 0, 1 } };
            int[,] sy = new int[3, 3]; //{ { 1, 2, 1 }, { 0, 0, 0 }, { -1, -2, -1 } };
            sx [0,0] = -1;  sx [0,1] = 0;  sx [0,2] = 1;
            sx [1,0] = -2;  sx [1,1] = 0;  sx [1,2] = 2;    
            sx [2,0] = -1;  sx [2,1] = 0;  sx [2,2] = 1;                      
            
            sy [0,0]=-1;  sy [0,1]=-2;  sy [0,2]=-1;
            sy [1,0]=0;  sy [1,1]=0;  sy [1,2]=0;
            sy [2,0]=1;  sy [2,1]=2;  sy [2,2]=1;


            for (int i = 0; i < gray.Height;i++ )
            {
                for(int j=0; j<gray.Width;j++)
                {
                     
                    if(i==0 || i == gray.Height-1 || j==0 || j == gray.Width-1  ) {

                        renk = Color.FromArgb(255, 255, 255);

                        buffer.SetPixel(j, i, renk);
                        valX = 0;
                        valY = 0;


                        }  else
                    {

                        valX= gray.GetPixel(j-1,i-1).R * sx[0,0] +
                            gray.GetPixel(j,i-1).R * sx[0,1] +
                            gray.GetPixel(j+1,i-1).R * sx[0,2] +
                            gray.GetPixel(j-1,i).R * sx[1,0] +
                            gray.GetPixel(j,i).R * sx[1,1] +
                            gray.GetPixel(j+1,i).R * sx[1,2] +
                            gray.GetPixel(j-1,i+1).R * sx[2,0] +
                            gray.GetPixel(j,i+1).R * sx[2,1] +
                            gray.GetPixel(j+1,i+1).R * sx[2,2] ;

                        valY = gray.GetPixel(j - 1, i - 1).R * sy[0, 0] +
                              gray.GetPixel(j, i - 1).R * sy[0, 1] +
                              gray.GetPixel(j + 1, i - 1).R * sy[0, 2] +
                              gray.GetPixel(j - 1, i).R * sy[1, 0] +
                              gray.GetPixel(j, i).R * sy[1, 1] +
                              gray.GetPixel(j + 1, i).R * sy[1, 2] +
                              gray.GetPixel(j - 1, i + 1).R * sy[2, 0] +
                              gray.GetPixel(j, i + 1).R * sy[2, 1] +
                              gray.GetPixel(j + 1, i + 1).R * sy[2, 2];


                        gradient = (int)(Math.Abs(valX) + Math.Abs(valY));

                        if(gradient < 0)
                        {
                            gradient=0 ;
                        }
                        if (gradient > 255) { gradient = 255; }

                        renk = Color.FromArgb(gradient, gradient, gradient);
                        buffer.SetPixel(j, i, renk);

                    }


                }


            }



      return buffer;
        }


        private Bitmap GrayScale(Bitmap bitmap1)
        {
            int i, j;
            Color renk;
            progressBar1.Maximum = bitmap1.Width * bitmap1.Height;
            for (i = 0; i <= bitmap1.Width - 1; i++)
            {
                for (j = 0; j <= bitmap1.Height - 1; j++)
                {
                    renk = bitmap1.GetPixel(i, j);
                    renk = Color.FromArgb((byte)((renk.R + renk.G + renk.B) / 3), (byte)((renk.R + renk.G + renk.B) / 3), (byte)((renk.R + renk.G + renk.B) / 3));
                    bitmap1.SetPixel(i, j, renk);
                    if ((i % 10) == 0)
                    {
                        progressBar1.Value = i * bitmap1.Height + j;
                        Application.DoEvents();
                    }
                }
            }

            return bitmap1;

        }


        private Bitmap NegativeImage(Bitmap bitmap1)
        {
        
            int i, j;
            Color renk;
            progressBar1.Maximum = bitmap1.Width * bitmap1.Height;
            for (i = 0; i <= bitmap1.Width - 1; i++)
            {
                for (j = 0; j <= bitmap1.Height - 1; j++)
                {
                    renk = bitmap1.GetPixel(i, j);
                  
                    renk = Color.FromArgb(renk.A, (byte)~renk.R, (byte)~renk.G, (byte)~renk.B);
                    bitmap1.SetPixel(i, j, renk);
                    if ((i % 10) == 0)
                    {
                        progressBar1.Value = i * bitmap1.Height + j;
                        Application.DoEvents();
                      
                    }
                   
                }

               

            }
            return bitmap1;
          
        }
    }
}
