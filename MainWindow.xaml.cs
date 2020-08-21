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

        bool GetUnfilledOnly = false;

        public MainWindow()
        {
            InitializeComponent();

            GetData();
        }

        private void GetData()
        {
            if (GetUnfilledOnly)
            {
                data = dataController.GetUnfilledData();
            }
            else
            {
                data = dataController.GetAllData();
            }

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
            Input.Text = (string)((IDictionary<string, object>)CurrentItem)[dataController.CurrentLanguage];
            comboBox.ItemsSource = dataController.Languages;
            comboBox.SelectedItem = dataController.CurrentLanguage;
            comboBox.Items.Refresh();

            if (data.Count() != 0)
            {
                HideError();
            }
            else
            {
                ShowError();
            }
        }

        private void SaveItem()
        {
            ((IDictionary<string, object>)CurrentItem)[dataController.CurrentLanguage] = Input.Text;
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

        private void HideError()
        {
            button_next.Visibility = Visibility.Visible;
            button_previous.Visibility = Visibility.Visible;
            Description.Visibility = Visibility.Visible;
            Input.Visibility = Visibility.Visible;
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

        private void button_AddLanguage_Click(object sender, RoutedEventArgs e)
        {
            button_AddLanguage.Visibility = Visibility.Hidden;
            textBox_language.Visibility = Visibility.Visible;
            label_language_prompt.Visibility = Visibility.Visible;
        }

        private void textBox_language_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                dataController.CreateLanguage(textBox_language.Text);
                dataController.Save();

                dataController.Languages.Add(textBox_language.Text);

                UpdateWindow();

                button_AddLanguage.Visibility = Visibility.Visible;
                textBox_language.Visibility = Visibility.Hidden;
                label_language_prompt.Visibility = Visibility.Hidden;
            }
        }

        private void Input_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (Keyboard.Modifiers.HasFlag(ModifierKeys.Shift))
                {
                    Input.Text += "\n";
                    Input.CaretIndex = Input.Text.Length;
                }
                else
                {
                    GetNext();
                }
            }
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SaveItem();
            dataController.CurrentLanguage = (string)comboBox.SelectedItem;
            UpdateWindow();
        }

        private void checkBox_unfilled_Click(object sender, RoutedEventArgs e)
        {
            CheckBox box = (CheckBox)sender;

            GetUnfilledOnly = (bool)box.IsChecked;
            GetData();
        }
    }
}
