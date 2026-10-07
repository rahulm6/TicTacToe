import { Move } from './move';

export interface Game {
  id: string;
  mode: number;
  board: string[][];
  currentPlayer: number;
  status: number;
  winner: number | null;
  winningCells: number[];
  moveHistory: Move[];
}