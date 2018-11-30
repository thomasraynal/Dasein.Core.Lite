using Dasein.Core.Lite.Shared;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dasein.Core.Lite
{
    public class HubContextHolder<TDto> : IHubContextHolder<TDto>, ICanLog
    {
        private static readonly Func<TDto, bool> _all = (p) => true;

        private readonly ConcurrentDictionary<string, List<String>> _connectedIds;
        private readonly ConcurrentDictionary<string, Func<TDto, bool>> _groups;

        public HubContextHolder()
        {
            _connectedIds = new ConcurrentDictionary<string, List<String>>();
            _groups = new ConcurrentDictionary<string, Func<TDto, bool>>();
        }

        public Func<TDto, bool> AcceptAll
        {
            get
            {
                return _all;
            }
        }

        public IDictionary<string, Func<TDto, bool>> Groups
        {
            get
            {
                return _groups;
            }
        }

        public void UnRegisterUserId(string hub, string userId)
        {
            if (_connectedIds.ContainsKey(hub) && _connectedIds[hub].Contains(userId))
            {
                _connectedIds[hub].Remove(userId);
            }

            _groups.Remove(userId, out var query);

            PublishState(hub);
        }

        public void RegisterUserId(string hub, string userId, Func<TDto, bool> query)
        {
            if (_connectedIds.ContainsKey(hub) && !_connectedIds[hub].Contains(userId))
            {
                _connectedIds[hub].Add(userId);
            }
            else
            {
                _connectedIds[hub] = new List<string>() { userId };
            }

            _groups[userId] = query;

            PublishState(hub);
        }

        private void PublishState(string hub)
        {
            this.LogInformation($"[{hub}] - [{_connectedIds[hub].Count()}] active(s)");
        }
    }
}
