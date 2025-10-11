terraform {
  required_version = ">= 1.6.0"

  backend "s3" {
    bucket = "tech-challenge-bucket-fiap"
    key    = "tech-challenge/terraform.tfstate"
    region = "us-east-1"
  }
}