import {
  ChangeDetectionStrategy,
  Component,
  OnInit,
  computed,
  signal
} from '@angular/core';

import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { Game } from '../../models/game';
import { Scoreboard } from '../../models/scoreboard';
import { Player } from '../../models/player';
import { GameService } from '../../services/game';

@Component({
  selector: 'app-game',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule
  ],
  templateUrl: './game.html',
  styleUrl: './game.css',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class GameComponent implements OnInit {

  game = signal<Game | null>(null);

  scoreboard = signal<Scoreboard | null>(null);

  errorMessage = signal('');

  selectedMode = signal(1);

  constructor(
    private gameService: GameService
  ) {}

  currentPlayerText = computed(() => {

    const currentGame = this.game();

    if (!currentGame) {
      return '';
    }

    return currentGame.currentPlayer === Player.X
      ? 'X'
      : 'O';
  });

  winnerText = computed(() => {

    const currentGame = this.game();

    if (!currentGame || currentGame.winner === null) {
      return '';
    }

    return currentGame.winner === Player.X
      ? 'X'
      : 'O';
  });

  ngOnInit(): void {
    this.loadScoreboard();
    this.createGame();
  }

  createGame(): void {

    this.errorMessage.set('');

    this.gameService
      .createGame(this.selectedMode())
      .subscribe({
        next: game => {
          this.game.set(game);
        },
        error: error => {
          this.errorMessage.set(
            error?.error?.error ?? 'Failed to create game'
          );
        }
      });
  }

  makeMove(
    row: number,
    column: number
  ): void {

    const currentGame = this.game();

    if (!currentGame) {
      return;
    }

    if (
      currentGame.status !== 1 ||
      currentGame.board[row][column]
    ) {
      return;
    }

    this.errorMessage.set('');

    this.gameService
      .makeMove(
        currentGame.id,
        currentGame.currentPlayer,
        row,
        column
      )
      .subscribe({
        next: (game) => {

          this.game.set(game);

          this.loadScoreboard();
        },
        error: (error) => {
          this.errorMessage.set(
            error?.error?.error ?? 'Move failed'
          );
        }
      });
  }

  undo(): void {

    const currentGame = this.game();

    if (!currentGame) {
      return;
    }

    this.errorMessage.set('');

    this.gameService
      .undo(currentGame.id)
      .subscribe({
        next: (game:Game) => {
          this.game.set(game);
        },
        error: (error:any) => {
          this.errorMessage.set(
            error?.error?.error ?? 'Undo failed'
          );
        }
      });
  }

  resetGame(): void {

    const currentGame = this.game();

    if (!currentGame) {
      return;
    }

    this.errorMessage.set('');

    this.gameService
      .resetGame(currentGame.id)
      .subscribe({
        next: (game:Game)  => {
          this.game.set(game);
        },
        error: (error:any) => {
          this.errorMessage.set(
            error?.error?.error ?? 'Reset failed'
          );
        }
      });
  }

  resetScoreboard(): void {

    this.errorMessage.set('');

    this.gameService
      .resetScoreboard()
      .subscribe({
        next: (scoreboard:Scoreboard) => {
          this.scoreboard.set(scoreboard);
        },
        error: (error:any) => {
          this.errorMessage.set(
            error?.error?.error ?? 'Reset scoreboard failed'
          );
        }
      });
  }

loadScoreboard(): void {

  this.gameService
    .getScoreboard()
    .subscribe({
      next: (scoreboard:Scoreboard) => {
        this.scoreboard.set(scoreboard);
      },
      error: (error:any) => {
        this.errorMessage.set(
          error?.error?.error ?? 'Failed to load scoreboard'
        );
      }
    });
}

onModeChange(event: Event): void {

  const value = (event.target as HTMLSelectElement).value;

  this.selectedMode.set(Number(value));
}

isWinningCell(
  row: number,
  column: number
): boolean {

  const currentGame = this.game();

  if (!currentGame) {
    return false;
  }

  const cellIndex = row * 3 + column;

  return currentGame.winningCells.includes(cellIndex);
}

getPlayerName(player: number): string {

  switch (player) {

    case Player.X:
      return 'X';

    case Player.O:
      return 'O';

    default:
      return '';
  }
}
}