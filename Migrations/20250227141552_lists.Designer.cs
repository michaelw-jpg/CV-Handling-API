﻿// <auto-generated />
using System;
using CV_Handling_API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CV_Handling_API.Migrations
{
    [DbContext(typeof(CVHandlingDBContext))]
    [Migration("20250227141552_lists")]
    partial class lists
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CV_Handling_API.Models.Education", b =>
                {
                    b.Property<int>("EducationID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EducationID"));

                    b.Property<string>("Degree")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("EduDescription")
                        .HasMaxLength(280)
                        .HasColumnType("nvarchar(280)");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("date");

                    b.Property<int>("PersonID_FK")
                        .HasColumnType("int");

                    b.Property<string>("School")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("date");

                    b.HasKey("EducationID");

                    b.HasIndex("PersonID_FK");

                    b.ToTable("Educations");
                });

            modelBuilder.Entity("CV_Handling_API.Models.Experience", b =>
                {
                    b.Property<int>("ExperienceID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ExperienceID"));

                    b.Property<string>("Company")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("date");

                    b.Property<int>("PersonID_FK")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("date");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("WorkDescription")
                        .HasMaxLength(280)
                        .HasColumnType("nvarchar(280)");

                    b.HasKey("ExperienceID");

                    b.HasIndex("PersonID_FK");

                    b.ToTable("Experiences");
                });

            modelBuilder.Entity("CV_Handling_API.Models.Person", b =>
                {
                    b.Property<int>("PersonID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PersonID"));

                    b.Property<string>("EmailAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PersonID");

                    b.ToTable("Persons");
                });

            modelBuilder.Entity("CV_Handling_API.Models.Education", b =>
                {
                    b.HasOne("CV_Handling_API.Models.Person", "Person")
                        .WithMany("Educations")
                        .HasForeignKey("PersonID_FK")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Person");
                });

            modelBuilder.Entity("CV_Handling_API.Models.Experience", b =>
                {
                    b.HasOne("CV_Handling_API.Models.Person", "Person")
                        .WithMany("Experiences")
                        .HasForeignKey("PersonID_FK")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Person");
                });

            modelBuilder.Entity("CV_Handling_API.Models.Person", b =>
                {
                    b.Navigation("Educations");

                    b.Navigation("Experiences");
                });
#pragma warning restore 612, 618
        }
    }
}
