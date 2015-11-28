using Microsoft.Data.Entity;
using Microsoft.Framework.DependencyInjection;
using System;
using System.Linq;
using Microsoft.Data.Entity.Storage;

namespace sc2iqapi.Models
{
    public static class SampleData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetService<Sc2IqContext>();

            //if(serviceProvider.GetService<IRelationalDatabaseCreator>().Exists())
            //{
                if(!context.Questions.Any())
                {
                    var question1 = new Question()
                    {
                        Q = "Test Question",
                        A1 = "Answer 1",
                        A2 = "Answer 2",
                        A3 = "Answer 3",
                        A4 = "Answer 4",
                        CorrectAnswerIndex = 1,
                        Difficulty = 4,
                        State = QuestionState.Published
                    };

                    context.AddRange(question1);

                    context.SaveChanges();
                }
            //}
        }
    }
}
