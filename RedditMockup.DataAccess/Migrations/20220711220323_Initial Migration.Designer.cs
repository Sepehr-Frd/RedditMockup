﻿// <auto-generated />

#nullable disable

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using RedditMockup.DataAccess.Context;

namespace RedditMockup.DataAccess.Migrations
{
    [DbContext(typeof(RedditMockupContext))]
    [Migration("20220711220323_Initial Migration")]
    partial class InitialMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("RedditMockup.Model.Entities.Answer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("LastUpdated")
                        .HasColumnType("datetime2");

                    b.Property<int>("QuestionId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("QuestionId");

                    b.HasIndex("UserId");

                    b.ToTable("Answers");
                });

            modelBuilder.Entity("RedditMockup.Model.Entities.AnswerVote", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("AnswerId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Kind")
                        .HasColumnType("bit");

                    b.Property<DateTime>("LastUpdated")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("AnswerId");

                    b.ToTable("Votes");
                });

            modelBuilder.Entity("RedditMockup.Model.Entities.Person", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Family")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("LastUpdated")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Persons");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CreationDate = new DateTime(2022, 7, 12, 2, 33, 23, 707, DateTimeKind.Local).AddTicks(3270),
                            Family = "Hoorbakht",
                            LastUpdated = new DateTime(2022, 7, 12, 2, 33, 23, 707, DateTimeKind.Local).AddTicks(3270),
                            Name = "Mahyar"
                        },
                        new
                        {
                            Id = 2,
                            CreationDate = new DateTime(2022, 7, 12, 2, 33, 23, 707, DateTimeKind.Local).AddTicks(3270),
                            Family = "Foroughi Rad",
                            LastUpdated = new DateTime(2022, 7, 12, 2, 33, 23, 707, DateTimeKind.Local).AddTicks(3270),
                            Name = "Sepehr"
                        });
                });

            modelBuilder.Entity("RedditMockup.Model.Entities.Profile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Bio")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("LastUpdated")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Profiles");
                });

            modelBuilder.Entity("RedditMockup.Model.Entities.Question", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("LastUpdated")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Questions");
                });

            modelBuilder.Entity("RedditMockup.Model.Entities.QuestionVote", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Kind")
                        .HasColumnType("bit");

                    b.Property<DateTime>("LastUpdated")
                        .HasColumnType("datetime2");

                    b.Property<int>("QuestionId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("QuestionId");

                    b.ToTable("QuestionVote");
                });

            modelBuilder.Entity("RedditMockup.Model.Entities.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("LastUpdated")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Roles");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CreationDate = new DateTime(2022, 7, 12, 2, 33, 23, 707, DateTimeKind.Local).AddTicks(3180),
                            Description = "Admin of Application",
                            LastUpdated = new DateTime(2022, 7, 12, 2, 33, 23, 707, DateTimeKind.Local).AddTicks(3180),
                            Title = "Admin"
                        },
                        new
                        {
                            Id = 2,
                            CreationDate = new DateTime(2022, 7, 12, 2, 33, 23, 707, DateTimeKind.Local).AddTicks(3220),
                            Description = "User of Application",
                            LastUpdated = new DateTime(2022, 7, 12, 2, 33, 23, 707, DateTimeKind.Local).AddTicks(3220),
                            Title = "User"
                        });
                });

            modelBuilder.Entity("RedditMockup.Model.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("LastUpdated")
                        .HasColumnType("datetime2");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PersonId")
                        .HasColumnType("int");

                    b.Property<int>("Score")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("PersonId")
                        .IsUnique();

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CreationDate = new DateTime(2022, 7, 12, 2, 33, 23, 707, DateTimeKind.Local).AddTicks(3290),
                            LastUpdated = new DateTime(2022, 7, 12, 2, 33, 23, 707, DateTimeKind.Local).AddTicks(3290),
                            Password = "519651f64e305662a4a9e64d516ccafc06442a3cc3e61dbec98ffe5b407e4daeb8df1612c22c94f0c2e737618a3944c4e8864d07e8524d4109942f4a9747c472",
                            PersonId = 1,
                            Score = 0,
                            Username = "admin_admin"
                        },
                        new
                        {
                            Id = 2,
                            CreationDate = new DateTime(2022, 7, 12, 2, 33, 23, 707, DateTimeKind.Local).AddTicks(3530),
                            LastUpdated = new DateTime(2022, 7, 12, 2, 33, 23, 707, DateTimeKind.Local).AddTicks(3530),
                            Password = "7f13d2c6d8191aaec19c5bb484deb750d7c79ce6d546815acbde63cbd857053310492804a674a16bfb18550e3e16df3c4bbd9801288e735057eb5010caa37ab8",
                            PersonId = 2,
                            Score = 0,
                            Username = "sepehr_frd"
                        });
                });

            modelBuilder.Entity("RedditMockup.Model.Entities.UserRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("LastUpdated")
                        .HasColumnType("datetime2");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserId");

                    b.ToTable("UserRoles");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CreationDate = new DateTime(2022, 7, 12, 2, 33, 23, 707, DateTimeKind.Local).AddTicks(3580),
                            LastUpdated = new DateTime(2022, 7, 12, 2, 33, 23, 707, DateTimeKind.Local).AddTicks(3580),
                            RoleId = 1,
                            UserId = 1
                        },
                        new
                        {
                            Id = 2,
                            CreationDate = new DateTime(2022, 7, 12, 2, 33, 23, 707, DateTimeKind.Local).AddTicks(3580),
                            LastUpdated = new DateTime(2022, 7, 12, 2, 33, 23, 707, DateTimeKind.Local).AddTicks(3580),
                            RoleId = 2,
                            UserId = 2
                        });
                });

            modelBuilder.Entity("RedditMockup.Model.Views.UserRolesView", b =>
                {
                    b.Property<string>("FullName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleTitle")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(max)");

                    b.ToView("UserRolesView");
                });

            modelBuilder.Entity("RedditMockup.Model.Entities.Answer", b =>
                {
                    b.HasOne("RedditMockup.Model.Entities.Question", "Question")
                        .WithMany("Answers")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RedditMockup.Model.Entities.User", "AnsweringUser")
                        .WithMany("Answers")
                        .HasForeignKey("UserId");

                    b.Navigation("AnsweringUser");

                    b.Navigation("Question");
                });

            modelBuilder.Entity("RedditMockup.Model.Entities.AnswerVote", b =>
                {
                    b.HasOne("RedditMockup.Model.Entities.Answer", "Answer")
                        .WithMany("Votes")
                        .HasForeignKey("AnswerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Answer");
                });

            modelBuilder.Entity("RedditMockup.Model.Entities.Profile", b =>
                {
                    b.HasOne("RedditMockup.Model.Entities.User", "User")
                        .WithOne("Profile")
                        .HasForeignKey("RedditMockup.Model.Entities.Profile", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("RedditMockup.Model.Entities.Question", b =>
                {
                    b.HasOne("RedditMockup.Model.Entities.User", "User")
                        .WithMany("Questions")
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("RedditMockup.Model.Entities.QuestionVote", b =>
                {
                    b.HasOne("RedditMockup.Model.Entities.Question", "Question")
                        .WithMany("Votes")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Question");
                });

            modelBuilder.Entity("RedditMockup.Model.Entities.User", b =>
                {
                    b.HasOne("RedditMockup.Model.Entities.Person", "Person")
                        .WithOne("User")
                        .HasForeignKey("RedditMockup.Model.Entities.User", "PersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Person");
                });

            modelBuilder.Entity("RedditMockup.Model.Entities.UserRole", b =>
                {
                    b.HasOne("RedditMockup.Model.Entities.Role", "Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RedditMockup.Model.Entities.User", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("RedditMockup.Model.Entities.Answer", b =>
                {
                    b.Navigation("Votes");
                });

            modelBuilder.Entity("RedditMockup.Model.Entities.Person", b =>
                {
                    b.Navigation("User");
                });

            modelBuilder.Entity("RedditMockup.Model.Entities.Question", b =>
                {
                    b.Navigation("Answers");

                    b.Navigation("Votes");
                });

            modelBuilder.Entity("RedditMockup.Model.Entities.Role", b =>
                {
                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("RedditMockup.Model.Entities.User", b =>
                {
                    b.Navigation("Answers");

                    b.Navigation("Profile");

                    b.Navigation("Questions");

                    b.Navigation("UserRoles");
                });
#pragma warning restore 612, 618
        }
    }
}