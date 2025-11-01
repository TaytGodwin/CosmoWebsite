namespace EC2Website.API.Data
{
    public class Picture
    {
        public int PictureId { get; set; }  // Matches picture_id SERIAL PRIMARY KEY

        public string S3Url { get; set; } = string.Empty;  // Matches s3_url TEXT

        public string PictureName { get; set; } = string.Empty;  // Matches picture_name TEXT

        public decimal? Price { get; set; }  // Matches price DECIMAL(10,2)
    }
}
