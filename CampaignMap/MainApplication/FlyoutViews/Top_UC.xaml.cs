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

namespace MainApplication
{
    /// <summary>
    /// Interaction logic for Top_UC.xaml
    /// </summary>
    public partial class Top_UC : UserControl
    {
        public Top_UC()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty SaveCampaignProperty =
            DependencyProperty.Register(
                "SaveCampaign",
                typeof(ICommand),
                typeof(Top_UC),
                new UIPropertyMetadata(null));
        public ICommand SaveCampaign
        {
            get { return (ICommand)GetValue(SaveCampaignProperty); }
            set { SetValue(SaveCampaignProperty, value); }
        }

        public static readonly DependencyProperty LoadCampaignProperty =
            DependencyProperty.Register(
                "LoadCampaign",
                typeof(ICommand),
                typeof(Top_UC),
                new UIPropertyMetadata(null));
        public ICommand LoadCampaign
        {
            get { return (ICommand)GetValue(LoadCampaignProperty); }
            set { SetValue(LoadCampaignProperty, value); }
        }

        public static readonly DependencyProperty CreateCampaignProperty =
            DependencyProperty.Register(
                "CreateCampaign",
                typeof(ICommand),
                typeof(Top_UC),
                new UIPropertyMetadata(null));
        public ICommand CreateCampaign
        {
            get { return (ICommand)GetValue(CreateCampaignProperty); }
            set { SetValue(CreateCampaignProperty, value); }
        }

    }
}
