# Non-Transitive Dice Game

Welcome to the **Non-Transitive Dice Game**, a console-based game that brings the fascinating concept of non-transitive dice to life. This game combines strategic gameplay with provable fairness using cryptographic techniques.
**This project is a challenge task designed to test understanding concepts like HMAC-based randomness, secure cryptographic key generation, and modular arithmetic, along with designing interactive console applications.**

## 🎲 Game Concept

Non-transitive dice are a special set of dice where the relationships between them are non-transitive. For example:
- Dice A may beat Dice B,
- Dice B may beat Dice C,
- But Dice C may beat Dice A.

This project allows players to experience this phenomenon while ensuring fairness and transparency in every decision.

---

## 🛠 Features

- **Command-Line Input**: Accepts dice configurations as input, supporting 3 or more dice with six sides each.
- **Provable Fairness**: Utilizes HMAC to guarantee fairness during gameplay.
- **Interactive CLI Gameplay**:
  - Intuitive menus for dice selection and gameplay.
  - Options for help and exiting during any stage.
- **Probability Table**: Displays the chances of winning for each dice pair during the selection phase.
- **Error Handling**: Clear and concise error messages for invalid inputs.
- **Replayability**: Play multiple rounds without restarting the application.