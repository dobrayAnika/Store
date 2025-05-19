using System;
using System.Reflection;

namespace StoreApp.Helpers
{
    public static class ReflectionHelper
    {
        public static void PrintObjectInfo(object obj)
        {
            var type = obj.GetType();
            Console.WriteLine($"Тип объекта: {type.Name}");
            foreach (var prop in type.GetProperties())
            {
                var value = prop.GetValue(obj);
                Console.WriteLine($"  {prop.Name}: {value}");
            }
        }
    }
}