using BusinessLayer.Interfaces;
using ModelLayer.Models;
using RepositoryLayer.Entities;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Services
{
    public class ReviewBusiness : IReviewBusiness
    {
        private readonly IReviewRepo reviewRepo;

        public ReviewBusiness (IReviewRepo reviewRepo)
        {
            this.reviewRepo = reviewRepo;
        }

        public ReviewEntity RecordReview(ReviewModel reviewModel)
        {
            return reviewRepo.RecordReview(reviewModel);
        }
    }
}