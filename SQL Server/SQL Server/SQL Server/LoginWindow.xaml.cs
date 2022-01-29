using System.Windows;
using System.Windows.Input;
using MaterialDesignThemes.Wpf;
using System.Data.SqlClient;
using System.Data;
using System;
namespace SQL_Server
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            sqlcon.conn = new SqlConnection("Data Source=DESKTOP-O4D2RM3;Initial Catalog=database;User ID=erwin;Password=123456789;");
        }


        private void loginbt_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                sqlcon.conn.Open();
                sqlcon.cmdText = "select count(*) from Table_1 where username = @name and password = @pass";
                sqlcon.cmd = new SqlCommand(sqlcon.cmdText, sqlcon.conn);
                sqlcon.cmd.Parameters.AddWithValue("@name", usernameBox.Text.Trim());
                sqlcon.cmd.Parameters.AddWithValue("@pass", passwordBox.Password.Trim());

                int numRow = (int)sqlcon.cmd.ExecuteScalar();

                if (numRow > 0)
                {
                    sqlcon.cmdText = "select * from Table_1 where username = '" + usernameBox.Text.Trim() + "'";
                    sqlcon.cmd = new SqlCommand(sqlcon.cmdText, sqlcon.conn);
                    sqlcon.dr = sqlcon.cmd.ExecuteReader();

                    if (sqlcon.dr != null)
                    {
                        sqlcon.dr.Read();
                        MainWindow mainwin = new MainWindow(this, $"{sqlcon.dr["firstname"]} {sqlcon.dr["lastname"]}", (byte[])sqlcon.dr["image"]);
                        mainwin.Show();
                        this.Hide();
                        sqlcon.dr.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Wrong username and / or password. Please try again.", "notification", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                sqlcon.conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message, "notifiaction", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                sqlcon.conn.Close();
            }
        }

        public bool IsDarkTheme { get; set; }
        private readonly PaletteHelper paletteHelper = new PaletteHelper();
        private void toggleTheme(object sender, RoutedEventArgs e)
        {
            ITheme theme = paletteHelper.GetTheme();

            if (IsDarkTheme = theme.GetBaseTheme() == BaseTheme.Dark)
            {
                IsDarkTheme = false;
                theme.SetBaseTheme(Theme.Light);
            }
            else
            {
                IsDarkTheme = true;
                theme.SetBaseTheme(Theme.Dark);
            }
            paletteHelper.SetTheme(theme);
        }

        private void exitApp(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            DragMove();
        }

        private void createbt_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            RegisterWindow rg = new RegisterWindow();
            rg.Show();
        }
    }
}
