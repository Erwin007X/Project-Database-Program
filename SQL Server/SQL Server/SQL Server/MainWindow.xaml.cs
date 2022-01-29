using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace SQL_Server
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        Window loginwin;
        public MainWindow(Window parent, string fullname, byte[] byteImage)
        {
            InitializeComponent();
            loginwin = parent;
            fullnameBox.Text = fullname;

            BitmapImage imgsource = new BitmapImage();

            using (MemoryStream ms = new MemoryStream())
            {
                ms.Write(byteImage, 0, byteImage.Length);
                ms.Seek(0, SeekOrigin.Begin);
                imgsource.BeginInit();
                imgsource.StreamSource = ms;
                imgsource.CacheOption = BitmapCacheOption.OnLoad;
                imgsource.EndInit();
                imgsource.Freeze();
                userimage.Source = imgsource;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            loginwin.Show();
        }
    }
}
