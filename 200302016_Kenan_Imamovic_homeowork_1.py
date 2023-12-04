import json

questions = json.loads(open("Quiz_Questions.json").read())["questions"]

player = {
    "name": "",
    "currentQuestionIndex": 0,
    "correctAnswers": 0,
    "incorrectAnswers": 0,
}

def main():
    global player
    player["name"] = input('What is your name? (Type "exit" to exit) ')
    
    if player["name"].lower() == "exit":
        return
    else:
        print(f'\nWelcome to my quiz, {player["name"]}!\n')
        game()

def game():
    global player
    if player["currentQuestionIndex"] < len(questions):
        ask_question(player["currentQuestionIndex"])
    else:
        generate_report()

def ask_question(index):
    global player
    question_data = questions[index]
    question = question_data["question"]
    choices = question_data["choices"]

    print(f'\n{question}\n')
    for i, choice in enumerate(choices):
        print(f"{i + 1}) {choice}")

    user_answer = input(f"\nEnter your choice (1-{len(choices)}): ")
    
    if not validate_answer(user_answer, len(choices)):
        print(f"Invalid input. Please enter a number between 1 and {len(choices)}.")
        ask_question(index)
    else:
        validate_question(question_data, user_answer)

def validate_answer(answer, num_choices):
    return answer.isdigit() and 1 <= int(answer) <= num_choices

def validate_question(question, user_answer):
    global player
    correct_answer = question["correctAnswer"]

    if correct_answer == user_answer:
        player["correctAnswers"] += 1
        print("Correct!")
    else:
        player["incorrectAnswers"] += 1
        print("Incorrect!")

    player["currentQuestionIndex"] += 1
    game()

def generate_report():
    global player
    print("\nQuiz Summary:")
    print(f"Name: {player['name']}")
    print(f"Total Questions: {len(questions)}")
    print(f"Correct Answers: {player['correctAnswers']}")
    print(f"Incorrect Answers: {player['incorrectAnswers']}")

if __name__ == "__main__":
    main()
