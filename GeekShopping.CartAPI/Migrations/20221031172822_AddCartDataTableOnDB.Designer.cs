﻿// <auto-generated />
using GeekShopping.CartAPI.Model.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace GeekShopping.CartAPI.Migrations
{
    [DbContext(typeof(MySQLContext))]
    [Migration("20221031172822_AddCartDataTableOnDB")]
    partial class AddCartDataTableOnDB
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("GeekShopping.CartAPI.Model.CartDetail", b =>
                {
                    b.Property<int>("CartDetailId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("cart_detail_id");

                    b.Property<int>("CartHeaderId")
                        .HasColumnType("int")
                        .HasColumnName("cart_header_id");

                    b.Property<int>("Count")
                        .HasColumnType("int")
                        .HasColumnName("count");

                    b.Property<int>("ProductId")
                        .HasColumnType("int")
                        .HasColumnName("product_id");

                    b.HasKey("CartDetailId");

                    b.HasIndex("CartHeaderId");

                    b.HasIndex("ProductId");

                    b.ToTable("cart_detail");
                });

            modelBuilder.Entity("GeekShopping.CartAPI.Model.CartHeader", b =>
                {
                    b.Property<int>("CartHeaderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("cart_header_id");

                    b.Property<string>("CouponCode")
                        .HasColumnType("longtext")
                        .HasColumnName("coupon_code");

                    b.Property<string>("UserId")
                        .HasColumnType("longtext")
                        .HasColumnName("user_id");

                    b.HasKey("CartHeaderId");

                    b.ToTable("cart_header");
                });

            modelBuilder.Entity("GeekShopping.CartAPI.Model.Category", b =>
                {
                    b.Property<int>("CategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("category_id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("name");

                    b.HasKey("CategoryId");

                    b.ToTable("category");
                });

            modelBuilder.Entity("GeekShopping.CartAPI.Model.Product", b =>
                {
                    b.Property<int>("ProductId")
                        .HasColumnType("int")
                        .HasColumnName("product_id");

                    b.Property<int>("CategoryId")
                        .HasColumnType("int")
                        .HasColumnName("category_id");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("varchar(500)")
                        .HasColumnName("description");

                    b.Property<string>("ImageURL")
                        .HasMaxLength(300)
                        .HasColumnType("varchar(300)")
                        .HasColumnName("image_url");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("varchar(150)")
                        .HasColumnName("name");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(65,30)")
                        .HasColumnName("price");

                    b.Property<int>("Stock")
                        .HasColumnType("int")
                        .HasColumnName("stock");

                    b.HasKey("ProductId");

                    b.HasIndex("CategoryId");

                    b.ToTable("product");
                });

            modelBuilder.Entity("GeekShopping.CartAPI.Model.CartDetail", b =>
                {
                    b.HasOne("GeekShopping.CartAPI.Model.CartHeader", "CartHeader")
                        .WithMany()
                        .HasForeignKey("CartHeaderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GeekShopping.CartAPI.Model.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CartHeader");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("GeekShopping.CartAPI.Model.Product", b =>
                {
                    b.HasOne("GeekShopping.CartAPI.Model.Category", "Category")
                        .WithMany("Products")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("GeekShopping.CartAPI.Model.Category", b =>
                {
                    b.Navigation("Products");
                });
#pragma warning restore 612, 618
        }
    }
}
