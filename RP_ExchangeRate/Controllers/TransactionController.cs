using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FYP_ExchangeRate.Models;
using FYPExchangeRateModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.IO;

namespace FYP_ExchangeRate.Controllers
{
    public class TransactionController : Controller
    {
        public FYPExchangeRateContext dbContext;
        public List<ExchangeRate> exchangerate;

        public TransactionController()
        {
            dbContext = new FYPExchangeRateContext();          
        }

        public IActionResult ManageTransactions()
        {
            ViewData["Title"] = "Manage Transactions";
            exchangerate = dbContext.SelectAllExchangeRate();
            ViewBag.Countries = GetCountrySelectList();
            ViewBag.ExchangeRateList = GetExchangeRateList();
            return View(GetTransactionList());
        }
//Sort transactions
        public IActionResult SortTransactions(TransactionModels model)
        {
            var result = GetTransactionList();
            if (result != null && result.Count() > 0 && model.CreatedDate.HasValue)
            {
                DateTime StartDate = new DateTime(model.CreatedDate.Value.Year, model.CreatedDate.Value.Month, 1);
                DateTime EndDate = StartDate.AddMonths(1).AddDays(-1);
                result = result.Where(t => t.CreatedDate.Value >= StartDate && t.CreatedDate <= EndDate).ToList();
            }
            if (model.GenerateExcel.HasValue && model.GenerateExcel.Value)
            {
                result = result.OrderBy(t => t.CreatedDate).ToList();
                string FileLocation  = @"C:\Downloads\Reports\";
                if (!Directory.Exists(FileLocation))
                {
                    Directory.CreateDirectory(FileLocation);
                }
                string TitleSheet = model.CreatedDate.HasValue ? string.Format("Transaction Report of {0}-{1}", model.CreatedDate.Value.Month, model.CreatedDate.Value.Year) : "All Transaction Reports";
                HSSFWorkbook NewReportWorkBook = new HSSFWorkbook();
                HSSFFont FontApplied = (HSSFFont)NewReportWorkBook.CreateFont();
                FontApplied.FontName = "Tahoma";
                FontApplied.IsBold = true;

                HSSFCellStyle borderedCellStyle = (HSSFCellStyle)NewReportWorkBook.CreateCellStyle();
                borderedCellStyle.SetFont(FontApplied);
                borderedCellStyle.BorderLeft = BorderStyle.Medium;
                borderedCellStyle.BorderTop = BorderStyle.Medium;
                borderedCellStyle.BorderRight = BorderStyle.Medium;
                borderedCellStyle.BorderBottom = BorderStyle.Medium;
                borderedCellStyle.VerticalAlignment = VerticalAlignment.Center;

                ISheet FirstSheet = NewReportWorkBook.CreateSheet(TitleSheet);

                //WorkSheet Headers
                IRow HeaderRow = FirstSheet.CreateRow(0);
                CreateCell(HeaderRow, 0, "TransactionID", borderedCellStyle);
                CreateCell(HeaderRow, 1, "Transaction Status", borderedCellStyle);
                CreateCell(HeaderRow, 2, "Transaction Date", borderedCellStyle);
                CreateCell(HeaderRow, 3, "Customer ID", borderedCellStyle);
                CreateCell(HeaderRow, 4, "Customer Name", borderedCellStyle);
                CreateCell(HeaderRow, 5, "Payment Method", borderedCellStyle);
                CreateCell(HeaderRow, 6, "Rate & Country From", borderedCellStyle);
                CreateCell(HeaderRow, 7, "Rate & Country To", borderedCellStyle);
                CreateCell(HeaderRow, 8, "Amount Exchanged", borderedCellStyle);
                CreateCell(HeaderRow, 9, "Total Transaction Amount", borderedCellStyle);

                //
                //WorkSheet Contents
                for (int a = 0; a < result.Count(); a++)
                {
                    IRow CurrentRow = FirstSheet.CreateRow(a+1);
                    CreateCell(CurrentRow, 0, result[a].TransactionID.ToString(), borderedCellStyle);
                    CreateCell(CurrentRow, 1, result[a].IsConfirmed.Value ? "Confirmed" : "Not Confirmed", borderedCellStyle);
                    CreateCell(CurrentRow, 2, result[a].CreatedDate.Value.ToString("dd-MMM-yyyy"), borderedCellStyle);
                    CreateCell(CurrentRow, 3, result[a].CustomerID.ToString(), borderedCellStyle);
                    CreateCell(CurrentRow, 4, result[a].CustomerName, borderedCellStyle);
                    CreateCell(CurrentRow, 5, result[a].PaymentMethod, borderedCellStyle);
                    CreateCell(CurrentRow, 6, result[a].CountryFrom + ": " + result[a].FromCountryRate.ToString(), borderedCellStyle);
                    CreateCell(CurrentRow, 7, result[a].CountryTo + ": " + result[a].ToCountryRate.ToString(), borderedCellStyle);
                    CreateCell(CurrentRow, 8, result[a].InputMoneyAmount.ToString(), borderedCellStyle);
                    CreateCell(CurrentRow, 9, result[a].TotalCostAmount.ToString(), borderedCellStyle);
                }
                //
                IRow LastRow = FirstSheet.CreateRow(result.Count() + 1);
                CreateCell(LastRow, 8, "Total Amount", borderedCellStyle);
                CreateCell(LastRow, 9, result.Sum(t => t.TotalCostAmount).ToString(), borderedCellStyle);

                int lastColumNum = FirstSheet.GetRow(0).LastCellNum;
                for (int i = 0; i <= lastColumNum; i++)
                {
                    FirstSheet.AutoSizeColumn(i);
                    GC.Collect();
                }

                using (var fileData = new FileStream(FileLocation + string.Format("CallReport_{0}.xls",DateTime.Now.ToString("ddMMyyyyHHmmmss")), FileMode.Create))
                {
                    NewReportWorkBook.Write(fileData);
                }

                return Json(new { Message = "Generate Succesfully" });
               
            }
            else
            {
                return PartialView("TransactionHistory", result);
            }
        }

