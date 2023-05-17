using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
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

namespace CS_Lab5_Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        HttpClient client = new HttpClient();
        string baseAdress = "http://localhost:5161/";
        string callerName = string.Empty;
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void connectButton_Click(object sender, RoutedEventArgs e)
        {
            if (connectButton.Content == "Connect")
            {
                callerName = callerNameTB.Text;
                using HttpResponseMessage response = await client.PutAsync($"{baseAdress}User/Connect/{callerName}", null);

                //response.EnsureSuccessStatusCode();
                connectLabel.Content = "Connected";
                callerNameTB.IsEnabled = false;
                connectButton.Content = "Disconnect";

                var jsonResponse = await response.Content.ReadAsStringAsync();
                valuesRTB.AppendText("\n" + jsonResponse);
            }
            else {
                using HttpResponseMessage response = await client.PostAsync($"{baseAdress}User/Disconnect/{callerName}", null);

                //response.EnsureSuccessStatusCode();
                connectLabel.Content = "Disconnected";
                callerNameTB.IsEnabled = true;
                connectButton.Content = "Connect";

                var jsonResponse = await response.Content.ReadAsStringAsync();
                valuesRTB.AppendText("\n" + jsonResponse);
            }
        }

        private async void executeButton_Click(object sender, RoutedEventArgs e)
        {
            switch (executeCB.SelectedIndex)
            {
                case 0:
                    using (HttpResponseMessage response = await client.GetAsync($"{baseAdress}Files/Users/{callerName}"))
                    {
                        var jsonResponse = await response.Content.ReadAsStringAsync();
                        valuesRTB.AppendText("\n" + jsonResponse);
                    }
                    break;
                case 1:
                    using (HttpResponseMessage response = await client.GetAsync($"{baseAdress}Files/Public/{callerName}"))
                    {
                        var jsonResponse = await response.Content.ReadAsStringAsync();
                        valuesRTB.AppendText("\n" + jsonResponse);
                    }
                    break;
                case 2:
                    using (HttpResponseMessage response = await client.GetAsync($"{baseAdress}Files/Private/{callerName}"))
                    {
                        var jsonResponse = await response.Content.ReadAsStringAsync();
                        valuesRTB.AppendText("\n" + jsonResponse);
                    }
                    break;
                case 3:
                    var textRange = new TextRange(ContentRTB.Document.ContentStart, ContentRTB.Document.ContentEnd);
                    string state;
                    if ((bool)privateRB.IsChecked)
                    {
                        state = "true";
                    }
                    else
                    {
                        state = "false";
                    }
                        using (HttpResponseMessage response = await client.PutAsync($"{baseAdress}Files/Add/{callerName}/{state}/{executionTB.Text}", new StringContent(textRange.Text, Encoding.UTF8,"application/json")))
                        {
                            var jsonResponse = await response.Content.ReadAsStringAsync();
                            valuesRTB.AppendText("\n" + jsonResponse);
                        }
            
                    break;
                case 4:
                    using (HttpResponseMessage response = await client.DeleteAsync($"{baseAdress}Files/Delete/{callerName}/{executionTB.Text}"))
                    {
                        var jsonResponse = await response.Content.ReadAsStringAsync();
                        valuesRTB.AppendText("\n" + jsonResponse);
                    }

                    break;
                case 5:
                    using (HttpResponseMessage response = await client.PostAsync($"{baseAdress}Files/ChangeStatus/{callerName}/{executionTB.Text}",null))
                    {
                        var jsonResponse = await response.Content.ReadAsStringAsync();
                        valuesRTB.AppendText("\n" + jsonResponse);
                    }

                    break;
            }

        }
    }
}
