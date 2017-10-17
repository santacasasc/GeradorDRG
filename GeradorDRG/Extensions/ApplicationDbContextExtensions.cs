﻿using GeradorDRG.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeradorDRG.Extensions
{
    public static class ApplicationDbContextExtensions
    {
        public static void Seed(this ApplicationDbContext context)
        {
            // Perform database delete and create
            //context.Database.EnsureDeleted();
            context.Database.Migrate();


            // Save changes and release resources
            context.SaveChanges();
            context.Dispose();
        }


    }
}