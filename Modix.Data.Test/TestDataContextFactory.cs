﻿using System;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

using NSubstitute;

namespace Modix.Data.Test
{
    public static class TestDataContextFactory
    {
        public static ModixContext BuildTestDataContext(Action<ModixContext> initializeAction = null)
        {
            var modixContext = Substitute.ForPartsOf<ModixContext>(new DbContextOptionsBuilder<ModixContext>()
                .UseInMemoryDatabase((++_databaseName).ToString())
                .ConfigureWarnings(warnings =>
                {
                    warnings.Ignore(InMemoryEventId.TransactionIgnoredWarning);
                })
                .Options);

            if (!(initializeAction is null))
            {
                initializeAction.Invoke(modixContext);

                modixContext.SaveChanges();

                modixContext.ClearReceivedCalls();
            }

            return modixContext;
        }

        private static int _databaseName;
    }
}
