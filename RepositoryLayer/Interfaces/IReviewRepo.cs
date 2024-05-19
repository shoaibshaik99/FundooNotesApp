using ModelLayer.Models;
using RepositoryLayer.Context;
using RepositoryLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interfaces
{
    public interface IReviewRepo
    {
        public ReviewEntity RecordReview(ReviewModel reviewModel);
        public ReviewEntity FetchReview(string username);
    }
}
