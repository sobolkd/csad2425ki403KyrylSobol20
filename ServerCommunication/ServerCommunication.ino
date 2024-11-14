// Код для Arduino Uno (сервер)
void setup() {
  // Встановлення швидкості UART (послідовного порту) на 9600 бод
  Serial.begin(9600);
}

void loop() {
  // Перевірка, чи є дані в послідовному порту
  if (Serial.available() > 0) {
    // Читання отриманого повідомлення
    String message = Serial.readString();

    // Перевірка, чи повідомлення не порожнє після обрізання пробілів
    message.trim();
    if (message.length() > 0) {
      // Модифікація повідомлення (наприклад, додаємо " - modified by server")
      message += " - modified by server";
      
      // Відправка модифікованого повідомлення назад до клієнта
      Serial.println(message);
    }
  }
}
