# Tic Tac Toe

A full-stack Tic Tac Toe application built using:

- Angular 22
- .NET 9 Web API
- REST APIs
- In Memory Storage
- xUnit

The backend is the source of truth and owns:

- Game state
- Turn validation
- Move history
- Computer AI
- Undo behavior
- Scoreboard

---

# Features

✅ Two Player Mode

✅ Computer Mode

✅ Move History

✅ Undo

✅ Reset Game

✅ Reset Scoreboard

✅ Win Detection

✅ Draw Detection

✅ Winning Cell Highlight

✅ Backend Driven State

✅ Unit Tests

---

# Tech Stack

Frontend:
- Angular 22
- TypeScript
- Signals
- Standalone Components

Backend:
- .NET Web API
- C#

Testing:
- xUnit
- FluentAssertions

Storage:
- In-Memory Repository


# API Summary

POST /api/games

Create new game

GET /api/games/{id}

Get current game state

POST /api/games/{id}/moves

Submit player move

POST /api/games/{id}/undo

Undo move

POST /api/games/{id}/reset

Reset current game

GET /api/scoreboard

Get scoreboard

POST /api/scoreboard/reset

Reset scoreboard


# Run Backend

cd TicTacToe.Api

dotnet restore

dotnet run

Swagger:
https://localhost:xxxx/swagger

# Run Frontend

cd tic-tac-toe-ui

npm install

ng serve


# Design Decisions

Backend owns all game state.

Frontend is a rendering layer.

Game state is stored in memory.

Computer AI uses a rule-based strategy:

1. Win if possible
2. Block opponent
3. Take center
4. Take corner
5. Take first available cell

Undo follows assignment requirements.

Option A was chosen:

Undo is disabled after game completion.


# Assumptions

Only one active browser session per game.

Data persistence after application restart is not required.

Scoreboard is maintained in memory.

Undo is disabled after Won or Draw state.


# AI Assisted Development

AI tools were used to:

- Break requirements into implementation steps
- Generate boilerplate code
- Create test cases
- Suggest Angular Signal patterns

All generated code was reviewed and modified manually where required.

# Known Limitations

State is not persisted after server restart.

Single-node in-memory storage only.

No authentication.

No multiplayer synchronization.

No minimax AI algorithm.


# Future Improvements

SQLite persistence.

SignalR real-time multiplayer.

Minimax AI.

Docker support.

Azure deployment.

Game replay feature.

User profiles.

Leaderboard support.