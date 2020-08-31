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
namespace FinalProject.UI
{
    /// <summary>
    /// InsertMoviePage.xaml 的交互逻辑
    /// </summary>
    public partial class InsertMoviePage : Page
    {
        private bool isEdit = false;
        Dictionary<string, string> keyValuePairs = null;

        public InsertMoviePage()
        {
            InitializeComponent();
        }

        //切换UI的模式
        private void SetUIMode(bool mode)
        {
            if (mode == false)
            {
                //导入模式
                CompleteInputBtn.Content = "确定插入";
            }
            else
            {
                //编辑模式
                CompleteInputBtn.Content = "确定修改";
            }

        }

        //编辑模式，从dbpage 传入电影字典
        public void Edit(Dictionary<string, string> dic)
        {
            keyValuePairs = dic;
            isEdit = true;
            SetUIMode(isEdit);//UI切换成编辑模式
            //把传过来的dic渲染到输入框
            mov_name.Text = dic["Name"];
            mov_startplaytime.Text = dic["StartPlayingTime"];
            mov_endplaytime.Text = dic["EndPlayingTime"];
            mov_price.Text = dic["Price"];
            mov_img_path.Text = dic["img_path"];
            mov_length.Text = dic["Length"];
            mov_type.Text = dic["Type"];
            mov_language.Text = dic["Language"];
            mov_country.Text = dic["Country"];
            mov_director.Text = dic["Director"];
            mov_scriptwriter.Text = dic["Scriptwriter"];
            mov_actor.Text = dic["Actor"];
            mov_introduce.Text = dic["Introduce"];
        }

        //导入模式，点击从文件导入按钮从文件传入数据
        private void InsertFromFile_Click(object sender, RoutedEventArgs e)
        {
            //从文件中导入
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = "文本文件|*.txt";
            if (dialog.ShowDialog() == true)
            {
                mov_name.Text = "美国队长 Captain America: The First Avenger (2011)";
                mov_length.Text = "124";
                mov_type.Text = "动作 / 科幻 / 冒险";
                mov_director.Text = "乔·庄斯顿";
                mov_scriptwriter.Text = " 克里斯托弗·马库斯 / 斯蒂芬·麦克菲利 / 乔·西蒙 / 杰克·科比";
                mov_actor.Text = " 克里斯·埃文斯 / 海莉·阿特维尔 / 塞巴斯蒂安·斯坦 / 汤米·李·琼斯 / 雨果·维文 / 多米尼克·库珀 / 理查德·阿米蒂奇 / 斯坦利·图齐 / 塞缪尔·杰克逊 / 托比·琼斯 / 尼尔·麦克唐纳 / 德里克·卢克 / 肯尼斯·崔 / 约翰·约瑟夫·菲尔德 / 娜塔莉·多默尔";
                mov_country.Text = "美国";
                mov_language.Text = "英语";
                mov_price.Text = "50";
                mov_introduce.Text = "上世纪40年代，纳粹及其邪恶轴心的战火烧遍世界各个角落。居住在布鲁克林的小个子史蒂夫·罗格斯（克里斯·埃文斯 Chris Evans 饰）心系国家，一心上阵杀敌，可是糟糕的体格让他始终被征兵办拒之门外。偶然的机会，在德籍科学家厄斯金博士（Stanley Tucci 饰）的帮助下，这个小个子男孩得以走入兵营，并接受了博士的试验，化身成为高大健壮、膂力过人的超级战士。与此同时，德国纳粹红骷髅部队的首领约翰·施密特（雨果·维文 Hugo Weaving 饰）依靠超自然的力量建立起一支超级战队，企图称霸全世界。";
                mov_img_path.Text = "meiguoduizhang.jpg";
                mov_startplaytime.Text = "2011-09-09";
                mov_endplaytime.Text = "2100-09-09";
                isEdit = false;
                SetUIMode(isEdit);//UI切换成插入模式
            }
        }

        //点击确定导入/修改
        private void CompleteInputBtn_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>
            {
                {"Name",mov_name.Text },
                {"Length",mov_length.Text },
                {"Type",mov_type.Text },
                {"Director",mov_director.Text },
                {"Scriptwriter",mov_scriptwriter.Text },
                {"Actor",mov_actor.Text },
                {"Country",mov_country.Text },
                {"Language",mov_language.Text },
                {"Price",mov_price.Text },
                {"Introduce",mov_introduce.Text },
                {"img_path",mov_img_path.Text },
                {"StartPlayingTime",mov_startplaytime.Text},
                {"EndPlayingTime",mov_endplaytime.Text }
            };
            if (isEdit == true)
            {
                dic["Id"] = keyValuePairs["Id"];
                dic["State"] = keyValuePairs["State"];
            }
            else
            {
                dic["State"] = Config.MOVIE_STATE[Config.MOVIE_STATE_COMING];
            }
            //检测输入框是否为空
            foreach (var item in dic)
            {
                if (item.Value == "")
                {
                    MessageBox.Show(String.Format("{0}为空", item.Key));
                    return;
                }
                
            }

            string message = "";
            string title = "";
            string endmessage_succ = "";
            string endmessage_faild = "";
            if (isEdit == false)
            {
                message = "确认插入?";
                title = "插入电影";
                endmessage_succ = "插入成功";
                endmessage_faild = "插入失败";
            }
            else
            {
                message = "确认修改?";
                title = "修改电影";
                endmessage_succ = "修改成功";
                endmessage_faild = "修改失败";
            }

            //确定是否开始插入
            if (MessageBox.Show(message, title, MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
            {

                //所有数据直接传入控制器，由service检测并执行
                string res = "";
                if (isEdit == false)
                {
                    res += AdminMainWindow.adminController.AddMovie(dic);
                }
                else
                {
                    res += AdminMainWindow.adminController.UpdateMovie(dic);
                }

                
                if (res == "")
                {
                    MessageBox.Show(endmessage_succ);
                    //插入成功之后清除当前数据，并恢复导入模式
                    ClearText();
                }
                else
                {
                    MessageBox.Show(endmessage_faild + res);
                }
            }
        }
        //清除显示的数据
        private void ClearText()
        {
            mov_name.Text = "";
            mov_length.Text = "";
            mov_type.Text = "";
            mov_director.Text = "";
            mov_scriptwriter.Text = "";
            mov_actor.Text = "";
            mov_country.Text = "";
            mov_language.Text = "";
            mov_price.Text = "";
            mov_introduce.Text = "";
            mov_img_path.Text = "";
            mov_startplaytime.Text = "";
            mov_endplaytime.Text = "";
            isEdit = false;
            SetUIMode(isEdit);
        }
        //点击清除按钮
        private void ClearBtn_Click(object sender, RoutedEventArgs e)
        {
            ClearText();
        }
    }
}
