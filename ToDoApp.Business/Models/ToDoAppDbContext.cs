using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ToDoApp.Business.SqlServer.Models;

public partial class ToDoAppDbContext : DbContext
{
    public ToDoAppDbContext()
    {
    }

    public ToDoAppDbContext(DbContextOptions<ToDoAppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ToDo> ToDos { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=ToDoAppDb;Integrated Security=true;Trust Server Certificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ToDo>(entity =>
        {
            entity.ToTable("ToDo");

            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Tasks).HasMaxLength(50);

            entity.HasOne(d => d.User).WithMany(p => p.ToDos)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ToDo_User");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");

            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.Username).HasMaxLength(50);

            entity.HasOne(d => d.ToDoList).WithMany(p => p.Users)
                .HasForeignKey(d => d.ToDoListId)
                .HasConstraintName("FK_User_ToDo");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
