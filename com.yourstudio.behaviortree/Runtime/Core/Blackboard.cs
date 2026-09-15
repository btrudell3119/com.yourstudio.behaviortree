using System.Collections.Generic;

namespace BehaviorTree
{
    /// <summary>
    /// Generic key/value store shared by every node in a tree.
    /// Keeps the tree decoupled from any specific game's data types,
    /// which is what makes the same tree logic reusable across projects.
    /// </summary>
    public class Blackboard
    {
        private readonly Dictionary<string, object> _data = new Dictionary<string, object>();

        public void Set<T>(string key, T value) => _data[key] = value;

        public T Get<T>(string key, T fallback = default)
        {
            if (_data.TryGetValue(key, out var value) && value is T typed)
                return typed;
            return fallback;
        }

        public bool TryGet<T>(string key, out T value)
        {
            if (_data.TryGetValue(key, out var raw) && raw is T typed)
            {
                value = typed;
                return true;
            }
            value = default;
            return false;
        }

        public bool Has(string key) => _data.ContainsKey(key);
        public void Remove(string key) => _data.Remove(key);
        public void Clear() => _data.Clear();
    }
}
