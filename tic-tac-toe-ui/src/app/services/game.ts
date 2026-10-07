import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

import { Game } from '../models/game';
import { Scoreboard } from '../models/scoreboard';

@Injectable({
  providedIn: 'root'
})
export class GameService {

  private readonly apiUrl = environment.apiUrl;

  constructor(
    private http: HttpClient
  ) {}

  createGame(
    mode: number
  ): Observable<Game> {

    return this.http.post<Game>(
      `${this.apiUrl}/games`,
      {
        mode
      }
    );
  }

  getGame(
    id: string
  ): Observable<Game> {

    return this.http.get<Game>(
      `${this.apiUrl}/games/${id}`
    );
  }

  makeMove(
    id: string,
    player: number,
    row: number,
    column: number
  ): Observable<Game> {

    return this.http.post<Game>(
      `${this.apiUrl}/games/${id}/moves`,
      {
        player,
        row,
        column
      }
    );
  }

  undo(
    id: string
  ): Observable<Game> {

    return this.http.post<Game>(
      `${this.apiUrl}/games/${id}/undo`,
      {}
    );
  }

  resetGame(
    id: string
  ): Observable<Game> {

    return this.http.post<Game>(
      `${this.apiUrl}/games/${id}/reset`,
      {}
    );
  }

  getScoreboard(): Observable<Scoreboard> {

    return this.http.get<Scoreboard>(
      `${this.apiUrl}/scoreboard`
    );
  }

  resetScoreboard(): Observable<Scoreboard> {

    return this.http.post<Scoreboard>(
      `${this.apiUrl}/scoreboard/reset`,
      {}
    );
  }
}