resource "aws_security_group" "security_group" {
  name        = "${var.project_name}-sg"
  description = "Usado para expor o nginx"
  vpc_id      = aws_vpc.vpc_tech_challenge.id

  ingress {
    description = "Allow HTTP traffic"
    from_port   = 80
    to_port     = 80
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }

  egress {
    description = "Allow all outbound traffic"
    from_port   = 0
    to_port     = 0
    protocol    = "-1"
    cidr_blocks = ["0.0.0.0/0"]
  }
}

# resource "aws_db_subnet_group" "db_subnet_group" {
#   name       = "${var.project_name}-db-subnet-group"
#   subnet_ids = aws_subnet.subnet_private[*].id
# }