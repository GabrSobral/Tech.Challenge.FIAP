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
# output "ecr_repository_url" {
#   value = aws_ecr_repository.app.repository_url
# }
# output "eks_cluster_name" {
#   value = aws_eks_cluster.this.name
# }
