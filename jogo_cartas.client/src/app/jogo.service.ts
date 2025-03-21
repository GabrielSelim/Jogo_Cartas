import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class JogoService {
  private apiUrl = this.getApiUrl();

  constructor(private http: HttpClient) { }

  private getApiUrl(): string {
    return window.location.hostname === 'localhost' ? 'https://localhost:44355' : 'https://api-seu-dominio.com';
  }

  criarBaralho(): Observable<any> {
    return this.http.get(`${this.apiUrl}/jogo/criar-baralho`, {});
  }

  embaralharCartas(deckId: string): Observable<any> {
    return this.http.post(`${this.apiUrl}/jogo/embaralhar-cartas?deckId=${deckId}`, {});
  }

  distribuirCartas(deckId: string, numeroDeJogadores: number): Observable<any> {
    return this.http.post(`${this.apiUrl}/jogo/distribuir-cartas?deckId=${deckId}&numeroDeJogadores=${numeroDeJogadores}`, {});
  }

  compararCartas(jogadores: any[]): Observable<any> {
    return this.http.post(`${this.apiUrl}/jogo/comparar-cartas`, jogadores);
  }

  finalizarJogo(deckId: string): Observable<any> {
    return this.http.post(`${this.apiUrl}/jogo/finalizar-jogo?deckId=${deckId}`, {});
  }
}
