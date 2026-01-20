# 1. Cria o Provedor OIDC no IAM (A "Ponte de Confiança")
resource "aws_iam_openid_connect_provider" "eks" {
  client_id_list  = ["sts.amazonaws.com"]
  thumbprint_list = [data.tls_certificate.eks.certificates[0].sha1_fingerprint]
  
  # Usando o link vindo do Data Source
  url = data.aws_eks_cluster.eks_cluster.identity[0].oidc[0].issuer
}