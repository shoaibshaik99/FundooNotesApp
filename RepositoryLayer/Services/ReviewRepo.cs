using ModelLayer.Models;
using RepositoryLayer.Context;
using RepositoryLayer.Entities;
using RepositoryLayer.Interfaces;
//using RepositoryLayer.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepositoryLayer.Services
{
    public class ReviewRepo : IReviewRepo
    {
        private readonly FundooDBContext context;

        public ReviewRepo(FundooDBContext context)
        {
            this.context = context;
        }

        public ReviewEntity RecordReview(ReviewModel reviewModel)
        {
            ReviewEntity reviewEntity = new ReviewEntity();

            reviewEntity.UserName = reviewModel.UserName;
            reviewEntity.Feedback = reviewModel.Feedback;
            reviewEntity.CreationTime = DateTime.Now;

            context.Reviews.Add(reviewEntity);
            context.SaveChanges();

            return reviewEntity;
        }

        public ReviewEntity FetchReview(string username)
        {
            var fetechedReview = context.Reviews.FirstOrDefault(u => u.UserName == username);
            if (fetechedReview != null)
            {
                return fetechedReview;
            }
            else
            {
                return null;
            }
        }
    }
}
