using DotNet.UI.Models;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DotNet.UI.Data
{
    public interface IApplicationDbContext : IDisposable
    {
        DbSet<Tag> Tag { get; set; }
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        int SaveChanges();
        void Add<TEntity>(TEntity entity) where TEntity : class;
        void Update<TEntity>(TEntity entity) where TEntity : class;
        void Delete<TEntity>(TEntity entity) where TEntity : class;
    }

    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>, IApplicationDbContext
    {

        public ApplicationDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {

        }
        public virtual DbSet<Tag> Tag { get; set; }

        public void Add<TEntity>(TEntity entity) where TEntity : class
        {
            Entry(entity).State = EntityState.Added;
        }

        public void Update<TEntity>(TEntity entity) where TEntity : class
        {
            Entry(entity).State = EntityState.Modified;
        }

        public void Delete<TEntity>(TEntity entity) where TEntity : class
        {
            Entry(entity).State = EntityState.Deleted;
        }

        public override int SaveChanges()
        {
            var entities = (from entry in ChangeTracker.Entries()
                            where entry.State == EntityState.Modified ||
                                entry.State == EntityState.Added
                            select entry.Entity);

            var validationResults = new List<ValidationResult>();
            foreach (var entity in entities)
            {
                if (!Validator.TryValidateObject(entity, new ValidationContext(entity), validationResults))
                {
                    var errorMessages = validationResults.Select(x => x.ErrorMessage);
                    var fullErrorMessage = string.Join("; ", errorMessages);
                    var exceptionMessage = string.Concat(" The validation errors are: ", fullErrorMessage);

                    throw new ValidationException(exceptionMessage);
                }
            }
            return base.SaveChanges();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<Tag>().HasData(
                 new Tag
                 {
                     Id = 1,
                     Key = "Colesterol Alto",
                     Value = @"O nível de colesterol alto está diretamente ligado ao consumo excessivo de alimentos gordurosos. 
                                    Esse fator é muito prejudicial à saúde, pode levar ao infarto, e ainda aumenta o risco para o surgimento de doenças cardiovasculares.
                                    Segundo levantamento feito pelo IBGE, pelo menos 12,5% dos brasileiros, ou seja, 18,4 milhões de pessoas no país, 
                                    já foram diagnosticas com colesterol Alto."
                 },
                new Tag
                {
                    Id = 2,
                    Key = "DPOC (Doença pulmonar obstrutiva crônica)",
                    Value = @"Também chamada de enfisema ou bronquite crônica, a doença pulmonar obstrutiva crônica,
                                    é uma doença que causa dificuldades respiratórias pois provoca inflamação nos brônquios. 
                                    Também pode provocar tosse e catarro.
                                    Ela geralmente é ocasionada devido a constante inalação de fumaça ou gases que prejudicam a saúde, 
                                    em função disso é muito comum entre os fumantes."
                },
                new Tag
                {
                    Id = 3,
                    Key = "Hipertensão",
                    Value = @"Hipertensão ou simplesmente pressão alta como é popularmente chamada, é uma doença que contrai os vasos sanguíneos, 
                                    forçando assim o coração a se esforçar mais em sua função.
                                    Os sintomas aparecem apenas quando a doença já prejudicou o organismo, incluem: dor de cabeça, tontura e mal-estar. 
                                    A hipertensão é capaz de desencadear vários outros problemas como doenças cardiovasculares, colesterol elevado, 
                                    infarto e derrame. Ao decorrer da pesquisa 31,3 milhões de pessoas afirmaram terem sido diagnosticadas com a doença."
                }
            );

        }

    }
}
