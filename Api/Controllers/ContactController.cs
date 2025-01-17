﻿using Api.Models;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Api.Controllers
{
    public class ContactController : ApiController
    {
       

            IOrganizationService service;


            List<Contact> contacts = new List<Contact>();





            public ContactController()
            {
                if (ConnectToCRm())
                {
                    GetAllContacts();


                }
                else
                {
                    GetError();
                }


            }

            private void GetError()
            {

            }

            public Boolean ConnectToCRm()
            {
                try
                {
                    var connectionString = @"AuthType = Office365; 
                            Url =https://orgcacc7db5.crm4.dynamics.com;
                            Username=ayari@isamm.u-manouba.tn;
                            Password=XgtS?%RTNj";
                    CrmServiceClient conn = new CrmServiceClient(connectionString);
                    service = (IOrganizationService)conn.
                     OrganizationWebProxyClient != null ? (IOrganizationService)conn.OrganizationWebProxyClient : (IOrganizationService)conn.OrganizationServiceProxy;




                    return (conn != null && conn.IsReady);



                }
                catch (Exception)
                {

                    throw;
                }
            }
            //public ActionResult error()
            //{
            //    return "Crm didnt COnnect";
            //} 

            //public ienumerable<contact> get()
            //{

            //    return contacts;
            //}


            public Contact Get(Guid id)
            {
                var contact = contacts.FirstOrDefault(c => c.ContactID == id);
                return contact;
            }


            public List<Contact> Get()
            {

                return contacts;
            }

            private EntityCollection GetAllContacts()
            {


                var result = new List<Contact>();
                EntityCollection resp;


                do
                {
                    var query = new QueryExpression("contact") { ColumnSet = new ColumnSet("fullname", "emailaddress1", "telephone1", "new_password") };
                    resp = service.RetrieveMultiple(query);


                    for (int i = 0; i < resp.Entities.Count; i++)
                    {
                        Contact contact = new Contact();
                        if (resp.Entities[i].Contains("fullname"))
                        {
                            contact.full_name = (string)resp.Entities[i].Attributes["fullname"];

                        }
                        if (resp.Entities[i].Contains("new_password"))
                        {
                            contact.password = (string)resp.Entities[i].Attributes["new_password"];
                        }
                        if (resp.Entities[i].Contains("emailaddress1"))
                        {
                            contact.Email = (string)resp.Entities[i].Attributes["emailaddress1"];
                        }
                        if (resp.Entities[i].Contains("telephone1"))
                        {
                            contact.Business_phone = (string)resp.Entities[i].Attributes["telephone1"];
                        }






                        contacts.Add(contact);



                    }

                } while (resp.MoreRecords);
                return resp;
            }

            [System.Web.Http.AcceptVerbs("GET", "POST")]
            [System.Web.Http.HttpGet]
            public IEnumerable<Contact> delete(Guid id)
            {
                service.Delete("contact", id);

                return contacts;

            }
            [System.Web.Http.AcceptVerbs("GET", "POST")]
            [System.Web.Http.HttpGet]
            public IEnumerable<Contact> Update(Guid id)
            {
                Entity newRecord = new Entity("contact");
                //   newRecord.Id = id;
                newRecord = service.Retrieve("contact", id, new ColumnSet("lastname"));
                newRecord["lastname"] = "lastnametest";
                service.Update(newRecord);

                return contacts;

            }







            // GET: Products
            //public ActionResult Index()
            //{
            //    return View();
            //}
        }
    }

