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

resource "aws_apigatewayv2_integration" "lambda_integration" {
  api_id           = aws_apigatewayv2_api.main.id
  integration_type = "AWS_PROXY"

  # O URI de integração para Lambda é o ARN de invocação
  integration_uri    = data.aws_lambda_function.tech_challenge_lambda.arn
  integration_method = "ANY" # Para Lambda, a integração interna é sempre POST
  
  # Versão 2.0 é recomendada para HTTP APIs (simplifica o payload JSON)
  payload_format_version = "2.0"
}

resource "aws_apigatewayv2_route" "lambda_route" {
  api_id    = aws_apigatewayv2_api.main.id
  
  # ⚠️ O SEGREDO ESTÁ AQUI:
  # Esta rota é mais específica que o "/{proxy+}" do ALB.
  # O Gateway vai preferir esta rota quando a URL bater.
  route_key = "ANY /lambda/{proxy+}" 
  
  target    = "integrations/${aws_apigatewayv2_integration.lambda_integration.id}"
}

resource "aws_apigatewayv2_route" "default_route" {
  api_id    = aws_apigatewayv2_api.main.id
  route_key = "ANY /api/{proxy+}"
  target    = "integrations/${aws_apigatewayv2_integration.alb_integration.id}"
}

resource "aws_apigatewayv2_stage" "default" {
  api_id      = aws_apigatewayv2_api.main.id
  name        = "$default"
  auto_deploy = true
}