﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SolarEdge.Monitoring.Demo.Database;
using SolarEdge.Monitoring.Demo.Models;
using SolarEdge.Monitoring.Demo.Services.Repositories.Base;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System;

namespace SolarEdge.Monitoring.Demo.Services.Repositories;

public class EnergyDetailsRepository(
  DataContext appDbContext,
  ILogger<EnergyDetailsRepository> logger)
  : BaseRepository<EnergyDetails>(appDbContext), IEnergyDetailsRepository
{
  public async Task<EnergyDetails> GetAsync(DateTime date, CancellationToken cancellationToken = default)
  {
    logger.LogDebug(nameof(GetAsync));
    return await GetSingleAsync(c => c.Time.Date == date.Date, cancellationToken: cancellationToken).ConfigureAwait(false);
  }

  public override async Task<EnergyDetails> GetSingleAsync(Expression<Func<EnergyDetails, bool>> predicate, bool disableTracking = true, CancellationToken cancellationToken = default)
  {
    logger.LogDebug(nameof(GetSingleAsync));
    IQueryable<EnergyDetails> query = DatabaseSet;
    if (disableTracking)
    {
      query = query.AsNoTracking();
    }
    var result = await query
      .SingleOrDefaultAsync(predicate, cancellationToken).ConfigureAwait(false);
    return result;
  }

  public override async Task<IList<EnergyDetails>> GetAllAsync(bool disableTracking = true, CancellationToken cancellationToken = default)
  {
    logger.LogDebug(nameof(GetAllAsync));
    IQueryable<EnergyDetails> query = DatabaseSet;
    if (disableTracking)
    {
      query = query.AsNoTracking();
    }
    var result = await query
      .ToListAsync(cancellationToken).ConfigureAwait(false);
    return result;
  }
}
