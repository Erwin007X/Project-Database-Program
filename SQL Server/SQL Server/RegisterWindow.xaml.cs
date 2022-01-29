using Microsoft.Win32;
using SQL_Server.Properties;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace SQL_Server
{
    /// <summary>
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    /// 

    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();
        }

        private void exithide_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginwin = new LoginWindow();
            loginwin.Show();
            this.Hide();
        }

        private void imagebt_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog()
            {
                Title = "add a image",
                Filter = "File image|*.*"
            };

            if (ofd.ShowDialog() == true)
            {
                userimage.Source = new BitmapImage(new Uri(ofd.FileName));
            }
        }
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            DragMove();
        }

        private void register_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                sqlcon.conn.Open();
                sqlcon.cmdText = "select * from Table_1 where username = '" + usernameBox.Text.Trim() + "'";
                sqlcon.cmd = new SqlCommand(sqlcon.cmdText, sqlcon.conn);
                sqlcon.dr = sqlcon.cmd.ExecuteReader();

                if (sqlcon.dr.HasRows)
                {
                    MessageBox.Show("Username already exists");
                }
                else
                {
                    sqlcon.conn.Close();
                    sqlcon.cmdText = "insert into Table_1(username,password,image,firstname,lastname) values ('" + usernameBox.Text.Trim() + "','" + passwordBox.Password.Trim() + "',@photo,'" + firstname.Text.Trim() + "','" + lastname.Text.Trim() + "')";
                    sqlcon.conn.Open();
                    sqlcon.cmd = new SqlCommand(sqlcon.cmdText, sqlcon.conn);

                    byte[] byteimage;
                    BitmapSource bitmap = (BitmapSource)userimage.Source;
                    PngBitmapEncoder encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(bitmap));

                    using (MemoryStream ms = new MemoryStream())
                    {
                        encoder.Save(ms);
                        byteimage = ms.ToArray();
                    }
                    sqlcon.cmd.Parameters.AddWithValue("@photo", byteimage);
                    sqlcon.cmd.ExecuteNonQuery();

                    MessageBox.Show("successfully", "notification", MessageBoxButton.OK, MessageBoxImage.Information);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DispatcherTimer dispatcherTimer1 = new DispatcherTimer();
            dispatcherTimer1.Tick += this.settingsTimer;
            dispatcherTimer1.Start();
        }


        private void settingsTimer(object source, EventArgs e)
        {

            bool topMost = Settings.Default.topmost;
            if (topMost)
            {
                base.Topmost = true;
                new Settings().topmost = true;
            }
            bool flag = !Settings.Default.topmost;
            if (flag)
            {
                base.Topmost = false;
                new Settings().topmost = false;
            }
        }
    }
}
