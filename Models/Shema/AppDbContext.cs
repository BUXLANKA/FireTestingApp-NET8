using Microsoft.EntityFrameworkCore;

namespace FireTestingApp_net8.Models.Shema;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Answer> Answers { get; set; }

    public virtual DbSet<Question> Questions { get; set; }

    public virtual DbSet<Result> Results { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Teststatus> Teststatuses { get; set; }

    public virtual DbSet<Ticket> Tickets { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Useranswer> Useranswers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=26.169.2.131:5432;Database=mchsDB;Username=postgres;Password=123");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Answer>(entity =>
        {
            entity.HasKey(e => e.Answerid).HasName("pk__answers__d4825024c2cfa766");

            entity.HasOne(d => d.Question).WithMany(p => p.Answers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk__answers__questio__412eb0b6");
        });

        modelBuilder.Entity<Question>(entity =>
        {
            entity.HasKey(e => e.Questionid).HasName("pk__question__0dc06f8cdc57268c");
        });

        modelBuilder.Entity<Result>(entity =>
        {
            entity.HasKey(e => e.Resultid).HasName("pk__results__976902281923d908");

            entity.Property(e => e.Testdate).HasDefaultValueSql("now()");

            entity.HasOne(d => d.Status).WithMany(p => p.Results)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk__results__statusi__45f365d3");

            entity.HasOne(d => d.User).WithMany(p => p.Results)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk__results__userid__440b1d61");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Roleid).HasName("pk__roles__8aface3acd8160d4");
        });

        modelBuilder.Entity<Teststatus>(entity =>
        {
            entity.HasKey(e => e.Statusid).HasName("pk__teststat__c8ee204352c6cf88");
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(e => e.Ticketid).HasName("pk__tickets__712cc62741682771");

            entity.HasOne(d => d.Fromuser).WithMany(p => p.Tickets)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk__tickets__fromuse__4d94879b");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Userid).HasName("pk__users__1788ccacf087a1a2");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk__users__roleid__3c69fb99");
        });

        modelBuilder.Entity<Useranswer>(entity =>
        {
            entity.HasKey(e => e.Useranswerid).HasName("pk__useransw__47ce235f6983c959");

            entity.HasOne(d => d.Answer).WithMany(p => p.Useranswers).HasConstraintName("fk__useranswe__answe__4ab81af0");

            entity.HasOne(d => d.Question).WithMany(p => p.Useranswers).HasConstraintName("fk__useranswe__quest__49c3f6b7");

            entity.HasOne(d => d.User).WithMany(p => p.Useranswers).HasConstraintName("fk__useranswe__useri__48cfd27e");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
