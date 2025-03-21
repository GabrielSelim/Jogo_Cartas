import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { JogoService } from './jogo.service';

@Component({
  selector: 'app-jogo',
  templateUrl: './jogo.component.html',
  styleUrls: ['./jogo.component.css'],
  standalone: true,
  imports: [CommonModule, FormsModule]
})
export class JogoComponent {
  deckId: string | null = null;
  jogadores: any[] = [];
  numeroDeJogadores: number = 2;
  resultadoComparacao: any = null;
  backImage: string = 'assets/back.png'; // Caminho para a imagem back.png
  baralhoEmbaralhado: boolean = false;
  vencedores: any[] = [];

  constructor(private jogoService: JogoService) { }

  criarBaralho() {
    this.jogoService.criarBaralho().subscribe(response => {
      this.deckId = response.deck_id; // Acessando a propriedade correta
      this.jogadores = [];
      this.resultadoComparacao = null;
      this.baralhoEmbaralhado = false;
      this.vencedores = [];
    });
  }

  embaralharCartas() {
    if (this.deckId) {
      this.jogoService.embaralharCartas(this.deckId).subscribe(response => {
        this.baralhoEmbaralhado = response.shuffled;
        this.jogadores = []; // Limpar a tela
        this.resultadoComparacao = null; // Limpar o resultado da comparação
        this.vencedores = []; // Limpar os vencedores
      });
    }
  }

  distribuirCartas() {
    if (this.deckId && this.numeroDeJogadores >= 2 && this.numeroDeJogadores <= 10) {
      this.jogoService.distribuirCartas(this.deckId, this.numeroDeJogadores).subscribe(response => {
        this.jogadores = response;
      });
    }
  }

  compararCartas() {
    if (this.jogadores.length > 0) {
      this.jogoService.compararCartas(this.jogadores).subscribe(response => {
        this.resultadoComparacao = response.resultado;
        this.vencedores = response.vencedores;
        this.jogadores = []; // Limpar a tela
        this.baralhoEmbaralhado = false; // Desabilitar o botão "Distribuir Cartas"
      });
    }
  }

  finalizarJogo() {
    if (this.deckId) {
      this.jogoService.finalizarJogo(this.deckId).subscribe(response => {
        this.deckId = null;
        this.jogadores = [];
        this.resultadoComparacao = null;
        this.baralhoEmbaralhado = false;
        this.vencedores = [];
      });
    }
  }
}
