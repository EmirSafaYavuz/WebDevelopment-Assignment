using System.ComponentModel.DataAnnotations;

namespace Assignment.Models
{
    public class User
    {
        [Required(ErrorMessage = "Ad ve soyad gerekli.")]
        [Display(Name = "Ad Soyad")] // Görünür adı ayarlar
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email adresi gerekli.")]
        [EmailAddress(ErrorMessage = "Geçerli bir email adresi girin.")]
        [Display(Name = "E-posta Adresi")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifre gerekli.")]
        [MinLength(6, ErrorMessage = "Şifre en az 6 karakter olmalıdır.")]
        [Display(Name = "Şifre")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Şifre tekrarı gerekli.")]
        [Compare("Password", ErrorMessage = "Şifreler eşleşmiyor.")]
        [Display(Name = "Şifre Tekrar")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Cinsiyet seçimi zorunludur.")]
        [Display(Name = "Cinsiyet")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Doğum tarihi zorunludur.")]
        [Display(Name = "Doğum Tarihi")]
        public DateTime? BirthDate { get; set; }

        [Required(ErrorMessage = "Kullanım şartlarını kabul etmelisiniz.")]
        [Display(Name = "Kullanım Şartları")]
        public bool AcceptTerms { get; set; }
    }
}