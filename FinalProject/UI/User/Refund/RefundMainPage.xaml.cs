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

namespace FinalProject.UI
{
    /// <summary>
    /// RefundMainPage.xaml 的交互逻辑
    /// </summary>
    public partial class RefundMainPage : Page
    {
        public delegate void MyEventHandler();
        public event MyEventHandler GoHomePage;

        public RefundMainPage(MyEventHandler fun)
        {
            InitializeComponent();
            GoHomePage += fun;
        }
        public void GoBack()
        {
            GoHomePage();
        }
    }
}
