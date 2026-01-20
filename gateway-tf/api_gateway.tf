# gateway-tf/api_gateway.tf

resource "aws_apigatewayv2_api" "main" {
  name          = "tech-challenge-api-gateway"
  protocol_type = "HTTP"
}

resource "aws_apigatewayv2_integration" "alb_integration" {
  api_id           = aws_apigatewayv2_api.main.id
  integration_type = "HTTP_PROXY"
  
# 🟢 CORRETO (Mude para isso):
  # Adicione o prefixo http:// e use o DNS do ALB
  integration_uri    = "http://${data.aws_lb.k8s_alb.dns_name}"
  integration_method = "ANY"
  connection_type    = "INTERNET" # Ou VPC_LINK se quiser privado
}

resource "aws_apigatewayv2_route" "default_route" {
  api_id    = aws_apigatewayv2_api.main.id
  route_key = "ANY /{proxy+}"
  target    = "integrations/${aws_apigatewayv2_integration.alb_integration.id}"
}

resource "aws_apigatewayv2_stage" "default" {
  api_id      = aws_apigatewayv2_api.main.id
  name        = "$default"
  auto_deploy = true
}