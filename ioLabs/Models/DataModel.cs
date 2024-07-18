using System.ComponentModel.DataAnnotations;

namespace ioLabs.Models
{
    public class DataModel
    {
        [Key]
        public int Id { get; set; }

        public DateTime RequestTime { get; set; }

        public string Request { get; set; }

        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }

        public string User { get; set; }
    }
}
