using System.ComponentModel.DataAnnotations;

namespace Umbraco.UIBuilder.Demo.Web.Dtos
{
    public class SubmitCommentDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Body { get; set; }
    }
}
