# gateway-tf/data.tf

# Busca o ALB filtrando pela Tag que definimos no YAML acima
data "aws_lb" "k8s_alb" {
  tags = {
    Project = "TechChallenge"
  }
}
# Busca o Listener (porta 80) desse ALB encontrado
data "aws_lb_listener" "k8s_listener" {
  load_balancer_arn = data.aws_lb.k8s_alb.arn
  # IMPORTANTE: Se esse LB for CLB/NLB (criado via Service), a porta pode ser 80 ou outra.
  # Se der erro aqui, confirme a porta no console da AWS.
  port              = 80 
}

# 1. BUSCAR O CLUSTER EXISTENTE NA AWS
# Isso diz ao Terraform: "Vá na AWS e leia os dados deste cluster"
data "aws_eks_cluster" "eks_cluster" {
  name = "tech-challenge-eks-cluster" # Certifique-se que este é o nome exato na AWS
}

# 2. BUSCAR O CERTIFICADO TLS (Baseado no cluster encontrado acima)
data "tls_certificate" "eks" {
  # Note que agora usamos "data." em vez de "aws_eks_cluster."
  url = data.aws_eks_cluster.eks_cluster.identity[0].oidc[0].issuer
}

# Buscando a Lambda existente (ajuste o nome da função)
data "aws_lambda_function" "tech_challenge_lambda" {
  function_name = var.auth_lambda_name
}