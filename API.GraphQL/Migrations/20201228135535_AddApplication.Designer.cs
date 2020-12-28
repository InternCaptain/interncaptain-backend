﻿// <auto-generated />
using API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace API.GraphQL.Migrations
{
    [DbContext(typeof(APIContext))]
    [Migration("20201228135535_AddApplication")]
    partial class AddApplication
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityByDefaultColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("API.Models.Application", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id")
                        .UseIdentityByDefaultColumn();

                    b.Property<long>("InternshipId")
                        .HasColumnType("bigint")
                        .HasColumnName("internship_id");

                    b.Property<int>("Status")
                        .HasColumnType("integer")
                        .HasColumnName("status");

                    b.Property<long>("StudentId")
                        .HasColumnType("bigint")
                        .HasColumnName("student_id");

                    b.HasKey("Id")
                        .HasName("pk_applications");

                    b.ToTable("applications", "ic_application");
                });

            modelBuilder.Entity("API.Models.Company", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id")
                        .UseIdentityByDefaultColumn();

                    b.Property<string>("Name")
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("pk_companies");

                    b.ToTable("companies", "ic_company");
                });

            modelBuilder.Entity("API.Models.Internship", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id")
                        .UseIdentityByDefaultColumn();

                    b.Property<long>("CompanyId")
                        .HasColumnType("bigint")
                        .HasColumnName("company_id");

                    b.Property<string>("Description")
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<int>("Domain")
                        .HasColumnType("integer")
                        .HasColumnName("domain");

                    b.Property<string>("PositionName")
                        .HasColumnType("text")
                        .HasColumnName("position_name");

                    b.Property<long>("RecruiterId")
                        .HasColumnType("bigint")
                        .HasColumnName("recruiter_id");

                    b.HasKey("Id")
                        .HasName("pk_internships");

                    b.ToTable("internships", "ic_internship");
                });

            modelBuilder.Entity("API.Models.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id")
                        .UseIdentityByDefaultColumn();

                    b.Property<string>("Email")
                        .HasColumnType("text")
                        .HasColumnName("email");

                    b.Property<string>("FirstName")
                        .HasColumnType("text")
                        .HasColumnName("first_name");

                    b.Property<string>("LastName")
                        .HasColumnType("text")
                        .HasColumnName("last_name");

                    b.Property<string>("Password")
                        .HasColumnType("text")
                        .HasColumnName("password");

                    b.Property<string>("ProfilePicture")
                        .HasColumnType("text")
                        .HasColumnName("profile_picture");

                    b.Property<int>("Role")
                        .HasColumnType("integer")
                        .HasColumnName("role");

                    b.HasKey("Id")
                        .HasName("pk_users");

                    b.ToTable("users", "ic_user");
                });
#pragma warning restore 612, 618
        }
    }
}
