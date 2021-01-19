using System.Collections.Generic;
using System.Linq;
using QuizApp.DataAccess.Entities;
using QuizApp.DataAccess.Repositories.Interfaces;

namespace QuizApp.DataAccess.Repositories
{
    public class QuizRepository : BaseRepository<QuizEntity>, IQuizRepository
    {
        private const string StoragePath = "QuizStorage.json";
        private static QuizRepository _instance;

        private QuizRepository(string path) : base(path)
        {
            
        }
        public static QuizRepository GetInstance()
        {
            return _instance ??= new QuizRepository(StoragePath);
        }
        
        public override QuizEntity Find(int id)
        {
            FetchItems();
            return Items.FirstOrDefault(a => a.Id == id);
        }

        public override bool Delete(int id)
        {
            FetchItems();
            var account = Items.FirstOrDefault(a => a.Id == id);
            if (account == null)
                return false;
            Items.Remove(account);
            SaveChanges();
            return true;
        }

        public bool Add(int ownerId, string name, string category)
        {
            if (string.IsNullOrWhiteSpace(name))
                return false;
            if (string.IsNullOrWhiteSpace(category))
            {
                category = "Others";
            }
            
            FetchItems();
            
            QuizEntity existingQuiz = Items.FirstOrDefault(q => q.Name.Equals(name.Trim()));
            if (existingQuiz != null)
            {
                return false;
            }
            
            int quizId = 0;

            if (Items.Count != 0)
            {
                quizId = Items.Max(a => a.Id) + 1;
            }

            QuizEntity newQuiz = new QuizEntity
            {
                Id = quizId,
                OwnerId = ownerId,
                Name = name.Trim(),
                Category = category.Trim(),
                Questions = new List<QuestionEntity>()
            };
            
            Items.Add(newQuiz);
            SaveChanges();
            return true;
        }

        public IEnumerable<QuizEntity> FindByOwner(int ownerId)
        {
            FetchItems();
            return Items.Where(q => q.OwnerId == ownerId).ToList();
        }

        public bool Update(QuizEntity quizEntity)
        {
            FetchItems();
            var quiz = Items.FirstOrDefault(q => q.Id == quizEntity.Id);
            if (quiz == null)
                return false;

            Items.Remove(quiz);
            Items.Add(quizEntity);
            SaveChanges();
            return true;
        }
        
    }
}