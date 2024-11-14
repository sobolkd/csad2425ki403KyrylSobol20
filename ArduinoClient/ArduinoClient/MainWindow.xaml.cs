using System.IO.Ports;
using System.Windows.Threading;
using System.Windows;


namespace ArduinoClient
{
    public partial class MainWindow : Window
    {
        private SerialPort serialPort;

        public MainWindow()
        {
            InitializeComponent();
            InitializeSerialPort();
        }

        private void InitializeSerialPort()
        {
            // Налаштовуємо COM5 для зв'язку з Arduino
            serialPort = new SerialPort("COM5", 9600);
            serialPort.DataReceived += SerialPort_DataReceived;
            serialPort.Open();
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            // Зчитуємо повідомлення від Arduino
            string receivedMessage = serialPort.ReadLine();
            Dispatcher.Invoke(() =>
            {
                ReceivedMessage.Text = receivedMessage;
            });
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            // Відправляємо повідомлення на Arduino
            if (serialPort.IsOpen)
            {
                serialPort.WriteLine(MessageToSend.Text);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Закриваємо послідовний порт при закритті вікна
            if (serialPort != null && serialPort.IsOpen)
            {
                serialPort.Close();
            }
        }
    }
}

