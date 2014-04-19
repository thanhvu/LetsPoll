using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.Net.Sockets;
using Microsoft.Phone.Net.NetworkInformation;
using System.Runtime.Serialization.Json;
namespace LetsPoll
{
    public partial class MainPage : PhoneApplicationPage
    {
        NetworkInterfaceType networkInterfaceType;
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            DataContext = App.ViewModel;
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
            Loaded += OnLoaded;
            DeviceNetworkInformation.NetworkAvailabilityChanged += new EventHandler<NetworkNotificationEventArgs>(DeviceNetworkInformation_NetworkAvailabilityChanged);
            LoadInitialInformation();
        }

        // Load data for the ViewModel Items
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            
            
           

        }
        public void LoadInitialInformation()
        {

            networkInterfaceType = NetworkInterface.NetworkInterfaceType;
            textBoxNetworkInterfaceType.Text = networkInterfaceType.ToString();
            checkBoxWifi.IsChecked = DeviceNetworkInformation.IsWiFiEnabled;
            checkBoxDataRoaming.IsChecked = DeviceNetworkInformation.IsCellularDataRoamingEnabled;
            checkBoxDataEnabled.IsChecked = DeviceNetworkInformation.IsCellularDataEnabled;
        }

        void DeviceNetworkInformation_NetworkAvailabilityChanged(object sender, NetworkNotificationEventArgs e)
        {
            LoadInitialInformation();
        }
        private void buttonRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadInitialInformation();
        }

        private void buttonBar_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new System.Uri(@"/BarChartResult.xaml", UriKind.RelativeOrAbsolute));
        }

        private void buttonPie_Click(object sender, RoutedEventArgs e)
        {

        }

        private void buttonSend_Click(object sender, RoutedEventArgs e)
        {
            bool retVal;
            SocketLibrary.SocketLibrary sl = new SocketLibrary.SocketLibrary();
            retVal = sl.EstablishTCPConnection(textBoxHostAddress.Text, int.Parse(textBoxHostPort.Text));
            retVal = sl.Send(textBoxTextToSend.Text);
            textBoxResponse.Text = sl.Receive();
            sl.CloseSocket();

        }
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            var webClient = new WebClient();

            webClient.OpenReadCompleted += OnOpenReadCompleted;

            webClient.OpenReadAsync(new Uri("http://compiledexperience.com/windows-phone-7/my-ip", UriKind.Absolute));
        }

        private void OnOpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            var serializer = new DataContractJsonSerializer(typeof(IPResult));
            var ipResult = (IPResult)serializer.ReadObject(e.Result);

            IPAddress.Text = ipResult.IP;
        }
    }
}