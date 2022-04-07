﻿using Appdoon.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Appdoon.Application.Interfaces
{
	public interface IDatabaseContext
	{
		DbSet<User> Users { get; set; } 
		int SaveChanges(bool acceptAllChangesOnSuccess);
		int SaveChanges();

		Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default);
		Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
	}
}