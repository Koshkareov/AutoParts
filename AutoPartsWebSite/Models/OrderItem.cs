namespace AutoPartsWebSite.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web.Mvc;
    [Table("OrderItem")]
    public partial class OrderItem
    {
    [Key]
    public int Id { get; set; }

    [ForeignKey("Order")]
    [Required]
    public int OrderId { get; set; }

    [Required]
    public int PartId { get; set; }

    [Required]
    [StringLength(128)]
    public string UserId { get; set; }

    [Display(Name = "Марка")]
    public string Brand { get; set; }

    [Display(Name = "Номер")]
    public string Number { get; set; }

    [Display(Name = "Название")]
    public string Name { get; set; }

    [Display(Name = "Описание")]
    public string Details { get; set; }

    [Display(Name = "Объем")]
    public string Size { get; set; }

    [Display(Name = "Вес")]
    public string Weight { get; set; }

    [Display(Name = "Наличие")]
    public string Quantity { get; set; }

    [Display(Name = "Цена")]
    public string Price { get; set; }

    [Display(Name = "Поставщик")]
    public string Supplier { get; set; }

    [Display(Name = "Срок поставки")]
    public string DeliveryTime { get; set; }

    [Display(Name = "Количество")]
    public int? Amount { get; set; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    [Display(Name = "Дата")]
    public DateTime? Data { get; set; }

    [Display(Name = "Статус")]
    public int State { get; set; }

    [Display(Name = "Стоимость")]
    public decimal? Total { get { return Amount * (Convert.ToDecimal(Price)); } }

    public virtual Order Order { get; set; }

    public List<SelectListItem> getOrderItemStates()
        {
            List<SelectListItem> StateItems = new List<SelectListItem>();
            StateItems.Add(new SelectListItem
            {
                Text = "В работе",
                Value = "1"
            });
            StateItems.Add(new SelectListItem
            {
                Text = "Закуплено",
                Value = "2",
                Selected = true
            });
            StateItems.Add(new SelectListItem
            {
                Text = "Снято",
                Value = "3"
            });
            StateItems.Add(new SelectListItem
            {
                Text = "Отправлено",
                Value = "4"
            });
            StateItems.Add(new SelectListItem
            {
                Text = "Готово к выдаче",
                Value = "5"
            });
            StateItems.Add(new SelectListItem
            {
                Text = "Выдано",
                Value = "6"
            });
            return StateItems;
        }
    }
}
