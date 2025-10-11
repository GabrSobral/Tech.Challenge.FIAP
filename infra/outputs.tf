# Outputs for VPC
output "vpc_id" {
  value = aws_vpc.vpc_tech_challenge.id
}
output "vpc_cidr" {
  value = aws_vpc.vpc_tech_challenge.cidr_block
}

# Outputs for Subnets
output "subnet_ids" {
  value = aws_subnet.subnet_public[*].id
}
output "subnet_cidrs" {
  value = aws_subnet.subnet_public[*].cidr_block
}


# # Outputs for EKS Cluster and ECR Repository
output "ecr_repository_url" {
  description = "A URL do repositório ECR"
  value       = aws_ecr_repository.app_repository.repository_url
}
output "eks_cluster_name" {
  value = aws_eks_cluster.eks_cluster.name
}

# Outputs for RDS Instance and Secrets Manager
output "db_instance_endpoint" {
  description = "The connection endpoint for the RDS instance."
  value       = aws_db_instance.postgres_db.endpoint
}
output "db_instance_port" {
  description = "The port for the RDS instance."
  value       = aws_db_instance.postgres_db.port
}
output "db_password_secret_arn" {
  description = "The ARN of the secret containing the database password."
  value       = aws_secretsmanager_secret.db_password.arn
}
output "rds_endpoint" {
  description = "O endpoint (endereço) da instância do banco de dados RDS."
  value       = aws_db_instance.postgres_db.address
}