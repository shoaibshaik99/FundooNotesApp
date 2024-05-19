using BusinessLayer.Interfaces;
using BusinessLayer.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Models;

namespace FundooNotesApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewBusiness reviewBusiness;

        public ReviewController (IReviewBusiness reviewBusiness)
        {
            this.reviewBusiness = reviewBusiness;
        }

        [HttpPost("RecordReview")]
        public IActionResult RecordReview(ReviewModel reviewModel)
        {
            var response = reviewBusiness.RecordReview(reviewModel);
            if (response != null)
            {
                return Ok(new {IsSuccess = true, Message = "Review recorded successfully", Data = response});
            }
            else
            {
                return BadRequest(new { IsSuccess = false, Message = "Review cannot be recorded!", Data = response });
            }
        }

    }
}
