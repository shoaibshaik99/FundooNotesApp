using ModelLayer.Models;
using RepositoryLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interfaces
{
    public interface IReviewBusiness
    {
        public ReviewEntity RecordReview(ReviewModel reviewModel);
    }
}
