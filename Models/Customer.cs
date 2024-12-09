
using System.ComponentModel.DataAnnotations;

namespace Assignment.Models
{
    public class Customer
    {
        [Key] // Primary key
        public int CustomerID { get; set; }

        [Required(ErrorMessage = "Ad gerekli.")]
        [StringLength(100, ErrorMessage = "Ad 100 karakterden uzun olamaz.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Soyad gerekli.")]
        [StringLength(100, ErrorMessage = "Soyad 100 karakterden uzun olamaz.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email gerekli.")]
        [EmailAddress(ErrorMessage = "Geçerli bir email adresi girin.")]
        [StringLength(255, ErrorMessage = "Email 255 karakterden uzun olamaz.")]
        public string Email { get; set; }

        [Phone(ErrorMessage = "Geçerli bir telefon numarası girin.")]
        [StringLength(20, ErrorMessage = "Telefon numarası 20 karakterden uzun olamaz.")]
        public string Phone { get; set; }

        [StringLength(255, ErrorMessage = "Adres 1 255 karakterden uzun olamaz.")]
        public string Address1 { get; set; }

        [StringLength(255, ErrorMessage = "Adres 2 255 karakterden uzun olamaz.")]
        public string Address2 { get; set; }

        [StringLength(100, ErrorMessage = "Şehir 100 karakterden uzun olamaz.")]
        public string City { get; set; }

        [StringLength(100, ErrorMessage = "Eyalet 100 karakterden uzun olamaz.")]
        public string State { get; set; }

        [StringLength(20, ErrorMessage = "Posta kodu 20 karakterden uzun olamaz.")]
        public string PostalCode { get; set; }

        [StringLength(100, ErrorMessage = "Ülke 100 karakterden uzun olamaz.")]
        public string Country { get; set; }

        [Required(ErrorMessage = "Şifre gerekli.")]
        public string PasswordHash { get; set; }

        [Display(Name = "Kayıt Tarihi")]
        public DateTime RegistrationDate { get; set; } = DateTime.Now;

        public bool IsActive { get; set; } = true;

        public bool IsDeleted { get; set; } = false;
    }
}