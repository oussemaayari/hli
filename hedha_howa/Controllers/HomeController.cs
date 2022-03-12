using hedha_howa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace hedha_howa.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Login(Contact contact)
        {
            var records = GetEntitiesFromApi();
            foreach (var elemet in records)
            {
                if (elemet.Email == contact.Email && elemet.password == contact.password)
                {
                    Session["id"] = contact.Email;
                    return RedirectToAction("Dashbord",contact);

                }



            }
            return View();
        }

       
        public ActionResult Dashbord()
        {
            return View();
        }
 
        public ActionResult Attribuer()
        {
            return View();
        }
        public ActionResult Note()
        {
            return View();
        }
        public ActionResult Affiche()
        {
            return View();
        }
        public ActionResult Recherche()
        {
            return View();
        }
        public ActionResult AjoutCommentaire()
        {
            return View();
        }
        public ActionResult ConsultationDeLaDemande()
        {
            return View();
        }

        public ActionResult Confirmation()
        {
            return View();
        }
        private List<Contact> GetEntitiesFromApi()
        {
            try
            {
                var resultList = new List<Contact>();
                var Client = new HttpClient();

                var getDataTAsk = Client.GetAsync("https://localhost:44306/api/Entity/get")
                    .ContinueWith(Response =>
                    {
                        var result = Response.Result;
                        if (result.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            var readResult = result.Content.ReadAsAsync<List<Contact>>();
                            readResult.Wait();
                            resultList = readResult.Result;
                        }
                    });
                getDataTAsk.Wait();
                return resultList;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

    }
}