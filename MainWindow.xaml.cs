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

namespace VTOLVR_Translation_tool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DataController dataController = new DataController();

        IEnumerable<dynamic> data;

        int index = 0;
        dynamic CurrentItem;

        public MainWindow()
        {
            InitializeComponent();

            data = dataController.GetAllData();

            if (data.Count() == 0)
            {
                ShowError();
            }
            else
            {
                FetchItem();
            }
        }

        private void FetchItem()
        {
            if (CurrentItem != null)
            {
                SaveItem();
            }

            CurrentItem = data.ElementAtOrDefault(index);
            UpdateWindow();
        }

        private void UpdateWindow()
        {
            label_title.Content = CurrentItem.en;
            Description.Content = CurrentItem.Description;
            label_index.Content = $"{index}/{data.Count() - 1}";
            Input.Text = CurrentItem.ko;
        }

        private void SaveItem()
        {
            CurrentItem.ko = Input.Text;
        }

        private void GetNext()
        {
            if (index != data.Count() - 1)
            {
                index++;
                FetchItem();
            }
            else
            {
                index = 0;
                FetchItem();
            }
        }

        private void GetPrevious()
        {
            if (index != 0)
            {
                index--;
                FetchItem();
            }
            else
            {
                index = data.Count() - 1;
                FetchItem();
            }
        }

        private void ShowError()
        {
            label_title.Content = "Something went wrong 😥";

            button_next.Visibility = Visibility.Hidden;
            button_previous.Visibility = Visibility.Hidden;
            Description.Visibility = Visibility.Hidden;
            Input.Visibility = Visibility.Hidden;
        }

        private void button_previous_Click(object sender, RoutedEventArgs e)
        {
            GetPrevious();
        }

        private void button_next_Click(object sender, RoutedEventArgs e)
        {
            GetNext();
        }

        private void button_save_Click(object sender, RoutedEventArgs e)
        {
            SaveItem();
            dataController.Save();
        }
    }
}
