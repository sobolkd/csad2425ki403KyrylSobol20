using System;
using System.IO;
using System.IO.Ports;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Newtonsoft.Json.Linq;

namespace ArduinoClient
{
    public partial class MainWindow : Window
    {
        private SerialPort serialPort;
        public string difficulty = "";

        /*!
         * @brief Конструктор для ініціалізації вікна і підключення до порту
         * @details Цей конструктор ініціалізує компоненти вікна та викликає метод для ініціалізації серійного порту.
         */
        public MainWindow()
        {
            InitializeComponent();
            InitializeSerialPort();
        }

        /*!
         * @brief Ініціалізація серійного порту для зв'язку з Arduino
         * @details Встановлює параметри серійного порту (COM5, 9600 біт/с) і відкриває його для зв'язку з Arduino.
         */
        private void InitializeSerialPort()
        {
            serialPort = new SerialPort("COM5", 9600)
            {
                NewLine = "\r\n"
            };
            serialPort.Open();
        }

        /**
         * @brief Обробник кнопки для вибору Easy mode
         * @param sender Об'єкт, що ініціював подію (кнопка)
         * @param e Параметри події
         * @details При виборі "Easy" рівень складності змінюється на "Easy" та приховуються кнопки для вибору складності.
         */
        private void Easy_Mode_Click(object sender, RoutedEventArgs e)
        {
            MakeDifButtonsCollapsed();
            difficulty = "Easy";
        }

        /**
         * @brief Обробник кнопки для вибору Normal mode
         * @param sender Об'єкт, що ініціював подію (кнопка)
         * @param e Параметри події
         * @details При виборі "Normal" рівень складності змінюється на "Normal" та приховуються кнопки для вибору складності.
         */
        private void Normal_Mode_Click(object sender, RoutedEventArgs e)
        {
            MakeDifButtonsCollapsed();
            difficulty = "Normal";
        }

        /**
         * @brief Обробник кнопки для вибору Impossible mode
         * @param sender Об'єкт, що ініціював подію (кнопка)
         * @param e Параметри події
         * @details При виборі "Impossible" рівень складності змінюється на "Impossible" та приховуються кнопки для вибору складності.
         */
        private void Impossible_Mode_Click(object sender, RoutedEventArgs e)
        {
            MakeDifButtonsCollapsed();
            difficulty = "Impossible";
        }

        /**
         * @brief Обробник для вибору Rock
         * @param sender Об'єкт, що ініціював подію (кнопка)
         * @param e Параметри події
         * @details Надсилає повідомлення з вибором "Rock" до Arduino.
         */
        private async void ChoiseRock_Click(object sender, RoutedEventArgs e)
        {
            await SendMessageAsync("Rock", difficulty);
        }

        /**
         * @brief Обробник для вибору Paper
         * @param sender Об'єкт, що ініціював подію (кнопка)
         * @param e Параметри події
         * @details Надсилає повідомлення з вибором "Paper" до Arduino.
         */
        private async void ChoisePaper_Click(object sender, RoutedEventArgs e)
        {
            await SendMessageAsync("Paper", difficulty);
        }

        /**
         * @brief Обробник для вибору Scissors
         * @param sender Об'єкт, що ініціював подію (кнопка)
         * @param e Параметри події
         * @details Надсилає повідомлення з вибором "Scissors" до Arduino.
         */
        private async void ChoiseScissors_Click(object sender, RoutedEventArgs e)
        {
            await SendMessageAsync("Scissors", difficulty);
        }

