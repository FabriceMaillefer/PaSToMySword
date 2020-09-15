﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MySwordTools.Commentaries.Model;

namespace MySwordTools.Commentaries.Migrations
{
    [DbContext(typeof(MySwordCommentaryDbContext))]
    [Migration("20200915091722_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.8");

            modelBuilder.Entity("MySwordTools.Commentaries.Model.Commentary", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("Book")
                        .HasColumnName("book")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("Chapter")
                        .HasColumnName("chapter")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Content")
                        .HasColumnName("data")
                        .HasColumnType("TEXT");

                    b.Property<int?>("FromVerse")
                        .HasColumnName("fromverse")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("ToVerse")
                        .HasColumnName("toverse")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("commentary");
                });

            modelBuilder.Entity("MySwordTools.Commentaries.Model.Details", b =>
                {
                    b.Property<string>("Title")
                        .HasColumnName("title")
                        .HasColumnType("TEXT");

                    b.Property<string>("Abbreviation")
                        .HasColumnName("abbreviation")
                        .HasColumnType("TEXT");

                    b.Property<string>("Autor")
                        .HasColumnName("author")
                        .HasColumnType("TEXT");

                    b.Property<string>("Comments")
                        .HasColumnName("comments")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnName("description")
                        .HasColumnType("TEXT");

                    b.Property<string>("PublishDate")
                        .HasColumnName("publishdate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Version")
                        .HasColumnName("version")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("VersionDate")
                        .HasColumnName("versiondate")
                        .HasColumnType("TEXT");

                    b.HasKey("Title");

                    b.ToTable("details");
                });
#pragma warning restore 612, 618
        }
    }
}
