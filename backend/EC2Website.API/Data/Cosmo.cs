namespace EC2Website.API.Data
{
    public class Cosmo
    {
        public int Id { get; set; } // Primary key (EF Core will recognize this automatically)

        public string PictureName { get; set; } = string.Empty;

        // This will store the binary image data
        public byte[] Picture { get; set; } = Array.Empty<byte>();
    }
}
