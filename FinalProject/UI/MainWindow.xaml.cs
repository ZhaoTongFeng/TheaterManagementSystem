using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using FinalProject.Model;
using FinalProject.UI;
namespace FinalProject
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private UserController userController = new UserController();
        private AdminController adminController = new AdminController();      
        
        AdminMainWindow adminMainWindow = new AdminMainWindow();
        UserMainWindow userMainWindow = new UserMainWindow();
        public MainWindow()
        {
            InitializeComponent();
            this.Closing += MainWindow_Closing;
            this.WindowStartupLocation = WindowStartupLocation.Manual;
            this.Left = 0;
            this.Top = 0;
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            adminMainWindow.Close();
            userMainWindow.Close();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string Content = (e.Source as Button).Content.ToString();
            if (Content == "管理员界面")
            {

                if (adminMainWindow.IsActive==false)
                {
                    adminMainWindow = new AdminMainWindow();
                }
                adminMainWindow.WindowStartupLocation = WindowStartupLocation.Manual;
                adminMainWindow.Left = 0;
                adminMainWindow.Top = 500;
                adminMainWindow.Show();

            }
            else if (Content == "用户界面")
            {

                if (userMainWindow.IsActive == false)
                {
                    userMainWindow = new UserMainWindow();
                }
                userMainWindow.WindowStartupLocation = WindowStartupLocation.Manual;
                userMainWindow.Left = 800;
                userMainWindow.Top = 500;
                userMainWindow.Show();
            }
        }

        /// <summary>
        /// 窗口关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            dbContext.db.Dispose();
            Application.Current.Shutdown();
        }

    }
}
