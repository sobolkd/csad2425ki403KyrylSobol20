enum Move { Rock, Paper, Scissors };

// Easy mode
Move getStupidMove(Move playerMove) {
  switch (playerMove) {
    case Rock: return Scissors; 
    case Paper: return Rock;
    case Scissors: return Paper;
    default: return getRandomMove();
  }
}

// Normal move
Move getRandomMove() {
  int randMove = random(0, 3);  // Випадковий вибір між 0, 1 і 2
  return static_cast<Move>(randMove);
}

// Impossible mode
Move getCounterMove(Move playerMove) {
  switch (playerMove) {
    case Rock: return Paper;
    case Paper: return Scissors;
    case Scissors: return Rock;
    default: return getRandomMove();
  }
}

// Convert move to string
String getMoveString(Move move) {
  switch (move) {
    case Rock: return "Rock";
    case Paper: return "Paper";
    case Scissors: return "Scissors";
    default: return "Unknown";
  }
}

void setup() {
  Serial.begin(9600); 
  randomSeed(analogRead(0));  // Ініціалізація генератора випадкових чисел
  delay(2000);  // Час для ініціалізації серійного з'єднання
}

void loop() {
  if (Serial.available()) {
    String message = Serial.readStringUntil('\n');
    message.trim();  // Очищаємо зайві пробіли

    int spaceIndex = message.indexOf(' ');
    if (spaceIndex != -1) {
      String playerMove = message.substring(0, spaceIndex);
      String difficulty = message.substring(spaceIndex + 1);
      
      Move player = getMoveFromString(playerMove);
      Move arduinoMove;

      if (difficulty == "Easy") {
        arduinoMove = getStupidMove(player);
      }
      else if (difficulty == "Normal") {
        arduinoMove = getRandomMove(); 
      }
      else if (difficulty == "Impossible") {
        arduinoMove = getCounterMove(player);
      } else {
        arduinoMove = getRandomMove(); // Якщо складність невідома, вибираємо випадковий хід
      }

      String arduinoMoveString = getMoveString(arduinoMove);
      Serial.println(arduinoMoveString);  // Відправляємо хід назад на ПК
    }
  }
}

// Перетворення тексту в тип Move
Move getMoveFromString(String move) {
  move.trim();  // Очищаємо зайві пробіли
  if (move == "Rock") return Rock;
  if (move == "Paper") return Paper;
  if (move == "Scissors") return Scissors;
  return Rock;  // За замовчуванням
}
