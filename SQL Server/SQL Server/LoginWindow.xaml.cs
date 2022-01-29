using System.Windows;
using System.Windows.Input;
using MaterialDesignThemes.Wpf;
using System.Data.SqlClient;
using System;
using System.Windows.Threading;
using SQL_Server.Properties;

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
            ////Connection to Sql Server
            sqlcon.conn = new SqlConnection("Data Source=DESKTOP-O4D2RM3;Initial Catalog=database;User ID=erwin;Password=123456789;");
        }


        #region button
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

        private void exitApp(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void createbt_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            RegisterWindow rg = new RegisterWindow();
            rg.Show();
        }

        #endregion


        #region drag move

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            DragMove();
        }

        #endregion


        #region toggle
        public bool IsDarkTheme { get; set; }
        private readonly PaletteHelper paletteHelper = new PaletteHelper();
        private void toggleTheme(object sender, RoutedEventArgs e)
        {
        }

        #region user config Toggel


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
                topmostToggle.IsChecked = true; 
                base.Topmost = true;
                new Settings().topmost = true;
            }
            bool flag =! Settings.Default.topmost;
            if (flag)
            {
                topmostToggle.IsChecked = false; 
                base.Topmost = false; 
                new Settings().topmost = false; 
            }

            ITheme theme = paletteHelper.GetTheme();
            bool thememode = Settings.Default.mode;
            if (thememode)
            {
                themeToggle.IsChecked = true;
                new Settings().mode = true;

                if (IsDarkTheme = theme.GetBaseTheme() == BaseTheme.Light)
                {
                    IsDarkTheme = true;
                    theme.SetBaseTheme(Theme.Dark);
                }
                paletteHelper.SetTheme(theme);
            }

            bool flag1 =! Settings.Default.mode;
            if (flag1)
            {
                themeToggle.IsChecked = false;
                new Settings().topmost = false;

                if (IsDarkTheme = theme.GetBaseTheme() == BaseTheme.Dark)
                {
                    IsDarkTheme = false;
                    theme.SetBaseTheme(Theme.Light);
                }
                paletteHelper.SetTheme(theme);
            }


        }

        private void topmostToggle_Checked(object sender, RoutedEventArgs e)
        {
            Settings.Default.topmost = this.topmostToggle.IsChecked.Value;
            Settings.Default.Save();
            
        }

        private void topmostToggle_Unchecked(object sender, RoutedEventArgs e)
        {
            Settings.Default.topmost = this.topmostToggle.IsChecked.Value;
            Settings.Default.Save();
        }

        private void themeToggle_Checked(object sender, RoutedEventArgs e)
        {
            Settings.Default.mode = this.themeToggle.IsChecked.Value;
            Settings.Default.Save();
        }

        private void themeToggle_Unchecked(object sender, RoutedEventArgs e)
        {
            Settings.Default.mode = this.themeToggle.IsChecked.Value;
            Settings.Default.Save();
        }
        #endregion

        #endregion

    }
}
