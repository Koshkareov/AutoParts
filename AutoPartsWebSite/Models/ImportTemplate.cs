namespace AutoPartsWebSite.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    using IdentityAutoPart.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.Owin;
    using System.Web;
    using System.Web.Mvc;

    [Table("ImportTemplate")]
    public partial class ImportTemplate
    {
        [Key]
        [Display(Name = "Номер")]
        public int Id { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Дата")]
        public DateTime? Date { get; set; }

        [StringLength(128)]
        public string UserId { get; set; }
        [Display(Name = "Пользователь")]
        [StringLength(128)]
        public string UserName
        {
            get
            {
                ApplicationUserManager userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var user = userManager.FindById(UserId);
                if (user != null)
                {
                    return user.FullName;
                }
                else
                {
                    return "";
                }
            }
        }

        [Required]
        [StringLength(70)]
        [Display(Name = "Название")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Начальная строка")]
        public int StartRow { get; set; }

        [Required]
        [StringLength(10)]
        [Display(Name = "Столбец-Марка")]
        public string BrandColumn { get; set; }

        [Required]
        [StringLength(10)]
        [Display(Name = "Столбец-Номер")]
        public string NumberColumn { get; set; }

        [Required]
        [StringLength(10)]
        [Display(Name = "Столбец-Название")]
        public string NameColumn { get; set; }

        [Required]
        [StringLength(10)]
        [Display(Name = "Столбец-Описание")]
        public string DetailsColumn { get; set; }

        [Required]
        [StringLength(10)]
        [Display(Name = "Столбец-Объем")]
        public string SizeColumn { get; set; }

        [Required]
        [StringLength(10)]
        [Display(Name = "Столбец-Вес")]
        public string WeightColumn { get; set; }

        [Required]
        [StringLength(10)]
        [Display(Name = "Столбец-Количество")]
        public string QuantityColumn { get; set; }

        [Required]
        [StringLength(10)]
        [Display(Name = "Столбец-Цена")]
        public string PriceColumn { get; set; }
    }
}