        /**
         * @brief Функція для надсилання повідомлення до Arduino і отримання відповіді
         * @param message Хід гравця ("Rock", "Paper", "Scissors")
         * @param dif Рівень складності ("Easy", "Normal", "Impossible")
         * @return Task Асинхронний результат виконання.
         * @details Функція надсилає повідомлення до Arduino, отримує відповідь у форматі JSON і відображає результат гри.
         */
        private async Task SendMessageAsync(string message, string dif)
        {
            try
            {
                // Перевіряємо, чи відкритий серійний порт
                if (serialPort != null && serialPort.IsOpen)
                {
                    // Надсилаємо повідомлення до Arduino
                    serialPort.WriteLine(message + " " + dif);
                }

                // Читаємо відповідь від Arduino
                string response = await Task.Run(() => serialPort.ReadLine());

                // Парсимо JSON відповідь
                JObject jsonResponse = JObject.Parse(response);
                string playerMove = jsonResponse["player_move"].ToString();
                string arduinoMove = jsonResponse["arduino_move"].ToString();
                string difficulty = jsonResponse["difficulty"].ToString();

                // Приховуємо кнопки вибору та відображаємо результат гри
                ChoiseText.Visibility = Visibility.Collapsed;
                Rock_Button.Visibility = Visibility.Collapsed;
                Paper_Button.Visibility = Visibility.Collapsed;
                Scissors_Button.Visibility = Visibility.Collapsed;
                GameOver_Menu.Visibility = Visibility.Visible;

                // Логіка для визначення результату гри
                if (arduinoMove == "Rock")
                {
                    if (message == "Paper")
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
                    Responce.Text = arduinoMove;
                    Choise.Text = message;
                }
                else if (arduinoMove == "Paper")
                {
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
                    Responce.Text = arduinoMove;
                    Choise.Text = message;
                }
                else if (arduinoMove == "Scissors")
                {
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
                    Responce.Text = arduinoMove;
                    Choise.Text = message;
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

        /**
         * @brief Обробник для початку нової гри
         * @param sender Об'єкт, що ініціював подію (кнопка)
         * @param e Параметри події
         * @details При натисканні на кнопку "One more game", приховуються елементи гри та відображаються кнопки для вибору складності.
         */
        private void OneMoreGame_Click(object sender, RoutedEventArgs e)
        {
            GameOver_Menu.Visibility = Visibility.Collapsed;
            MakeDifButtonsCollapsed();
        }

        /**
         * @brief Обробник для зміни рівня складності
         * @param sender Об'єкт, що ініціював подію (кнопка)
         * @param e Параметри події
         * @details При натисканні відкривається меню вибору складності.
         */
        private void ChangeDif_Click(object sender, RoutedEventArgs e)
        {
            GameOver_Menu.Visibility = Visibility.Collapsed;
            MakeDifButtonsVisible();
        }

        /**
         * @brief Приховує кнопки вибору складності та відображає кнопки для гри
         * @details Цей метод приховує кнопки вибору складності та відображає кнопки для вибору ходів (Rock, Paper, Scissors).
         */
        private void MakeDifButtonsCollapsed()
        {
            EasyButton.Visibility = Visibility.Collapsed;
            NormalButton.Visibility = Visibility.Collapsed;
            ImpossibleButton.Visibility = Visibility.Collapsed;
            GameName.Visibility = Visibility.Collapsed;
            CreateBy.Visibility = Visibility.Collapsed;
            ChoiseText.Visibility = Visibility.Visible;
            Rock_Button.Visibility = Visibility.Visible;
            Paper_Button.Visibility = Visibility.Visible;
            Scissors_Button.Visibility = Visibility.Visible;
        }

        /**
         * @brief Показує кнопки для вибору складності
         * @details Цей метод показує кнопки для вибору складності гри (Easy, Normal, Impossible).
         */
        private void MakeDifButtonsVisible()
        {
            GameName.Visibility = Visibility.Visible;
            CreateBy.Visibility = Visibility.Visible;
            EasyButton.Visibility = Visibility.Visible;
            NormalButton.Visibility = Visibility.Visible;
            ImpossibleButton.Visibility = Visibility.Visible;
            ChoiseText.Visibility = Visibility.Collapsed;
            Rock_Button.Visibility = Visibility.Collapsed;
            Paper_Button.Visibility = Visibility.Collapsed;
            Scissors_Button.Visibility = Visibility.Collapsed;
        }

        /**
         * @brief Закриває серійний порт при закритті вікна
         * @param sender Об'єкт, що ініціював подію (вікно)
         * @param e Параметри події
         * @details При закритті вікна, цей метод закриває серійний порт, якщо він відкритий.
         */
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (serialPort != null && serialPort.IsOpen)
            {
                serialPort.Close();
            }
        }
    }
}