using UserControl.Core.QueryFilters;
using System;

namespace UserControl.Infrastructure.Interfaces
{
    public interface IUriService
    {
        Uri GetPostPaginationUri(PostQueryFilter filter, string actionUrl);
    }
}