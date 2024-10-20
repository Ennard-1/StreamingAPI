﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StreamingAPI.Data;

#nullable disable

namespace StreamingAPI.Migrations
{
    [DbContext(typeof(StreamingDbContext))]
    [Migration("20241019190203_c")]
    partial class c
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.0");

            modelBuilder.Entity("StreamingAPI.Models.Conteudo", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CriadorID")
                        .HasColumnType("INTEGER");

                    b.Property<string>("NomeArquivo")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Thumbnail")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Tipo")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Titulo")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.HasIndex("CriadorID");

                    b.ToTable("Conteudos");
                });

            modelBuilder.Entity("StreamingAPI.Models.Criador", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("Criadores");
                });

            modelBuilder.Entity("StreamingAPI.Models.ItemPlaylist", b =>
                {
                    b.Property<int>("PlaylistID")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ConteudoID")
                        .HasColumnType("INTEGER");

                    b.HasKey("PlaylistID", "ConteudoID");

                    b.HasIndex("ConteudoID");

                    b.ToTable("ItemPlaylist");
                });

            modelBuilder.Entity("StreamingAPI.Models.Playlist", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("UsuarioID")
                        .HasColumnType("INTEGER");

                    b.HasKey("ID");

                    b.HasIndex("UsuarioID");

                    b.ToTable("Playlists");
                });

            modelBuilder.Entity("StreamingAPI.Models.Usuario", b =>
                {
                    b.Property<int>("ID")
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

                    b.HasKey("ID");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Usuarios");
                });

            modelBuilder.Entity("StreamingAPI.Models.Conteudo", b =>
                {
                    b.HasOne("StreamingAPI.Models.Criador", "Criador")
                        .WithMany("Conteudos")
                        .HasForeignKey("CriadorID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Criador");
                });

            modelBuilder.Entity("StreamingAPI.Models.ItemPlaylist", b =>
                {
                    b.HasOne("StreamingAPI.Models.Conteudo", "Conteudo")
                        .WithMany("ItemPlaylists")
                        .HasForeignKey("ConteudoID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("StreamingAPI.Models.Playlist", "Playlist")
                        .WithMany("ItemPlaylists")
                        .HasForeignKey("PlaylistID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Conteudo");

                    b.Navigation("Playlist");
                });

            modelBuilder.Entity("StreamingAPI.Models.Playlist", b =>
                {
                    b.HasOne("StreamingAPI.Models.Usuario", "Usuario")
                        .WithMany("Playlists")
                        .HasForeignKey("UsuarioID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("StreamingAPI.Models.Conteudo", b =>
                {
                    b.Navigation("ItemPlaylists");
                });

            modelBuilder.Entity("StreamingAPI.Models.Criador", b =>
                {
                    b.Navigation("Conteudos");
                });

            modelBuilder.Entity("StreamingAPI.Models.Playlist", b =>
                {
                    b.Navigation("ItemPlaylists");
                });

            modelBuilder.Entity("StreamingAPI.Models.Usuario", b =>
                {
                    b.Navigation("Playlists");
                });
#pragma warning restore 612, 618
        }
    }
}
