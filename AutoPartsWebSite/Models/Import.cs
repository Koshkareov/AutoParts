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
    [Table("Import")]
    public partial class Import
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



        [Display(Name = "Поставщик")]
        public string Supplier
        {
            get
            {
                SupplierModel db = new SupplierModel();
                Supplier supplier = db.Suppliers.Find(SupplierId);
                if (supplier == null)
                {
                    return "";
                }
                return supplier.Name.ToString();

            }
        }

        [Display(Name = "Номер поставщика")]
        public int? SupplierId { get; set; }
        public IEnumerable<SelectListItem> Suppliers { get; set; }

        [Display(Name = "Срок поставки")]
        [StringLength(10)]
        public string DeliveryTime
        {
            get
            {
                SupplierModel db = new SupplierModel();
                Supplier supplier = db.Suppliers.Find(SupplierId);
                if (supplier == null)
                {
                    return "";
                }
                return supplier.DeliveryTime.ToString();

            }
        }

        [Display(Name = "Имя файла")]
        public string FileName { get; set; }
    }
}
