using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Castle.Core.Logging;
using iqrasys.api.Models;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace iqrasys.api.Data
{
    public class DataContextSeed
    {
        public static  void SeedData(UserManager<User> userManager, DataContext context)
        {

            if (!userManager.Users.Any())
            {
                var metaUser = new User
                {
                    UserName = "meta",
                    Email = "meta@mail.com"
                };


                userManager.CreateAsync(metaUser, "password").Wait();

                var result = userManager.CreateAsync(metaUser, "password").Result;
            }

            if (!context.Solutions.Any())
            {
                var solutionsData = System.IO.File.ReadAllText("Data/Solutions.json");
                var solutions = JsonConvert.DeserializeObject<List<Solution>>(solutionsData);


                foreach (var solution in solutions)
                {
                    context.Solutions.Add(solution);
                }
            }
        }
    }
}