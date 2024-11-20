using System.IO;
using System.IO.Ports;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace ArduinoClient
{
    public partial class MainWindow : Window
    {
        private SerialPort serialPort;
        public string difficulty = "";
        public MainWindow()
        {
            InitializeComponent();
            InitializeSerialPort();
        }

        private void InitializeSerialPort()
        {
            serialPort = new SerialPort("COM5", 9600)
            {
                NewLine = "\r\n"
            };
            serialPort.Open();
        }
        private void Easy_Mode_Click(object sender, EventArgs e)
        {
            MakeDifButtonsCollapsed();
            difficulty = "Easy";
        }
        private void Normal_Mode_Click(object sender, EventArgs e)
        {
            MakeDifButtonsCollapsed();
            difficulty = "Normal";
        }
        private void Impossible_Mode_Click(object sender, EventArgs e)
        {
            MakeDifButtonsCollapsed();
            difficulty = "Impossible";
        }
        private async void ChoiseRock_Click(object sender, RoutedEventArgs e)
        {
            await SendMessageAsync("Rock", difficulty);
        }
        private async void ChoisePaper_Click(object sender, RoutedEventArgs e)
        {
            await SendMessageAsync("Paper", difficulty);
        }
        private async void ChoiseScissors_Click(object sender, RoutedEventArgs e)
        {
            await SendMessageAsync("Scissors", difficulty);
        }

        private async Task SendMessageAsync(string message, string dif)
        {
            try
            { 
                if (serialPort != null && serialPort.IsOpen)
                {
                    serialPort.WriteLine(message + " " + difficulty);
                }

                string response = await Task.Run(() => serialPort.ReadLine());
                ChoiseText.Visibility = Visibility.Collapsed;
                Rock_Button.Visibility = Visibility.Collapsed;
                Paper_Button.Visibility= Visibility.Collapsed;
                Scissors_Button.Visibility= Visibility.Collapsed;  
                GameOver_Menu.Visibility = Visibility.Visible;

                switch (response) 
                {
                    case "Rock":
                        if(message == "Paper")
                        {
                            TextAferGame.Text = "You won!";
                            TextAferGame.Foreground = Brushes.Green;
                        }
                        else if (message == "Scissors")
                        {
                            TextAferGame.Text = "You lose!";
                            TextAferGame.Foreground = Brushes.Red;
                        }
                        else
                        {
                            TextAferGame.Text = "Draw!";
                            TextAferGame.Foreground = Brushes.Black;
                        }
                        Responce.Text = response;
                        Choise.Text = message;
                        break;

                    case "Paper":
                        if (message == "Paper")
                        {
                            TextAferGame.Text = "Draw!";
                            TextAferGame.Foreground = Brushes.Black;
                        }
                        else if (message == "Scissors")
                        {
                            TextAferGame.Text = "You won!";
                            TextAferGame.Foreground = Brushes.Green;
                        }
                        else
                        {
                            TextAferGame.Text = "You lose!";
                            TextAferGame.Foreground = Brushes.Red;
                        }
                        Responce.Text = response;
                        Choise.Text = message;
                        break;

                    case "Scissors":
                        if (message == "Paper")
                        {
                            TextAferGame.Text = "You lose!";
                            TextAferGame.Foreground = Brushes.Red;
                        }
                        else if (message == "Scissors")
                        {
                            TextAferGame.Text = "Draw!";
                            TextAferGame.Foreground = Brushes.Black;
                        }
                        else
                        {
                            TextAferGame.Text = "You won!";
                            TextAferGame.Foreground = Brushes.Green;
                        }
                        Responce.Text = response;
                        Choise.Text = message;
                        break;
                }
            }
            catch (TimeoutException)
            {
                MessageBox.Show("No response from Arduino.", "Timeout", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (IOException ex)
            {
                MessageBox.Show($"Communication error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OneMoreGame_Click (object sender, RoutedEventArgs e)
        {
            GameOver_Menu.Visibility = Visibility.Collapsed;
            MakeDifButtonsCollapsed();
        }

        private void ChangeDif_Click (object sender, RoutedEventArgs e)
        {
            GameOver_Menu.Visibility = Visibility.Collapsed;
            MakeDifButtonsVisible();
        }

        private void MakeDifButtonsCollapsed()
        {
            EasyButton.Visibility = Visibility.Collapsed;
            NormalButton.Visibility = Visibility.Collapsed;
            ImpossibleButton.Visibility = Visibility.Collapsed;
            GameName.Visibility = Visibility.Collapsed;
            CreateBy.Visibility = Visibility.Collapsed;
            ChoiseText.Visibility = Visibility.Visible;
            Rock_Button.Visibility=Visibility.Visible;
            Paper_Button.Visibility=Visibility.Visible;
            Scissors_Button.Visibility=Visibility.Visible; 
        }

        private void MakeDifButtonsVisible()
        {
            GameName.Visibility = Visibility.Visible;
            CreateBy.Visibility= Visibility.Visible;
            EasyButton.Visibility = Visibility.Visible;
            NormalButton.Visibility = Visibility.Visible;
            ImpossibleButton.Visibility = Visibility.Visible;
            ChoiseText.Visibility = Visibility.Collapsed;
            Rock_Button.Visibility = Visibility.Collapsed;
            Paper_Button.Visibility = Visibility.Collapsed;
            Scissors_Button.Visibility = Visibility.Collapsed;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (serialPort != null && serialPort.IsOpen)
            {
                serialPort.Close();
            }
        }
    }
}
