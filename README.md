# Jogo de Cartas

Este � um projeto de um jogo de cartas desenvolvido em C# com .NET 8. O sistema permite criar baralhos, distribuir cartas para jogadores, embaralhar cartas, comparar cartas entre jogadores e finalizar o jogo.

## Funcionalidades

- **Criar Baralho**: Cria um novo baralho de cartas.
- **Distribuir Cartas**: Distribui cartas para um n�mero especificado de jogadores.
- **Embaralhar Cartas**: Embaralha as cartas de um baralho existente.
- **Comparar Cartas**: Compara as cartas dos jogadores para determinar o vencedor.
- **Finalizar Jogo**: Finaliza o jogo e retorna o baralho.

## Estrutura do Projeto

- **Jogo_Cartas.Server**: Cont�m a l�gica do servidor, incluindo controladores, modelos e servi�os.
- **Jogo_Cartas.Client**: Cont�m a l�gica do cliente, incluindo componentes e estilos.
- **Jogo_Cartas.Testes**: Cont�m os testes unit�rios para o projeto.

## Pr�-requisitos

- .NET 8 SDK
- Visual Studio 2022
- IIS Express

## Configura��o do Projeto

1. **Clone o reposit�rio**:
git clone https://github.com/seu-usuario/jogo-de-cartas.git cd jogo-de-cartas


2. **Abra o projeto no Visual Studio 2022**:
   - Abra o Visual Studio 2022.
   - Selecione `File > Open > Project/Solution`.
   - Navegue at� o diret�rio do projeto e selecione o arquivo `Jogo_Cartas.sln`.

3. **Configure o IIS Express**:
   - No Visual Studio, clique com o bot�o direito no projeto `Jogo_Cartas.Server` e selecione `Properties`.
   - V� para a aba `Debug` e selecione `IIS Express` como o servidor de hospedagem.
   - Certifique-se de que a URL do aplicativo est� configurada corretamente.

4. **Restaurar pacotes NuGet**:
   - No Visual Studio, abra o `Package Manager Console` (`Tools > NuGet Package Manager > Package Manager Console`).
   - Execute o comando:
    dotnet restore

## Executando o Projeto

1. **Inicie o servidor**:
   - No Visual Studio, selecione `Jogo_Cartas.Server` como o projeto de inicializa��o.
   - Pressione `F5` ou clique no bot�o `Start` para iniciar o servidor usando o IIS Express.

2. **Acesse o cliente**:
   - Abra um navegador e navegue at� a URL do cliente (geralmente `http://localhost:5000`).

## Endpoints da API

- **Criar Baralho**: `GET /jogo/criar-baralho`
- **Distribuir Cartas**: `POST /jogo/distribuir-cartas?deckId={deckId}&numeroDeJogadores={numeroDeJogadores}`
- **Embaralhar Cartas**: `POST /jogo/embaralhar-cartas?deckId={deckId}`
- **Comparar Cartas**: `POST /jogo/comparar-cartas`
- **Finalizar Jogo**: `POST /jogo/finalizar-jogo?deckId={deckId}`

---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
# Testes Unit�rios

Os testes unit�rios s�o uma parte essencial do desenvolvimento de software, garantindo que cada unidade de c�digo funcione conforme o esperado. No projeto "Jogo de Cartas", os testes unit�rios foram implementados para verificar a funcionalidade dos controladores, servi�os e modelos.

## Estrutura dos Testes

Os testes unit�rios est�o localizados no projeto `Jogo_Cartas.Testes`. Este projeto utiliza o framework de testes xUnit e a biblioteca Moq para criar mocks dos servi�os.

## Testes Implementados

### CriarBaralho

- **CriarBaralho deve retornar OkResult com o Baralho criado**:
  - Verifica se o m�todo `CriarBaralho` retorna um `OkResult` com o baralho criado.

### DistribuirCartas

- **DistribuirCartas deve retornar BadRequest se a API estiver fora do ar**:
  - Verifica se o m�todo `DistribuirCartas` retorna um `BadRequest` se a API estiver fora do ar.
- **DistribuirCartas deve retornar OkResult com a lista de jogadores**:
  - Verifica se o m�todo `DistribuirCartas` retorna um `OkResult` com a lista de jogadores.
- **DistribuirCartas deve retornar BadRequest se o n�mero de jogadores for menor que 2**:
  - Verifica se o m�todo `DistribuirCartas` retorna um `BadRequest` se o n�mero de jogadores for menor que 2.
- **DistribuirCartas deve retornar BadRequest se o n�mero de jogadores for maior que o m�ximo permitido**:
  - Verifica se o m�todo `DistribuirCartas` retorna um `BadRequest` se o n�mero de jogadores for maior que o m�ximo permitido.
- **DistribuirCartas deve retornar BadRequest se o baralho n�o for encontrado**:
  - Verifica se o m�todo `DistribuirCartas` retorna um `BadRequest` se o baralho n�o for encontrado.
- **DistribuirCartas deve retornar BadRequest se o baralho n�o tiver 52 cartas**:
  - Verifica se o m�todo `DistribuirCartas` retorna um `BadRequest` se o baralho n�o tiver 52 cartas.
- **DistribuirCartas deve retornar BadRequest se o baralho n�o estiver embaralhado**:
  - Verifica se o m�todo `DistribuirCartas` retorna um `BadRequest` se o baralho n�o estiver embaralhado.

### CompararCartas

- **CompararCartas deve retornar OkResult com os vencedores**:
  - Verifica se o m�todo `CompararCartas` retorna um `OkResult` com os vencedores.
- **CompararCartas deve retornar OkResult com empate**:
  - Verifica se o m�todo `CompararCartas` retorna um `OkResult` com empate.
- **CompararCartas deve retornar BadRequest se a lista de jogadores for nula ou vazia**:
  - Verifica se o m�todo `CompararCartas` retorna um `BadRequest` se a lista de jogadores for nula ou vazia.
- **CompararCartas deve retornar BadRequest se algum jogador tiver menos de 5 cartas**:
  - Verifica se o m�todo `CompararCartas` retorna um `BadRequest` se algum jogador tiver menos de 5 cartas.
- **CompararCartas deve retornar BadRequest se algum campo da carta for inv�lido**:
  - Verifica se o m�todo `CompararCartas` retorna um `BadRequest` se algum campo da carta for inv�lido.

### FinalizarJogo

- **FinalizarJogo deve retornar OkResult com o Baralho finalizado**:
  - Verifica se o m�todo `FinalizarJogo` retorna um `OkResult` com o baralho finalizado.

## Executando os Testes

Para executar os testes unit�rios, siga os passos abaixo:

1. **Abra o Visual Studio 2022**.
2. **Abra o projeto `Jogo_Cartas.sln`**.
3. **Abra o `Test Explorer`**: V� para `Test > Test Explorer`.
4. **Execute todos os testes**: Clique no bot�o `Run All` para executar todos os testes unit�rios.

Os testes unit�rios garantem que as funcionalidades principais do sistema est�o funcionando corretamente e ajudam a identificar problemas antes que o c�digo seja implantado em produ��o.


---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
