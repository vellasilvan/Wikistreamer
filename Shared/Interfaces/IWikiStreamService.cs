﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Interfaces
{
    public interface IWikiStreamService
    {
        public Task GetWikiStreamsAsync(CancellationToken cancellationToken = default);
    }
}
