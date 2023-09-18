using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace e_commerceWebSite.Models
{
    public class e_commerceStoreContext : IdentityDbContext<ApplicationUser>
    {
        public virtual DbSet<TbProduct> TbProducts { get; set; }
        public virtual DbSet<TbCategory> TbCategories { get; set; }
        public virtual DbSet<TbCart> TbCarts { get; set; }
        public virtual DbSet<TbCartStatus> TbCartStatuses { get; set; }
        public virtual DbSet<TbCartProduct> TbCartProducts { get; set; }
        public virtual DbSet<TbNotification> TbNotifications { get; set; }



        public e_commerceStoreContext(DbContextOptions<e_commerceStoreContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<TbCategory>(option =>
            {
                option.Property(p => p.InsertionDate).HasDefaultValueSql("GETDATE()");
                option.Property(p => p.ModifiedDate).HasDefaultValueSql("GETDATE()");
                option.Property(P => P.IsDeleted).HasDefaultValue(false);
            });
            modelBuilder.Entity<TbProduct>(option =>
            {
                option.Property(p => p.IsActive).HasDefaultValue(true);
                option.Property(P => P.IsDeleted).HasDefaultValue(false);
                option.Property(p => p.ExipirationDate).IsRequired(false);
            });
            modelBuilder.Entity<TbCart>(option =>
            {
                option.Property(p => p.InsertionData).HasDefaultValueSql("GETDATE()");
            });

            modelBuilder.Entity<TbProduct>().HasQueryFilter(a => !a.IsDeleted && a.IsActive);
            modelBuilder.Entity<TbCategory>().HasQueryFilter(a => !a.IsDeleted);

            modelBuilder.Entity<TbCartProduct>(option =>
            {
                option.Property(p => p.InsertionData).HasDefaultValueSql("GETDATE()");
            });
            modelBuilder.Entity<TbCartProduct>().HasKey(k=> new {k.ProductId ,k.CartId });
            modelBuilder.Entity<TbCartProduct>().HasOne(c=>c.Cart)
                                                .WithMany(cp=>cp.TbCartProducts)
                                                .HasForeignKey(c=>c.CartId)
                                                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TbCartProduct>().HasOne(p => p.Product)
                                                .WithMany(cp => cp.TbCartProducts)
                                                .HasForeignKey(c => c.ProductId)
                                                .OnDelete(DeleteBehavior.Restrict);
            

            // can use identityrole or any table of that created using identity To control this column  
            modelBuilder.Entity<ApplicationUser>(option =>
            {
                option.Property(p => p.IsActive).HasDefaultValue(true);
            });
           
        }
    }
}
