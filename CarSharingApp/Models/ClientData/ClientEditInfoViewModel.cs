﻿using System.ComponentModel.DataAnnotations;

namespace CarSharingApp.Models.ClientData
{
    public class ClientEditInfoViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        [MaxLength(255, ErrorMessage = "Wrong First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        [MaxLength(255, ErrorMessage = "Wrong Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [MaxLength(255, ErrorMessage = "Username must contains less then 30 symbols")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Phone Number is required")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Valid Driver License Number is required")]
        [MaxLength(20, ErrorMessage = "Wrong Driver License Number")]
        public string DriverLicenseNumber { get; set; }

        [Required]
        public bool IsAcceptedNewsSharing { get; set; }

        // Описание в личном кабинете пользователя

        [Required(ErrorMessage = "Account description is required")]
        [MaxLength(60, ErrorMessage = "Acount description can't be more then 60 symbols length")]
        [MinLength(15, ErrorMessage = "Acount description can't be less then 15 symbols length")]
        public string AccountDescription { get; set; }

        [Required(ErrorMessage = "Image can't be empty")]
        public string UserImage { get; set; }
    }
}
