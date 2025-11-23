using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace SberVolunteerAPI.Models;

public partial class SberVolunteerContext : DbContext
{
    public SberVolunteerContext()
    {
    }

    public SberVolunteerContext(DbContextOptions<SberVolunteerContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<EventsToVolunteer> EventsToVolunteers { get; set; }

    public virtual DbSet<User> Users { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.IdEvent).HasName("PRIMARY");

            entity.ToTable("events");

            entity.HasIndex(e => e.CreatorId, "creator_id");

            entity.HasIndex(e => e.EventTitle, "event_title").IsUnique();

            entity.Property(e => e.IdEvent).HasColumnName("Id_event");
            entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("creationDate");
            entity.Property(e => e.CreatorId).HasColumnName("creator_id");
            entity.Property(e => e.DatetimeEnd)
                .HasColumnType("datetime")
                .HasColumnName("datetime_end");
            entity.Property(e => e.DatetimeStart)
                .HasColumnType("datetime")
                .HasColumnName("datetime_start");
            entity.Property(e => e.EventDescription)
                .HasColumnType("text")
                .HasColumnName("event_description");
            entity.Property(e => e.EventState)
                .HasDefaultValueSql("'active'")
                .HasColumnType("enum('active','closed','cancelled')")
                .HasColumnName("event_state");
            entity.Property(e => e.EventTitle)
                .HasMaxLength(300)
                .HasColumnName("event_title");

            entity.HasOne(d => d.Creator).WithMany(p => p.Events)
                .HasForeignKey(d => d.CreatorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("events_ibfk_1");
        });

        modelBuilder.Entity<EventsToVolunteer>(entity =>
        {
            entity.HasKey(e => e.IdRecord).HasName("PRIMARY");

            entity.ToTable("events_to_volunteers");

            entity.HasIndex(e => new { e.IdEvent, e.IdVolunteer }, "Id_event").IsUnique();

            entity.HasIndex(e => e.IdVolunteer, "Id_volunteer");

            entity.Property(e => e.IdRecord).HasColumnName("Id_record");
            entity.Property(e => e.IdEvent).HasColumnName("Id_event");
            entity.Property(e => e.IdVolunteer).HasColumnName("Id_volunteer");
            entity.Property(e => e.RequestStatus)
                .HasDefaultValueSql("'pending'")
                .HasColumnType("enum('pending','approved','rejected')")
                .HasColumnName("request_status");
            entity.Property(e => e.VisitStatus)
                .HasDefaultValueSql("'unknown'")
                .HasColumnType("enum('unknown','came','absent')")
                .HasColumnName("visit_status");

            entity.HasOne(d => d.IdEventNavigation).WithMany(p => p.EventsToVolunteers)
                .HasForeignKey(d => d.IdEvent)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("events_to_volunteers_ibfk_1");

            entity.HasOne(d => d.IdVolunteerNavigation).WithMany(p => p.EventsToVolunteers)
                .HasForeignKey(d => d.IdVolunteer)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("events_to_volunteers_ibfk_2");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.IdUser).HasName("PRIMARY");

            entity.ToTable("users");

            entity.HasIndex(e => e.UserLogin, "user_login").IsUnique();

            entity.Property(e => e.IdUser).HasColumnName("Id_user");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.UserLogin).HasColumnName("user_login");
            entity.Property(e => e.UserMiddlename)
                .HasMaxLength(255)
                .HasColumnName("user_middlename");
            entity.Property(e => e.UserName)
                .HasMaxLength(255)
                .HasColumnName("user_name");
            entity.Property(e => e.UserRole)
                .HasColumnType("enum('organiser','volunteer')")
                .HasColumnName("user_role");
            entity.Property(e => e.UserSurname)
                .HasMaxLength(255)
                .HasColumnName("user_surname");
            entity.Property(e => e.VolunteersHours)
                .HasDefaultValueSql("'0'")
                .HasColumnName("volunteers_hours");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
