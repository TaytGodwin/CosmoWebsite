using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EC2Website.API.Data
{
    [Table("pictures")]                // table name in postgres
    public class Picture
    {
        [Key]
        [Column("picture_id")]         // primary key column
        public int PictureId { get; set; }

        [Column("picture_name")]
        public string PictureName { get; set; } = string.Empty;

        [Column("s3_url")]
        public string S3Url { get; set; } = string.Empty;

        [Column("price")]
        public decimal Price { get; set; }
    }
}
