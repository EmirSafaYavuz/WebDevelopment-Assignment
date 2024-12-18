namespace Assignment.Models;

public class Category : IEntity
{
    public int CategoryID { get; set; }
    public string CategoryName { get; set; }
    public string Description { get; set; }
    public string Picture { get; set; }
    public bool Active { get; set; } = true;

    public ICollection<Product> Products { get; set; }
}