        private void CreateCell(IRow CurrentRow, int CellIndex, string Value, HSSFCellStyle Style)
        {
            ICell Cell = CurrentRow.CreateCell(CellIndex);
            Cell.SetCellValue(Value);
            Cell.CellStyle = Style;
        }

        //Submit transaction request
        public IActionResult InsertTransactions(TransactionModels model)
        {
            try
            {
                var updateResult = dbContext.InsertNewTransactions(new Transaction { 
                     FromCountryID = model.FromCountryID,
                     FromCountryRate = model.FromCountryRate,
                     ToCountryID = model.ToCountryID,
                     ToCountryRate = model.ToCountryRate,
                     CustomerID = model.CustomerID,
                     PaymentMethod  = model.PaymentMethod,
                     InputMoneyAmount = model.InputMoneyAmount,
                     TotalCostAmount = model.TotalCostAmount,
                     Remarks = model.Remarks
                });
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessages = ex.Message;
            }
            finally
            {
                exchangerate = dbContext.SelectAllExchangeRate();
                ViewBag.Countries = GetCountrySelectList();
                ViewBag.ExchangeRateList = GetExchangeRateList();
            }
            return View("ManageTransactions",GetTransactionList());
        }
//Delete transaction
        public IActionResult DeleteTransactions(TransactionModels model)
        {
            try
            {
                var updateResult = dbContext.DeleteTransactions(new Transaction
                {
                    TransactionID = model.TransactionID
                });
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessages = ex.Message;
            }
            finally
            {
                exchangerate = dbContext.SelectAllExchangeRate();
                ViewBag.Countries = GetCountrySelectList();
                ViewBag.ExchangeRateList = GetExchangeRateList();
            }
            return View("ManageTransactions", GetTransactionList());
        }
        //Confirm Transaction
        public IActionResult ConfirmTransactions(TransactionModels model)
        {
            try
            {
                var TransactionConfirm = dbContext.SelectAllTransactions().Where(t => t.TransactionID == model.TransactionID).FirstOrDefault();
                var toCountry = dbContext.SelectAllCountries().Where(t => t.CountryID == TransactionConfirm.ToCountryID).FirstOrDefault();
                if (toCountry.CurrentCurrencyRate < TransactionConfirm.TotalCostAmount)
                {
                    throw new Exception(string.Format("The Remaining Amount of Currency {0} is insufficient", toCountry.CountryName));
                }
                var updateResult = dbContext.ConfirmTransactions(new Transaction
                {
                    TransactionID = model.TransactionID,
                    IsConfirmed = true
                });;

                updateResult = dbContext.UpdateRemainingAmount(TransactionConfirm, true);
                updateResult = dbContext.UpdateRemainingAmount(TransactionConfirm, false);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessages = ex.Message;
            }
            finally
            {
                exchangerate = dbContext.SelectAllExchangeRate();
                ViewBag.Countries = GetCountrySelectList();
                ViewBag.ExchangeRateList = GetExchangeRateList();
            }
            return View("ManageTransactions", GetTransactionList());
        }

