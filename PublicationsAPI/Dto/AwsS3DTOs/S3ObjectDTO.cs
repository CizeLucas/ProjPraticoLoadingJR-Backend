namespace PublicationsAPI.DTO.AwsS3
{
    public class S3ObjectDTO
    {
        public string? Name { get; set; }
        public string? PresignedUrl { get; set;}
    }
}