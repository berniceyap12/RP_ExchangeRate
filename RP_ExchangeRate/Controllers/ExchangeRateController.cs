using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FYP_ExchangeRate.Models;
using FYPExchangeRateModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace FYP_ExchangeRate.Controllers
{
    public class ExchangeRateController : Controller
    {
        public FYPExchangeRateContext dbContext;

        public ExchangeRateController()
        {
            dbContext = new FYPExchangeRateContext();
        }

        public IActionResult ManageCountries()
        {
            ViewData["Title"] = "Manage Countries";
            var result = GetCountryList();
            ViewBag.Countries = result;
            return View("ManageCountries",new CountriesModel());
        }

        public IActionResult AddNewCountry(CountriesModel model)
        {
            ViewData["Title"] = "Manage Countries";
            try
            {
                var CurrentCountry = dbContext.SelectAllCountries().Where(t => t.CountryName == model.CountryName).ToList();
                if (CurrentCountry != null && CurrentCountry.Count() >0)
                {
                    throw new Exception("The same country has existed !");
                }
                var updateResult = dbContext.InsertNewCountry(new Country
                {
                    CountryName = model.CountryName,
                    CurrentCurrencyRate = model.CurrentCurrencyRate,
                    Remarks = model.Remarks
                });
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessages = ex.Message;
            }
            finally
            {
                ViewBag.Countries = GetCountryList();
            }
            return View("ManageCountries", new CountriesModel());
        }

        public IActionResult EditCountry(CountriesModel model)
        {
            ViewData["Title"] = "Manage Countries";
            try
            {
                var updateResult = dbContext.UpdateCountry(new Country
                {
                    CountryName = model.CountryName,
                    CurrentCurrencyRate = model.CurrentCurrencyRate,
                    Remarks = model.Remarks,
                    CountryID = model.CountryID
                });
                ViewBag.Countries = GetCountryList();
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessages = ex.Message;
            }

            return View("ManageCountries", new CountriesModel());
        }

        public IActionResult DeleteCountry(CountriesModel model)
        {
            ViewData["Title"] = "Manage Countries";
            try
            {
                var updateResult = dbContext.DeleteCountry(new Country
                {
                    CountryName = model.CountryName,
                    CurrentCurrencyRate = model.CurrentCurrencyRate,
                    Remarks = model.Remarks,
                    CountryID = model.CountryID
                });
                ViewBag.Countries = GetCountryList();
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessages = ex.Message;
            }
            return View("ManageCountries", new CountriesModel());
        }

        public IActionResult ManageExchangeRates()
        {
            ViewBag.ExchangeRates = GetExchangeRateList();
            ViewBag.Countries = GetCountrySelectList();
            return View("ManageExchangeRates", new ExchangeRateModel());
        }
//Add Exchange Rates - JZ
        public IActionResult AddingNewExchangeRate(ExchangeRateModel exchangerate)
        {
            var CurrentRate = GetExchangeRateList();
            try
            {
                if (CurrentRate.Where(t=>t.ToCountryID == exchangerate.ToCountryID && t.FromCountryID == exchangerate.FromCountryID).Count() >0)
                {
                    ViewBag.ErrorMessages = string.Format("The rate for the selected country is already existed !");
                }
                else
                {
                    var addingTransaction = dbContext.InsertNewExchangeRate(new ExchangeRate
                    {
                        ToCountryID = exchangerate.ToCountryID,
                        ToCountryRate = exchangerate.ToCountryRate,
                        FromCountryID = exchangerate.FromCountryID,
                        FromCountryRate = exchangerate.FromCountryRate,
                        Remarks = exchangerate.Remarks
                    });
                }
            }
            catch(Exception ex)
            {
                ViewBag.ErrorMessages = string.Format("There is an error occurred /n {0}", ex.Message);
            }
            ViewBag.ExchangeRates = CurrentRate;
            ViewBag.Countries = GetCountrySelectList();
            return View("ManageExchangeRates", new ExchangeRateModel());
        }
//Edit Exchange Rates - Bernice
        public IActionResult UpdateExchangeRate(ExchangeRateModel exchangerate)
        {
            try
            {
                var addingTransaction = dbContext.UpdateExchangeRate(new ExchangeRate
                {
                    ToCountryID = exchangerate.ToCountryID,
                    ToCountryRate = exchangerate.ToCountryRate,
                    FromCountryID = exchangerate.FromCountryID,
                    FromCountryRate = exchangerate.FromCountryRate,
                    Remarks = exchangerate.Remarks,
                    ExchangeRateID = exchangerate.ExchangeRateID
                });
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessages = string.Format("There is an error occurred /n {0}", ex.Message);
            }
            ViewBag.ExchangeRates = GetExchangeRateList();
            ViewBag.Countries = GetCountrySelectList();
            return View("ManageExchangeRates", new ExchangeRateModel());
        }
//Remove Exchange Rates - Wanli
        public IActionResult DeleteExchangeRate(ExchangeRateModel exchangerate)
        {
            try
            {
                var addingTransaction = dbContext.DeleteExchangeRate(new ExchangeRate
                {
                    ExchangeRateID = exchangerate.ExchangeRateID
                });
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessages = string.Format("There is an error occurred /n {0}", ex.Message);
            }
            ViewBag.ExchangeRates = GetExchangeRateList();
            ViewBag.Countries = GetCountrySelectList();
            return View("ManageExchangeRates", new ExchangeRateModel());
        }

        public SelectList GetCountrySelectList()
        {
            SelectList CountryModel = null;
            var result = GetCountryList();
            if (result != null && result.Count() > 0)
            {
                var selectItems = new List<SelectListItem>();
                foreach (var cty in result)
                {
                    selectItems.Add(new SelectListItem
                    {
                        Value = cty.CountryID.ToString(),
                        Text = cty.CountryName
                    });
                }
                CountryModel = new SelectList(selectItems, "Value", "Text");
            }
            return CountryModel;
        }

        public List<CountriesModel> GetCountryList()
        {
            List<CountriesModel> modelResult = new List<CountriesModel>();
            var result = dbContext.SelectAllCountries().Where(t=>t.IsActive.Value).ToList();
            if (result != null && result.Count() > 0)
            {
                modelResult = new List<CountriesModel>();
                foreach (var cty in result)
                {
                    //Record stock in SGD - JZ
                    decimal CurrencyInSGD = 0;
                    if (cty.CountryName != "Singapore")
                    {
                        //Singapore is taken as the based to calculate other currencies (SGD to YEN, SGD to ruppee, not vice versa, Rupee to SGD, Jap Yen to SGD
                        var OtherCurrency = dbContext.SelectAllExchangeRate().Where(t => t.CountryFrom.ToLower() == "singapore" && t.ToCountryID == cty.CountryID).ToList();
                        if (OtherCurrency != null && OtherCurrency.Count() > 0)
                        {
                            var ToCountryRate = OtherCurrency.First().ToCountryRate.HasValue ? OtherCurrency.First().ToCountryRate.Value : 0;
                            var FromCountryRate = OtherCurrency.First().ToCountryRate.HasValue ? OtherCurrency.First().FromCountryRate.Value : 0;
                            if (ToCountryRate > 0)
                            {
                                CurrencyInSGD = decimal.Round(cty.CurrentCurrencyRate.Value * FromCountryRate / ToCountryRate,2);
                            }
                            else
                            {
                                CurrencyInSGD = decimal.Round(cty.CurrentCurrencyRate.Value);
                            }
                        }
                    }

                    modelResult.Add(new CountriesModel
                    {
                        CountryID = cty.CountryID,
                        IsActiveRemark = cty.IsActive.Value ? "Active" : "Deactive",
                        CountryName = cty.CountryName,
                        CreatedDate = cty.CreatedDate.Value,
                        ModifiedDate = cty.ModifiedDate.Value,
                        CurrentCurrencyRate = cty.CurrentCurrencyRate.HasValue ? cty.CurrentCurrencyRate.Value : 0,
                        CurrencyInSGD = CurrencyInSGD,
                        Remarks = cty.Remarks,
                        IsActive = cty.IsActive.Value
                    });
                }
            }
            return modelResult;
        }

        public List<ExchangeRateModel> GetExchangeRateList()
        {
            List<ExchangeRateModel> modelResult = new List<ExchangeRateModel>();
            var result = dbContext.SelectAllExchangeRate();
            if (result != null && result.Count() > 0)
            {
                foreach (var cty in result)
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
    }
}