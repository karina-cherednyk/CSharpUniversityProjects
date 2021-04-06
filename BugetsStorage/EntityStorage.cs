using Budgets.BusinessLayer.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace BugetsStorage
{
    public class EntityStorage<T> where T: class, IStorable
    {
        private static readonly string FolderName =
             Path.Combine(Environment.GetFolderPath(
                 Environment.SpecialFolder.ApplicationData),
                 "BudgetsStorage", typeof(T).Name);
        
        public EntityStorage()
        {
            if (!Directory.Exists(FolderName))
                Directory.CreateDirectory(FolderName);
        }

        public  async Task AddOrUpdateAsync(T obj)
        {
            var stringObj = JsonSerializer.Serialize(obj);

            using (StreamWriter sw = new StreamWriter(Path.Combine(FolderName, obj.Guid.ToString("N")), false))
            {
                await sw.WriteAsync(stringObj);
            }
        }

        public async Task<T> GetAsync(Guid guid)
        {
            string stringObj = null;
            string filePath = Path.Combine(FolderName, guid.ToString("N"));

            if (!File.Exists(filePath))
                return null;

            using (StreamReader sw = new StreamReader(filePath))
            {
                stringObj = await sw.ReadToEndAsync();
            }

            return JsonSerializer.Deserialize<T>(stringObj);
        }

        public async Task<List<T>> GetAllAsync()
        {
            var res = new List<T>();
            foreach (var file in Directory.EnumerateFiles(FolderName))
            {
                string stringObj = null;

                using (StreamReader sw = new StreamReader(file))
                {
                    stringObj = await sw.ReadToEndAsync();
                }

                res.Add(JsonSerializer.Deserialize<T>(stringObj));
            }

            return res;
        }
    }
}
