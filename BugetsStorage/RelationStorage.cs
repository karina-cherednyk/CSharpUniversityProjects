﻿using Budgets.BusinessLayer.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace BudgetsStorage
{
    class RelationStorage<T, U>
        where T: class, IStorable
        where U: class, IStorable
    {
        private static readonly string FolderName = Path.Combine(Environment.GetFolderPath(
                                                               Environment.SpecialFolder.ApplicationData),
                                                               "BudgetsStorage", typeof(T).Name + "-" + typeof(U).Name);
        public RelationStorage()
        {
            if (!Directory.Exists(FolderName))
                Directory.CreateDirectory(FolderName);
        }
        private string FileName(T obj)
        {
            return Path.Combine(FolderName, obj.Guid.ToString("N"));
        }
        public async Task<bool> AddAsync(T obj, U u)
        {
            HashSet<Guid> pairs = await GetSetAsync(obj);
            if (pairs.Contains(u.Guid))
                return false;

            pairs.Add(u.Guid);
            string stringObj = JsonSerializer.Serialize(pairs.ToList());
            using StreamWriter sw = new StreamWriter(FileName(obj), false);
            await sw.WriteAsync(stringObj);

            return true;
        }
        private async Task<HashSet<Guid>> GetSetAsync(T obj) => (await GetAsync(obj)).ToHashSet();
        public async Task<List<Guid>> GetAsync(T obj)
        {
            string filePath = FileName(obj);

            if (!File.Exists(filePath) || new FileInfo(filePath).Length==0)
                return new ();

            using StreamReader sw = new StreamReader(filePath);
            var stringObj = await sw.ReadToEndAsync();


            List<Guid> us = JsonSerializer.Deserialize<List<Guid>>(stringObj);

            return us;
        }

        public async Task<bool> RemoveAsync(T obj, U u)
        {
            HashSet<Guid> pairs = await GetSetAsync(obj);
            if (!pairs.Contains(u.Guid))
                return false;

            pairs.Remove(u.Guid);
            string stringObj = JsonSerializer.Serialize(pairs.ToList());
            using StreamWriter sw = new StreamWriter(FileName(obj), false);
            await sw.WriteAsync(stringObj);

            return true;
        }
    }
}
