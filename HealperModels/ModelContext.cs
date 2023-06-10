using System;
using System.Collections.Generic;
using HealperModels.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace HealperModels
{
    public partial class ModelContext : DbContext
    {
        private readonly IConfiguration _Configuration;
        public ModelContext(IConfiguration configuration)
        {
            _Configuration = configuration;
        }

        public ModelContext(DbContextOptions<ModelContext> options, IConfiguration configuration)
            : base(options)
        {
            _Configuration = configuration;
        }

        public virtual DbSet<ChatMessage> ChatMessages { get; set; } = null!;
        public virtual DbSet<Client> Clients { get; set; } = null!;
        public virtual DbSet<ConsultHistory> ConsultHistories { get; set; } = null!;
        public virtual DbSet<Consultant> Consultants { get; set; } = null!;
        public virtual DbSet<PsychologyScale> PsychologyScales { get; set; } = null!;
        public virtual DbSet<ScaleRecord> ScaleRecords { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySQL($"Data Source={_Configuration["DataSource"]};Database={_Configuration["Database"]};Password={_Configuration["Password"]};User ID={_Configuration["UserID"]};");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChatMessage>(entity =>
            {
                entity.ToTable("chat_message");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ClientId).HasColumnName("client_id");

                entity.Property(e => e.ConsultantId).HasColumnName("consultant_id");

                entity.Property(e => e.Content)
                    .HasMaxLength(1024)
                    .HasColumnName("content");

                entity.Property(e => e.CreateTime).HasColumnName("create_time");

                entity.Property(e => e.Sender)
                    .HasMaxLength(1)
                    .HasColumnName("sender");
            });

            modelBuilder.Entity<Client>(entity =>
            {
                entity.ToTable("client");

                entity.HasIndex(e => e.Id, "id")
                    .IsUnique();

                entity.HasIndex(e => e.Userphone, "userphone")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Age).HasColumnName("age");

                entity.Property(e => e.ExConsultantId).HasColumnName("ex_consultant_id");

                entity.Property(e => e.Nickname)
                    .HasMaxLength(64)
                    .HasColumnName("nickname");

                entity.Property(e => e.Password)
                    .HasMaxLength(32)
                    .HasColumnName("password");

                entity.Property(e => e.Profile)
                    .HasMaxLength(128)
                    .HasColumnName("profile")
                    .HasDefaultValueSql("'https://cube.elemecdn.com/0/88/03b0d39583f48206768a7534e55bcpng.png'");

                entity.Property(e => e.Sex)
                    .HasMaxLength(1)
                    .HasColumnName("sex");

                entity.Property(e => e.Userphone)
                    .HasMaxLength(11)
                    .HasColumnName("userphone");
            });

            modelBuilder.Entity<ConsultHistory>(entity =>
            {
                entity.ToTable("consult_history");

                entity.HasIndex(e => e.Id, "id")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Advice)
                    .HasMaxLength(1024)
                    .HasColumnName("advice");

                entity.Property(e => e.ClientId).HasColumnName("client_id");

                entity.Property(e => e.ConsultantId).HasColumnName("consultant_id");

                entity.Property(e => e.EndTime).HasColumnName("end_time");

                entity.Property(e => e.Expense).HasColumnName("expense");

                entity.Property(e => e.StartTime).HasColumnName("start_time");

                entity.Property(e => e.Status)
                    .HasMaxLength(1)
                    .HasColumnName("status");

                entity.Property(e => e.Summary)
                    .HasMaxLength(1024)
                    .HasColumnName("summary");
            });

            modelBuilder.Entity<Consultant>(entity =>
            {
                entity.ToTable("consultant");

                entity.HasIndex(e => e.Userphone, "userphone")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Age).HasColumnName("age");

                entity.Property(e => e.Expense)
                    .HasColumnName("expense")
                    .HasDefaultValueSql("'-1'");

                entity.Property(e => e.Label)
                    .HasMaxLength(64)
                    .HasColumnName("label");

                entity.Property(e => e.Password)
                    .HasMaxLength(32)
                    .HasColumnName("password");

                entity.Property(e => e.Profile)
                    .HasMaxLength(128)
                    .HasColumnName("profile")
                    .HasDefaultValueSql("'https://gxyrious.oss-cn-hangzhou.aliyuncs.com/img/tj-3-1/image-20221201134410488.png'");

                entity.Property(e => e.QrCodeLink)
                    .HasMaxLength(128)
                    .HasColumnName("qr_code_link");

                entity.Property(e => e.Realname)
                    .HasMaxLength(16)
                    .HasColumnName("realname");

                entity.Property(e => e.Sex)
                    .HasMaxLength(1)
                    .HasColumnName("sex");

                entity.Property(e => e.Userphone)
                    .HasMaxLength(11)
                    .HasColumnName("userphone");
            });

            modelBuilder.Entity<PsychologyScale>(entity =>
            {
                entity.ToTable("psychology_scale");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Content)
                    .HasColumnType("text")
                    .HasColumnName("content");

                entity.Property(e => e.Image)
                    .HasMaxLength(128)
                    .HasColumnName("image");

                entity.Property(e => e.Name)
                    .HasMaxLength(32)
                    .HasColumnName("name");

                entity.Property(e => e.QuesNum).HasColumnName("ques_num");

                entity.Property(e => e.Subjective)
                    .HasMaxLength(64)
                    .HasColumnName("subjective");

                entity.Property(e => e.Summary)
                    .HasMaxLength(64)
                    .HasColumnName("summary");
            });

            modelBuilder.Entity<ScaleRecord>(entity =>
            {
                entity.ToTable("scale_record");

                entity.HasIndex(e => e.ScaleId, "scale_record_psychology_scale_id_fk");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ClientId).HasColumnName("client_id");

                entity.Property(e => e.EndTime).HasColumnName("end_time");

                entity.Property(e => e.IsHidden).HasColumnName("is_hidden");

                entity.Property(e => e.Record)
                    .HasColumnType("text")
                    .HasColumnName("record");

                entity.Property(e => e.ScaleId).HasColumnName("scale_id");

                entity.Property(e => e.Subjective)
                    .HasColumnType("text")
                    .HasColumnName("subjective");

                entity.HasOne(d => d.Scale)
                    .WithMany(p => p.ScaleRecords)
                    .HasForeignKey(d => d.ScaleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("scale_record_psychology_scale_id_fk");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
