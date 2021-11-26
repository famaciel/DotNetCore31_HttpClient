using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HttpClient.Api.Models
{
    public class AlbumModel
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int userId { get; set; }
        public int id { get; set; }
        [Required]
        public string title { get; set; }
    }
}
