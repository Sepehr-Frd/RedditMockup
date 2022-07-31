﻿// <auto-generated />

#nullable disable

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using RedditMockup.DataAccess.Context;

namespace RedditMockup.DataAccess.Migrations
{
    [DbContext(typeof(RedditMockupContext))]
    partial class RedditMockupContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.7")
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
                            CreationDate = new DateTime(2022, 7, 22, 15, 55, 9, 264, DateTimeKind.Local).AddTicks(6770),
                            Family = "Hoorbakht",
                            LastUpdated = new DateTime(2022, 7, 22, 15, 55, 9, 264, DateTimeKind.Local).AddTicks(6770),
                            Name = "Mahyar"
                        },
                        new
                        {
                            Id = 2,
                            CreationDate = new DateTime(2022, 7, 22, 15, 55, 9, 264, DateTimeKind.Local).AddTicks(6780),
                            Family = "Foroughi Rad",
                            LastUpdated = new DateTime(2022, 7, 22, 15, 55, 9, 264, DateTimeKind.Local).AddTicks(6780),
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

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Bio = "",
                            CreationDate = new DateTime(2022, 7, 22, 15, 55, 9, 264, DateTimeKind.Local).AddTicks(7110),
                            Email = "",
                            LastUpdated = new DateTime(2022, 7, 22, 15, 55, 9, 264, DateTimeKind.Local).AddTicks(7110),
                            UserId = 1
                        },
                        new
                        {
                            Id = 2,
                            Bio = "",
                            CreationDate = new DateTime(2022, 7, 22, 15, 55, 9, 264, DateTimeKind.Local).AddTicks(7110),
                            Email = "",
                            LastUpdated = new DateTime(2022, 7, 22, 15, 55, 9, 264, DateTimeKind.Local).AddTicks(7110),
                            UserId = 2
                        });
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
                            CreationDate = new DateTime(2022, 7, 22, 15, 55, 9, 264, DateTimeKind.Local).AddTicks(6700),
                            Description = "Admin of Application",
                            LastUpdated = new DateTime(2022, 7, 22, 15, 55, 9, 264, DateTimeKind.Local).AddTicks(6700),
                            Title = "Admin"
                        },
                        new
                        {
                            Id = 2,
                            CreationDate = new DateTime(2022, 7, 22, 15, 55, 9, 264, DateTimeKind.Local).AddTicks(6750),
                            Description = "User of Application",
                            LastUpdated = new DateTime(2022, 7, 22, 15, 55, 9, 264, DateTimeKind.Local).AddTicks(6750),
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
                            CreationDate = new DateTime(2022, 7, 22, 15, 55, 9, 264, DateTimeKind.Local).AddTicks(6800),
                            LastUpdated = new DateTime(2022, 7, 22, 15, 55, 9, 264, DateTimeKind.Local).AddTicks(6800),
                            Password = "519651f64e305662a4a9e64d516ccafc06442a3cc3e61dbec98ffe5b407e4daeb8df1612c22c94f0c2e737618a3944c4e8864d07e8524d4109942f4a9747c472",
                            PersonId = 1,
                            Score = 0,
                            Username = "admin_admin"
                        },
                        new
                        {
                            Id = 2,
                            CreationDate = new DateTime(2022, 7, 22, 15, 55, 9, 264, DateTimeKind.Local).AddTicks(7060),
                            LastUpdated = new DateTime(2022, 7, 22, 15, 55, 9, 264, DateTimeKind.Local).AddTicks(7060),
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
                            CreationDate = new DateTime(2022, 7, 22, 15, 55, 9, 264, DateTimeKind.Local).AddTicks(7120),
                            LastUpdated = new DateTime(2022, 7, 22, 15, 55, 9, 264, DateTimeKind.Local).AddTicks(7120),
                            RoleId = 1,
                            UserId = 1
                        },
                        new
                        {
                            Id = 2,
                            CreationDate = new DateTime(2022, 7, 22, 15, 55, 9, 264, DateTimeKind.Local).AddTicks(7130),
                            LastUpdated = new DateTime(2022, 7, 22, 15, 55, 9, 264, DateTimeKind.Local).AddTicks(7130),
                            RoleId = 2,
                            UserId = 2
                        });
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
