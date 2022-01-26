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

        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<JobOffer> JobOffers { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<Review> Reviews { get; set; }
        public virtual DbSet<Service> Services { get; set; }
        public virtual DbSet<Status> Statuses { get; set; }
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

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.Cid)
                    .HasName("Customer_cid_primary");

                entity.ToTable("Customer");

                entity.Property(e => e.Cid)
                    .ValueGeneratedNever()
                    .HasColumnName("CId");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.HasOne(d => d.UserNameNavigation)
                    .WithMany(p => p.Customers)
                    .HasForeignKey(d => d.UserName)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Customer_username_foreign");
            });

            modelBuilder.Entity<JobOffer>(entity =>
            {
                entity.HasKey(e => e.Jid)
                    .HasName("joboffer_jid_primary");

                entity.ToTable("JobOffer");

                entity.Property(e => e.Jid)
                    .ValueGeneratedNever()
                    .HasColumnName("JId");

                entity.Property(e => e.Cid).HasColumnName("CId");

                entity.Property(e => e.Rid).HasColumnName("RId");

                entity.Property(e => e.Swid).HasColumnName("SWId");

                entity.Property(e => e.Wid).HasColumnName("WId");

                entity.HasOne(d => d.CidNavigation)
                    .WithMany(p => p.JobOffers)
                    .HasForeignKey(d => d.Cid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("joboffer_cid_foreign");

                entity.HasOne(d => d.RidNavigation)
                    .WithMany(p => p.JobOffers)
                    .HasForeignKey(d => d.Rid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("joboffer_rid_foreign");

                entity.HasOne(d => d.StatusNavigation)
                    .WithMany(p => p.JobOffers)
                    .HasForeignKey(d => d.Status)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("joboffer_status_foreign");

                entity.HasOne(d => d.Sw)
                    .WithMany(p => p.JobOffers)
                    .HasForeignKey(d => d.Swid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("joboffer_swid_foreign");

                entity.HasOne(d => d.WidNavigation)
                    .WithMany(p => p.JobOffers)
                    .HasForeignKey(d => d.Wid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("joboffer_wid_foreign");
            });

            modelBuilder.Entity<Location>(entity =>
            {
                entity.HasKey(e => e.Lid)
                    .HasName("location_lid_primary");

                entity.ToTable("Location");

                entity.Property(e => e.Lid)
                    .ValueGeneratedNever()
                    .HasColumnName("LId");

                entity.Property(e => e.Adress)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<Review>(entity =>
            {
                entity.HasKey(e => e.Rid)
                    .HasName("Review_rid_primary");

                entity.ToTable("Review");

                entity.Property(e => e.Rid)
                    .ValueGeneratedNever()
                    .HasColumnName("RId");

                entity.Property(e => e.Cid).HasColumnName("CId");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.Jid).HasColumnName("JId");

                entity.Property(e => e.Sid).HasColumnName("SId");

                entity.Property(e => e.Sname)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("SName");

                entity.Property(e => e.Wid).HasColumnName("WId");

                entity.HasOne(d => d.CidNavigation)
                    .WithMany(p => p.Reviews)
                    .HasForeignKey(d => d.Cid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Review_cid_foreign");

                entity.HasOne(d => d.JidNavigation)
                    .WithMany(p => p.Reviews)
                    .HasForeignKey(d => d.Jid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Review_jid_foreign");

                entity.HasOne(d => d.WidNavigation)
                    .WithMany(p => p.Reviews)
                    .HasForeignKey(d => d.Wid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Review_wid_foreign");
            });

            modelBuilder.Entity<Service>(entity =>
            {
                entity.HasKey(e => e.Sid)
                    .HasName("Service_sid_primary");

                entity.ToTable("Service");

                entity.Property(e => e.Sid)
                    .ValueGeneratedNever()
                    .HasColumnName("SId");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<Status>(entity =>
            {
                entity.ToTable("Status");

                entity.Property(e => e.StatusId).ValueGeneratedNever();

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserName)
                    .HasName("user_username_primary");

                entity.ToTable("User");

                entity.Property(e => e.UserName).HasMaxLength(255);

                entity.Property(e => e.Adress)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Birthday).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.UserPswd)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<Worker>(entity =>
            {
                entity.HasKey(e => e.Wid)
                    .HasName("Worker_wid_primary");

                entity.ToTable("Worker");

                entity.Property(e => e.Wid)
                    .ValueGeneratedNever()
                    .HasColumnName("WId");

                entity.Property(e => e.Lid).HasColumnName("LId");

                entity.Property(e => e.Service)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.HasOne(d => d.LidNavigation)
                    .WithMany(p => p.Workers)
                    .HasForeignKey(d => d.Lid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Worker_lid_foreign");

                entity.HasOne(d => d.UserNameNavigation)
                    .WithMany(p => p.Workers)
                    .HasForeignKey(d => d.UserName)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Worker_username_foreign");
            });

            modelBuilder.Entity<WorkerService>(entity =>
            {
                entity.HasKey(e => e.Swid)
                    .HasName("WorkerService_swid_primary");

                entity.ToTable("WorkerService");

                entity.Property(e => e.Swid)
                    .ValueGeneratedNever()
                    .HasColumnName("SWId");

                entity.Property(e => e.Sid).HasColumnName("SId");

                entity.Property(e => e.Wid).HasColumnName("WId");

                entity.HasOne(d => d.SidNavigation)
                    .WithMany(p => p.WorkerServices)
                    .HasForeignKey(d => d.Sid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("WorkerService_sid_foreign");

                entity.HasOne(d => d.WidNavigation)
                    .WithMany(p => p.WorkerServices)
                    .HasForeignKey(d => d.Wid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("WorkerService_wid_foreign");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
