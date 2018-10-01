using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuthWebApi.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().HasData(
                    new User
                    {
                        Id = "fa435b98-bd28-4a20-8b4a-62b124d9841b",
                        UserName = "test",
                        NormalizedUserName = "TEST",
                        Email = "a@a.a",
                        NormalizedEmail = "A@A.A",
                        PasswordHash = "AQAAAAEAACcQAAAAEODZFtx31yVlQlAo6GcTs2dIyi/Dcch0/uqv27PvT/xXzy5+JAZEMVS5SvM13yrMdQ=="
                    },
                    new User
                    {
                        Id = "73dcc714-8fbc-41ac-a6af-756986ade684",
                        UserName = "test2",
                        NormalizedUserName = "TEST2",
                        Email = "a@a.a",
                        NormalizedEmail = "A@A.A",
                        PasswordHash = "AQAAAAEAACcQAAAAEODZFtx31yVlQlAo6GcTs2dIyi/Dcch0/uqv27PvT/xXzy5+JAZEMVS5SvM13yrMdQ=="
                    }
            );

            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = "fb435b98-bd28-4a20-ab4a-62b124d9841b",
                    Name = "admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole
                {
                    Id = "fb467b98-bd28-6720-ab4a-645124d9834b",
                    Name = "user",
                    NormalizedName = "USER"
                }
            );

            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {

                    UserId = "fa435b98-bd28-4a20-8b4a-62b124d9841b",
                    RoleId = "fb435b98-bd28-4a20-ab4a-62b124d9841b"
                },
                new IdentityUserRole<string>
                {
                    UserId = "fa435b98-bd28-4a20-8b4a-62b124d9841b",
                    RoleId = "fb467b98-bd28-6720-ab4a-645124d9834b"
                },
                new IdentityUserRole<string>
                {
                    UserId = "73dcc714-8fbc-41ac-a6af-756986ade684",
                    RoleId = "fb467b98-bd28-6720-ab4a-645124d9834b"
                }
            );
        }
    }
}