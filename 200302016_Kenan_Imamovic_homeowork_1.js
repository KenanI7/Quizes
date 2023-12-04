
// nodejs enviroment used, run by node quiz.js

const questions = require("./Quiz_Questions.json").questions;
const readline = require("readline");

const rl = readline.createInterface({
  input: process.stdin,
  output: process.stdout,
});

const player = {
  name: "",
  currentQuestionIndex: 0,
  correctAnswers: 0,
  incorrectAnswers: 0,
};

(function main() {
  rl.question('What is your name? (Type "exit" to exit) ', (answer) => {
    if (answer.toLowerCase() === "exit") {
      rl.close();
    } else {
      player.name = answer;
      console.log(`\nWelcome to my quiz, ${player.name}!\n`);
      game();
    }
  });
})();

function game() {
  if (player.currentQuestionIndex < questions.length) {
    askQuestion(player.currentQuestionIndex);
  } else {
    generateReport();
  }
}

function askQuestion(index) {
  const { question, choices } = questions[index];

  // Display question and choices
  console.log(`\n${question}\n`);
  choices.forEach((choice, i) => {
    console.log(`${i + 1}) ${choice}`);
  });

  rl.question(`\nEnter your choice (1-${choices.length}): `, (answer) => {
    if (!validateAnswer(answer, choices.length)) {
      console.log("Incorrect input!");
      askQuestion(index);
    } else {
      validateQuestion(questions[index], answer);
    }
  });
}

function validateAnswer(answer, numChoices) {
  return !isNaN(answer) && parseInt(answer) > 0 && parseInt(answer) <= numChoices;
}

function validateQuestion(question, userAnswer) {
  const { correctAnswer } = question;
  if (correctAnswer === userAnswer) {
    player.correctAnswers++;
    console.log("Correct!");
  } else {
    player.incorrectAnswers++;
    console.log("Incorrect!");
  }
  player.currentQuestionIndex++;
  game();
}

function generateReport() {
  const {
    name,
    correctAnswers,
    incorrectAnswers,
    currentQuestionIndex: totalQuestions,
  } = player;
  console.table({
    name,
    totalQuestions,
    correctAnswers,
    incorrectAnswers,
  });
  rl.close();
}
