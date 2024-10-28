﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StreamingAPI.Data;

#nullable disable

namespace StreamingAPI.Migrations
{
    [DbContext(typeof(StreamingDbContext))]
    [Migration("20241026225949_k")]
    partial class k
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.0");

            modelBuilder.Entity("Playlist", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("UsuarioID")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("UsuarioID");

                    b.ToTable("Playlists");
                });

            modelBuilder.Entity("StreamingAPI.Models.Conteudo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("NomeArquivo")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("PlaylistId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Thumbnail")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Tipo")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Titulo")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("UsuarioID")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("PlaylistId");

                    b.HasIndex("UsuarioID");

                    b.ToTable("Conteudos");
                });

            modelBuilder.Entity("StreamingAPI.Models.Usuario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<string>("SenhaHash")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Usuarios");
                });

            modelBuilder.Entity("Playlist", b =>
                {
                    b.HasOne("StreamingAPI.Models.Usuario", null)
                        .WithMany("Playlists")
                        .HasForeignKey("UsuarioID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("StreamingAPI.Models.Conteudo", b =>
                {
                    b.HasOne("Playlist", null)
                        .WithMany("Conteudos")
                        .HasForeignKey("PlaylistId");

                    b.HasOne("StreamingAPI.Models.Usuario", "Usuario")
                        .WithMany("Conteudos")
                        .HasForeignKey("UsuarioID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("Playlist", b =>
                {
                    b.Navigation("Conteudos");
                });

            modelBuilder.Entity("StreamingAPI.Models.Usuario", b =>
                {
                    b.Navigation("Conteudos");

                    b.Navigation("Playlists");
                });
#pragma warning restore 612, 618
        }
    }
}
