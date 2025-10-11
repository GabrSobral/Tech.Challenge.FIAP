resource "aws_internet_gateway" "igw_tech_challenge" {
  # Create an Internet Gateway for the VPC
  vpc_id = aws_vpc.vpc_tech_challenge.id

  tags = {
    Name = "${var.project_name}-igw"
  }
}

# Don't need this resource, as the IGW is already attached to the VPC when created with the "vpc_id" argument
# resource "aws_internet_gateway_attachment" "igw_attachment" {
#     internet_gateway_id = aws_internet_gateway.igw_tech_challenge.id
#     vpc_id             = aws_vpc.vpc_tech_challenge.id
# }