# PORTIFÓLIO: PROPOSTA DE PROJETO PRÁTICO - BACKEND - Processo Seletivo 2024.1 LOADING JR

### A **Loading Jr** é a Empresa Júnior (sem fins lucrativos) de Desenvolvimento da UFC Campus Sobral e que em seu **processo seletivo 2024.1**, desafiou e avaliou a performance dos participantes através deste desafio que consistia em criar uma **API REST para gerenciamento de postagens**.

## Tecnologias utilizadas na aplicação:
1. .NET (C#)
2. Json Web Tokens (JWT)
3. Docker
4. Amazon S3 Bucket
5. Insomnia e Swagger

## Como Executar este projeto:
**Clone** este este repositório e dentro da pasta **PublicationsAPI** crie um arquivo com nome de **appsettings.json**, cole neste arquivo o conteúdo abaixo fazendo as devidas substituições:s

    {
    "ConnectionStrings": {
        "DefaultConnection": "<sua string de conexão*>",
        "LocalConnection":"<sua string de conexão*>"
    },
    "Logging": {
        "LogLevel": {
        "Default": "Information",
        "Microsoft.AspNetCore": "Warning"
        }
    },
    "AllowedHosts": "*",
    "AWS": {
        "Profile": "<o nome do perfil que você configurou no AWS CLI**>",
        "Region": "<região em que foi criado o bucket>",
        "AwsS3Bucket": "<nome do seu bucket S3>"
    },
    "JWT":{
        "Issuer": "http://localhost:5130",
        "Audience": "http://localhost:5130",
        "ExpireTimeInMinutes": 720,
        "SigningKey": "<sua chave signing key para a assinatura dos tokens JWT gerados pela aplicação>"
    }
    }

*: Confira em program.cs qual das strings de conexão está sendo lida e utilizada <br>
**: Crie um bucket e Instale e configure o AWS CLI em sua máquina antes de preencher as informações relacionadas à conexão do Bucket S3

## Features implentadas nessa aplicação:
* **Autenticação** via ASP.NET Core Identity
* **Autorização** via JWT
* **CRUD completo** de Usuários e Publicações
* Publicações e Usuários com suporte à **upload de imagens implementado com o AWS S3 Bucket**
* **Repository e Service Layer Patterns**
* **DTOs** para configuração de respostas, requisições assim como as validações
* Rotas sem autenticação para visualização (limitada) de conteúdos
* **Visualização paginada** de Usuários e Publicações
* Criação de uma **Imagem Docker** com essa aplicação para fácil deploy
Observação: Para mais detalhes sobre a aplicação, veja também o arquivo [`Proposta Projeto Final`](.Proposta Projeto Final - BACK_END - P.S 2024.1.pdf) da raiz do repositório.

## To-Do:
* Implementar Exception Handling (gerenciamento de erros).
* Adicionar sistema de monitoramento e logs da aplicação
* Escrever e executar testes unitários e de integração para a aplicação
