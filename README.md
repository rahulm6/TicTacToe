# Tic Tac Toe

## 1. Project Overview

This project is a full-stack Tic Tac Toe application built using Angular 22 and .NET Web API.

The application supports:

- Two Player Mode
- Computer Mode
- Move History Tracking
- Undo Functionality
- Scoreboard Tracking
- Win Detection
- Draw Detection
- Winning Cell Highlighting

The backend acts as the source of truth and owns all game state, validations, move history, AI decisions, and scoreboard management.

The frontend communicates with the backend through REST APIs and renders the current game state returned by the server.

---

## 2. Tech Stack

### Frontend

- Angular 22
- TypeScript
- Angular Signals
- Standalone Components
- HttpClient

### Backend

- .NET 10 Web API
- C#
- REST API Architecture

### Testing

- xUnit
- FluentAssertions

### Storage

- In-Memory Repository

---

## 3. Features Implemented

### Game Modes

✅ Two Player Mode

✅ Computer Mode

### Game Play

✅ 3x3 Tic Tac Toe Board

✅ Turn Validation

✅ Move Validation

✅ Current Player Tracking

✅ Win Detection

✅ Draw Detection

✅ Winning Cell Highlighting

### Move Tracking

✅ Move History

✅ Move Number Tracking

### Undo Support

✅ Undo for Two Player Mode

✅ Undo for Computer Mode

### Scoreboard

✅ Track X Wins

✅ Track O Wins

✅ Track Draws

✅ Reset Scoreboard

### Testing

✅ Unit Tests for Core Game Logic

---

## 4. How To Run The Backend Locally

Navigate to the backend project:

```bash
cd TicTacToe.Api
```

Restore NuGet packages:

```bash
dotnet restore
```

Run the application:

```bash
dotnet run
```

The API will start and display the application URL in the console.

Example:

```text
https://localhost:7074
```

API documentation can be accessed using:

```text
https://localhost:7074/swagger
```

---

## 5. How To Run The Frontend Locally

Navigate to the Angular application:

```bash
cd tic-tac-toe-ui
```

Install dependencies:

```bash
npm install
```

Run the application:

```bash
ng serve
```

Open:

```text
http://localhost:4200
```

---

## 6. API Endpoint Summary

### Create Game

```http
POST /api/games
```

Creates a new game.

Request:

```json
{
  "mode": 1
}
```

Mode Values:

```text
1 = Two Player
2 = Computer
```

---

### Get Game

```http
GET /api/games/{id}
```

Returns the current game state.

---

### Submit Move

```http
POST /api/games/{id}/moves
```

Request:

```json
{
  "player": 1,
  "row": 0,
  "column": 0
}
```

---

### Undo Move

```http
POST /api/games/{id}/undo
```

Undoes move(s) according to game mode.

---

### Reset Game

```http
POST /api/games/{id}/reset
```

Resets the board, move history and game status.

---

### Get Scoreboard

```http
GET /api/scoreboard
```

Returns current scoreboard values.

---

### Reset Scoreboard

```http
POST /api/scoreboard/reset
```

Resets all scoreboard counters.

---

## 7. How To Run Tests

Navigate to the solution root:

```bash
dotnet test
```

Expected result:

```text
Passed
Failed: 0
```

---

## Test Coverage

The following scenarios are covered through unit tests:

### Game Rules

✅ Valid Move

✅ Invalid Move

✅ Occupied Cell Validation

✅ Wrong Player Validation

### Turn Management

✅ Turn Switching

✅ Current Player Tracking

### Winner Detection

✅ Row Win

✅ Column Win

✅ Diagonal Win

✅ Winner Assignment

✅ Winning Cell Identification

### Draw Detection

✅ Draw State Detection

### Undo Functionality

✅ Undo In Two Player Mode

✅ Undo In Computer Mode

### Scoreboard

✅ X Win Counter

✅ O Win Counter

✅ Draw Counter

✅ Scoreboard Reset

### Computer AI

✅ Winning Move Selection

✅ Opponent Blocking

✅ Center Selection

✅ Corner Selection

---

## 8. AI Tools And Prompt Summary

AI assistance was used during development to:

- Analyze requirements
- Break requirements into implementation tasks
- Generate initial API scaffolding
- Generate DTO structures
- Generate unit test ideas
- Suggest Angular Signal patterns
- Assist with README creation

All generated code was manually reviewed, modified, tested, and validated before being included in the final solution.

---

