# gateway-tf/data.tf

# Busca o ALB filtrando pela Tag que definimos no YAML acima
data "aws_lb" "k8s_alb" {
  tags = {
    Project = "TechChallenge"
  }
  
  # O Terraform vai falhar se não achar nenhum ALB com essa tag.
  # Isso é bom, pois garante que ele só roda se o Ingress funcionou.
}

# Busca o Listener (porta 80) desse ALB encontrado
data "aws_lb_listener" "k8s_listener" {
  load_balancer_arn = data.aws_lb.k8s_alb.arn
  port              = 80
}