using Catalog.Products.Events;

namespace Catalog.Products.Models;

internal class Product:Aggregate<Guid>
{
    internal string Name { get; private set; } = default!;
    internal List<string> Category { get; private set; } = [];
    internal string Description { get; private set; } = default!;
    internal string ImageFile { get; private set; } = default!;
    internal decimal Price { get; private set; }


    internal static Product Create(Guid id,string name,List<string> category,
    string description, string imageFile,decimal price)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price);

        var product = new Product
        {
            Name = name,
            Category = category,
            Description = description,
            ImageFile = imageFile,
            Price = price
        };
        product.AddDomainEvent(new ProductCreatedEvent(product));
        return product;
    }
    internal void Update(string name, List<string> category,
    string description, string imageFile, decimal price)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price);

        //Update product fields
        Name = name;
        Category = category;
        Description = description;
        ImageFile = imageFile;

        //Verify if product's price has changed
        if (Price != price)
        {
            Price = price;
            AddDomainEvent(new ProductPriceChangedEvent(this));
        }

    }
}
