# gateway-tf/data.tf

# Busca o ALB filtrando pela Tag que definimos no YAML acima
data "aws_lb" "k8s_alb" {
  # Filtramos pela tag exata que o Kubernetes colocou no seu Load Balancer
  tags = {
    "kubernetes.io/service-name" = "default/techchallenge-service"
  }
}

# Busca o Listener (porta 80) desse ALB encontrado
data "aws_lb_listener" "k8s_listener" {
  load_balancer_arn = data.aws_lb.k8s_alb.arn
  # IMPORTANTE: Se esse LB for CLB/NLB (criado via Service), a porta pode ser 80 ou outra.
  # Se der erro aqui, confirme a porta no console da AWS.
  port              = 80 
}