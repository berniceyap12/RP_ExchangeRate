using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FYP_ExchangeRate.Models;
using FYPExchangeRateModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FYP_ExchangeRate.Controllers
{

//Ashitha - Submit Enquiry
    public class EnquiryController : Controller
    {
        private readonly ILogger<EnquiryController> _logger;
        public FYPExchangeRateContext dbContext;

        public EnquiryController(ILogger<EnquiryController> logger)
        {
            _logger = logger;
            dbContext = new FYPExchangeRateContext();
        }

        public IActionResult ViewEnquiries()
        {
            ViewData["Title"] = "View your enquiries";
            var result = GetEnquiry();
            return View(result);
        }
        [HttpPost]

        public IActionResult AddingEnquiries(EnquiryModel model)
        {
            try
            {
                dbContext.InsertEnquiry(model.EnquirySubject,model.MessageID,model.EnquiryMessage,model.CustomerID,model.TransactionID,string.Empty, model.CreatedBy);
            }
            catch(Exception ex)
            {
                ViewBag.ErrorMessages = ex.Message;
            }
            if (model.RefreshPage.HasValue && model.RefreshPage.Value)
            {
                return View("ViewEnquiries", GetEnquiry());
            }
            else
            {
                return Json(new { AddingRecord = true });
            }
        }

        public IActionResult ViewMessage(EnquiryModel model)
        {
            List<EnquiryModel> result = new List<EnquiryModel>();
            try
            {
                int MessageID = int.Parse(HttpContext.Request.Query["MessageID"]);
                var query = dbContext.SelectMessage(MessageID);
                if (query != null && query.Count() > 0)
                {
                    foreach (var obj in query)
                    {
                        result.Add(new EnquiryModel
                        {
                            CreatedDate = obj.CreatedDate,
                            CustomerID = obj.CustomerID,
                            EnquiryMessage = obj.EnquiryMessage,
                            MessageID = obj.MessageID,
                            EnquirySubject = obj.EnquirySubject,
                            TransactionID = obj.TransactionID,
                            CustomerName = obj.CustomerName,
                            CreatedName = obj.CreatedName
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessages = ex.Message;
            }

            result = result.OrderBy(t => t.CreatedDate).ToList();
            return PartialView("ManageMessages",result);
        }


        public List<EnquiryModel> GetEnquiry()
        {
            List<EnquiryModel> result = new List<EnquiryModel>();
            string UserType = HttpContext.Session.GetString("UserType");
            int? UserID = null;
            try
            {
                if (!string.IsNullOrEmpty(UserType))
                {
                    if (UserType == "3")
                    {
                        UserID = int.Parse(HttpContext.Session.GetString("UserID"));
                    }

                    var query = dbContext.SelectEnquiries(UserID);
                    if (query != null && query.Count() > 0)
                    {
                        foreach (var obj in query)
                        {
                            result.Add(new EnquiryModel
                            {
                                CustomerID = obj.CustomerID,
                                EnquiryMessage = obj.EnquiryMessage,
                                MessageID = obj.MessageID,
                                EnquirySubject = obj.EnquirySubject,
                                TransactionID = obj.TransactionID,
                                CustomerName = obj.CustomerName,
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessages = ex.Message;
            }
            return result;
        }
    }
}