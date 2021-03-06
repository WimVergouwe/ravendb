﻿// -----------------------------------------------------------------------
//  <copyright file="AdminFSController.cs" company="Hibernating Rhinos LTD">
//      Copyright (c) Hibernating Rhinos LTD. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Raven.Abstractions.Data;
using Raven.Database.Extensions;
using Raven.Database.Server.Controllers.Admin;
using Raven.Json.Linq;

namespace Raven.Database.Counters.Controllers
{
    public class AdminCounterStorageController : BaseAdminController
    {
        [HttpPut]
        [Route("counterstorage/admin/{*id}")]
        public async Task<HttpResponseMessage> Put(string id)
        {
            var docKey = "Raven/Counters/" + id;

            if (IsCounterStorageNameExists(id))
            {
                return GetMessageWithString(string.Format("Counter Storage {0} already exists", id), HttpStatusCode.BadRequest);
            }

            var dbDoc = await ReadJsonObjectAsync<DatabaseDocument>();
            CountersLandlord.Protect(dbDoc);
            var json = RavenJObject.FromObject(dbDoc);
            json.Remove("Id");

            Database.Documents.Put(docKey, null, json, new RavenJObject(), null);

            return GetEmptyMessage(HttpStatusCode.Created);
        }

		[HttpDelete]
        [Route("counterstorage/admin/{*id}")]
		public HttpResponseMessage Delete(string id)
		{
            var docKey = "Raven/Counters/" + id;
            var configuration = CountersLandlord.CreateTenantConfiguration(id);

			if (configuration == null)
				return GetEmptyMessage();


            if (!IsCounterStorageNameExists(id))
            {
                return GetMessageWithString(string.Format("Counter Storage {0} was not found exists", id), HttpStatusCode.BadRequest);
            }

			Database.Documents.Delete(docKey, null, null);
			bool result;

			if (bool.TryParse(InnerRequest.RequestUri.ParseQueryString()["hard-delete"], out result) && result)
			{
				IOExtensions.DeleteDirectory(configuration.DataDirectory);
			}

			return GetEmptyMessage();
		}

        private bool IsCounterStorageNameExists(string id)
        {
            string errorMessage = null;
            var docKey = "Raven/Counters/" + id;
            var database = Database.Documents.Get(docKey, null);
            return database != null;

        }
    }
}