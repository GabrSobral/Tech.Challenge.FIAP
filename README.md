- [Fase 1 - Bem-vindo a Arquitetura de Software](#fase-1---bem-vindo-a-arquitetura-de-software)
  - [Objetivo](#objetivo)
  - [Funcionalidades](#funcionalidades)
  - [Tecnologias e requisitos técnicos](#tecnologias-e-requisitos-técnicos)
  - [Getting Started](#getting-started)
  - [Testes](#testes)
- [Fase 2 - Gerenciamento de Kubernetes](#fase-2---gerenciamento-de-kubernetes)
  - [Objetivo](#objetivo-1)
  - [Arquitetura proposta](#arquitetura-proposta)
    - [Componentes da aplicação](#componentes-da-aplicação)
    - [Infraestrutura Provisionada na AWS](#infraestrutura-provisionada-na-aws)
    - [Fluxo de Deploy no CI/CD](#fluxo-de-deploy-no-cicd)
  - [Como rodar o Kubernetes](#como-rodar-o-kubernetes)
    - [Download e instalação](#download-e-instalação)
    - [Rodar o Kubernetes](#rodar-o-kubernetes)
    - [Tornando a aplicação acessível localmente](#tornando-a-aplicação-acessível-localmente)
  - [Terraform](#terraform)
    - [Recursos](#recursos)
    - [Como rodar](#como-rodar)

# Fase 1 - Bem-vindo a Arquitetura de Software

## Objetivo

Este projeto é a primeira versão (MVP) do back-end de um Sistema Integrado de Atendimento e Execução de Serviços para uma oficina mecânica de médio porte. O sistema visa organizar e otimizar a gestão de ordens de serviço, clientes, veículos e peças, garantindo eficiência, qualidade e segurança no processo.

## Videos de apresentação

[![Assista no YouTube](https://img.shields.io/badge/YouTube-FF0000?style=for-the-badge&logo=youtube&logoColor=white)](https://youtu.be/ZYQu4lvmfEY)

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


# Fase 2 - Gerenciamento de Kubernetes

## Vídeo de apresentação

[![Assista no YouTube](https://img.shields.io/badge/YouTube-FF0000?style=for-the-badge&logo=youtube&logoColor=white)](https://youtu.be/Y1XzZpgSIoQ)

## Objetivo

Evoluir a aplicação desenvolvida na Fase 1 para garantir qualidade, 
resiliência e escalabilidade, incorporando práticas modernas de infraestrutura e 
automação. 

## Arquitetura proposta

### Componentes da aplicação
A aplicação está seguindo a Clean Architecture, seguindo os componentes principais.

<image src="https://raw.githubusercontent.com/GabrSobral/Tech.Challenge.FIAP/refs/heads/master/.github/clean-architecture.png" alt="Diagrama da arquitetura limpa" height="250">

### Infraestrutura Provisionada na AWS
O Cloud Provider escolhido foi a AWS, subindo os recursos principais para o EKS e para o banco de Dados (RDS).

<image src="https://raw.githubusercontent.com/GabrSobral/Tech.Challenge.FIAP/refs/heads/master/.github/aws-architecture.png" alt="Diagrama da arquitetura limpa">

### Fluxo de Deploy no CI/CD
Os fluxos usados no Github Actions são os seguintes.

<image src="https://raw.githubusercontent.com/GabrSobral/Tech.Challenge.FIAP/refs/heads/master/.github/deploy-flow.png" alt="Diagrama da arquitetura limpa">

## Como rodar o Kubernetes:

Para executar localmente, primeiro você precisará de alguma engine para rodar o Kubernetes. Entre elas, existem algumas famosas como o Kind e o Minikube.

O tutorial contemplará as instruções para rodar com o Minikube, por conta de ter sido ele o usado para o desenvolvimento local da atividade.

### Download e instalação

Para baixar, basta entrar no site oficial do Minikube, e baixar o software. Baixe a versão que corresponda ao seu OS.

<a href="https://minikube.sigs.k8s.io/docs/start/?arch=%2Fwindows%2Fx86-64%2Fstable%2F.exe+download">Link para DOWNLOAD do Minikube</a>

### Rodar o Kubernetes

Para rodar na sua máquina, você deve estar na pasta ```/k8s-debug```. Essa pasta contém os seguintes manifestos:

- Deployment para a API e para o PostgresSQL
- Service para a API e para o PostgreSQL
- ConfigMap
- HPA (Horizontal Pod AutoScaler)
- Secret


Tem uma diferenciação entre a pasta ```/k8s``` porque essa pasta é usada no ambiente cloud, que contém arquivos e dados que no ambiente de produção não devem estar (como o arquivo de ```secret.yaml```) e as instruções para rodar o banco de dados (PostgreSQL).

No ambiente da AWS está sendo usado o RDS (Relational Database Service), então não é necessário o script de deploy do banco de dados pelo Kubernetes.

---

Após entrar na pasta ```/k8s-debug```, basta executar os comandos:

```bash
minikube start &&
minikube docker-env | Invoke-Expression &&
docker build  -t tech-challenge/tech-challenge:latest -f ..\src\Tech.Challenge\Dockerfile ..\src\ &&
kubectl apply -f .
```

`minikube start` inicia a instância do Minikube.

`minikube docker-env | Invoke-Expression` reconfigura o seu terminal PowerShell para que o seu cliente Docker local (docker) passe a se comunicar diretamente com o daemon Docker que está rodando dentro da máquina virtual do Minikube, em vez do seu Docker Desktop local.

`docker build  -t tech-challenge/tech-challenge:latest -f ..\src\Tech.Challenge\Dockerfile ..\src\` cria a imagem docker dentro do ambiente do Minikube, para poder ser usado pelo Kubernetes.

`kubectl apply -f .` aplica as configurações dos arquivos do Kubernetes, iniciando o cluster e os pods.

### Tornando a aplicação acessível localmente

Para tornar a aplicação acessível na sua máquina, basta usar o seguinte comando:

```bash
minikube service techchallenge-service
```

*Obs: tenha certeza de que o seu Kubernetes aponta para o contexto local, para ter certeza rode o comando `kubectl config current-context` e veja se o contexto está como Minikube,*
*Caso não esteja com o Minikube setado, use o comando `kubectl config use-context minikube` para setar o minikube manualmente como contexto.*

Após tornar a aplicação acessível, deve aparecer um IP com uma porta na no seu terminal. Basta usar essa URL para fazer as requisições locais.

---

## Terraform

O terraform é usado como `IaC` (*Infrastructure as Code*) para criar os recursos no provedor cloud.

O provedor usado nesse projeto está sendo a AWS (*Amazon Web Services*).

### Recursos

Os recursos criados pelo Terraform são os seguintes:

- VPC (Virtual Private Cloud)
- Subnets (pública e privada)
- Subnet Groups
- Security Groups
- Route Table (associadas às subnets públicas)
- Internet Gateway (para a VPC)
- Node do EKS
- Cluster do EKS
- ECR (Elastic Container Registry)
- RDS (Relational Database Service)
- Bucket S3
- EKS Access Entry (com as devidas políticas)

### Como rodar

Para rodar, primeiramente deve-se ter o Kubernetes instalado localmente.

<a href="https://developer.hashicorp.com/terraform/install">Link para DOWNLOAD do Terraform</a>

Após isso, tenha certeza de ter a CLI da AWS instalada também.

<a href="https://docs.aws.amazon.com/pt_br/cli/latest/userguide/getting-started-install.html">LInk para DOWNLOAD da AWS CLI</a>.

Após isso, use o seguinte comando para configurar a AWS CLI:

```bash
aws configure
```

Ele vai pedir alguns dados de acesso de um usuário do IAM com os devidos acessos para os recursos. Basta você informar os dados.

Logo em seguida, vamos iniciar o Terraform com os seguintes comandos:

```bash
terraform init
```

Com o terraform iniciado, use o comando `plan` para ser exibidos os recursos que vão ser criados.

```bash
terraform plan
```

Após confirmar tudo que deve ser criado, aplique os recursos com o comando `apply`:

```bash
terraform apply
```

Com tudo isso, os recursos vão ser criados na AWS corretamente.


