using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Context
{
    public class FundooDBContext : DbContext
    {
        public FundooDBContext(DbContextOptions dbContext) : base(dbContext) { }

        public DbSet<UserEntity> Users { get; set; }

        public DbSet<ReviewEntity> Reviews { get; set; }

        public DbSet<NoteEntity> Notes { get; set; }

        public DbSet<LabelEntity> Labels { get; set; }

        public DbSet<LabelsLogEntity> LabelsLogs { get; set; }

        public DbSet<CollaboratorEntity> Collaborators { get; set; }

    }
}