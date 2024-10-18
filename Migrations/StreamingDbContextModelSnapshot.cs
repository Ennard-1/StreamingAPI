﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StreamingAPI.Data;

#nullable disable

namespace StreamingAPI.Migrations
{
    [DbContext(typeof(StreamingDbContext))]
    partial class StreamingDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.0");

            modelBuilder.Entity("StreamingAPI.Models.Playlist", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Playlists");
                });

            modelBuilder.Entity("StreamingAPI.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsCreator")
                        .HasColumnType("INTEGER");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("StreamingAPI.Models.Video", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CreatorId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("VideoPath")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CreatorId");

                    b.ToTable("Videos");
                });

            modelBuilder.Entity("StreamingAPI.Models.VideoPlaylist", b =>
                {
                    b.Property<int>("VideoId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("PlaylistId")
                        .HasColumnType("INTEGER");

                    b.HasKey("VideoId", "PlaylistId");

                    b.HasIndex("PlaylistId");

                    b.ToTable("VideoPlaylists");
                });

            modelBuilder.Entity("StreamingAPI.Models.Playlist", b =>
                {
                    b.HasOne("StreamingAPI.Models.User", "Owner")
                        .WithMany("Playlists")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("StreamingAPI.Models.Video", b =>
                {
                    b.HasOne("StreamingAPI.Models.User", "Creator")
                        .WithMany("Videos")
                        .HasForeignKey("CreatorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Creator");
                });

            modelBuilder.Entity("StreamingAPI.Models.VideoPlaylist", b =>
                {
                    b.HasOne("StreamingAPI.Models.Playlist", "Playlist")
                        .WithMany("VideoPlaylists")
                        .HasForeignKey("PlaylistId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("StreamingAPI.Models.Video", "Video")
                        .WithMany("VideoPlaylists")
                        .HasForeignKey("VideoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Playlist");

                    b.Navigation("Video");
                });

            modelBuilder.Entity("StreamingAPI.Models.Playlist", b =>
                {
                    b.Navigation("VideoPlaylists");
                });

            modelBuilder.Entity("StreamingAPI.Models.User", b =>
                {
                    b.Navigation("Playlists");

                    b.Navigation("Videos");
                });

            modelBuilder.Entity("StreamingAPI.Models.Video", b =>
                {
                    b.Navigation("VideoPlaylists");
                });
#pragma warning restore 612, 618
        }
    }
}
