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
  numeroDeJogadoresValido: boolean = true;
  resultadoComparacao: any = null;
  backImage: string = 'assets/back.png'; // Caminho para a imagem back.png
  baralhoEmbaralhado: boolean = false;
  vencedores: any[] = [];
  mensagemErro: string | null = null;

  constructor(private jogoService: JogoService) { }

  criarBaralho() {
    this.jogoService.criarBaralho().subscribe({
      next: response => {
        this.deckId = response.deck_id; // Acessando a propriedade correta
        this.jogadores = [];
        this.resultadoComparacao = null;
        this.baralhoEmbaralhado = false;
        this.vencedores = [];
        this.mensagemErro = null;
      },
      error: err => {
        this.mensagemErro = err.error.mensagem;
      }
    });
  }

  embaralharCartas() {
    if (this.deckId) {
      this.jogoService.embaralharCartas(this.deckId).subscribe({
        next: response => {
          this.baralhoEmbaralhado = response.shuffled;
          this.jogadores = []; // Limpar a tela
          this.resultadoComparacao = null; // Limpar o resultado da comparação
          this.vencedores = []; // Limpar os vencedores
          this.mensagemErro = null;
        },
        error: err => {
          this.mensagemErro = err.error.mensagem;
        }
      });
    }
  }

  distribuirCartas() {
    if (this.deckId && this.numeroDeJogadoresValido) {
      this.jogoService.distribuirCartas(this.deckId, this.numeroDeJogadores).subscribe({
        next: response => {
          this.jogadores = response;
          this.mensagemErro = null;
        },
        error: err => {
          this.mensagemErro = err.error.mensagem;
        }
      });
    }
  }

  compararCartas() {
    if (this.jogadores.length > 0) {
      this.jogoService.compararCartas(this.jogadores).subscribe({
        next: response => {
          this.resultadoComparacao = response.resultado;
          this.vencedores = response.vencedores;
          this.jogadores = []; // Limpar a tela
          this.baralhoEmbaralhado = false; // Desabilitar o botão "Distribuir Cartas"
          this.mensagemErro = null;
        },
        error: err => {
          this.mensagemErro = err.error.mensagem;
        }
      });
    }
  }

  finalizarJogo() {
    if (this.deckId) {
      this.jogoService.finalizarJogo(this.deckId).subscribe({
        next: response => {
          this.deckId = null;
          this.jogadores = [];
          this.resultadoComparacao = null;
          this.baralhoEmbaralhado = false;
          this.vencedores = [];
          this.mensagemErro = null;
        },
        error: err => {
          this.mensagemErro = err.error.mensagem;
        }
      });
    }
  }

  validarNumeroDeJogadores() {
    this.numeroDeJogadoresValido = this.numeroDeJogadores >= 2 && this.numeroDeJogadores <= 10;
  }

  validarTecla(event: KeyboardEvent) {
    const input = event.target as HTMLInputElement;
    const value = parseInt(input.value + event.key, 10);
    if (isNaN(value) || value < 2 || value > 10) {
      event.preventDefault();
    }
  }
}

