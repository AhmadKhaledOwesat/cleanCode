﻿namespace DcpTracker.Domain.Entities.Filters
{
    public class PrivilegeFilter : SearchParameters<Privilege>
    {
        public string Name { get; set; }
    }
}
