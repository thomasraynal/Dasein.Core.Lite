using System;
using System.Collections.Generic;
using System.Text;

namespace Dasein.Core.Lite
{
    public interface IHubContextHolder<TDto>
    {
        Func<TDto, bool> AcceptAll { get; }
        IDictionary<string, Func<TDto, bool>> Groups { get; }
        void RegisterUserId(string hub, string userId, Func<TDto, bool> query);
        void UnRegisterUserId(string hub, string userId);
    }
}
