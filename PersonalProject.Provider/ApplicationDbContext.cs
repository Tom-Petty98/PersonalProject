﻿using PersonalProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using PersonalProject.Provider.Configurations;

namespace PersonalProject.Provider;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Address> Addresses { get; set; }
    public DbSet<Application> Applications { get; set; }
    public DbSet<ApplicationDashboard> ApplicationDashboards { get; set; }
    public DbSet<ApplicationDetail> ApplicationDetails { get; set; }
    public DbSet<ApplicationStatus> ApplicationStatuses { get; set; }
    public DbSet<ApplicationStatusHistory> ApplicationStatusHistories { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }
    public DbSet<Document> Documents { get; set; }
    public DbSet<DocumentType> DocumentTypes { get; set; }
    public DbSet<GlobalSettings> GlobalSettings { get; set; }
    public DbSet<Installer> Installers { get; set; }
    public DbSet<InstallerDashboard> InstallerDashboards { get; set; }
    public DbSet<InstallerDetail> InstallerDetails { get; set; }
    public DbSet<InstallerStatus> InstallerStatuses { get; set; }
    public DbSet<InstallerStatusHistory> InstallerStatusHistories { get; set; }
    public DbSet<Note> Notes { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<TechType> TechTypes { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserInvite> UserInvites { get; set; }
    public DbSet<UserInviteStatus> UserInviteStatuses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationEntityConfiguration).Assembly);

        modelBuilder.Entity<ApplicationStatus>().HasIndex(b => b.Code).HasDatabaseName("AS_Index_Code");
        modelBuilder.Entity<InstallerStatus>().HasIndex(b => b.Code).HasDatabaseName("IS_Index_Code");
        modelBuilder.Entity<UserInviteStatus>().HasIndex(b => b.Code).HasDatabaseName("UIS_Index_Code");

        modelBuilder.Entity<Application>().HasIndex(p => p.RefNumber).IsUnique(true);
        modelBuilder.Entity<Installer>().HasIndex(p => p.RefNumber).IsUnique(true);

        modelBuilder.Entity<ApplicationDashboard>(e => { e.HasNoKey(); e.ToView("vw_Dashboard_Application"); });
        modelBuilder.Entity<InstallerDashboard>().ToView("vw_Dashboard_Installer").HasNoKey();

        modelBuilder.Entity<User>()
            .HasMany(x => x.Roles)
            .WithMany(y => y.Users)
            .UsingEntity(z => z.ToTable("UserRoles"));
    }
}