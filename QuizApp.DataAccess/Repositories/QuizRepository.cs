﻿using System.Collections.Generic;
using System.Linq;
using QuizApp.DataAccess.Entities;
using QuizApp.DataAccess.Repositories.Interfaces;

namespace QuizApp.DataAccess.Repositories
{
    public class QuizRepository : BaseRepository<QuizEntity>, IQuizRepository
    {
        private const string StoragePath = "QuizStorage.json";
        public QuizRepository() : base(StoragePath)
        {
        }
        
        public override QuizEntity Find(int id)
        {
            FetchItems();
            return Items.FirstOrDefault(a => a.Id == id);
        }

        public override void Delete(int id)
        {
            FetchItems();
            var account = Items.FirstOrDefault(a => a.Id == id);
            if (account == null)
                return;
            Items.Remove(account);
        }
    }
}