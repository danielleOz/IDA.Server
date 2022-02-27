using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace IDA.ServerBL.Models
{
    public partial class IDADBContext : DbContext
    {
        public IDADBContext()
        {
        }

        public IDADBContext(DbContextOptions<IDADBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ChatMessage> ChatMessages { get; set; }
        public virtual DbSet<JobOffer> JobOffers { get; set; }
        public virtual DbSet<JobOfferStatus> JobOfferStatuses { get; set; }
        public virtual DbSet<Service> Services { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Worker> Workers { get; set; }
        public virtual DbSet<WorkerService> WorkerServices { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=localhost\\sqlexpress;Database=IDA.DB;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Hebrew_CI_AS");

            modelBuilder.Entity<ChatMessage>(entity =>
            {
                entity.ToTable("ChatMessage");

                entity.Property(e => e.MessageDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.MessageText)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.RecieverId).HasColumnName("RecieverID");

                entity.Property(e => e.SenderId).HasColumnName("SenderID");

                entity.HasOne(d => d.Reciever)
                    .WithMany(p => p.ChatMessageRecievers)
                    .HasForeignKey(d => d.RecieverId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ChatMessage_wid_foreign");

                entity.HasOne(d => d.Sender)
                    .WithMany(p => p.ChatMessageSenders)
                    .HasForeignKey(d => d.SenderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ChatMessage_uid_foreign");
            });

            modelBuilder.Entity<JobOffer>(entity =>
            {
                entity.ToTable("JobOffer");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.PublishDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.WorkerReviewDate).HasColumnType("datetime");

                entity.Property(e => e.WorkerReviewDescriptipon).HasMaxLength(500);

                entity.HasOne(d => d.ChosenWorker)
                    .WithMany(p => p.JobOffers)
                    .HasForeignKey(d => d.ChosenWorkerId)
                    .HasConstraintName("joboffer_wid_foreign");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.JobOffers)
                    .HasForeignKey(d => d.ServiceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("joboffer_serviceid_foreign");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.JobOffers)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("joboffer_sid_foreign");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.JobOffers)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("joboffer_uid_foreign");
            });

            modelBuilder.Entity<JobOfferStatus>(entity =>
            {
                entity.ToTable("JobOfferStatus");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<Service>(entity =>
            {
                entity.ToTable("Service");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.HasIndex(e => e.Email, "user_email_unique")
                    .IsUnique();

                entity.Property(e => e.Apartment)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Birthday).HasColumnType("datetime");

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.HouseNumber)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Street)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.UserPswd)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<Worker>(entity =>
            {
                entity.ToTable("Worker");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.AvailbleUntil).HasColumnType("datetime");

                entity.Property(e => e.RadiusKm).HasColumnName("RadiusKM");

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.Worker)
                    .HasForeignKey<Worker>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Worker_uid_foreign");
            });

            modelBuilder.Entity<WorkerService>(entity =>
            {
                entity.HasKey(e => new { e.ServiceId, e.WorkerId })
                    .HasName("WorkerService_swid_primary");

                entity.ToTable("WorkerService");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.WorkerServices)
                    .HasForeignKey(d => d.ServiceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("WorkerService_sid_foreign");

                entity.HasOne(d => d.Worker)
                    .WithMany(p => p.WorkerServices)
                    .HasForeignKey(d => d.WorkerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("WorkerService_wid_foreign");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
