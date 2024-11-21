#include <Arduino.h>
#include <ArduinoJson.h>
#include <AUnit.h>

enum Move { Rock, Paper, Scissors };

// Функції, які тестуються
Move getStupidMove(Move playerMove) {
  switch (playerMove) {
    case Rock: return Scissors;
    case Paper: return Rock;
    case Scissors: return Paper;
    default: return getRandomMove();
  }
}

Move getRandomMove() {
  int randMove = random(0, 3);
  return static_cast<Move>(randMove);
}

Move getCounterMove(Move playerMove) {
  switch (playerMove) {
    case Rock: return Paper;
    case Paper: return Scissors;
    case Scissors: return Rock;
    default: return getRandomMove();
  }
}

String getMoveString(Move move) {
  switch (move) {
    case Rock: return "Rock";
    case Paper: return "Paper";
    case Scissors: return "Scissors";
    default: return "Unknown";
  }
}

Move getMoveFromString(String move) {
  move.trim();
  if (move == "Rock") return Rock;
  if (move == "Paper") return Paper;
  if (move == "Scissors") return Scissors;
  return Rock;
}

void setup() {
  Serial.begin(9600);
  randomSeed(analogRead(0));
  delay(2000);

  Serial.println("Setup complete. Running tests...");
}

void loop() {
  // Запускаємо тести. AUnit сама обробляє результати.
  aunit::TestRunner::run();
}

// --- Тести ---
test(getStupidMove_Rock) {
  assertEqual(getStupidMove(Rock), Scissors);
}

test(getStupidMove_Paper) {
  assertEqual(getStupidMove(Paper), Rock);
}

test(getStupidMove_Scissors) {
  assertEqual(getStupidMove(Scissors), Paper);
}

test(getRandomMove) {
  for (int i = 0; i < 10; ++i) {
    Move move = getRandomMove();
    assertTrue(move == Rock || move == Paper || move == Scissors);
  }
}

test(getCounterMove_Rock) {
  assertEqual(getCounterMove(Rock), Paper);
}

test(getCounterMove_Paper) {
  assertEqual(getCounterMove(Paper), Scissors);
}

test(getCounterMove_Scissors) {
  assertEqual(getCounterMove(Scissors), Rock);
}

test(getMoveString_Rock) {
  assertEqual(getMoveString(Rock), "Rock");
}

test(getMoveString_Paper) {
  assertEqual(getMoveString(Paper), "Paper");
}

test(getMoveString_Scissors) {
  assertEqual(getMoveString(Scissors), "Scissors");
}

test(getMoveFromString_Rock) {
  assertEqual(getMoveFromString("Rock"), Rock);
}

test(getMoveFromString_Paper) {
  assertEqual(getMoveFromString("Paper"), Paper);
}

test(getMoveFromString_Scissors) {
  assertEqual(getMoveFromString("Scissors"), Scissors);
}
