using DAL.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.FileDBContext
{
    public class StorageDbContext : IdentityDbContext<SysIdentityUser>, IStorageDbContext
    {
        public StorageDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(StorageDbContext).Assembly);
        }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.ApplyConfigurationsFromAssembly(typeof(SysDbContext).Assembly);
        //}
        public virtual DbSet<UserProfile> UserProfiles { get; set; }

    }

    public class UserProfileEntityTypeConfiguration : IEntityTypeConfiguration<UserProfile>
    {
        public void Configure(EntityTypeBuilder<UserProfile> builder)
        {
            builder
                .HasKey(x => x.Id);

            builder
                .HasOne(x => x.UserStorage)
                .WithOne(x => x.UserProfile)
                .HasForeignKey<UserProfile>(x => x.UserStorageId);
            //.HasForeignKey<UserStorage>(x => x.UserProfileId);

            builder
               .HasMany(x => x.AccessibleStorages)
               .WithOne()
               .HasForeignKey(x=>x.UserProfileId);

            builder
               .HasMany(x => x.AccessibleFiles)
               .WithOne()
               .HasForeignKey(x => x.UserProfileId);

            //builder
            //   .HasOne(x => x.AccessibleFile)
            //   .WithOne(x => x.UserProfile)
            //   .HasForeignKey<UserProfile>(x => x.FileAccessibilityId)
            //   .HasForeignKey<FileAccessibility>(x => x.UserProfileId);
        }
    }

    public class StorageAccessibilityEntityTypeConfiguration : IEntityTypeConfiguration<StorageAccessibility>
    {
        public void Configure(EntityTypeBuilder<StorageAccessibility> builder)
        {
            builder
                .HasKey(x => x.Id);

            //builder
            //    .HasOne(x => x.UserProfile)
            //    .WithMany(x => x.AccessibleStorages)
            //    .HasForeignKey(x => x.UserProfileId);

            builder
                .HasOne(x => x.UserStorage)
                .WithOne()
                .HasForeignKey<StorageAccessibility>(x => x.StorageId);
        }
    }

    public class FileAccessibilityEntityTypeConfiguration : IEntityTypeConfiguration<FileAccessibility>
    {
        public void Configure(EntityTypeBuilder<FileAccessibility> builder)
        {
            builder
                .HasKey(x => x.Id);

            //builder
            //    .HasOne(x => x.UserProfile)
            //    .WithMany(x => x.AccessibleFiles)
            //    .HasForeignKey(x => x.UserProfileId);

            builder
                .HasOne(x => x.File)
                .WithOne()
                .HasForeignKey<FileAccessibility>(x => x.FileId);
        }
    }

    public class SysIdentityUserEntityTypeConfiguration : IEntityTypeConfiguration<SysIdentityUser>
    {
        public void Configure(EntityTypeBuilder<SysIdentityUser> builder)
        {
            builder
                .HasKey(x => x.Id);

            builder
                .HasOne(x => x.userProfile)
                .WithOne(x => x.SysIdentityUser)
                .HasForeignKey<UserProfile>(x => x.SysIdentityId);
        }
    }

  
}
