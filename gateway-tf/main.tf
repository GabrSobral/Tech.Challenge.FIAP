terraform {
  required_version = ">= 1.5.0"

  required_providers {
    aws = {
      source  = "hashicorp/aws"
      version = ">= 5.0"
    }
  }

  # --- ATENÇÃO MÁXIMA AQUI ---
  # Você deve usar o mesmo Bucket e DynamoDB criados no Repo 2 (Terraform State).
  # MAS, você DEVE mudar a 'key' (o caminho do arquivo).
  # Se você usar a mesma key do Repo 2, você vai CORROMPER sua infraestrutura.
  backend "s3" {
    bucket         = "tech-challenge-fiap-s3-bucket"
    key            = "app-gateway/terraform.tfstate" # <--- NOME NOVO (Pasta diferente)
    region         = "us-east-1"
    dynamodb_table = "tech-challenge-fiap-F-lock"
  }
}

provider "aws" {
  region = "us-east-1"

  # Boas práticas: Tags padrão para tudo que esse Terraform criar
  default_tags {
    tags = {
      Project     = "TechChallenge"
      Environment = "Production"
      ManagedBy   = "Terraform"
      Repository  = "Repo-4-App"
      Component   = "API-Gateway"
    }
  }
}