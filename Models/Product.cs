namespace Assignment.Models;

public class Product : IEntity
{
    public int ProductID { get; set; }
    public string SKU { get; set; }
    public string IDSKU { get; set; }
    public string VendorProductID { get; set; }
    public string ProductName { get; set; }
    public string ProductDescription { get; set; }
    public int? SupplierID { get; set; }
    public int? CategoryID { get; set; }
    public string QuantityPerUnit { get; set; }
    public decimal? UnitPrice { get; set; }
    public decimal? MSRP { get; set; }
    public string AvailableSize { get; set; }
    public string AvailableColors { get; set; }
    public string Size { get; set; }
    public string Color { get; set; }
    public decimal? Discount { get; set; }
    public decimal? UnitWeight { get; set; }
    public int? UnitsInStock { get; set; }
    public int? UnitsOnOrder { get; set; }
    public int? ReorderLevel { get; set; }
    public bool ProductAvailable { get; set; } = true;
    public bool DiscountAvailable { get; set; } = false;
    public int? CurrentOrder { get; set; }
    public string Picture { get; set; }
    public int? Ranking { get; set; }
    public string Note { get; set; }

    public Category Category { get; set; }
}