# Jogo de Cartas

Este é um projeto de um jogo de cartas desenvolvido em C# com .NET 8. O sistema permite criar baralhos, distribuir cartas para jogadores, embaralhar cartas, comparar cartas entre jogadores e finalizar o jogo.

## Funcionalidades

- **Criar Baralho**: Cria um novo baralho de cartas.
- **Distribuir Cartas**: Distribui cartas para um número especificado de jogadores.
- **Embaralhar Cartas**: Embaralha as cartas de um baralho existente.
- **Comparar Cartas**: Compara as cartas dos jogadores para determinar o vencedor.
- **Finalizar Jogo**: Finaliza o jogo e retorna o baralho.

## Estrutura do Projeto

- **Jogo_Cartas.Server**: Contém a lógica do servidor, incluindo controladores, modelos e serviços.
- **Jogo_Cartas.Client**: Contém a lógica do cliente, incluindo componentes e estilos.
- **Jogo_Cartas.Testes**: Contém os testes unitários para o projeto.

## Pré-requisitos

- .NET 8 SDK
- Visual Studio 2022
- IIS Express

## Configuração do Projeto

1. **Clone o repositório**:
git clone https://github.com/seu-usuario/jogo-de-cartas.git cd jogo-de-cartas


2. **Abra o projeto no Visual Studio 2022**:
   - Abra o Visual Studio 2022.
   - Selecione `File > Open > Project/Solution`.
   - Navegue até o diretório do projeto e selecione o arquivo `Jogo_Cartas.sln`.

3. **Configure o IIS Express**:
   - No Visual Studio, clique com o botão direito no projeto `Jogo_Cartas.Server` e selecione `Properties`.
   - Vá para a aba `Debug` e selecione `IIS Express` como o servidor de hospedagem.
   - Certifique-se de que a URL do aplicativo está configurada corretamente.

4. **Restaurar pacotes NuGet**:
   - No Visual Studio, abra o `Package Manager Console` (`Tools > NuGet Package Manager > Package Manager Console`).
   - Execute o comando:
    dotnet restore

## Executando o Projeto

1. **Inicie o servidor**:
   - No Visual Studio, selecione `Jogo_Cartas.Server` como o projeto de inicialização.
   - Pressione `F5` ou clique no botão `Start` para iniciar o servidor usando o IIS Express.

2. **Acesse o cliente**:
   - Abra um navegador e navegue até a URL do cliente (geralmente `http://localhost:5000`).

## Endpoints da API

- **Criar Baralho**: `GET /jogo/criar-baralho`
- **Distribuir Cartas**: `POST /jogo/distribuir-cartas?deckId={deckId}&numeroDeJogadores={numeroDeJogadores}`
- **Embaralhar Cartas**: `POST /jogo/embaralhar-cartas?deckId={deckId}`
- **Comparar Cartas**: `POST /jogo/comparar-cartas`
- **Finalizar Jogo**: `POST /jogo/finalizar-jogo?deckId={deckId}`

---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
# Testes Unitários

Os testes unitários são uma parte essencial do desenvolvimento de software, garantindo que cada unidade de código funcione conforme o esperado. No projeto "Jogo de Cartas", os testes unitários foram implementados para verificar a funcionalidade dos controladores, serviços e modelos.

## Estrutura dos Testes

Os testes unitários estão localizados no projeto `Jogo_Cartas.Testes`. Este projeto utiliza o framework de testes xUnit e a biblioteca Moq para criar mocks dos serviços.

## Testes Implementados

### CriarBaralho

- **CriarBaralho deve retornar OkResult com o Baralho criado**:
  - Verifica se o método `CriarBaralho` retorna um `OkResult` com o baralho criado.

### DistribuirCartas

- **DistribuirCartas deve retornar BadRequest se a API estiver fora do ar**:
  - Verifica se o método `DistribuirCartas` retorna um `BadRequest` se a API estiver fora do ar.
- **DistribuirCartas deve retornar OkResult com a lista de jogadores**:
  - Verifica se o método `DistribuirCartas` retorna um `OkResult` com a lista de jogadores.
- **DistribuirCartas deve retornar BadRequest se o número de jogadores for menor que 2**:
  - Verifica se o método `DistribuirCartas` retorna um `BadRequest` se o número de jogadores for menor que 2.
- **DistribuirCartas deve retornar BadRequest se o número de jogadores for maior que o máximo permitido**:
  - Verifica se o método `DistribuirCartas` retorna um `BadRequest` se o número de jogadores for maior que o máximo permitido.
- **DistribuirCartas deve retornar BadRequest se o baralho não for encontrado**:
  - Verifica se o método `DistribuirCartas` retorna um `BadRequest` se o baralho não for encontrado.
- **DistribuirCartas deve retornar BadRequest se o baralho não tiver 52 cartas**:
  - Verifica se o método `DistribuirCartas` retorna um `BadRequest` se o baralho não tiver 52 cartas.
- **DistribuirCartas deve retornar BadRequest se o baralho não estiver embaralhado**:
  - Verifica se o método `DistribuirCartas` retorna um `BadRequest` se o baralho não estiver embaralhado.

### CompararCartas

- **CompararCartas deve retornar OkResult com os vencedores**:
  - Verifica se o método `CompararCartas` retorna um `OkResult` com os vencedores.
- **CompararCartas deve retornar OkResult com empate**:
  - Verifica se o método `CompararCartas` retorna um `OkResult` com empate.
- **CompararCartas deve retornar BadRequest se a lista de jogadores for nula ou vazia**:
  - Verifica se o método `CompararCartas` retorna um `BadRequest` se a lista de jogadores for nula ou vazia.
- **CompararCartas deve retornar BadRequest se algum jogador tiver menos de 5 cartas**:
  - Verifica se o método `CompararCartas` retorna um `BadRequest` se algum jogador tiver menos de 5 cartas.
- **CompararCartas deve retornar BadRequest se algum campo da carta for inválido**:
  - Verifica se o método `CompararCartas` retorna um `BadRequest` se algum campo da carta for inválido.

### FinalizarJogo

- **FinalizarJogo deve retornar OkResult com o Baralho finalizado**:
  - Verifica se o método `FinalizarJogo` retorna um `OkResult` com o baralho finalizado.

## Executando os Testes

Para executar os testes unitários, siga os passos abaixo:

1. **Abra o Visual Studio 2022**.
2. **Abra o projeto `Jogo_Cartas.sln`**.
3. **Abra o `Test Explorer`**: Vá para `Test > Test Explorer`.
4. **Execute todos os testes**: Clique no botão `Run All` para executar todos os testes unitários.

Os testes unitários garantem que as funcionalidades principais do sistema estão funcionando corretamente e ajudam a identificar problemas antes que o código seja implantado em produção.


---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