## 9. Design Decisions

### Backend As Source Of Truth

The backend owns:

- Board State
- Current Player
- Game Status
- Winner Detection
- Move History
- Undo Logic
- AI Decisions
- Scoreboard State

The frontend is responsible only for rendering data and invoking APIs.

---

### Rule-Based Computer AI

The AI follows the exact priority order specified in the assignment:

1. Play winning move if available
2. Block opponent winning move
3. Take center
4. Take corner
5. Take first available position

---

### Separate Scoreboard Resource

The scoreboard is exposed through its own API endpoints:

```http
GET /api/scoreboard
POST /api/scoreboard/reset
```

This avoids duplicating scoreboard data inside every game response.

---

### Undo Strategy

Selected Option A:

```text
Undo is disabled after the game reaches Won or Draw state.
```

Reason:

- Keeps scoreboard results immutable
- Simplifies logic
- Avoids rollback complexity

---

### Thread Safety

The repository uses:

```csharp
ConcurrentDictionary<Guid, Game>
```

instead of:

```csharp
Dictionary<Guid, Game>
```

because the repository is registered as a Singleton and may be accessed concurrently by multiple requests.

---

## 10. Clarifications And Assumptions

### Storage

- In-memory storage is used as allowed by the assignment.
- Data persistence after application restart is not required.

### Game Sessions

- Every game has a unique Game ID.
- Multiple game sessions can coexist in memory.

### Computer Mode

- Human player is always X.
- Computer player is always O.

### Undo

- Undo is disabled after Won or Draw state.
- This behavior is a deliberate design choice.

### Scoreboard

- Scoreboard values are maintained in memory for the lifetime of the application.

---

## 11. Known Limitations

- Game state is lost after application restart.
- No persistent database storage.
- No authentication or authorization.
- No SignalR based multiplayer support.
- No real-time synchronization between clients.
- Computer opponent uses rule-based logic rather than Minimax.
- Intended for local execution only.
- Frontend unit tests were not implemented as the primary focus was validating business logic on the backend, where all game rules and state management reside.

---

## 12. Future Improvements

### Persistence

- SQLite
- SQL Server
- Entity Framework Core

### Multiplayer

- SignalR Integration
- Real-time Game Synchronization

### AI

- Minimax Algorithm
- Difficulty Levels

### Deployment

- Docker Support
- Azure App Service Deployment
- CI/CD Pipelines

### User Experience

- Game Replay
- Move Timeline Visualization
- Sound Effects
- Animations

### User Management

- Authentication
- User Profiles
- Persistent Statistics
- Leaderboards

---

## Architecture
 
```text
Angular Frontend
|
| REST APIs
|
.NET Web API
|
|
In-Memory Repository
```

## Screenshots

### Main Screen

<img width="1646" height="1080" alt="image" src="https://github.com/user-attachments/assets/e9369aa6-c03f-4205-874d-773433ab0ef7" />

### Two Player Mode

<img width="1646" height="1080" alt="image" src="https://github.com/user-attachments/assets/b08d3a82-7258-493b-9620-10fb00afcbf3" />

### Computer Mode

<img width="1641" height="1067" alt="image" src="https://github.com/user-attachments/assets/9fecc853-ac92-4dc9-8cbc-51a4c291a431" />

### Winner Highlight

<img width="1643" height="1026" alt="image" src="https://github.com/user-attachments/assets/0d5e1637-6fd1-4404-9b69-6d397794159b" />

### Move History And Scoreboard

<img width="1646" height="1026" alt="image" src="https://github.com/user-attachments/assets/826e2323-4b00-42c0-9a4a-5d5682c01f8b" />

---

## Reviewer Notes

This solution was intentionally designed with the backend acting as the source of truth.

All game rules, state transitions, undo behavior, move history tracking, AI decisions, winner detection, draw detection, and scoreboard management are implemented on the server.

The Angular frontend is a presentation layer responsible for rendering state returned by the REST APIs and providing user interaction.


## Submission Checklist

✅ Angular Frontend

✅ .NET Backend

✅ REST APIs

✅ Two Player Mode

✅ Computer Mode

✅ Move History

✅ Undo Functionality

✅ Winner Detection

✅ Draw Detection

✅ Winning Cell Highlighting

✅ Scoreboard

✅ Unit Tests

✅ README Documentation

✅ API Documentation

✅ AI Prompt Summary

✅ Design Decisions

✅ Assumptions And Limitations
