# Tech Challenge - FIAP

## Objetivo

Este projeto é a primeira versão (MVP) do back-end de um Sistema Integrado de Atendimento e Execução de Serviços para uma oficina mecânica de médio porte. O sistema visa organizar e otimizar a gestão de ordens de serviço, clientes, veículos e peças, garantindo eficiência, qualidade e segurança no processo.

## Funcionalidades

- **Criação de Ordem de Serviço (OS)**  
  - Identificação do cliente por CPF/CNPJ  
  - Cadastro de veículo (placa, marca, modelo, ano)  
  - Inclusão de serviços solicitados e peças necessárias  
  - Orçamento automático e envio para aprovação do cliente  

- **Acompanhamento da OS**  
  - Controle de status: Recebida, Em diagnóstico, Aguardando aprovação, Em execução, Finalizada, Entregue  
  - Consulta do status da OS via API para clientes  

- **Gestão administrativa**  
  - CRUD de clientes, veículos, serviços, peças e insumos com controle de estoque  
  - Listagem e detalhamento das ordens de serviço  
  - Monitoramento do tempo médio de execução dos serviços  

- **Segurança e qualidade**  
  - Autenticação JWT para APIs administrativas  
  - Validação de dados sensíveis (CPF/CNPJ, placa de veículo)  
  - Testes unitários e de integração com cobertura mínima de 80% nos domínios críticos  

## Tecnologias e requisitos técnicos

- Back-end monolítico com arquitetura em camadas  
- Banco de dados livre (justificado no projeto)  
- APIs RESTful documentadas via Swagger  
- Containerização com Docker e orquestração com docker-compose  
- Configuração para execução local simples  

---

Este projeto faz parte de um desafio acadêmico que integra conhecimentos de várias disciplinas, com foco em Domain Driven Design (DDD), qualidade de software e segurança.

## Getting Started

Primeiramente clone o repositório, após isso basta rodar o seguinte comando na raiz do projeto

```bash
docker compose -f ./docker/docker-compose.yml up
```

Com isso, tanto o banco de dados PostgreSQL quanto a aplicação .NET 9 devem buildar e iniciar.

## Testes

Para rodar os testes, basta usar o seguinte comando, estando na raiz do repositório:

```bash
dotnet test .\tests\Tech.Challenge.Unit\
```

