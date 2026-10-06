# Tic Tac Toe

## Project Overview

This project implements a browser-based Tic Tac Toe application using Angular 22 and .NET Web API.

The application allows users to:

- Play Tic Tac Toe in Two Player Mode
- Play against a Computer Opponent
- Track Move History
- Undo Moves
- View and Reset Scoreboard
- Detect Wins and Draws
- Highlight Winning Cells

The backend acts as the source of truth and owns all game state, validation, move history, AI decisions, game status transitions, and scoreboard updates.

The frontend communicates with the backend exclusively through REST APIs and renders the latest game state returned by the server.

---

## Architecture

```text
+----------------------+
| Angular 22 Frontend |
+----------------------+
          |
          | REST API
          |
+----------------------+
| .NET Web API        |
+----------------------+
          |
          |
+----------------------+
| In-Memory Storage   |
+----------------------+

## Tech Stack

### Frontend

- Angular 22
- TypeScript
- Angular Signals
- Standalone Components
- HttpClient

### Backend

- .NET Web API
- C#
- REST API Architecture

### Testing

- xUnit
- FluentAssertions

### Storage

- In-Memory Repository

---

## Design Decisions

### Backend as Source of Truth

The backend owns all game state and business rules.

Responsibilities include:

- Move validation
- Turn management
- Winner detection
- Draw detection
- Undo processing
- Computer AI decisions
- Scoreboard updates

The frontend acts purely as a presentation layer and renders the latest state returned by the API.

---

### Separate Scoreboard Resource

The scoreboard is exposed through dedicated endpoints:

```http
GET /api/scoreboard
POST /api/scoreboard/reset
```

Reason:

- Keeps scoreboard management independent from game sessions.
- Avoids duplicating scoreboard data in every game response.
- Follows better separation of concerns.

---

### Rule-Based Computer AI

The AI follows the exact priority order specified in the requirements.

Priority order:

1. Play a winning move if available.
2. Block the opponent's winning move.
3. Take the center position.
4. Take an available corner.
5. Take the first available position.

This approach satisfies the assignment requirements while keeping the implementation simple and easy to explain during the review.

---

### Undo Strategy

The application implements the required undo behavior.

#### Two Player Mode

Undo removes only the most recent move.

Example:

```text
X -> O -> Undo
```

Result:

```text
O move removed
It becomes O's turn again
```

#### Computer Mode

Undo removes both:

- Computer's last move
- Human player's previous move

Example:

```text
X -> O (Computer) -> Undo
```

Result:

```text
Both moves removed
It becomes X's turn again
```

#### Completion Strategy

Option A was selected.

```text
Undo is disabled after the game is Won or Drawn.
```

Reason:

- Keeps scoreboard consistent.
- Prevents result rollback complexity.
- Produces simpler and more reliable behavior.

---

## API Summary

### Create Game

```http
POST /api/games
```

Creates a new game session.

Request:

```json
{
  "mode": 1
}
```

Response:

```json
{
  "id": "guid",
  "mode": 1,
  "board": [
    ["", "", ""],
    ["", "", ""],
    ["", "", ""]
  ]
}
```

---

### Get Game State

```http
GET /api/games/{id}
```

Returns the latest game state.

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

The backend validates:

- Correct player turn
- Valid board position
- Empty cell
- Game still in progress

---

### Undo Move

```http
POST /api/games/{id}/undo
```

Applies undo according to the selected game mode.

---

### Reset Game

```http
POST /api/games/{id}/reset
```

Resets:

- Board
- Move history
- Winner state
- Draw state
- Current player

Does NOT reset the scoreboard.

---

### Get Scoreboard

```http
GET /api/scoreboard
```

Response:

```json
{
  "xWins": 2,
  "oWins": 1,
  "draws": 3
}
```

---

### Reset Scoreboard

```http
POST /api/scoreboard/reset
```

Resets all scoreboard counters to zero.

## Run Backend

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

The API will start and the console will display the application URL.

Example:

```text
https://localhost:7074
```

Swagger documentation can be accessed using:

```text
https://localhost:7074/swagger
```

---

## Run Frontend

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

Launch the application:

```text
http://localhost:4200
```

---

## Run Tests

Navigate to the solution root directory:

```bash
dotnet test
```

All backend unit tests should pass successfully.

---

## Test Coverage

The solution includes backend unit tests covering core game functionality.

### Game Rules

✅ Valid Move

✅ Invalid Move

✅ Move On Occupied Cell

✅ Wrong Player Turn

✅ Move After Game Completion

---

### Turn Management

✅ Correct Turn Switching

✅ Current Player Tracking

---

### Winner Detection

✅ Row Win Detection

✅ Column Win Detection

✅ Diagonal Win Detection

✅ Winning Player Assignment

✅ Winning Cells Identification

---

### Draw Detection

✅ Full Board Draw Detection

✅ Draw Scoreboard Update

---

### Undo Functionality

✅ Undo In Two Player Mode

✅ Undo In Computer Mode

✅ Move History Restoration

✅ Turn Restoration

---

### Reset Functionality

✅ Reset Game

✅ Reset Scoreboard

---

### Computer AI

✅ Winning Move Selection

✅ Opponent Blocking

✅ Center Selection

✅ Corner Selection

✅ Available Cell Selection

---

## Backend State Ownership

The backend is implemented as the single source of truth.

The following state is managed exclusively by the backend:

- Board State
- Current Player
- Game Status
- Winner
- Winning Cells
- Move History
- Undo Logic
- Computer AI Decisions
- Scoreboard

The frontend never calculates game outcomes or modifies game rules.

All updates are performed through REST API calls.

---

## Clarifications And Assumptions

### Storage

- In-memory storage is used as permitted by the assignment.
- Data persistence across application restarts is not required.
- Scoreboard values are maintained for the lifetime of the running application.

---

### Game Sessions

- Each game is uniquely identified using a Game ID.
- Multiple game sessions can exist simultaneously in memory.
- Each game maintains its own independent board state and move history.

---

### Computer Mode

- Human player is always Player X.
- Computer player is always Player O.
- Computer moves are generated automatically after a valid human move.
- Computer moves follow the priority sequence specified in the assignment.

---

### Undo

Selected approach:

```text
Option A - Disable Undo After Completion
```

Undo is disabled after a game reaches:

- Won
- Draw

This ensures scoreboard consistency and avoids result rollback complexity.

---

## AI Prompt Summary

AI assistance was used during development for:

- Requirement analysis
- Architecture planning
- API design suggestions
- DTO generation
- Unit test generation
- Angular component scaffolding
- Angular Signal recommendations
- README generation assistance

Generated content was manually reviewed, modified, tested and validated before inclusion in the final solution.

---

## Known Limitations

### Storage

- Game state is stored in memory.
- All state is lost when the application stops.

---

### Multiplayer

- No real-time multiplayer synchronization.
- No SignalR implementation.

---

### Security

- No authentication.
- No authorization.

---

### AI

- Uses rule-based decision making.
- Does not implement the Minimax algorithm.

---

### Deployment

- Intended for local execution.
- No containerization configuration included.

---

## Future Improvements

Potential future enhancements include:

### Persistence

- SQLite
- SQL Server
- Entity Framework Core

---

### Multiplayer

- SignalR Integration
- Real-time game synchronization

---

### Computer AI

- Minimax Algorithm
- Difficulty Levels
- Advanced Strategy Evaluation

---

### Deployment

- Docker Support
- Azure App Service Deployment
- CI/CD Pipelines

---

### User Experience

- Game Replay
- Move Timeline Visualization
- Sound Effects
- Animations

---

### User Management

- Authentication
- User Profiles
- Persistent Statistics
- Leaderboards

---

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

## Submission Checklist

✅ Angular Frontend

✅ .NET Backend

✅ REST APIs

✅ Two Player Mode

✅ Computer Mode

✅ Move History

✅ Undo Functionality

✅ Win Detection

✅ Draw Detection

✅ Highlight Winning Cells

✅ Scoreboard

✅ Reset Game

✅ Reset Scoreboard

✅ Unit Tests

✅ README Documentation

✅ API Summary

✅ AI Prompt Summary

✅ Design Decisions

✅ Assumptions And Limitations
