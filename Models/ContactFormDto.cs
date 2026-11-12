using System.ComponentModel.DataAnnotations;

namespace MYAcademy_Portfolio.Models;

public class ContactFormDto
{
    [Required(ErrorMessage = "Ad soyad zorunludur.")]
    [StringLength(100)]
    public string NameSurname { get; set; }

    [Required(ErrorMessage = "E-posta zorunludur.")]
    [StringLength(100)]
    public string Email { get; set; }

    [Required(ErrorMessage = "Konu zorunludur.")]
    [StringLength(100)]
    public string Subject { get; set; }

    [Required(ErrorMessage = "Mesaj zorunludur.")]
    [StringLength(1000)]
    public string Message { get; set; }


}
