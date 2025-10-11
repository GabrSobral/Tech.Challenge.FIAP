# 3. Cria um "DB Subnet Group", que informa ao RDS em quais subnets ele pode ser criado
resource "aws_db_subnet_group" "db_subnet_group" {
  name       = "tech-challenge-database-subnet-group"
  subnet_ids = aws_subnet.subnet_private[*].id

  tags = {
    Name = "Tech Challenge Database Subnet Group"
  }
}