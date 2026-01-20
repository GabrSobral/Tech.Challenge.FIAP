variable "aws_region" {
  description = "Região da AWS"
  default     = "us-east-1"
}

variable "project_name" {
  description = "Nome do Projeto"
  default     = "tech-challenge"
}

variable "auth_lambda_name" {
  description = "Nome exato da função lambda criada no outro repositório"
  type        = string
  default     = "tech-challenge-dotnet-lambda"
}