using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Configator
{
    public static class Configator
    {
        static readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings {ContractResolver = new PrivateContractResolver()};

        static readonly UpdatableConfigRegister _register = new UpdatableConfigRegister();
        
        public static T Get<T>(string configPath = null) where T : new() => Get(new T(), configPath);

        public static T Get<T>(T obj, string configPath = null)
        {
            var type = obj.GetType();
            
            if (configPath == null) 
                configPath = GetAttributePath(obj);

            if (!File.Exists(configPath))
            {
                Set(obj, configPath);
                return obj;
            }
            
            var configObj = JsonConvert.DeserializeObject( File.ReadAllText(configPath), type, JsonSerializerSettings);

            foreach (var info in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
                info.SetValue(obj, info.GetValue(configObj));

            _register.Register(obj, configPath);
            
            return obj;
        }

        public static void Set<T>(T obj, string configPath = null)
        {
            if (configPath == null) 
                configPath = GetAttributePath(obj);

            File.WriteAllText(configPath, JsonConvert.SerializeObject(obj, JsonSerializerSettings));
        }

        static string GetAttributePath<T>(T obj)
        {
            var type = typeof(T);
            var attribute = type.GetCustomAttributes(true).FirstOrDefault(a => a is ConfigatorAttribute) as ConfigatorAttribute;
            if(attribute == null) throw new ArgumentException($"No ConfigatorAttribute found for type {type.FullName}");
            return attribute.ConfigPath;
        }
    }

    internal class PrivateContractResolver : DefaultContractResolver
    {
        protected override List<MemberInfo> GetSerializableMembers(Type objectType) =>
            objectType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Concat<MemberInfo>(objectType.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Where(propInfo => propInfo.CanWrite))
                .ToList();

        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization) => 
            base.CreateProperties(type, MemberSerialization.Fields);
    }
}