        public SelectList GetCountrySelectList()
        {
            SelectList CountryModel = null;
            if (exchangerate != null && exchangerate.Count() > 0)
            {
                var selectItems = new List<SelectListItem>();
                foreach (var cty in exchangerate)
                {
                    if (!(selectItems.Where(t => t.Value == cty.FromCountryID.ToString()).ToList().Count >0))
                    {
                        selectItems.Add(new SelectListItem
                        {
                            Value = cty.FromCountryID.ToString(),
                            Text = cty.CountryFrom
                        });
                    }
                }
                CountryModel = new SelectList(selectItems, "Value", "Text");
            }
            return CountryModel;
        }

        public List<ExchangeRateModel> GetExchangeRateList()
        {
            List<ExchangeRateModel> modelResult = new List<ExchangeRateModel>();
            if (exchangerate != null && exchangerate.Count() > 0)
            {
                foreach (var cty in exchangerate)
                {
                    modelResult.Add(new ExchangeRateModel
                    {
                        ExchangeRateID = cty.ExchangeRateID,
                        ToCountryName = cty.CountryTo,
                        FromCountryName = cty.CountryFrom,
                        FromCountryID = cty.FromCountryID.HasValue ? cty.FromCountryID.Value : 0,
                        ToCountryID = cty.ToCountryID.HasValue ? cty.ToCountryID.Value : 0,
                        ToCountryRate = cty.ToCountryRate.HasValue ? cty.ToCountryRate.Value : 0,
                        FromCountryRate = cty.FromCountryRate.HasValue ? cty.FromCountryRate.Value : 0,                       
                        IsActiveRemark = cty.IsActive.Value ? "Active" : "Deactive",
                        CreatedDate = cty.CreatedDate.Value,
                        ModifiedDate = cty.ModifiedDate.Value,
                        Remarks = cty.Remarks,
                        IsActive = cty.IsActive.Value
                    });
                }
            }
            return modelResult;
        }

        public List<TransactionModels> GetTransactionList()
        {
            List<TransactionModels> modelResult = new List<TransactionModels>();
            string UserType = HttpContext.Session.GetString("UserType");
            if (!string.IsNullOrEmpty(UserType))
            {
                var list = dbContext.SelectAllTransactions();
                if (list != null && list.Count() > 0)
                {
                    switch(UserType)
                    {
                        case "1":
                        case "2":
                            list = list.Where(t => t.IsActive.HasValue && t.IsActive.Value).ToList();
                            break;
                        case "3":
                            int userID = int.Parse(HttpContext.Session.GetString("UserID"));
                            list = list.Where(t => t.CustomerID == userID && t.IsActive.HasValue && t.IsActive.Value).ToList();
                            break;
                    }
                    foreach (var cty in list)
                    {
                        modelResult.Add(new TransactionModels
                        {
                            TransactionID = cty.TransactionID,
                            FromCountryID = cty.FromCountryID.HasValue ? cty.FromCountryID.Value : 0,
                            FromCountryRate = cty.FromCountryRate.HasValue ? cty.FromCountryRate.Value : 0,
                            ToCountryID = cty.ToCountryID.HasValue ? cty.ToCountryID.Value : 0,
                            ToCountryRate = cty.ToCountryRate.HasValue ? cty.ToCountryRate.Value : 0,
                            CountryFrom = cty.CountryFrom,
                            CountryTo = cty.CountryTo,
                            PaymentMethod = cty.PaymentMethod,
                            InputMoneyAmount = cty.InputMoneyAmount,
                            TotalCostAmount = cty.TotalCostAmount,
                            CustomerID = cty.CustomerID.HasValue ? cty.CustomerID :0,
                            CustomerName = cty.CustomerName,
                            IsActiveRemark = cty.IsActive.Value ? "Active" : "Deactive",
                            CreatedDate = cty.CreatedDate.Value,
                            ModifiedDate = cty.ModifiedDate.Value,
                            Remarks = cty.Remarks,
                            IsActive = cty.IsActive.Value,
                            IsConfirmed = cty.IsConfirmed.Value
                        });
                    }
                }
            }
            return modelResult;
        }
    }
}