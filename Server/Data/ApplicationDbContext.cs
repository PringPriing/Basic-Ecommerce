using Ecommerce.Server.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Server.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<ProductCategory> ProductCategories => Set<ProductCategory>();
    public DbSet<CartItem> CartItems => Set<CartItem>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ProductCategory>()
            .HasKey(pc => new { pc.ProductId, pc.CategoryId });

        builder.Entity<ProductCategory>()
            .HasOne(pc => pc.Product)
            .WithMany(p => p.ProductCategories)
            .HasForeignKey(pc => pc.ProductId);

        builder.Entity<ProductCategory>()
            .HasOne(pc => pc.Category)
            .WithMany(c => c.ProductCategories)
            .HasForeignKey(pc => pc.CategoryId);

        builder.Entity<Product>()
            .Property(p => p.Price)
            .HasPrecision(18, 2);

        builder.Entity<CartItem>()
            .HasOne(ci => ci.User)
            .WithMany(u => u.CartItems)
            .HasForeignKey(ci => ci.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<CartItem>()
            .HasOne(ci => ci.Product)
            .WithMany(p => p.CartItems)
            .HasForeignKey(ci => ci.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Order>()
            .HasOne(o => o.User).WithMany(u => u.Orders)
            .HasForeignKey(o => o.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.Entity<Order>()
            .Property(o => o.TotalAmount).HasPrecision(18, 2);
        builder.Entity<Order>()
            .HasIndex(o => o.StripeSessionId).IsUnique();

        builder.Entity<OrderItem>()
            .HasOne(oi => oi.Order).WithMany(o => o.Items)
            .HasForeignKey(oi => oi.OrderId).OnDelete(DeleteBehavior.Cascade);
        builder.Entity<OrderItem>()
            .HasOne(oi => oi.Product).WithMany(p => p.OrderItems)
            .HasForeignKey(oi => oi.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.Entity<OrderItem>()
            .Property(oi => oi.UnitPrice).HasPrecision(18, 2);

        SeedData(builder);
    }

    private static void SeedData(ModelBuilder builder)
    {
        var seedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        // Categories
        builder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Electronics", Description = "Gadgets and devices", IsActive = true },
            new Category { Id = 2, Name = "Clothing", Description = "Apparel and accessories", IsActive = true },
            new Category { Id = 3, Name = "Books", Description = "Physical and digital books", IsActive = true },
            new Category { Id = 4, Name = "Home & Garden", Description = "Furniture, tools and decor", IsActive = true },
            new Category { Id = 5, Name = "Sports", Description = "Sporting goods and outdoor gear", IsActive = true }
        );

        // Products
        builder.Entity<Product>().HasData(
            // Electronics
            new Product { Id = 1, Name = "Wireless Noise-Cancelling Headphones", Description = "Premium over-ear headphones with active noise cancellation, 30-hour battery life, and foldable design. Compatible with all Bluetooth devices.", Price = 149.99m, StockQuantity = 80, IsActive = true, CreatedAt = seedDate, ImageUrl = "https://storagecommerceprince.blob.core.windows.net/product-images/3b6b1698-2ada-4c0c-9de9-4b6b406655da.png?sv=2025-05-05&se=2036-05-11T11%3A18%3A03Z&sr=b&sp=r&sig=Lk8LNPH9tSUoCMLhPDQ6ncvKgKlrBu0dUr65er5s%2FrY%3D", ImageBlobName = "3b6b1698-2ada-4c0c-9de9-4b6b406655da.png" },
            new Product { Id = 2, Name = "4K Smart TV 55\"", Description = "Ultra HD 4K smart television with built-in streaming apps, HDR support, and voice remote. Slim bezels and vibrant QLED display.", Price = 699.99m, StockQuantity = 25, IsActive = true, CreatedAt = seedDate, ImageUrl = "https://storagecommerceprince.blob.core.windows.net/product-images/fd4142c9-c21f-466d-91f8-35254adce270.jpg?sv=2025-05-05&se=2036-05-11T10%3A33%3A23Z&sr=b&sp=r&sig=vWlgAJU8lxWRWN7%2Bmx0oM1jQowVtQrgSgRtut9A5NgQ%3D", ImageBlobName = "fd4142c9-c21f-466d-91f8-35254adce270.jpg" },
            new Product { Id = 3, Name = "Mechanical Gaming Keyboard", Description = "Full-size mechanical keyboard with RGB backlighting, tactile switches, and anti-ghosting. Detachable USB-C cable included.", Price = 89.99m, StockQuantity = 120, IsActive = true, CreatedAt = seedDate, ImageUrl = "https://storagecommerceprince.blob.core.windows.net/product-images/96f04441-a3f0-4301-bfd4-4777bac1586c.jpeg?sv=2025-05-05&se=2036-05-11T11%3A15%3A12Z&sr=b&sp=r&sig=UxN2Ug9IqksugoJw13bhcKS%2F2KK3zyx1NE7WA5cs%2F7Q%3D", ImageBlobName = "96f04441-a3f0-4301-bfd4-4777bac1586c.jpeg" },
            new Product { Id = 4, Name = "Portable Bluetooth Speaker", Description = "Waterproof (IPX7) compact speaker with 360-degree surround sound and 20-hour battery life. Perfect for outdoor adventures.", Price = 59.99m, StockQuantity = 200, IsActive = true, CreatedAt = seedDate, ImageUrl = "https://storagecommerceprince.blob.core.windows.net/product-images/c97fd5d4-fb43-440b-ba4b-4603a9bb3638.jpeg?sv=2025-05-05&se=2036-05-11T11%3A16%3A04Z&sr=b&sp=r&sig=3tao0%2B2ZsO2DqU6oZizsOwpeIYzJEnufUhABn1DuLyo%3D", ImageBlobName = "c97fd5d4-fb43-440b-ba4b-4603a9bb3638.jpeg" },
            new Product { Id = 5, Name = "Laptop Stand Adjustable", Description = "Ergonomic aluminum laptop stand with 6 adjustable height levels. Compatible with 10–17 inch laptops. Foldable and lightweight.", Price = 34.99m, StockQuantity = 300, IsActive = true, CreatedAt = seedDate, ImageUrl = "https://storagecommerceprince.blob.core.windows.net/product-images/c462600c-e35c-4043-b6b8-7b36463178d9.jpeg?sv=2025-05-05&se=2036-05-11T11%3A14%3A59Z&sr=b&sp=r&sig=KAojd%2BMwg0uwK2ooB5Jik4bCJP4vMiXVjMLuGm6rzjU%3D", ImageBlobName = "c462600c-e35c-4043-b6b8-7b36463178d9.jpeg" },

            // Clothing
            new Product { Id = 6, Name = "Classic Fit Crew-Neck T-Shirt", Description = "100% organic cotton crew-neck tee in a relaxed fit. Pre-shrunk, machine washable. Available in 12 colours.", Price = 19.99m, StockQuantity = 500, IsActive = true, CreatedAt = seedDate, ImageUrl = "https://storagecommerceprince.blob.core.windows.net/product-images/5c7cb8fd-91a2-4a28-be9c-8c4f1f5f448b.jpeg?sv=2025-05-05&se=2036-05-11T11%3A13%3A40Z&sr=b&sp=r&sig=c2Axl3A%2BrPfxcG31gkVSHwEG%2BbjmpyeqPh2MmfJXji4%3D", ImageBlobName = "5c7cb8fd-91a2-4a28-be9c-8c4f1f5f448b.jpeg" },
            new Product { Id = 7, Name = "Slim Fit Chino Pants", Description = "Stretch-cotton chinos with a modern slim fit. Five-pocket styling, zip fly. Ideal for smart-casual occasions.", Price = 49.99m, StockQuantity = 250, IsActive = true, CreatedAt = seedDate, ImageUrl = "https://storagecommerceprince.blob.core.windows.net/product-images/3920d271-da68-4dec-a75b-0cd594725c7c.jpeg?sv=2025-05-05&se=2036-05-11T11%3A17%3A16Z&sr=b&sp=r&sig=rwCh07ePT5YYHXSxHO7i0o%2BDyQCFzMxH5buTUXq%2FaRs%3D", ImageBlobName = "3920d271-da68-4dec-a75b-0cd594725c7c.jpeg" },
            new Product { Id = 8, Name = "Waterproof Hiking Jacket", Description = "Lightweight packable rain jacket with taped seams, adjustable hood and cuffs, and zippered chest pocket.", Price = 89.99m, StockQuantity = 150, IsActive = true, CreatedAt = seedDate, ImageUrl = "https://storagecommerceprince.blob.core.windows.net/product-images/a32be3ff-2715-440c-8b8d-40ca878601ec.jpeg?sv=2025-05-05&se=2036-05-11T11%3A17%3A47Z&sr=b&sp=r&sig=i%2FRUGEG1JrKpgjlFP9g57k6pv903PdfvCvxLryIOd2I%3D", ImageBlobName = "a32be3ff-2715-440c-8b8d-40ca878601ec.jpeg" },
            new Product { Id = 9, Name = "Running Sneakers", Description = "Breathable mesh upper with responsive foam midsole. Ideal for road running and gym workouts. Neutral support.", Price = 74.99m, StockQuantity = 180, IsActive = true, CreatedAt = seedDate, ImageUrl = "https://storagecommerceprince.blob.core.windows.net/product-images/cfa78e29-5f29-4ec4-83ce-0339d8df3ae7.jpeg?sv=2025-05-05&se=2036-05-11T11%3A16%3A24Z&sr=b&sp=r&sig=itW%2BBGoyc63acMaI4m5xfKMw6JrJ9%2BR5p%2BBXn3vT3M0%3D", ImageBlobName = "cfa78e29-5f29-4ec4-83ce-0339d8df3ae7.jpeg" },

            // Books
            new Product { Id = 10, Name = "Clean Code by Robert C. Martin", Description = "A handbook of agile software craftsmanship. Covers writing readable, maintainable code with practical examples in Java.", Price = 34.99m, StockQuantity = 400, IsActive = true, CreatedAt = seedDate, ImageUrl = "https://storagecommerceprince.blob.core.windows.net/product-images/31cc70a4-4c43-4b5c-9f50-396cd58458d8.jpeg?sv=2025-05-05&se=2036-05-11T11%3A14%3A00Z&sr=b&sp=r&sig=0O6oPZw1f1A8H9C%2FcbYhVFZhVuOny%2FcZ6i%2Fk4n4mZSk%3D", ImageBlobName = "31cc70a4-4c43-4b5c-9f50-396cd58458d8.jpeg" },
            new Product { Id = 11, Name = "The Pragmatic Programmer", Description = "Timeless lessons for software developers by Andrew Hunt and David Thomas. Updated 20th anniversary edition.", Price = 39.99m, StockQuantity = 350, IsActive = true, CreatedAt = seedDate, ImageUrl = "https://storagecommerceprince.blob.core.windows.net/product-images/ff53c7d0-52de-41c2-93cc-c0e223cb8b0b.jpeg?sv=2025-05-05&se=2036-05-11T11%3A17%3A35Z&sr=b&sp=r&sig=W7GP99UeNlQzeiXULJZX3FVvdb14Ei96rJqsTnqVRvc%3D", ImageBlobName = "ff53c7d0-52de-41c2-93cc-c0e223cb8b0b.jpeg" },
            new Product { Id = 12, Name = "Designing Data-Intensive Applications", Description = "Deep dive into the principles, techniques and tradeoffs of modern distributed data systems by Martin Kleppmann.", Price = 44.99m, StockQuantity = 300, IsActive = true, CreatedAt = seedDate, ImageUrl = "https://storagecommerceprince.blob.core.windows.net/product-images/d2d19b7a-ee28-4125-82c3-1891a30f0c16.png?sv=2025-05-05&se=2036-05-11T11%3A14%3A27Z&sr=b&sp=r&sig=uGLcO0Vyl3QRsOd2FQ26KBlxx%2BLphLp8z8Iaoc3TmpA%3D", ImageBlobName = "d2d19b7a-ee28-4125-82c3-1891a30f0c16.png" },

            // Home & Garden
            new Product { Id = 13, Name = "Ceramic Pour-Over Coffee Set", Description = "Hand-crafted ceramic dripper with matching server and 50 paper filters. Brews 1–4 cups. Dishwasher safe.", Price = 42.99m, StockQuantity = 180, IsActive = true, CreatedAt = seedDate, ImageUrl = "https://storagecommerceprince.blob.core.windows.net/product-images/305bd23e-013f-48b1-9de9-d25aee1c3067.jpeg?sv=2025-05-05&se=2036-05-11T11%3A13%3A30Z&sr=b&sp=r&sig=j8WDkLmGhlNgpVVqY96tXv%2BVFV7xHznx%2FtQpdVVS4OI%3D", ImageBlobName = "305bd23e-013f-48b1-9de9-d25aee1c3067.jpeg" },
            new Product { Id = 14, Name = "Stainless Steel Cookware Set (10-Piece)", Description = "Tri-ply stainless steel set including saucepans, skillets and stockpot. Oven-safe to 500°F, induction compatible.", Price = 199.99m, StockQuantity = 60, IsActive = true, CreatedAt = seedDate, ImageUrl = "https://storagecommerceprince.blob.core.windows.net/product-images/f4b8e6f7-558c-43e0-a28a-a950266b4cbb.jpg?sv=2025-05-05&se=2036-05-11T11%3A18%3A49Z&sr=b&sp=r&sig=HSOb7F59F5hrdPP4y2JnNtNMYRHRHZoep1p7b9K2VG0%3D", ImageBlobName = "f4b8e6f7-558c-43e0-a28a-a950266b4cbb.jpg" },
            new Product { Id = 15, Name = "Scented Soy Candle — Lavender & Vanilla", Description = "Hand-poured 8 oz soy wax candle with cotton wick. 45-hour burn time. Made with essential oils. No paraffin.", Price = 16.99m, StockQuantity = 500, IsActive = true, CreatedAt = seedDate, ImageUrl = "https://storagecommerceprince.blob.core.windows.net/product-images/5c9d1fa8-3f68-4601-afd1-a95a7f73573a.jpeg?sv=2025-05-05&se=2036-05-11T11%3A16%3A57Z&sr=b&sp=r&sig=4%2FY0eBkOKJCp%2F58ENbOExRKeL9pqgiV4PDgOF%2FJhftg%3D", ImageBlobName = "5c9d1fa8-3f68-4601-afd1-a95a7f73573a.jpeg" },

            // Sports
            new Product { Id = 16, Name = "Adjustable Dumbbell Set (5–52.5 lb)", Description = "Space-saving dumbbell pair with quick-adjust dial system. Replaces 15 sets of weights. Includes storage tray.", Price = 299.99m, StockQuantity = 40, IsActive = true, CreatedAt = seedDate, ImageUrl = "https://storagecommerceprince.blob.core.windows.net/product-images/d8fc5481-790b-4fc4-ade6-2d511aec4601.jpg?sv=2025-05-05&se=2036-05-11T11%3A13%3A19Z&sr=b&sp=r&sig=Hjn9N3g6rPoQVJLhNO0BmihiSMgZp5jyFLJjAgAVFlQ%3D", ImageBlobName = "d8fc5481-790b-4fc4-ade6-2d511aec4601.jpg" },
            new Product { Id = 17, Name = "Yoga Mat Non-Slip", Description = "6mm thick eco-friendly TPE yoga mat with alignment lines. Superior grip, sweat-resistant surface. Includes carry strap.", Price = 29.99m, StockQuantity = 400, IsActive = true, CreatedAt = seedDate, ImageUrl = "https://storagecommerceprince.blob.core.windows.net/product-images/f380aa1c-f813-4d9e-9edf-06431121497e.jpg?sv=2025-05-05&se=2036-05-11T11%3A18%3A16Z&sr=b&sp=r&sig=%2FwCZCFA8OabgGy0y8uN6Kry%2Bf2VznPIUmkKXI12rwt0%3D", ImageBlobName = "f380aa1c-f813-4d9e-9edf-06431121497e.jpg" },
            new Product { Id = 18, Name = "Cycling Helmet", Description = "Lightweight MIPS-equipped road helmet with 18 vents, adjustable fit system and removable visor. CE EN 1078 certified.", Price = 64.99m, StockQuantity = 90, IsActive = true, CreatedAt = seedDate, ImageUrl = "https://storagecommerceprince.blob.core.windows.net/product-images/fcb810d1-fe16-4d65-a31b-03dccecfa553.jpg?sv=2025-05-05&se=2036-05-11T11%3A14%3A15Z&sr=b&sp=r&sig=R8e77NP4IgbYt2hvULGFnWTNKq%2FF5NCdvBb9TooUZfQ%3D", ImageBlobName = "fcb810d1-fe16-4d65-a31b-03dccecfa553.jpg" }
        );

        // ProductCategory join table
        builder.Entity<ProductCategory>().HasData(
            // Electronics
            new ProductCategory { ProductId = 1, CategoryId = 1 },
            new ProductCategory { ProductId = 2, CategoryId = 1 },
            new ProductCategory { ProductId = 3, CategoryId = 1 },
            new ProductCategory { ProductId = 4, CategoryId = 1 },
            new ProductCategory { ProductId = 5, CategoryId = 1 },

            // Clothing
            new ProductCategory { ProductId = 6, CategoryId = 2 },
            new ProductCategory { ProductId = 7, CategoryId = 2 },
            new ProductCategory { ProductId = 8, CategoryId = 2 },

            // Clothing + Sports cross-listing
            new ProductCategory { ProductId = 9, CategoryId = 2 },
            new ProductCategory { ProductId = 9, CategoryId = 5 },

            // Books
            new ProductCategory { ProductId = 10, CategoryId = 3 },
            new ProductCategory { ProductId = 11, CategoryId = 3 },
            new ProductCategory { ProductId = 12, CategoryId = 3 },

            // Home & Garden
            new ProductCategory { ProductId = 13, CategoryId = 4 },
            new ProductCategory { ProductId = 14, CategoryId = 4 },
            new ProductCategory { ProductId = 15, CategoryId = 4 },

            // Sports
            new ProductCategory { ProductId = 16, CategoryId = 5 },
            new ProductCategory { ProductId = 17, CategoryId = 5 },
            new ProductCategory { ProductId = 18, CategoryId = 5 }
        );
    }
}
