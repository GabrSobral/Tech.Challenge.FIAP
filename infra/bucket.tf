resource "aws_s3_bucket" "bucket-backend" {
  bucket = "tech-challenge-bucket-fiap"

  tags = {
    Name = "tech-challenge-bucket-fiap"
  }
}