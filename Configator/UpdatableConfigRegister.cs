using System;
using System.Collections.Generic;
using System.IO;

namespace Configator
{
    public class UpdatableConfigRegister
    {
        Dictionary<string, FileSystemWatcher> _watchers = new Dictionary<string, FileSystemWatcher>();
        Dictionary<string, HashSet<object>> _configables = new Dictionary<string, HashSet<object>>();
        
        public void Register(object obj, string path)
        {
            if (!path.Contains("/") && !path.Contains("\\"))
                path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);

            var dirPath = Path.GetDirectoryName(path);
            
            if (!_watchers.ContainsKey(dirPath))
            {
                var watcher = new FileSystemWatcher(dirPath)
                {
                    NotifyFilter = NotifyFilters.LastWrite,
                    EnableRaisingEvents = true,
                };
                watcher.Changed += OnConfigFileChanged;
                _watchers[dirPath] = watcher;
                _configables[path] = new HashSet<object>();
            }
            
            _configables[path].Add(obj);
        }

        void OnConfigFileChanged(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine($"Change detected for {e.FullPath}");
            
            if(!_configables.ContainsKey(e.FullPath))
                return;
                
            foreach (var obj in _configables[e.FullPath]) 
                Configator.Get(obj, e.FullPath);
        }

        ~UpdatableConfigRegister()
        {
            foreach (var watcher in _watchers.Values) 
                watcher.Dispose();
        }
    }
}