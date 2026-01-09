# gateway-tf/data.tf

# 1. Busca o ALB criado pelo Kubernetes filtrando pelas Tags
data "aws_lb" "k8s_alb" {
  tags = {
    "ingress.k8s.aws/stack" = "tech-challenge-ingress" # O Controller geralmente coloca essa tag
    # OU filtre pelo nome se souber o padrão
  }
  depends_on = [
    # O Terraform precisa rodar DEPOIS que o ALB existir
  ]
}

# 2. Busca o Listener do ALB (geralmente porta 80)
data "aws_lb_listener" "k8s_listener" {
  load_balancer_arn = data.aws_lb.k8s_alb.arn
  port              = 80
}

# 3. Busca a VPC e Subnets (vindas do Repo 2 via Remote State ou Tags)
data "aws_vpc" "main" {
  tags = { Name = "tech-challenge-vpc" }
}