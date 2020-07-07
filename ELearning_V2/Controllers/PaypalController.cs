using ELearning_V2.DTO;
using ELearning_V2.Models;
using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ELearning_V2.Controllers
{
    public class PaypalController : Controller
    {
        // GET: Paypal
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PaymentWithCreditCard()
        {
            //create and item for which you are taking payment
            //if you need to add more items in the list
            //Then you will need to create multiple item objects or use some loop to instantiate object
            Item item = new Item();
            item.name = "Demo Item";
            item.currency = "USD";
            item.price = "5";
            item.quantity = "1";
            item.sku = "sku";

            //Now make a List of Item and add the above item to it
            //you can create as many items as you want and add to this list
            List<Item> itms = new List<Item>();
            itms.Add(item);
            ItemList itemList = new ItemList();
            itemList.items = itms;

            //Address for the payment
            Address billingAddress = new Address();
            billingAddress.city = "NewYork";
            billingAddress.country_code = "US";
            billingAddress.line1 = "23rd street kew gardens";
            billingAddress.postal_code = "43210";
            billingAddress.state = "NY";


            //Now Create an object of credit card and add above details to it
            //Please replace your credit card details over here which you got from paypal
            CreditCard crdtCard = new CreditCard();
            crdtCard.billing_address = billingAddress;
            crdtCard.cvv2 = "874";  //card cvv2 number
            crdtCard.expire_month = 1; //card expire date
            crdtCard.expire_year = 2020; //card expire year
            crdtCard.first_name = "Aman";
            crdtCard.last_name = "Thakur";
            crdtCard.number = "1234567890123456"; //enter your credit card number here
            crdtCard.type = "visa"; //credit card type here paypal allows 4 types

            // Specify details of your payment amount.
            Details details = new Details();
            details.shipping = "1";
            details.subtotal = "5";
            details.tax = "1";

            // Specify your total payment amount and assign the details object
            Amount amnt = new Amount();
            amnt.currency = "USD";
            // Total = shipping tax + subtotal.
            amnt.total = "7";
            amnt.details = details;

            // Now make a transaction object and assign the Amount object
            Transaction tran = new Transaction();
            tran.amount = amnt;
            tran.description = "Description about the payment amount.";
            tran.item_list = itemList;
            tran.invoice_number = "your invoice number which you are generating";

            // Now, we have to make a list of transaction and add the transactions object
            // to this list. You can create one or more object as per your requirements

            List<Transaction> transactions = new List<Transaction>();
            transactions.Add(tran);

            // Now we need to specify the FundingInstrument of the Payer
            // for credit card payments, set the CreditCard which we made above

            FundingInstrument fundInstrument = new FundingInstrument();
            fundInstrument.credit_card = crdtCard;

            // The Payment creation API requires a list of FundingIntrument

            List<FundingInstrument> fundingInstrumentList = new List<FundingInstrument>();
            fundingInstrumentList.Add(fundInstrument);

            // Now create Payer object and assign the fundinginstrument list to the object
            Payer payr = new Payer();
            payr.funding_instruments = fundingInstrumentList;
            payr.payment_method = "credit_card";

            // finally create the payment object and assign the payer object & transaction list to it
            Payment pymnt = new Payment();
            pymnt.intent = "sale";
            pymnt.payer = payr;
            pymnt.transactions = transactions;

            try
            {
                //getting context from the paypal
                //basically we are sending the clientID and clientSecret key in this function
                //to the get the context from the paypal API to make the payment
                //for which we have created the object above.

                //Basically, apiContext object has a accesstoken which is sent by the paypal
                //to authenticate the payment to facilitator account.
                //An access token could be an alphanumeric string

                APIContext apiContext = Configuration.GetAPIContext();

                //Create is a Payment class function which actually sends the payment details
                //to the paypal API for the payment. The function is passed with the ApiContext
                //which we received above.

                Payment createdPayment = pymnt.Create(apiContext);

                //if the createdPayment.state is "approved" it means the payment was successful else not

                if (createdPayment.state.ToLower() != "approved")
                {
                    return View("FailureView");
                }
            }
            catch (PayPal.PayPalException ex)
            {
                Logger.Log("Error: " + ex.Message);
                return View("FailureView");
            }

            return View("SuccessView");
        }
        public ActionResult PaymentWithPaypal()
        {
            //getting the apiContext as earlier
            APIContext apiContext = Configuration.GetAPIContext();

            try
            {
                string payerId = Request.Params["PayerID"];
                var itemSku = Request.Params["ItemSku"];
                var courseID = Request.Params["CourseID"];
                HttpFileCollectionBase files = Request.Files;
                if (string.IsNullOrEmpty(payerId))
                {
                    //this section will be executed first because PayerID doesn't exist
                    //it is returned by the create function call of the payment class

                    // Creating a payment
                    // baseURL is the url on which paypal sendsback the data.
                    // So we have provided URL of this controller only
                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority +
                                "/Paypal/PaymentWithPayPal?";
                    string cancelURI;
                    //guid we are generating for storing the paymentID received in session
                    //after calling the create function and it is used in the payment execution

                    var guid = Convert.ToString((new Random()).Next(100000));

                    //CreatePayment function gives us the payment approval url
                    //on which payer is redirected for paypal account payment
                    var item = new Item();
                    {
                        if (courseID == null)
                        {
                            if (itemSku == "CFPRE")
                            {
                                item.name = "Đăng ký lớp cao cấp";
                                item.price = "200";
                                item.sku = itemSku;
                            }
                            if (itemSku == "CFEXC")
                            {
                                item.name = "Đăng ký lớp đặc quyền";
                                item.price = "500";
                                item.sku = itemSku;
                            }
                            cancelURI = Request.Url.Scheme + "://" + Request.Url.Authority +"/Lop/DayThem";
                            Session.Add("TypePay", 1); //Tạo lớp
                        }
                        else
                        {
                            using (ELearningDB db = new ELearningDB())
                            {
                                var course = db.Courses.Find(Convert.ToInt64(courseID));
                                item.name = "Học phí " + course.Name;
                                item.price = course.Price.ToString();
                                item.sku = "CF" + courseID.ToString();
                                cancelURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/Lop/ViewCourse/"+courseID;
                                Session.Add("TypePay", 2); //Đăng ký lớp
                            }
                        }
                    }
                    var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid, cancelURI, item);

                    //get links returned from paypal in response to Create function call

                    var links = createdPayment.links.GetEnumerator();

                    string paypalRedirectUrl = null;

                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;

                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            //saving the payapalredirect URL to which user will be redirected for payment
                            paypalRedirectUrl = lnk.href;
                        }
                    }

                    // saving the paymentID in the key guid
                    Session.Add(guid, createdPayment.id);
                    return Json(paypalRedirectUrl, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    // This section is executed when we have received all the payments parameters

                    // from the previous call to the function Create

                    // Executing a payment

                    var guid = Request.Params["guid"];

                    var executedPayment = ExecutePayment(apiContext, payerId, Session[guid] as string);

                    if (executedPayment.state.ToLower() != "approved")
                    {
                        return View("FailureView");
                    }
                }
            }
            catch (Exception ex)
            {
                //throw;
                Logger.Log("Error: " + ex.Message);
                return Json("1", JsonRequestBehavior.AllowGet);
            }
            var type = (int)Session["TypePay"];
            if (type == 1)
            {
                CreateCourse();
                TempData["PayMessage"] = "Đăng ký mở lớp thành công";
                return RedirectToAction("DayThem", "Lop");
            }
            CreateCourseDetail();
            TempData["PayMessage"] = "Đăng ký vào lớp thành công";
            return RedirectToAction("ViewCourse", "Lop", new { ID = (long)Session["CourseID"] });
        }

        public ActionResult GetPayout()
        {
            var pay = CreatePayout();
            if (pay.batch_header.batch_status == "PENDING")
            {
                APIContext ApiContext = Configuration.GetAPIContext();
                var payout = new Payout();
                payout.Create(ApiContext);
                return Json(Payout.Get(ApiContext, pay.batch_header.payout_batch_id), JsonRequestBehavior.AllowGet);
            }
            return Json(-1, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ReviceCourse()
        {
            Course c = new Course();
            c.Name = Request["Name"];
            string sa = Request["Capacity"];
            if (sa != "null")
            {
                c.Capacity = Convert.ToInt32(sa);
            }
            else
            {
                c.Capacity = null;
            }
            c.Description = null;
            c.Price = Convert.ToDouble(Request["Price"]);
            c.Description = Request["Description"];
            c.Schedule = Request["Schedule"];
            c.Description = Request["Description"];
            c.Condition = Request["Condition"];
            c.Type = Convert.ToInt32(Request["Type"]);
            c.MaMonHoc = Convert.ToInt32(Request["MaMonHoc"]);
            HttpFileCollectionBase files = Request.Files;
            Session.Add("Item", c);
            Session["UploadedFiles"] = files;
            return Json(1, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ReviceCourseDetail(CourseDetailDTO c)
        {
            var User = (TaiKhoan)Session["User"];
            if (User == null)
            {
                return RedirectToAction("Login", "Login");
            }
            using (ELearningDB db = new ELearningDB())
            {
                var course = db.Courses.Find(c.CourseID);
                if (course != null)
                {
                    if (course.CourseDetails.Count < course.Capacity || course.Capacity == null)
                    {
                        if (course.Status == 1)
                        {
                            Session.Add("CourseID", c.CourseID);
                        }
                        else
                        {
                            return Json(-3, JsonRequestBehavior.AllowGet); //Lớp đang khóa
                        }
                    }
                    else
                    {
                        return Json(-1, JsonRequestBehavior.AllowGet); //Lớp đã đủ người 
                    }
                }
                else
                {
                    return Json(-2, JsonRequestBehavior.AllowGet); //Không tìm thấy lớp
                }
            }
            return Json(1, JsonRequestBehavior.AllowGet);
        }
        private PayPal.Api.Payment payment;

        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            APIContext ApiContext = Configuration.GetAPIContext();
            var paymentExecution = new PaymentExecution() { payer_id = payerId };
            var payment = new Payment() { id = paymentId };
            var payout = new Payout();
            return payment.Execute(ApiContext, paymentExecution);
        }
        private Payment CreatePayment(APIContext apiContext, string redirectUrl, string cancelURI, Item i)
        {

            //similar to credit card create itemlist and add item objects to it
            var itemList = new ItemList() { items = new List<Item>() };

            itemList.items.Add(new Item()
            {
                name = i.name,
                currency = "USD",
                price = i.price,
                quantity = "1",
                sku = i.sku
            });

            var payer = new Payer() { payment_method = "paypal" };

            // Configure Redirect Urls here with RedirectUrls object
            var redirUrls = new RedirectUrls()
            {
                cancel_url = cancelURI,
                return_url = redirectUrl
            };

            // similar as we did for credit card, do here and create details object
            var details = new Details()
            {
                tax = "0",
                shipping = "0",
                subtotal = i.price
            };

            // similar as we did for credit card, do here and create amount object
            var amount = new Amount()
            {
                currency = "USD",
                total = i.price, // Total must be equal to sum of shipping, tax and subtotal.
                details = details
            };

            var transactionList = new List<Transaction>();

            transactionList.Add(new Transaction()
            {
                description = "Transaction description.",
                invoice_number = Convert.ToString((new Random()).Next(100000)),
                amount = amount,
                item_list = itemList
            });

            this.payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };

            // Create a payment using a APIContext
            return this.payment.Create(apiContext);
        }
        private PayoutBatch CreatePayout()
        {
            var payout = new Payout();
            APIContext ApiContext = Configuration.GetAPIContext();

            var sender_batch_header = new PayoutSenderBatchHeader()
            {
                email_subject = "You have a payout!",
                sender_batch_id = "Payouts_2018_" + Convert.ToString((new Random()).Next(100000))
            };
            var amount = new Currency()
            {
                currency = "USD",
                value = "100"
            };
            var payoutitem = new List<PayoutItem>();
            var item = new PayoutItem()
            {
                recipient_type = PayoutRecipientType.PHONE,
                amount = amount,
                note = "Học phí",
                sender_item_id = Convert.ToString((new Random()).Next(100000)),
                receiver = "408-491-2437"
            };
            payoutitem.Add(item);
            payout.items = payoutitem;
            payout.sender_batch_header = sender_batch_header;
            return Payout.Create(ApiContext, payout);
        }

        private void CreateCourseDetail()
        {
            var User = (TaiKhoan)Session["User"];
            using (ELearningDB db = new ELearningDB())
            {
                CourseDetail cd = new CourseDetail()
                {
                    CourseID = (long)Session["CourseID"],
                    UserID = User.ID
                };
                db.CourseDetails.Add(cd);
                db.SaveChanges();
            }
        }
        private void CreateCourse()
        {
            Course course = new Course();
            course = (Course)Session["Item"];
            HttpFileCollectionBase file = (HttpFileCollectionBase)Session["UploadedFiles"];
            using (ELearningDB db = new ELearningDB())
            {
                var id = db.Courses.OrderByDescending(x => x.ID).FirstOrDefault().ID;
                var User = (TaiKhoan)Session["User"];
                course.Status = 1;
                course.UserID = User.ID;
                course.Image = id.ToString() + ".jpg";
                db.Courses.Add(course);
                db.SaveChanges();
                HttpPostedFileBase f = file[0];
                string fname = System.IO.Path.Combine(Server.MapPath("~/Content/img/ClassImage"), id.ToString() + ".jpg");
                f.SaveAs(fname);
            }
        }

    }
}