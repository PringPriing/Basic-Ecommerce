using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Server.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProductImageUrls : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ImageBlobName", "ImageUrl" },
                values: new object[] { "3b6b1698-2ada-4c0c-9de9-4b6b406655da.png", "https://storagecommerceprince.blob.core.windows.net/product-images/3b6b1698-2ada-4c0c-9de9-4b6b406655da.png?sv=2025-05-05&se=2036-05-11T11%3A18%3A03Z&sr=b&sp=r&sig=Lk8LNPH9tSUoCMLhPDQ6ncvKgKlrBu0dUr65er5s%2FrY%3D" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ImageBlobName", "ImageUrl" },
                values: new object[] { "fd4142c9-c21f-466d-91f8-35254adce270.jpg", "https://storagecommerceprince.blob.core.windows.net/product-images/fd4142c9-c21f-466d-91f8-35254adce270.jpg?sv=2025-05-05&se=2036-05-11T10%3A33%3A23Z&sr=b&sp=r&sig=vWlgAJU8lxWRWN7%2Bmx0oM1jQowVtQrgSgRtut9A5NgQ%3D" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ImageBlobName", "ImageUrl" },
                values: new object[] { "96f04441-a3f0-4301-bfd4-4777bac1586c.jpeg", "https://storagecommerceprince.blob.core.windows.net/product-images/96f04441-a3f0-4301-bfd4-4777bac1586c.jpeg?sv=2025-05-05&se=2036-05-11T11%3A15%3A12Z&sr=b&sp=r&sig=UxN2Ug9IqksugoJw13bhcKS%2F2KK3zyx1NE7WA5cs%2F7Q%3D" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ImageBlobName", "ImageUrl" },
                values: new object[] { "c97fd5d4-fb43-440b-ba4b-4603a9bb3638.jpeg", "https://storagecommerceprince.blob.core.windows.net/product-images/c97fd5d4-fb43-440b-ba4b-4603a9bb3638.jpeg?sv=2025-05-05&se=2036-05-11T11%3A16%3A04Z&sr=b&sp=r&sig=3tao0%2B2ZsO2DqU6oZizsOwpeIYzJEnufUhABn1DuLyo%3D" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "ImageBlobName", "ImageUrl" },
                values: new object[] { "c462600c-e35c-4043-b6b8-7b36463178d9.jpeg", "https://storagecommerceprince.blob.core.windows.net/product-images/c462600c-e35c-4043-b6b8-7b36463178d9.jpeg?sv=2025-05-05&se=2036-05-11T11%3A14%3A59Z&sr=b&sp=r&sig=KAojd%2BMwg0uwK2ooB5Jik4bCJP4vMiXVjMLuGm6rzjU%3D" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "ImageBlobName", "ImageUrl" },
                values: new object[] { "5c7cb8fd-91a2-4a28-be9c-8c4f1f5f448b.jpeg", "https://storagecommerceprince.blob.core.windows.net/product-images/5c7cb8fd-91a2-4a28-be9c-8c4f1f5f448b.jpeg?sv=2025-05-05&se=2036-05-11T11%3A13%3A40Z&sr=b&sp=r&sig=c2Axl3A%2BrPfxcG31gkVSHwEG%2BbjmpyeqPh2MmfJXji4%3D" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "ImageBlobName", "ImageUrl" },
                values: new object[] { "3920d271-da68-4dec-a75b-0cd594725c7c.jpeg", "https://storagecommerceprince.blob.core.windows.net/product-images/3920d271-da68-4dec-a75b-0cd594725c7c.jpeg?sv=2025-05-05&se=2036-05-11T11%3A17%3A16Z&sr=b&sp=r&sig=rwCh07ePT5YYHXSxHO7i0o%2BDyQCFzMxH5buTUXq%2FaRs%3D" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "ImageBlobName", "ImageUrl" },
                values: new object[] { "a32be3ff-2715-440c-8b8d-40ca878601ec.jpeg", "https://storagecommerceprince.blob.core.windows.net/product-images/a32be3ff-2715-440c-8b8d-40ca878601ec.jpeg?sv=2025-05-05&se=2036-05-11T11%3A17%3A47Z&sr=b&sp=r&sig=i%2FRUGEG1JrKpgjlFP9g57k6pv903PdfvCvxLryIOd2I%3D" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "ImageBlobName", "ImageUrl" },
                values: new object[] { "cfa78e29-5f29-4ec4-83ce-0339d8df3ae7.jpeg", "https://storagecommerceprince.blob.core.windows.net/product-images/cfa78e29-5f29-4ec4-83ce-0339d8df3ae7.jpeg?sv=2025-05-05&se=2036-05-11T11%3A16%3A24Z&sr=b&sp=r&sig=itW%2BBGoyc63acMaI4m5xfKMw6JrJ9%2BR5p%2BBXn3vT3M0%3D" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "ImageBlobName", "ImageUrl" },
                values: new object[] { "31cc70a4-4c43-4b5c-9f50-396cd58458d8.jpeg", "https://storagecommerceprince.blob.core.windows.net/product-images/31cc70a4-4c43-4b5c-9f50-396cd58458d8.jpeg?sv=2025-05-05&se=2036-05-11T11%3A14%3A00Z&sr=b&sp=r&sig=0O6oPZw1f1A8H9C%2FcbYhVFZhVuOny%2FcZ6i%2Fk4n4mZSk%3D" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "ImageBlobName", "ImageUrl" },
                values: new object[] { "ff53c7d0-52de-41c2-93cc-c0e223cb8b0b.jpeg", "https://storagecommerceprince.blob.core.windows.net/product-images/ff53c7d0-52de-41c2-93cc-c0e223cb8b0b.jpeg?sv=2025-05-05&se=2036-05-11T11%3A17%3A35Z&sr=b&sp=r&sig=W7GP99UeNlQzeiXULJZX3FVvdb14Ei96rJqsTnqVRvc%3D" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "ImageBlobName", "ImageUrl" },
                values: new object[] { "d2d19b7a-ee28-4125-82c3-1891a30f0c16.png", "https://storagecommerceprince.blob.core.windows.net/product-images/d2d19b7a-ee28-4125-82c3-1891a30f0c16.png?sv=2025-05-05&se=2036-05-11T11%3A14%3A27Z&sr=b&sp=r&sig=uGLcO0Vyl3QRsOd2FQ26KBlxx%2BLphLp8z8Iaoc3TmpA%3D" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "ImageBlobName", "ImageUrl" },
                values: new object[] { "305bd23e-013f-48b1-9de9-d25aee1c3067.jpeg", "https://storagecommerceprince.blob.core.windows.net/product-images/305bd23e-013f-48b1-9de9-d25aee1c3067.jpeg?sv=2025-05-05&se=2036-05-11T11%3A13%3A30Z&sr=b&sp=r&sig=j8WDkLmGhlNgpVVqY96tXv%2BVFV7xHznx%2FtQpdVVS4OI%3D" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "ImageBlobName", "ImageUrl" },
                values: new object[] { "f4b8e6f7-558c-43e0-a28a-a950266b4cbb.jpg", "https://storagecommerceprince.blob.core.windows.net/product-images/f4b8e6f7-558c-43e0-a28a-a950266b4cbb.jpg?sv=2025-05-05&se=2036-05-11T11%3A18%3A49Z&sr=b&sp=r&sig=HSOb7F59F5hrdPP4y2JnNtNMYRHRHZoep1p7b9K2VG0%3D" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "ImageBlobName", "ImageUrl" },
                values: new object[] { "5c9d1fa8-3f68-4601-afd1-a95a7f73573a.jpeg", "https://storagecommerceprince.blob.core.windows.net/product-images/5c9d1fa8-3f68-4601-afd1-a95a7f73573a.jpeg?sv=2025-05-05&se=2036-05-11T11%3A16%3A57Z&sr=b&sp=r&sig=4%2FY0eBkOKJCp%2F58ENbOExRKeL9pqgiV4PDgOF%2FJhftg%3D" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "ImageBlobName", "ImageUrl" },
                values: new object[] { "d8fc5481-790b-4fc4-ade6-2d511aec4601.jpg", "https://storagecommerceprince.blob.core.windows.net/product-images/d8fc5481-790b-4fc4-ade6-2d511aec4601.jpg?sv=2025-05-05&se=2036-05-11T11%3A13%3A19Z&sr=b&sp=r&sig=Hjn9N3g6rPoQVJLhNO0BmihiSMgZp5jyFLJjAgAVFlQ%3D" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "ImageBlobName", "ImageUrl" },
                values: new object[] { "f380aa1c-f813-4d9e-9edf-06431121497e.jpg", "https://storagecommerceprince.blob.core.windows.net/product-images/f380aa1c-f813-4d9e-9edf-06431121497e.jpg?sv=2025-05-05&se=2036-05-11T11%3A18%3A16Z&sr=b&sp=r&sig=%2FwCZCFA8OabgGy0y8uN6Kry%2Bf2VznPIUmkKXI12rwt0%3D" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "ImageBlobName", "ImageUrl" },
                values: new object[] { "fcb810d1-fe16-4d65-a31b-03dccecfa553.jpg", "https://storagecommerceprince.blob.core.windows.net/product-images/fcb810d1-fe16-4d65-a31b-03dccecfa553.jpg?sv=2025-05-05&se=2036-05-11T11%3A14%3A15Z&sr=b&sp=r&sig=R8e77NP4IgbYt2hvULGFnWTNKq%2FF5NCdvBb9TooUZfQ%3D" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ImageBlobName", "ImageUrl" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ImageBlobName", "ImageUrl" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ImageBlobName", "ImageUrl" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ImageBlobName", "ImageUrl" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "ImageBlobName", "ImageUrl" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "ImageBlobName", "ImageUrl" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "ImageBlobName", "ImageUrl" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "ImageBlobName", "ImageUrl" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "ImageBlobName", "ImageUrl" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "ImageBlobName", "ImageUrl" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "ImageBlobName", "ImageUrl" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "ImageBlobName", "ImageUrl" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "ImageBlobName", "ImageUrl" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "ImageBlobName", "ImageUrl" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "ImageBlobName", "ImageUrl" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "ImageBlobName", "ImageUrl" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "ImageBlobName", "ImageUrl" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "ImageBlobName", "ImageUrl" },
                values: new object[] { null, null });
        }
    }
}
